using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;



namespace Telegram_Bot_Project
{
    public class CategoryProduct
    {
        public async Task Category(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            using var db = new DatabaseContext();
            var category = db.Categories1s.FirstOrDefault();
            var category1 = db.Categories2s.FirstOrDefault();

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData(category.Categories, "category") },
                new [] { InlineKeyboardButton.WithCallbackData(category1.Categories, "category1") }
            });

            var sentMessage = await botClient.SendTextMessageAsync(
                       chatId: message.Chat.Id,
                       text: "Каталог товаров",
                       replyMarkup: inlineKeyboard,
                       cancellationToken: cancellationToken);
        
             
        }
    }
}
