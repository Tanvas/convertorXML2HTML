using CourceToWebUC.Model.Helpers;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Saxon.Api;
using CommonConvertLib.StepMessager;
using CommonConvertLib.Helpers;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// интерфейс для получения
    /// элементов курса
    /// </summary>
    internal interface IItemsAdapter
    {
        /// <summary>
        /// получить элемент курса
        /// </summary>
        /// <param name="id">идентификатор элемента</param>
        /// <returns>null-элемент не найден</returns>
        IItemModel GetCourseItem(Guid id);
    }
    
    /// <summary>
    /// базоваый класс преобразования курса
    /// с помощью xslt
    /// </summary>
    internal abstract class XSLTConvertorBase : StepChangeMessager, IOperationModel, IItemsAdapter
    {
        
        /// <summary>
        /// курс для конвертации
        /// </summary>
        protected CourseForXSLT course;
        /// <summary>
        /// параметры конвертации курса
        /// </summary>
        protected XSLTConvertParams convParams;
        /// <summary>
        /// элемент курса для конвертации
        /// </summary>
        protected XElement convertCurse;
        /// <summary>
        /// список подопреаций конвертации
        /// курса
        /// </summary>
        protected List<IOperationModel> convertOperations;
        /// <summary>
        /// информация о последней ошибке
        /// </summary>
        public string LastError
        {
            get;
            protected set;
        }
        /// <summary>
        /// базовый класс преобразования курса
        /// с помощью xslt
        /// </summary>
        /// <param name="_curse">курс для преобразования</param>
        /// <param name="_params">параметры преобразования</param>
        public XSLTConvertorBase(CourseForXSLT _course, XSLTConvertParams _params)
        {
            if (_course == null || _params == null)
                throw new ArgumentNullException();

            course = _course;
            convParams = _params;
            convertOperations = new List<IOperationModel>();
          
        }
        
        /// <summary>
        /// подготовка к конвертации
        /// </summary>
        public void Prepare()
        {
            DoBefore();

            convertOperations.ForEach(x => x.StepChange += conv_StepChange);
        }
        /// <summary>
        /// действия перед началом конвертации
        /// инициализация компонент
        /// </summary>
        abstract protected void DoBefore();
        /// <summary>
        /// конвертировать курс
        /// </summary>
        /// <exception cref="AppException">ошибка конвертации</exception>
        public void Do()
        {
            try
            {
                DirectoryHelper.CreateDirectory(convParams.OutItemsPath, true);

                convertOperations.ForEach(x => x.Do());
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                throw new Exception(ex.Message);
            }


        }
       
       
        /// <summary>
        /// завершающие действия
        /// при конвертации
        /// </summary>
        public void PostStep()
        {
            try
            {               
                convertOperations.ForEach(x => x.PostStep());
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// отменить конвертацию
        /// </summary>
        /// <returns>успешность отмены</returns>
        public bool Undo()
        {
            try
            {
                DirectoryHelper.ClearDirectory(convParams.OutputAbsPath);
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// получить элемент курса
        /// </summary>
        /// <param name="id">идентификатор элемента</param>
        /// <returns>null-элемент не найден</returns>
        public IItemModel GetCourseItem(Guid id)
        {
            try
            {
                return course.GetItemById(id);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// обработчик события смены
        /// этапа конвертации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conv_StepChange(object sender, CommonConvertLib.StepMessager.StepChangeArg e)
        {
            SendMessage(e.StepDiscription);
        }
    }
}
