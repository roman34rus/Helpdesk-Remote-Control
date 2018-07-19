using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Компьютер
    /// </summary>
    public class Computer
    {
        // Атрибут SCCM: SMS_R_System.Name
        public string Name { get; set; }

        // Атрибут SCCM: SMS_R_System.IPAddresses[]
        public string IPAddresses { get; set; }

        // Атрибут SCCM: SMS_R_System.LastLogonUserDomain
        public string LastUserDomain { get; set; }

        // Атрибут SCCM: SMS_R_System.LastLogonUserName
        public string LastUserName { get; set; }

        // Атрибут SCCM: SMS_R_System.LastLogonTimeStamp
        public DateTime LastUserLogonTime { get; set; }

        // Атрибут SCCM: SMS_R_System.ClientVersion
        public string SCCMClientVersion { get; set; }

        // Атрибут SCCM: SMS_R_System.SMSAssignedSites
        public string SCCMAssignedSites { get; set; }

        // Атрибут AD: Computer.Description
        public string Description { get; set; }

        // Атрибут AD: Computer.ms-Mcs-AdmPwd (содержит пароль лок. админа если используется LAPS)
        public string LocalAdminPassword { get; set; }

        // Переопределение метода ToString() нужно для корректного отображения в ListBox
        public override string ToString()
        {
            return Name;
        }
    }
}
