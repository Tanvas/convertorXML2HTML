/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: FileNameValidator.cs
 * Создан: 10.03.2015
 * Редактирован: 10.03.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CourceToWebUC.Model.Helpers
{
    /// <summary>
    /// класс проверки соответствия
    /// строки названию файла
    /// </summary>
    class FileNameValidator
    {
        /// <summary>
        /// исходная строка
        /// </summary>
        readonly string baseString;
        /// <summary>
        ///название параметра
        /// <summary>
        readonly string name;       
 
        /// <summary>
        /// класс проверки соответствия
        /// строки названию файла
        /// </summary>
        /// <param name="_str">исходная строка</param>
        public FileNameValidator(string _str,string _name)
        {
            baseString = _str;
            name = _name;
        }

        /// <summary>
        /// проверить название
        /// файла на корректность
        /// (построена на выбросе исключений
        /// в случае ошибки)
        /// </summary>
        /// <exception cref="AppException">Содержит описание ошибки</exception>
        public void Validate()
        {
            
            if(string.IsNullOrEmpty(baseString))
            {
           
                throw new Exception("Параметр "+name+" не задан");
            }

            if (baseString.IndexOf(".") < 1)
            {
                throw new Exception("Не задано расширение файла " + name);

            }

            char[] ns=System.IO.Path.GetInvalidFileNameChars();

                IEnumerable<char> stringQuery =
                    from ch in baseString
                        from x in ns
                        where x==ch
                    select ch;

            if(stringQuery==null)
               return;

            char[] resArr =stringQuery.ToArray();
            if(resArr.Length>0)                
                throw new Exception("Параметр " + name + " содержит запрещенные для имени файла символы");
        
        }


    }
}
