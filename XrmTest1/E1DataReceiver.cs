using System;
using System.Net.Http;
using System.Web;

namespace XrmTest1
{
    /// <summary>
    /// Реализация интерфейса загрузки данных для сайта E1
    /// </summary>
    class E1DataReceiver : IDataReceiver
    {
        private const string ApiUrl = "http://rabota.e1.ru/api/v1/resumes";

        public int GetCount()
        {
            Response response = GetData(1,0);
            return response != null ? response.Metadata.resultset.Count : -1;
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <param name="limit">Количество записей для загрузки</param>
        /// <param name="offset">Смещение</param>
        /// <returns></returns>
        public Response GetData(int limit, int offset)
        {
            // Формирование запроса
            UriBuilder builder = new UriBuilder(ApiUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            builder.Query = query.ToString();

            // Получение данных
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Response>().Result : null;
            }
        }
    }
}