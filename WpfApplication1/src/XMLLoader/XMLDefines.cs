using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArdClock.src.XMLLoader
{
    class XMLDefines
    {
        private readonly string _typeKeyWord;

        private XMLDefines(string typeKeyWord)
        { _typeKeyWord = typeKeyWord; }

        public override string ToString()
        { return _typeKeyWord; }



        public class XMLTag : XMLDefines
        {
            public readonly static XMLDefines Pages = new XMLDefines("Pages");
            public readonly static XMLDefines Page = new XMLDefines("Page");
            public readonly static XMLDefines PageEl = new XMLDefines("PageEl");

            protected XMLTag(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLPageAttr : XMLDefines
        {
            public readonly static XMLDefines Name = new XMLDefines("Name");
            public readonly static XMLDefines ID = new XMLDefines("ID");

            protected XMLPageAttr(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLBaseElTag : XMLDefines
        {
            public readonly static XMLDefines Position = new XMLDefines("Position");

            protected XMLBaseElTag(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLBaseElAttr : XMLDefines
        {
            public readonly static XMLDefines TyprEl = new XMLDefines("TPageEl");
            public readonly static XMLDefines PosX = new XMLDefines("Pos_x");
            public readonly static XMLDefines PosY = new XMLDefines("Pos_y");

            protected XMLBaseElAttr(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLStringAttr : XMLBaseElAttr
        {
            public readonly static XMLDefines ColorValue = new XMLDefines("Value");
            public readonly static XMLDefines SizeValue = new XMLDefines("Value");
            public readonly static XMLDefines Data = new XMLDefines("Value");

            protected XMLStringAttr(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLStringTag : XMLBaseElTag
        {
            public readonly static XMLDefines Color = new XMLDefines("Color");
            public readonly static XMLDefines Size = new XMLDefines("Size");
            public readonly static XMLDefines Data = new XMLDefines("Data");

            protected XMLStringTag(string typeKeyWord)
                : base(typeKeyWord) { }
        }
        public class XMLTimeAttr : XMLStringAttr
        {
            public readonly static XMLDefines DataSec = new XMLDefines("Sec");
            public readonly static XMLDefines DataMin = new XMLDefines("Minut");
            public readonly static XMLDefines DataHour = new XMLDefines("Hour");

            protected XMLTimeAttr(string typeKeyWord)
                : base(typeKeyWord) { }
        }

        public class XMLTimeTag : XMLStringTag
        {
            protected XMLTimeTag(string typeKeyWord)
                : base(typeKeyWord) { }
        }
    }
}
