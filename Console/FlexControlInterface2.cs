//=================================================================
// FlexControlInterface1.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2003-2013  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

// For BASIC FC operation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flex.Control;
using System.Diagnostics;
namespace PowerSDR
{
    public class FlexControlInterface2
    {
        private Console console = null;

        private FlexControl flexControl = null;
        public FlexControl FlexControl
        {
            get { return flexControl; }
            set
            {
                if (flexControl != null)  // unwire the FC
                {
                    flexControl.KnobRotated -= new FlexControl.KnobRotatedEventHandler(FlexControl_KnobRotated);
                    flexControl.ButtonClicked -= new FlexControl.ButtonClickedEventHandler(FlexControl_ButtonClicked);
                    flexControl.SetLEDStatus(false, false, false);
                }

                flexControl = value;

                if (flexControl != null)  //wire up the FC
                {
                    flexControl.KnobRotated += new FlexControl.KnobRotatedEventHandler(FlexControl_KnobRotated);
                    flexControl.ButtonClicked += new FlexControl.ButtonClickedEventHandler(FlexControl_ButtonClicked);
                    //flexControl.SetLEDStatus(false, false, false);
                    flexControl.SetLEDStatus(
                        last_button_clicked == FlexControl.Button.Left,
                        last_button_clicked == FlexControl.Button.Middle,
                        last_button_clicked == FlexControl.Button.Right);
                }
            }
        }

        public FlexControlInterface2(Console c)
        {
            console = c;
        }

        public void Cleanup()
        {
            if (flexControl == null) return;

            try
            {
                flexControl.SetLEDStatus(false, false, false);
            }
            catch { }
        }

        private bool tuning_acceleration = true;
        public bool TuningAcceleration
        {
            get { return tuning_acceleration; }
            set { tuning_acceleration = value; }
        }

        private FlexControlKnobFunction current_knob_function = FlexControlKnobFunction.TuneVFOA;

        private FlexControlKnobFunction button_left_function = FlexControlKnobFunction.TuneRIT;
        public FlexControlKnobFunction ButtonLeftFunction
        {
            get { return button_left_function; }
            set
            {
                button_left_function = value;
                if (last_button_clicked == FlexControl.Button.Left)
                    current_knob_function = value;
            }
        }

        private FlexControlKnobFunction button_mid_function = FlexControlKnobFunction.TuneAF;
        public FlexControlKnobFunction ButtonMidFunction
        {
            get { return button_mid_function; }
            set
            {
                button_mid_function = value;
                if (last_button_clicked == FlexControl.Button.Middle)
                    current_knob_function = value;
            }
        }

        private FlexControlKnobFunction button_right_function = FlexControlKnobFunction.TuneVFOB;
        public FlexControlKnobFunction ButtonRightFunction
        {
            get { return button_right_function; }
            set
            {
                button_right_function = value;
                if (last_button_clicked == FlexControl.Button.Right)
                    current_knob_function = value;
            }
        }

        private FlexControlKnobFunction button_knob_function = FlexControlKnobFunction.TuneVFOA;
        public FlexControlKnobFunction ButtonKnobFunction
        {
            get { return button_knob_function; }
            set
            {
                button_knob_function = value;
                if (last_button_clicked == FlexControl.Button.Knob)
                    current_knob_function = value;
            }
        }

        private FlexControl.Button last_button_clicked = FlexControl.Button.Knob;

        /// <summary>
        /// Gets the appropraite Tune Step based on acceleration settings
        /// </summary>
        /// <param name="steps">How many input steps -- translates into bumping the 
        /// Tune Step if acceleration is turned on.</param>
        /// <returns>The Tune Step in Hz</returns>
        private int GetTuneStep(int steps)
        {

            if (tuning_acceleration && steps > 1)
            {
                int index = console.TuneStepIndex; // get current step index

                if (console.setupForm.chkBoxIND2.Checked == true)
                {
                    index = console.TuneStepIndex2; // get current step index
                }
                int var_index = index + (steps = 1); // increment the step index based on accerated input

                if (var_index > console.TuneStepList.Count - 1) // cap at the top index
                    var_index = console.TuneStepList.Count - 1;

                return console.TuneStepList[var_index].StepHz;
            }
            else
            {
                if (console.setupForm.chkBoxIND2.Checked == true)
                {
                    return console.CurrentTuneStepHz2;
                }
                return console.CurrentTuneStepHz;
            }

        } // GetTuneStep(int steps)



