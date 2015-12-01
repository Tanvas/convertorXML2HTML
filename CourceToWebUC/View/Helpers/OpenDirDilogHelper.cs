/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: OpenDirDilogHelper.cs
 * Создан: 22.04.2015
 * Редактирован: 22.04.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace CourceToWebUC.View.Helpers
{

    /// <summary>
    /// класс диалога выбора директории
    /// </summary>
    class OpenDirDilogHelper:IOpenDirDialog
    {
        FolderBrowserDialog dialog;
      
        public OpenDirDilogHelper(string _startDir)
        {
            dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "\tДиректория должна быть пустой!\nДля создания новой директории нажмите кнопку \"Новая папка\" (\"Make New Folder\")";
            dialog.SelectedPath = _startDir;
            
           
        }
        /// <summary>
        /// выбранная директория
        /// </summary>
        public string SelectDir
        {
            get { return dialog.SelectedPath; }
        }

        /// <summary>
        /// открыть диалог выбора 
        /// директории
        /// </summary>
        /// <returns>true-директория выбрана</returns>
        public bool Open()
        {
            DialogResult res = dialog.ShowDialog();
            bool isSelect = res == DialogResult.OK ? true : false;

            return isSelect;
        }

        public bool PathValidate()
        {
            throw new NotImplementedException();
        }
    }
}
