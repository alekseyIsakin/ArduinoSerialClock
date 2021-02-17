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

        public UIBaseEl(int Height)
        {
            UIDockPanel = new DockPanel();
            UIDockPanel.Height = Height;
            UIDockPanel.LastChildFill = false; 
        }        



        public virtual PageEl CompileElement() {
            return null; 
        }

        public void AddDelButton()
        {
            Button bt = new Button();

            bt.Content = "Del";
            bt.Uid = "delBtn";
            bt.Click += delClick;

            UIDockPanel.Children.Add(bt);
        }

        private void delClick(object sender, EventArgs e)
        {
            if (DelClick != null)
                DelClick.Invoke(this, e);
        }
    }
}
