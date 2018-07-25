namespace HelpdeskRemoteControl.AD
{
    /// <summary>
    /// Содержит атрибуты компьютера из Active Directory.
    /// </summary>
    public class ADComputer
    {
        // Атрибут Name
        public string Name { get; set; }

        // Атрибут Description
        public string Description { get; set; }

        // Атрибут ms-Mcs-AdmPwd (содержит пароль локального администратора если используется LAPS)
        public string LocalAdminPassword { get; set; }
    }
}
