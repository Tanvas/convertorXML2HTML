using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс валидатора 
    /// параметров 
    /// </summary>
    public interface IParamValidator
    {
        /// <summary>
        /// корректны ли параметры
        /// </summary>
        bool IsParamsValid { get; }
    }
}
