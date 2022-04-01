#region Assembly FlexCW, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// C:\Users\RADIO\source\PowerSDR_v2.8.0\Source\bin\Release\FlexCW.dll
// Decompiled with ICSharpCode.Decompiler 6.1.0.5902
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlexCW
{
    public class CWKeyer
    {
        public enum IambicMode
        {
            ModeA,
            ModeB,
            ModeBStrict,
            Bug
        }

        private enum CWKeyerState
        {
            Idle,
            Dot,
            DotTimeout,
            Dash,
            DashTimeout,
            CharSpace,
            StraightKeyToneOn
        }

        private static Queue<CWSensorItem> sensor_queue = new Queue<CWSensorItem>();

        private static Queue<CWToneItem> tone_queue = new Queue<CWToneItem>();

        private static Queue<CWPTTItem> ptt_queue = new Queue<CWPTTItem>();

        private static Queue<CWMuteItem> mute_queue = new Queue<CWMuteItem>();

        private static int wpm = 25;

        private static int weight = 50;

        private static bool auto_char_space = false;

        private static bool iambic = true;

        private static IambicMode current_iambic_mode = IambicMode.ModeB;

        private static bool break_in = true;

        private static double break_in_delay = 300.0;

        private static double hw_key_down_delay = 20.0;

        private static double min_qsk_time = 5.0;

        private static double audio_latency = 12.0;

        private static CWKeyerState current_state = CWKeyerState.Idle;

        private static double space_length = 60.0;

        private static double dot_length = 60.0;

        private static double dash_length = 180.0;

        private static bool dot_state = false;

        private static bool dash_state = false;

        private static bool key_state = false;

        private static double current_token_begin_time = 0.0;

        private static double current_state_timeout = 0.0;

        private static double callback_time = 0.0;

        private static double last_callback_time = 0.0;

        public static int WPM
        {
            get
            {
                return wpm;
            }
            set
            {
                wpm = value;
                CalculateLengths();
            }
        }

        public static int Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                CalculateLengths();
            }
        }

        public static bool AutoCharSpace
        {
            get
            {
                return auto_char_space;
            }
            set
            {
                auto_char_space = value;
            }
        }

        public static bool Iambic
        {
            get
            {
                return iambic;
            }
            set
            {
                iambic = value;
            }
        }

        public static IambicMode CurrentIambicMode
        {
            get
            {
                return current_iambic_mode;
            }
            set
            {
                current_iambic_mode = value;
            }
        }

        public static bool BreakIn
        {
            get
            {
                return break_in;
            }
            set
            {
                break_in = value;
            }
        }

        public static double BreakInDelay
        {
            get
            {
                return break_in_delay;
            }
            set
            {
                break_in_delay = value;
            }
        }

        public static double HWKeyDownDelay
        {
            get
            {
                return hw_key_down_delay;
            }
            set
            {
                hw_key_down_delay = value;
            }
        }

        public static double MinQSKTime
        {
            get
            {
                return min_qsk_time;
            }
            set
            {
                min_qsk_time = value;
            }
        }

        public static double AudioLatency
        {
            get
            {
                return audio_latency;
            }
            set
            {
                audio_latency = value;
            }
        }

        private static int SensorQueueCount()
        {
            return sensor_queue.Count;
        }

        private static void SensorQueueClear()
        {
            sensor_queue.Clear();
        }

        private static CWSensorItem SensorQueuePeek()
        {
            return sensor_queue.Peek();
        }

        public static void SensorQueuePrint()
        {
            foreach (CWSensorItem item in sensor_queue)
            {
                _ = item;
            }

            sensor_queue.Clear();
        }

        public static int ToneQueueCount()
        {
            return tone_queue.Count;
        }

        private static void ToneQueueClear()
        {
            tone_queue.Clear();
        }

        private static CWToneItem ToneQueuePeek()
        {
            return tone_queue.Peek();
        }

        public static void ToneQueuePrint()
        {
            foreach (CWToneItem item in tone_queue)
            {
                _ = item;
            }

            tone_queue.Clear();
        }

        public static void PTTQueuePrint()
        {
            foreach (CWPTTItem item in ptt_queue)
            {
                _ = item;
            }

            ptt_queue.Clear();
        }

        public static int PTTQueueCount()
        {
            return ptt_queue.Count;
        }

        private static void PTTQueueClear()
        {
            ptt_queue.Clear();
        }

        public static CWPTTItem PTTQueuePeek()
        {
            return ptt_queue.Peek();
        }

        public static int MuteQueueCount()
        {
            return mute_queue.Count;
        }

        public static void MuteEnqueue(CWMuteItem item)
        {
            mute_queue.Enqueue(item);
        }

        public static CWMuteItem MuteDequeue()
        {
            return mute_queue.Dequeue();
        }

        public static CWMuteItem MuteQueuePeek()
        {
            return mute_queue.Peek();
        }

        [Conditional("FlexCWDebugKeyer")]
        private static void DebugWriteLine(string s)
        {
        }

        [Conditional("FlexCWDebugKeyer")]
        private static void DebugWrite(string s)
        {
        }

        public static void Reset()
        {
            SensorQueueClear();
            ToneQueueClear();
            PTTQueueClear();
        }

        private static void CalculateLengths()
        {
            space_length = 1200.0 / (double)wpm;
            dot_length = space_length * (double)weight / 50.0;
            dash_length = space_length * (double)weight / 50.0 + space_length * 2.0;
        }

        public static void SensorEnqueue(CWSensorItem item)
        {
            lock (sensor_queue)
            {
                sensor_queue.Enqueue(item);
            }
        }

        private static CWSensorItem SensorDequeue()
        {
            CWSensorItem cWSensorItem;
            lock (sensor_queue)
            {
                cWSensorItem = sensor_queue.Dequeue();
            }

            switch (cWSensorItem.Type)
            {
                case CWSensorItem.InputType.Dot:
                    dot_state = cWSensorItem.State;
                    break;
                case CWSensorItem.InputType.Dash:
                    dash_state = cWSensorItem.State;
                    break;
                case CWSensorItem.InputType.StraightKey:
                    key_state = cWSensorItem.State;
                    break;
            }

            return cWSensorItem;
        }

        private static void ToneEnqueue(CWToneItem item)
        {
            tone_queue.Enqueue(item);
        }

        public static CWToneItem ToneDequeue()
        {
            return tone_queue.Dequeue();
        }

        public static void PTTEnqueue(CWPTTItem item)
        {
            lock (ptt_queue)
            {
                ptt_queue.Enqueue(item);
            }
        }

        public static CWPTTItem PTTDequeue()
        {
            lock (ptt_queue)
            {
                return ptt_queue.Dequeue();
            }
        }

        private static void FlushSensorQueue(double time)
        {
            while (sensor_queue.Count != 0 && sensor_queue.Peek().Time < time)
            {
                SensorDequeue();
            }
        }

        private static void FlushPTTQueue(double time)
        {
            while (ptt_queue.Count != 0 && ptt_queue.Peek().Time < time)
            {
                PTTDequeue();
            }
        }

        private static bool DotAtTime(double time)
        {
            bool state = dot_state;
            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (!(item.Time > time))
                    {
                        if (item.Type == CWSensorItem.InputType.Dot)
                        {
                            state = item.State;
                        }

                        continue;
                    }

                    return state;
                }

                return state;
            }
        }

        private static bool DotWasTrue(double start, double end)
        {
            bool result = false;
            if (DotAtTime(start))
            {
                return true;
            }

            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (item.Time >= start && item.Time <= end && item.Type == CWSensorItem.InputType.Dot && item.State)
                    {
                        result = true;
                    }
                }

                return result;
            }
        }

        private static bool DashAtTime(double time)
        {
            bool state = dash_state;
            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (!(item.Time > time))
                    {
                        if (item.Type == CWSensorItem.InputType.Dash)
                        {
                            state = item.State;
                        }

                        continue;
                    }

                    return state;
                }

                return state;
            }
        }

        private static bool DashWasTrue(double start, double end)
        {
            if (DashAtTime(start))
            {
                return true;
            }

            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (item.Time >= start && item.Time <= end && item.Type == CWSensorItem.InputType.Dash && item.State)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool KeyAtTime(double time)
        {
            bool state = key_state;
            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (!(item.Time > time))
                    {
                        if (item.Type == CWSensorItem.InputType.StraightKey)
                        {
                            state = item.State;
                        }

                        continue;
                    }

                    return state;
                }

                return state;
            }
        }

        private static bool CWXWasFalse(double start, double end)
        {
            if (!KeyAtTime(start))
            {
                return true;
            }

            lock (sensor_queue)
            {
                foreach (CWSensorItem item in sensor_queue)
                {
                    if (item.Time >= start && item.Time <= end && item.Type == CWSensorItem.InputType.StraightKey && !item.State)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void GoToDot(double start_time)
        {
            current_state = CWKeyerState.Dot;
            current_state_timeout = start_time + space_length;
            current_token_begin_time = start_time;
            double num = Math.Max(hw_key_down_delay, audio_latency);
            double time = start_time + num;
            double time2 = start_time + dot_length + num;
            ToneEnqueue(new CWToneItem(_state: true, time));
            ToneEnqueue(new CWToneItem(_state: false, time2));
            if (!break_in)
            {
                return;
            }

            double num2 = Math.Max(0.0, audio_latency - hw_key_down_delay);
            double num3 = Math.Max(0.0, audio_latency + num - break_in_delay);
            double num4 = start_time - hw_key_down_delay + num2;
            double time3 = start_time + dot_length + break_in_delay + num3;
            bool flag = false;
            if (ptt_queue.Count > 0)
            {
                lock (ptt_queue)
                {
                    foreach (CWPTTItem item in ptt_queue)
                    {
                        if (item.Time > num4 - audio_latency - min_qsk_time && !item.State)
                        {
                            item.Ignore = true;
                            flag = true;
                        }
                    }
                }
            }

            if (!flag)
            {
                PTTEnqueue(new CWPTTItem(_state: true, num4));
            }

            PTTEnqueue(new CWPTTItem(_state: false, time3));
        }

        private static void GoToDash(double start_time)
        {
            current_state = CWKeyerState.Dash;
            current_state_timeout = start_time + space_length * 3.0;
            double num = Math.Max(hw_key_down_delay, audio_latency);
            ToneEnqueue(new CWToneItem(_state: true, start_time + num));
            ToneEnqueue(new CWToneItem(_state: false, start_time + dash_length + num));
            current_token_begin_time = start_time;
            if (!break_in)
            {
                return;
            }

            double num2 = Math.Max(0.0, audio_latency - hw_key_down_delay);
            double num3 = Math.Max(0.0, audio_latency + num - break_in_delay);
            double num4 = start_time - hw_key_down_delay + num2;
            double time = start_time + dash_length + break_in_delay + num3;
            bool flag = false;
            if (ptt_queue.Count > 0)
            {
                lock (ptt_queue)
                {
                    foreach (CWPTTItem item in ptt_queue)
                    {
                        if (item.Time > num4 - audio_latency - min_qsk_time && !item.State)
                        {
                            item.Ignore = true;
                            flag = true;
                        }
                    }
                }
            }

            if (!flag)
            {
                PTTEnqueue(new CWPTTItem(_state: true, num4));
            }

            PTTEnqueue(new CWPTTItem(_state: false, time));
        }

        private static void GoToStraightKey(double start_time)
        {
            current_state = CWKeyerState.StraightKeyToneOn;
            double num = Math.Max(hw_key_down_delay, audio_latency);
            tone_queue.Enqueue(new CWToneItem(_state: true, start_time + num));
            if (!break_in)
            {
                return;
            }

            double num2 = Math.Max(0.0, audio_latency - hw_key_down_delay);
            double time = start_time - hw_key_down_delay + num2;
            bool flag = false;
            if (ptt_queue.Count > 0)
            {
                lock (ptt_queue)
                {
                    foreach (CWPTTItem item in ptt_queue)
                    {
                        if (item.Time > start_time - audio_latency - min_qsk_time && !item.State)
                        {
                            item.Ignore = true;
                            flag = true;
                        }
                    }
                }
            }

            if (!flag)
            {
                PTTEnqueue(new CWPTTItem(_state: true, time));
            }
        }

        public static void Advance(double time)
        {
            callback_time = time;
            bool flag = false;
            while (!flag)
            {
                switch (current_state)
                {
                    case CWKeyerState.Idle:
                        if (sensor_queue.Count == 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            if (sensor_queue.Count <= 0)
                            {
                                break;
                            }

                            CWSensorItem cWSensorItem3 = SensorDequeue();
                            switch (cWSensorItem3.Type)
                            {
                                case CWSensorItem.InputType.Dot:
                                    if (cWSensorItem3.State)
                                    {
                                        if (iambic)
                                        {
                                            GoToDot(callback_time);
                                        }
                                        else
                                        {
                                            GoToStraightKey(callback_time);
                                        }
                                    }

                                    break;
                                case CWSensorItem.InputType.Dash:
                                    if (cWSensorItem3.State)
                                    {
                                        if (iambic)
                                        {
                                            GoToDash(callback_time);
                                        }
                                        else
                                        {
                                            GoToStraightKey(callback_time);
                                        }
                                    }

                                    break;
                                case CWSensorItem.InputType.StraightKey:
                                    if (cWSensorItem3.State)
                                    {
                                        GoToStraightKey(callback_time);
                                    }

                                    break;
                            }
                        }

                        break;
                    case CWKeyerState.Dot:
                        if (callback_time >= current_state_timeout)
                        {
                            current_state = CWKeyerState.DotTimeout;
                            current_state_timeout += space_length;
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                    case CWKeyerState.Dash:
                        if (callback_time >= current_state_timeout)
                        {
                            current_state = CWKeyerState.DashTimeout;
                            current_state_timeout += space_length;
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                    case CWKeyerState.DotTimeout:
                        if (callback_time >= current_state_timeout)
                        {
                            bool flag2 = false;
                            bool flag3 = false;
                            bool flag4 = false;
                            switch (current_iambic_mode)
                            {
                                case IambicMode.ModeA:
                                    {
                                        double start = current_token_begin_time + audio_latency;
                                        double end = current_token_begin_time + space_length * 2.0 + audio_latency;
                                        if (DashWasTrue(start, end) && DotWasTrue(start, end) && !DashAtTime(end) && !DotAtTime(end))
                                        {
                                            flag3 = false;
                                            flag4 = true;
                                        }
                                        else if (DashWasTrue(start, end))
                                        {
                                            flag3 = true;
                                        }

                                        break;
                                    }
                                case IambicMode.ModeB:
                                    {
                                        double start = current_token_begin_time + audio_latency;
                                        double end = current_token_begin_time + space_length * 2.0 + audio_latency;
                                        if (DashWasTrue(start, end))
                                        {
                                            flag3 = true;
                                        }

                                        break;
                                    }
                                case IambicMode.ModeBStrict:
                                    {
                                        double start = current_token_begin_time + dot_length * 0.5 + audio_latency;
                                        double end = current_token_begin_time + space_length * 2.0 + audio_latency;
                                        if (DashWasTrue(start, end))
                                        {
                                            flag3 = true;
                                        }

                                        break;
                                    }
                            }

                            if (!flag3)
                            {
                                switch (current_iambic_mode)
                                {
                                    case IambicMode.ModeA:
                                        {
                                            _ = current_token_begin_time;
                                            _ = audio_latency;
                                            double time5 = current_token_begin_time + space_length * 2.0 + audio_latency;
                                            if (flag4)
                                            {
                                                flag2 = false;
                                            }
                                            else if (DotAtTime(time5))
                                            {
                                                flag2 = true;
                                            }

                                            break;
                                        }
                                    case IambicMode.ModeB:
                                        {
                                            double time4 = current_token_begin_time + space_length * 2.0 + audio_latency;
                                            if (DotAtTime(time4))
                                            {
                                                flag2 = true;
                                            }

                                            break;
                                        }
                                    case IambicMode.ModeBStrict:
                                        {
                                            double time4 = current_token_begin_time + space_length * 2.0 + audio_latency;
                                            if (DotAtTime(time4))
                                            {
                                                flag2 = true;
                                            }

                                            break;
                                        }
                                }
                            }

                            if (flag3)
                            {
                                GoToDash(current_state_timeout);
                            }
                            else if (flag2)
                            {
                                GoToDot(current_state_timeout);
                            }
                            else if (auto_char_space)
                            {
                                current_state = CWKeyerState.CharSpace;
                                current_state_timeout += space_length * 2.0;
                            }
                            else
                            {
                                current_state = CWKeyerState.Idle;
                                flag = true;
                            }

                            FlushSensorQueue(current_token_begin_time + space_length + space_length + audio_latency);
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                    case CWKeyerState.DashTimeout:
                        if (callback_time >= current_state_timeout)
                        {
                            bool flag5 = false;
                            bool flag6 = false;
                            bool flag7 = false;
                            switch (current_iambic_mode)
                            {
                                case IambicMode.ModeA:
                                    {
                                        double start2 = current_token_begin_time + audio_latency;
                                        double end2 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                        if (DashWasTrue(start2, end2) && DotWasTrue(start2, end2) && !DashAtTime(end2) && !DotAtTime(end2))
                                        {
                                            flag5 = false;
                                            flag7 = true;
                                        }
                                        else if (DotWasTrue(start2, end2))
                                        {
                                            flag5 = true;
                                        }

                                        break;
                                    }
                                case IambicMode.ModeB:
                                    {
                                        double start2 = current_token_begin_time + audio_latency;
                                        double end2 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                        if (DotWasTrue(start2, end2))
                                        {
                                            flag5 = true;
                                        }

                                        break;
                                    }
                                case IambicMode.ModeBStrict:
                                    {
                                        double start2 = current_token_begin_time + dash_length * 0.5 + audio_latency;
                                        double end2 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                        if (DotWasTrue(start2, end2))
                                        {
                                            flag5 = true;
                                        }

                                        break;
                                    }
                            }

                            if (!flag5)
                            {
                                switch (current_iambic_mode)
                                {
                                    case IambicMode.ModeA:
                                        {
                                            _ = current_token_begin_time;
                                            _ = audio_latency;
                                            double time7 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                            if (flag7)
                                            {
                                                flag6 = false;
                                            }
                                            else if (DashAtTime(time7))
                                            {
                                                flag6 = true;
                                            }

                                            break;
                                        }
                                    case IambicMode.ModeB:
                                        {
                                            double time6 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                            if (DashAtTime(time6))
                                            {
                                                flag6 = true;
                                            }

                                            break;
                                        }
                                    case IambicMode.ModeBStrict:
                                        {
                                            double time6 = current_token_begin_time + space_length * 4.0 + audio_latency;
                                            if (DashAtTime(time6))
                                            {
                                                flag6 = true;
                                            }

                                            break;
                                        }
                                }
                            }

                            if (flag5)
                            {
                                GoToDot(current_state_timeout);
                            }
                            else if (flag6)
                            {
                                GoToDash(current_state_timeout);
                            }
                            else if (auto_char_space)
                            {
                                current_state = CWKeyerState.CharSpace;
                                current_state_timeout += space_length * 2.0;
                            }
                            else
                            {
                                current_state = CWKeyerState.Idle;
                                flag = true;
                            }

                            FlushSensorQueue(current_token_begin_time + space_length * 4.0 + audio_latency);
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                    case CWKeyerState.CharSpace:
                        if (callback_time >= current_state_timeout)
                        {
                            current_state = CWKeyerState.Idle;
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                    case CWKeyerState.StraightKeyToneOn:
                        if (sensor_queue.Count > 0)
                        {
                            if (iambic)
                            {
                                if (DotWasTrue(last_callback_time, callback_time) || DashWasTrue(last_callback_time, callback_time))
                                {
                                    current_state = CWKeyerState.Idle;
                                }
                                else if (CWXWasFalse(last_callback_time, callback_time))
                                {
                                    CWSensorItem cWSensorItem;
                                    do
                                    {
                                        cWSensorItem = SensorDequeue();
                                    }
                                    while (key_state);
                                    double num = Math.Max(hw_key_down_delay, audio_latency);
                                    tone_queue.Enqueue(new CWToneItem(_state: false, cWSensorItem.Time + num));
                                    if (break_in)
                                    {
                                        double num2 = Math.Max(0.0, audio_latency + num - break_in_delay);
                                        double time2 = cWSensorItem.Time + break_in_delay + num2;
                                        PTTEnqueue(new CWPTTItem(_state: false, time2));
                                    }

                                    current_state = CWKeyerState.Idle;
                                }
                                else
                                {
                                    FlushSensorQueue(callback_time);
                                    flag = true;
                                }

                                break;
                            }

                            CWSensorItem cWSensorItem2 = SensorDequeue();
                            if (!dot_state && !dash_state && !key_state)
                            {
                                double num3 = Math.Max(hw_key_down_delay, audio_latency);
                                tone_queue.Enqueue(new CWToneItem(_state: false, cWSensorItem2.Time + num3));
                                if (break_in)
                                {
                                    double num4 = Math.Max(0.0, audio_latency + num3 - break_in_delay);
                                    double time3 = cWSensorItem2.Time + break_in_delay + num4;
                                    PTTEnqueue(new CWPTTItem(_state: false, time3));
                                }

                                current_state = CWKeyerState.Idle;
                            }
                        }
                        else
                        {
                            flag = true;
                        }

                        break;
                }
            }

            last_callback_time = time;
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
