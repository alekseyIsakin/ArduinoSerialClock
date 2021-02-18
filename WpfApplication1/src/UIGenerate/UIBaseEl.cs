using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    public class UIBaseEl
    {
        public DockPanel UIDockPanel;
        public event EventHandler DelClick;

        private ContextMenu DPContextMenu;

        public UIBaseEl(int Height)
        {
            DPContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem();

            UIDockPanel = new DockPanel();
            UIDockPanel.Height = Height;
            UIDockPanel.LastChildFill = false;

            mi.Header = "Del";
            mi.Click += delClick;

            DPContextMenu.Items.Add(mi);

            UIDockPanel.ContextMenu = DPContextMenu; 
        }        



        public virtual PageEl CompileElement() {
            return null; 
        }

        private void delClick(object sender, EventArgs e)
        {
            if (DelClick != null)
                DelClick.Invoke(this, e);
        }
    }
}
