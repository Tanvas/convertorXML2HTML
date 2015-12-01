using CourceToWebUC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourceToWebUC.View.Helpers
{
    /// <summary>
    /// диалоговое окно процесса 
    /// </summary>
    public partial class WndProgressBar : Window
    {
        
        /// <summary>
        /// событие нажатия кнопки Отмена
        /// </summary>
        public event EventHandler OnCancel;
        /// <summary>
        /// событие завершения процесса
        /// </summary>
        public event EventHandler OnProcessEnd;
        /// <summary>
        /// окно с пргогресс баром
        /// </summary>
        public WndProgressBar()
        {
            InitializeComponent();
          
        }

        
        /// <summary>
        /// кнопка отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancel != null)
                OnCancel(sender, e);
            
        }
        /// <summary>
        /// сообщить о завершении процесса
        /// </summary>
        public void SetProcessComplete()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (OnProcessEnd != null)
                    OnProcessEnd(this, new EventArgs());
            }));
        }

        /// <summary>
        /// сообщение о состоянии конвертации
        /// </summary>
        public void SetMessage(string _mes)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.tbStatus.Text += _mes;

            }));
        }
    }
}
