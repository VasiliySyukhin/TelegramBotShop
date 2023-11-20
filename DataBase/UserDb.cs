using System;
using System.Collections.Generic;

namespace Telegram_Bot_Project.DataBase;

public partial class UserDb
{
    public decimal Id { get; set; }

    public long UserId { get; set; }

    public decimal Money { get; set; }
}
