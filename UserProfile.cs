﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Telegram_Bot_Project
{
    public class UserProfile
    {
        public async Task Profile(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            DatabaseContext dataBase = new DatabaseContext();
            var id = 0;
            var UserId = message.Chat.Id;
            User userId = dataBase.Users.FirstOrDefault(x => x.UserId == UserId);
            var Money = userId.Money;

            Message sentMessage = await botClient.SendTextMessageAsync(
                       chatId: message.Chat.Id,
                       text:"Профиль:\n" + "Ваш Id: " + UserId + "\nВаш баланс " + Money + "руб", 
                       cancellationToken: cancellationToken);
        }
    }
}
