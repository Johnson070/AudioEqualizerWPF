using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioEqualizer.MVVM.Model
{
    sealed class AudioHomeModel
    {
        public DataOfValue<SByte> Volume = new DataOfValue<SByte>(0, -87, 0);
        public DataOfValue<SByte> Bass = new DataOfValue<SByte>(0, 0, 0);
        public DataOfValue<SByte> Treble = new DataOfValue<SByte>(0, 0, 0);

        public bool Mute = false;
        public DataOfValue<Byte> Gain = new DataOfValue<Byte>(0, 0, 0);
        public DataOfValue<SByte> balance = new DataOfValue<SByte>(0, 0, 0);
        public byte InAudio = 0;
        public DataOfValue<Byte> Brightness = new DataOfValue<Byte>(255, 0, 255);
        public DataOfValue<Byte> TimeSleep = new DataOfValue<Byte>(1, 1, 1);

        public string Postfix = "dB";

        public class DataOfValue<T>
        {
            public T Value;
            public T SelStart;
            public T SelEnd;

            public DataOfValue(T _value, T _selStart, T _selEnd)
            {
                Value = _value;
                SelEnd = _selEnd;
                SelStart = _selStart;
            }
        }
    }
}
