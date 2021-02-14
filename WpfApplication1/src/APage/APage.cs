using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArdClock.src.HelpingClass;
using ArdClock.src.APage.PageElements;

namespace ArdClock.src.APage
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

        public List<PageEl> Elements { get; private set; }

        public APage(string name, int id, List<PageEl> elements) 
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
        {
            return string.Format("Page {0}", Name);
        }

        public List<byte> GenSendData()
        {
            List<byte> out_dt = new List<byte>();
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

    public enum TPageEl
    {
        BaseEl,
        String = 65,
        ClearCode = 127
    }

    //
    // Строка 
    //
    
    //
    // Вспомогательные классы
    //
}
