using CourceToWebUC.Model;
using CourceToWebUC.Model.xslt;
using CourceToWebUC.Model.xslt.DataLoader;
using CourceToWebUC.View.Helpers;
using CourceToWebUC.View.Validations;
using CourceToWebUC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourceToWebUC
{
    /// <summary>
    /// элемент управления конвертацией
    /// курса
    /// </summary> 
    public partial class UCCourse : UserControl,ICourseUC
    {
        string statrDirName;
        /// <summary>
        /// элемент управления конвертацией
        /// курса
        /// </summary> 
        public UCCourse()
        {
            InitializeComponent();
            statrDirName= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /// <summary>
        /// установить курс
        /// </summary>
        /// <param name="_course">курс</param>
        /// <param name="_absPathToParamsFolder">путь до папки с параметрами</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetCourse(ICourseModel _course, string _absPathToParamsFolder)
        {
            if (_course == null || string.IsNullOrEmpty(_absPathToParamsFolder) )
                throw new ArgumentNullException();

            pnlConvParam.IsEnabled = true;

            //XSLTParamFromXML xmlPrm = new XSLTParamFromXML(_absPathToParamsFolder);
            XSLTConvertParams prm = new XSLTConvertParams();
            XSLTParamsView prmv = new XSLTParamsView(prm);
            CourseViewModel ivm = new CourseViewModel(_course);
            XSLTConvertView cv = new XSLTConvertView(ivm, prmv);
            this.DataContext = ivm;
            tree.ItemsSource = ivm.Children;
            convBtn.Command = cv.СonvertCommand;

            Binding bi0 = new Binding();
            bi0.Source = prmv;
            bi0.Path = new PropertyPath("TemplateFilePath");
            bi0.Mode = BindingMode.TwoWay;
            bi0.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            
            tbParam.SetBinding(TextBox.TextProperty, bi0);

            Binding bi1 = new Binding();
            bi1.Source = prmv;
            bi1.Path = new PropertyPath("OutputAbsPath");
            bi1.Mode = BindingMode.TwoWay;
            bi1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bi1.ValidationRules.Add(new FolderTryValidation());
            tbPath.SetBinding(TextBox.TextProperty, bi1);

            Binding bi2 = new Binding();
            bi2.Source = prmv;
            bi2.Path = new PropertyPath("IsToScorm");
            bi2.Mode = BindingMode.TwoWay;
            bi2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;           
            IsScorm.SetBinding(CheckBox.IsCheckedProperty, bi2);
           // Binding bi = new Binding();
          //  bi.Source = _course.Items;
            // bi.Path = new PropertyPath("Sections");
            //  bi.Mode = BindingMode.TwoWay;
            // bi.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //tvCourse.SetBinding(TreeView.ItemsSourceProperty, bi);
            base.CommandBindings.Add(
               new CommandBinding(
                   ApplicationCommands.Undo,
                   (sender, e) => // Execute
                   {
                       e.Handled = true;
                       ivm.IsChecked = false;
                       this.tree.Focus();
                   },
                   (sender, e) => // CanExecute
                   {
                       e.Handled = true;
                       e.CanExecute = (ivm.IsChecked != false);
                   }));

            base.CommandBindings.Add(
              new CommandBinding(
                  ApplicationCommands.Redo,
                  (sender, e) => // Execute
                  {
                      e.Handled = true;
                      ivm.IsChecked = true;
                      this.tree.Focus();
                  },
                  (sender, e) => // CanExecute
                  {
                      e.Handled = true;
                      e.CanExecute = (ivm.IsChecked != true);
                  }));

            this.tree.Focus();
            
            
        }


        public ValidationRule FolderTryValidation { get; set; }

        private void openDirDilog_Click(object sender, RoutedEventArgs e)
        {
            CourceToWebUC.View.Helpers.OpenDirDilogHelper dirop = new View.Helpers.OpenDirDilogHelper(statrDirName);
            bool isSelect=dirop.Open();
            if (isSelect)
                this.tbPath.Text = dirop.SelectDir;
        }

        private void openFileDilog_Click(object sender, RoutedEventArgs e)
        {
            IOpenFileDialog fd = new OpenFileParamHelper("");

            if (!FileDilogOpen(fd))
                return;

            string templatePath = fd.SelectFile;
            this.tbParam.Text = templatePath;
        }

        bool FileDilogOpen(IOpenFileDialog fd)
        {
            try
            {
                if (fd.Open())
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выбора файла шаблона. " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FileDilogOpen(fd);
            }
            return false;
        }
    }
}
