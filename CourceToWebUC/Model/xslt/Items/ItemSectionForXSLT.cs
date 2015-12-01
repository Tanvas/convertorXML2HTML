/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemSectionForXSLT.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */


using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс темы для конвертации
    /// xslt
    /// </summary>
    internal class ItemSectionForXSLT: IItemModelForXSLT
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        public Guid Id { get { return item.identifier; } }   
        /// <summary>
        /// элемент курса
        /// </summary>
        LearningSection item;
        /// <summary>
        /// название элемента
        /// </summary>
        public string Title
        {
            get { return item.title; }
        }
        /// <summary>
        /// название файла элемента
        /// </summary>
        /// <remarks>для секций отсутствует, т.к.
        /// они не имеют отдельного файла</remarks>
        public string FileName
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// тип элемента курса
        /// </summary>
        public ItemType Type { get { return ItemType.Section;} }

        /// <summary>
        /// дочерние элементы
        /// </summary>
        public IList<IItemModel> Items { get; private set; }        
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
            get{return _isConv;}
            
        }
        /// <summary>
        /// класс темы для конвертации
        /// xslt 
        /// </summary>
        /// <param name="_item">тема курса</param>
        /// <param name="_factory">фабрика элементов курса</param>
        public ItemSectionForXSLT(LearningSection _item,XSLTItemsFactory _factory) 
        {
            if (_item == null || _factory==null)
                throw new ArgumentNullException();
            try
            {
                item = _item;
                Items = new ObservableCollection<IItemModel>();
                InitChild(_item,_factory);
                
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// инициализирует дочерние элементы
        /// </summary>
        /// <param name="_item">корневой элемент</param>
        /// <param name="_factory">фабрика элементов курса</param>
        /// <exception cref="ArgumentNullException">Нулевой дочерний элемент</exception>
        /// <exception cref="AppException">Не удалось идентифицировать дочерний элемент</exception>
        void InitChild(LearningSection _item, XSLTItemsFactory _factory)
        {
            IItemModelForXSLT ch = null;
            bool validCreating = false;

            foreach (AbstractLearningItem li in _item.childs)
            {
                try
                {
                    ch = _factory.Create(li, out validCreating);
                    if (!validCreating)
                        throw new Exception(item.identifier + ": неопределенный тип элемента курса: " + item.GetType());
                    
                    if (ch != null)
                        Items.Add(ch);
                    
                }
                catch (ArgumentNullException)
                {
                    throw new Exception("Один из дочерних элементов нулевой: " + _item.identifier);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
                
            }
        }




        public void CopyFiles(string _outputFolder)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// получить элемент
        /// как данные xml,
        /// при условии, что он помечен
        /// для конвертации
        /// </summary>
        /// <returns>  xml представление элемента</returns>
        public XElement AsXMLForConvert()
        {
            XElement section = 
            new XElement(NodeNames.SECTION_ROOT,
                new XElement(NodeNames.IDENTIFER, Id),
                new XElement(NodeNames.TITLE, Title),
                from el in Items
                let xel=(IItemModelForXSLT)el
                where xel.IsConvert!=false
                select new XElement(xel.AsXMLForConvert())                          
                      
              );



            return section;
        }

        
        /// <summary>
        /// получить элемент
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <returns>элемент</returns>
        public IItemModel GetItemInChildren(Guid _id)
        {
            if (_id == null)
                throw new ArgumentNullException();

            IItemModel tmp;

            foreach(IItemModel iim in Items)
            {
                if (iim.Id == _id)
                    return iim;
                tmp = iim.GetItemInChildren(_id);
               
                if (tmp != null)
                    return tmp;
                
            }
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
            if(Id==_id)
            {
                _isConv = _isConvert;
                return true;
            }
            foreach(IItemModel iim in Items)
            {
                if (iim.SetIsConvertSelfOrChildren(_id, _isConvert))
                {
                    _isConv = _isConvert;
                    return true; 
                }
            }
            return false;
        }


    }
}
