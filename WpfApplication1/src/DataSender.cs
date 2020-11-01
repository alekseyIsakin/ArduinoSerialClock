using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace WpfApplication1.src
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

        public void Send(src.APage page)
        {
            List<byte> arrOut = new List<byte>();

            try
            {
                SPort.Write(arrOut.ToArray(), 0, arrOut.Count);
            }
            catch
            { 
                throw; 
            }
        }

        public void Send(string txt)
        {
            PageString ps = new PageString(0, 0, 0x001f, 7, txt);
            List<byte> arrOut = ps.GenSendData();
            string s = "";

            foreach (var c in arrOut.ToArray())
                s += (Char)(c);

            try
                { 
                    SPort.Write(s); 
                }
            catch 
                { throw; }
        }
    }
}
