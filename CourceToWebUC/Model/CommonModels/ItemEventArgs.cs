/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemEventArgs.cs
 * Создан: 25.02.2015
 * Редактирован: 26.02.2015
 */
using ContentLib.Core.Content.Learning;
using System;
using System.Collections.Generic;


namespace CourceToWebUC.Model
{
    /// <summary>
    /// класс для добавления
    /// свойств в обработчик события 
    /// ICourseModel.ItemConvert
    /// </summary>
    public class ItemEventArgs:EventArgs
    {
        public IItemModel Item { get; private set; }

        public ItemEventArgs(IItemModel _item)
        {
            Item = _item;
        }
    }
}
