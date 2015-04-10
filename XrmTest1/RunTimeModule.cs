using Ninject.Modules;
using XrmTest1.Core;
using XrmTest1.Data;
using XrmTest1.ViewModels;

namespace XrmTest1
{
    /// <summary>
    /// Настройка Ninject для времени исполнения
    /// </summary>
    internal class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<MainViewModel>().ToSelf().InSingletonScope();
            Bind<IRepository<Resume>>().To<ResumeRepository>();
            Bind<IDataReceiver>().To<E1DataReceiver>();
        }
    }
}