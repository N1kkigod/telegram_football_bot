using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_bet_bot
{
	class Program
	{
		private static string token = "1826830737:AAFCipRRtisTvi87Leo_43sbC9WA3JteYOE";
		private static string webhoookurl = "https://716197882eaa.ngrok.io" + "/api/update";
		static void Main(string[] args)
		{
			// token, который вернул BotFather
			var client = new TelegramBotClient(token);
			client.SetWebhookAsync(webhoookurl).Wait();
			Console.ReadLine();
			client.SetWebhookAsync("").Wait();
		}
	}
}
