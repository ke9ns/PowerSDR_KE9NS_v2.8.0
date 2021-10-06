namespace PowerSDR
{
    partial class FlexControlBasicForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (fc_interface == null) return;
            fc_interface.Cleanup();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexControlBasicForm));
            this.picFlexControl = new System.Windows.Forms.PictureBox();
            this.lblAUX1 = new System.Windows.Forms.Label();
            this.lblAUX3 = new System.Windows.Forms.Label();
            this.lblAUX2 = new System.Windows.Forms.Label();
            this.lblKnob = new System.Windows.Forms.Label();
            this.chkBoxIND = new System.Windows.Forms.CheckBoxTS();
            this.btnTuneStepChangeLarger2 = new System.Windows.Forms.ButtonTS();
            this.btnTuneStepChangeSmaller2 = new System.Windows.Forms.ButtonTS();
            this.groupBoxTS1 = new System.Windows.Forms.GroupBoxTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.chkBoxPM = new System.Windows.Forms.CheckBoxTS();
            this.udSpeedPM = new System.Windows.Forms.NumericUpDownTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.txtWheelTune2 = new System.Windows.Forms.TextBoxTS();
            this.labelTS15 = new System.Windows.Forms.LabelTS();
            this.chkAutoDetect = new System.Windows.Forms.CheckBoxTS();
            this.radModeAdvanced = new System.Windows.Forms.RadioButtonTS();
            this.radModeBasic = new System.Windows.Forms.RadioButtonTS();
            this.comboButtonKnob = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonRight = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonMid = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonLeft = new System.Windows.Forms.ComboBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.picFlexControl)).BeginInit();
            this.groupBoxTS1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSpeedPM)).BeginInit();
            this.SuspendLayout();
            // 
            // picFlexControl
            // 
            this.picFlexControl.Image = ((System.Drawing.Image)(resources.GetObject("picFlexControl.Image")));
            this.picFlexControl.Location = new System.Drawing.Point(111, 55);
            this.picFlexControl.Name = "picFlexControl";
            this.picFlexControl.Size = new System.Drawing.Size(400, 453);
            this.picFlexControl.TabIndex = 0;
            this.picFlexControl.TabStop = false;
            // 
            // lblAUX1
            // 
            this.lblAUX1.AutoSize = true;
            this.lblAUX1.Location = new System.Drawing.Point(24, 132);
            this.lblAUX1.Name = "lblAUX1";
            this.lblAUX1.Size = new System.Drawing.Size(71, 13);
            this.lblAUX1.TabIndex = 9;
            this.lblAUX1.Text = "AUX 1 Action";
            // 
            // lblAUX3
            // 
            this.lblAUX3.AutoSize = true;
            this.lblAUX3.Location = new System.Drawing.Point(526, 132);
            this.lblAUX3.Name = "lblAUX3";
            this.lblAUX3.Size = new System.Drawing.Size(71, 13);
            this.lblAUX3.TabIndex = 10;
            this.lblAUX3.Text = "AUX 3 Action";
            // 
            // lblAUX2
            // 
            this.lblAUX2.AutoSize = true;
            this.lblAUX2.Location = new System.Drawing.Point(267, 11);
            this.lblAUX2.Name = "lblAUX2";
            this.lblAUX2.Size = new System.Drawing.Size(71, 13);
            this.lblAUX2.TabIndex = 11;
            this.lblAUX2.Text = "AUX 2 Action";
            // 
            // lblKnob
            // 
            this.lblKnob.AutoSize = true;
            this.lblKnob.Location = new System.Drawing.Point(12, 278);
            this.lblKnob.Name = "lblKnob";
            this.lblKnob.Size = new System.Drawing.Size(94, 13);
            this.lblKnob.TabIndex = 12;
            this.lblKnob.Text = "Knob Press Action";
            // 
            // chkBoxIND
            // 
            this.chkBoxIND.Image = null;
            this.chkBoxIND.Location = new System.Drawing.Point(548, 409);
            this.chkBoxIND.Name = "chkBoxIND";
            this.chkBoxIND.Size = new System.Drawing.Size(58, 31);
            this.chkBoxIND.TabIndex = 7;
            this.chkBoxIND.Text = "ON";
            this.chkBoxIND.Visible = false;
            // 
            // btnTuneStepChangeLarger2
            // 
            this.btnTuneStepChangeLarger2.FlatAppearance.BorderSize = 0;
            this.btnTuneStepChangeLarger2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTuneStepChangeLarger2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnTuneStepChangeLarger2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnTuneStepChangeLarger2.Image = null;
            this.btnTuneStepChangeLarger2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.btnTuneStepChangeLarger2.Location = new System.Drawing.Point(595, 374);
            this.btnTuneStepChangeLarger2.Name = "btnTuneStepChangeLarger2";
            this.btnTuneStepChangeLarger2.Size = new System.Drawing.Size(16, 19);
            this.btnTuneStepChangeLarger2.TabIndex = 79;
            this.btnTuneStepChangeLarger2.Text = "+";
            this.btnTuneStepChangeLarger2.Visible = false;
            this.btnTuneStepChangeLarger2.Click += new System.EventHandler(this.btnTuneStepChangeLarger2_Click);
            // 
            // btnTuneStepChangeSmaller2
            // 
            this.btnTuneStepChangeSmaller2.FlatAppearance.BorderSize = 0;
            this.btnTuneStepChangeSmaller2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTuneStepChangeSmaller2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnTuneStepChangeSmaller2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnTuneStepChangeSmaller2.Image = null;
            this.btnTuneStepChangeSmaller2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.btnTuneStepChangeSmaller2.Location = new System.Drawing.Point(533, 373);
            this.btnTuneStepChangeSmaller2.Name = "btnTuneStepChangeSmaller2";
            this.btnTuneStepChangeSmaller2.Size = new System.Drawing.Size(16, 19);
            this.btnTuneStepChangeSmaller2.TabIndex = 78;
            this.btnTuneStepChangeSmaller2.Text = "-";
            this.btnTuneStepChangeSmaller2.Visible = false;
            this.btnTuneStepChangeSmaller2.Click += new System.EventHandler(this.btnTuneStepChangeSmaller2_Click);
            // 
            // groupBoxTS1
            // 
            this.groupBoxTS1.Controls.Add(this.labelTS1);
            this.groupBoxTS1.Controls.Add(this.chkBoxPM);
            this.groupBoxTS1.Controls.Add(this.udSpeedPM);
            this.groupBoxTS1.Location = new System.Drawing.Point(518, 198);
            this.groupBoxTS1.Name = "groupBoxTS1";
            this.groupBoxTS1.Size = new System.Drawing.Size(117, 105);
            this.groupBoxTS1.TabIndex = 54;
            this.groupBoxTS1.TabStop = false;
            this.groupBoxTS1.Text = "PowerMate";
            this.groupBoxTS1.Visible = false;
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(6, 25);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(48, 16);
            this.labelTS1.TabIndex = 5;
            this.labelTS1.Text = "Speed:";
            // 
            // chkBoxPM
            // 
            this.chkBoxPM.Image = null;
            this.chkBoxPM.Location = new System.Drawing.Point(11, 60);
            this.chkBoxPM.Name = "chkBoxPM";
            this.chkBoxPM.Size = new System.Drawing.Size(62, 31);
            this.chkBoxPM.TabIndex = 6;
            this.chkBoxPM.Text = "Active";
            this.chkBoxPM.CheckedChanged += new System.EventHandler(this.chkBoxPM_CheckedChanged);
            // 
            // udSpeedPM
            // 
            this.udSpeedPM.DecimalPlaces = 1;
            this.udSpeedPM.Increment = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.udSpeedPM.Location = new System.Drawing.Point(60, 23);
            this.udSpeedPM.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.udSpeedPM.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udSpeedPM.Name = "udSpeedPM";
            this.udSpeedPM.Size = new System.Drawing.Size(41, 20);
            this.udSpeedPM.TabIndex = 4;
            this.udSpeedPM.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udSpeedPM.ValueChanged += new System.EventHandler(this.udSpeedPM_ValueChanged);
            // 
            // labelTS2
            // 
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(515, 31);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(126, 82);
            this.labelTS2.TabIndex = 80;
            this.labelTS2.Text = "For Alternate Tune Step Rate for FlexControl:\r\nSetup->General->User Interface->Po" +
    "werMate and FlexControl";
            // 
            // txtWheelTune2
            // 
            this.txtWheelTune2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.txtWheelTune2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWheelTune2.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtWheelTune2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtWheelTune2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtWheelTune2.Location = new System.Drawing.Point(549, 373);
            this.txtWheelTune2.Name = "txtWheelTune2";
            this.txtWheelTune2.ReadOnly = true;
            this.txtWheelTune2.Size = new System.Drawing.Size(48, 20);
            this.txtWheelTune2.TabIndex = 16;
            this.txtWheelTune2.Text = "1kHz";
            this.txtWheelTune2.Visible = false;
            this.txtWheelTune2.TextChanged += new System.EventHandler(this.txtWheelTune2_TextChanged);
            this.txtWheelTune2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtWheelTune2_MouseDown);
            // 
            // labelTS15
            // 
            this.labelTS15.Image = null;
            this.labelTS15.Location = new System.Drawing.Point(545, 320);
            this.labelTS15.Name = "labelTS15";
            this.labelTS15.Size = new System.Drawing.Size(89, 51);
            this.labelTS15.TabIndex = 13;
            this.labelTS15.Text = "Tune Step for FlexControl ";
            this.labelTS15.Visible = false;
            // 
            // chkAutoDetect
            // 
            this.chkAutoDetect.AutoSize = true;
            this.chkAutoDetect.Checked = true;
            this.chkAutoDetect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDetect.Image = null;
            this.chkAutoDetect.Location = new System.Drawing.Point(12, 481);
            this.chkAutoDetect.Name = "chkAutoDetect";
            this.chkAutoDetect.Size = new System.Drawing.Size(83, 17);
            this.chkAutoDetect.TabIndex = 8;
            this.chkAutoDetect.Text = "Auto-Detect";
            this.chkAutoDetect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAutoDetect.UseVisualStyleBackColor = true;
            this.chkAutoDetect.CheckedChanged += new System.EventHandler(this.chkAutoDetect_CheckedChanged);
            // 
            // radModeAdvanced
            // 
            this.radModeAdvanced.AutoSize = true;
            this.radModeAdvanced.Image = null;
            this.radModeAdvanced.Location = new System.Drawing.Point(90, 9);
            this.radModeAdvanced.Name = "radModeAdvanced";
            this.radModeAdvanced.Size = new System.Drawing.Size(74, 17);
            this.radModeAdvanced.TabIndex = 7;
            this.radModeAdvanced.Text = "Advanced";
            this.radModeAdvanced.UseVisualStyleBackColor = true;
            this.radModeAdvanced.CheckedChanged += new System.EventHandler(this.radModeAdvanced_CheckedChanged);
            // 
            // radModeBasic
            // 
            this.radModeBasic.AutoSize = true;
            this.radModeBasic.Checked = true;
            this.radModeBasic.Image = null;
            this.radModeBasic.Location = new System.Drawing.Point(12, 9);
            this.radModeBasic.Name = "radModeBasic";
            this.radModeBasic.Size = new System.Drawing.Size(51, 17);
            this.radModeBasic.TabIndex = 6;
            this.radModeBasic.TabStop = true;
            this.radModeBasic.Text = "Basic";
            this.radModeBasic.UseVisualStyleBackColor = true;
            this.radModeBasic.CheckedChanged += new System.EventHandler(this.radModeBasic_CheckedChanged);
            // 
            // comboButtonKnob
            // 
            this.comboButtonKnob.Enabled = false;
            this.comboButtonKnob.FormattingEnabled = true;
            this.comboButtonKnob.Location = new System.Drawing.Point(12, 295);
            this.comboButtonKnob.Name = "comboButtonKnob";
            this.comboButtonKnob.Size = new System.Drawing.Size(93, 21);
            this.comboButtonKnob.TabIndex = 4;
            this.comboButtonKnob.SelectedIndexChanged += new System.EventHandler(this.comboButtonKnob_SelectedIndexChanged);
            // 
            // comboButtonRight
            // 
            this.comboButtonRight.Enabled = false;
            this.comboButtonRight.FormattingEnabled = true;
            this.comboButtonRight.Location = new System.Drawing.Point(517, 150);
            this.comboButtonRight.Name = "comboButtonRight";
            this.comboButtonRight.Size = new System.Drawing.Size(93, 21);
            this.comboButtonRight.TabIndex = 3;
            this.comboButtonRight.SelectedIndexChanged += new System.EventHandler(this.comboButtonRight_SelectedIndexChanged);
            // 
            // comboButtonMid
            // 
            this.comboButtonMid.Enabled = false;
            this.comboButtonMid.FormattingEnabled = true;
            this.comboButtonMid.Location = new System.Drawing.Point(258, 28);
            this.comboButtonMid.Name = "comboButtonMid";
            this.comboButtonMid.Size = new System.Drawing.Size(93, 21);
            this.comboButtonMid.TabIndex = 2;
            this.comboButtonMid.SelectedIndexChanged += new System.EventHandler(this.comboButtonMid_SelectedIndexChanged);
            // 
            // comboButtonLeft
            // 
            this.comboButtonLeft.Enabled = false;
            this.comboButtonLeft.FormattingEnabled = true;
            this.comboButtonLeft.Location = new System.Drawing.Point(12, 150);
            this.comboButtonLeft.Name = "comboButtonLeft";
            this.comboButtonLeft.Size = new System.Drawing.Size(93, 21);
            this.comboButtonLeft.TabIndex = 1;
            this.comboButtonLeft.SelectedIndexChanged += new System.EventHandler(this.comboButtonLeft_SelectedIndexChanged);
            // 
            // FlexControlBasicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 522);
            this.Controls.Add(this.labelTS2);
            this.Controls.Add(this.chkBoxIND);
            this.Controls.Add(this.btnTuneStepChangeLarger2);
            this.Controls.Add(this.btnTuneStepChangeSmaller2);
            this.Controls.Add(this.groupBoxTS1);
            this.Controls.Add(this.txtWheelTune2);
            this.Controls.Add(this.labelTS15);
            this.Controls.Add(this.lblKnob);
            this.Controls.Add(this.lblAUX2);
            this.Controls.Add(this.lblAUX3);
            this.Controls.Add(this.lblAUX1);
            this.Controls.Add(this.chkAutoDetect);
            this.Controls.Add(this.radModeAdvanced);
            this.Controls.Add(this.radModeBasic);
            this.Controls.Add(this.comboButtonKnob);
            this.Controls.Add(this.comboButtonRight);
            this.Controls.Add(this.comboButtonMid);
            this.Controls.Add(this.comboButtonLeft);
            this.Controls.Add(this.picFlexControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlexControlBasicForm";
            this.Text = "FlexControl - Basic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlexControlSimpleForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picFlexControl)).EndInit();
            this.groupBoxTS1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udSpeedPM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picFlexControl;
        private System.Windows.Forms.ComboBoxTS comboButtonLeft;
        private System.Windows.Forms.ComboBoxTS comboButtonMid;
        private System.Windows.Forms.ComboBoxTS comboButtonRight;
        private System.Windows.Forms.ComboBoxTS comboButtonKnob;
        private System.Windows.Forms.RadioButtonTS radModeBasic;
        private System.Windows.Forms.RadioButtonTS radModeAdvanced;
        private System.Windows.Forms.CheckBoxTS chkAutoDetect;
        private System.Windows.Forms.Label lblAUX1;
        private System.Windows.Forms.Label lblAUX3;
        private System.Windows.Forms.Label lblAUX2;
        private System.Windows.Forms.Label lblKnob;
        private System.Windows.Forms.LabelTS labelTS15;
        public System.Windows.Forms.GroupBoxTS groupBoxTS1;
        private System.Windows.Forms.LabelTS labelTS1;
        public System.Windows.Forms.CheckBoxTS chkBoxPM;
        public System.Windows.Forms.NumericUpDownTS udSpeedPM;
        private System.Windows.Forms.ButtonTS btnTuneStepChangeLarger2;
        private System.Windows.Forms.ButtonTS btnTuneStepChangeSmaller2;
        public System.Windows.Forms.TextBoxTS txtWheelTune2;
        public System.Windows.Forms.CheckBoxTS chkBoxIND;
        private System.Windows.Forms.LabelTS labelTS2;
    }
}