using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.src
{
    public class APage
    {
        public string Name { get; private set; }
        public int ID     { get; private set; }

        public APage(string name, int id) 
        {
            Name = name;
            ID   = id;
        }
        public APage() 
        {
            Name = "NoName";
            ID = 0;
        }

        public override string ToString() 
        {
            return string.Format("Page {0}", Name);
        }
    }
    class PageEl 
    {
        private byte x;
        private byte y;

        public List<byte> GetPos()
        { 
            List<byte> lout = new List<byte>();
            lout.Add(x);
            lout.Add(y); 
            return lout; 
        }

        public PageEl(byte x, byte y) 
        {
            this.x = x;
            this.y = y;
        }
    }

    enum TPageEl
    {
        String=1
    }

    class PageString : PageEl
    {
        enum StrPageEl
        {
            Position=1,
            Color,
            Size,
            Data
        }

        private UInt16 Color;
        private byte Size;
    
        private string Data;

        public PageString(byte x, byte y, UInt16 clr, byte sz, string str) : base(x, y)
        {
            this.Color = clr;
            this.Size = sz;
            this.Data = str;
        }
        public List<byte> GetByteColor() 
        {
            List<byte> lout = new List<byte>();

            lout.Add((byte)(Color & byte.MaxValue));
            // #### ####
            //      ^^^^
            lout.Add((byte)((Color - (Color & byte.MaxValue)) >> 8)); //
            // #### ####
            // ^^^^

            return lout;
        }

        public List<byte> GenSendData() 
        {
            List<byte> lout = new List<byte>();

            lout.Add((byte)TPageEl.String); // З

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
