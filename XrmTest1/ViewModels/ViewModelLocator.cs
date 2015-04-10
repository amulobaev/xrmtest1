using Ninject;

namespace XrmTest1.ViewModels
{
    /// <summary>
    /// Локатор моделей представления
    /// </summary>
    internal class ViewModelLocator
    {
        /// <summary>
        /// Модель представления главного окна
        /// </summary>
        public MainViewModel Main
        {
            get { return IocContainer.Default.Get<MainViewModel>(); }
        }
    }
}
