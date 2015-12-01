/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: XSLTParamsView.cs
 * Создан: 03.03.2015
 * Редактирован: 03.03.2015
 */

using CourceToWebUC.Model;
using CourceToWebUC.Model.xslt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// представление параметров
    /// конвертации курса
    /// </summary>
    class XSLTParamsView:Notifier,IParamsView
    {
        /// <summary>
        /// константа, определяющая изменение
        /// выходного кталога
        /// </summary>
        public const string CHANGE_PATH_PROPERRTY_NAME
           = "ChangeParams";
       
        /// <summary>
        /// файл шаблона
        /// </summary>
        private string templatePath;

        /// <summary>
        /// файл шаблона
        /// </summary>
        public string TemplateFilePath
        {
            get { return templatePath; }
            set
            {
                templatePath = value;
                // outputPath = outputPath.AsPath();
                NotifyPropertyChanged(CHANGE_PATH_PROPERRTY_NAME);
                Updated(this, new EventArgs());
            }
        }
        /// <summary>
        /// формировать scorm пакет
        /// </summary>
        bool isScorm;
        /// <summary>
        /// формировать scorm пакет
        /// </summary>
        public bool IsToScorm
        {
            get { return isScorm; }
            set
            {
                isScorm = value;
                // outputPath = outputPath.AsPath();
                NotifyPropertyChanged(CHANGE_PATH_PROPERRTY_NAME);
                Updated(this, new EventArgs());
            }
        }
        /// <summary>
        /// выходной каталог 
        /// </summary>
        string outputPath;
        /// <summary>
        /// выходной каталог 
        /// </summary>
        public string OutputAbsPath
        {
            get {   return outputPath; }
            set 
            {   
                outputPath = value;
               // outputPath = outputPath.AsPath();
                NotifyPropertyChanged(CHANGE_PATH_PROPERRTY_NAME);
                Updated(this, new EventArgs());
            }
        }


        public event EventHandler<EventArgs> Updated = delegate { };

        /// <summary>
        /// параметры конвертации
        /// </summary>
        XSLTConvertParams convertParams;

        /// <summary>
        /// корректны ли параметры
        /// </summary>
        public bool IsParamsValid 
        { 
            get
            {
                if(string.IsNullOrEmpty(templatePath))
                {
                    ErrorMessage = "Не задан файл шаблона";
                    return false;
                }
                if (string.IsNullOrEmpty(outputPath))
                {
                    ErrorMessage = "Не задан выходной каталог";
                    return false;
                }
                
                try
                {
                    if(!System.IO.File.Exists(templatePath))
                    {
                        ErrorMessage = "Не найден файл шаблона";
                        return false;
                    }
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(outputPath);
                    if (!di.Exists)
                    { 
                        ErrorMessage = "Не создан выходной каталог";
                        return false;
                    }
                    if (di.GetDirectories().Length > 0 || di.GetFiles().Length > 0)
                    { 
                        ErrorMessage = "Выходной каталог должен быть пустым";
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        
        }
        /// <summary>
        /// представление параметров
        /// конвертации курса
        /// </summary>
        /// <param name="_params">параметры конвертации</param>
        /// <exception cref="ArgumentNullException"></exception>
        public XSLTParamsView(XSLTConvertParams _params)
        {
            if (_params == null)
                throw new ArgumentNullException();

            convertParams = _params;
            outputPath = _params.OutputAbsPath;


        }
        /// <summary>
        /// обновить параметры
        /// </summary>
        public void Update()
        {
            try
            {
                CommonParams cp = new CommonParams(templatePath, outputPath, isScorm);
                convertParams.Update(cp);
               
               
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


      




        public IConvertParam GetParams()
        {
            return convertParams;
        }





        public string ErrorMessage
        {
            get;
            private set;
        }



        /// <summary>
        /// обновить параметры
        /// </summary>     
        /// <param name="_param">параметры</param>
        public void Update(IConvertParam _param)
        {
            if (_param == null)
                throw new ArgumentNullException("Update(IConvertParam _param)");

            templatePath = _param.TemplateFilePath;
            outputPath = _param.OutputAbsPath;
            isScorm = _param.IsToScorm;
        }
    }
}
