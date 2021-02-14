using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ArdClock.src.APage;
using ArdClock.src.APage.PageElements;

namespace ArdClock.src.UIGenerate
{
    class UIPageString
    {
        public DockPanel UIDockPanel;
        public UIPageString(PageString ps) 
        {
            UIDockPanel = new DockPanel();
            UIDockPanel.Height = 30;
            UIDockPanel.LastChildFill = false;

            // Интерфейс для настройки позиции
            Label lbl_pos = new Label();
            TextBox tbX = new TextBox();
            TextBox tbY = new TextBox();

            lbl_pos.Content = "Позиция";
            tbX.Text = ps.GetPos()[0].ToString();
            tbY.Text = ps.GetPos()[1].ToString();

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
            ComboBox CBSendText = new ComboBox();

            //
            
            UIDockPanel.Children.Add(lbl_pos);
            UIDockPanel.Children.Add(tbX);
            UIDockPanel.Children.Add(NewGridSplitter(5, Brushes.White));
            UIDockPanel.Children.Add(tbY);

            UIDockPanel.Children.Add(NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_clr);
            UIDockPanel.Children.Add(tbC);
            UIDockPanel.Children.Add(rectC);

            UIDockPanel.Children.Add(NewGridSplitter(10, Brushes.White));

            UIDockPanel.Children.Add(lbl_size);
            UIDockPanel.Children.Add(tbS);
            
        }

        private GridSplitter NewGridSplitter(int width, Brush bgColor)
        {
            GridSplitter gs = new GridSplitter
            {
                Width = width,
                Background = bgColor
            };

            return gs;
        }
    }
}
