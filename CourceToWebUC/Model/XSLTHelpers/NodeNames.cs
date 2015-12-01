/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: DataServiceCourseLib.cs
 * Создан: 31.03.2015
 * Редактирован: 31.03.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.XSLTHelpers
{
    /// <summary>
    /// названия узлов при
    /// формировании xml
    /// документа курса
    /// </summary>
    public static class NodeNames
    {
        /// <summary>
        /// корневой элемент курса
        /// </summary>
        public const string COURSE_ROOT = "Course";
        /// <summary>
        /// Идентификатор
        /// </summary>
        public const string IDENTIFER = "id";
        /// <summary>
        /// заголовок курса/элемента
        /// курса
        /// </summary>
        public const string TITLE = "title";
        /// <summary>
        /// Идентификатор элемента
        /// для имени выходного файла элемента
        /// </summary>
        public const string ITEM_FILE_ID = "file";        
        /// <summary>
        /// название корневого узла 
        /// xml представления секции
        /// </summary>
        public const string SECTION_ROOT = "LearningSection";
        /// <summary>
        /// название корневого узла 
        /// xml представления темы
        /// </summary>
        public const string THEME_ROOT = "LearningItem";
        /// <summary>
        /// название корневого узла 
        /// xml представления вопроса
        /// </summary>
        public const string TEST_ROOT = "Test";
        /// <summary>
        /// название xml узла
        /// варианта ответа
        /// </summary>
        public const string ANSWER = "option";
        /// <summary>
        /// название xml атрибута
        /// идентификатора
        /// </summary>
        public const string NEW_ATTR_ID = "id";
        /// <summary>
        /// название xml атрибута
        /// хэша варианта ответа (от идентификатора или 
        /// порядкового номера (order))
        /// </summary>
        public const string NEW_ATTR_HASH = "hash";       
        /// <summary>
        /// название  узла 
        /// кадров темы
        /// </summary>
        public const string STEPS_ROOT = "learningSteps";
        /// <summary>
        /// название корневого узла 
        /// xml представления кадра
        /// </summary>
        public const string STEP_ROOT = "LearningStep";
        /// <summary>
        /// название  узла 
        /// изображения кадра
        /// </summary>
        public const string PICTURE = "picture";
        /// <summary>
        /// название  узла 
        /// текста кадра
        /// </summary>
        public const string TEXT = "text";
        /// <summary>
        /// название корневого узла 
        /// xml инструментов
        /// </summary>
        public const string TOOLS_ROOT = "Tools";
        /// <summary>
        /// название  узла 
        /// xml инструмента
        /// </summary>
        public const string TOOL = "Tool";
        /// <summary>
        /// название xml атрибута
        /// названия
        /// </summary>
        public const string NEW_ATTR_NAME = "name";
        /// <summary>
        /// название xml атрибута
        /// имени файла
        /// </summary>
        public const string NEW_ATTR_FILE = "fileName";
        /// <summary>
        /// название xml атрибута
        /// маркировки
        /// </summary>
        public const string NEW_ATTR_MARKING = "marking";
        /// <summary>
        /// название узла 
        /// xml предупреждения
        /// </summary>
        public const string WARNING_ROOT = "warning";
        /// <summary>
        /// название xml атрибута
        /// типа предупреждения
        /// </summary>
        public const string ATTR_TYPE_WARN = "flag";
        /// <summary>
        /// название xml атрибута
        /// текста предупреждения
        /// </summary>
        public const string ATTR_TEXT_WARN = "text";
        /// <summary>
        /// новый атрибут кадра,
        /// указывающий я вляется ли видео
        /// однокадровым
        /// </summary>
        public const string NEW_ATTR_IS_SINGLE_FRAME = "isSingle";
        /// <summary>
        /// локальный идентификатор
        /// </summary>
        public const string LOCALIDENTIFER = "locid";


    }
}
