using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;

namespace HelpdeskRemoteControl.AD
{
    /// <summary>
    /// Содержит методы для поиска в Active Directory.
    /// </summary>
    public class ADSearcher
    {
        private DirectorySearcher _search;

        /// <summary>
        /// Получает ADComputer из SearchResult. Возвращает ADComputer или null.
        /// </summary>
        private ADComputer SearchResultToADComputer(SearchResult searchResult)
        {
            if (searchResult == null)
                return null;

            ADComputer computer = new ADComputer();

            if (searchResult.Properties.Contains("name"))
                computer.Name = (String)searchResult.Properties["name"][0];

            if (searchResult.Properties.Contains("description"))
                computer.Description = (String)searchResult.Properties["description"][0];

            if (searchResult.Properties.Contains("ms-Mcs-AdmPwd"))
                computer.LocalAdminPassword = (String)searchResult.Properties["ms-Mcs-AdmPwd"][0];

            return computer;
        }
        
        /// <summary>
        /// Инициализирует новый экземпляр класса для поиска в Active Directory по указанному пути.
        /// </summary>
        public ADSearcher(string adSearchRoot)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(adSearchRoot);

                _search = new DirectorySearcher(entry);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при подключении к Active Directory. " + e.Message);
            }
        }

        /// <summary>
        /// Возвращает список пользователей по имени или логину.
        /// </summary>
        public List<ADUser> GetUsersByNameOrLogin(string nameOrLogin)
        {
            List<ADUser> users = new List<ADUser>();

            string searchString;

            if (String.IsNullOrWhiteSpace(nameOrLogin))
                searchString = @"*";
            else
                searchString = @"*" + nameOrLogin + @"*";

            try
            {
                _search.Filter = "(&(objectCategory=person)(objectClass=user)(|(name=" + searchString + ")(displayname=" + searchString + ")(samaccountname=" + searchString + ")))";

                _search.PropertiesToLoad.Add("samaccountname");
                _search.PropertiesToLoad.Add("displayname");
                _search.PropertiesToLoad.Add("company");
                _search.PropertiesToLoad.Add("department");
                _search.PropertiesToLoad.Add("title");
                _search.PropertiesToLoad.Add("physicaldeliveryofficename");
                _search.PropertiesToLoad.Add("mail");
                _search.PropertiesToLoad.Add("telephonenumber");
                _search.PropertiesToLoad.Add("mobile");
                _search.PropertiesToLoad.Add("ipphone");
                _search.PropertiesToLoad.Add("thumbnailphoto");
                _search.PropertiesToLoad.Add("pwdlastset");

                SearchResultCollection searchResults = _search.FindAll();

                foreach (SearchResult searchResult in searchResults)
                {
                    ADUser user = new ADUser();

                    if (searchResult.Properties.Contains("samaccountname"))
                        user.Login = (String)searchResult.Properties["samaccountname"][0];

                    if (searchResult.Properties.Contains("displayname"))
                        user.DisplayName = (String)searchResult.Properties["displayname"][0];

                    if (searchResult.Properties.Contains("company"))
                        user.Company = (String)searchResult.Properties["company"][0];

                    if (searchResult.Properties.Contains("department"))
                        user.Department = (String)searchResult.Properties["department"][0];

                    if (searchResult.Properties.Contains("title"))
                        user.JobTitle = (String)searchResult.Properties["title"][0];

                    if (searchResult.Properties.Contains("physicaldeliveryofficename"))
                        user.Office = (String)searchResult.Properties["physicaldeliveryofficename"][0];

                    if (searchResult.Properties.Contains("mail"))
                        user.Mail = (String)searchResult.Properties["mail"][0];

                    if (searchResult.Properties.Contains("telephonenumber"))
                        user.WorkPhone = (String)searchResult.Properties["telephonenumber"][0];

                    if (searchResult.Properties.Contains("mobile"))
                        user.MobilePhone = (String)searchResult.Properties["mobile"][0];

                    if (searchResult.Properties.Contains("ipphone"))
                        user.IPPhone = (String)searchResult.Properties["ipphone"][0];

                    if (searchResult.Properties.Contains("thumbnailphoto"))
                        user.Photo = (byte[])searchResult.Properties["thumbnailphoto"][0];

                    if (searchResult.Properties.Contains("pwdlastset"))
                        user.PasswordLastSet = DateTime.FromFileTime((Int64)searchResult.Properties["pwdlastset"][0]);

                    users.Add(user);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске в Active Directory. " + e.Message);
            }
            
            return users.OrderBy(x => x.DisplayName).ToList();
        }

        /// <summary>
        /// Получает компьютер по имени. Возвращает ADComputer или null.
        /// </summary>
        public ADComputer GetComputerByName(string name)
        {
            try
            {
                _search.Filter = "(&(objectClass=computer)(name=" + name + "))";

                _search.PropertiesToLoad.Add("name");
                _search.PropertiesToLoad.Add("description");
                _search.PropertiesToLoad.Add("ms-Mcs-AdmPwd");

                SearchResult searchResult = _search.FindOne();

                return SearchResultToADComputer(searchResult);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске в Active Directory. " + e.Message);
            }
        }

        /// <summary>
        /// Возвращает список компьютеров по имени или описанию.
        /// </summary>
        public List<ADComputer> GetComputersByNameOrDescription(string nameOrDescription)
        {
            List<ADComputer> computers = new List<ADComputer>();

            string searchString;

            if (String.IsNullOrWhiteSpace(nameOrDescription))
                searchString = @"*";
            else
                searchString = @"*" + nameOrDescription + @"*";
                
            try
            {
                _search.Filter = "(&(objectClass=computer)(|(name=" + searchString + ")(description=" + searchString + ")))";

                _search.PropertiesToLoad.Add("name");
                _search.PropertiesToLoad.Add("description");
                _search.PropertiesToLoad.Add("ms-Mcs-AdmPwd");

                SearchResultCollection searchResults = _search.FindAll();

                foreach (SearchResult searchResult in searchResults)
                {
                    ADComputer computer = SearchResultToADComputer(searchResult);

                    if (computer != null)
                        computers.Add(computer);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске в Active Directory. " + e.Message);
            }

            return computers.OrderBy(x => x.Name).ToList();
        }
    }
}
