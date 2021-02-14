using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArdClock.src.HelpingClass;

namespace ArdClock.src.APage.PageElements
{
    class PageString : PageEl
    {
        enum StrPageEl
        {
            Position = 1,
            Color,
            Size,
            Data,
        }

        public AColor TextColor;
        public byte Size;

        public string Data;

        public override TPageEl GetTypeEl()
        { return TPageEl.String; }

        public PageString(byte x, byte y, AColor clr, byte sz, string str)
            : base(x, y)
        {
            this.TextColor = clr;
            this.Size = sz;
            this.Data = str;
        }

        public List<byte> GetByteColor()
        {
            return TextColor.GetByteColor();
        }

        public override List<byte> GenSendData()
        {
            List<byte> lout = new List<byte>();

            lout.Add((byte)TPageEl.String);

            lout.Add((byte)StrPageEl.Position);
            lout.AddRange(GetPos());

            lout.Add((byte)StrPageEl.Color);
            lout.AddRange(GetByteColor());

            lout.Add((byte)StrPageEl.Size);
            lout.Add(Size);

            lout.Add((byte)StrPageEl.Data);

            foreach (Char chr in Data)
            {
                lout.Add((byte)chr);
            }

            lout.Add(0x00);
            return lout;
        }
    }

}
