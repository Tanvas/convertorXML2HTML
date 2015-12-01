/* 
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemThemeModel.cs
 * Создан: 10.03.2015
 * Редактирован: 10.03.2015
 */

using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.CommonModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace CourceToWebUC.Model
{
    /// <summary>
    /// класс элемента курса
    /// кадр
    /// </summary>
    internal class ItemStepModel 
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        public Guid Id { get { return item.identifier; } }   
        /// <summary>
        /// заголовок элемента
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// тип элемента курса
        /// </summary>
        public ItemType Type { get { return ItemType.Theme; } }

        /// <summary>
        /// подкадровый текст
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// относительный путь 
        /// до изображения кадра
        /// </summary>
        public string PathToPic { get; private set; }
        /// <summary>
        /// название файла изображения
        /// без расширения
        /// </summary>
        public string PicName { get; private set; }

        /// <summary>
        /// полный путь до
        /// "оригинального" 
        /// изображения кадра
        /// </summary>
        public string OriginFullPicPath { get; private set; }      
        /// <summary>
        /// коллекция дочерних элементов
        /// вырождена (всегда пустая)
        /// </summary>
        public IList<IItemModel> Items
        {
            get;
            private set;
        }
        /// <summary>
        /// предупреждение
        /// </summary>
        public StepMarkModel Warning { get; private set; }
        /// <summary>
        /// список инструментов кадра
        /// </summary>
        public IList<ToolModel> Tools { get; private set; }
        
        /// <summary>
        /// тема курса
        /// </summary>
        protected LearningStep item;

        /// <summary>
        /// класс элемента курса
        /// кадр
        /// </summary>
        /// <param name="_item">кадр темы</param>
        /// <param name="_absPathByThis">абсолютный путь до папки кадра
        /// не включает в себя часть пути самого кадра (тэг picture)</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemStepModel(LearningStep _item, string _absPathByThis)
        {
            if (_item == null || string.IsNullOrEmpty(_absPathByThis))
                throw new ArgumentNullException("Отсутствие информации о кадре темы");

                  
            try
            {
                item = _item;                
                Title = _item.title;
                PathToPic = Path.GetDirectoryName(_item.picture);
                PicName = Path.GetFileName(_item.picture);
               
                Text = ConvertByteToHTMLString(_item.text);

                OriginFullPicPath = Path.Combine(_absPathByThis.AsPath(), _item.picture);                
                Warning = new StepMarkModel(_item.warningFlag, _item.warningText);

                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

      
        /// <summary>
        /// инициировать инструменты
        /// </summary>
        /// <param name="_factory">фабрика инструментов</param>
        public void InitTools(ToolsFactory _factory)
        {
            List<Guid>tools=item.attributes.toolRefs;
            if (tools == null || tools.Count < 1)
                return;
            Tools=new ObservableCollection<ToolModel>();
            try
            { 
                tools.ForEach(x=> Tools.Add(_factory.Create(x)));
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании списка инструментов кадра " + Id + ". " + ex.Message);
            }
        }
        /// <summary>
        /// конвертировать rtf текст
        /// в html
        /// </summary>
        /// <param name="_textByte">массив байтов</param>
        /// <returns>строка в формате html</returns>
        public string ConvertByteToHTMLString(byte[] _textByte)
        {
            if (_textByte == null)
                throw new ArgumentNullException("Отсутсвует подкадровый текст "+item.identifier);
            try
            { 
                string rtfString = FormatConvertors.Base64ToTextUTF8Convertor.ByteArrayToUTF8String(_textByte);
                Convertor.RTF.HTML.TranslatorRtf rtfTransl = new Convertor.RTF.HTML.TranslatorRtf(rtfString);
                string htmlStr = rtfTransl.translate();

                if (string.IsNullOrEmpty(htmlStr))
                    throw new ArgumentNullException();

                return htmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка преобразования подкадрового текста "+item.identifier+" в формат html. "+ex.Message);
            }

        }

    }
}