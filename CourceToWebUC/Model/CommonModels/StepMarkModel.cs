/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: StepMarkModel.cs
 * Создан: 21.04.2015
 * Редактирован: 21.04.2015
 */

using ContentLib.Core.Content.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// класс предупреждения к кадру
    /// </summary>
    internal class StepMarkModel
    {
        /// <summary>
        /// тип предупреждения
        /// </summary>
        public readonly string Type; 
        /// <summary>
        /// текст предупреждения
        /// </summary>
        public readonly string Text;
        /// <summary>
        /// класс предупреждения к кадру
        /// </summary>
        /// <param name="_type">тип предупреждения</param>
        /// <param name="_text">текст предупреждения</param>
        public StepMarkModel(WarningType _type,string _text )
        {
            Type = _type.ToString("G");
            Text = _text;
        }
    }
}
