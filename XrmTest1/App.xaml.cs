using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Threading;

namespace XrmTest1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // Инициализация DispatcherHelper
            DispatcherHelper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Установка локали для корректного форматирования дат и чисел
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // Создание главного окна
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
