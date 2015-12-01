using CourceToWebUC.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt.DataLoader
{
    /// <summary>
    /// класс параметров xslt
    /// преобразования курса
    /// десериализованные из xml 
    /// файла
    /// </summary>
    [Serializable]
    public class SerializeXSLTParams
    {
        /// <summary>
        /// название основного
        /// (стартового) файла сконвертированного
        /// курса
        /// </summary>
        public string StartFileName;
        /// <summary>
        /// имя корневой папки с данными
        /// (на обном уровне со стартовым
        /// файлом сконвертированного курса)
        /// </summary>
        public string RootFolderName;
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится медиа файлы курса
        /// </summary>
        public string CourseDataFolderName;
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится инструменты курса
        /// </summary>
        public string ToolsDataFolderName;
        /// <summary>
        /// путь к папке шаблона
        /// (копируется полностью)
        /// </summary>
        public string TemplatePath; 
        /// <summary>
        /// путь к схеме для
        /// преобразования темы курса
        /// </summary>
        public string ThemeShemePath;
        /// <summary>
        /// путь к схеме для
        /// преобразования теста курса
        /// </summary>
        public string TestShemePath;
        /// <summary>
        /// путь к схеме для 
        /// преобразования дерева
        /// курса
        /// </summary>
        public string ContentShemePath;
        /// <summary>
        /// параметры изображения инструмента
        /// </summary>
        public ToolImgParam ToolImg;
        /// <summary>
        /// конвертировать видео в html5 формат
        /// </summary>
        public int ConvertVideoToHTML5;
        /// <summary>
        /// схема конвертации манифеста scorm
        /// </summary>
        public string ManifestSheme;
      
    }
}