        public void FlexControl_KnobRotated(FlexControl.RotateDirection dir, int num_steps)
        {
            if (console == null) return;

            int step = GetTuneStep(num_steps);

            if (num_steps > 1 && tuning_acceleration) num_steps = 1;

            switch (current_knob_function)
            {
                case FlexControlKnobFunction.TuneVFOA:
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:

                            if (Console.CTUN == true)
                            {
                                if ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL) || (Display.CurrentDisplayMode == DisplayMode.PANASCOPE) || (Display.CurrentDisplayMode == DisplayMode.WATERFALL))
                                {
                                    Console.UPDATEOFF = 2; // ke9ns add let system know not to update screen for a little while pan
                                }

                                double temp1 = console.SnapTune(0.0, step, num_steps); // in mhz
                                Console.CTUN1_HZ = Console.CTUN1_HZ + (long)(temp1 * 1e6);// ke9ns add allow bandpass window to scroll across display instead of display freq scroll under bandpass.
                                console.tempVFOAFreq = console.VFOAFreq + temp1; // vfoafreq in mhz
                                console.CalcDisplayFreq(); // ke9ns keep display from moving

                            }
                            else // CTUN == false
                            {
                                console.VFOAFreq = console.SnapTune(console.VFOAFreq, step, num_steps);
                            }

                            break;
                        case FlexControl.RotateDirection.CounterClockwise:

                            if (Console.CTUN == true)
                            {
                                if ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL) || (Display.CurrentDisplayMode == DisplayMode.PANASCOPE) || (Display.CurrentDisplayMode == DisplayMode.WATERFALL))
                                {
                                    Console.UPDATEOFF = 2; // ke9ns add let system know not to update screen for a little while pan
                                }

                                double temp1 = console.SnapTune(0.0, step, -num_steps); // in mhz
                                Console.CTUN1_HZ = Console.CTUN1_HZ + (long)(temp1 * 1e6);// ke9ns add allow bandpass window to scroll across display instead of display freq scroll under bandpass.
                                console.tempVFOAFreq = console.VFOAFreq + temp1; // vfoafreq in mhz
                                console.CalcDisplayFreq(); // ke9ns keep display from moving

                            }
                            else // CTUN == false
                            {
                                console.VFOAFreq = console.SnapTune(console.VFOAFreq, step, -num_steps);
                            }

                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneVFOB:
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.VFOBFreq = console.SnapTune(console.VFOBFreq, step, num_steps);
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.VFOBFreq = console.SnapTune(console.VFOBFreq, step, -num_steps);
                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneVFOASub:
                    if (console.CurrentModel != Model.FLEX5000 || !console.RX2Enabled) return;
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.VFOASubFreq = console.SnapTune(console.VFOASubFreq, step, num_steps);
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.VFOASubFreq = console.SnapTune(console.VFOASubFreq, step, -num_steps);
                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneRIT:
                    if (!console.RITOn) console.RITOn = true;
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.RITValue += num_steps * 2;
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.RITValue -= num_steps * 2;
                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneXIT:
                    if (!console.XITOn) console.XITOn = true;
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.XITValue += num_steps * 2;
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.XITValue -= num_steps * 2;
                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneAF:
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.AF += num_steps;
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.AF -= num_steps;
                            break;
                    }
                    break;
                case FlexControlKnobFunction.TuneAGCT:
                    switch (dir)
                    {
                        case FlexControl.RotateDirection.Clockwise:
                            console.RF += num_steps;
                            break;
                        case FlexControl.RotateDirection.CounterClockwise:
                            console.RF -= num_steps;
                            break;
                    }
                    break;
                case FlexControlKnobFunction.None:
                    //do nothing
                    break;
            }
        }

        public void FlexControl_ButtonClicked(FlexControl.Button button, FlexControl.ClickType type)
        {
            FlexControlKnobFunction function = FlexControlKnobFunction.None;

            switch (button)
            {
                case FlexControl.Button.Left: function = button_left_function; break;
                case FlexControl.Button.Middle: function = button_mid_function; break;
                case FlexControl.Button.Right: function = button_right_function; break;
                case FlexControl.Button.Knob: function = button_knob_function; break;
            }

            if (function == FlexControlKnobFunction.TuneRIT && type == FlexControl.ClickType.Double)
            {
                console.RITValue = 0;
                console.RITOn = false;
            }
            else if (function == FlexControlKnobFunction.TuneXIT && type == FlexControl.ClickType.Double)
            {
                console.XITValue = 0;
                console.XITOn = false;
            }
            else
            {
                current_knob_function = function;
                last_button_clicked = button;
                flexControl.SetLEDStatus(
                    button == FlexControl.Button.Left,
                    button == FlexControl.Button.Middle,
                    button == FlexControl.Button.Right);
            }
        }
    }
}
