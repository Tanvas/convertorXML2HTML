/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: DataServiceCourseLib.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using CourceToWebUC.Model.Helpers;
using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.XSLTHelpers;
using CommonConvertLib.StepMessager;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// курс для конвертации
    /// xslt
    /// </summary>
    public class CourseForXSLT : StepChangeMessager,ICourseModel
    {

        /// <summary>
        /// идентификатор курса
        /// </summary>
        public Guid Id
        {
            get { return origCourse.identifier; }
        }
        /// <summary>
        /// курс
        /// </summary>
        Course origCourse;   
        /// <summary>
        /// параметры флэш-роликов
        /// для всего курса
        /// </summary>
        FlashSettings flashParam;
        /// <summary>
        /// конвертировать ли 
        /// элемент курса
        /// </summary>
        bool? isConvert = false;
        /// <summary>
        /// конвертировать ли 
        /// элемент курса
        /// </summary>
        public bool IsConvert
        {
            get
            {

                foreach(IItemModel iim in Items)
                {
                    if (iim.IsConvert)
                        return true;
                }
                return false;
            }
            private set { isConvert = value; }
        }   
        
        /// <summary>
        /// тип элемента
        /// </summary>
        public ItemType Type
        {
            get { return ItemType.Course; }
        }
       
        /// <summary>
        /// название курса
        /// </summary>
        public string Title {   get; private set; }
        /// <summary>
        /// параметры конвертации
        /// курса
        /// </summary>
        XSLTConvertParams convParams;
        /// <summary>
        /// элементы курса
        /// </summary>
        public IList<IItemModel> Items { get; private set; }
        
        /// <summary>
        /// курс для преобразования xslt
        /// </summary>
        /// <param name="_data">провайдер данных для курса</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CourseForXSLT(IDataService _data)
        {
            if (_data == null)
                throw new ArgumentNullException();

            try
            {
                origCourse = _data.Course;
                
                Title = _data.GetTitle();
                Items = new ObservableCollection<IItemModel>(_data.GetItems());
                flashParam = new FlashSettings(600, 400);
               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
       

       

        /// <summary>
        /// конвертировать курс
        /// </summary>
        public void Convert(IConvertParam _param)
        {

            if (_param == null)
                throw new ArgumentNullException("Не переданы параметры конвертации");

            if (IsConvert == false)
                throw new Exception("Не выбран элемент для конвертации");

            convParams = (XSLTConvertParams)_param;
            if (convParams == null)
                throw new Exception("Неидентифицированные пареметры конвертации: " + _param.GetType());

            convParams.SetFlashParam(flashParam);
           
            if (!convParams.IsParamsValid)
                throw new Exception("Ошибка параметров конвертации. " + convParams.ErrorMessage+"\n Исправьте ошибку и перезапустите программу.");

            IOperationModel conv =ConvertorFactory.Create(this, convParams);
            conv.StepChange += conv_StepChange;
            try
            {

                conv.Prepare();
                conv.Do();
                conv.PostStep();
            }
            catch(Exception ex)
            {
                if (conv.Undo())
                    throw new Exception(ex.Message);
                else
                    throw new Exception("Произошла ошибка при конвертации: " + ex.Message + ". Действия конвертации не могут быть отменены: " + conv.LastError);
            }
           
        }

        /// <summary>
        /// перехватчик события смены
        /// этапа конвертации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conv_StepChange(object sender, StepChangeArg e)
        {
            SendMessage(e.StepDiscription);
        }

       
        /// <summary>
        /// получить элемент курса по идентификатору
        /// </summary>
        /// <param name="_id">идентификатор</param>
        /// <returns>элемент курса</returns>
        public IItemModel GetItemById(Guid _id)
        {
            if (_id == null)
                throw new ArgumentNullException();

            if (Id == _id)
                return this;

            return GetItemInChildren(_id);
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

            foreach (IItemModel iim in Items)
            {
                if (iim.Id == _id)
                    return iim;
                tmp = iim.GetItemInChildren(_id);

                if (tmp != null)
                    return tmp;

            }
            throw new Exception("Элемент с идентификатором:" + _id + " не найден.");
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
            XElement curse = 
            new XElement(NodeNames.COURSE_ROOT,
                new XElement(NodeNames.TITLE, Title),
                from el in Items
                let xel = (IItemModelForXSLT)el
                where xel.IsConvert != false
                select new XElement(xel.AsXMLForConvert())
                           

              );



            return curse;
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
                IsConvert = _isConvert;
                return true;
            }
            foreach (IItemModel iim in Items)
            {
                if (iim.SetIsConvertSelfOrChildren(_id, _isConvert))
                { 
                    IsConvert = _isConvert;
                    return true;
                }
            }
            throw new Exception("Элемент с идентификатором:" + _id + " не найден.");
        }

    }
}
