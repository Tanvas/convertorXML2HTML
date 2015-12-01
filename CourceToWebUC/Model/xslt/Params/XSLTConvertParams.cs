/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: XSLTConvertParams.cs
 * Создан: 25.02.2015
 * Редактирован: 03.03.2015
 */

using CourceToWebUC.Model.CommonModels;
using CourceToWebUC.Model.Helpers;
using CourceToWebUC.Model.xslt.DataLoader;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс параметров конвертации
    /// курса с помощью xslt 
    /// </summary>
    public class XSLTConvertParams : IXSLTConvertParam, IConvertParam, IParamValidator
    {
        /// <summary>
        /// файл шаблона
        /// </summary>
        public string TemplateFilePath { get; private set; }
        /// <summary>
        /// выходной каталог 
        /// </summary>
        public string OutputAbsPath{get;private set;}
        /// <summary>
        /// сформировать в конце Scorm пакет
        /// </summary>
        public bool IsToScorm { get; private set; }      
        /// <summary>
        /// каталог для файлов элементов курса
        /// и шаблонов
        /// </summary>
        public string OutItemsPath{get{return Path.GetFullPath(Path.Combine(OutputAbsPath, RootFolderName)).AsPath();}}
        /// <summary>
        /// выходной каталог для медиа файлов
        /// </summary>
        public string OutMediaFiles {get{return Path.Combine(OutputAbsPath, RootFolderName, CourseDataFolderName).AsPath();}}
        /// <summary>
        /// обработчик обновления       
        /// выходного каталога        
        /// </summary>
        public event EventHandler<EventArgs> PathUpdated = delegate { };
        /// <summary>
        /// базовые параметры 
        /// конвертации
        /// </summary>
        IXSLTConvertParam baseParams;

        #region поля IXSLTConvertParam
        /// <summary>
        /// название основного
        /// (стартового) файла сконвертированного
        /// курса
        /// </summary>
        public string StartFileName
        {
            get { return baseParams==null?null:baseParams.StartFileName; }
        }

        /// <summary>
        /// имя корневой папки с данными
        /// (на одном уровне со стартовым
        /// файлом сконвертированного курса)
        /// </summary>
        public string RootFolderName
        {
            get { return baseParams == null ? null : baseParams.RootFolderName; }
        }

        /// <summary>
        /// имя папки, в которой будут 
        /// хранится медиа файлы курса
        /// </summary>
        public string CourseDataFolderName
        {
            get { return baseParams == null ? null : baseParams.CourseDataFolderName; }
        }
        /// <summary>
        /// имя папки, в которой будут 
        /// хранится инструменты курса
        /// </summary>
        public string ToolsDataFolderName 
        {
            get { return baseParams == null ? null : baseParams.ToolsDataFolderName; } 
        }
        /// <summary>
        /// путь к папке шаблона
        /// (копируется полностью)
        /// </summary>
        public string TemplatePath
        {
            get { return baseParams == null ? null : baseParams.TemplatePath; }
        }

        /// <summary>
        /// путь к схеме для
        /// преобразования темы курса
        /// </summary>
        public string ThemeShemePath
        {
            get { return baseParams == null ? null : baseParams.ThemeShemePath; }
        }
        /// <summary>
        /// путь к схеме для
        /// преобразования теста курса
        /// </summary>
        public string TestShemePath
        {
            get { return baseParams == null ? null : baseParams.TestShemePath; }
        }
        /// <summary>
        /// путь к схеме для 
        /// преобразования дерева
        /// курса
        /// </summary>
        public string ContentShemePath
        {
            get { return baseParams == null ? null : baseParams.ContentShemePath; }
        }
        /// <summary>
        /// параметры изображения инструмента
        /// </summary>
        public ToolImgParam ToolImg
        {
            get { return baseParams == null ? null : baseParams.ToolImg; }
        }

        /// <summary>
        /// конвертировать видео в html5 формат
        /// </summary>
        public bool ConvertVideoToHTML5
        {
            get { return baseParams == null ? false : baseParams.ConvertVideoToHTML5; }
        }
        /// <summary>
        /// схема конвертации манифеста scorm
        /// </summary>
        public string ManifestSheme 
        {
            get { return baseParams == null ? null : baseParams.ManifestSheme; } 
        }
        /// <summary>
        /// получить параметры флэша
        /// </summary>
        public FlashSettings FlashParam { get; private set; }
        #endregion

        /// <summary>
        /// параметры 
        /// </summary>
        /// <param name="_xsltBaseParams">базовые параметры</param>
        public XSLTConvertParams(IXSLTConvertParam _xsltBaseParams)
        {
            if (_xsltBaseParams == null)
                throw new ArgumentNullException();

            baseParams = _xsltBaseParams;
            
        }

        /// <summary>
        /// параметры 
        /// </summary>       
        public XSLTConvertParams()
        {
           

        }

        /// <summary>
        /// обновить путь до файла шаблона конвертации
        /// </summary>
        /// <param name="_path">путь до файла шаблона конвертации</param>
        void UpdateTemplateFilePath(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                throw new ArgumentNullException("UpdateTamplateFilePath(string _path)");
            try 
            {
                FileInfo fi = new FileInfo(_path);
                if (!fi.Exists)
                    throw new Exception("Файл шаблона конвертации не найден "+_path);

                baseParams = new XSLTParamFromXML(_path);
                
            }
             catch(System.Security.SecurityException )
            {
                 throw new Exception("Ошибка доступа к файлу шаблона "+_path);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// обновить флаг формирования scorm пакета
        /// </summary>
        /// <param name="_val">значение</param>
        void UpdateScormFlag(bool _val)
        {
            IsToScorm = _val;
        }
        /// <summary>
        /// обновить выходной каталог
        /// </summary>
        /// <param name="_path">выходной каталог</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AppException">не верный каталог</exception>
        void UpdateOutputAbsPath(string _path)
        {
            if (_path == null)
                throw new ArgumentNullException();
            try
            { 
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(_path);
                if(!di.Exists)
                    throw new Exception("Каталог не создан: "+_path);

                OutputAbsPath=_path;
              
                PathUpdated(this,new EventArgs());
            }
            catch(Exception ex)
            { 
                throw new Exception("Выходной катклог не может быть обновлен: "+ex.Message);
            }
        }

        /// <summary>
        /// сообщение об ошибочном
        /// параметре
        /// </summary>
        public string ErrorMessage{get;private set;}

        /// <summary>
        /// корректны ли параметры
        /// </summary>
        public bool IsParamsValid
        {
            get 
            {
                if (baseParams == null)
                    throw new Exception("Не заданы параметры конветации из шаблона.");

                try
                {
                    FileNameValidator fv = new FileNameValidator(StartFileName, "Имя стартового файла");
                    fv.Validate();

                    DirectoryNameValidator dv=new DirectoryNameValidator(RootFolderName,"Имя корневой директории данных");
                    dv.Validate();

                    dv = new DirectoryNameValidator(CourseDataFolderName, "Имя папки медиа файлов курса");
                    dv.Validate();

                    dv = new DirectoryNameValidator(ToolsDataFolderName, "Имя папки инструментов курса");
                    dv.Validate();

                    DirectoryInfo di = new DirectoryInfo(TemplatePath);
                    if (!di.Exists)
                        throw new Exception("Директория шаблона не найдена " + TemplatePath);

                    FileInfo fi = new FileInfo(ThemeShemePath);
                    if(!fi.Exists)
                        throw new Exception("Шаблон преобразования темы курса не найден " + ThemeShemePath);

                    fi = new FileInfo(TestShemePath);
                    if (!fi.Exists)
                        throw new Exception("Шаблон преобразования теста курса не найден " + ThemeShemePath);

                    fi = new FileInfo(ContentShemePath);
                    if (!fi.Exists)
                        throw new Exception("Шаблон преобразования дерева курса не найден " + ContentShemePath);

                    if(!ToolImg.IsParamsValid)
                        throw new Exception("Не установлены параметры изображения инструмента.");

                    if (FlashParam == null)
                        throw new Exception("Не установлены параметры флэш.");
                    
                    if(!IsToScorm)
                        return true;

                    fi = new FileInfo(ManifestSheme);
                    if (!fi.Exists)
                        throw new Exception("Шаблон преобразования манифеста scorm не найден " + ManifestSheme);

                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;

                    return false;
                }
                

            }
        }
        /// <summary>
        /// утановить параметры флэш ролика
        /// </summary>
        /// <param name="_flSts">параметры флэш ролика</param>
        public void SetFlashParam(FlashSettings _flSts)
        {
            FlashParam = _flSts;
        }



        public event EventHandler<EventArgs> Updated;
        /// <summary>
        /// обновить параметры
        /// </summary>
        /// <param name="_param">параметры</param>
        public void Update(IConvertParam _param)
        {
            if (_param == null)
                throw new ArgumentNullException("Update(IConvertParam _param)");
            try 
            {
                UpdateTemplateFilePath(_param.TemplateFilePath);
                UpdateOutputAbsPath(_param.OutputAbsPath);
                UpdateScormFlag(_param.IsToScorm);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
