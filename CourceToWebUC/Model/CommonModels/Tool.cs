/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: Tool.cs
 * Создан: 21.04.2015
 * Редактирован: 21.04.2015
 */
using ContentLib.Core.Content.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.CommonModels
{
    /// <summary>
    /// класс инструмента
    /// </summary>
    internal  class ToolModel
    {
        /// <summary>
        /// инструмент
        /// </summary>
        Tool tool;
        /// <summary>
        /// является ли файл инструмента 
        /// swf
        /// </summary>
        public bool IsToolSWF
        {
            get
            {
                return AbsOrigPath.IsEquivExtention("swf");
            }
        }
        /// <summary>
        /// идентификатор инструмента
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// название инструмента
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// полный путь к файлу 
        /// изображения инструмента
        /// </summary>
        public string AbsOrigPath { get; private set; }
        /// <summary>
        /// обозначение инструмента
        /// </summary>
        public string Marking { get; private set; }

        /// <summary>
        /// инструмент
        /// </summary>
        /// <param name="_tool">инструмент</param>
        /// <param name="_absPath">абсолютный путь до файла с перечнем инструментов</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ToolModel(Tool _tool,string _absPath)
        {
            if (_tool == null || string.IsNullOrEmpty(_absPath))
                throw new ArgumentNullException("Нулевой инструмент в конструкторе ToolModel");

            tool = _tool;
            Id = _tool.id;
            Name = _tool.iname;
            Marking = _tool.obozn;            
            AbsOrigPath = System.IO.Path.Combine(_absPath.AsPath(), _tool.ipic);
            

        }
    }
}
