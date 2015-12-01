
using CommonConvertLib.Helpers;
using CourceToWebUC.Model.XSLTHelpers;
using Saxon.Api;
using SWFToHTML5;
using SWFToHTML5.Helpers;
using SWFToHTML5.ResourseHlp;
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
    /// xslt конвертор темы курса
    /// с преобразованием в HTML5
    /// </summary>
    class ThemeXSLTHTML5 : ThemeXSLTConvertor
    {
        /// <summary>
        /// текущая временная папка
        /// </summary>
        string currTempDir;
        /// <summary>
        /// видео ресурсы
        /// </summary>
        IResourse videoRes;
        /// <summary>
        /// операция конвертации тем курса
        /// </summary>
        /// <param name="_themes">список тем</param>
        /// <param name="_params">параметры конвертации</param>
        public ThemeXSLTHTML5(IEnumerable<XElement> _themes, XSLTConvertParams _params, IItemsAdapter _itemsAdapter):
            base(_themes,_params,_itemsAdapter)
        {
           
        }
      
        /// <summary>
        /// функция создана для классов-наследников
        /// (в частности для возможности конвертации
        /// видео файлов темы в другие форматы)
        /// </summary>
        /// <remarks>создание данной функции - не самый 
        /// лучший способ, обеспечения гибкости кода, но решающий
        /// поставленную в настоящий момент задачу и
        /// подпбные задачи в обозримом будущем</remarks>
        protected override void DoOtherOperation(ItemThemeForXSLT _theme)
        {
            if (_theme == null)
                    throw new ArgumentNullException("CopyThemeFiles(ItemThemeForXSLT _theme)");
            try 
            { 
                
                string mediaDirectory = Path.Combine(convParams.OutMediaFiles.AsPath(), _theme.FileName).AsPath();
                currTempDir = System.IO.Path.Combine(mediaDirectory, "temp");

                ConvertSWF(mediaDirectory);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                DirectoryHelper.DeleteDirectory(currTempDir);
            }
            
        
        }

        /// <summary>
        /// конвертировать видео 
        /// в формате swf
        /// </summary>
        /// <param name="_inputDir">исходная директория с файлами</param>
        private void ConvertSWF(string _inputDir)
        {

            if (string.IsNullOrEmpty(_inputDir))
                throw new ArgumentNullException("ConvertSWF(string _inputDir)");

            FlashSettings courseFl = convParams.FlashParam;

            FlashConvertSettings flset = new FlashConvertSettings(courseFl.WidthPx, courseFl.HeightPx, currTempDir);
            ConvertorSTH5 conv = new ConvertorSTH5(flset);
            conv.StepChange += conv_StepChange;
            videoRes = new SWFResourser(_inputDir);
            foreach (string swf in videoRes)
            {
                conv.SetInputSWF(swf);
                string outPath = System.IO.Path.GetDirectoryName(swf);
                conv.Convert(outPath);
            }
        }

        /// <summary>
        /// обработчик события смены
        /// этапа конвертации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conv_StepChange(object sender, CommonConvertLib.StepMessager.StepChangeArg e)
        {
            SendMessage(e.StepDiscription);
        }
       
    }
}
