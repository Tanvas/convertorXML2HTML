/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: XSLTConvertParams.cs
 * Создан: 06.03.2015
 * Редактирован: 06.03.2015
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CourceToWebUC.Model.xslt.DataLoader
{
    /// <summary>
    /// класс получения параметров
    /// xslt преобразования из
    /// xml файла
    /// </summary>
    class XSLTParamFromXML:IXSLTConvertParam
    {
      
        /// <summary>
        /// посленяя ошибка
        /// 
        /// </summary>
        public string LastError { get; private set; }
        /// <summary>
        /// путь к папке с данными
        /// конвертации
        /// </summary>
        readonly string dataPath;
        /// <summary>
        /// название основного
        /// (стартового) файла сконвертированного
        /// курса
        /// </summary>
        public string StartFileName
        {
            get { return data.StartFileName; }
        }

        /// <summary>
        /// имя корневой папки с данными
        /// (на обном уровне со стартовым
        /// файлом сконвертированного курса)
        /// </summary>
        public string RootFolderName
        {
            get { return data.RootFolderName; }
        }

        /// <summary>
        /// имя папки, в которой будут 
        /// хранится медиа файлы курса
        /// </summary>
        public string CourseDataFolderName
        {
            get { return data.CourseDataFolderName; }
        }
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится инструменты курса
        /// </summary>
        public string ToolsDataFolderName 
        {
            get { return data.ToolsDataFolderName; } 
        }
        /// <summary>
        /// путь к папке шаблона
        /// (копируется полностью)
        /// </summary>
        public string TemplatePath
        {
            get { return Path.Combine(dataPath,data.TemplatePath); }
        }

        /// <summary>
        /// путь к схеме для
        /// преобразования темы курса
        /// </summary>
        public string ThemeShemePath
        {
            get { return Path.Combine(dataPath, data.ThemeShemePath); }
        }
        /// <summary>
        /// путь к схеме для
        /// преобразования теста курса
        /// </summary>
        public string TestShemePath
        {
            get { return Path.Combine(dataPath, data.TestShemePath); }
        }
        /// <summary>
        /// путь к схеме для 
        /// преобразования дерева
        /// курса
        /// </summary>
        public string ContentShemePath
        {
            get { return Path.Combine(dataPath,data.ContentShemePath); }
        }
        /// <summary>
        /// параметры изображения инструмента
        /// </summary>
        public CommonModels.ToolImgParam ToolImg
        {
            get { return data.ToolImg; }
        }
        /// <summary>
        /// конвертировать видео в html5 формат
        /// </summary>
        public bool ConvertVideoToHTML5
        {
            get { return data.ConvertVideoToHTML5 == 1; }
        }
        /// <summary>
        /// схема конвертации манифеста scorm
        /// </summary>
        public string ManifestSheme 
        {
            get { return Path.Combine(dataPath,data.ManifestSheme); } 
        }
        /// <summary>
        /// параметры флэш не грузятся из xml
        /// (в данной версии приложения)
        /// </summary>
        public XSLTHelpers.FlashSettings FlashParam
        {
            get { return null; }
        }

        /// <summary>
        /// класс получения параметров
        /// из xml файла
        /// </summary>
        /// <param name="_pathToDataFile">путь к папке с параметрами конвертации</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AppException">ошибка получения данных</exception>
        public XSLTParamFromXML(string _pathToDataFile)
        {
            if (_pathToDataFile == null)
                throw new ArgumentNullException();

            try
            {
                dataPath =System.IO.Path.GetDirectoryName(_pathToDataFile);


                LoadData(_pathToDataFile);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);

            }
           
        }
        /// <summary>
        /// данные десериализации
        /// </summary>
        SerializeXSLTParams data;
        /// <summary>
        /// загрузить данные из
        /// xml файла
        /// </summary>
        /// <param name="_xmlPath">путь к файлу</param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AppException"></exception>
        private void LoadData(string _xmlPath)
        {
            try
            {
                TextReader tr = new StreamReader(_xmlPath);
                XmlSerializer reader = new XmlSerializer(typeof(SerializeXSLTParams));
                data = (SerializeXSLTParams)reader.Deserialize (tr);
                tr.Close();

                
                data.ToolImg.FixParams();
            }            
            catch(IOException ex)
            {
                throw new Exception("Ошибка исходного файла xml: " + _xmlPath + " " + ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                throw new Exception("Невозможно представить данные xml файла в виде структуры данных параметров xslt преобразования: "+ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка считывания и преобразования данных XSLT преобразования из xml файла: " + ex.Message);
            }
        }





        
    }
}
