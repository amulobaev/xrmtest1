using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using XrmTest1.Core;
using XrmTest1.Data;

namespace XrmTest1.ViewModels
{
    /// <summary>
    /// Модель представления главного окна
    /// </summary>
    class MainViewModel : ValidatedViewModelBase
    {
        /// <summary>
        /// Количество запрашиваемых резюме с сайта за одно обращение
        /// </summary>
        private const int PageSize = 10;

        #region Поля

        private readonly IRepository<Resume> _resumeRepository;
        private int? _offset = 0;
        private readonly ObservableCollectionEx<Resume> _resumes = new ObservableCollectionEx<Resume>();
        private bool _isLoadingFromSite;
        private int _resumesCountOnServer;
        private readonly IDataReceiver _dataReceiver;
        private ICommand _loadResumeFromSiteCommand;
        private ICommand _updateCountCommand;
        private int? _count = 50;
        private int _progress;
        private ICommand _loadResumeFromBaseCommand;
        private bool _isLoadingFromBase;
        private bool _filterByName;
        private bool _filterByHeader;
        private bool _filterByAge;
        private string _name;
        private string _header;
        private int? _ageFrom;
        private int? _ageTo;

        #endregion Поля

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataReceiver">Реализация интерфейса загрузки данных</param>
        /// <param name="resumeRepository">Реализация репозитария для резюме</param>
        public MainViewModel(IDataReceiver dataReceiver, IRepository<Resume> resumeRepository)
        {
            _dataReceiver = dataReceiver;
            _resumeRepository = resumeRepository;

            // Запросить количество резюме на сайте 
            UpdateCount();
        }

        #region Свойства

        /// <summary>
        /// Общее количество резюме на сайте
        /// </summary>
        public int ResumesCountOnServer
        {
            get { return _resumesCountOnServer; }
            set { Set(() => ResumesCountOnServer, ref _resumesCountOnServer, value); }
        }

        /// <summary>
        /// Количество резюме для загрузки
        /// </summary>
        [Range(1, 999999, ErrorMessage = @"Некорректное значение")]
        public int? Count
        {
            get { return _count; }
            set { Set(() => Count, ref _count, value); }
        }

        /// <summary>
        /// Смещение при загрузке (сколько пропустить резюме)
        /// </summary>
        [Range(0, 999999, ErrorMessage = @"Некорректное значение")]
        public int? Offset
        {
            get { return _offset; }
            set { Set(() => Offset, ref _offset, value); }
        }

        /// <summary>
        /// Флаг загрузки резюме с сайта
        /// </summary>
        public bool IsLoadingFromSite
        {
            get { return _isLoadingFromSite; }
            set { Set(() => IsLoadingFromSite, ref _isLoadingFromSite, value); }
        }

        /// <summary>
        /// Флаг загрузки резюме из репозитария
        /// </summary>
        public bool IsLoadingFromBase
        {
            get { return _isLoadingFromBase; }
            set { Set(() => IsLoadingFromBase, ref _isLoadingFromBase, value); }
        }

        /// <summary>
        /// Команда загрзуки резюме с сайта
        /// </summary>
        public ICommand LoadResumeFromSiteCommand
        {
            get
            {
                return _loadResumeFromSiteCommand ?? (_loadResumeFromSiteCommand = new RelayCommand(LoadResumeFromSite));
            }
        }

        /// <summary>
        /// Команда загрузки резюме из базы
        /// </summary>
        public ICommand LoadResumeFromBaseCommand
        {
            get
            {
                return _loadResumeFromBaseCommand ??
                       (_loadResumeFromBaseCommand = new RelayCommand(LoadResumeFromBase, IsValidLoadFromBase));
            }
        }

        /// <summary>
        /// Коллекция резюме для отображения в таблице
        /// </summary>
        public ObservableCollection<Resume> Resumes
        {
            get { return _resumes; }
        }

        /// <summary>
        /// Команда обновления количества резюме на сайте
        /// </summary>
        public ICommand UpdateCountCommand
        {
            get { return _updateCountCommand ?? (_updateCountCommand = new RelayCommand(UpdateCount)); }
        }

        /// <summary>
        /// Прогресс загрузки резюме с сайта (в процентах)
        /// </summary>
        public int Progress
        {
            get { return _progress; }
            set { Set(() => Progress, ref _progress, value); }
        }

        /// <summary>
        /// Флаг активации фильтра по имени
        /// </summary>
        public bool FilterByName
        {
            get { return _filterByName; }
            set { Set(() => FilterByName, ref _filterByName, value); }
        }

