using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Класс для поиска в Active Directory.
    /// </summary>
    public class ADSearcher
    {
        private DirectorySearcher _search;
                
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ADSearcher()
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(SettingsReader.ADSearchRoot);

                _search = new DirectorySearcher(entry);
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при подключении к Active Directory. " + e.Message);
            }
        }

        /// <summary>
        /// Получение списка пользователей с заданным именем или логином.
        /// </summary>
        /// <param name="searchString">Имя или логин пользователя.</param>
        /// <returns>Список пользователей c сортировкой по DisplayName.</returns>
        public List<User> GetUsersByNameOrLogin(string searchString)
        {
            List<User> result = new List<User>();

            string searchStringEx;

            if (String.IsNullOrWhiteSpace(searchString))
                searchStringEx = @"*";
            else
                searchStringEx = @"*" + searchString + @"*";

            try
            {
                _search.Filter = "(&(objectCategory=person)(objectClass=user)(|(name=" + searchStringEx + ")(displayname=" + searchStringEx + ")(samaccountname=" + searchStringEx + ")))";

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

                SearchResultCollection searchResults;

                searchResults = _search.FindAll();

                foreach (SearchResult searchResult in searchResults)
                {
                    var user = new User();

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
                    {
                        DateTime pwdLastSet = DateTime.FromFileTime((Int64)searchResult.Properties["pwdlastset"][0]);

                        int daysAgo = DateTime.Now.Subtract(pwdLastSet).Days;

                        user.PasswordLastSet = pwdLastSet.ToString("dd.MM.yyyy HH:mm:ss") + " (" + daysAgo.ToString() + " дней назад)";
                    }
                        

                    result.Add(user);
                }
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при поиске в Active Directory. " + e.Message);
            }
            
            return result.OrderBy(x => x.DisplayName).ToList();
        }

        /// <summary>
        /// Возвращает описание заданного компьютера.
        /// </summary>
        /// <param name="name">Имя компьютера.</param>
        /// <returns>Описание компьютера.</returns>
        public string GetComputerDescriptionByName(string name)
        {
            string result = "";

            try
            {
                _search.Filter = "(&(objectClass=computer)(name=" + name + "))";

                _search.PropertiesToLoad.Add("description");

                SearchResult searchResult = _search.FindOne();

                if (searchResult != null)
                    if (searchResult.Properties.Contains("description"))
                        result = (String)searchResult.Properties["description"][0];

                return result;
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при поиске в Active Directory. " + e.Message);
            }

            return result;
        }
    }
}
