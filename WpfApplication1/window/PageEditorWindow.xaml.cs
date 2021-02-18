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
using ArdClock.src.ArdPage.HelpingClass;
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
        public List<APage> pageList { get; private set; }
        private List<UIBaseEl> UIControlList;
        public APage curPage { get; private set; }

        public string pathToXML = System.Environment.CurrentDirectory + "\\ListPages.xml";

        System.Windows.Threading.DispatcherTimer timerPopup;

        public PageEditorWindow()
        {
            InitializeComponent();
            //Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");

            timerPopup = new System.Windows.Threading.DispatcherTimer();
            timerPopup.Tick += ClosePopup;

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
            UpdateListPageEl();
        }

        public void SoftUpdate()
        {
            // Загружает информацию из сохранённого списка
            //

            elementsPageStackPanel.Children.Clear();

            Label pageNameLabel = new Label();
            pageNameLabel.Content = curPage.Name;
            elementsPageStackPanel.Children.Add(pageNameLabel);

            for (int i = 0; i < UIControlList.Count; i++)
            {
                UIBaseEl el = UIControlList[i];

                el.UIDockPanel.Background =
                    (i % 2 == 0) ? Brushes.WhiteSmoke : Brushes.LightGray;

                elementsPageStackPanel.Children.Add(UIControlList[i].UIDockPanel);
                elementsPageStackPanel.Children.Add(
                    UIGenerateHelping.NewSeparator(1, Brushes.Black));

            }
        }

        public void UpdateListPageEl(UIBaseEl new_el = null)
        {
            // Загружает информацию напрямую из сохранённой страницы
            //

            if (list_page_name.SelectedIndex == -1)
                return;

            APage editPage = pageList[list_page_name.SelectedIndex];

            elementsPageStackPanel.Children.Clear();

            if (new_el != null)
                editPage.Elements.Add(new_el.CompileElement());

            UIControlList.Clear();

            foreach (var el in editPage.Elements)
            {
                UIBaseEl UIel = PageElCenter.TryGenUiControl(el);

                if (UIel != null)
                {
                    AppendNewUIel(UIel);
                }
            }

            curPage = editPage;

            SoftUpdate();
        }

        private void AppendNewUIel(UIBaseEl UIel) 
        {

            UIel.DelClick += UIPageEl_DelClick;
            UIControlList.Add(UIel);
        }

        //
        // Events
        //

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
                UpdateListPageEl();

                ShowPopup(String.Format(
                    "Сохранено: {0} эл.", new_elements.Count.ToString()));
            }
            else { ShowPopup("Ничего не сохранено :("); }
        }

        private void UIPageEl_DelClick(object sender, EventArgs e)
        {
            UIControlList.Remove((UIBaseEl)sender);
            SoftUpdate();
        }
        //
        // Popup logic
        //
        public void ShowPopup(String text, double sec = 1)
        {
            popupTextBox.Opacity = 0.8;
            popupTextBox.Text = text;

            popup1.IsOpen = true;

            timerPopup.Interval = TimeSpan.FromSeconds(sec);
            timerPopup.Start();
        }

        private void ClosePopup(object sender, EventArgs e)
        {
            timerPopup.Interval = TimeSpan.FromMilliseconds(50);
            popupTextBox.Opacity *= 0.8;

            if (popupTextBox.Opacity <= 0.2)
            {
                popup1.IsOpen = false;
                popupTextBox.Opacity = 0.8;
                timerPopup.Stop();
            }
        }

        private void popup1_MouseLeave(object sender, MouseEventArgs e)
        {
            timerPopup.Interval = TimeSpan.FromMilliseconds(500);
            timerPopup.Start();
        }

        private void popup1_MouseEnter(object sender, MouseEventArgs e)
        {
            popupTextBox.Opacity = 0.8;
            timerPopup.Stop();
        }


        //
        // Context Menu events
        //

        // Context Menu: New Element
        private void MenuItemAdd_MouseEnter(object sender, RoutedEventArgs e)
        {
            List<MenuItem> lm = new List<MenuItem>();

            foreach (KeyValuePair<int, string> kv in PageElCenter.getDict())
            {
                MenuItem mi = new MenuItem();

                mi.Header = kv.Value;
                mi.Click += MenuItemAddPageEl_Click;

                lm.Add(mi);
            }

            ((MenuItem)sender).ItemsSource = lm;
        }

        public void MenuItemAddPageEl_Click(object sender, RoutedEventArgs e)
        {
            string nm = ((MenuItem)sender).Header.ToString();

            foreach (KeyValuePair<int, string> kv in PageElCenter.getDict())
            {
                if (nm == kv.Value)
                {
                    UIBaseEl UIel = PageElCenter.TryGenUiControl(kv.Key);
                    if (UIel != null)
                    {
                        AppendNewUIel(UIel);
                        SoftUpdate();
                        break;
                    }
                }
            }
        }
        //
        //
        //


    }
}
