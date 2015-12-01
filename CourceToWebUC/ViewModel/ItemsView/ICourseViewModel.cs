/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ICourseViewModel.cs
 * Создан: 25.02.2015
 * Редактирован: 02.03.2015
 */

using CommonConvertLib.StepMessager;
using CourceToWebUC.Model;
using CourceToWebUC.View.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// интерфейс представления
    /// курса
    /// </summary>
    public interface ICourseViewModel : IItemViewModel, IStepMessager
    {
        /// <summary>
        /// событие завершения конвертации
        /// </summary>        
        event EventHandler OnConverted;      
        /// <summary>
        /// конвертировать курс
        /// </summary>
       // void ConvertCourse();
       
        /// <summary>
        /// конвертировать курс        
        /// </summary>
        /// <param name="_param">параметры конвертации</param>
        void Convert(IConvertParam _param);
        
    }
}
