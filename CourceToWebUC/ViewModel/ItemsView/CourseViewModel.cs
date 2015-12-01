/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: CourseViewModel.cs
 * Создан: 25.02.2015
 * Редактирован: 06.08.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentLib.Core.Content.Learning;
using System.Collections.ObjectModel;
using CourceToWebUC.Model;
using System.Windows.Input;
using System.Xml.XPath;
using CourceToWebUC.View.Helpers;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// модель-представление, 
    /// содержащие логику и состояние
    /// представления
    /// CourseView 
    /// </summary>
    public class CourseViewModel : ItemViewModel,ICourseViewModel
    {
        public const string CHECK_COURSE_PROPERRTY_NAME
            = "IsChecked";
        /// <summary>
        /// данные курса
        /// </summary>
        private readonly ICourseModel model;
        /// <summary>
        /// список выбранных элементов
        /// (однозначно)
        /// </summary>
        private List<Guid> checkedItems;
        /// <summary>
        /// событие завершения конвертации
        /// </summary>
        public event EventHandler OnConverted;
        /// <summary>
        /// событие смены этапов конвертации
        /// </summary>
        public event EventHandler<CommonConvertLib.StepMessager.StepChangeArg> StepChange;
        /// <summary>
        /// представление курса
        /// </summary>
        /// <param name="_model">курс</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CourseViewModel(ICourseModel _model)
            : base(_model, null)
        {
            model = _model;

            checkedItems = new List<Guid>();
           
            SetExpanded(this);
        }

       

      
        /// <summary>
        /// изменилось состояние 
        /// выделения элемента курса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ChangeCheckItem(object sender, EventArgs e)
        {
            try
            {
                if (sender==null)
                    throw new ArgumentNullException();
                 
                IItemViewModel checkedItem =(IItemViewModel)sender;

                if (checkedItem == null)
                    throw new Exception("Неидентифицированный объект курса " + sender.GetType());

                bool isChecked = checkedItem.IsChecked == false ? false : true;
               /*  model.SetIsConvertSelfOrChildren(checkedItem.Id, isChecked);*/

               if (checkedItem.IsChecked == false)
                   RemoveCheckedItem(checkedItem.Id);
                else
                   AddCheckedItem(checkedItem.Id);
             
                              
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// удалить элемент курса из
        /// списка выбранных элементов
        /// </summary>
        /// <param name="_id">идентификатор элемента курса</param>
        /// <exception cref="ArgumentNullException"></exception>
        private void RemoveCheckedItem(Guid _id)
        {
            if (_id == null)
                throw new ArgumentNullException();

            if (checkedItems.Contains(_id))
                checkedItems.Remove(_id);
        }

        /// <summary>
        /// Добавить элемент курса к
        /// списку выбранных элементов
        /// </summary>
        /// <param name="_id">идентификатор элемента курса</param>
        /// <exception cref="ArgumentNullException"></exception>
        private void AddCheckedItem(Guid _id)
        {
            if (_id == null)
                throw new ArgumentNullException();

            if (!checkedItems.Contains(_id))
                checkedItems.Add(_id);
            
        }

        /// <summary>
        /// установить "развернутость"
        /// узла
        /// </summary>
        private void SetExpanded(IItemViewModel itm)
        {
            if (itm.Children == null || itm.Children.Count<1)
                return;

            itm.Children[0].IsExpanded = true;
            foreach (IItemViewModel iiv in itm.Children[0].Children)
                SetExpanded(iiv);

            
        }



        /// <summary>
        /// конвертировать курс
        /// </summary>
        /// <param name="_param">параметры конвертации</param>
        public void Convert(IConvertParam _param)
        {
            if (_param == null)
                throw new ArgumentNullException();
            try
            {
                foreach(Guid id in checkedItems)
                {
                   
                    model.SetIsConvertSelfOrChildren(id, true);
                }
                model.StepChange += model_StepChange;
                
                model.Convert(_param);
                

                if (OnConverted != null)
                    OnConverted(this,new EventArgs());
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// перехватчик события смены 
        /// этапа конвертации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void model_StepChange(object sender, CommonConvertLib.StepMessager.StepChangeArg e)
        {
            if (StepChange != null)
                StepChange(this, e);
        }



        
    }
}

