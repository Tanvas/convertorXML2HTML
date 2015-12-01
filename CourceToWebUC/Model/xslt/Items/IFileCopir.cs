using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt.Items
{
    /// <summary>
    /// интерфейс копирования файлов
    /// </summary>
    interface IFileCopir
    {
        /// <summary>
        /// копирование файлов
        /// </summary>
        /// <param name="otputPath">выходная папка</param>
        void Copy(string otputPath);
    }
}
