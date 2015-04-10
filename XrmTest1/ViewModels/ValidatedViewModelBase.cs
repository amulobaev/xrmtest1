using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;

namespace XrmTest1.ViewModels
{
    /// <summary>
    /// Базовая модель представления с валидацией
    /// </summary>
    public abstract class ValidatedViewModelBase : ViewModelBase, IDataErrorInfo
    {
        private readonly Dictionary<string, ValidationAttribute[]> _validators;

        private readonly Dictionary<string, Func<ValidatedViewModelBase, object>> _propertyGetters;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected ValidatedViewModelBase()
        {
            _validators =
               this.GetType()
                   .GetProperties()
                   .Where(p => GetValidations(p).Length != 0)
                   .ToDictionary(p => p.Name, p => GetValidations(p));

            _propertyGetters =
                this.GetType()
                    .GetProperties()
                    .Where(p => GetValidations(p).Length != 0)
                    .ToDictionary(p => p.Name, p => GetValueGetter(p));
        }

        #region Свойства

        /// <summary>
        /// Количество валидных свойств
        /// </summary>
        protected int ValidPropertiesCount
        {
            get
            {
                return _validators.Count(x => x.Value.All(attribute => attribute.IsValid(_propertyGetters[x.Key](this))));
            }
        }

        /// <summary>
        /// Всего свойств с валидацией
        /// </summary>
        protected int TotalPropertiesWithValidationCount
        {
            get { return _validators.Count(); }
        }

        #endregion Свойства

        #region IDataErrorInfo

        public string Error
        {
            get
            {
                IEnumerable<string> errors = from validator in _validators
                                             from attribute in validator.Value
                                             where !attribute.IsValid(_propertyGetters[validator.Key](this))
                                             select attribute.ErrorMessage;
                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (_propertyGetters.ContainsKey(columnName))
                {
                    object propertyValue = _propertyGetters[columnName](this);
                    string[] errorMessages = _validators[columnName]
                        .Where(v => !v.IsValid(propertyValue))
                        .Select(v => v.ErrorMessage).ToArray();
                    return string.Join(Environment.NewLine, errorMessages);
                }

                return string.Empty;
            }
        }

        #endregion

        #region Методы

        private ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        private Func<ValidatedViewModelBase, object> GetValueGetter(PropertyInfo property)
        {
            return viewmodel => property.GetValue(viewmodel, null);
        }

        public virtual bool IsValid()
        {
            return string.IsNullOrEmpty(Error) && ValidPropertiesCount == TotalPropertiesWithValidationCount;
        }

        #endregion
    }
}
