using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Статический класс для чтения настроек из файла Settings.xml.
    /// </summary>
    public static class SettingsReader
    {
        /// <summary>
        /// Контейнер для поиска в Active Directory.
        /// </summary>
        public static string ADSearchRoot { get; private set; }

        /// <summary>
        /// Имя сервера Configuration Manager. 
        /// </summary>
        public static string SCCMServer { get; private set; }

        /// <summary>
        /// Чтение настроек из файла.
        /// </summary>
        public static void ReadSettings()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"Settings.xml");

                ADSearchRoot = xdoc.SelectSingleNode("/Settings/ActiveDirectory/SearchRoot").InnerText;
                SCCMServer = xdoc.SelectSingleNode("/Settings/ConfigurationManager/Server").InnerText;
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при чтении файла Settings.xml. " + e.Message);
            }
        }
    }
}
