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
using ArdClock.src.APage;
using ArdClock.src.HelpingClass;
using ArdClock.src.UIGenerate;
using ArdClock.src.APage.PageElements;

namespace ArdClock.window
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 
    public partial class PageEditorWindow : Window
    {
        public List<APage> pageList { get; private set;}
        
        public PageEditorWindow()
        {
            InitializeComponent();
            //Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");
            
            LoadPageListFromXML();

            if (pageList.Count > 0) 
            {
                list_page_name.ItemsSource = pageList;
            }
        }

        private void LoadPageListFromXML()
        {
            XmlDocument xdd = new XmlDocument();
            pageList = new List<APage>();

            string fileName = System.Environment.CurrentDirectory + "\\ListPages.xml";
            //string fileName = System.Environment.CurrentDirectory + "\\ListPages.xml";

            try
            {
                xdd.Load(fileName);

                XmlNode root = xdd.DocumentElement;

                foreach (XmlNode nd_page in root)
                {
                    // Просмотр записанных страниц
                    // Чтение имени и ID страницы
                    XmlNode ndName = nd_page.Attributes.GetNamedItem("Name");
                    XmlNode ndID = nd_page.Attributes.GetNamedItem("ID");
                    List<PageEl> page_elements = new List<PageEl>();

                    if (ndName != null && ndID != null)
                    {
                        // Если имя и ID не нулевые
                        // Читаем элементы чтраницы

                        foreach (XmlNode nd_el in nd_page)
                        {
                            if (nd_el.Name == "PageEl")
                            {
                                // Собираем элемент страницы по шаблону
                                //
                                string type_ep = nd_el.Attributes.GetNamedItem("TPageEl").Value;

                                // Строка
                                if (type_ep == ((int)TPageEl.String).ToString())
                                {
                                    string pos_x = "0", pos_y = "0",
                                        clr_hex = "0x", sz = "0", dt_str = "";

                                    foreach (XmlNode nd_string_par in nd_el)
                                    {
                                        switch (nd_string_par.Name)
                                        {
                                            case ("Position"):

                                                pos_x = nd_string_par.Attributes.GetNamedItem("Pos_x").Value;
                                                pos_y = nd_string_par.Attributes.GetNamedItem("Pos_y").Value;
                                                break;

                                            case ("Color"):
                                                clr_hex = nd_string_par.Attributes.GetNamedItem("Value").Value;
                                                break;

                                            case ("Size"):
                                                sz = nd_string_par.Attributes.GetNamedItem("Value").Value;
                                                break;

                                            case ("Data"):
                                                dt_str = nd_string_par.Attributes.GetNamedItem("Value").Value;
                                                break;
                                        }
                                    }

                                    try
                                    {
                                        Byte b_pos_x, b_pos_y, b_sz;
                                        AColor color;

                                        b_pos_x = Convert.ToByte(pos_x);
                                        b_pos_y = Convert.ToByte(pos_y);
                                        b_sz = Convert.ToByte(sz);

                                        color = new AColor(clr_hex);
                                        PageString page_string = new PageString(b_pos_x, b_pos_y, color, b_sz, dt_str);
                                        
                                        page_elements.Add(page_string);
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("Ошибка сборки строки\n" + e.Message);
                                    }
                                }
                            }
                        }
                    }

                    APage page = new APage(
                        ndName.Value,
                        int.Parse(ndID.Value),
                        page_elements
                        );
                    pageList.Add(page);
                    //
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                var xmlDeclaration = xdd.CreateXmlDeclaration("1.0", "UTF-8", null);
                var root = xdd.CreateElement("pages");

                xdd.AppendChild(xmlDeclaration);
                xdd.AppendChild(root);

                xdd.Save(fileName);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
                        UIPageString ps = new UIPageString((PageString)el);
                        stackPanel1.Children.Add(ps.UIDockPanel);
                        break;
                }
            }
        }
    }
}
