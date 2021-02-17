﻿using System;
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
        public UIPageTime(PageTime pt) : base (47)
        {
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

            // Флаги
            StackPanel spFlasgs = new StackPanel();

            CheckBox cbSecond = new CheckBox();
            CheckBox cbMinut = new CheckBox();
            CheckBox cbHour = new CheckBox();

            cbSecond.IsChecked = pt.Second;
            cbMinut.IsChecked = pt.Minut;
            cbHour.IsChecked = pt.Hour;

            cbSecond.Content = "сек";
            cbMinut.Content = "мин";
            cbHour.Content = "час";

            spFlasgs.Children.Add(cbSecond);
            spFlasgs.Children.Add(cbMinut);
            spFlasgs.Children.Add(cbHour);
            //

            UIDockPanel.Children.Add(lbl_pos);
            UIDockPanel.Children.Add(tbX);
            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, UIDockPanel.Background));
            UIDockPanel.Children.Add(tbY);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, UIDockPanel.Background));

            UIDockPanel.Children.Add(lbl_clr);
            UIDockPanel.Children.Add(tbC);
            UIDockPanel.Children.Add(rectC);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, UIDockPanel.Background));

            UIDockPanel.Children.Add(lbl_size);
            UIDockPanel.Children.Add(tbS);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, UIDockPanel.Background));

            UIDockPanel.Children.Add(spFlasgs);

            UIDockPanel.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, UIDockPanel.Background));

            AddDelButton();

            tbC.Uid = "tbC";
            tbX.Uid = "tbX";
            tbY.Uid = "tbY";
            tbS.Uid = "tbS";
            spFlasgs.Uid = "spF";
            cbSecond.Uid = "cbS";
            cbMinut.Uid = "cbM";
            cbHour.Uid = "cbH";
        }
        public override PageEl CompileElement() 
        {
            bool sec = false;
            bool min = false;
            bool hour = false;
            HelpingClass.AColor clr = null;
            int px = 0;
            int py = 0;
            int sz = 0;
            


            foreach (UIElement ch in UIDockPanel.Children) 
            {
                switch (ch.Uid) 
                {
                    case "spF":
                        StackPanel sp = (StackPanel)ch;

                        sec = (bool)((CheckBox)sp.Children[0]).IsChecked;
                        min = (bool)((CheckBox)sp.Children[1]).IsChecked;
                        hour = (bool)((CheckBox)sp.Children[2]).IsChecked;
                        break;

                    case "tbS":
                        if (int.TryParse(((TextBox)ch).Text, out sz))
                            sz = (sz & byte.MaxValue);
                        else
                            sz = 0;
                        break;
                    case "tbY":
                        if (int.TryParse(((TextBox)ch).Text, out py))
                            py = (py & byte.MaxValue);
                        else
                            py = 0;
                        break;
                    case "tbX":
                        if (int.TryParse(((TextBox)ch).Text, out px))
                            px = (px & byte.MaxValue);
                        else
                            px = 0;
                        break;
                    case "tbC":
                        try
                        {
                            clr = new HelpingClass.AColor(((TextBox)ch).Text);
                        }
                        catch 
                        {
                            clr = HelpingClass.AColors.WHITE;
                        }
                        break;

                }
            }

            PageTime p_out = new PageTime(
                (byte)px, (byte)py, 
                clr, 
                (byte)sz);

            p_out.Second = sec;
            p_out.Minut = min;
            p_out.Hour = hour;
            
            return p_out;
        }
    }
}
