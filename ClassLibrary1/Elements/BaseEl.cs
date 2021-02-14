using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasePageLib.Elements
{
    public class PageEl
    {
        public byte x;
        public byte y;

        public PageEl(byte x, byte y)
        {
            SetPos(x, y);
        }

        public void SetPos(byte x, byte y)
        {
            this.x = (byte)(x / 2);
            this.y = (byte)(y / 2);
        }

        public List<byte> GetPos()
        {
            List<byte> lout = new List<byte>();
            lout.Add(x);
            lout.Add(y);
            return lout;
        }

        public virtual List<byte> GenSendData()
        {
            return new List<byte>();
        }
        public virtual TPageEl GetTypeEl()
        { return TPageEl.BaseEl; }
    }
}
