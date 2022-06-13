using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioEqualizer.MVVM.Model
{
    class DeviceDataModel
    {
        public SByte Volume { get; set; }
        public SByte Bass { get; set; }
        public SByte Treble { get; set; }
        public bool Mute { get; set; }


        public Byte Gain { get; set; }
        public SByte Balance { get; set; }
        public Byte inAudio { get; set; }
        public Byte Brightness { get; set; }
        public Byte TimeSleep { get; set; }
        public bool PercentVolume { get; set; }

        public string Version { get; set; }
    }
}
