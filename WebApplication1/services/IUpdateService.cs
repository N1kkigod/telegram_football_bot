using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApplication1.services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}
