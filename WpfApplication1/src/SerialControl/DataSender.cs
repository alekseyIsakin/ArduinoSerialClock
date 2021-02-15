using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using ArdClock.src.HelpingClass;
using ArdClock.src.ArdPage;
using ArdClock.src.ArdPage.PageElements;

namespace ArdClock.src.SerialControl
{
    class DataSender
    {
        public int BaudRate{ get; private set; }
        public string PortName{ get; private set; }
        private SerialPort SPort;

        public DataSender() : this("None", 300) { }
 
        public DataSender(string portName, int baudRate) 
        {

            SPort = new SerialPort();

            this.PortName = portName;
            this.BaudRate = baudRate;

            try
                { SPort = new SerialPort(this.PortName, this.BaudRate); }
            catch
                { throw; }

            SPort.Parity = Parity.None;
            SPort.StopBits = StopBits.One;
            SPort.DataBits = 8;
            SPort.Handshake = Handshake.None;
            SPort.RtsEnable = true;
        }
        public bool IsConnect() { return SPort.IsOpen; }

        public void SetPortName(string PortName) { SPort.PortName = PortName; }
        public void SetBaudRate(int BaudRate) { SPort.BaudRate = BaudRate; }

        public void Connect() { SPort.Open(); }
        public void Disconnect() { SPort.Close(); }

        public void Send(src.ArdPage.APage page)
        {
            List<byte> arrOut = new List<byte>();
            arrOut = page.GenSendData();

            if (arrOut.Count == 0)
                return;

            try
            {
                SPort.Write(arrOut.ToArray(), 0, arrOut.Count);
            }
            catch
            {
                throw;
            }
        }

        public void SetReadyToSend() 
        {
        
        }
        public void Send(string txt1)
        {
            PageString ps1 = new PageString(0, 0, new AColor(), 7, txt1);

            List<byte> arrOut1 = ps1.GenSendData();

            string s1 = "";
            
            foreach (var c in arrOut1.ToArray()) 
                s1 += (Char)(c);

            try
                {
                    //System.Windows.Forms.MessageBox.Show(sd + "\n" + s1 + "\n" + s2);
                    SPort.Write(s1);
                }
            catch 
                { throw; }
        }

        public void SendClearCode() 
        {
            string send = "";

            send += (char)((byte)(TPageEl.ClearCode));
            send += (char)(0);
            try 
            {
                SPort.Write(send);
            }
            catch
            { throw; }
        }
    }
}
