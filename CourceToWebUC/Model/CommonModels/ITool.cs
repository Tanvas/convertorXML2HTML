/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ITool.cs
 * Создан: 21.04.2015
 * Редактирован: 21.04.2015
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.CommonModels
{
    /// <summary>
    /// интерфейс инструмента
    /// </summary>
    internal interface ITool
    {        
        /// <summary>
        /// идентификатор инструмента
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// название инструмента
        /// </summary>
        string Name { get; }
        /// <summary>
        /// имя файла инструмента
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// полный путь к файлу 
        /// изображения инструмента
        /// </summary>
        string AbsOrigPath { get;}
        /// <summary>
        /// обозначение инструмента
        /// </summary>
        string Marking { get; }
    }
}
