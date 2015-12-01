/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IXSLTConvertParam.cs
 * Создан: 06.03.2015
 * Редактирован: 06.03.2015
 * 
 * 
 * Описание:
 * Section - Учебный раздел, входящий в курс или в другой учебный раздел
 * Theme -  Учебная тема
 */

using CourceToWebUC.Model.CommonModels;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// параметры конвертации 
    /// курса преобразованием
    /// xslt
    /// </summary>
    public interface IXSLTConvertParam 
    {
        /// <summary>
        /// название основного
        /// (стартового) файла сконвертированного
        /// курса
        /// </summary>
        string StartFileName { get; }
        /// <summary>
        /// имя корневой папки с данными
        /// (на обном уровне со стартовым
        /// файлом сконвертированного курса)
        /// </summary>
        string RootFolderName { get; }
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится медиа файлы курса
        /// </summary>
        string CourseDataFolderName { get; }
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится инструменты курса
        /// </summary>
        string ToolsDataFolderName { get; }
        /// <summary>
        /// путь к папке шаблона
        /// (копируется полностью)
        /// </summary>
        string TemplatePath { get; }
        /// <summary>
        /// путь к схеме для
        /// преобразования темы курса
        /// </summary>
        string ThemeShemePath { get; }
        /// <summary>
        /// путь к схеме для
        /// преобразования теста курса
        /// </summary>
        string TestShemePath { get; }
        /// <summary>
        /// путь к схеме для 
        /// преобразования дерева
        /// курса
        /// </summary>
        string ContentShemePath { get; }
        /// <summary>
        /// параметры изображения инструмента
        /// </summary>
        ToolImgParam ToolImg { get; }
        /// <summary>
        /// конвертировать видео в html5 формат
        /// </summary>
        bool ConvertVideoToHTML5 { get; }
        /// <summary>
        /// схема конвертации манифеста scorm
        /// </summary>
        string ManifestSheme { get; }
        /// <summary>
        /// получить параметры флэша
        /// </summary>
        FlashSettings FlashParam { get; }

        
       
    }
}
