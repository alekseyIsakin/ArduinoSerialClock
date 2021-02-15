using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

using ArdClock.src;
using ArdClock.src.ArdPage;
using ArdClock.src.HelpingClass;
using ArdClock.src.UIGenerate;
using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.XMLLoader
{
    static class Loader
    {
        static public List<APage> LoadPageListFromXML(string fileName)
        {
            
            XmlDocument xdd = new XmlDocument();
            List<APage> pageList = new List<APage>();

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
                                int test = int.Parse(
                                    nd_el.Attributes.GetNamedItem("TPageEl").Value);
                                
                                TPageEl type_ep = (TPageEl)test;
                                
                                switch (type_ep) 
                                {
                                    case TPageEl.String:
                                        page_elements.Add(ReadLikePageString(nd_el));
                                        break;
                                    case TPageEl.Time:
                                        page_elements.Add(ReadLikePageTime(nd_el));
                                        break;
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
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            return pageList;
        }

        /*
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         */
        static private PageEl ReadLikePageString(XmlNode nd_el) 
        {
            PageEl out_pageEl = new PageEl();
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
                out_pageEl = new PageString(b_pos_x, b_pos_y, color, b_sz, dt_str);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сборки строки строки\n" + e.Message);
            }
            return out_pageEl;
        }

        /*
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         * ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
         */
        static private PageEl ReadLikePageTime(XmlNode nd_el)
        {
            PageEl out_pageEl = new PageEl();
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
                        dt_str = nd_string_par.Attributes.GetNamedItem("Sec").Value;
                        dt_str += nd_string_par.Attributes.GetNamedItem("Minut").Value;
                        dt_str += nd_string_par.Attributes.GetNamedItem("Hour").Value;
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
                out_pageEl = new PageTime(b_pos_x, b_pos_y, color, b_sz);

                ((PageTime)out_pageEl).Second = (dt_str[0] == '1');
                ((PageTime)out_pageEl).Minut = (dt_str[1] == '1');
                ((PageTime)out_pageEl).Hour = (dt_str[2] == '1');
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сборки строки строки\n" + e.Message);
            }
            return out_pageEl;
        }
    }
}
