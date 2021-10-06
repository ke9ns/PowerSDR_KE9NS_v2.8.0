//=================================================================
// FlexControlForm.cs
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using Flex.Control;
using System.Diagnostics;

namespace PowerSDR
{
    public partial class FlexControlAdvancedForm : Form
    {
        #region Variable Declaration

        private Console console;
        private FlexControlInterface1 fc_interface = null;

        private FlexControlInterface1.KnobMode current_knob_mode = FlexControlInterface1.KnobMode.A1;
        private PanelHighlighter hlKnobA1;
        private PanelHighlighter hlKnobA2;
        private PanelHighlighter hlKnobB1;
        private PanelHighlighter hlKnobB2;
        private PanelHighlighter hlKnobADouble;
        private PanelHighlighter hlKnobBDouble;

        private PanelHighlighter hlLeftSingle;
        private PanelHighlighter hlLeftDouble;
        private PanelHighlighter hlLeftLong;
        private PanelHighlighter hlMidSingle;
        private PanelHighlighter hlMidDouble;
        private PanelHighlighter hlMidLong;
        private PanelHighlighter hlRightSingle;
        private PanelHighlighter hlRightDouble;
        private PanelHighlighter hlRightLong;

        #endregion

        #region Constructor 

        public FlexControlAdvancedForm(Console c)
        {
            InitializeComponent();

            console = c;

            fc_interface = new FlexControlInterface1(c);

            comboModeA1.Items.Clear();
            comboModeA2.Items.Clear();
            comboModeB1.Items.Clear();
            comboModeB2.Items.Clear();

            comboModeADouble.Items.Clear();
            comboModeBDouble.Items.Clear();

            foreach (Control ctrl in grpButton.Controls)
            {
                if (ctrl.GetType() == typeof(ComboBoxTS))
                {
                    ComboBoxTS combo = (ComboBoxTS)ctrl;
                    combo.Items.Clear();
                }
            }

            // populate knob combobox controls
            foreach (FlexControlKnobFunction function in Enum.GetValues(typeof(FlexControlKnobFunction)))
            {
                string s = KnobFunction2String(function);
                comboModeA1.Items.Add(s);
                comboModeA2.Items.Add(s);
                comboModeB1.Items.Add(s);
                comboModeB2.Items.Add(s);
            }

            // populate button combobox controls
            foreach (FlexControlButtonFunction function in Enum.GetValues(typeof(FlexControlButtonFunction)))
            {
                string s = ButtonFunction2String(function);

                comboModeADouble.Items.Add(s);
                comboModeBDouble.Items.Add(s);

                foreach (Control ctrl in grpButton.Controls)
                {
                    if (ctrl.GetType() == typeof(ComboBoxTS))
                    {
                        ComboBoxTS combo = (ComboBoxTS)ctrl;
                        combo.Items.Add(s);
                    }
                }
            }

            SetupDefaults();

            Common.RestoreForm(this, "FlexControlAdvancedForm", false);

            comboModeA1_SelectedIndexChanged(this, EventArgs.Empty);
            comboModeA2_SelectedIndexChanged(this, EventArgs.Empty);
            comboModeB1_SelectedIndexChanged(this, EventArgs.Empty);
            comboModeB2_SelectedIndexChanged(this, EventArgs.Empty);


            fc_interface.CurrentKnobModeChanged += new FlexControlInterface1.KnobModeChanged(UpdateCurrentKnobMode);

            hlKnobA1 = new PanelHighlighter(panelModeA1);
            hlKnobA2 = new PanelHighlighter(panelModeA2);
            hlKnobB1 = new PanelHighlighter(panelModeB1);
            hlKnobB2 = new PanelHighlighter(panelModeB2);
            hlKnobADouble = new PanelHighlighter(panelModeADouble);
            hlKnobBDouble = new PanelHighlighter(panelModeBDouble);

            hlLeftSingle = new PanelHighlighter(panelLeftSingle);
            hlLeftDouble = new PanelHighlighter(panelLeftDouble);
            hlLeftLong = new PanelHighlighter(panelLeftLong);
            hlMidSingle = new PanelHighlighter(panelMidSingle);
            hlMidDouble = new PanelHighlighter(panelMidDouble);
            hlMidLong = new PanelHighlighter(panelMidLong);
            hlRightSingle = new PanelHighlighter(panelRightSingle);
            hlRightDouble = new PanelHighlighter(panelRightDouble);
            hlRightLong = new PanelHighlighter(panelRightLong);
        }

