using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        // Атрибут AD: User.sAMAccountName.
        public string Login { get; set; }

        // Атрибут AD: User.DisplayName.
        public string DisplayName { get; set; }

        // Атрибут AD: User.Company.
        public string Company { get; set; }

        // Атрибут AD: User.Department.
        public string Department { get; set; }

        // Атрибут AD: User.Title.
        public string JobTitle { get; set; }

        // Атрибут AD: User.PhysicalDeliveryOfficeName.
        public string Office { get; set; }

        // Атрибут AD: User.Mail.
        public string Mail { get; set; }

        // Атрибут AD: User.TelephoneNumber.
        public string WorkPhone { get; set; }

        // Атрибут AD: User.Mobile.
        public string MobilePhone { get; set; }

        // Атрибут AD: User.IPPhone.
        public string IPPhone { get; set; }

        // Атрибут AD: User.ThumbnailPhoto
        public byte[] Photo { get; set; }

        // Атрибут AD: User.pwdLastSet
        public string PasswordLastSet { get; set; }

        // Переопределение метода ToString() нужно для корректного отображения в ListBox.
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
