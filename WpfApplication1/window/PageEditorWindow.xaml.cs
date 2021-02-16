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
        private List<UIBaseEl> UIControlList;
        public APage curPage { get; private set; }
        public string pathToXML = System.Environment.CurrentDirectory + "\\ListPages.xml";

        public PageEditorWindow()
        {
            InitializeComponent();
            //Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");

            pageList = Loader.LoadPageListFromXML(pathToXML);

            UIControlList = new List<UIBaseEl>();

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

            UIControlList.Clear();
            foreach (var el in editPage.Elements) 
            {
                UIBaseEl UIel = null;
                switch (el.GetTypeEl()) 
                {
                    case TPageEl.String:
                        UIel = new UIPageString((PageString)el);

                        stackPanel1.Children.Add(new Separator());
                        stackPanel1.Children.Add((UIel).UIDockPanel);
                        break;
                    case TPageEl.Time:
                        UIel = new UIPageTime((PageTime)el);

                        stackPanel1.Children.Add(new Separator());
                        stackPanel1.Children.Add(UIel.UIDockPanel);
                        break;
                }

                if  (UIel != null)
                    UIControlList.Add(UIel);
            }

            curPage = editPage;
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            List<PageEl> new_elements = new List<PageEl>();

            foreach (var UIel in UIControlList) 
            {
                new_elements.Add(UIel.CompileElement());
            }

            if (new_elements.Count >= 1)
            {
                curPage = new APage(curPage.Name, curPage.ID, new_elements);

                if (list_page_name.SelectedIndex != -1)
                    pageList[list_page_name.SelectedIndex] = curPage;

                Writer.WritePageListToXML(pageList, pathToXML);
                listBox1_SelectionChanged(list_page_name, null);

                MessageBox.Show("Сохранено");
            }
        }
    }
}
