using System;

namespace HelpdeskRemoteControl.SCCM
{
    /// <summary>
    /// Содержит данные компьютера из Configuration Manager.
    /// </summary>
    public class SCCMComputer
    {
        // SMS_R_System.Name
        public string Name { get; set; }

        // SMS_R_System.IPAddresses[]
        public string IPAddresses { get; set; }

        // SMS_R_System.LastLogonUserDomain
        public string LastUserDomain { get; set; }

        // SMS_R_System.LastLogonUserName
        public string LastUserName { get; set; }

        // SMS_R_System.LastLogonTimeStamp
        public DateTime LastUserLogonTime { get; set; }

        // SMS_R_System.ClientVersion
        public string SCCMClientVersion { get; set; }

        // SMS_R_System.SMSAssignedSites
        public string SCCMAssignedSites { get; set; }
    }
}
