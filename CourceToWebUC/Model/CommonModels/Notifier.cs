/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: Notifier.cs
 * Создан: 25.02.2015
 * Редактирован: 25.02.2015
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// реализует интерфейс INotifyPropertyChanged и используется в модели
    /// и модели-представлении для уведомлений об изменениях
    /// </summary>
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
