using AudioEqualizer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioEqualizer.Core
{
    class ComMessenger
    {
        public static ComMessenger Default { get; } = new ComMessenger();
        protected SerialPort serialPort = new SerialPort();

        private bool connected;
        private bool checkDevice;

        public ComMessenger()
        {
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.ErrorReceived += SerialPort_ErrorReceived;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = false;
            serialPort.DtrEnable = true;
            serialPort.BaudRate = 9600;

            serialPort.ReadTimeout = 5000;
            serialPort.WriteTimeout = 2500;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //if (serialPort.IsOpen)
            //{
            if (checkDevice)
            {
                string data = (sender as SerialPort).ReadLine();

                if (data.Contains("OK"))
                {
                    connected = true;
                    checkDevice = false;
                }
                else
                {
                    connected = false;
                }
            }
            else
            {
                byte[] data = new byte[9];
                serialPort.Read(data, 0, 9);

                Messenger.Default.Send<DeviceDataModel>(new DeviceDataModel() 
                {
                    Volume = (SByte)data[0],
                    Bass = (SByte)data[1],
                    Treble = (SByte)data[2],
                    Mute = data[3] == 0 ? false : true,
                    Gain = (Byte)data[4],
                    Balance = (SByte)data[5],
                    inAudio = (Byte)data[6],
                    Brightness = (Byte)data[7],
                    TimeSleep = (Byte)data[8],
                    Version = serialPort.ReadExisting()
                });
            }
            //}
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) { }

        public DeviceDataModel GetAllDataFromDevice()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { 0xAA }, 0, 1);
                }
            }
            catch (TimeoutException) { }
            catch { connected = false; }

            return null;
        }

        public string GetVersionFirmware()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { 0x21 }, 0, 1);
                    return serialPort.ReadExisting();
                }
            }
            catch (TimeoutException) { }
            catch
            { connected = false; }

            return "";
        }

        public SByte GetFromDeviceS(Byte addr)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { addr, 127 }, 0, 2);
                    return (SByte)serialPort.ReadByte();
                }
            }
            catch (TimeoutException) { }
            catch
            { connected = false; }

            return 0;
        }

        public Byte GetFromDevice(Byte addr)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { addr, 255 }, 0, 2);
                    return (Byte)serialPort.ReadByte();
                }
            }
            catch (TimeoutException) { }
            catch
            { connected = false; }

            return 0;
        }

        public void SendToDevice(Byte addr, SByte data)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { addr, (byte)((byte)data | (data < 0 ? (byte)0b_1000_0000 : (byte)0b_0)) }, 0, 2);
                }
            }
            catch { connected = false; }
        }

        public void SendToDevice(Byte addr, Byte data)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(new byte[] { addr, data }, 0, 2);
                }
            }
            catch { connected = false; }
        }


        async public Task<bool> CheckDevice()
        {
            checkDevice = false;

            foreach (string com_port in SerialPort.GetPortNames())
            {
                Console.WriteLine($"Find: {com_port}");
                try
                {
                    serialPort.PortName = com_port;
                    try
                    {
                        serialPort.Close();
                    }
                    catch { }

                    
                    try
                    {
                        serialPort.Open();
                    }
                    catch (Exception e)
                    {
                        serialPort.Close();
                        Console.WriteLine(e.Message.ToString());
                        continue;
                    }

                    await Task.Delay(2300);

                    checkDevice = true;

                    try
                    {
                        serialPort.Write(new byte[] { 0x20 }, 0, 1);
                    }
                    catch
                    {
                        serialPort.Close();
                        continue;
                    }

                    await Task.Delay(200);

                    if (connected)
                    {
                        return true;
                    }
                    else serialPort.Close();
                }
                catch { }
            }

            return false;
        }

        public bool DeviceConnected()
        {
            bool connect = false;

            foreach (var com in SerialPort.GetPortNames())
            {
                if (serialPort.PortName == com)
                {
                    connect = true;
                    break;
                }
            }
            if (!connect)
            {
                serialPort.Close();
                connected = false;
            }

            //if (connect && connected)
            //{
            //    serialPort.DataReceived -= SerialPort_DataReceived;
            //}
            //else serialPort.DataReceived += SerialPort_DataReceived;

            return connect && connected;
        }
    }
}
