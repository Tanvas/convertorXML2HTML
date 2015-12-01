/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: PathTryValidation.cs
 * Создан: 05.03.2015
 * Редактирован: 05.03.2015
 */

using System;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Security.AccessControl;

namespace CourceToWebUC.View.Validations
{
    /// <summary>
    /// проверка верности пути к каталогу
    /// </summary>
    public class FolderTryValidation : ValidationRule
    {
       

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
           
           
           
            try
            {
                string str = (string)value;

                if (string.IsNullOrEmpty(str))
                {
                    return new ValidationResult(false, "Необходимо указать путь к выходному каталогу");
                }

                str = str.Trim();

                if (string.IsNullOrEmpty(str))
                {
                    return new ValidationResult(false, "Необходимо указать путь к выходному каталогу");
                }

                DirectoryInfo di = new DirectoryInfo(str);

                if(!di.Exists)
                    return new ValidationResult(false, "Не найден каталог");

                if(di.GetDirectories().Length>0 || di.GetFiles().Length>0)
                    return new ValidationResult(false, "Выбранный каталог должен быть пустым");

                 UserFileAccessRights rights = new UserFileAccessRights(str);
                if (!rights.canWrite() ) 
                {
                    return new ValidationResult(false, "У вас нет прав доступа на запись в каталог");
                } 
                if (rights.canRead())
                {
                    return new ValidationResult(true, "У вас нет прав доступа на чтение каталога");
                }       
        
            
            }
            catch (ArgumentException)
            {
                return new ValidationResult(false, "Не верно задан путь к каталогу");
            }

            return new ValidationResult(true, null);

        }
    }
}
