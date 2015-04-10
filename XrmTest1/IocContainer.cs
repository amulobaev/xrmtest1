using GalaSoft.MvvmLight;
using Ninject;

namespace XrmTest1
{
    /// <summary>
    /// Обёртка над Ninject
    /// </summary>
    internal sealed class IocContainer
    {
        private static readonly IKernel DefaultInstance = ViewModelBase.IsInDesignModeStatic
            ? new StandardKernel(new DesignTimeModule())
            : new StandardKernel(new RunTimeModule());

        static IocContainer()
        {
        }

        /// <summary>
        /// Экзепляр ядра Ninject по умолчанию
        /// </summary>
        public static IKernel Default
        {
            get { return DefaultInstance; }
        }
    }
}
