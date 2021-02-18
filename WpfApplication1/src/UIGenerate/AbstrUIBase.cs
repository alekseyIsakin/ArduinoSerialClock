using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    public abstract class AbstrUIBase
    {
        public abstract PageEl CompileElement();
        public Panel Container;
        public event EventHandler DelClick;


        protected void delClick(object sender, EventArgs e)
        {
            if (DelClick != null)
                DelClick.Invoke(this, e);
        }
    }
}
