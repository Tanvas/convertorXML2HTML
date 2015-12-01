using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// команда конвертации курса
    /// </summary>
   public class ConvertCommand:ICommand
    {
        const int ARE_EQUAL = 0;
       
        public event EventHandler CanExecuteChanged = delegate { };
        
       /// <summary>
       /// представление конвертора
       /// </summary>
        IConvertView convertor;
        /// <summary>
        /// команда конвертации курса
        /// </summary>
        /// <param name="_viewModel">представление курса</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConvertCommand(IConvertView _convertor)
        {
            if (_convertor == null)
                throw new ArgumentNullException();

            convertor = _convertor;
            convertor.PropertyChanged += vm_PropertyChanged;
            
        }

        
        private void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           // System.Windows.MessageBox.Show("GHJKKK " + e.PropertyName);
            if (string.Compare(e.PropertyName, XSLTConvertView.CHANGE_PARAMS_PROPERRTY_NAME) == ARE_EQUAL)
            { 
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public bool CanExecute(object parameter)
        {
            return (convertor.CanConvert);
              
        }

       
        public void Execute(object parameter)
        {
            convertor.Convert();
        }
       

    }
}
