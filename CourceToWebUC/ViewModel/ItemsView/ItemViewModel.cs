/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemViewModel.cs
 * Создан: 25.02.2015
 * Редактирован: 02.03.2015
 */

using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace CourceToWebUC.ViewModel
{
    /// <summary>
    /// класс представления единицы курса
    /// </summary>
    public class ItemViewModel : Notifier, IItemViewModel
    {
        public Guid Id
        {
            get;
            private set;
        }
        /// <summary>
        /// событие смены выделения 
        /// элемента курса
        /// </summary>
        public event EventHandler CheckedItem = delegate { };
        /// <summary>
        /// выбор элемента
        /// </summary>
        private bool? isChecked = false;
        /// <summary>
        /// определяет
        /// стартовую развернутость узла
        /// </summary>
        public bool IsExpanded { get; set; }
        /// <summary>
        /// название элемента
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// родительский элемент
        /// </summary>
        public IItemViewModel Parent { get; private set; }
        public bool IsInitiallySelected { get; private set; }
     
        /// <summary>
        /// список дочерних элементов
        /// элемента курса
        /// </summary>
        public List<IItemViewModel> Children { get; private set; }
        
        /// <summary>
        /// класс представления единицы курса
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemViewModel(IItemModel _item, IItemViewModel _parent)
        {
            if (_item == null)
                throw new ArgumentNullException();

            Id = _item.Id;
           
            Name = _item.Title;

            Parent = _parent;

            InitChildren(_item);

            CacherChangeChildProperty();
            
        }

        /// <summary>
        /// перехват события выделения 
        /// от потомков
        /// </summary>
        private void CacherChangeChildProperty()
        {
            if (this.Children == null)
                return;
            foreach (IItemViewModel iiv in this.Children)
            {
                iiv.CheckedItem += ChangeCheckItem;
            }
        }

        /// <summary>
        /// обработчик события выделения
        /// дочернего узла
        /// </summary>
        /// <param name="sender">узел</param>
        /// <param name="e">параметр события</param>
        /// <remarks>метод генерит событие
        /// для передачи информации о выделенном узле
        /// "наверх"</remarks>
        protected virtual void ChangeCheckItem(object sender, EventArgs e)
        {
            this.CheckedItem(sender, e);
        }

       

        

        /// <summary>
        /// инициализировать представления
        /// дочерних элементов
        /// </summary>
        private void InitChildren(IItemModel _item)
        {
              Children = new List<IItemViewModel>();
             
            if (_item.Items == null)
                return;

           
            IItemViewModel tmp = null;
            foreach(IItemModel im in _item.Items)
            {
                tmp = new ItemViewModel(im,this);
                Children.Add(tmp);
            }
            
            IsInitiallySelected = true;
        }
       
        /// <summary>
        /// выбрана ли единица курса
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
               
                 SetIsChecked(value, true, true); 
            }
        }

        /// <summary>
        /// определяет выбор
        /// элемента
        /// </summary>
        /// <param name="value">значение текущего элемента</param>
        /// <param name="updateChildren">обновление потомков</param>
        /// <param name="updateParent">обновление родительских элементов</param>
        public void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == isChecked)
                return;

            isChecked = value;
         


            NotifyPropertyChanged(CourseViewModel.CHECK_COURSE_PROPERRTY_NAME);
            CheckedItem(this, new EventArgs());

            if (updateChildren && isChecked.HasValue)
                Children.ForEach(c => c.SetIsChecked(isChecked, true, false));

            

            if (updateParent && Parent != null)
                Parent.VerifyCheckState();

            
          
        }

        public void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }








    }
}
