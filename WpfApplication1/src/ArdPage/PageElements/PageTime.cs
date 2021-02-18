using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArdClock.src.ArdPage.HelpingClass;

namespace ArdClock.src.ArdPage.PageElements
{
    class PageTime : PageString
    {
        public override byte GetTypeEl()
        { return 66; }

        public bool Hour = true;
        public bool Minut= true;
        public bool Second= false;

        public PageTime() : this(0,0, AColors.WHITE, 5) { }

        public PageTime(byte x, byte y, AColor clr, byte sz) :
            base(x, y, clr, sz, "") {}

        public override List<byte> GenSendData() 
        {
            base.Data = "";
            string tmp = "";

            if (Hour)
            {
                tmp += System.DateTime.Now.Hour.ToString();
                while (tmp.Length < 2)
                    tmp = "0" + tmp;
                base.Data += tmp;
                tmp = "";
            }
            if (Minut)
            {
                base.Data += Hour ? ":" : "";
                tmp += System.DateTime.Now.Minute.ToString();
                while (tmp.Length < 2) 
                    tmp = "0" + tmp;
                base.Data += tmp;
                tmp = "";
            }
            if (Second)
            {
                base.Data += (Hour || Minut) ? ":" : "";
                tmp += System.DateTime.Now.Second.ToString();
                while (tmp.Length < 2)
                    tmp = "0" + tmp;
                base.Data += tmp;
                tmp = "";
            }

            return base.GenSendData();
        }
    }
}
