using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskRemoteControl.AD;
using HelpdeskRemoteControl.SCCM;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Содержит методы для поиска пользователей и компьютеров.
    /// </summary>
    public class Searcher
    {
        private ADSearcher _adSearcher;
        private SCCMSearcher _sccmSearcher;

        /// <summary>
        /// Инициализирует новый экземпляр класса для поиска пользователей и компьютеров.
        /// </summary>
        /// <param name="adSearchRoot">Путь для поиска в Active Directory.</param>
        /// <param name="sccmServer">Имя или адрес сервера Configuration Manager.</param>
        public Searcher(string adSearchRoot, string sccmServer)
        {
            try
            {
                _adSearcher = new ADSearcher(adSearchRoot);
                _sccmSearcher = new SCCMSearcher(sccmServer);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Возвращает список пользователей по имени или логину. Имя/логин можно указать не полностью.
        /// </summary>
        public List<User> GetUsersByNameOrLogin(string nameOrLogin)
        {
            List<ADUser> adUsers = new List<ADUser>();

            try
            {
                adUsers = _adSearcher.GetUsersByNameOrLogin(nameOrLogin);
            }
            catch
            {
                throw;
            }

            List<User> users = new List<User>();

            foreach (ADUser adUser in adUsers)
                users.Add(new User(adUser));

            return users;
        }

        /// <summary>
        /// Возвращает список компьютеров по логину пользователя.
        /// </summary>
        public List<Computer> GetComputersByUserLogin(string userLogin)
        {
            List<SCCMComputer> sccmComputers = new List<SCCMComputer>();
        
            try
            {
                sccmComputers = _sccmSearcher.GetComputersByUserLogin(userLogin);
            }
            catch
            {
                throw;
            }

            List<Computer> computers = new List<Computer>();

            foreach (SCCMComputer sccmComputer in sccmComputers)
            {
                ADComputer adComputer;

                try
                {
                    adComputer = _adSearcher.GetComputerByName(sccmComputer.Name);
                }
                catch
                {
                    throw;
                }

                computers.Add(new Computer(sccmComputer, adComputer));
            }

            return computers;
        }
    }
}
