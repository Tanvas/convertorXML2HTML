using CommonConvertLib.StepMessager;
using CourceToWebUC.Model.XSLTHelpers;
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
    /// xslt конвертор манифеста 
    /// scorm
    /// </summary>
    class ScormXSLTConvertor : StepChangeMessager,IOperationModel
    {
        /// <summary>
        /// корневой элемент файлов для
        /// формирования манифеста 
        /// </summary>
        const string ROOT_SCORM_FILES = "scorm";
        /// <summary>
        /// элемент, содержащий путь к файлу
        /// </summary>
        const string SCORM_FILE = "file";
        /// <summary>
        /// курс для конвертации
        /// </summary>
        XElement course;
        /// <summary>
        /// параметры конвертации
        /// </summary>
        XSLTConvertParams convParams;
        /// <summary>
        /// последняя ошибка
        /// </summary>
        public string LastError { get; private set; }
       
        /// <summary>
        /// операция конвертации структуры курса
        /// </summary>
        /// <param name="_cource">курс</param>
        /// <param name="_params">параметры конвертации</param>
        public ScormXSLTConvertor(XElement _cource, XSLTConvertParams _params)
        {
            if (_cource == null)
                throw new ArgumentNullException("Нет курса для конвертации scorrm манифеста");

            course = _cource;
            convParams = _params;
            
        }
        /// <summary>
        /// подготовка к конвертации
        /// </summary>
        public void Prepare()
        {
            
        }
        /// <summary>
        /// выполнить конвертацию структуры
        /// </summary>
        public void Do()
        {
            try
            {
                SendMessage("\n- Создание манифеста scorm");
                XDocument doc = new XDocument(course);
                doc.Document.Declaration = new XDeclaration("1.0", "utf-8", "true");
                XPathNavigator nv = doc.CreateNavigator();

                XslCompiledTransform xslt = new XslCompiledTransform();

                xslt.Load(convParams.ContentShemePath);

                XsltArgumentList xslArg = new XsltArgumentList();
                xslArg.AddParam("itemsPath", "", convParams.RootFolderName);

                string outFile = Path.Combine(convParams.OutputAbsPath, convParams.StartFileName);

                using (FileStream fs = new FileStream(outFile, FileMode.Create))
                {
                    xslt.Transform(nv, xslArg, fs);
                }
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                throw new Exception("Исключение при конвертации структуры курса: "+ex.Message);
            }
        }

       
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

        /// <summary>
        /// подготовить файлы для тем
        /// курса
        /// </summary>
        void PrepareThemes()
        {
            IEnumerable<XElement> themes = course.Descendants(NodeNames.THEME_ROOT);
            if (themes == null || themes.Count<XElement>() < 1)
                return;

            foreach (XElement th in themes)
            {
                AddThemeFiles(th);
            }
                

        }
        
        /// <summary>
        /// добавить необходимые файлы 
        /// темы
        /// </summary>
        /// <param name="th">элемент темы</param>
        private void AddThemeFiles(XElement _theme)
        {
            if (_theme == null)
                return;

            string themeMediaDir = Path.Combine(convParams.OutMediaFiles.AsPath(), _theme.Element(NodeNames.ITEM_FILE_ID).Value).AsPath();
            if (!Directory.Exists(themeMediaDir))
            {
                SendMessage("\n!!!!!Отсутствует директория " + themeMediaDir);
                return;
            }
            string[] mdfiles = Directory.GetFiles(themeMediaDir, "*", SearchOption.AllDirectories);
            IEnumerable<XElement> tools = _theme.Descendants(NodeNames.TOOL);
            XElement scorm =
            new XElement(ROOT_SCORM_FILES,   
                from el in mdfiles           
                select new XElement(SCORM_FILE,                      
                        GetRelativePath(el)
                           ),
                from tl in tools
                select new XElement(SCORM_FILE,
                         GetRelativeToolPath(tl.Attribute(NodeNames.NEW_ATTR_FILE).Value)
                         )
              );
            _theme.Add(scorm);
        }

        /// <summary>
        /// получить путь относительно корневого
        /// каталога веб курса до файла инструмента
        /// </summary>
        /// <param name="_toolFileName">название файла инструмента</param>
        /// <returns>относительный путь</returns>
        private string GetRelativeToolPath(string _toolFileName)
        {
            string relativeToolPath = Path.Combine(convParams.OutMediaFiles.AsPath(), convParams.ToolsDataFolderName.AsPath()).ToLower();
            string rootDir = convParams.OutputAbsPath.ToLower().AsPath();
            if (relativeToolPath.StartsWith(rootDir))
            {
                relativeToolPath=relativeToolPath.Substring(rootDir.Length);
            }

            return Path.Combine(relativeToolPath, _toolFileName);
        }

        /// <summary>
        /// получить относительный путь 
        /// (относительно корневой папки курса) 
        /// </summary>
        /// <param name="_absPath">абсолютный путь</param>
        /// <returns>путь относительно корневой директории веб курса</returns>
        private string GetRelativePath(string _absPath)
        {
            if(string.IsNullOrEmpty(_absPath))
                throw new ArgumentNullException(" GetRelativePath(string _absPath)");

            _absPath=_absPath.ToLower();
            string rootDir = convParams.OutputAbsPath.ToLower().AsPath();
            if(_absPath.StartsWith(rootDir))
            {
                return _absPath.Substring(rootDir.Length);
            }

            return _absPath;
               
        }

        /// <summary>
        /// подготовить файлы для тестов
        /// курса
        /// </summary>
        void PrepareTests()
        {
            IEnumerable<XElement> tests = course.Descendants(NodeNames.TEST_ROOT);
            if (tests == null || tests.Count<XElement>() < 1)
                return;

            foreach (XElement tst in tests)
            {
                AddTestFiles(tst);
            }


        }
        /// <summary>
        /// добавить файлы теста
        /// </summary>
        /// <param name="_test">элемент теста</param>
        private void AddTestFiles(XElement _test)
        {
            if (_theme == null)
                return;

            string themeMediaDir = Path.Combine(convParams.OutMediaFiles.AsPath(), _theme.Element(NodeNames.ITEM_FILE_ID).Value).AsPath();
            if (!Directory.Exists(themeMediaDir))
            {
                SendMessage("\n!!!!!Отсутствует директория " + themeMediaDir);
                return;
            }
            string[] mdfiles = Directory.GetFiles(themeMediaDir, "*", SearchOption.AllDirectories);
            IEnumerable<XElement> tools = _theme.Descendants(NodeNames.TOOL);
            XElement scorm =
            new XElement(ROOT_SCORM_FILES,
                from el in mdfiles
                select new XElement(SCORM_FILE,
                        GetRelativePath(el)
                           ),
                from tl in tools
                select new XElement(SCORM_FILE,
                         GetRelativeToolPath(tl.Attribute(NodeNames.NEW_ATTR_FILE).Value)
                         )
              );
            _theme.Add(scorm);
        }
       

    }
}
