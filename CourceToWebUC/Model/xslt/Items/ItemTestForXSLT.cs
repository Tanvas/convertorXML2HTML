/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemTestForXSLT.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */


using CommonConvertLib.Helpers;
using ContentLib.Core.Content.Learning;
using ContentLib.Core.Content.Testing;
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
    /// класс теста для конвертации
    /// xslt
    /// </summary>
    public class ItemTestForXSLT: IItemModelForXSLT
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
        /// тест курса
        /// </summary>
        Test item;        
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
        /// список вопросов теста
        /// </summary>
        IList<ISubItemForXSLT> questions; 
        
        /// <summary>
        /// тип элемента курса
        /// </summary>
        public ItemType Type { get { return ItemType.Test; } }
        /// <summary>
        /// путь к xml файлу
        /// </summary>
        public string XMLFilePath { get; private set; }
        /// <summary>
        ///  класс теста для конвертации
        /// xslt
        /// </summary>
        /// <param name="_item">тест курса</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemTestForXSLT(Test _item) 
        {
            if (_item == null)
                throw new ArgumentNullException();

            item = _item;
            if (item.questions == null)
                throw new ArgumentNullException("Отсутствуют вопросы в тесте "+item.identifier);

            number++;
            IdInTheme = number;

            XMLFilePath = System.IO.Path.Combine(item.rootDirectory.AsPath(), item.path);

            FileName = "test" + IdInTheme;
            initQuestions();
            
        }

        /// <summary>
        /// инициализировать вопросы
        /// теста
        /// </summary>
        private void initQuestions()
        {
            try
            {
                string pathTest = System.IO.Path.Combine(item.rootDirectory.AsPath()
                                                        , item.pathDirectory.AsPath());
                questions = new ObservableCollection<ISubItemForXSLT>();
                
                string path = pathTest.AsPath();
                foreach (AbstractQuestion qs in item.questions)
                {
                    questions.Add(new ItemQestionForXSLT(qs, path));
                }
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Отсутствуют вопросы теста: " + item.identifier);
            }
            catch(Exception ex)
            {
                throw new Exception("Невозможно создать коллекцию вопросов теста: " + item.identifier+". "+ex.Message);
            }            
        }





        /// <summary>
        /// копирование необходимых медиафайлов
        /// </summary>
        /// <param name="_outputFolder">папка для медиафайлов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для медиафайлов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов теста</exception>
        public void CopyFiles(string _outputFolder)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для медиафайлов вопроса");
            try
            {
                string quesPath = _outputFolder.AsPath();
                DirectoryHelper.CreateDirectory(quesPath, false);
                
                foreach (ItemQestionForXSLT qt in questions)
                    qt.CopyFiles(quesPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов вопроса " + this.Id + "." + ex.Message);
            }
        }

        /// <summary>
        /// получить элемент курса 
        /// как xml данные
        /// </summary>
        /// <returns>xml представление элемента</returns>
        public XElement AsXMLForConvert()
        {
            XElement test = 
             new XElement(NodeNames.TEST_ROOT,
                 new XElement(NodeNames.IDENTIFER, Id),
                 new XElement(NodeNames.TITLE, Title),
                 new XElement(NodeNames.ITEM_FILE_ID,FileName)
               );


          
            return test;
            
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
