using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArdClock.src.ArdPage.PageElements
{
    public abstract class AbstrPageEl
    {
        public abstract List<byte> GenSendData();
        public abstract byte GetTypeEl();
    }
}
