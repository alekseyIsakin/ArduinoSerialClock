using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

using ArdClock.src.HelpingClass;
using ArdClock.src.APage.PageElements;
using ArdClock.src.APage;

namespace ArdClock
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private System.Timers.Timer timer2;
        private System.Windows.Threading.DispatcherTimer timer;

        private src.SerialControl.DataSender DSender;
        private src.PageHolder PHolder;

        public window.PageEditorWindow PEWindow;

        //
        // Предотвращение запуска второго экземпляра программы
        //
        private static System.Threading.Mutex mt;
        private static bool isSingleProgram;

        static MainWindow()
        {
            mt = new System.Threading.Mutex(false, " ", out isSingleProgram);

            if (!isSingleProgram)
                System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //
        //
        //

        //
        // Логика NotifyIcon
        //
        private src.NIcon NIcon = null;

        public void notifyIcon_Click(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (Visibility == Visibility.Hidden)
                {
                    Visibility = Visibility.Visible;
                }
                Activate();
            }
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) 
            {
                ChangeVisbleWindow();
                WindowState = WindowState.Normal;
            }
        }
        public void onClose(object sender, EventArgs e) { Application.Current.Shutdown(); }
        public void ChangeVisbleWindow() 
        {
            Visibility = (Visibility == Visibility.Hidden) ? Visibility.Visible : 
                                                             Visibility.Hidden;

        }


        //
        //
        //
        public MainWindow()
        {
            InitializeComponent();

            NIcon = new src.NIcon(Icon);
            NIcon.Click += notifyIcon_Click;
            NIcon.DoubleClick += notifyIcon_Click;

            NIcon.ContextMenuClose += onClose;
            NIcon.ContextMenuConnect += ConnectPortContext_Click;

            //timer = new System.Timers.Timer();
            timer = new System.Windows.Threading.DispatcherTimer();

            //timer.AutoReset = true;
            //timer.Elapsed += TimerElapsed;

            timer.Tick +=TimerElapsed;

            DSender = new src.SerialControl.DataSender();

            string[] lstSpd = { "300", "1200", "2400", "4800", "9600", "19200", "38400" }; 
            comboBoxSPD.ItemsSource = lstSpd;
            comboBoxSPD.SelectedIndex = 4;

            if (SerialPort.GetPortNames().Length > 0)
            {
                comboBoxPort.ItemsSource = SerialPort.GetPortNames();
                comboBoxPort.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет доступных портов");
            }
        }

        private void ConnectPort_Click(object sender, RoutedEventArgs e)
        { Connect(); }
        private void ConnectPortContext_Click(object sender, EventArgs e) 
        { Connect(); }

        private void Connect() 
        {
            if (DSender.IsConnect())
            { 
                DSender.Disconnect();
                timer.Stop();
            }
            else
            {
                try
                {
                    //string portName = comboBoxPort.SelectedValue.ToString();
                    //SPort.ErrorReceived
                    
                    string portName = comboBoxPort.Text;
                    int baudRate = int.Parse(comboBoxSPD.Text);

                    if (DSender.BaudRate != baudRate || DSender.PortName != portName)
                    {
                        DSender.SetBaudRate(baudRate);
                        DSender.SetPortName(portName);
                        //DSender = new src.SerialControl.DataSender(portName, baudRate);
                    }
                    DSender.Connect();

                    if ((bool)TimerCheckBox.IsChecked)
                    {
                        timer.Start();
                        TimerElapsed(timer, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }

            try
            { setConnectGuiState(DSender.IsConnect()); }
            catch (InvalidOperationException ex)
            { }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void setConnectGuiState(bool state)
        {
            try
            {
                if (state)
                {
                    string portName = comboBoxPort.Text;
                    int baudRate = int.Parse(comboBoxSPD.Text);

                    CurPortLabel.Content = portName + " " + baudRate.ToString();
                    ConnectPortButton.Content = "Отключиться";
                }
                else
                {
                    CurPortLabel.Content = "Disconnect";
                    ConnectPortButton.Content = "Подключиться";
                }
                NIcon.SetIcon(DSender.IsConnect());
                LockEditPortField(!state);
            }
            catch { throw; }
        }

        private void LockEditPortField(bool state) 
        {
            comboBoxSPD.IsEnabled    = state;
            comboBoxPort.IsEnabled   = state;
            textBoxSender.IsEnabled = !state;
            //TimerCheckBox.IsEnabled = !state;
        }

        // Отправка сообщения через клавишу Enter
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && DSender.IsConnect()) 
            {
                for (int i = 0; i < textBoxSender.Text.Length; i++) 
                {
                    //DSender.Send(textBoxSender.Text[i].ToString());
                }
                textBoxSender.Clear();
            }
        }

        // Запуск/Остановка таймера
        private void TimerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (DSender.IsConnect())
            {
                try
                {
                    timer.Interval = TimeSpan.FromSeconds(Convert.ToInt32(((ComboBoxItem)TimeCountComboBox.SelectedItem).Content));
                }
                catch (Exception ex)
                {
                    timer.Interval = TimeSpan.FromSeconds(60);
                    MessageBox.Show("Ошибка: " + ex.Message + "\nИнтервал установлен в 60 сек.");
                }

                if ((bool)TimerCheckBox.IsChecked)
                {
                    SendCurTime();
                    timer.Start();
                }
                else
                    timer.Stop();
            }
        }

        private void TimerElapsed(object sender, EventArgs e) 
        {
            SendCurTime();

            if ((bool)TimerCheckBox.IsChecked) 
            { timer.Start(); }
        }

        private void SendCurTime() 
        {
            try
            {
                List<PageEl> page_el = new List<PageEl>();

                page_el.Add(new src.APage.PageElements.PageTime(0, 0, AColors.WHITE, 7));

                src.APage.APage page = new src.APage.APage(
                    "name",
                    0,
                    page_el
                    );

                DSender.Send(page);

                Title = System.DateTime.Now.ToLongTimeString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки: " + ex.Message);
                timer.Stop();
            }
            
        }

        private void PageSettingButton_Click(object sender, RoutedEventArgs e)
        {
            PEWindow = new window.PageEditorWindow();
            PEWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            PEWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NIcon.Dispose();
        }

        private void Button_sendClear_Click(object sender, RoutedEventArgs e)
        {
            string send = "";
            
            send += (char)((byte)(TPageEl.ClearCode));
            send += (char)(0);
            DSender.Send(send);
        }
    }
}
