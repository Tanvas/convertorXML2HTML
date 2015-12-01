/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IConvertParam.cs
 * Создан: 25.02.2015
 * Редактирован: 03.03.2015
 */

using CourceToWebUC.Model.XSLTHelpers;
using System;


namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс параметров конвертации 
    /// </summary>
    public interface IConvertParam : IDataValidator
    {
        /// <summary>
        /// файл шаблона
        /// </summary>
        string TemplateFilePath { get; }
        /// <summary>
        /// выходной каталог 
        /// </summary>
        string OutputAbsPath { get; }
        /// <summary>
        /// сформировать в конце Scorm пакет
        /// </summary>
        bool IsToScorm { get; }
        

        /// <summary>
        /// обработчик обновления       
        /// параметров       
        /// </summary>
        event EventHandler<EventArgs> Updated;
        /// <summary>
        /// обновить параметры
        /// </summary>     
        /// <param name="_param">параметры</param>
        void Update(IConvertParam _param);

        
                
    }
}
