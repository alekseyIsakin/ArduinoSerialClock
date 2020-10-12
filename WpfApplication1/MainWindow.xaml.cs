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

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private System.Timers.Timer timer;
        private src.DataSender DSender;
        private src.NIcon notifyIcon = null;

        //
        // Логика NotifyIcon
        //
        public void notifyIcon_Click(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Left)
            {
                ChangeVisbleWindow();
                
                Topmost = true;
                Topmost = false;
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
        public void onClose(object sender, EventArgs e) { Close(); }
        public void ChangeVisbleWindow() 
        {
            if (Visibility == Visibility.Hidden)
            {
                Visibility = Visibility.Visible;
            }
            else
                Visibility = Visibility.Hidden;
        }
        //
        //
        //

        public MainWindow()
        {
            InitializeComponent();
            
            notifyIcon = new src.NIcon(Icon);
            notifyIcon.Click += notifyIcon_Click;
            notifyIcon.DoubleClick += notifyIcon_Click;

            notifyIcon.ContextMenuClose += onClose;
            notifyIcon.ContextMenuConnect += ConnectPortContext_Click;

            timer = new System.Timers.Timer();

            timer.AutoReset = true;
            timer.Elapsed += TimerElapsed;

            DSender = new src.DataSender();

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
                CurPortLabel.Content = "Disconnect";
                ConnectPortButton.Content = "Подключиться";
            }
            else
            {
                try
                {
                    //string portName = comboBoxPort.SelectedValue.ToString();
                    string portName = comboBoxPort.Text;
                    int baudRate = int.Parse(comboBoxSPD.Text);

                    CurPortLabel.Content = portName + " " + baudRate.ToString();

                    if (DSender.BaudRate != baudRate || DSender.PortName != portName)
                    {
                        DSender.SetBaudRate(baudRate);
                        DSender.SetPortName(portName);
                    }
                    DSender.Connect();
                    ConnectPortButton.Content = "Отключиться";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }

            notifyIcon.SetIcon(DSender.IsConnect());
            LockEditPortField(!(DSender.IsConnect()));
        }
        private void LockEditPortField(bool state) 
        {
            comboBoxSPD.IsEnabled    = state;
            comboBoxPort.IsEnabled   = state;
            textBoxSender.IsEnabled = !state;
            TimerCheckBox.IsEnabled = !state;
        }

        // Отправка сообщения через клавишу Enter
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && DSender.IsConnect()) 
            {
                DSender.Send(textBoxSender.Text);
                textBoxSender.Clear();
            }
        }

        // Запуск/Остановка таймера
        private void TimerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Interval = Convert.ToDouble(((ComboBoxItem)TimeCountComboBox.SelectedItem).Content);
            }
            catch (Exception ex)
            {
                timer.Interval = 60;
                MessageBox.Show("Ошибка: " + ex.Message + "\nИнтервал установлен в 60 сек.");
            }
            timer.Interval *= 1000;

            if ((bool)TimerCheckBox.IsChecked)
            {
                SendCurTime();
                timer.Start();
            }
            else
                timer.Stop();
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e) 
        {
            SendCurTime();
        }

        private void SendCurTime() 
        {
            try
            { DSender.Send(System.DateTime.Now.ToShortTimeString()); }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
                Connect();
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
