using AudioEqualizer.Core;
using AudioEqualizer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioEqualizer.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private AudioHomeModel audioHome = new AudioHomeModel();


        public SByte Volume
        {
            get { return audioHome.Volume.Value; }
            set
            {
                if (audioHome.Volume.Value != value)
                {
                    audioHome.Volume.Value = value;
                    VolumeSelectionEnd = value;

                    ComMessenger.Default.SendToDevice(0x22, audioHome.Volume.Value);

                    OnPropertyChanged("Volume");
                }
            }
        }
        public SByte Bass
        {
            get { return audioHome.Bass.Value; }
            set
            {
                if (audioHome.Bass.Value != value)
                {
                    audioHome.Bass.Value = value;

                    if (value > 0)
                    {
                        BassSelectionEnd = value;
                    }
                    else if (value == 0)
                    {
                       BassSelectionStart = 0;
                       BassSelectionEnd = 0;
                    }
                    else
                    {
                        BassSelectionStart = value;
                    }

                    ComMessenger.Default.SendToDevice(0x23, audioHome.Bass.Value);

                    OnPropertyChanged("Bass");
                }
            }
        }
        public SByte Treble
        {
            get { return audioHome.Treble.Value; }
            set
            {
                if (audioHome.Treble.Value != value)
                {
                    audioHome.Treble.Value = value;

                    if (value > 0)
                    {
                        TrebleSelectionEnd= value;
                    }
                    else if (value == 0)
                    {
                        TrebleSelectionStart = 0;
                        TrebleSelectionEnd = 0;
                    }
                    else
                    {
                        TrebleSelectionStart = value;
                    }

                    ComMessenger.Default.SendToDevice(0x24, audioHome.Treble.Value);

                    OnPropertyChanged("Treble");
                }
            }
        }

        public string Postfix
        {
            get { return audioHome.Postfix; }
            set { if (audioHome.Postfix != value) { audioHome.Postfix = value; OnPropertyChanged("Postfix"); } }
        }
        public SByte VolumeSelectionStart
        {
            get { return audioHome.Volume.SelStart; }
            set { if (audioHome.Volume.SelStart != value) { audioHome.Volume.SelStart = value; OnPropertyChanged("VolumeSelectionStart"); } }
        }
        public SByte VolumeSelectionEnd
        {
            get { return audioHome.Volume.SelEnd; }
            set { if (audioHome.Volume.SelEnd != value) { audioHome.Volume.SelEnd = value; OnPropertyChanged("VolumeSelectionEnd"); } }
        }

        public SByte BassSelectionStart
        {
            get { return audioHome.Bass.SelStart; }
            set { if (audioHome.Bass.SelStart != value) { audioHome.Bass.SelStart = value; OnPropertyChanged("BassSelectionStart"); } }
        }
        public SByte BassSelectionEnd
        {
            get { return audioHome.Bass.SelEnd; }
            set { if (audioHome.Bass.SelEnd != value) { audioHome.Bass.SelEnd = value; OnPropertyChanged("BassSelectionEnd"); } }
        }

        public SByte TrebleSelectionStart
        {
            get { return audioHome.Treble.SelStart; }
            set { if (audioHome.Treble.SelStart != value) { audioHome.Treble.SelStart = value; OnPropertyChanged("TrebleSelectionStart"); } }
        }
        public SByte TrebleSelectionEnd
        {
            get { return audioHome.Treble.SelEnd; }
            set { if (audioHome.Treble.SelEnd != value) { audioHome.Treble.SelEnd = value; OnPropertyChanged("TrebleSelectionEnd"); } }
        }
        public bool Mute
        {
            get { return audioHome.Mute; }
            set { if (audioHome.Mute != value) { audioHome.Mute = value; OnPropertyChanged("Mute"); ComMessenger.Default.SendToDevice(0x25, audioHome.Mute ? (Byte)0x1 : (Byte)0x0); } }
        }

        public RelayCommand ButtonResetClick { get; set; }

        public HomeViewModel()
        {
            Messenger.Default.Register<DeviceDataModel>(data => 
            {
                Volume = data.Volume;
                Bass = data.Bass;
                Treble = data.Treble;
                Mute = data.Mute;
            });

            ButtonResetClick = new RelayCommand(o => 
            {
                Volume = -60;
                Bass = 0;
                Treble = 0;
            });
        }
    }
}
