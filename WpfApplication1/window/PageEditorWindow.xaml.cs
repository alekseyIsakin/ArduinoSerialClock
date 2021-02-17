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

        public void UpdateListPageEl(UIBaseEl new_el = null) 
        {
            if (list_page_name.SelectedIndex == -1)
                return;

            List<DockPanel> StackPanelEntryDP = new List<DockPanel>();

            APage editPage = pageList[list_page_name.SelectedIndex];

            elementsPageStackPanel.Children.Clear();

            Label pageNameLabel = new Label();
            pageNameLabel.Content = editPage.Name;

            elementsPageStackPanel.Children.Add(pageNameLabel);

            if (new_el != null)
                editPage.Elements.Add(new_el.CompileElement());

            UIControlList.Clear();

            foreach (var el in editPage.Elements)
            {
                UIBaseEl UIel = null;
                switch (el.GetTypeEl())
                {
                    case TPageEl.String:
                        UIel = new UIPageString((PageString)el);
                        StackPanelEntryDP.Add((UIel).UIDockPanel);
                        break;
                    case TPageEl.Time:
                        UIel = new UIPageTime((PageTime)el);
                        StackPanelEntryDP.Add(UIel.UIDockPanel);
                        break;
                }

                if (UIel != null)
                    UIControlList.Add(UIel);
                   
            }

            for (int i = 0; i < StackPanelEntryDP.Count; i++)
            {
                StackPanelEntryDP[i].Background =
                    (i % 2 == 0) ? Brushes.WhiteSmoke : Brushes.LightGray;

                elementsPageStackPanel.Children.Add(StackPanelEntryDP[i]);
                elementsPageStackPanel.Children.Add(
                    UIGenerateHelping.NewSeparator(1, Brushes.Black));

            }
            curPage = editPage;
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

        //
        // Popup logic
        //
        public void ShowPopup(String text, double sec=1) 
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

            foreach (TPageEl el in Enum.GetValues(typeof(TPageEl))) 
            {
                if ((int)el > 64 && (int)el < 127) 
                {
                    MenuItem mi = new MenuItem();

                    mi.Header = el.ToString();
                    mi.Click += MenuItemAddPageEl_Click;

                    lm.Add(mi);
                }
            }

            ((MenuItem)sender).ItemsSource = lm;
        }

        public void MenuItemAddPageEl_Click(object sender, RoutedEventArgs e) 
        {
            string nm = ((MenuItem)sender).Header.ToString();

            foreach (TPageEl el in Enum.GetValues(typeof(TPageEl)))
            {
                if (nm == el.ToString()) 
                {
                    switch (el)
                    {
                        case TPageEl.String:
                            UpdateListPageEl(new UIPageString(new PageString()));
                            break;

                        case TPageEl.Time:
                            UpdateListPageEl(new UIPageTime(new PageTime()));
                            break;
                    }
                    break;
                }
            }
        }
    }
}
