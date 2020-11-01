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
using WpfApplication1.src;
using System.Xml;

namespace WpfApplication1.window
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class PageEditorWindow : Window
    {
        public List<APage> pageList { get; private set;}

        public PageEditorWindow()
        {
            InitializeComponent();
            LoadPageListFromXML();

            if (pageList.Count > 0) 
            {
                listBox1.ItemsSource = pageList;
            }
        }

        private void LoadPageListFromXML()
        {
            XmlDocument xdd = new XmlDocument();
            pageList = new List<APage>();

            string fileName = System.Environment.CurrentDirectory + "\\ListPages.xml";

            try
            {
                xdd.Load(fileName);

                XmlNode root = xdd.DocumentElement;

                foreach (XmlNode nd in root) 
                {
                    if (nd.Attributes.Count > 0) 
                    {
                        XmlNode ndName = nd.Attributes.GetNamedItem("Name");
                        XmlNode ndID = nd.Attributes.GetNamedItem("ID");

                        if (ndName != null && ndID != null) 
                        {
                            APage page = new APage(ndName.Value, int.Parse(ndID.Value));
                            pageList.Add(page);
                        }
                    }
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
    }
}
