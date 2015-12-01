using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.XSLTHelpers
{
    /// <summary>
    /// класс параметров флэша
    /// </summary>
    public class FlashSettings
    {
        public int WidthPx { get; private set; }
        public int HeightPx { get; private set; }
        /// <summary>      
        /// класс параметров флэша    
        /// </summary>
        /// <param name="wd">ширина ролика</param>
        /// <param name="hg">высота ролика</param>
        public FlashSettings(int wd,int hg)
        {
            WidthPx = wd;
            HeightPx = hg;
        }
    }
}
