/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IDataService.cs
 * Создан: 25.02.2015
 * Редактирован: 25.02.2015
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.CommonModels;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс получения 
    /// курса
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// получить курс
        /// </summary>
        Course Course { get; }
        /// <summary>
        /// название курса
        /// </summary>
        /// <returns>название курса</returns>
        string GetTitle();
        /// <summary>
        /// получает список
        /// единиц курса
        /// </summary>
        /// <returns>список единиц курса</returns>
        /// <exception cref="AppException">
        /// Возникает в случае ошибки получения</exception>
        IList<IItemModel> GetItems();
       
    }


}
