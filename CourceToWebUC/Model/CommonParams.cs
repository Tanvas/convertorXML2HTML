using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    class CommonParams:IConvertParam
    {
        /// <summary>
        /// файл шаблона
        /// </summary>
        public string TemplateFilePath{get;private set;}

        /// <summary>
        /// выходной каталог 
        /// </summary>
        public string OutputAbsPath { get; private set; }
        /// <summary>
        /// сформировать в конце Scorm пакет
        /// </summary>
        public bool IsToScorm{ get; private set; }
     

        public event EventHandler<EventArgs> Updated;
        /// <summary>
        /// общие параметры конвертации
        /// </summary>
        /// <param name="_templatePath">путь к файлу шаблона</param>
        /// <param name="_outPath">выходной путь к папке</param>
        /// <param name="_isScorm">создавать ли скорм пакет</param>
        public CommonParams(string _templatePath,string _outPath,bool _isScorm)
        {
            if (string.IsNullOrEmpty(_templatePath) || string.IsNullOrEmpty(_outPath))
                throw new ArgumentNullException("CommonParams(string _templatePath,string _outPath,bool _isScorm)");

            this.TemplateFilePath = _templatePath;
            this.OutputAbsPath = _outPath;
            this.IsToScorm = _isScorm;

        }
        /// <summary>
        /// обновить параметры
        /// </summary>     
        /// <param name="_param">параметры</param>
        public void Update(IConvertParam _param)
        {
            if (_param == null)
                throw new ArgumentNullException("Update(IConvertParam _param)");
            this.TemplateFilePath = _param.TemplateFilePath;
            this.OutputAbsPath = _param.OutputAbsPath;
            this.IsToScorm = _param.IsToScorm;
        }

        public bool IsParamsValid
        {
            get { return (string.IsNullOrEmpty(TemplateFilePath) && string.IsNullOrEmpty(OutputAbsPath)); }
        }

        public string ErrorMessage
        {
            get { return ""; }
        }
    }
}
