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
    class UIPageTime : UIBaseEl
    {
        public UIPageTime(PageTime pt) 
        {
            UIDockPanel = new DockPanel();
            UIDockPanel.Height = 45;
            UIDockPanel.LastChildFill = false;

            // Интерфейс для настройки позиции
            Label lbl_pos = new Label();
            TextBox tbX = new TextBox();
            TextBox tbY = new TextBox();

            lbl_pos.Content = "Позиция";
            lbl_pos.VerticalAlignment = VerticalAlignment.Center;

            tbX.Text = pt.X.ToString();
            tbY.Text = pt.Y.ToString();

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

            string clr = pt.TextColor.ToHex();

            lbl_clr.Content = "Цвет";
            lbl_clr.VerticalAlignment = VerticalAlignment.Center;

            tbC.Text = clr;

            tbC.Width = 65;
            tbC.Height = 23;
            tbC.TextAlignment = TextAlignment.Center;
            tbC.MaxLength = 6;

            rectC.Fill = new SolidColorBrush(
                pt.TextColor.GetColor()
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
            lbl_size.VerticalAlignment = VerticalAlignment.Center;

            tbS.Text = pt.Size.ToString();

            tbS.Width = 25;
            tbS.Height = 23;
            //

            // Текст
            CheckBox cbSecond = new CheckBox();
            CheckBox cbMinut = new CheckBox();
            CheckBox cbHour = new CheckBox();

            cbSecond.IsChecked = pt.Second;
            cbMinut.IsChecked = pt.Minut;
            cbHour.IsChecked = pt.Hour;

            cbSecond.Content = "сек";
            cbMinut.Content = "мин";
            cbHour.Content = "час";

            DockPanel.SetDock(cbSecond, Dock.Top);
            DockPanel.SetDock(cbMinut, Dock.Top);
            DockPanel.SetDock(cbHour, Dock.Top);
            //

            UIDockPanel.Children.Add(lbl_pos);
            UIDockPanel.Children.Add(tbX);
            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));
            UIDockPanel.Children.Add(tbY);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_clr);
            UIDockPanel.Children.Add(tbC);
            UIDockPanel.Children.Add(rectC);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_size);
            UIDockPanel.Children.Add(tbS);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(cbSecond);
            UIDockPanel.Children.Add(cbMinut);
            UIDockPanel.Children.Add(cbHour);

        }
        public override PageEl CompileElement() 
        {
            PageEl p_out = new PageEl();

            return p_out;
        }
    }
}
