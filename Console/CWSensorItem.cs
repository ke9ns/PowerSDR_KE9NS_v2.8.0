#region Assembly FlexCW, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// C:\Users\RADIO\source\PowerSDR_v2.8.0\Source\bin\Release\FlexCW.dll
// Decompiled with ICSharpCode.Decompiler 6.1.0.5902
#endregion

using Timing;

namespace FlexCW
{
    public class CWSensorItem
    {
        public enum InputType
        {
            Dot,
            Dash,
            StraightKey
        }

        private static HiPerfTimer t1 = new HiPerfTimer();

        private double time;

        private InputType type;

        private bool state;

        public double Time => time;

        public InputType Type => type;

        public bool State => state;

        public CWSensorItem(InputType _type, bool _state)
        {
            t1.Stop();
            time = t1.DurationMsec;
            type = _type;
            state = _state;
        }

        public static void Init()
        {
            t1.Start();
        }

        public static double GetCurrentTime()
        {
            t1.Stop();
            return t1.DurationMsec;
        }

        public override string ToString()
        {
            return time.ToString("f1") + ": " + type.ToString() + " " + state;
        }
    }
}
#if false // Decompilation log
'39' items in cache
------------------
Resolve: 'mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll'
------------------
Resolve: 'System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll'
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll'
#endif
