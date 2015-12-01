/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemThemeForXSLT.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */


using CommonConvertLib.Helpers;
using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.CommonModels;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс темы для конвертации
    /// xslt
    /// </summary>
    internal class ItemThemeForXSLT : IItemModelForXSLT
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        public Guid Id { get { return item.identifier; } }   
        /// <summary>
        /// переменная для формирования уникальных
        /// имен в пределах коллекции тем
        /// </summary>
        static int number = 0;
        /// <summary>
        /// уникальный идентификатор
        ///  в пределах коллекции тем
        /// </summary>
        public int IdInTheme { get; private set; }

        /// <summary>
        /// конвертировать ли 
        /// элемент курса
        /// </summary>
        bool _isConv = false;
        /// <summary>
        /// конвертировать ли 
        /// элемент курса
        /// </summary>
        public bool IsConvert
        {
            get { return _isConv; }

        }
        /// <summary>
        /// дочерние элементы отсутствуют
        /// (только кадры)
        /// </summary>
        public IList<IItemModel> Items
        {
            get { return null; }
        }
        /// <summary>
        /// тема курса
        /// </summary>
        LearningItem item;
        /// <summary>
        /// список кадров темы
        /// </summary>
        IList<ISubItemForXSLT> steps;
        /// <summary>
        /// название элемента
        /// </summary>
        public string Title
        {
            get { return item.title; }
        }
        

        /// <summary>
        /// имя файла элемента
        /// без расширения
        /// </summary>
        public string FileName { get; private set; }
        
        /// <summary>
        /// тип элемента курса
        /// </summary>
        public ItemType Type { get { return ItemType.Theme; } }
        /// <summary>
        /// класс темы для конвертации
        /// xslt 
        /// </summary>
        /// <param name="_item">тема курса</param>
        /// <param name="_tlFactory">фабрика инструментов</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemThemeForXSLT(LearningItem _item,ToolsFactory _tlFactory ) 
        {
            if (_item == null || _tlFactory==null)
                throw new ArgumentNullException();

            item = _item;
            if (item.childs == null)
                throw new ArgumentNullException("Отсутствуют кадры в теме "+item.identifier);

            number++;
            IdInTheme = number;

           

            FileName = "theme" + IdInTheme;
            initSteps(_tlFactory);
            
        }

        /// <summary>
        /// инициализировать кадры
        /// темы
        /// </summary>
        private void initSteps(ToolsFactory _tlFactory)
        {
            try
            {
                string pathTheme = System.IO.Path.Combine(item.rootDirectory.AsPath()
                                                        , item.pathDirectory.AsPath());
                steps = new ObservableCollection<ISubItemForXSLT>();
                ItemStepModel tmpStep = null;
                string path = pathTheme.AsPath();
                ItemStepForXSLT.ResetIdInStep();
                foreach (LearningStep ls in item.learningSteps)
                {
                    tmpStep = new ItemStepModel(ls, path);
                    tmpStep.InitTools(_tlFactory);
                    steps.Add(new ItemStepForXSLT(tmpStep));
                }
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Отсутствуют кадры темы: " + item.identifier);
            }
            catch
            {
                //throw new Exception("Невозможно создать коллекцию кадров темы: " + item.identifier);
            }            
        }





        /// <summary>
        /// копирование необходимых медиафайлов
        /// </summary>
        /// <param name="_outputFolder">папка для медиафайлов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для медиафайлов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов темы</exception>
        public void CopyFiles(string _outputFolder)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для медиафайлов");
            try
            {
                string stepsPath = _outputFolder.AsPath();
                DirectoryHelper.CreateDirectory(_outputFolder, false);
                
                foreach (ItemStepForXSLT st in steps)
                    st.CopyFiles(stepsPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов темы " + this.Title + "." + ex.Message);
            }
        }
        /// <summary>
        /// копирование необходимых инструментов
        /// </summary>
        /// <param name="_outputFolder">папка для инструментов</param>
        /// <param name="_toolParams">параметры изображения инструментов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для инструментов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов инструментов</exception>
        public void CopyToolsFiles(string _outputFolder,ToolImgParam _toolParams)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для инструментов");
            if (_toolParams==null)
                throw new ArgumentNullException("Не заданы параметры изображения инструментов");
            try
            {
                string toolsPath = _outputFolder.AsPath();
                DirectoryHelper.CreateDirectory(_outputFolder, false);

                foreach (ItemStepForXSLT st in steps)
                    st.CopyToolsFiles(toolsPath, _toolParams);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов инструментов темы " + this.Title + "." + ex.Message);
            }
        }

        /// <summary>
        /// получить элемент курса 
        /// как xml данные
        /// </summary>
        /// <returns>xml представление элемента</returns>
        public XElement AsXMLForConvert()
        {
            XElement theme = 
             new XElement(NodeNames.THEME_ROOT,
                 new XElement(NodeNames.IDENTIFER, Id),
                 new XElement(NodeNames.TITLE, Title),
                 new XElement(NodeNames.ITEM_FILE_ID,FileName),            
                 new XElement(NodeNames.STEPS_ROOT,
                        from el in steps
                        select new XElement(el.AsXMLForConvert())
                            )
                       
               );


          
            return theme;
            
        }

        /// <summary>
        /// получить элемент
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <returns>элемент</returns>
        /// <remarks>всегда возвращает null</remarks>
        public IItemModel GetItemInChildren(Guid _id)
        {
            
            return null;
        }
        /// <summary>
        /// установить флаг конвертации
        /// себе или дочерниму элементу
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <param name="_isConvert">значение флага</param>
        /// <returns>успешность установки</returns>
        public bool SetIsConvertSelfOrChildren(Guid _id, bool _isConvert)
        {
            if (Id == _id)
            {
                _isConv = _isConvert;
                return true;
            }
            
            return false;
        }
        
    }
}
