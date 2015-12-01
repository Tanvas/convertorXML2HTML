using ContentLib.Core.Content.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CourceToWebUC.Model
{
    /// <summary>
    /// расширения элементов курса
    /// </summary>
    public static class PathExtensions
    {
        static char slash = System.IO.Path.DirectorySeparatorChar;
        /// <summary>
        /// возвращает строку как 
        /// путь (добавляет слэш)
        /// </summary>
        /// <param name="self">строка</param>
        /// <returns>строка как путь или пустая строка</returns>
        public static string AsPath(this string self)
        {
            try
            {

                char end = self[self.Length - 1];
                char slash = System.IO.Path.DirectorySeparatorChar;
                if (end != slash)
                    self += slash;

                return self;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// определить эквивалентность расширению
        /// </summary>
        /// <param name="_fileName">имя файла</param>
        /// <param name="_extention">расширение</param>
        /// <returns></returns>
        public static bool IsEquivExtention(this string self, string _extention)
        {
            try
            {
                _extention = _extention[0] == '.' ? _extention : '.' + _extention;
                string fext = System.IO.Path.GetExtension(self).ToLower();
                if (string.Compare(fext, _extention.ToLower()) == 0)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
