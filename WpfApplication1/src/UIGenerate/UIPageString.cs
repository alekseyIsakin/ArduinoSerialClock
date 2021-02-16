using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using ArdClock.src.ArdPage;
using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    class UIPageString : UIBaseEl
    {
        public UIPageString(PageString ps) 
        {
            UIDockPanel = new DockPanel();
            UIDockPanel.Height = 60;
            UIDockPanel.LastChildFill = false;

            // Интерфейс для настройки позиции
            Label lbl_pos = new Label();
            TextBox tbX = new TextBox();
            TextBox tbY = new TextBox();

            lbl_pos.Content = "Позиция";
            tbX.Text = ps.X.ToString();
            tbY.Text = ps.Y.ToString();

            tbX.MaxLength = 3;
            tbY.MaxLength = 3;

            tbX.Width = 25;
            tbY.Width = 25;

            tbX.Height = 23;
            tbY.Height = 23;

            tbX.TextAlignment = TextAlignment.Center;
            tbY.TextAlignment = TextAlignment.Center;

            //

            // Цвет
            Label lbl_clr = new Label();
            TextBox tbC = new TextBox();
            Rectangle rectC = new Rectangle();

            string clr = ps.TextColor.ToHex();

            lbl_clr.Content = "Цвет";
            tbC.Text = clr;

            tbC.Width = 65;
            tbC.Height = 23;
            tbC.TextAlignment = TextAlignment.Center;
            tbC.MaxLength = 6;

            rectC.Fill = new SolidColorBrush(
                ps.TextColor.GetColor()
                );
            rectC.Stroke = Brushes.Black;
            rectC.StrokeThickness = 3;

            rectC.Width = 23;
            rectC.Height = 23;
            //

            // Размер
            Label lbl_size = new Label();
            TextBox tbS = new TextBox();

            lbl_size.Content = "Размер";
            tbS.Text = ps.Size.ToString();

            tbS.Width = 25;
            tbS.Height = 23;
            //

            // Текст
            TextBox tbT = new TextBox();

            tbT.Height = 23;
            tbT.Text = ps.Data;

            DockPanel.SetDock(tbT, Dock.Bottom);
            //


            UIDockPanel.Children.Add(tbT);
            tbT.Uid = "tbT";

            UIDockPanel.Children.Add(lbl_pos);
            UIDockPanel.Children.Add(tbX);
            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));
            UIDockPanel.Children.Add(tbY);

            tbX.Uid = "tbX";
            tbY.Uid = "tbY";

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_clr);
            UIDockPanel.Children.Add(tbC);
            UIDockPanel.Children.Add(rectC);

            tbC.Uid = "tbC";

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_size);
            UIDockPanel.Children.Add(tbS);

            tbS.Uid = "tbS";
        }

        public override PageEl CompileElement()
        {
            string dt = "";
            string clr = "";
            byte px = 0;
            byte py = 0;
            byte sz = 0;

            foreach (UIElement ch in UIDockPanel.Children) 
            {
                switch (ch.Uid) 
                {
                    case "tbT":
                        dt = ((TextBox)ch).Text;
                        break;
                }
            }

            PageString p_out = new PageString(
                0,0, 
                HelpingClass.AColors.CYAN, 
                4,
                dt);

            return p_out;
        }
    }
}
