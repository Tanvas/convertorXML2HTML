/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IOpenFileDialog.cs
 * Создан: 24.04.2015
 * Редактирован: 24.04.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.View.Helpers
{
    /// <summary>
    /// интерфейс открытия 
    /// диалогового окна 
    /// выбора файла
    /// </summary>
    interface IOpenFileDialog
    {
        /// <summary>
        /// выбранный файл
        /// </summary>
        string SelectFile
        {
            get;

        }

        /// <summary>
        /// открыть диалоговое 
        /// окно
        /// </summary>
        /// <returns>флаг выбора файла</returns>
        bool Open();

      
        
    }
}
