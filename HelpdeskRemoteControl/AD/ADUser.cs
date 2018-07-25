using System;

namespace HelpdeskRemoteControl.AD
{
    /// <summary>
    /// Содержит атрибуты пользователя из Active Directory.
    /// </summary>
    public class ADUser
    {
        // Атрибут sAMAccountName
        public string Login { get; set; }

        // Атрибут DisplayName
        public string DisplayName { get; set; }

        // Атрибут Company
        public string Company { get; set; }

        // Атрибут Department
        public string Department { get; set; }

        // Атрибут Title
        public string JobTitle { get; set; }

        // Атрибут PhysicalDeliveryOfficeName
        public string Office { get; set; }

        // Атрибут Mail
        public string Mail { get; set; }

        // Атрибут TelephoneNumber
        public string WorkPhone { get; set; }

        // Атрибут Mobile
        public string MobilePhone { get; set; }

        // Атрибут IPPhone
        public string IPPhone { get; set; }

        // Атрибут ThumbnailPhoto
        public byte[] Photo { get; set; }

        // Атрибут pwdLastSet
        public DateTime PasswordLastSet { get; set; }
    }
}
