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
using HelpdeskRemoteControl.Core;

namespace HelpdeskRemoteControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        //-----------------------------------------------------------------
        // Вспомогательные cвойства методы.
        //-----------------------------------------------------------------

        /// <summary>
        /// Название приложения.
        /// </summary>
        private string _productName;

        /// <summary>
        /// Версия приложения.
        /// </summary>
        private string _productVersion;

        /// <summary>
        /// Информация об авторских правах.
        /// </summary>
        private string _copyright;

        /// <summary>
        /// Поиск пользователей, вывод результатов в ListBox UserList.
        /// </summary>
        private void SearchUser()
        {
            ADSearcher adSearcher = new ADSearcher();
            
            UserList.ItemsSource = adSearcher.GetUsersByNameOrLogin(UserSearchString.Text.ToString());
        }

        /// <summary>
        /// Поиск компьютеров, вывод результатов в ListBox ComputerList.
        /// </summary>
        private void SearchComputer()
        {
            SCCMSearcher sccmSearcher = new SCCMSearcher();
            
            ComputerList.ItemsSource = sccmSearcher.GetComputersByUserLogin(ComputerSearchString.Text.ToString());
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
        // Обработчики событий.
        //-----------------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();

            // Получение данных из информации о сборке.
            _productVersion = Application.Current.MainWindow.GetType().Assembly.GetName().Version.ToString();

            object[] attributes = Application.Current.MainWindow.GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
            if (attributes.Length > 0)
            {
                _productName = ((AssemblyProductAttribute)attributes[0]).Product;
            }

            attributes = Application.Current.MainWindow.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            if (attributes.Length > 0)
            {
                _copyright = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
            
            // Установка параметров окна.
            Title = _productName;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            UserSearchString.Focus();

            SettingsReader.ReadSettings();
        }

        private void SearchUserButton_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        private void UserSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchUser();
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

                // Заполняем поле для поиска компьютеров.
                ComputerSearchString.Text = user.Login;
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
                ComputerSearchString.Text = "";
            }
        }

        private void UserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserList.SelectedIndex != -1)
                ComputerTab.Focus();
        }   

        private void SearchComputerButton_Click(object sender, RoutedEventArgs e)
        {
            SearchComputer();
        }

        private void ComputerSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchComputer();
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
                ComputerLastUserDomainAndName.Text = computer.LastUserDomain + @"\" + computer.LastUserName;
                ComputerLastUserLoginTime.Text = computer.LastUserLogonTime.ToString();
                ComputerSCCMClientVersion.Text = computer.SCCMClientVersion;
                ComputerSCCMAssignedSites.Text = computer.SCCMAssignedSites;
                ComputerDescription.Text = computer.Description;

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
            MessageBox.Show(_productName + Environment.NewLine + "v" + _productVersion + Environment.NewLine + _copyright, "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
