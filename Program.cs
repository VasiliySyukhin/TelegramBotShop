using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Polling;
using static Telegram_Bot_Project.Handlers.Handlers;

namespace Telegram_Bot_Project
{
    class Program
    {
        private static string token { get; set; } = "Введите токен бота";
        private static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(token);

            var cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
            Bot.StartReceiving(HandleUpdateAsync,
                               HandleErrorAsync,
                               receiverOptions,
                               cts.Token);

            Console.WriteLine($"Бот запущен и ждет сообщения...");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}