        #endregion

        #region Properties

        private FlexControl flexControl = null;
        public FlexControl FlexControl
        {
            get
            {

                return flexControl;
            }
            set
            {


                if (flexControl != null)
                {
                    flexControl.KnobRotated -= KnobRotated;
                    flexControl.ButtonClicked -= ButtonClicked;
                    flexControl.Disconnected -= FlexControl_Disconnected;
                }

                flexControl = value;

                if (flex_control_mode == FlexControlMode.Advanced)
                {
                    fc_interface.FlexControl = value;

                    if (flexControl != null)
                    {
                        flexControl.KnobRotated += new FlexControl.KnobRotatedEventHandler(KnobRotated);
                        flexControl.ButtonClicked += new FlexControl.ButtonClickedEventHandler(ButtonClicked);
                        flexControl.Disconnected += new FlexControl.DisconnectHandler(FlexControl_Disconnected);
                    }
                }
            }
        }

        void FlexControl_Disconnected(FlexControl flex_control)
        {
            FlexControl = null;
        }

        private bool auto_detect = true;
        public bool AutoDetect
        {
            get { return auto_detect; }
            set
            {
                auto_detect = value;
                if (chkAutoDetect != null && chkAutoDetect.Checked != auto_detect)
                    chkAutoDetect.Checked = value;
            }
        }

        private FlexControlMode flex_control_mode = FlexControlMode.Basic;
        public FlexControlMode FlexControlMode
        {
            get { return flex_control_mode; }
            set
            {
                flex_control_mode = value;
                switch (flex_control_mode)
                {
                    case FlexControlMode.Basic:
                        radModeBasic.Checked = true;
                        break;
                    case FlexControlMode.Advanced:
                        radModeAdvanced.Checked = true;
                        break;
                }
            }
        }

        #endregion

        #region Routines

        private void SetupDefaults()
        {
            chkAutoDetect.Checked = true;

            // set knob defaults
            comboModeA1.Text = "Tune VFO A";
            comboModeA2.Text = "Tune RIT";
            comboModeB1.Text = "Tune VFO B";
            comboModeB2.Text = "Tune XIT";

            // set button defaults
            comboModeADouble.Text = "Toggle TX VFO";
            comboModeBDouble.Text = "Swap VFOs";

            comboLeftSingle.Text = "CW Speed Down";
            comboLeftDouble.Text = "CW Speed Up";
            comboLeftLong.Text = "Copy VFO A to B";
            comboMidSingle.Text = "Tune Step Down";  //used to be "CWX Speed Up"
            comboMidDouble.Text = "Tune Step Up"; //used to be "CWX Speed down"
            comboMidLong.Text = "Swap VFOs";
            comboRightSingle.Text = "Previous Filter";
            comboRightDouble.Text = "Next Filter";
            comboRightLong.Text = "Copy VFO B to A";
        }

