using System;
using System.Collections.Generic;
using System.Linq;

// Added the below Configuration Manager DLL references to support basic SMS Provider operations:
//    C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\Microsoft.ConfigurationManagement.ManagementProvider.dll
//    C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\AdminUI.WqlQueryEngine.dll
// Added the below Configuration Manager namespaces to support basic SMS Provider operations:
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace HelpdeskRemoteControl.SCCM
{
    /// <summary>
    /// Содержит методы для поиска в Configuration Manager.
    /// </summary>
    public class SCCMSearcher
    {
        private WqlConnectionManager _connection;

        private SCCMComputer QueryResultToSCCMComputer(IResultObject queryResult)
        {
            if (queryResult == null)
                return null;

            SCCMComputer computer = new SCCMComputer();

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

            return computer;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса для поиска на указанном сервере Configuration Manager.
        /// </summary>
        public SCCMSearcher(string sccmServer)
        {
            try
            {
                SmsNamedValuesDictionary namedValues = new SmsNamedValuesDictionary();

                _connection = new WqlConnectionManager(namedValues);

                _connection.Connect(sccmServer);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при подключении к Configuration Manager. " + e.Message);
            }
        }

        /// <summary>
        /// Возвращает список компьютеров по логину последнего пользователя.
        /// </summary>
        public List<SCCMComputer> GetComputersByUserLogin(string userLogin)
        {
            List<SCCMComputer> result = new List<SCCMComputer>();

            try
            {
                string query = @"SELECT * FROM SMS_R_System WHERE LOWER(SMS_R_System.LastLogonUserName) like '%" + userLogin.ToLower() + @"%'";

                IResultObject queryResults = _connection.QueryProcessor.ExecuteQuery(query);

                foreach (IResultObject queryResult in queryResults)
                {
                    SCCMComputer computer = QueryResultToSCCMComputer(queryResult);

                    if (computer != null)
                        result.Add(computer);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске в Configuration Manager. " + e.Message);
            }

            return result.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Получает компьютер по имени. Возвращает SCCMComputer или null.
        /// </summary>
        public SCCMComputer GetComputerByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            string query = @"SELECT * FROM SMS_R_System WHERE LOWER(SMS_R_System.Name) = '" + name.ToLower() + @"'";

            IResultObject queryResults;

            try
            {
                queryResults = _connection.QueryProcessor.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске в Configuration Manager. " + e.Message);
            }

            SCCMComputer computer = null;

            foreach (IResultObject queryResult in queryResults)
            {
                computer = QueryResultToSCCMComputer(queryResult);

                break;
            }

            return computer;
        }
    }
}
