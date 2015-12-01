using ContentLib.Core.Content.Learning;
/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ToolsFactory.cs
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
    /// фабрика инструментов
    /// </summary>
    internal class ToolsFactory
    {
        /// <summary>
        /// список инструментов
        /// </summary>
        readonly ToolsList tools;
        /// <summary>
        /// абсолютный путь до инструментов
        /// </summary>
        readonly string absPathToTools;

        /// <summary>
        /// фабрика инструментов
        /// </summary>
        /// <param name="_tools">список инструментов</param>
        /// <param name="_absPathToTools">абсолютный путь до инструментов</param>
        public ToolsFactory(ToolsList _tools, string _absPathToTools)
        {
            if (_tools == null || string.IsNullOrEmpty(_absPathToTools))
                throw new ArgumentNullException("Переда нулевой список инструментов в конструктор ToolsFactory");
            
            tools = _tools;
            absPathToTools = _absPathToTools;
        }
        /// <summary>
        /// создать инструмент
        /// </summary>
        /// <param name="_id">идентификатор инструмента</param>
        /// <returns>инструмент</returns>
        public ToolModel Create(Guid _id)
        {
            if(_id==null)
                throw new ArgumentNullException("Невозможно создать инструмент по нулевому идентификатору ToolsFactory");


            if (!tools.tools.Keys.Contains<Guid>(_id))
                throw new Exception("Не найден инструмент с идентификатором " + _id);

            Tool tl=tools.tools[_id];

            return new ToolModel(tl, absPathToTools);
        }
    }
}
