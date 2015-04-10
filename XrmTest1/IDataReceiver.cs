namespace XrmTest1
{
    /// <summary>
    /// Интерфейс загрузки данных
    /// </summary>
    public interface IDataReceiver
    {
        /// <summary>
        /// Запрос количества элементов
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <param name="count">Количество резюме</param>
        /// <param name="offset">Смещение</param>
        /// <returns></returns>
        Response GetData(int count, int offset);
    }
}