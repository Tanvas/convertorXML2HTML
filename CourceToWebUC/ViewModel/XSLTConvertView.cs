/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: UpdatePathCommand.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */

using CourceToWebUC.Model;
using CourceToWebUC.Model.xslt;
using CourceToWebUC.View.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// класс конвертации курса 
    /// xslt
    /// </summary>
    class XSLTConvertView :Notifier, IConvertView
    {
        public const string CHANGE_PARAMS_PROPERRTY_NAME
           = "IsChangeParams";
        /// <summary>
        /// команда конвертации
        /// </summary>
        public readonly ICommand СonvertCommand;
        /// <summary>
        /// процесс конвертации
        /// </summary>
        Thread converting;
        /// <summary>
        /// представление курса
        /// </summary>
        ICourseViewModel course;
        /// <summary>
        /// окно состояния процесса
        /// конвертации
        /// </summary>
        WndProgressBar windowProgressBar;
        /// <summary>
        /// прервана ли конвертация
        /// пользователем
        /// </summary>
        bool isCancelConv = false;
        /// <summary>
        /// представление параметров конвертации
        /// </summary>
        XSLTParamsView convParams;
        /// <summary>
        /// конвертация в данный момент
        /// </summary>
        bool isConvertNow = false;
        /// <summary>
        /// возможна ли конвертация курса
        /// </summary>
        public bool CanConvert 
        {
            get 
            { 
                if(course.IsChecked != false && convParams.IsParamsValid && !isConvertNow)
                    return true;

                return false; 
            } 
        
        }
        /// <summary>
        /// класс конвертации курса 
        /// xslt
        /// </summary>
        /// <param name="_course">представление курса</param>
        /// <param name="_params">xslt параметры</param>
        /// <exception cref="ArgumentNullException"></exception>
        public XSLTConvertView(ICourseViewModel _course,XSLTParamsView _params)
        {
            if (_course == null || _params == null)
                throw new ArgumentNullException();

            course = _course;
            convParams = _params;

            
            course.CheckedItem += ChangeProperty;
            convParams.Updated += ChangeProperty;
            СonvertCommand = new ConvertCommand(this);
        }

       

      

        

        void ChangeProperty(object sender, EventArgs e)
        {
            NotifyPropertyChanged(XSLTConvertView.CHANGE_PARAMS_PROPERRTY_NAME);
        }
        
        /// <summary>
        /// конвертировать курс
        /// </summary>
        public void Convert()
        {
            try
            {
                convParams.Update();
               
            if (course.IsChecked == false)
                throw new Exception("Не выбран элемент курса для конвертации");
            isCancelConv = false;
            windowProgressBar = new WndProgressBar();

            windowProgressBar.OnProcessEnd += WindowProgressBar_ProcessEnd;
            windowProgressBar.OnCancel += WindowProgressBar_OnCancel;

            course.OnConverted += course_OnConverted;
            course.StepChange += course_StepChange;
            ThreadStart worker = new ThreadStart(DoConvert);
            converting = new Thread(worker);
            
          
            converting.Start();
            isConvertNow = true;
            NotifyPropertyChanged(XSLTConvertView.CHANGE_PARAMS_PROPERRTY_NAME); 
           
            windowProgressBar.Show();
           
            
            
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Ошибка конвертации", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                
            }

        }

        /// <summary>
        /// обработчик события смены 
        /// этапов конвертации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void course_StepChange(object sender, CommonConvertLib.StepMessager.StepChangeArg e)
        {
            windowProgressBar.SetMessage(e.StepDiscription);
        }

        /// <summary>
        /// нажатие кнопки отмена
        /// на форме прогрессбара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WindowProgressBar_OnCancel(object sender, EventArgs e)
        {
            if (converting.IsAlive)
            { 
               isCancelConv = true;
                converting.Abort();
             
            }
            isConvertNow = false;
            windowProgressBar.Close();
        }

        
        private void DoConvert()
        {
             try
            {
                
                course.Convert((XSLTConvertParams)convParams.GetParams());
            
            }           
             catch (Exception ex)
             {
                 if(isCancelConv)
                    System.Windows.MessageBox.Show("Конвертация прервана пользователем."); 
                 else
                 { 
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка конвертации", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    
                 }
             }
             
        }

        void EndConvertSucess()
        {
            NotifyPropertyChanged(XSLTConvertView.CHANGE_PARAMS_PROPERRTY_NAME);
            isConvertNow = false;
            System.Windows.MessageBox.Show("Конвертация успешно завершена");
            System.Diagnostics.Process.Start(convParams.OutputAbsPath);
        }
        /// <summary>
        /// окно состояния процесса 
        /// конвертации закрылось
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WindowProgressBar_ProcessEnd(object sender, EventArgs e)
        {   
            windowProgressBar.Close();
            EndConvertSucess();
         
        }


        /// <summary>
        /// конвертация завершена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void course_OnConverted(object sender, EventArgs e)
        {
            windowProgressBar.SetProcessComplete();
           
        }
       
    }
}
