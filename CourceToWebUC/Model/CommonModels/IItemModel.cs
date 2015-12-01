/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IItemModel.cs
 * Создан: 25.02.2015
 * Редактирован: 04.03.2015
 * 
 * 
 * Описание:
 * Section - Учебный раздел, входящий в курс или в другой учебный раздел
 * Theme -  Учебная тема
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс элемента 
    /// курса
    /// </summary>
    public interface IItemModel
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        Guid Id {get;}     
        /// <summary>
        /// тип элемента
        /// </summary>
        ItemType Type { get; }
        /// <summary>
        /// название
        /// </summary>
        string Title { get; }
        /// <summary>
        /// коллекция дочерних 
        /// элементов курса
        /// </summary>
        IList<IItemModel> Items { get; }
        /// <summary>
        /// конвертировать
        /// ли элемент курса
        /// </summary>
        bool IsConvert { get; }
        /// <summary>
        /// установить флаг конвертации
        /// себе или дочерниму элементу
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="_isConvert">значение флага</param>
        /// <returns>успешность установки</returns>
        bool SetIsConvertSelfOrChildren(Guid id, bool _isConvert);
        /// <summary>
        /// получить элемент курса по 
        /// идентификатору в потомках
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <returns>элемент курса</returns>
        IItemModel GetItemInChildren(Guid _id);
       
      
    }
}
