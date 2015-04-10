namespace XrmTest1.Data
{
    /// <summary>
    /// Фильтр 
    /// </summary>
    public class Filter
    {
        private readonly FilterType _filterType;
        private readonly object _value1;
        private readonly object _value2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public Filter(FilterType filterType, object value1, object value2 = null)
        {
            _filterType = filterType;
            _value1 = value1;
            _value2 = value2;
        }

        /// <summary>
        /// Тип фильтра
        /// </summary>
        public FilterType Type
        {
            get { return _filterType; }
        }

        public object Value1
        {
            get { return _value1; }
        }

        public object Value2
        {
            get { return _value2; }
        }
    }
}