/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IItemModelForXSLT.cs
 * Создан: 25.02.2015
 * Редактирован: 03.03.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// интерфейс элемента курса
    /// для конвертации его
    /// с помощью xslt 
    /// </summary>
    interface IItemModelForXSLT : IItemModel, ISubItemForXSLT
    {
       
       
       


        //GetXSLT();
    }
}
