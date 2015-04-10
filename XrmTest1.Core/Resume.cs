namespace XrmTest1.Core
{
    /// <summary>
    /// Резюме
    /// реализованы не все свойства для загрузки
    /// </summary>
    public class Resume
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Возраст
        /// </summary>
        public int? Age { get; set; }
        
        /// <summary>
        /// Заголовок (должность)
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Контактные данные
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Зарплата
        /// </summary>
        public string Salary { get; set; }
    }
}
