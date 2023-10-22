using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Bot_Project
{
    public class UserSupport
    {
        public async Task Support(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
                      text: "В случае проблем писать ему:\n" + "Введите свой телеграм Id",
                      cancellationToken: cancellationToken);
        }
    }
}
