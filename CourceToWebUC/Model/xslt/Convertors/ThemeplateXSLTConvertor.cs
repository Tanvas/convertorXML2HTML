using CommonConvertLib.Helpers;
using CommonConvertLib.StepMessager;
using CourceToWebUC.Model.Helpers;
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
    /// xslt конвертор шаблона
    /// </summary>
    class ThemeplateXSLTConvertor : StepChangeMessager, IOperationModel
    {
        
        /// <summary>
        /// параметры конвертации
        /// </summary>
        XSLTConvertParams convParams;
        /// <summary>
        /// посленяя ошибка
        /// 
        /// </summary>
        public string LastError { get; private set; }
        /// <summary>
        /// операция конвертации структуры курса
        /// </summary>
        /// <param name="_cource">курс</param>        
        public ThemeplateXSLTConvertor( XSLTConvertParams _params)
        {
            
            convParams = _params;
            
        }
        /// <summary>
        /// подготовка к конвертации
        /// </summary>
        public void Prepare()
        {
            
        }
        /// <summary>
        /// выполнить копирование 
        /// папки с шаблоном
        /// (полное копирование папки с шаблоном)
        /// </summary>
        public void  Do()
        {
            try
            {
                SendMessage("\n- Копирование файлов шаблона");
                DirectoryInfo source = new DirectoryInfo(convParams.TemplatePath);

                string dirName = source.Name;
                dirName = dirName.AsPath();

                string newDir = Path.Combine(convParams.OutItemsPath, dirName);
                DirectoryHelper.CreateDirectory(newDir, true);

                DirectoryInfo newDirect = new DirectoryInfo(newDir);

                DirectoryHelper.CopyFiles(source, newDirect);

                
            }
            catch(Exception ex)
            {
                SendMessage("\n!!!!! Ошибка при копировании файлов шаблона курса: " + ex.Message + " Попробуйте скопировать вручную");
                
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
