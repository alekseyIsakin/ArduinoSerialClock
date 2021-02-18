﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using ArdClock.src.ArdPage;
using ArdClock.src.ArdPage.HelpingClass;
using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.UIGenerate
{
    class UIPageString : UIBaseEl
    {
        public UIPageString(AbstrPageEl pEl)
            : base(60)
        {
            PageString ps = (PageString)pEl; 

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


            Container.Children.Add(tbT);

            Container.Children.Add(lbl_pos);
            Container.Children.Add(tbX);
            Container.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Container.Background));
            Container.Children.Add(tbY);

            Container.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Container.Background));

            Container.Children.Add(lbl_clr);
            Container.Children.Add(tbC);
            Container.Children.Add(rectC);


            Container.Children.Add(
                UIGenerateHelping.NewGridSplitter(10, Container.Background));

            Container.Children.Add(lbl_size);
            Container.Children.Add(tbS);

            tbC.Uid = "tbC";
            tbX.Uid = "tbX";
            tbY.Uid = "tbY";
            tbS.Uid = "tbS";
            tbT.Uid = "tbT";
        }

        public override PageEl CompileElement()
        {
            string dt = "";
            AColor clr = null;
            int px = 0;
            int py = 0;
            int sz = 0;

            foreach (UIElement ch in Container.Children) 
            {
                switch (ch.Uid) 
                {
                    case "tbT":
                        dt = ((TextBox)ch).Text;
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
                            clr = new AColor(((TextBox)ch).Text);
                        }
                        catch 
                        {
                            clr = AColors.WHITE;
                        }
                        break;
                }
            }

            PageString p_out = new PageString(
                (byte)px, (byte)py, 
                clr, 
                (byte)sz,
                dt);

            return p_out;
        }
    }
}
