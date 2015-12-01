using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.AppException
{
    /// <summary>
    /// класс исключений
    /// для записи в ЛОГ
    /// </summary>
    /// <remarks>при возникновении ошибки 
    /// в процессе записи в лог файл информации 
    /// исключение не генерится!!!!</remarks>
    public class LogException
    {
        /// <summary>
        /// инициирован ли класс
        /// </summary>
        static bool isInit=false;
        /// <summary>
        /// генерировать ли исключения
        /// </summary>
        static bool isThrowException;
        /// <summary>
        /// абсолютный путь к файлу лога
        /// </summary>
        static string absLogPath = "";
        
        /// <summary>
        /// инициализировать лог-исключение
        /// </summary>
        /// <param name="_isThrow">флаг генерации исключения</param>
        /// <returns>успех установки флага</returns>
        /// <remarks>инициализация происходит один раз
        /// за все время работы приложения</remarks>
        public static bool Init(bool _isThrow,string _absLogPath)
        {
            if (isInit)
                return false;

            isInit=true;           
            isThrowException = _isThrow;
            absLogPath=_absLogPath;
            return isInit;
        }
      
        /// <summary>
        /// установить исключение лога
        /// </summary>
        /// <param name="_message">текст исключения</param>
        /// <remarks>ошибка при записи/создании в лог файл не генерится!!!!</remarks>
        public void SetLogException(string _message)
        {
            if (isThrowException)
                throw new Exception(_message);
            try
            {
                if (!System.IO.File.Exists(absLogPath))
                    System.IO.File.Create(absLogPath);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(absLogPath, true))
                {
                    file.WriteLine("\n >>>"+_message);
                }
            }
            catch
            {
                ///Исключение не генериться!!!!!!
            }

        }

    }
}
