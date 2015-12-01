/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: DataServiceXSLTLib.cs
 * Создан: 25.02.2015
 * Редактирован: 25.02.2015
 */
using System;
using System.Collections.Generic;
using ContentLib.Core.Content.Learning;
using System.Collections.ObjectModel;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс получение структуры курса
    /// с использованием библиотеки
    /// CourseLib для последующего преобразования
    /// xslt
    /// </summary>

    public class DataServiceXSLTLib:IDataService
    {
        /// <summary>
        /// курс
        /// </summary>
        public Course Course { get; private set; }
        /// <summary>
        /// название курса
        /// </summary>
        string title;
        /// <summary>
        /// фабрика элементов курса
        /// </summary>
        XSLTItemsFactory itemsFactory;
        /// <summary>
        /// загружены ли части курса
        /// </summary>
        bool isItemsLoad = false;
        /// <summary>
        /// получить название курса
        /// </summary>
        /// <returns>название курса</returns>
        public string GetTitle()
        {
            return title;
        }

        /// <summary>
        /// список элементов курса
        /// </summary>
        IList<IItemModel> items; 

        /// <summary>
        /// получение структуры курса
        /// </summary>
        /// <param name="_cource">курс</param>
        /// <exception cref="ArgumentNullException">Не задан курс</exception>
        /// <exception cref="AppException">Неверные данные</exception>
        public DataServiceXSLTLib(Course _cource)
        {
            if (_cource == null)
                throw new ArgumentNullException("Загрузчик не может быть создан. Нет данных о курсе");

            Course = _cource;
            try
            {
                itemsFactory = new XSLTItemsFactory(_cource);
                Init();
            }
            catch (Exception ex)
            {
                throw new Exception("Загрузчик не может быть создан. " + ex.Message);
            }
            
        }

        /// <summary>
        /// инициализация курса
        /// </summary>
        /// <exception cref="AppException">
        /// Возникает в случае ошибки загрузки курса
        /// (возможно ранее не был указан путь)</exception>
        private void Init()
        {
            if (!Course.isLoaded)
            {
                try
                {
                    Course.LoadFromPath(CourseLoadType.Full);
                }
                catch(Exception ex)
                {
                    throw new Exception("Ошибка загрузки курса." +"/n"+ex.Message);
                }
                
            }

            title = Course.title;
        }

        /// <summary>
        /// получить список единиц курса
        /// </summary>
        /// <exception cref="AppException">
        /// Возникает в случае ошибки получения единиц курса
        /// (возможно ранее не был указан путь)</exception>
        public IList<IItemModel> GetItems()
        {
            try
            { 
                if (!isItemsLoad)
                    LoadCourseItems();
                
                return items;
            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.Message);
            }

            
        }

        /// <summary>
        /// загрузка единиц курса
        /// </summary>
        /// <exception cref="AppException">
        /// Возникает в случае ошибки загрузки единиц курса
        /// (возможно ранее не был указан путь)</exception>
        void LoadCourseItems()
        {
            try
            {
                IItemModel im = null;
                items = new ObservableCollection<IItemModel>();
                bool validCreating = false;

                foreach (AbstractLearningItem item in Course)
                { 
                    if ((item.isReferenced) && (!item.isLoaded))
                        item.LoadFromPath(CourseLoadType.Full);
                }
                foreach (AbstractLearningItem item in Course.childs)
                {
                    im = itemsFactory.Create(item, out validCreating);

                    if (!validCreating)
                        throw new Exception(item.identifier+": неопределенный тип элемента курса: " + item.GetType());
                    if(im!=null)
                        items.Add(im);
                }

                isItemsLoad = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка загрузки единиц курса."+"/n"+ex.Message);
            }

        }



    }
}
