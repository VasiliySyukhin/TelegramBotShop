using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot_Project
{
    public class UserRegistration
    {
        public long UserRegistrationInDatabase(long ChatId)
        {
            using (DatabaseContext dataBase = new DatabaseContext())
            {
                var userId = dataBase.Users.Where(c => c.UserId == ChatId).FirstOrDefault();

                if (null == userId)
                {
                    User user = new User();
                    user.UserId = ChatId;
                    user.Money = 0;
                    
                    dataBase.Users.Add(user);

                    dataBase.SaveChanges();

                    return (long)user.Id;
                }
                return (long)userId.Id;
            }
        }
    }
}
