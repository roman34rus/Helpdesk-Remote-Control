using System;
using HelpdeskRemoteControl.AD;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Содержит данные о пользователе.
    /// </summary>
    public class User
    {
        public string Login { get; set; }

        public string DisplayName { get; set; }

        public string Company { get; set; }

        public string Department { get; set; }

        public string JobTitle { get; set; }

        public string Office { get; set; }

        public string Mail { get; set; }

        public string WorkPhone { get; set; }

        public string MobilePhone { get; set; }

        public string IPPhone { get; set; }

        public byte[] Photo { get; set; }

        public string PasswordLastSet { get; set; }

        public User(ADUser adUser)
        {
            if (adUser != null)
            {
                Login = adUser.Login;
                DisplayName = adUser.DisplayName;
                Company = adUser.Company;
                Department = adUser.Department;
                JobTitle = adUser.JobTitle;
                Office = adUser.Office;
                Mail = adUser.Mail;
                WorkPhone = adUser.WorkPhone;
                MobilePhone = adUser.MobilePhone;
                IPPhone = adUser.IPPhone;
                Photo = adUser.Photo;

                int daysFromPasswordSet = DateTime.Now.Subtract(adUser.PasswordLastSet).Days;

                PasswordLastSet = adUser.PasswordLastSet.ToString("dd.MM.yyyy HH:mm:ss") + " (" + daysFromPasswordSet.ToString() + " дней назад)";
            }
        }

        // Переопределение метода ToString() нужно для корректного отображения в ListBox
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
