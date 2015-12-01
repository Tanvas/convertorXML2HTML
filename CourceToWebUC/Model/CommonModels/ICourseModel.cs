/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ICourseModel.cs
 * Создан: 25.02.2015
 * Редактирован: 26.02.2015
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContentLib.Core.Content.Learning;
using CommonConvertLib.StepMessager;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс курса
    /// </summary>
    public interface ICourseModel : IItemModel, IStepMessager
    {
        
        
      /*  event EventHandler<ItemEventArgs> ItemConvert;
       /// <summary>
       /// конвертировать единицу курса
       /// </summary>
       /// <param name="_convertItem">единица курса</param>
        void ConvertItem(IItemModel _convertItem);*/
        /// <summary>
        /// получить элемент курса 
        /// по идентификатору 
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <returns>элемент курса</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IItemModel GetItemById(Guid _id);
        /// <summary>
        /// конвертировать курс
        /// </summary>
        /// <param name="_param">параметры конвертации</param>
        void Convert(IConvertParam _param);
       

    }
}
