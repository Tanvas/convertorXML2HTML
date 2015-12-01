/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: CourseModel.cs
 * Создан: 27.02.2015
 * Редактирован: 27.02.2015
 */


using CourceToWebUC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// интерфейс
    /// пользовательского элемента
    /// курса
    /// </summary>
    public interface ICourseUC
    {
        /// <summary>
        /// установить курс
        /// </summary>
        /// <param name="_course">курс</param>
        /// <param name="_absPathToParamsFolder">путь до папки с параметрами</param>
        void SetCourse(ICourseModel _course, string _absPathToParamsFolder);
    }
}
