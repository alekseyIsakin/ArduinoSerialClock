using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    abstract class UIBaseEl
    {
        public DockPanel UIDockPanel;

        public abstract PageEl CompileElement();
    }
}
