/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IItemViewModel.cs
 * Создан: 25.02.2015
 * Редактирован: 02.03.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model;
using System.Xml.XPath;
using System.ComponentModel;
using System.Xml.Linq;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// интерфейс единицы курса
    /// </summary>
    public interface IItemViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// выбран элемент курса
        /// </summary>
        event EventHandler CheckedItem;
        /// <summary>
        /// название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// коллекция дочерних 
        /// элементов курса
        /// </summary>
        List<IItemViewModel> Children { get; }
        /// <summary>
        /// определяет
        /// стартовую развернутость узла
        /// </summary>
        bool IsExpanded { get; set; }
        /// <summary>
        /// родительский элемент
        /// </summary>
        IItemViewModel Parent { get; }

        /// <summary>
        /// выбор единицы курса в 
        /// дереве курса
        /// </summary>
        bool? IsChecked { get; set; }

        bool IsInitiallySelected { get; }

        /// <summary>
        /// установить параметры выделения
        /// элемента
        /// </summary>
        /// <param name="value">состояние выделения</param>
        /// <param name="updateChildren">обновлять ли потомков</param>
        /// <param name="updateParent">обновлять ли родительский узел</param>
        void SetIsChecked(bool? value, bool updateChildren, bool updateParent);

        /// <summary>
        /// проверить состояние выбора
        /// </summary>
        void VerifyCheckState();

      
       
    }
}
