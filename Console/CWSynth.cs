

using System;
using System.Diagnostics;

namespace PowerSDR
{
    public class CWSynth
    {
        private enum ToneState
        {
            ToneOff,
            RampUp,
            ToneOn,
            RampDown
        }

        private static int pitch = 600;

        private static int sample_rate = 48000;

        private static double ramp_time = 5.0;

        private static double image_gain = 1.0;

        private static double image_phase = 0.0;

        private static ToneState current_state = ToneState.ToneOff;

        private static int ramp_count = 0;

        private static double running_time = 0.0;

        private static double last_callback_time = 0.0;

        private static double sample_duration = 1.0 / (double)sample_rate * 1000.0;

        private static int ramp_samples = 240;

        private static double phase_accumulator = 0.0;

        private static double phase_step = 0.0;

        private static CWToneItem next_item = null;

        public static int Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                UpdateRampVars();
            }
        }

        public static int SampleRate
        {
            get
            {
                return sample_rate;
            }
            set
            {
                sample_rate = value;
                UpdateRampVars();
            }
        }

        public static double RampTime
        {
            get
            {
                return ramp_time;
            }
            set
            {
                ramp_time = value;
                UpdateRampVars();
            }
        }

        public static double ImageGain
        {
            get
            {
                return image_gain;
            }
            set
            {
                image_gain = 1.0 + 0.001 * value;
            }
        }

        public static double ImagePhase
        {
            get
            {
                return image_phase;
            }
            set
            {
                image_phase = 0.001 * value;
            }
        }

        [Conditional("FlexCWDebugSynth")]
        private static void DebugWriteLine(string s)
        {
        }

        [Conditional("FlexCWDebugSynth")]
        private static void DebugWrite(string s)
        {
        }

        private static void UpdateRampVars()
        {
            ramp_samples = (int)(0.5 + ramp_time * 0.001 * (double)sample_rate);
            phase_step = (double)pitch / (double)sample_rate * 2.0 * Math.PI;
            sample_duration = 1.0 / (double)sample_rate * 1000.0;
        }

        private static void GetQuadratureSample(out float l_sample, out float r_sample)
        {
            double num = Math.Cos(phase_accumulator);
            double num2 = Math.Sin(phase_accumulator);
            phase_accumulator += phase_step;
            l_sample = (float)(num + image_phase * num2);
            r_sample = (float)(num2 * image_gain);
        }

        private static void ChangeCurrentState(ToneState state)
        {
            current_state = state;
        }

        public unsafe static void Advance(float* buf_l, float* buf_r, int num_samples, double current_time)
        {
            running_time = current_time;
            for (int i = 0; i < num_samples; i++)
            {
                if (next_item == null && CWKeyer.ToneQueueCount() > 0)
                {
                    next_item = CWKeyer.ToneDequeue();
                }

                if (next_item != null && running_time >= next_item.Time)
                {
                    if (next_item.State)
                    {
                        ChangeCurrentState(ToneState.RampUp);
                    }
                    else if (!next_item.State)
                    {
                        ChangeCurrentState(ToneState.RampDown);
                    }

                    if (CWKeyer.ToneQueueCount() > 0)
                    {
                        next_item = CWKeyer.ToneDequeue();
                    }
                    else
                    {
                        next_item = null;
                    }
                }

                switch (current_state)
                {
                    case ToneState.ToneOff:
                        buf_l[i] = (buf_r[i] = 0f);
                        break;
                    case ToneState.RampUp:
                        if (ramp_count++ < ramp_samples)
                        {
                            GetQuadratureSample(out buf_l[i], out buf_r[i]);
                            float num2 = (float)Math.Sin((double)ramp_count * (1.0 / (double)(ramp_samples - 1)) * Math.PI * 0.5);
                            buf_l[i] *= num2;
                            buf_r[i] *= num2;
                        }
                        else
                        {
                            GetQuadratureSample(out buf_l[i], out buf_r[i]);
                            ChangeCurrentState(ToneState.ToneOn);
                        }

                        break;
                    case ToneState.ToneOn:
                        GetQuadratureSample(out buf_l[i], out buf_r[i]);
                        break;
                    case ToneState.RampDown:
                        if (ramp_count-- >= 0)
                        {
                            GetQuadratureSample(out buf_l[i], out buf_r[i]);
                            float num = (float)Math.Sin((double)ramp_count * (1.0 / (double)(ramp_samples - 1)) * Math.PI * 0.5);
                            buf_l[i] *= num;
                            buf_r[i] *= num;
                        }
                        else
                        {
                            buf_l[i] = (buf_r[i] = 0f);
                            ChangeCurrentState(ToneState.ToneOff);
                        }

                        break;
                }

                running_time += sample_duration;
            }

            last_callback_time = current_time;
        }
    }
}

