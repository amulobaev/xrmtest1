using System.Collections.Generic;

namespace XrmTest1.Data
{
    /// <summary>
    /// Обобщённый интерфейс репозитария
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Получение сущности по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        IEnumerable<T> GetFiltered(IEnumerable<Filter> filters);

        /// <summary>
        /// Создание сущности
        /// </summary>
        /// <param name="model"></param>
        void Create(T model);
        
        /// <summary>
        /// Обновление данных сущности
        /// </summary>
        /// <param name="model"></param>
        void Update(T model);
        
        /// <summary>
        /// Создание или обновление данных сущности по идентификатору
        /// </summary>
        /// <param name="model"></param>
        void CreateOrUpdate(T model);
        
        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="model"></param>
        void Delete(T model);
    }
}