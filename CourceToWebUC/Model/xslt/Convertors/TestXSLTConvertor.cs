using CommonConvertLib.Helpers;
using CommonConvertLib.StepMessager;
using CourceToWebUC.Model.Helpers;
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
    /// xslt конвертор тестов курса
    /// </summary>
    class TestXSLTConvertor : StepChangeMessager,IOperationModel
    {
        /// <summary>
        /// посленяя ошибка
        /// 
        /// </summary>
        public string LastError { get; private set; }
        /// <summary>
        /// тесты для конвертации
        /// </summary>
        IEnumerable<XElement> tests;
        /// <summary>
        /// параметры конвертации
        /// </summary>
        XSLTConvertParams convParams;
        /// <summary>
        /// адаптер для получения
        /// элементов курса
        /// </summary>
        IItemsAdapter itemsAdapter;
        /// <summary>
        /// список используемых идентификаторов 
        /// ответов в тесте
        /// </summary>
        List<string> usedIdentifers;
        /// <summary>
        /// операция конвертации тестов курса
        /// </summary>
        /// <param name="_themes">список тестов</param>
        /// <param name="_params">параметры конвертации</param>
        public TestXSLTConvertor(IEnumerable<XElement> _tests, XSLTConvertParams _params, IItemsAdapter _itemsAdapter)
        {
            tests = _tests;
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
                SendMessage("\n- Конвертация тестов");

                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(convParams.TestShemePath);

                foreach (XElement tt in tests)
                {
                    usedIdentifers = new List<string>();

                    Guid idTest = Guid.Parse(tt.Element(NodeNames.IDENTIFER).Value);

                    ItemTestForXSLT test = (ItemTestForXSLT)itemsAdapter.GetCourseItem(idTest);
                    if (test == null)
                        throw new Exception("Не найден тест с идентификатором: " + idTest);

                    XDocument testDoc = XDocument.Load(test.XMLFilePath);
                    if (testDoc == null)
                        throw new Exception("Не удается загрузить тест " + idTest + ": " + test.XMLFilePath);
                    
                    CorrectTest(testDoc);
                    
                   

                    string outFile = Path.Combine(convParams.OutItemsPath, test.FileName + ".html");

                    string testFilesNewPath = Path.Combine(convParams.OutMediaFiles.AsPath(), test.FileName).AsPath();

                    DirectoryHelper.CreateDirectory(testFilesNewPath, false);


                    ///SAXON
                    Processor proc = new Processor();
                    XdmNode input = proc.NewDocumentBuilder().Build(testDoc.Root.CreateReader());

                    XsltTransformer transformer = proc.NewXsltCompiler().Compile(new Uri(convParams.TestShemePath)).Load();
                    transformer.InitialContextNode = input;

                    transformer.SetParameter(new QName("", "", "mediaFolderName"), new XdmAtomicValue(convParams.CourseDataFolderName));
                    transformer.SetParameter(new QName("", "", "currentFolderName"), new XdmAtomicValue(test.FileName));

                    ///задать адрес директории для xsl:result-document
                    transformer.BaseOutputUri = new Uri(testFilesNewPath);
                    Serializer serializer = new Serializer();

                    serializer.SetOutputFile(outFile);

                    transformer.Run(serializer);

                
                    CopyTestFiles(test, testFilesNewPath);


                }
            }
            catch(Exception ex)
            {
                throw new Exception("Исключение при конвертации теста курса: "+ex.Message);
            }
        }

        /// <summary>
        /// корректировать тест        
        /// </summary>
        /// <param name="testDoc">узел теста</param>
        private void CorrectTest(XDocument testDoc)
        {
            SetAdditionalInfo(testDoc);
            ConvertText(testDoc);
        }

        /// <summary>
        /// конвертировать текст
        /// вопросов/ответов
        /// </summary>
        /// <param name="testDoc">узел теста</param>
        private void ConvertText(XDocument testDoc)
        {
            IEnumerable<XElement> text = testDoc.Descendants(NodeNames.TEXT);
            char[] denied = new[] {'\n', '\t', '\r' };
            
            string repl = "</br>";
            foreach (XElement txtEl in text)
            {   
                StringBuilder newText = new StringBuilder();
                string txt = txtEl.Value;
                foreach (var ch in txt)
                {
                    if (!denied.Contains(ch))
                        newText.Append(ch);
                    else
                        newText.Append(repl);

                }
                newText = newText.Replace("\"", "'");
                txtEl.SetValue(newText);
            }
        }

        /// <summary>
        /// добавить дополнительную информацию
        /// к узлу теста
        /// </summary>
        /// <param name="testDoc">узел теста</param>
        void SetAdditionalInfo(XDocument testDoc)
        {

            IEnumerable<XElement> options = testDoc.Descendants(NodeNames.ANSWER);

            foreach (XElement op in options)
            {

                string id = GetIdentifer(10);                
                string hash = Cripto.GetMd5(id);

                op.SetAttributeValue(NodeNames.NEW_ATTR_ID, id);
                op.SetAttributeValue(NodeNames.NEW_ATTR_HASH, hash);
            }

        }

        /// <summary>
        /// копирование файлов теста
        /// </summary>
        /// <param name="_test">тест</param>
        /// <param name="_toPath">конечная директория</param>
        private void CopyTestFiles(ItemTestForXSLT _test, string _toPath)
        {
            if (_test == null || string.IsNullOrEmpty(_toPath))
                throw new ArgumentNullException();

            _test.CopyFiles(_toPath);


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
        /// получить идентификатор 
        /// варианта ответа
        /// </summary>
        /// <param name="_length">длина идентификатора</param>
        /// <returns>идентификатор</returns>
        string GetIdentifer(int _length)
        {
            int finderId = 100;
            string tmpId = Cripto.GetRandomSeq(_length);
            while (usedIdentifers.Contains(tmpId) && finderId > 0)
            {
                tmpId = Cripto.GetRandomSeq(_length);
                finderId--;
            }
            if (finderId == 0)
                throw new Exception("Не сгенерирован уникальный ключ варианта ответа теста. Пожалуйста, повторите попытку конвертации.");

            usedIdentifers.Add(tmpId);
            return tmpId;
        }
    }
}
