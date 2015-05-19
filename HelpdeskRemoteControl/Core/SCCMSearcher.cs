using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Added the below Configuration Manager DLL references to support basic SMS Provider operations:
//    C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\Microsoft.ConfigurationManagement.ManagementProvider.dll
//    C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\AdminUI.WqlQueryEngine.dll
// Added the below Configuration Manager namespaces to support basic SMS Provider operations:
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine; 

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Класс для поиска в Configuration Manager.
    /// </summary>
    public class SCCMSearcher
    {
        private WqlConnectionManager _connection;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SCCMSearcher()
        {
            try
            {
                SmsNamedValuesDictionary namedValues = new SmsNamedValuesDictionary();

                _connection = new WqlConnectionManager(namedValues);

                _connection.Connect(SettingsReader.SCCMServer);
            }
            catch (SmsException e)
            {
                Helper.ErrorMessage("Ошибка при подключении к Configuration Manager. " + e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Helper.ErrorMessage("Ошибка при аутентификации в Configuration Manager. " + e.Message);
            }
        }

        /// <summary>
        /// Получение списка компьютеров, на которых последним логинился указанный пользователь.
        /// </summary>
        /// <param name="searchString">Логин пользователя.</param>
        /// <returns>Список компьютеров с сортировкой по имени компьютера.</returns>
        public List<Computer> GetComputersByUserLogin(string searchString)
        {
            List<Computer> result = new List<Computer>();

            try
            {
                string query = @"SELECT * FROM SMS_R_System WHERE LOWER(SMS_R_System.LastLogonUserName) like '%" + searchString.ToLower() + @"%'";

                IResultObject queryResults = _connection.QueryProcessor.ExecuteQuery(query);

                foreach (IResultObject queryResult in queryResults)
                {
                    var computer = new Computer();

                    computer.Name = queryResult["Name"].StringValue;
                    foreach (string address in queryResult["IPAddresses"].StringArrayValue)
                    {
                        computer.IPAddresses += address + " ";
                    }
                    computer.LastUserDomain = queryResult["LastLogonUserDomain"].StringValue;
                    computer.LastUserName = queryResult["LastLogonUserName"].StringValue;
                    computer.LastUserLogonTime = queryResult["LastLogonTimeStamp"].DateTimeValue;
                    computer.SCCMClientVersion = queryResult["ClientVersion"].StringValue;
                    foreach (string site in queryResult["SMSAssignedSites"].StringArrayValue)
                    {
                        computer.SCCMAssignedSites += site + " ";
                    }

                    // Добавляем описание компьютера из Active Directory.
                    ADSearcher adSearcher = new ADSearcher();
                    computer.Description = adSearcher.GetComputerDescriptionByName(computer.Name);

                    result.Add(computer);
                }
            }
            catch (SmsException e)
            {
                Helper.ErrorMessage("Ошибка при поиске в Configuration Manager. " + e.Message);
            }

            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
