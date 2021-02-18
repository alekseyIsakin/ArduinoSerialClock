﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

using ArdClock.src;
using ArdClock.src.ArdPage;
using ArdClock.src.ArdPage.HelpingClass;
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
                    XmlNode ndName = nd_page.Attributes.GetNamedItem(XMLDefines.XMLPageAttr.Name);
                    XmlNode ndID = nd_page.Attributes.GetNamedItem(XMLDefines.XMLPageAttr.ID);

                    List<PageEl> page_elements = new List<PageEl>();

                    foreach (XmlNode nd_el in nd_page)
                    {
                        if (nd_el.Name == XMLDefines.XMLTag.PageEl)
                        {
                            // Собираем элемент страницы по шаблону
                            //

                            int type_ep = int.Parse(
                                nd_el.Attributes.GetNamedItem(
                                    XMLDefines.XMLBaseElAttr.TypeEl
                                ).Value);

                            PageEl pl = (PageEl)PageElCenter.TryLoadFromXml(type_ep, nd_el);

                            if (pl != null)
                                page_elements.Add(pl);
                        }
                    }

                    APage page = new APage(
                        ndName.Value,
                        int.Parse(ndID.Value),
                        page_elements
                        );
                    pageList.Add(page);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            return pageList;
        }
        
    }
}
