﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArdClock.src.HelpingClass;
using ArdClock.src.ArdPage.PageElements;
using ArdClock.src.UIGenerate;

namespace ArdClock.src.ArdPage
{
    /*
     Объявление всех классов, которые будут хранить
     представление данных, которые будут отрисованы
     на ардуино
    */

    //
    // Хранит имя и ID одной страницы
    // а также сведения об элементах на данной странице
    //

    public class APage
    {
        public string Name { get; private set; }
        public int ID      { get; private set; }

        public List<AbstrPageEl> Elements { get; private set; }

        public APage(string name, int id, List<AbstrPageEl> elements) 
        {
            Name = name;
            ID   = id;
            Elements = elements;
            
        }

        public APage() :
            this("NoName", -1, null) { }

        public bool TestPage(List<Byte> in_out_dt=null) 
        {
            bool result = true;
            List<Byte> out_dt = new List<byte>();

            if (in_out_dt == null)
            {
                foreach (var e in Elements)
                {
                    out_dt.AddRange(e.GenSendData());
                }
            }
            else
                out_dt = in_out_dt;

            if (out_dt.Count > 64)
                result = false;

            return result;
        }
        
        public override string ToString() 
        { return string.Format("Page {0}", Name); }

        public List<byte> GenSendData()
        {
            List<byte> out_dt = new List<byte>();

            if (Elements == null)
            {
                System.Windows.MessageBox.Show("Пустая страница");
                return out_dt;
            }
            foreach (var e in Elements)
            {
                out_dt.AddRange(e.GenSendData());
            }

            if (TestPage())
                return out_dt;
            else
                return new List<byte>();
        }
    }
}
