/*
 * Библиотека для конвертации курса в web формат
 * Разработчик: Лобанова Т.В.
 * 
 * Файл: ItemQestionForXSLT.cs
 * Создан: 09.04.2015
 * Редактирован: 09.04.2015
 */


using ContentLib.Core.Content.Learning;
using ContentLib.Core.Content.Testing;
using CourceToWebUC.Model.XSLTHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// класс вопроса в тесте для конвертации
    /// xslt
    /// </summary>
    public class ItemQestionForXSLT:ISubItemForXSLT
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        public Guid Id { get { return item.identifier; } }   
      

        /// <summary>
        /// кадр темы
        /// </summary>
        AbstractQuestion item;
        /// <summary>
        /// полный путь до
        /// папки с xml структурой 
        /// теста
        /// </summary>
        public string OriginFullTestPath { get; private set; }
        /// <summary>
        /// имя файла элемента
        /// </summary>
        public string FileName { get { throw new Exception("Не задано имя файла для вопроса теста."); ; } }
        
        /// <summary>
        /// класс вопроса в тесте для конвертации
        /// xslt 
        /// </summary>
        /// <param name="_item">вопрос теста курса</param>
        public ItemQestionForXSLT(AbstractQuestion _item, string _absPathByThis) 
        {
            if (_item == null || string.IsNullOrEmpty(_absPathByThis))
                throw new ArgumentNullException("Отсутствие информации о вопросе теста");


            try
            {
                item = _item;

                OriginFullTestPath = _absPathByThis.AsPath();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }





        /// <summary>
        /// копирование необходимых медиафайлов
        /// </summary>
        /// <param name="_outputFolder">папка для медиафайлов</param>
        /// <exception cref="ArgumentNullException">Не задана выходная папка для медиафайлов</exception>
        /// <exception cref="AppException">Ошибка при копировании файлов кадра</exception>
        public void CopyFiles(string _outputFolder)
        {
            List<PictureItem> pil = item.attributes.pictureItems;
            if (pil == null || pil.Count < 1)
                return;

            if (string.IsNullOrEmpty(_outputFolder))
                throw new ArgumentNullException("Не задана выходная папка для медиафайлов");
            try
            { 
                _outputFolder=_outputFolder.AsPath();
                foreach (PictureItem pi in pil)
                {
                    CopyPic(_outputFolder, pi);
                }
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// копирование картинки в папку вопроса
        /// </summary>
        /// <param name="_to">выходная папка</param>
        /// <param name="pi">экземпляр изображения</param>
        private void CopyPic(string _to, PictureItem pi)
        {
            if(pi==null)
                throw new ArgumentNullException("Отсутствует изображение вопроса "+item.identifier);
            try
            {
                string picOrigPath = System.IO.Path.Combine(OriginFullTestPath, pi.fileName);
                string picName = System.IO.Path.GetFileName(pi.fileName);
                string picNewPath = System.IO.Path.Combine(_to, picName);

                System.IO.File.Copy(picOrigPath, picNewPath, true);
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка при копировании файлов вопроса " + this.Id + ". " + ex.Message);
            }

        }

        /// <summary>
        /// представление кадра 
        /// в виде xml данных
        /// </summary>
        /// <returns></returns>
        public XElement AsXMLForConvert()
        {
            throw new Exception("Не задано представление xml для вопроса теста.");
        }




    }
}
