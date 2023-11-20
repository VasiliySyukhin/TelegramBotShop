using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Telegram_Bot_Project.Classes
{
    public class AccountReplenishment
    {
        public async Task ButtonReplenishment(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData("50", "50") },
                new [] { InlineKeyboardButton.WithCallbackData("100", "100") }
            });

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Выберите сумму пополнения",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        public async Task Qiwi(ITelegramBotClient botClient, Message message, Update update, CancellationToken cancellationToken)
        {
            int money = 0;
            var callbackQuery = update.CallbackQuery;

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (callbackQuery.Data == "50")
                {
                    money = 50;
                }
                if (callbackQuery.Data == "100")
                {
                    money = 100;
                }
            }

            {
                CreateForm(money);

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Оплатить",
                    url: CreateForm(money))
                });

                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    text: "Ссылка для оплаты",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }
        }
        private static string UserBillId { get; set; }

        public static string CreateForm(decimal money)
        {
            BillPaymentsClient clientQiwi = BillPaymentsClientFactory.Create(secretKey: "eyJ2ZXJzaW9uIjoiUDJQIiwiZGF0YSI6eyJwYXlpbl9tZXJjaGFudF9zaXRlX3VpZCI6Im53MW9oai0wMCIsInVzZXJfaWQiOiI3OTAyNzAzMTQzMCIsInNlY3JldCI6IjlmZDA4NGYzZDJmNWNmYWZiNTNkZDkxNmMxYWQ3NmRiM2Y3OTY5NWU1MjBhNzRmMTkxOTBhYjAyNGE0MjhiMjkifX0=");

            var newPayment = clientQiwi.CreateBill(
                info: new CreateBillInfo
                {
                    BillId = Guid.NewGuid().ToString(),
                    Amount = new MoneyAmount
                    {
                        ValueDecimal = money,
                        CurrencyEnum = CurrencyEnum.Rub
                    },
                    Comment = "comment",
                    ExpirationDateTime = DateTime.Now.AddDays(5),
                    Customer = new Customer
                    {
                        Email = "example@mail.org",
                        Account = Guid.NewGuid().ToString(),
                        Phone = ""
                    }
                },
                customFields: new CustomFields
                {
                    ThemeCode = "кодСтиля"
                }
            );
            UserBillId = newPayment.BillId.ToString();
            string url = newPayment.PayUrl.ToString();
            return url;
        }
        public async Task<bool> UpdatePayments(string[] idAcc)
        {
            BillPaymentsClient clientQiwi = BillPaymentsClientFactory.Create(secretKey: "eyJ2ZXJzaW9uIjoiUDJQIiwiZGF0YSI6eyJwYXlpbl9tZXJjaGFudF9zaXRlX3VpZCI6Im53MW9oai0wMCIsInVzZXJfaWQiOiI3OTAyNzAzMTQzMCIsInNlY3JldCI6IjlmZDA4NGYzZDJmNWNmYWZiNTNkZDkxNmMxYWQ3NmRiM2Y3OTY5NWU1MjBhNzRmMTkxOTBhYjAyNGE0MjhiMjkifX0=");
            var localInfoUserBill = UserBillId;
            int i = 0;
            string Status = "WAIT";
            while (Status != "PAID")
            {
                Status = clientQiwi.GetBillInfo(localInfoUserBill).Status.ValueString;
                i++;
                if (i == 600)
                {

                    clientQiwi.CancelBill(billId: localInfoUserBill);
                    return false;
                }
                var taskdealy = Task.Delay(1000);
                await taskdealy;
            }

            return true;
        }
    }
}
