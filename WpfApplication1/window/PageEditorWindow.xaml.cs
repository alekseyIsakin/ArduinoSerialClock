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
using System.Windows.Shapes;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;

using ArdClock.src;
using ArdClock.src.ArdPage;
using ArdClock.src.HelpingClass;
using ArdClock.src.UIGenerate;
using ArdClock.src.ArdPage.PageElements;
using ArdClock.src.XMLLoader;

namespace ArdClock.window
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 
    public partial class PageEditorWindow : Window
    {
        public List<APage> pageList { get; private set;}
        public APage CurPage { get; private set; }

        public PageEditorWindow()
        {
            InitializeComponent();
            //Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");

            pageList = Loader.LoadPageListFromXML(
                System.Environment.CurrentDirectory + "\\ListPages.xml");

            if (pageList.Count > 0) 
            {
                list_page_name.ItemsSource = pageList;
            }
        }

        // Вывод интефейса для редактирования элементов 
        // страницы
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            APage editPage = pageList[list_page_name.SelectedIndex];
            //MessageBox.Show(editPage.Name);

            stackPanel1.Children.Clear();

            Label pageNameLabel = new Label();
            pageNameLabel.Content = editPage.Name;

            stackPanel1.Children.Add(pageNameLabel);

            foreach (var el in editPage.Elements) 
            {
                switch (el.GetTypeEl()) 
                {
                    case TPageEl.String:
                        stackPanel1.Children.Add(new Separator());
                        stackPanel1.Children.Add(
                            (new UIPageString((PageString)el)).UIDockPanel);
                        break;
                    case TPageEl.Time:
                        stackPanel1.Children.Add(new Separator());
                        stackPanel1.Children.Add(
                            new UIPageTime(((PageTime)el)).UIDockPanel);
                        break;
                }
            }

            CurPage = editPage;
        }
    }
}
