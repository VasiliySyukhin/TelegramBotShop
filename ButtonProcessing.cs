using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;

namespace Telegram_Bot_Project
{
    public class ButtonProcessing
    {
        public async Task KeyboardButtonHandlingg(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Message message)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                if (callbackQuery.Data.Contains("category"))
                {
                    await HandleCategoryCallbackQuery(botClient, callbackQuery, cancellationToken);
                }
                else if (callbackQuery.Data.Contains("product"))
                {
                    await HandleProductCallbackQuery(botClient, callbackQuery, cancellationToken);
                } 
            }
        }

        private async Task HandleCategoryCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            DatabaseContext db = new DatabaseContext();
            var lastEntity1 = db.Set<Categories1>().OrderByDescending(e => e.Id).FirstOrDefault();
            var lastEntity2 = db.Set<Categories2>().OrderByDescending(e => e.Id).FirstOrDefault();
            var lastId1 = lastEntity1.Id;
            var lastId2 = lastEntity2.Id;
            var lastId = (lastId1 > lastId2) ? lastId1 : lastId2;

            int num = 0;
            var products = new List<object>();

            for (decimal id = 1; id < lastId; id++)
            {
                if (callbackQuery.Data == "category")
                {
                    var catalog = db.Categories1s.Find(id);

                    if (catalog != null)
                    {
                        products.Add(catalog);
                    }
                }
                else if (callbackQuery.Data == "category1")
                {
                    var catalog = db.Categories2s.Find(id);

                    if (catalog != null)
                    {
                        products.Add(catalog);
                    }
                }
                
            }

            var inlineKeyboardButtons = new List<InlineKeyboardButton>();

            foreach (var product in products)
            {
                if (product is Categories1 product1)
                {
                    inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(text: product1.ProductName + " " + "\n" + product1.Price, callbackData: $"product{num++}"));
                }
                else if (product is Categories2 product2)
                {
                    inlineKeyboardButtons.Add(InlineKeyboardButton.WithCallbackData(text: product2.ProductName + " " + "\n" + product2.Price, callbackData: $"product{num++}"));
                }
            }

            InlineKeyboardMarkup inlineKeyboard = new(inlineKeyboardButtons.ToArray());
 
            string message = inlineKeyboardButtons.Any() ? "Товар" : "Товар закончился";

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: message,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

        }


        private async Task HandleProductCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);

            DatabaseContext db = new DatabaseContext();
            Categories1 categories1 = db.Categories1s.FirstOrDefault();
            Categories2 categories2 = db.Categories2s.FirstOrDefault();   

            int id = 0;
            string data = "";
            decimal userId = callbackQuery.Message.Chat.Id;
            var category = categories1.Categories;
            var category1 = categories2.Categories;
            var chat = db.Users.FirstOrDefault(x => x.UserId == userId);
            string selectionId = callbackQuery.Data;

            selectionId = selectionId.Substring(7);
            id = int.Parse(selectionId);
            
            if (category == "Discord")
            {
                var firstEntity = db.Categories1s.Skip(id).FirstOrDefault();
                var firstEntityId = firstEntity.Id;
                Categories1 categories1Data = db.Categories1s.Find(firstEntityId);
                data = categories1Data.LoginPassword;

                ProcessTransaction(botClient, callbackQuery, cancellationToken, db, chat, categories1,categories2, data);
            }else if (category1 == "Csgo")
            {
                var firstEntity = db.Categories2s.Skip(id).FirstOrDefault();
                var firstEntityId = firstEntity.Id;
                Categories2 categories2Data = db.Categories2s.Find(firstEntityId);
                data = categories2Data.LoginPassword;

                ProcessTransaction(botClient, callbackQuery, cancellationToken, db, chat, categories1, categories2, data);
            }
        }

        private async void ProcessTransaction(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken, DatabaseContext db, User chat, Categories1 categories1, Categories2 categories2, string data)
        {
            if (categories1 != null && chat.Money >= categories1.Price && categories1.Categories == "Discord")
            {
                await ProcessSuccessfulTransaction(botClient, callbackQuery, cancellationToken, db, chat, categories1, categories2, data);
            }
            else if(categories2 != null && chat.Money >= categories2.Price && categories2.Categories == "Csgo")
            {
                await ProcessSuccessfulTransaction(botClient, callbackQuery, cancellationToken, db, chat, categories1, categories2, data);
            }
            await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "У вас недостаточно средств",
                    cancellationToken: cancellationToken);
        }

        private async Task ProcessSuccessfulTransaction(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken, DatabaseContext db, User chat, Categories1 categories1, Categories2 categories2, string data)
        {
            if (categories1.Categories == "Discord")
            {
                chat.Money -= categories1.Price;
                db.Users.Update(chat);
                db.SaveChanges();
                db.Categories1s.Remove(categories1);
                db.SaveChanges();
            }else if(categories2.Categories == "Csgo")
            {
                chat.Money -= categories2.Price;
                db.Users.Update(chat);
                db.SaveChanges();
                db.Categories2s.Remove(categories2);
                db.SaveChanges();
            }
                Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: data,
                cancellationToken: cancellationToken);
        }
    }
}
