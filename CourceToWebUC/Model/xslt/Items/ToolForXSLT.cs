/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ToolForXSLT.cs
 * Создан: 21.04.2015
 * Редактирован: 21.04.2015
 */

using CourceToWebUC.Model.CommonModels;
using CourceToWebUC.Model.Helpers;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// инструмент для конвертации xslt
    /// </summary>
    internal class ToolForXSLT:ISubItemForXSLT,ITool
    {
        /// <summary>
        /// инструмент
        /// </summary>
        ToolModel tool;
        /// <summary>
        /// параметры изображения инструментов
        /// (нужны для преобразования swf в png)
        /// </summary>
        ToolImgParam imgParams;
        /// <summary>
        /// имя файла инструмента
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// идентификатор инструмента
        /// </summary>
        public Guid Id { get { return tool.Id; } }
        /// <summary>
        /// название инструмента
        /// </summary>
        public string Name { get { return tool.Name; } }        
        /// <summary>
        /// полный путь к файлу 
        /// изображения инструмента
        /// </summary>
        public string AbsOrigPath { get { return tool.AbsOrigPath; } }
        /// <summary>
        /// обозначение инструмента
        /// </summary>
        public string Marking { get { return tool.Marking; } }
        
        /// <summary>
        /// инструмент для конвертации xslt
        /// </summary>
        /// <param name="_tool">модель инструмента</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ToolForXSLT(ToolModel _tool)
        {
            if (_tool == null)
                throw new ArgumentNullException("Нулевое значение инструмента");
            tool = _tool;

            if (!tool.IsToolSWF)
                FileName = System.IO.Path.GetFileName(_tool.AbsOrigPath);
            else
                SetFileNameAsPng();
        }

        /// <summary>
        /// установить имя файла инструмента
        /// с расширением jpg
        /// </summary>
        private void SetFileNameAsPng()
        {
            string flName = System.IO.Path.GetFileNameWithoutExtension(tool.AbsOrigPath);
            FileName = flName + ".png";
        }
        /// <summary>
        /// копировать файл инструмента
        /// </summary>
        /// <param name="_outputFolder">выходная директория</param>
        /// <exception cref="ArgumentNullException"></exception> 
        public void CopyFiles(string _outputFolder)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для инструментов");
            try
            {
                _outputFolder = _outputFolder.AsPath();
               
               string toolNewPath = System.IO.Path.Combine(_outputFolder, FileName);
               if(!tool.IsToolSWF)
               {
                    if (!System.IO.File.Exists(toolNewPath))
                    { 
                        System.IO.File.Copy(tool.AbsOrigPath, toolNewPath, false);
                    }
               }
               else
               {
                   FileConvertor.ConvertSWFToPng(tool.AbsOrigPath, toolNewPath, imgParams.Width, imgParams.Height);
               }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов инструмента " + tool.Id + ". " + ex.Message);
            }
        }

        /// <summary>
        /// получить инструмент
        /// в xml виде
        /// </summary>
        /// <returns>элемент-инструмент</returns>
        public System.Xml.Linq.XElement AsXMLForConvert()
        {
            XElement xtool =
             new XElement(NodeNames.TOOL,
                   new XAttribute(NodeNames.IDENTIFER, Id),
                   new XAttribute(NodeNames.NEW_ATTR_NAME, Name),
                   new XAttribute(NodeNames.NEW_ATTR_FILE, FileName),
                   new XAttribute(NodeNames.NEW_ATTR_MARKING, Marking)

           );

            return xtool;
        }
        /// <summary>
        /// установить параметры копирования изображений инструментов
        /// </summary>
        /// <param name="_toolParams">параметры изображения инструментов</param>
        internal void SetCopyParams(ToolImgParam _toolParams)
        {
            if (_toolParams == null)
                throw new ArgumentNullException("Не заданы параметры изображения инструментов");

            imgParams = _toolParams;
        }
    }
}
