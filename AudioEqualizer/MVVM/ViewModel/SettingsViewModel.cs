using AudioEqualizer.Core;
using AudioEqualizer.MVVM.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AudioEqualizer.MVVM.ViewModel
{
    class SettingsViewModel : ObservableObject
    {
        private SettingsModel settingsModel = new SettingsModel();

        private Thread FindDeviceCOM;
        private Thread CheckDeviceConnected;

        public RelayCommand SaveEEPROMClick { get; set; }

        public bool Connected
        {
            get { return settingsModel.connected; }
            set { if (value != settingsModel.connected) { settingsModel.connected = value; OnPropertyChanged("Connected"); Messenger.Default.Send<bool>(value); } }
        }

        public string Version
        {
            get { return settingsModel.Version; }
            set { if (settingsModel.Version != value) { settingsModel.Version = value; OnPropertyChanged("Version"); } }
        }

        public Byte Gain
        {
            get { return settingsModel.Gain.Value; }
            set { if (settingsModel.Gain.Value != value) { settingsModel.Gain.Value = value; OnPropertyChanged("Gain"); 
                    ComMessenger.Default.SendToDevice(0x26, settingsModel.Gain.Value);
                }
            }
        }

        public SByte Balance
        {
            get { return settingsModel.Balance.Value; }
            set 
            { 
                if (settingsModel.Balance.Value != value) 
                { 
                    settingsModel.Balance.Value = value;

                    if (value > 0)
                    {
                        BalanceSelEnd = value;
                    }
                    else if (value == 0)
                    {
                        BalanceSelEnd = 0;
                        BalanceSelStart = 0;
                    }
                    else
                    {
                        BalanceSelStart = value;
                    }

                    ComMessenger.Default.SendToDevice(0x27, settingsModel.Balance.Value);

                    OnPropertyChanged("Balance"); 
                } 
            }
        }

        public SByte BalanceSelStart
        {
            get { return settingsModel.Balance.SelStart; }
            set { if (settingsModel.Balance.SelStart != value) { settingsModel.Balance.SelStart = value; OnPropertyChanged("BalanceSelStart"); } }
        }
        public SByte BalanceSelEnd
        {
            get { return settingsModel.Balance.SelEnd; }
            set { if (settingsModel.Balance.SelEnd != value) { settingsModel.Balance.SelEnd = value; OnPropertyChanged("BalanceSelEnd"); } }
        }

        public Byte Brightness
        {
            get { return settingsModel.Brightness.Value; }
            set { if (settingsModel.Brightness.Value != value) { settingsModel.Brightness.Value = value; OnPropertyChanged("Brightness"); 
                    ComMessenger.Default.SendToDevice(0x29, settingsModel.Brightness.Value);
                }
            }
        }

        public Byte TimeSleep
        {
            get { return settingsModel.TimeSleep.Value; }
            set { if (settingsModel.TimeSleep.Value != value) { settingsModel.TimeSleep.Value = value; OnPropertyChanged("TimeSleep");
                    ComMessenger.Default.SendToDevice(0x30, settingsModel.TimeSleep.Value);
                }
            }
        }

        public int InAudio
        {
            get { return settingsModel.InAudio; }
            set { if (settingsModel.InAudio != value) { settingsModel.InAudio = value; OnPropertyChanged("InAudio");
                    ComMessenger.Default.SendToDevice(0x28, (byte)settingsModel.InAudio);
                } }
        }



        public SettingsViewModel()
        {
            CheckDeviceConnected = new Thread(() => CheckDeviceConnect());
            FindDeviceCOM = new Thread(() => CheckDeviceLoop());

            CheckDeviceConnected.Start();

            SaveEEPROMClick = new RelayCommand(o =>
            {
                ComMessenger.Default.SendToDevice(0x31,(byte)0x00);
            });

            Messenger.Default.Register<DeviceDataModel>(data =>
            {
                Gain = data.Gain;
                Balance = data.Balance;
                Brightness = data.Brightness;
                TimeSleep = data.TimeSleep;
                InAudio = data.inAudio;
                Version = data.Version;
            });
        }

        async private void CheckDeviceConnect()
        {
            for (; ; )
            {
                Connected = ComMessenger.Default.DeviceConnected();

                if (FindDeviceCOM.ThreadState == ThreadState.Unstarted) FindDeviceCOM.Start();

                Messenger.Default.Send<bool>(Connected);

                await Task.Delay(500);
            }
        }

        async private void CheckDeviceLoop()
        {
            for (; ; )
            {
                await Task.Delay(5000);

                if (!Connected)
                {
                    var connect = await ComMessenger.Default.CheckDevice();

                    if (connect)
                    {
                        ComMessenger.Default.GetAllDataFromDevice();
                    }
                }

                await Task.Delay(5000);
            }
        }
    }
}
