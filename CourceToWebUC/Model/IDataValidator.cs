using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс валидации данных
    /// </summary>
    public interface IDataValidator
    {
        /// <summary>
        /// корректны ли параметры
        /// </summary>
        bool IsParamsValid { get; }
        /// <summary>
        /// сообщение об ошибочном
        /// параметре
        /// </summary>
        string ErrorMessage { get; }
    }
}
