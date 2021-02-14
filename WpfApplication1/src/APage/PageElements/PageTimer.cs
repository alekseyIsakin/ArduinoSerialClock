using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArdClock.src.HelpingClass;

namespace ArdClock.src.APage.PageElements
{
    class PageTime : PageString
    {
        public PageTime(byte x, byte y, AColor clr, byte sz) :
            base(x, y, clr, sz, "") 
        {
        
        }
        public override List<byte> GenSendData() 
        {
            base.Data = System.DateTime.Now.ToLongTimeString();

            return base.GenSendData();
        }
    }
}
