

using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс создания элементов курса 
    /// для xslt преобразования
    /// </summary>
    class XSLTItemsFactory
    {
        /// <summary>
        /// фабрика инструментов
        /// </summary>
        ToolsFactory tlFactory;
        public XSLTItemsFactory(Course _course)
        {
            if (_course == null)
                throw new ArgumentNullException("Не задан курс для создания элементов");

            string pathTools = System.IO.Path.Combine(_course.rootDirectory.AsPath()
                                                        , _course.pathDirectory.AsPath()
                                                        , _course.attributes.toolsList.directory.AsPath());
            tlFactory = new ToolsFactory(_course.attributes.toolsList, pathTools);

        }
        /// <summary>
        /// получить объект элемента курса
        /// IItemModelForXSLT
        /// </summary>
        /// <param name="_item">объект элемента курса IItemModel</param>
        /// <param name="_isValid">выходная переменная для подтверждения корректности опреации
        /// создания элемента</param>
        /// <returns>объект элемента курса IItemModelForXSLT
        /// null - объект не идентифицирован</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>выходная переменная внесена для предотвращения исключений
        /// с возвратом нулевого элемента заведомо учтенных данных (входной, выходной тесты)</remarks>
        public IItemModelForXSLT Create(AbstractLearningItem _item,out bool _isValid)
        {
            if (_item == null)
                throw new ArgumentNullException();

            IItemModelForXSLT im = null;
            _isValid = false;

            ItemType iType = _item.GetItemType();

          

            switch (iType)
            {
                case ItemType.Section:
                    im = new ItemSectionForXSLT((LearningSection)_item,this);                    
                    break;
                case ItemType.Theme:
                    im = new ItemThemeForXSLT((LearningItem)_item, tlFactory);                    
                    break;
                case ItemType.Test:
                    im = new ItemTestForXSLT((ContentLib.Core.Content.Testing.Test)_item);
                    break;
                case ItemType.InTest:
                case ItemType.OutTest:
                    _isValid = true;
                    break;

            }

            if (im != null)
                _isValid = true;

            return im;
        }
    }
}