        private void ButtonClicked(FlexControl.Button button, FlexControl.ClickType type)
        {
            if (!this.Visible) return; // form is still initializing

            switch (button)
            {
                case FlexControl.Button.Left:
                    switch (type)
                    {
                        case FlexControl.ClickType.Single: hlLeftSingle.NewEvent(); break;
                        case FlexControl.ClickType.Double: hlLeftDouble.NewEvent(); break;
                        case FlexControl.ClickType.Long: hlLeftLong.NewEvent(); break;
                    }
                    break;
                case FlexControl.Button.Middle:
                    switch (type)
                    {
                        case FlexControl.ClickType.Single: hlMidSingle.NewEvent(); break;
                        case FlexControl.ClickType.Double: hlMidDouble.NewEvent(); break;
                        case FlexControl.ClickType.Long: hlMidLong.NewEvent(); break;
                    }
                    break;
                case FlexControl.Button.Right:
                    switch (type)
                    {
                        case FlexControl.ClickType.Single: hlRightSingle.NewEvent(); break;
                        case FlexControl.ClickType.Double: hlRightDouble.NewEvent(); break;
                        case FlexControl.ClickType.Long: hlRightLong.NewEvent(); break;
                    }
                    break;
                case FlexControl.Button.Knob:
                    switch (type)
                    {
                        case FlexControl.ClickType.Double:
                            switch (current_knob_mode)
                            {
                                case FlexControlInterface1.KnobMode.A1:
                                case FlexControlInterface1.KnobMode.A2:
                                    hlKnobADouble.NewEvent();
                                    break;
                                case FlexControlInterface1.KnobMode.B1:
                                case FlexControlInterface1.KnobMode.B2:
                                    hlKnobBDouble.NewEvent();
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void KnobRotated(FlexControl.RotateDirection direction, int num_steps)
        {
            if (!this.Visible) return; // form is still initializing

            switch (current_knob_mode)
            {
                case FlexControlInterface1.KnobMode.A1: hlKnobA1.NewEvent(); break;
                case FlexControlInterface1.KnobMode.A2: hlKnobA2.NewEvent(); break;
                case FlexControlInterface1.KnobMode.B1: hlKnobB1.NewEvent(); break;
                case FlexControlInterface1.KnobMode.B2: hlKnobB2.NewEvent(); break;
            }
        }

        private void UpdateCurrentKnobMode(FlexControlInterface1.KnobMode mode)
        {
            if (!this.Visible) return;

            current_knob_mode = mode;
            Invoke(new FlexControlInterface1.KnobModeChanged(UpdateActiveComboBox), new object[] { mode });
        }

        private void UpdateActiveComboBox(FlexControlInterface1.KnobMode mode)
        {
            switch (mode)
            {
                case FlexControlInterface1.KnobMode.A1:
                    panelModeA1.Visible = true;
                    panelModeA2.Visible = false;
                    panelModeB1.Visible = false;
                    panelModeADouble.Visible = true;
                    panelModeBDouble.Visible = false;
                    break;
                case FlexControlInterface1.KnobMode.A2:
                    panelModeA2.Visible = true;
                    panelModeA1.Visible = false;
                    panelModeB2.Visible = false;
                    panelModeADouble.Visible = true;
                    panelModeBDouble.Visible = false;
                    break;
                case FlexControlInterface1.KnobMode.B1:
                    panelModeB1.Visible = true;
                    panelModeA1.Visible = false;
                    panelModeB2.Visible = false;
                    panelModeADouble.Visible = false;
                    panelModeBDouble.Visible = true;
                    break;
                case FlexControlInterface1.KnobMode.B2:
                    panelModeB2.Visible = true;
                    panelModeA2.Visible = false;
                    panelModeB1.Visible = false;
                    panelModeADouble.Visible = false;
                    panelModeBDouble.Visible = true;
                    break;
            }
        }

        private string KnobFunction2String(FlexControlKnobFunction function)
        {
            string ret_val = "";
            switch (function)
            {
                case FlexControlKnobFunction.TuneVFOA: ret_val = "Tune VFO A"; break;
                case FlexControlKnobFunction.TuneVFOB: ret_val = "Tune VFO B"; break;
                case FlexControlKnobFunction.TuneVFOASub: ret_val = "Tune VFO A Sub"; break;
                case FlexControlKnobFunction.TuneRIT: ret_val = "Tune RIT"; break;
                case FlexControlKnobFunction.TuneXIT: ret_val = "Tune XIT"; break;
                case FlexControlKnobFunction.TuneAF: ret_val = "Audio Gain"; break;
                case FlexControlKnobFunction.TuneAGCT: ret_val = "Tune AGC-T"; break;
                case FlexControlKnobFunction.None: ret_val = "None"; break;
            }

            return ret_val;
        }

        private FlexControlKnobFunction String2KnobFunction(string s)
        {
            FlexControlKnobFunction ret_val = FlexControlKnobFunction.TuneVFOA;
            switch (s)
            {
                case "Tune VFO A": ret_val = FlexControlKnobFunction.TuneVFOA; break;
                case "Tune VFO B": ret_val = FlexControlKnobFunction.TuneVFOB; break;
                case "Tune VFO A Sub": ret_val = FlexControlKnobFunction.TuneVFOASub; break;
                case "Tune RIT": ret_val = FlexControlKnobFunction.TuneRIT; break;
                case "Tune XIT": ret_val = FlexControlKnobFunction.TuneXIT; break;
                case "Audio Gain": ret_val = FlexControlKnobFunction.TuneAF; break;
                case "Tune AGC-T": ret_val = FlexControlKnobFunction.TuneAGCT; break;
                case "None": ret_val = FlexControlKnobFunction.None; break;
            }

            return ret_val;
        }

        private string ButtonFunction2String(FlexControlButtonFunction function)
        {
            string ret_val = "";
            switch (function)
            {
                case FlexControlButtonFunction.ToggleTXVFO: ret_val = "Toggle TX VFO"; break;
                case FlexControlButtonFunction.SwapVFO: ret_val = "Swap VFOs"; break;
                case FlexControlButtonFunction.ToggleSplit: ret_val = "Toggle Split"; break;
                case FlexControlButtonFunction.ClearRIT: ret_val = "Clear RIT"; break;
                case FlexControlButtonFunction.ClearXIT: ret_val = "Clear XIT"; break;
                case FlexControlButtonFunction.CWSpeedUp: ret_val = "CW Speed Up"; break;
                case FlexControlButtonFunction.CWSpeedDown: ret_val = "CW Speed Down"; break;
                case FlexControlButtonFunction.CWXSpeedUp: ret_val = "CWX Speed Up"; break;
                case FlexControlButtonFunction.CWXSpeedDown: ret_val = "CWX Speed Down"; break;
                case FlexControlButtonFunction.FilterNext: ret_val = "Next Filter"; break;
                case FlexControlButtonFunction.FilterPrevious: ret_val = "Previous Filter"; break;
                case FlexControlButtonFunction.SaveVFOA: ret_val = "Save VFO A"; break;
                case FlexControlButtonFunction.RestoreVFOA: ret_val = "Restore VFO A"; break;
                case FlexControlButtonFunction.TuneStepUp: ret_val = "Tune Step Up"; break;
                case FlexControlButtonFunction.TuneStepDown: ret_val = "Tune Step Down"; break;
                case FlexControlButtonFunction.CopyVFOAtoB: ret_val = "Copy VFO A to B"; break;
                case FlexControlButtonFunction.CopyVFOBtoA: ret_val = "Copy VFO B to A"; break;
                case FlexControlButtonFunction.ZeroBeat: ret_val = "Zero Beat"; break;
                case FlexControlButtonFunction.None: ret_val = "None"; break;
            }

            return ret_val;
        }

        private FlexControlButtonFunction String2ButtonFunction(string s)
        {
            FlexControlButtonFunction ret_val = FlexControlButtonFunction.ToggleTXVFO;
            switch (s)
            {
                case "Toggle TX VFO": ret_val = FlexControlButtonFunction.ToggleTXVFO; break;
                case "Swap VFOs": ret_val = FlexControlButtonFunction.SwapVFO; break;
                case "Toggle Split": ret_val = FlexControlButtonFunction.ToggleSplit; break;
                case "Clear RIT": ret_val = FlexControlButtonFunction.ClearRIT; break;
                case "Clear XIT": ret_val = FlexControlButtonFunction.ClearXIT; break;
                case "CW Speed Up": ret_val = FlexControlButtonFunction.CWSpeedUp; break;
                case "CW Speed Down": ret_val = FlexControlButtonFunction.CWSpeedDown; break;
                case "CWX Speed Up": ret_val = FlexControlButtonFunction.CWXSpeedUp; break;
                case "CWX Speed Down": ret_val = FlexControlButtonFunction.CWXSpeedDown; break;
                case "Next Filter": ret_val = FlexControlButtonFunction.FilterNext; break;
                case "Previous Filter": ret_val = FlexControlButtonFunction.FilterPrevious; break;
                case "Save VFO A": ret_val = FlexControlButtonFunction.SaveVFOA; break;
                case "Restore VFO A": ret_val = FlexControlButtonFunction.RestoreVFOA; break;
                case "Tune Step Up": ret_val = FlexControlButtonFunction.TuneStepUp; break;
                case "Tune Step Down": ret_val = FlexControlButtonFunction.TuneStepDown; break;
                case "Copy VFO A to B": ret_val = FlexControlButtonFunction.CopyVFOAtoB; break;
                case "Copy VFO B to A": ret_val = FlexControlButtonFunction.CopyVFOBtoA; break;
                case "Zero Beat": ret_val = FlexControlButtonFunction.ZeroBeat; break;
                case "None": ret_val = FlexControlButtonFunction.None; break;
            }

            return ret_val;
        }

        #endregion

        #region Event Handlers

        private void FlexControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;

            Common.SaveForm(this, "FlexControlAdvancedForm");
        }

        private void comboModeA1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobA1 = String2KnobFunction(comboModeA1.Text);
        }

        private void comboModeA2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobA2 = String2KnobFunction(comboModeA2.Text);
        }

        private void comboModeB1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobB1 = String2KnobFunction(comboModeB1.Text);
        }

