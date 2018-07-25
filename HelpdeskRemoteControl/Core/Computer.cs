using System;
using HelpdeskRemoteControl.AD;
using HelpdeskRemoteControl.SCCM;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Содержит данные о компьютере.
    /// </summary>
    public class Computer
    {
        public string Name { get; set; }

        public string IPAddresses { get; set; }

        public string LastUserDomain { get; set; }

        public string LastUserName { get; set; }

        public DateTime LastUserLogonTime { get; set; }

        public string SCCMClientVersion { get; set; }

        public string SCCMAssignedSites { get; set; }

        public string Description { get; set; }

        public string LocalAdminPassword { get; set; }

        public Computer(SCCMComputer sccmComputer, ADComputer adComputer)
        {
            if (sccmComputer != null)
            {
                Name = sccmComputer.Name;
                IPAddresses = sccmComputer.IPAddresses;
                LastUserDomain = sccmComputer.LastUserDomain;
                LastUserName = sccmComputer.LastUserName;
                LastUserLogonTime = sccmComputer.LastUserLogonTime;
                SCCMClientVersion = sccmComputer.SCCMClientVersion;
                SCCMAssignedSites = sccmComputer.SCCMAssignedSites;
            }
            if (adComputer != null)
            {
                Name = adComputer.Name;
                Description = adComputer.Description;
                LocalAdminPassword = adComputer.LocalAdminPassword;
            }
        }

        // Переопределение метода ToString() нужно для корректного отображения в ListBox.
        public override string ToString()
        {
            return Name;
        }
    }
}
