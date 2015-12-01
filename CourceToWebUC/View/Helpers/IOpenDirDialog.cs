using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.View.Helpers
{
    /// <summary>
    /// интерфейс диалогового окна 
    /// выбора директории
    /// </summary>
    internal interface IOpenDirDialog
    {
        string SelectDir
        {
            get;

        }


        bool Open();


        bool PathValidate();
    }
}
