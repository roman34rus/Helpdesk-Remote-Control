using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace HelpdeskRemoteControl.Core
{
    /// <summary>
    /// Статический класс, содержащий вспомогательные методы.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Выводит сообщение об ошибке.
        /// </summary>
        public static void ErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK,  MessageBoxImage.Error);
        }

        /// <summary>
        /// Создает BitmapImage из массива byte[].
        /// </summary>
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray != null && byteArray.Length > 0)
            {
                var ms = new MemoryStream(byteArray);
                var bi = new BitmapImage();
 
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }
            else
            {
                return null;
            }
        }
    }
}
