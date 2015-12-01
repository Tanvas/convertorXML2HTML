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
using Saxon.Api;

namespace CourceToWebUC.Model.xslt
{
    
    
    /// <summary>
    /// класс преобразования курса
    /// с помощью xslt
    /// </summary>
    internal class XSLTConvertor : XSLTConvertorBase
    {
        
      
        /// <summary>
        /// класс преобразования курса
        /// с помощью xslt
        /// </summary>
        /// <param name="_curse">курс для преобразования</param>
        /// <param name="_params">параметры преобразования</param>
        public XSLTConvertor(CourseForXSLT _course, XSLTConvertParams _params):base(_course,_params)
        {
                       
        }

        /// <summary>
        /// действия перед началом конвертации
        /// инициализация компонент
        /// </summary>       
        protected override void DoBefore()
        {
            convertCurse = course.AsXMLForConvert();

            convertOperations = new List<IOperationModel>();

            IEnumerable<XElement> themes = convertCurse.Descendants(NodeNames.THEME_ROOT);
            IOperationModel themeConv = new ThemeXSLTConvertor(themes, convParams,this);           
            convertOperations.Add(themeConv);

            IEnumerable<XElement> tests = convertCurse.Descendants(NodeNames.TEST_ROOT);
            IOperationModel testsConv = new TestXSLTConvertor(tests, convParams, this);
            convertOperations.Add(testsConv);

            IOperationModel themplate = new ThemeplateXSLTConvertor(convParams);
            convertOperations.Add(themplate);

            IOperationModel content = new ContentXSLTConvertor(convertCurse,convParams);
            convertOperations.Add(content);


            convertOperations.ForEach(x => x.Prepare());

        }
        
    }
}
