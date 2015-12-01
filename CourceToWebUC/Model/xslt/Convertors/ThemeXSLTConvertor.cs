using CommonConvertLib.StepMessager;
using CourceToWebUC.Model.XSLTHelpers;
using Saxon.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CourceToWebUC.Model.xslt
{
    /// <summary>
    /// xslt конвертор темы курса
    /// </summary>
    class ThemeXSLTConvertor : StepChangeMessager,IOperationModel
    {
        /// <summary>
        /// темы для конвертации
        /// </summary>
        IEnumerable<XElement> themes;
        /// <summary>
        /// параметры конвертации
        /// </summary>
        protected XSLTConvertParams convParams;
        /// <summary>
        /// адаптер для получения
        /// элементов курса
        /// </summary>
        IItemsAdapter itemsAdapter;
        /// <summary>
        /// посленяя ошибка
        /// 
        /// </summary>
        public string LastError { get; private set; }
        /// <summary>
        /// операция конвертации тем курса
        /// </summary>
        /// <param name="_themes">список тем</param>
        /// <param name="_params">параметры конвертации</param>
        public ThemeXSLTConvertor(IEnumerable<XElement> _themes,XSLTConvertParams _params,IItemsAdapter _itemsAdapter)
        {
            themes = _themes;
            convParams = _params;
            itemsAdapter = _itemsAdapter;
        }
        /// <summary>
        /// подготовка к конвертации
        /// </summary>
        public void Prepare()
        {
            
        }
        /// <summary>
        /// выполнить конвертацию тем
        /// </summary>
        public void Do()
        {
            try
            {
                if (themes == null || themes.Count<XElement>() < 1)
                    return;

              
                //XslCompiledTransform xslt = new XslCompiledTransform();
                //xslt.Load(convParams.ThemeShemePath);

                foreach (XElement th in themes)
                {
                    SendMessage("- Конвертация темы "+th.Element(NodeNames.TITLE).Value);

                    XDocument doc = new XDocument(th);
                    string fn = th.Element(NodeNames.ITEM_FILE_ID).Value;
                    string themeFilesNewPath = Path.Combine(convParams.OutMediaFiles.AsPath(), fn).AsPath();
                    string outFile = Path.Combine(convParams.OutItemsPath, fn + ".html");
                    ///SAXON
                    Processor proc = new Processor();
                    XdmNode input = proc.NewDocumentBuilder().Build(doc.Root.CreateReader());

                    XsltTransformer transformer = proc.NewXsltCompiler().Compile(new Uri(convParams.ThemeShemePath)).Load();
                    transformer.InitialContextNode = input;

                    transformer.SetParameter(new QName("", "", "mediaPath"), new XdmAtomicValue(convParams.CourseDataFolderName));
                    transformer.SetParameter(new QName("", "", "picFolder"), new XdmAtomicValue(th.Element(NodeNames.ITEM_FILE_ID).Value));
                    transformer.SetParameter(new QName("", "", "toolFolder"), new XdmAtomicValue(convParams.ToolsDataFolderName));

                    ///задать адрес директории для xsl:result-document
                    transformer.BaseOutputUri = new Uri(themeFilesNewPath);
                    Serializer serializer = new Serializer();

                    serializer.SetOutputFile(outFile);

                    transformer.Run(serializer);

                    //doc.Document.Declaration = new XDeclaration("1.0", "utf-8", "true");
                    //XPathNavigator nv = doc.CreateNavigator();

                    //XsltArgumentList xslArg = new XsltArgumentList();

                    //xslArg.AddParam("mediaPath", "", convParams.CourseDataFolderName);
                    //xslArg.AddParam("picFolder", "", th.Element(NodeNames.ITEM_FILE_ID).Value);
                    //xslArg.AddParam("toolFolder", "", convParams.ToolsDataFolderName);

                    //string outFile = Path.Combine(convParams.OutItemsPath, th.Element(NodeNames.ITEM_FILE_ID).Value + ".html");
                    //using (FileStream fs = new FileStream(outFile, FileMode.Create))
                    //{
                    //    xslt.Transform(nv, xslArg, fs);
                    //}

                    Guid idTheme = Guid.Parse(th.Element(NodeNames.IDENTIFER).Value);
                    ItemThemeForXSLT theme = GetTheme(idTheme);
                    CopyThemeFiles(theme);
                    DoOtherOperation(theme);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Исключение при конвертации темы курса: "+ex.Message);
            }
        }

        /// <summary>
        /// получить тему по идентификатору
        /// </summary>
        /// <param name="idTheme">идентификатор темы
        private ItemThemeForXSLT GetTheme(Guid _id)
        {
            if (_id == null)
                throw new ArgumentNullException();

            ItemThemeForXSLT theme = (ItemThemeForXSLT)itemsAdapter.GetCourseItem(_id);
            if (theme == null)
                throw new Exception("Не найдена тема с идентификатором: " + _id);

            return theme;
        }

        /// <summary>
        /// копирование файлов темы
        /// </summary>
        /// <param name="_theme">тема</param>
        private void CopyThemeFiles(ItemThemeForXSLT _theme)
        {
            if (_theme == null)
                throw new ArgumentNullException("CopyThemeFiles(ItemThemeForXSLT _theme)");

            SendMessage("Копирование файлов");

           string outMediaDirectory = Path.Combine(convParams.OutMediaFiles.AsPath(), _theme.FileName);
            _theme.CopyFiles(outMediaDirectory);
            _theme.CopyToolsFiles(Path.Combine(convParams.OutMediaFiles.AsPath(), convParams.ToolsDataFolderName.AsPath()), convParams.ToolImg);


        }

        /// <summary>
        /// функция создана для классов-наследников
        /// (в частности для возможности конвертации
        /// видео файлов темы в другие форматы)
        /// </summary>
        /// <remarks>создание данной функции - не самый 
        /// лучший способ, обеспечения гибкости кода, но решающий
        /// поставленную в настоящий момент задачу и
        /// подпбные задачи в обозримом будущем</remarks>
        protected virtual void DoOtherOperation(ItemThemeForXSLT _theme) { }
        /// <summary>
        /// действия после конвертации
        /// </summary>
        public void PostStep()
        {
           
        }
        /// <summary>
        /// отмена конвертации
        /// </summary>
        /// <remarks>отмена реализована в
        /// общем классе конвертации курса (метод
        /// оставлен на будующее)</remarks>
        public bool Undo()
        {
            return false;
        }
    }
}
