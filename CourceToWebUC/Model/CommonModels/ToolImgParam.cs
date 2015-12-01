using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.CommonModels
{
    /// <summary>
    /// параметры визуализации
    /// инструментов
    /// </summary>
    [Serializable]
    public class ToolImgParam:IParamValidator
    {
        /// <summary>
        /// ширина 
        /// </summary>
        string widthStr;
        /// <summary>
        /// ширина в пикселях
        /// (строковый формат)
        /// </summary>
        public string WidthPxStr { get; set; }
       
        /// <summary>
        /// установлены ли параметры
        /// </summary>
        bool isSetParams=false;
        /// <summary>
        /// высота 
        /// </summary>
        string heightStr;
        /// <summary>
        /// высота в пикселях
        /// (строковый формат)
        /// </summary>
        public string HeightPxStr { get; set; }
        /// <summary>
        /// ширина
        /// </summary>
        public int Width
        {
            get
            {
                int wd;
                bool isInt = Int32.TryParse(widthStr, out wd);
                if (isInt)
                    return wd;
                return 0;
            }
        }
        /// <summary>
        /// высота
        /// </summary>
        public int Height
        {
            get
            {
                int hg;
                bool isInt = Int32.TryParse(heightStr, out hg);
                if (isInt)
                    return hg;
                return 0;
            }
        }

       
        /// <summary>
        /// зафиксировать параметры
        /// (для избежания дальнейших изменений) 
        /// </summary>
        public void FixParams()
        {
            if (isSetParams)
                return;

            isSetParams = true;
            widthStr = WidthPxStr;
            heightStr = HeightPxStr;
        }
        /// <summary>
        /// корректны ли параметры
        /// </summary>
        public bool IsParamsValid
        {
	        get 
            {
                if (Width > 0 && Height > 0)
                    return true;

                return false;
            }
        }

    }
}
