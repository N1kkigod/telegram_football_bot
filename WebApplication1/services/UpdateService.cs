using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WebApplication1.database;
using WebApplication1.Models;

namespace WebApplication1.services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;
        private ApplicationContext Context { get; set; }
        public UpdateService(IBotService botService, ILogger<UpdateService> logger, ApplicationContext context)
        {
            _botService = botService;
            _logger = logger;
            Context = context;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString());
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });
            if (message.Text == "/getMatches") // getMatches test
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, Context.Set<Match>().First().ToString(), replyMarkup: inlineKeyboard);
            }
            if(message.Text == "Test")
            {
                Context.Set<Models.User>().Where(user => user.UserTelegramID == message.Chat.Id).First().Score += 10;
                Context.SaveChanges();
            }
            if (message.Text == "Tournaments") // TournamentsButton
            {
                foreach (Tournament tournament in Context.Set<Tournament>())
                {
                    InlineKeyboardMarkup tournamentKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Matches", "inlineMatchesFromTournament" + tournament.TournamentID),
                            InlineKeyboardButton.WithCallbackData("Information", "inlineInformationFromTournament" + tournament.TournamentID),
                        }
                    });
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, tournament.TournamentName, replyMarkup: tournamentKeyboard);
                }
            }

            if (message.Text == "Profile") // ProfileButton
            {
                Models.User thisUser = new();
                try // Check if user exists - show information
                {
                    thisUser = Context.Set<Models.User>().Where(user => user.UserTelegramID == message.Chat.Id).First();
                }
                catch // If user isn't exists - create new user
                {
                    try
                    {
                        thisUser.UserID = Context.Set<Models.User>().OrderBy(newUser => newUser.UserID).Last().UserID + 1;
                    }
                    catch
                    {
                        thisUser.UserID = 1;
                    }
                    thisUser.Username = message.Chat.Username;
                    thisUser.UserTelegramID = Convert.ToInt32(message.Chat.Id);
                    thisUser.Score = 10; // 10 Default Points
                    Context.Set<Models.User>().Add(thisUser);
                    Context.SaveChanges();
                }
                finally
                {
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Username: " + thisUser.Username + "\nScore: " + thisUser.Score);
                }
            }

            var action = message.Text.Split(' ').First() switch
            {
                "/inline" => SendInlineKeyboard(_botService.Client, message),
                "/keyboard" => SendReplyKeyboard(_botService.Client, message),
                "/remove" => RemoveKeyboard(_botService.Client, message),
                "/photo" => SendFile(_botService.Client, message),
                "/request" => RequestContactAndLocation(_botService.Client, message),
                _ => Usage(_botService.Client, message)
            };
            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");


            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: inlineKeyboard);
            }

            static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "Tournaments", "Matches" },
                        new KeyboardButton[] { "Profile", "Test" },
                    },
                    resizeKeyboard: true
                );

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: replyKeyboardMarkup);
            }

            static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Removing keyboard",
                                                      replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                return await bot.SendPhotoAsync(chatId: message.Chat.Id,
                                                photo: new InputOnlineFile(fileStream, fileName),
                                                caption: "Nice Picture");
            }

            static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup RequestReplyKeyboard = new(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Who or Where are you?",
                                                      replyMarkup: RequestReplyKeyboard);
            }

            static async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: usage,
                                                      replyMarkup: new ReplyKeyboardRemove());
            }
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.Contains("inlineBetFromMatch"))
            {
                int userId = Context.Set<Models.User>().Where(user => user.UserTelegramID == callbackQuery.Message.Chat.Id).First().UserID;
                if (Context.Set<Models.User>().Find(userId).Score < 5)
                {
                    await _botService.Client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Not enought points to bet");
                }
                else
                {

                    Bet newBet = new();
                    try
                    {
                        newBet.BetID = Context.Set<Bet>().OrderBy(bet => bet.BetID).Last().BetID + 1;
                    }
                    catch
                    {
                        newBet.BetID = 1;
                    }

                    int matchId = Convert.ToInt32(callbackQuery.Data.Substring(18, 1));
                    newBet.MatchID = matchId;
                    newBet.BetValue = 5;
                    if (callbackQuery.Data.Substring(19) == "Command1")
                        newBet.Command = Context.Set<Match>().Find(matchId).Command1;
                    else
                        newBet.Command = Context.Set<Match>().Find(matchId).Command2;
                    newBet.UserID = userId;
                    Context.Set<Models.User>().Find(userId).Score -= 5;
                    Context.Set<Bet>().Add(newBet);
                    Context.SaveChanges();
                    await _botService.Client.AnswerCallbackQueryAsync(callbackQuery.Id,
                                                      "Bet accepted!", showAlert: true);
                }
            }
            else if(callbackQuery.Data.Contains("inlineMatchesFromTournament"))
            {
                
                string tournamentName = Context.Set<Tournament>().Find(Convert.ToInt32(callbackQuery.Data.Substring(27))).TournamentName;
                foreach (Match match in Context.Set<Match>().Where(match => match.TournamentName == tournamentName))
                {
                    InlineKeyboardMarkup matchKeyboard = new(new[]
                    {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Bet Command1", "inlineBetFromMatch" + match.MatchID + "Command1"),
                            InlineKeyboardButton.WithCallbackData("Bet Command2", "inlineBetFromMatch" + match.MatchID + "Command2"),
                        }
                    });
                    await _botService.Client.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                                                      text: "Match: \n" + match.ToString(),
                                                      replyMarkup: matchKeyboard);
                    //await _botService.Client.AnswerCallbackQueryAsync(callbackQuery.Id, match.MatchID.ToString(), );
                }
            }
            else
            {
                await _botService.Client.AnswerCallbackQueryAsync(callbackQuery.Id,
                                                      $"Received {callbackQuery.Data}");

                await _botService.Client.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                                                      $"Received {callbackQuery.Data}");
            }
        }

    }
}
