using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Атрибуты компьютера из Active Directory
    /// </summary>
    public class ADComputer
    {
        // Атрибут AD: Computer.Description
        public string Description { get; set; }

        // Атрибут AD: Computer.ms-Mcs-AdmPwd (содержит пароль лок. админа если используется LAPS)
        public string LocalAdminPassword { get; set; }
    }
}