        /// <summary>
        /// Флаг активации фильтра по должности
        /// </summary>
        public bool FilterByHeader
        {
            get { return _filterByHeader; }
            set { Set(() => FilterByHeader, ref _filterByHeader, value); }
        }

        /// <summary>
        /// Флаг активации фильтра по возрасту
        /// </summary>
        public bool FilterByAge
        {
            get { return _filterByAge; }
            set { Set(() => FilterByAge, ref _filterByAge, value); }
        }

        /// <summary>
        /// Имя для фильтра
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        /// <summary>
        /// Должность для фильтра
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { Set(() => Header, ref _header, value); }
        }

        /// <summary>
        /// "Возраст от" для фильтра
        /// </summary>
        public int? AgeFrom
        {
            get { return _ageFrom; }
            set { Set(() => AgeFrom, ref _ageFrom, value); }
        }

        /// <summary>
        /// "Возраст до" для фильтра
        /// </summary>
        public int? AgeTo
        {
            get { return _ageTo; }
            set { Set(() => AgeTo, ref _ageTo, value); }
        }

        #endregion Свойства

        #region Методы

        /// <summary>
        /// Запрос количества резюме на сайте (асинхронно)
        /// </summary>
        private void UpdateCount()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, args) =>
            {
                args.Result = _dataReceiver.GetCount();
            };
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                ResumesCountOnServer = (int)args.Result;
            };
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Загрузка резюме с сайта (асинхронно)
        /// </summary>
        private void LoadResumeFromSite()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            backgroundWorker.DoWork += (sender, args) =>
            {
                Tuple<int, int> inputArgs = args.Argument as Tuple<int, int>;
                int count = inputArgs.Item1;
                int offset = inputArgs.Item2;

                // Загрузка данных с сайта постранично
                while (count > 0)
                {
                    int limit = count > PageSize ? PageSize : count;
                    Response response = _dataReceiver.GetData(limit, offset);
                    if (response != null && response.Resumes != null && response.Resumes.Any())
                    {
                        foreach (Resume item in response.Resumes)
                            _resumeRepository.CreateOrUpdate(item);
                    }
                    count -= limit;
                    offset += limit;

                    // Уведомление о прогрессе загрузки
                    backgroundWorker.ReportProgress((int)(((double)offset / inputArgs.Item1) * 100));
                }
            };
            backgroundWorker.ProgressChanged += (sender, args) =>
            {
                Progress = args.ProgressPercentage;
            };
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                Progress = 0;
                IsLoadingFromSite = false;
            };

            backgroundWorker.RunWorkerAsync(new Tuple<int, int>(Count.Value, Offset.Value));
            IsLoadingFromSite = true;
        }

        /// <summary>
        /// Загрузка резюме из репозитария (асинхронно)
        /// </summary>
        private void LoadResumeFromBase()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (sender, args) =>
            {
                var filterArgs = args.Argument as List<Filter>;
                // Запрос данных из репозитария
                args.Result = filterArgs != null && filterArgs.Any()
                    ? _resumeRepository.GetFiltered(filterArgs)
                    : _resumeRepository.GetAll().ToList();
            };
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                // Загрузка данных в таблицу резюме
                _resumes.Clear();
                _resumes.AddRange(args.Result as List<Resume>);
                IsLoadingFromBase = false;
            };

            // Подготовка списка фильтров
            List<Filter> filters = new List<Filter>();
            if (FilterByName)
                filters.Add(new Filter(FilterType.Name, Name));
            if (FilterByHeader)
                filters.Add(new Filter(FilterType.Header, Header));
            if (FilterByAge)
                filters.Add(new Filter(FilterType.Age, AgeFrom, AgeTo));

            IsLoadingFromBase = true;
            backgroundWorker.RunWorkerAsync(filters);
        }

        /// <summary>
        /// Проверка настроек фильтра
        /// </summary>
        /// <returns></returns>
        private bool IsValidLoadFromBase()
        {
            if (FilterByName && string.IsNullOrEmpty(Name))
            {
                //MessageBox.Show("Не указано имя");
                return false;
            }

            if (FilterByHeader && string.IsNullOrEmpty(Header))
            {
                //MessageBox.Show("Не указана должность");
                return false;
            }

            if (FilterByAge && !AgeFrom.HasValue && !AgeTo.HasValue)
            {
                //MessageBox.Show("Не указан возраст");
                return false;
            }

            return true;
        }

        #endregion Методы

    }
}