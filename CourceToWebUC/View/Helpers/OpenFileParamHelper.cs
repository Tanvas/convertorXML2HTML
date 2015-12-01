/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: OpenFileParamHelper.cs
 * Создан: 24.04.2015
 * Редактирован: 24.04.2015
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.View.Helpers
{
    /// <summary>
    /// класс, предоставляющий
    /// выбор файла
    /// </summary>
    class OpenFileParamHelper:IOpenFileDialog
    {
       
        OpenFileDialog dialog;
        bool tryChoose;

        public string SelectFile
        {
            get;
            private set;
        }
        /// <summary>
        /// класс, предоставляющий
        /// выбор файла
        /// </summary>
        public OpenFileParamHelper(string _fileSelect)
        {
           
            dialog = new OpenFileDialog();
            dialog.FileOk += new System.ComponentModel.CancelEventHandler(_dialog_FileOk);
            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            SelectFile = string.Empty;

            SetUpDialog();

        }

        void _dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!dialog.FileName.EndsWith(".xml"))
            {
                tryChoose = false;
            }
        }

        private void SetUpDialog()
        {
 	        dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            
        }

       

        public bool Open()
        {
            tryChoose = true;
            bool isOk =dialog.ShowDialog()==true ? true:false;

            if (!tryChoose)
                throw new Exception("Выбранный файл не является файлом шаблона конвертации!");

            if (isOk == true)
            {
                SelectFile = dialog.FileName;                
                return true;
            }
            return false;
        }

       

    }
}
