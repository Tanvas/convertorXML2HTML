/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: IOperationModel.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */

using CommonConvertLib.StepMessager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// интерфейс действия    
    /// </summary>
    public interface IOperationModel : IStepMessager
    {
        /// <summary>
        /// информация о последней ошибке
        /// </summary>
        string LastError { get; }
        /// <summary>
        /// выполнить подготовительные 
        /// действия
        /// </summary>
        void Prepare();
        /// <summary>
        /// выполнить дествия
        /// </summary>       
        void Do();
        /// <summary>
        /// завершающие действия
        /// </summary>
        void PostStep();
        /// <summary>
        /// отменить операцию
        /// </summary>
        bool Undo();
    }
}
