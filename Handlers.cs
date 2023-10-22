using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;


namespace Telegram_Bot_Project
{
    public class Handlers
    {

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
         {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await BotOnMessageReceived(botClient, update.Message, update, cancellationToken);
                    break;

                case UpdateType.EditedMessage:
                    await BotOnMessageReceived(botClient, update.EditedMessage, update, cancellationToken);
                    break;

                case UpdateType.CallbackQuery:
                    await BotOnCallbackQueryReceived(botClient, update.CallbackQuery, update, cancellationToken, update.Message);
                    break;

                case UpdateType.ChosenInlineResult:
                    await BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult);
                    break;

                default:
                    await UnknownUpdateHandlerAsync(botClient, update);
                    break;
            }
        }

        private static string[] IsCommand(string Text)
        {
            string[] Words = Text.Split(' ');

            if (Words == null || Words.Length < 2 || !Words[0].StartsWith("/"))
                return null;

            return Words;
        }

        static async Task<Message> BotMenu(ITelegramBotClient botClient, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
        new KeyboardButton[] { "Купить", "Пополнить" },
        new KeyboardButton[] { "Профиль", "Поддержка" },
            })
            {
                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Воспользуйтесь меню",
                replyMarkup: replyKeyboardMarkup);
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            UserRegistration userRegistration = new UserRegistration();

            if (message.Text.ToLower() == "/start")
            {
                long ChatInnerId = userRegistration.UserRegistrationInDatabase(message.Chat.Id);
                await BotMenu(botClient, message);
                return;
            }

            string[] Words = IsCommand(message.Text.ToLower());

            CategoryProduct categoryProduct = new CategoryProduct();
            UserProfile userProfile = new UserProfile();
            AccountReplenishment accountReplenishment = new AccountReplenishment();
            UserSupport userSupport = new UserSupport();
            

            switch (message.Text.ToLower())
            {
                case "купить":
                    await categoryProduct.Category(botClient, message, update, cancellationToken);
                    break;
                case "профиль":
                    await userProfile.Profile(botClient, message, update, cancellationToken);
                    break;
                case "пополнить":
                    await accountReplenishment.ButtonReplenishment(botClient, message, update, cancellationToken);
                    break;
                case "поддержка":
                    await userSupport.Support(botClient, message, update, cancellationToken);
                    break;
            }
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery, Update update, CancellationToken cancellationToken, Message message)
        {
            ButtonProcessing buttonProcessing = new ButtonProcessing();
            await buttonProcessing.KeyboardButtonHandlingg(botClient, update, cancellationToken, message);
            if(callbackQuery.Data == "50" | callbackQuery.Data == "100")
            { 
            AccountReplenishment accountReplenishment = new AccountReplenishment();
            await accountReplenishment.Qiwi(botClient, message, update, cancellationToken);
            }

        }

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

    }
}