        private void comboModeB2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobB2 = String2KnobFunction(comboModeB2.Text);
        }

        private void comboModeADouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobModeAButtonDouble = String2ButtonFunction(comboModeADouble.Text);
        }

        private void comboModeBDouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.KnobModeBButtonDouble = String2ButtonFunction(comboModeBDouble.Text);
        }

        private void comboLeftSingle_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonLeftSingle = String2ButtonFunction(comboLeftSingle.Text);
        }

        private void comboLeftDouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonLeftDouble = String2ButtonFunction(comboLeftDouble.Text);
        }

        private void comboLeftLong_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonLeftLong = String2ButtonFunction(comboLeftLong.Text);
        }

        private void comboMidSingle_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonMidSingle = String2ButtonFunction(comboMidSingle.Text);
        }

        private void comboMidDouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonMidDouble = String2ButtonFunction(comboMidDouble.Text);
        }

        private void comboMidLong_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonMidLong = String2ButtonFunction(comboMidLong.Text);
        }

        private void comboRightSingle_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonRightSingle = String2ButtonFunction(comboRightSingle.Text);
        }

        private void comboRightDouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonRightDouble = String2ButtonFunction(comboRightDouble.Text);
        }

        private void comboRightLong_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc_interface.ButtonRightLong = String2ButtonFunction(comboRightLong.Text);
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to reset all settings to the defaults?\n" +
                "All current FlexControl settings will be lost.",
                "Reset to Defaults?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No) return;

            SetupDefaults();
        }

        private void chkAutoDetect_CheckedChanged(object sender, EventArgs e)
        {
            auto_detect = chkAutoDetect.Checked;
            FlexControl = null;

            if (console == null) return;

            if (console.FlexControlAutoDetect != auto_detect)
                console.FlexControlAutoDetect = chkAutoDetect.Checked;
        }

        private void chkAutoClearRITXIT_CheckedChanged(object sender, EventArgs e)
        {
            fc_interface.AutoClearRITXIT = chkAutoClearRITXIT.Checked;
        }

        private void chkVRT_CheckedChanged(object sender, EventArgs e)
        {
            fc_interface.TuningAcceleration = chkVRT.Checked;
        }

        #endregion

        #region PanelHighlighter

        private class PanelHighlighter
        {
            #region Variables

            private enum State
            {
                Idle,
                RampUp,
                Hold,
                RampDown,
            }

            private HiPerfTimer timer = new HiPerfTimer();
            private State current_state = State.Idle;
            private double last_event = double.MinValue;
            Panel p;
            private Color panel_active_color = Color.FromArgb(100, 255, 255);
            private Color panel_inactive_color = Color.FromArgb(42, 72, 239);

            #endregion

            #region Constructor

            public PanelHighlighter(Panel panel)
            {
                p = panel;
                timer.Start();
            }

            #endregion

            #region Properties

            private double ramp_time = 250; // in ms
            public double RampTime
            {
                get { return ramp_time; }
                set { ramp_time = value; }
            }

            private double hold_time = 1000; // in ms
            public double HoldTime
            {
                get { return hold_time; }
                set { hold_time = value; }
            }

            #endregion

            #region Routines

            /// <summary>
            /// Blend colors from c1 to c2
            /// </summary>
            /// <param name="c1">Color to blend from (0.0)</param>
            /// <param name="c2">Color to blend to (1.0)</param>
            /// <param name="alpha">Percentage to blend.  Valid from 0.0 to 1.0.</param>
            /// <returns>The blended color.</returns>
            private Color BlendColors(Color c1, Color c2, double alpha)
            {
                if (alpha < 0.0) alpha = 0.0;
                if (alpha > 1.0) alpha = 1.0;

                return Color.FromArgb(
                    (int)((1 - alpha) * c1.R + alpha * c2.R),  // R
                    (int)((1 - alpha) * c1.G + alpha * c2.G),  // G
                    (int)((1 - alpha) * c1.B + alpha * c2.B)); // B
            }

            private void RampAlphaUp()
            {
                int STEPS = 50;
                for (int i = 0; i <= STEPS && current_state == State.RampUp; i++)
                {
                    p.BackColor = BlendColors(panel_inactive_color, panel_active_color, i / (double)STEPS);
                    Application.DoEvents();
                    Thread.Sleep(5);
                }

                if (current_state == State.RampUp)
                {
                    current_state = State.Hold;

                    Thread t = new Thread(new ThreadStart(HoldHighlight));
                    t.Name = "Hold Highlight Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();
                }
            }

            private void RampAlphaDown()
            {
                int STEPS = 100;
                for (int i = 0; i <= STEPS && current_state == State.RampDown; i++)
                {
                    p.BackColor = BlendColors(panel_active_color, panel_inactive_color, i / (double)STEPS);
                    Application.DoEvents();
                    Thread.Sleep(5);
                }

                if (current_state == State.RampDown)
                    current_state = State.Idle;
            }

            private void HoldHighlight()
            {
                while (current_state == State.Hold)
                {
                    timer.Stop();
                    if (timer.DurationMsec > last_event + hold_time)
                    {
                        current_state = State.RampDown;
                        p.Invoke(new MethodInvoker(RampAlphaDown));
                    }
                    Thread.Sleep(100);
                }
            }

            public void NewEvent()
            {
                timer.Stop();
                double time = timer.DurationMsec;
                switch (current_state)
                {
                    case State.Idle:
                    case State.RampDown:
                        last_event = time;
                        current_state = State.RampUp;
                        p.Invoke(new MethodInvoker(RampAlphaUp));
                        break;
                    case State.RampUp:
                    case State.Hold:
                        last_event = time;
                        break;
                }
            }

            #endregion
        }

        #endregion                        

        private void radModeBasic_CheckedChanged(object sender, EventArgs e)
        {
            if (radModeBasic.Checked)
            {
                if (fc_interface != null)
                    fc_interface.FlexControl = null;

                object obj = null;
                if (radModeBasic.Focused)
                    obj = this;

                console.SetCurrentFlexControlMode(obj, FlexControlMode.Basic);

                if (flexControl != null)
                {
                    flexControl.KnobRotated -= new FlexControl.KnobRotatedEventHandler(KnobRotated);
                    flexControl.ButtonClicked -= new FlexControl.ButtonClickedEventHandler(ButtonClicked);
                }

                this.Hide();
            }
        }

        private void radModeAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (radModeAdvanced.Checked)
            {
                if (fc_interface.FlexControl != flexControl)
                    fc_interface.FlexControl = flexControl;

                if (flexControl != null)
                {
                    flexControl.KnobRotated += new FlexControl.KnobRotatedEventHandler(KnobRotated);
                    flexControl.ButtonClicked += new FlexControl.ButtonClickedEventHandler(ButtonClicked);
                }

                object obj = null;
                if (radModeAdvanced.Focused)
                    obj = this;

                console.SetCurrentFlexControlMode(obj, FlexControlMode.Advanced);
            }
        }
    }
}
