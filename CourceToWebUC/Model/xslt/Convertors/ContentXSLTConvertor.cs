using CommonConvertLib.StepMessager;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// xslt конвертор структуры курса
    /// </summary>
    class ContentXSLTConvertor : StepChangeMessager,IOperationModel
    {

        /// <summary>
        /// курс для конвертации
        /// </summary>
        XElement course;
        /// <summary>
        /// параметры конвертации
        /// </summary>
        XSLTConvertParams convParams;
        /// <summary>
        /// последняя ошибка
        /// </summary>
        public string LastError { get; private set; }
       
        /// <summary>
        /// операция конвертации структуры курса
        /// </summary>
        /// <param name="_cource">курс</param>
        /// <param name="_params">параметры конвертации</param>
        public ContentXSLTConvertor(XElement _cource, XSLTConvertParams _params)
        {
            if (_cource == null)
                throw new ArgumentNullException("Нет курса для конвертации структуры");

            course = _cource;
            convParams = _params;
            
        }
        /// <summary>
        /// подготовка к конвертации
        /// </summary>
        public void Prepare()
        {
            
        }
        /// <summary>
        /// выполнить конвертацию структуры
        /// </summary>
        public void Do()
        {
            try
            {
                SendMessage("\n- Конвертация структуры курса");
                XDocument doc = new XDocument(course);
                doc.Document.Declaration = new XDeclaration("1.0", "utf-8", "true");
                XPathNavigator nv = doc.CreateNavigator();

                XslCompiledTransform xslt = new XslCompiledTransform();

                xslt.Load(convParams.ContentShemePath);

                XsltArgumentList xslArg = new XsltArgumentList();
                xslArg.AddParam("itemsPath", "", convParams.RootFolderName);

                string outFile = Path.Combine(convParams.OutputAbsPath, convParams.StartFileName);

                using (FileStream fs = new FileStream(outFile, FileMode.Create))
                {
                    xslt.Transform(nv, xslArg, fs);
                }
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                throw new Exception("Исключение при конвертации структуры курса: "+ex.Message);
            }
        }

       
        /// <summary>
        /// действия после конвертации
        /// </summary>
        public void PostStep()
        {
           
        }
        /// <summary>
        /// отмена конвертации
        /// </summary>
        /// <remarks>отмена реализована в
        /// общем классе конвертации курса (метод
        /// оставлен на будующее)</remarks>
        public bool Undo()
        {
            return false;
        }
    }
}
