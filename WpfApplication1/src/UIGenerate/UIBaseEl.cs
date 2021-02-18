using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    public class UIBaseEl : AbstrUIBase
    {
        private ContextMenu DPContextMenu;

        public UIBaseEl(int Height)
        {
            DPContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem();

            Container = new DockPanel();

            Container.Height = Height;
            ((DockPanel)Container).LastChildFill = false;

            mi.Header = "Del";
            mi.Click += delClick;

            DPContextMenu.Items.Add(mi);

            Container.ContextMenu = DPContextMenu; 
        }        



        public override PageEl CompileElement() {
            return null; 
        }
    }
}
