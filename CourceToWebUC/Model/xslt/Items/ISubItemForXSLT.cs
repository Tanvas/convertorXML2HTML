using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CourceToWebUC.Model.xslt
{
  
    /// <summary>
    /// интерфейс подэлемента
    /// курса для конвертации
    /// </summary>
    public interface ISubItemForXSLT
    {
        /// <summary>
        /// имя файла элемента
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// копировать файлы элемента курса
        /// в папку
        /// </summary>
        /// <param name="_outputFolder">путь к папке</param>
        void CopyFiles(string _outputFolder);

        /// <summary>
        /// получить элемент курса 
        /// как xml данные
        /// </summary>
        /// <returns>xml представление элемента</returns>
        /// <remarks>учитываются данные, помеченные для конвертации</remarks>
        XElement AsXMLForConvert();
    }
}
