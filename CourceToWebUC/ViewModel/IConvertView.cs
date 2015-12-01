using CourceToWebUC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// интерфейс конвертации
    /// курса
    /// </summary>
    public interface IConvertView : INotifyPropertyChanged
    {
        /// <summary>
        /// возможна ли конвертация курса
        /// </summary>
        bool CanConvert{get;}        
        /// <summary>
        /// конвертировать курс
        /// </summary>
        void Convert();
    }
}
