using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// фабрика конверторов
    /// </summary>
    class ConvertorFactory
    {
        /// <summary>
        /// создать конвертор
        /// </summary>
        /// <param name="_params">параметры конвертации</param>
        /// <param name="_course">курс для конвертации</param>
        /// <returns>конвертор</returns>
        public static IOperationModel Create(CourseForXSLT _course, XSLTConvertParams _params)
        {
            if (_params == null || _course==null)
                throw new ArgumentNullException("Create(CourseForXSLT _course, XSLTConvertParams _params)");
            IOperationModel conv;

            bool isHtml5 = _params.ConvertVideoToHTML5;
            if (isHtml5)
                conv = new XSLTConvertorHTML5(_course, _params);
            else
                conv = new XSLTConvertor(_course, _params);

            return conv;
        }
    }
}
