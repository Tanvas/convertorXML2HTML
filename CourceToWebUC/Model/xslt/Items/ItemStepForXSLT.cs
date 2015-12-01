/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemThemeForXSLT.cs
 * Создан: 04.03.2015
 * Редактирован: 04.03.2015
 */


using ContentLib.Core.Content.Learning;
using CourceToWebUC.Model.CommonModels;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс кадра для конвертации
    /// xslt
    /// </summary>
    internal class ItemStepForXSLT : ISubItemForXSLT
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        public Guid Id { get { return item.Id; } }
        /// <summary>
        /// переменная для формирования уникальных
        /// имен кадров в пределах коллекции тем
        /// </summary>
        static int number = 0;
        /// <summary>
        /// уникальный идентификатор
        ///  в пределах коллекции кадров темы
        /// </summary>
       // public int IdInStep { get; private set; }
        /// <summary>
        /// инструменты
        /// </summary>
        IList<ToolForXSLT> tools;
        /// <summary>
        /// кадр темы
        /// </summary>
        ItemStepModel item;
        /// <summary>
        /// название элемента
        /// </summary>
        public string Title
        {
            get { return item.Title; }
        }
        /// <summary>
        /// флаг установки параметра видео
        /// </summary>
        bool isSetSingleFrame = false;
        /// <summary>
        /// флаг указывающий, является ли
        /// видео кадра одной картинкой
        /// </summary>
        bool isSingleFrame; 
        /// <summary>
        /// имя файла элемента
        /// </summary>
        public string FileName { get { return item.PicName; } }
        /// <summary>
        /// идентификатор  
        /// кадра в пределах темы
        /// (для формирования уникального
        /// файла инструментов)
        /// </summary>
        string strIdentifer;
        /// <summary>
        /// тип элемента курса
        /// </summary>
        public ItemType Type { get { return ItemType.Step; } }
        /// <summary>
        /// класс темы для конвертации
        /// xslt 
        /// </summary>
        /// <param name="_item">тема курса</param>
        public ItemStepForXSLT(ItemStepModel _item) 
        {
            if (_item == null)
                throw new ArgumentNullException();

            item = _item;

            number++;           

            strIdentifer = "step" + number;
            InitTools();
        }
        /// <summary>
        /// инициировать инструменты кадра
        /// </summary>
        private void InitTools()
        {
            tools = new ObservableCollection<ToolForXSLT>();

            if (item.Tools == null || item.Tools.Count < 1)
                return;            
            foreach(CourceToWebUC.Model.CommonModels.ToolModel tm in item.Tools)
            {
                ToolForXSLT tx = new ToolForXSLT(tm);
                tools.Add(tx);
            }
        }



         /// <summary>
        /// сбросить уникальный идентификатор
        ///  в пределах коллекции кадров темы
        /// </summary>
        public static void ResetIdInStep()
        {
            number = 0;
        }

        /// <summary>
        /// копирование необходимых медиафайлов
        /// </summary>
        /// <param name="_outputFolder">папка для медиафайлов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для медиафайлов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов кадра</exception>
        public void CopyFiles(string _outputFolder)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для медиафайлов");
            try
            { 
                _outputFolder=_outputFolder.AsPath();
                string picNewPath = System.IO.Path.Combine(_outputFolder, item.PicName);

                System.IO.File.Copy(item.OriginFullPicPath, picNewPath, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов кадра " + this.Title + ". " + ex.Message);
            }
        }

        /// <summary>
        /// копирование необходимых инструментов
        /// </summary>
        /// <param name="_outputFolder">папка для инструментов</param>
        /// <param name="_toolParams">параметры изображения инструментов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для инструментов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов инструментов</exception>
        public void CopyToolsFiles(string _outputFolder, ToolImgParam _toolParams)
        {
            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для инструментов");
            if (_toolParams == null)
                throw new ArgumentNullException("Не заданы параметры изображения инструментов");
            try
            {
                
                foreach (ToolForXSLT tl in tools)
                {
                    tl.SetCopyParams(_toolParams);
                    tl.CopyFiles(_outputFolder); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// представление кадра 
        /// в виде xml данных
        /// </summary>
        /// <returns></returns>
        public XElement AsXMLForConvert()
        {
            SetFlagSingleFrame();
            XElement step = 
              new XElement(NodeNames.STEP_ROOT,
                    new XElement(NodeNames.IDENTIFER,Id),
                    new XElement(NodeNames.LOCALIDENTIFER, strIdentifer),
                    new XElement(NodeNames.TITLE, Title),
                    new XElement(NodeNames.PICTURE,
                        new XAttribute(NodeNames.NEW_ATTR_IS_SINGLE_FRAME,isSingleFrame),
                        item.PicName),
                    new XElement(NodeNames.TEXT, item.Text),
                    new XElement(NodeNames.TOOLS_ROOT,
                        from el in tools
                        select new XElement(el.AsXMLForConvert())
                        ),
                    new XElement(NodeNames.WARNING_ROOT,
                        new XAttribute(NodeNames.ATTR_TYPE_WARN,item.Warning.Type),
                        new XAttribute(NodeNames.ATTR_TEXT_WARN,item.Warning.Text)
                        )
                    
            );
            
            return step;
        }


        /// <summary>
        /// установить флаг простого (однокадрового)
        /// видео кадра
        /// </summary>
        private void SetFlagSingleFrame()
        {
            if (isSetSingleFrame)
                return;
            isSetSingleFrame = true;
            SWFToImage.SWFToImageObject obj = new SWFToImage.SWFToImageObject();
            obj.InitLibrary("demo", "demo");
            obj.InputSWFFileName =item.OriginFullPicPath;
            obj.Execute_Begin();
            int frameCount = obj.FramesCount;
            obj.Execute_End();
            isSingleFrame = frameCount == 1 ? true : false;
        }
    }
}
