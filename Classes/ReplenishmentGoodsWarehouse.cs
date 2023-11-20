using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram_Bot_Project.Categories;
using Telegram_Bot_Project.DataBase;

namespace Telegram_Bot_Project.Classes
{
    public class ReplenishmentGoodsWarehouse
    {
        public void Products()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Categories2 dataBase = new Categories2();
                dataBase.Categories = "Csgo";
                dataBase.ProductName = "Csgo";
                dataBase.Price = 150;
                dataBase.LoginPassword = "German:qwer1213";
                db.Categories2s.Add(dataBase);
                db.SaveChanges();
            }
            return;
        }
    }
}
