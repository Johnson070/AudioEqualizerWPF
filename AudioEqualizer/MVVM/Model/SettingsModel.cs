using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AudioEqualizer.MVVM.Model.AudioHomeModel;

namespace AudioEqualizer.MVVM.Model
{
    class SettingsModel
    {
        public string Version = "";
        public bool connected;

        public DataOfValue<Byte> Gain = new DataOfValue<Byte>(0, 0, 0);
        public DataOfValue<SByte> Balance = new DataOfValue<SByte>(0, 0, 0);
        public DataOfValue<Byte> Brightness = new DataOfValue<Byte>(255, 0, 255);
        public DataOfValue<Byte> TimeSleep = new DataOfValue<Byte>(1, 1, 1);

        public int InAudio = 0;
    }
}
