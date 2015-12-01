/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IParamsView.cs
 * Создан: 03.03.2015
 * Редактирован: 04.03.2015
 */

using CourceToWebUC.Model;
using System;
using System.ComponentModel;


namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// интерфейс представления 
    /// параметров конвертации
    /// </summary>
    public interface IParamsView :INotifyPropertyChanged, IConvertParam
    {
        
        
        /// <summary>
        /// обновить параметры
        /// </summary>
        void Update();
        /// <summary>
        /// получить модель 
        /// параметров конвертации
        /// </summary>
        /// <returns>модель параметров конвертации</returns>
        IConvertParam GetParams();
    }
}
