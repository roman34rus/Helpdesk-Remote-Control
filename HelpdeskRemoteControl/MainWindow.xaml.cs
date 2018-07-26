using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using HelpdeskRemoteControl.Core;

namespace HelpdeskRemoteControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        //-----------------------------------------------------------------
        // Вспомогательные cвойства и методы
        //-----------------------------------------------------------------

        // Информация о сборке
        // Нужно вручную дублировать в свойствах проекта
        private const string _name = "Helpdesk Remote Control";
        private const string _version = "1.1.3";
        private const string _copyright = "Copyright © Любимов Роман 2015-2018";

        // Настройки из файла Settings.xml
        private string _adSearchRoot;
        private string _sccmServer;

        private Searcher _searcher;

        /// <summary>
        /// Поиск пользователей, вывод результатов в ListBox UserList.
        /// </summary>
        private void SearchUsers()
        {
            try
            {
                UserList.ItemsSource = _searcher.GetUsersByNameOrLogin(UserSearchString.Text.ToString());
            }
            catch (Exception e)
            {
                Helper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// Поиск компьютеров по имени последнего пользователя, вывод результатов в ListBox ComputerList.
        /// </summary>
        private void SearchComputersByUserLogin()
        {
            string userLogin = ComputerSearchStringUserLogin.Text.ToString();

            if (String.IsNullOrWhiteSpace(userLogin))
            {
                Helper.ErrorMessage("Нужно ввести логин пользователя.");

                return;
            }

            try
            {
                ComputerList.ItemsSource = _searcher.GetComputersByUserLogin(userLogin);
            }
            catch (Exception e)
            {
                Helper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// Поиск компьютеров по имени или описанию, вывод результатов в ListBox ComputerList.
        /// </summary>
        private void SearchComputersByNameOrDescription()
        {
            try
            {
                ComputerList.ItemsSource = _searcher.GetComputersByNameOrDescription(ComputerSearchStringNameOrDesc.Text.ToString());
            }
            catch (Exception e)
            {
                Helper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// Запуск процесса.
        /// </summary>
        /// <param name="fileName">Имя исполняемого файла.</param>
        /// <param name="parameters">Параметры запуска.</param>
        private void StartProcess(string fileName, string parameters)
        {
            try
            {
                Process.Start(fileName, parameters);
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при запуске " + fileName + ". " + e.Message);
            }
        }

        //-----------------------------------------------------------------
        // Обработчики событий
        //-----------------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();

            // Установка параметров окна
            Title = _name;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            UserSearchString.Focus();

            // Чтение настроек из файла Settings.xml
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"Settings.xml");

                _adSearchRoot = xdoc.SelectSingleNode("/Settings/ActiveDirectory/SearchRoot").InnerText;
                _sccmServer = xdoc.SelectSingleNode("/Settings/ConfigurationManager/Server").InnerText;
            }
            catch (Exception e)
            {
                Helper.ErrorMessage("Ошибка при чтении файла Settings.xml. " + e.Message);
            }
            
            // Создание объекта для поиска
            try
            {
                _searcher = new Searcher(_adSearchRoot, _sccmServer);
            }
            catch (Exception e)
            {
                Helper.ErrorMessage(e.Message);
            }
        }

        private void SearchUserButton_Click(object sender, RoutedEventArgs e)
        {
            SearchUsers();
        }

        private void UserSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchUsers();
            }
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = UserList.SelectedItem as User;

            if (user != null)
            {
                // Заполняем поля информации о пользователе.
                UserDisplayName.Text = user.DisplayName;
                UserCompany.Text = user.Company;
                UserDepartment.Text = user.Department;
                UserJobTitle.Text = user.JobTitle;
                UserOffice.Text = user.Office;
                UserMail.Text = user.Mail;
                UserWorkPhone.Text = user.WorkPhone;
                UserMobilePhone.Text = user.MobilePhone;
                UserIPPhone.Text = user.IPPhone;
                UserPhoto.Source = Helper.ByteArrayToBitmapImage(user.Photo);
                UserPasswordLastSet.Text = user.PasswordLastSet.ToString();

                // Заполняем поле для поиска компьютеров.
                ComputerSearchStringUserLogin.Text = user.Login;
            }
            else
            {
                // Очищаем поля информации о пользователе.
                UserDisplayName.Text = "";
                UserCompany.Text = "";
                UserDepartment.Text = "";
                UserJobTitle.Text = "";
                UserOffice.Text = "";
                UserMail.Text = "";
                UserWorkPhone.Text = "";
                UserMobilePhone.Text = "";
                UserIPPhone.Text = "";
                UserPhoto.Source = null;

                // Очищаем поле для поиска компьютеров.
                ComputerSearchStringUserLogin.Text = "";
            }
        }

        private void UserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserList.SelectedIndex != -1)
                ComputerTab.Focus();
        }   

        private void SearchComputerByUserLoginButton_Click(object sender, RoutedEventArgs e)
        {
            SearchComputersByUserLogin();
        }

        private void ComputerSearchStringUserLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchComputersByUserLogin();
            }
        }

        private void SearchComputerByNameOrDescButton_Click(object sender, RoutedEventArgs e)
        {
            SearchComputersByNameOrDescription();
        }

        private void ComputerSearchStringNameOrDesc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchComputersByNameOrDescription();
            }
        }

        private void ComputerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var computer = ComputerList.SelectedItem as Computer;

            if (computer != null)
            {
                // Заполняем поля информации о компьютере.
                ComputerName.Text = computer.Name;
                ComputerIPAddresses.Text = computer.IPAddresses;
                ComputerLastUserDomainAndName.Text = computer.LastUserDomainName;
                ComputerLastUserLoginTime.Text = computer.LastUserLogonTime.ToString();
                ComputerSCCMClientVersion.Text = computer.SCCMClientVersion;
                ComputerSCCMAssignedSites.Text = computer.SCCMAssignedSites;
                ComputerDescription.Text = computer.Description;
                ComputerLocalAdminPassword.Text = computer.LocalAdminPassword;

                // Заполняем поле для подключения к компьютеру.
                ConnectionString.Text = computer.Name;
            }
            else
            {
                // Очищаем поля информации о компьютере.
                ComputerName.Text = "";
                ComputerIPAddresses.Text = "";
                ComputerLastUserDomainAndName.Text = "";
                ComputerLastUserLoginTime.Text = "";
                ComputerSCCMClientVersion.Text = "";
                ComputerSCCMAssignedSites.Text = "";
                ComputerDescription.Text = "";
                ComputerLocalAdminPassword.Text = "";

                // Очищаем поле для подключения к компьютеру.
                ConnectionString.Text = "";
            }
        }

        private void ComputerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ComputerList.SelectedIndex != -1)
                RemoteTab.Focus();
        }

        private void PingButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"cmd.exe", @"/k ping " + ConnectionString.Text + @" -t");
        }

        private void CmdRemoteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"psexec.exe", @"\\" + ConnectionString.Text + @" cmd");
        }

        private void PSRemoteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"powershell.exe", @"-NoExit -Command Enter-PSSession " + ConnectionString.Text);
        }

        private void ComputerManagementButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"compmgmt.msc", @"/computer=\\" + ConnectionString.Text);
        }

        private void SCCMRemoteControlButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"RemoteControl\CmRcViewer.exe", ConnectionString.Text);
        }

        private void RemoteAssistantButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"msra.exe", @"/offerRA " + ConnectionString.Text);
        }

        private void RemoteDesktopButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcess(@"mstsc.exe", @"/v:" + ConnectionString.Text);
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_name + Environment.NewLine + "v" + _version + Environment.NewLine + _copyright, "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
