namespace PowerSDR
{
    partial class FlexControlAdvancedForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexControlAdvancedForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAutoDetect = new System.Windows.Forms.CheckBoxTS();
            this.btnDefaults = new System.Windows.Forms.ButtonTS();
            this.grpButton = new System.Windows.Forms.GroupBoxTS();
            this.lblLongClick = new System.Windows.Forms.LabelTS();
            this.lblDoubleClick = new System.Windows.Forms.LabelTS();
            this.lblSingleClick = new System.Windows.Forms.LabelTS();
            this.lblButtonRight = new System.Windows.Forms.LabelTS();
            this.lblButtonMiddle = new System.Windows.Forms.LabelTS();
            this.lblButtonLeft = new System.Windows.Forms.LabelTS();
            this.comboRightLong = new System.Windows.Forms.ComboBoxTS();
            this.comboMidLong = new System.Windows.Forms.ComboBoxTS();
            this.comboLeftLong = new System.Windows.Forms.ComboBoxTS();
            this.panelLeftLong = new System.Windows.Forms.Panel();
            this.panelMidLong = new System.Windows.Forms.Panel();
            this.panelRightLong = new System.Windows.Forms.Panel();
            this.comboRightDouble = new System.Windows.Forms.ComboBoxTS();
            this.comboRightSingle = new System.Windows.Forms.ComboBoxTS();
            this.comboMidDouble = new System.Windows.Forms.ComboBoxTS();
            this.comboLeftDouble = new System.Windows.Forms.ComboBoxTS();
            this.comboMidSingle = new System.Windows.Forms.ComboBoxTS();
            this.comboLeftSingle = new System.Windows.Forms.ComboBoxTS();
            this.panelMidSingle = new System.Windows.Forms.Panel();
            this.panelLeftSingle = new System.Windows.Forms.Panel();
            this.panelLeftDouble = new System.Windows.Forms.Panel();
            this.panelMidDouble = new System.Windows.Forms.Panel();
            this.panelRightSingle = new System.Windows.Forms.Panel();
            this.panelRightDouble = new System.Windows.Forms.Panel();
            this.grpKnob = new System.Windows.Forms.GroupBoxTS();
            this.chkVRT = new System.Windows.Forms.CheckBoxTS();
            this.chkAutoClearRITXIT = new System.Windows.Forms.CheckBoxTS();
            this.picArrowDouble = new System.Windows.Forms.PictureBox();
            this.lblDoubleLongClick = new System.Windows.Forms.LabelTS();
            this.picArrowB = new System.Windows.Forms.PictureBox();
            this.lbl2LongClick = new System.Windows.Forms.LabelTS();
            this.picArrowA = new System.Windows.Forms.PictureBox();
            this.comboModeBDouble = new System.Windows.Forms.ComboBoxTS();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblKnobDoubleClick = new System.Windows.Forms.LabelTS();
            this.picArrow1 = new System.Windows.Forms.PictureBox();
            this.comboModeADouble = new System.Windows.Forms.ComboBoxTS();
            this.lblASingleClick = new System.Windows.Forms.LabelTS();
            this.comboModeB2 = new System.Windows.Forms.ComboBoxTS();
            this.comboModeB1 = new System.Windows.Forms.ComboBoxTS();
            this.comboModeA2 = new System.Windows.Forms.ComboBoxTS();
            this.comboModeA1 = new System.Windows.Forms.ComboBoxTS();
            this.panelModeA2 = new System.Windows.Forms.Panel();
            this.panelModeA1 = new System.Windows.Forms.Panel();
            this.panelModeB1 = new System.Windows.Forms.Panel();
            this.panelModeB2 = new System.Windows.Forms.Panel();
            this.panelModeADouble = new System.Windows.Forms.Panel();
            this.panelModeBDouble = new System.Windows.Forms.Panel();
            this.lbl1LongClick = new System.Windows.Forms.LabelTS();
            this.radModeAdvanced = new System.Windows.Forms.RadioButtonTS();
            this.radModeBasic = new System.Windows.Forms.RadioButtonTS();
            this.grpButton.SuspendLayout();
            this.grpKnob.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowDouble)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrow1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAutoDetect
            // 
            this.chkAutoDetect.AutoSize = true;
            this.chkAutoDetect.Checked = true;
            this.chkAutoDetect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDetect.Image = null;
            this.chkAutoDetect.Location = new System.Drawing.Point(26, 403);
            this.chkAutoDetect.Name = "chkAutoDetect";
            this.chkAutoDetect.Size = new System.Drawing.Size(83, 17);
            this.chkAutoDetect.TabIndex = 4;
            this.chkAutoDetect.Text = "Auto-Detect";
            this.chkAutoDetect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.chkAutoDetect, "Uncheck this box to use FlexControl with third party programs like DDUtil");
            this.chkAutoDetect.UseVisualStyleBackColor = true;
            this.chkAutoDetect.CheckedChanged += new System.EventHandler(this.chkAutoDetect_CheckedChanged);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Image = null;
            this.btnDefaults.Location = new System.Drawing.Point(470, 403);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnDefaults.TabIndex = 3;
            this.btnDefaults.Text = "Defaults";
            this.toolTip1.SetToolTip(this.btnDefaults, "Click this button to return to the default settings for the controls shown on thi" +
                    "s form");
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // grpButton
            // 
            this.grpButton.Controls.Add(this.lblLongClick);
            this.grpButton.Controls.Add(this.lblDoubleClick);
            this.grpButton.Controls.Add(this.lblSingleClick);
            this.grpButton.Controls.Add(this.lblButtonRight);
            this.grpButton.Controls.Add(this.lblButtonMiddle);
            this.grpButton.Controls.Add(this.lblButtonLeft);
            this.grpButton.Controls.Add(this.comboRightLong);
            this.grpButton.Controls.Add(this.comboMidLong);
            this.grpButton.Controls.Add(this.comboLeftLong);
            this.grpButton.Controls.Add(this.panelLeftLong);
            this.grpButton.Controls.Add(this.panelMidLong);
            this.grpButton.Controls.Add(this.panelRightLong);
            this.grpButton.Controls.Add(this.comboRightDouble);
            this.grpButton.Controls.Add(this.comboRightSingle);
            this.grpButton.Controls.Add(this.comboMidDouble);
            this.grpButton.Controls.Add(this.comboLeftDouble);
            this.grpButton.Controls.Add(this.comboMidSingle);
            this.grpButton.Controls.Add(this.comboLeftSingle);
            this.grpButton.Controls.Add(this.panelMidSingle);
            this.grpButton.Controls.Add(this.panelLeftSingle);
            this.grpButton.Controls.Add(this.panelLeftDouble);
            this.grpButton.Controls.Add(this.panelMidDouble);
            this.grpButton.Controls.Add(this.panelRightSingle);
            this.grpButton.Controls.Add(this.panelRightDouble);
            this.grpButton.Location = new System.Drawing.Point(12, 224);
            this.grpButton.Name = "grpButton";
            this.grpButton.Size = new System.Drawing.Size(533, 172);
            this.grpButton.TabIndex = 2;
            this.grpButton.TabStop = false;
            this.grpButton.Text = "Button Settings";
            // 
            // lblLongClick
            // 
            this.lblLongClick.AutoSize = true;
            this.lblLongClick.Image = null;
            this.lblLongClick.Location = new System.Drawing.Point(11, 135);
            this.lblLongClick.Name = "lblLongClick";
            this.lblLongClick.Size = new System.Drawing.Size(57, 13);
            this.lblLongClick.TabIndex = 28;
            this.lblLongClick.Text = "Long Click";
            // 
            // lblDoubleClick
            // 
            this.lblDoubleClick.AutoSize = true;
            this.lblDoubleClick.Image = null;
            this.lblDoubleClick.Location = new System.Drawing.Point(11, 92);
            this.lblDoubleClick.Name = "lblDoubleClick";
            this.lblDoubleClick.Size = new System.Drawing.Size(67, 13);
            this.lblDoubleClick.TabIndex = 27;
            this.lblDoubleClick.Text = "Double Click";
            // 
            // lblSingleClick
            // 
            this.lblSingleClick.AutoSize = true;
            this.lblSingleClick.Image = null;
            this.lblSingleClick.Location = new System.Drawing.Point(11, 49);
            this.lblSingleClick.Name = "lblSingleClick";
            this.lblSingleClick.Size = new System.Drawing.Size(62, 13);
            this.lblSingleClick.TabIndex = 26;
            this.lblSingleClick.Text = "Single Click";
            // 
            // lblButtonRight
            // 
            this.lblButtonRight.Image = null;
            this.lblButtonRight.Location = new System.Drawing.Point(390, 21);
            this.lblButtonRight.Name = "lblButtonRight";
            this.lblButtonRight.Size = new System.Drawing.Size(125, 13);
            this.lblButtonRight.TabIndex = 25;
            this.lblButtonRight.Text = "Right";
            this.lblButtonRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblButtonMiddle
            // 
            this.lblButtonMiddle.Image = null;
            this.lblButtonMiddle.Location = new System.Drawing.Point(237, 21);
            this.lblButtonMiddle.Name = "lblButtonMiddle";
            this.lblButtonMiddle.Size = new System.Drawing.Size(125, 13);
            this.lblButtonMiddle.TabIndex = 24;
            this.lblButtonMiddle.Text = "Middle";
            this.lblButtonMiddle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblButtonLeft
            // 
            this.lblButtonLeft.Image = null;
            this.lblButtonLeft.Location = new System.Drawing.Point(84, 21);
            this.lblButtonLeft.Name = "lblButtonLeft";
            this.lblButtonLeft.Size = new System.Drawing.Size(125, 13);
            this.lblButtonLeft.TabIndex = 23;
            this.lblButtonLeft.Text = "Left";
            this.lblButtonLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboRightLong
            // 
            this.comboRightLong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRightLong.FormattingEnabled = true;
            this.comboRightLong.Location = new System.Drawing.Point(392, 132);
            this.comboRightLong.Name = "comboRightLong";
            this.comboRightLong.Size = new System.Drawing.Size(121, 21);
            this.comboRightLong.TabIndex = 19;
            this.comboRightLong.SelectedIndexChanged += new System.EventHandler(this.comboRightLong_SelectedIndexChanged);
            // 
            // comboMidLong
            // 
            this.comboMidLong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMidLong.FormattingEnabled = true;
            this.comboMidLong.Location = new System.Drawing.Point(239, 132);
            this.comboMidLong.Name = "comboMidLong";
            this.comboMidLong.Size = new System.Drawing.Size(121, 21);
            this.comboMidLong.TabIndex = 18;
            this.comboMidLong.SelectedIndexChanged += new System.EventHandler(this.comboMidLong_SelectedIndexChanged);
            // 
            // comboLeftLong
            // 
            this.comboLeftLong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLeftLong.FormattingEnabled = true;
            this.comboLeftLong.Location = new System.Drawing.Point(86, 132);
            this.comboLeftLong.Name = "comboLeftLong";
            this.comboLeftLong.Size = new System.Drawing.Size(121, 21);
            this.comboLeftLong.TabIndex = 17;
            this.comboLeftLong.SelectedIndexChanged += new System.EventHandler(this.comboLeftLong_SelectedIndexChanged);
            // 
            // panelLeftLong
            // 
            this.panelLeftLong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelLeftLong.Location = new System.Drawing.Point(84, 130);
            this.panelLeftLong.Name = "panelLeftLong";
            this.panelLeftLong.Size = new System.Drawing.Size(125, 25);
            this.panelLeftLong.TabIndex = 20;
            // 
            // panelMidLong
            // 
            this.panelMidLong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelMidLong.Location = new System.Drawing.Point(237, 130);
            this.panelMidLong.Name = "panelMidLong";
            this.panelMidLong.Size = new System.Drawing.Size(125, 25);
            this.panelMidLong.TabIndex = 21;
            // 
            // panelRightLong
            // 
            this.panelRightLong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelRightLong.Location = new System.Drawing.Point(390, 130);
            this.panelRightLong.Name = "panelRightLong";
            this.panelRightLong.Size = new System.Drawing.Size(125, 25);
            this.panelRightLong.TabIndex = 22;
            // 
            // comboRightDouble
            // 
            this.comboRightDouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRightDouble.FormattingEnabled = true;
            this.comboRightDouble.Location = new System.Drawing.Point(392, 89);
            this.comboRightDouble.Name = "comboRightDouble";
            this.comboRightDouble.Size = new System.Drawing.Size(121, 21);
            this.comboRightDouble.TabIndex = 10;
            this.comboRightDouble.SelectedIndexChanged += new System.EventHandler(this.comboRightDouble_SelectedIndexChanged);
            // 
            // comboRightSingle
            // 
            this.comboRightSingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRightSingle.FormattingEnabled = true;
            this.comboRightSingle.Location = new System.Drawing.Point(392, 46);
            this.comboRightSingle.Name = "comboRightSingle";
            this.comboRightSingle.Size = new System.Drawing.Size(121, 21);
            this.comboRightSingle.TabIndex = 8;
            this.comboRightSingle.SelectedIndexChanged += new System.EventHandler(this.comboRightSingle_SelectedIndexChanged);
            // 
            // comboMidDouble
            // 
            this.comboMidDouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMidDouble.FormattingEnabled = true;
            this.comboMidDouble.Location = new System.Drawing.Point(239, 89);
            this.comboMidDouble.Name = "comboMidDouble";
            this.comboMidDouble.Size = new System.Drawing.Size(121, 21);
            this.comboMidDouble.TabIndex = 3;
            this.comboMidDouble.SelectedIndexChanged += new System.EventHandler(this.comboMidDouble_SelectedIndexChanged);
            // 
            // comboLeftDouble
            // 
            this.comboLeftDouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLeftDouble.FormattingEnabled = true;
            this.comboLeftDouble.Location = new System.Drawing.Point(86, 89);
            this.comboLeftDouble.Name = "comboLeftDouble";
            this.comboLeftDouble.Size = new System.Drawing.Size(121, 21);
            this.comboLeftDouble.TabIndex = 2;
            this.comboLeftDouble.SelectedIndexChanged += new System.EventHandler(this.comboLeftDouble_SelectedIndexChanged);
            // 
            // comboMidSingle
            // 
            this.comboMidSingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMidSingle.FormattingEnabled = true;
            this.comboMidSingle.Location = new System.Drawing.Point(239, 46);
            this.comboMidSingle.Name = "comboMidSingle";
            this.comboMidSingle.Size = new System.Drawing.Size(121, 21);
            this.comboMidSingle.TabIndex = 1;
            this.comboMidSingle.SelectedIndexChanged += new System.EventHandler(this.comboMidSingle_SelectedIndexChanged);
            // 
            // comboLeftSingle
            // 
            this.comboLeftSingle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLeftSingle.FormattingEnabled = true;
            this.comboLeftSingle.Location = new System.Drawing.Point(86, 46);
            this.comboLeftSingle.Name = "comboLeftSingle";
            this.comboLeftSingle.Size = new System.Drawing.Size(121, 21);
            this.comboLeftSingle.TabIndex = 0;
            this.comboLeftSingle.SelectedIndexChanged += new System.EventHandler(this.comboLeftSingle_SelectedIndexChanged);
            // 
            // panelMidSingle
            // 
            this.panelMidSingle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelMidSingle.Location = new System.Drawing.Point(237, 44);
            this.panelMidSingle.Name = "panelMidSingle";
            this.panelMidSingle.Size = new System.Drawing.Size(125, 25);
            this.panelMidSingle.TabIndex = 12;
            // 
            // panelLeftSingle
            // 
            this.panelLeftSingle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelLeftSingle.Location = new System.Drawing.Point(84, 44);
            this.panelLeftSingle.Name = "panelLeftSingle";
            this.panelLeftSingle.Size = new System.Drawing.Size(125, 25);
            this.panelLeftSingle.TabIndex = 13;
            // 
            // panelLeftDouble
            // 
            this.panelLeftDouble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelLeftDouble.Location = new System.Drawing.Point(84, 87);
            this.panelLeftDouble.Name = "panelLeftDouble";
            this.panelLeftDouble.Size = new System.Drawing.Size(125, 25);
            this.panelLeftDouble.TabIndex = 13;
            // 
            // panelMidDouble
            // 
            this.panelMidDouble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelMidDouble.Location = new System.Drawing.Point(237, 87);
            this.panelMidDouble.Name = "panelMidDouble";
            this.panelMidDouble.Size = new System.Drawing.Size(125, 25);
            this.panelMidDouble.TabIndex = 14;
            // 
            // panelRightSingle
            // 
            this.panelRightSingle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelRightSingle.Location = new System.Drawing.Point(390, 44);
            this.panelRightSingle.Name = "panelRightSingle";
            this.panelRightSingle.Size = new System.Drawing.Size(125, 25);
            this.panelRightSingle.TabIndex = 15;
            // 
            // panelRightDouble
            // 
            this.panelRightDouble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelRightDouble.Location = new System.Drawing.Point(390, 87);
            this.panelRightDouble.Name = "panelRightDouble";
            this.panelRightDouble.Size = new System.Drawing.Size(125, 25);
            this.panelRightDouble.TabIndex = 16;
            // 
            // grpKnob
            // 
            this.grpKnob.Controls.Add(this.chkVRT);
            this.grpKnob.Controls.Add(this.chkAutoClearRITXIT);
            this.grpKnob.Controls.Add(this.picArrowDouble);
            this.grpKnob.Controls.Add(this.lblDoubleLongClick);
            this.grpKnob.Controls.Add(this.picArrowB);
            this.grpKnob.Controls.Add(this.lbl2LongClick);
            this.grpKnob.Controls.Add(this.picArrowA);
            this.grpKnob.Controls.Add(this.comboModeBDouble);
            this.grpKnob.Controls.Add(this.pictureBox1);
            this.grpKnob.Controls.Add(this.lblKnobDoubleClick);
            this.grpKnob.Controls.Add(this.picArrow1);
            this.grpKnob.Controls.Add(this.comboModeADouble);
            this.grpKnob.Controls.Add(this.lblASingleClick);
            this.grpKnob.Controls.Add(this.comboModeB2);
            this.grpKnob.Controls.Add(this.comboModeB1);
            this.grpKnob.Controls.Add(this.comboModeA2);
            this.grpKnob.Controls.Add(this.comboModeA1);
            this.grpKnob.Controls.Add(this.panelModeA2);
            this.grpKnob.Controls.Add(this.panelModeA1);
            this.grpKnob.Controls.Add(this.panelModeB1);
            this.grpKnob.Controls.Add(this.panelModeB2);
            this.grpKnob.Controls.Add(this.panelModeADouble);
            this.grpKnob.Controls.Add(this.panelModeBDouble);
            this.grpKnob.Controls.Add(this.lbl1LongClick);
            this.grpKnob.Location = new System.Drawing.Point(12, 32);
            this.grpKnob.Name = "grpKnob";
            this.grpKnob.Size = new System.Drawing.Size(533, 162);
            this.grpKnob.TabIndex = 1;
            this.grpKnob.TabStop = false;
            this.grpKnob.Text = "Knob Settings";
            // 
            // chkVRT
            // 
            this.chkVRT.AutoSize = true;
            this.chkVRT.Checked = true;
            this.chkVRT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVRT.Image = null;
            this.chkVRT.Location = new System.Drawing.Point(157, 139);
            this.chkVRT.Name = "chkVRT";
            this.chkVRT.Size = new System.Drawing.Size(121, 17);
            this.chkVRT.TabIndex = 22;
            this.chkVRT.Text = "Tuning Acceleration";
            this.chkVRT.UseVisualStyleBackColor = true;
            this.chkVRT.CheckedChanged += new System.EventHandler(this.chkVRT_CheckedChanged);
            // 
            // chkAutoClearRITXIT
            // 
            this.chkAutoClearRITXIT.AutoSize = true;
            this.chkAutoClearRITXIT.Image = null;
            this.chkAutoClearRITXIT.Location = new System.Drawing.Point(22, 139);
            this.chkAutoClearRITXIT.Name = "chkAutoClearRITXIT";
            this.chkAutoClearRITXIT.Size = new System.Drawing.Size(118, 17);
            this.chkAutoClearRITXIT.TabIndex = 21;
            this.chkAutoClearRITXIT.Text = "Auto Clear RIT/XIT";
            this.chkAutoClearRITXIT.UseVisualStyleBackColor = true;
            this.chkAutoClearRITXIT.CheckedChanged += new System.EventHandler(this.chkAutoClearRITXIT_CheckedChanged);
            // 
            // picArrowDouble
            // 
            this.picArrowDouble.Image = ((System.Drawing.Image)(resources.GetObject("picArrowDouble.Image")));
            this.picArrowDouble.Location = new System.Drawing.Point(418, 61);
            this.picArrowDouble.Name = "picArrowDouble";
            this.picArrowDouble.Size = new System.Drawing.Size(20, 30);
            this.picArrowDouble.TabIndex = 20;
            this.picArrowDouble.TabStop = false;
            // 
            // lblDoubleLongClick
            // 
            this.lblDoubleLongClick.Image = null;
            this.lblDoubleLongClick.Location = new System.Drawing.Point(446, 62);
            this.lblDoubleLongClick.Name = "lblDoubleLongClick";
            this.lblDoubleLongClick.Size = new System.Drawing.Size(36, 34);
            this.lblDoubleLongClick.TabIndex = 19;
            this.lblDoubleLongClick.Text = "Long Click";
            // 
            // picArrowB
            // 
            this.picArrowB.Image = ((System.Drawing.Image)(resources.GetObject("picArrowB.Image")));
            this.picArrowB.Location = new System.Drawing.Point(262, 61);
            this.picArrowB.Name = "picArrowB";
            this.picArrowB.Size = new System.Drawing.Size(20, 30);
            this.picArrowB.TabIndex = 18;
            this.picArrowB.TabStop = false;
            // 
            // lbl2LongClick
            // 
            this.lbl2LongClick.Image = null;
            this.lbl2LongClick.Location = new System.Drawing.Point(290, 62);
            this.lbl2LongClick.Name = "lbl2LongClick";
            this.lbl2LongClick.Size = new System.Drawing.Size(36, 34);
            this.lbl2LongClick.TabIndex = 17;
            this.lbl2LongClick.Text = "Long Click";
            // 
            // picArrowA
            // 
            this.picArrowA.Image = ((System.Drawing.Image)(resources.GetObject("picArrowA.Image")));
            this.picArrowA.Location = new System.Drawing.Point(53, 61);
            this.picArrowA.Name = "picArrowA";
            this.picArrowA.Size = new System.Drawing.Size(20, 30);
            this.picArrowA.TabIndex = 4;
            this.picArrowA.TabStop = false;
            // 
            // comboModeBDouble
            // 
            this.comboModeBDouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeBDouble.FormattingEnabled = true;
            this.comboModeBDouble.Location = new System.Drawing.Point(388, 98);
            this.comboModeBDouble.Name = "comboModeBDouble";
            this.comboModeBDouble.Size = new System.Drawing.Size(121, 21);
            this.comboModeBDouble.TabIndex = 10;
            this.comboModeBDouble.SelectedIndexChanged += new System.EventHandler(this.comboModeBDouble_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(157, 99);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 20);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lblKnobDoubleClick
            // 
            this.lblKnobDoubleClick.Image = null;
            this.lblKnobDoubleClick.Location = new System.Drawing.Point(386, 12);
            this.lblKnobDoubleClick.Name = "lblKnobDoubleClick";
            this.lblKnobDoubleClick.Size = new System.Drawing.Size(125, 13);
            this.lblKnobDoubleClick.TabIndex = 9;
            this.lblKnobDoubleClick.Text = "Double Click";
            this.lblKnobDoubleClick.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // picArrow1
            // 
            this.picArrow1.Image = ((System.Drawing.Image)(resources.GetObject("picArrow1.Image")));
            this.picArrow1.Location = new System.Drawing.Point(157, 34);
            this.picArrow1.Name = "picArrow1";
            this.picArrow1.Size = new System.Drawing.Size(60, 20);
            this.picArrow1.TabIndex = 2;
            this.picArrow1.TabStop = false;
            // 
            // comboModeADouble
            // 
            this.comboModeADouble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeADouble.FormattingEnabled = true;
            this.comboModeADouble.Location = new System.Drawing.Point(388, 33);
            this.comboModeADouble.Name = "comboModeADouble";
            this.comboModeADouble.Size = new System.Drawing.Size(121, 21);
            this.comboModeADouble.TabIndex = 8;
            this.comboModeADouble.SelectedIndexChanged += new System.EventHandler(this.comboModeADouble_SelectedIndexChanged);
            // 
            // lblASingleClick
            // 
            this.lblASingleClick.Image = null;
            this.lblASingleClick.Location = new System.Drawing.Point(148, 12);
            this.lblASingleClick.Name = "lblASingleClick";
            this.lblASingleClick.Size = new System.Drawing.Size(78, 13);
            this.lblASingleClick.TabIndex = 6;
            this.lblASingleClick.Text = "Single Click";
            this.lblASingleClick.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboModeB2
            // 
            this.comboModeB2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeB2.FormattingEnabled = true;
            this.comboModeB2.Location = new System.Drawing.Point(229, 98);
            this.comboModeB2.Name = "comboModeB2";
            this.comboModeB2.Size = new System.Drawing.Size(121, 21);
            this.comboModeB2.TabIndex = 3;
            this.comboModeB2.SelectedIndexChanged += new System.EventHandler(this.comboModeB2_SelectedIndexChanged);
            // 
            // comboModeB1
            // 
            this.comboModeB1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeB1.FormattingEnabled = true;
            this.comboModeB1.Location = new System.Drawing.Point(24, 98);
            this.comboModeB1.Name = "comboModeB1";
            this.comboModeB1.Size = new System.Drawing.Size(121, 21);
            this.comboModeB1.TabIndex = 2;
            this.comboModeB1.SelectedIndexChanged += new System.EventHandler(this.comboModeB1_SelectedIndexChanged);
            // 
            // comboModeA2
            // 
            this.comboModeA2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeA2.FormattingEnabled = true;
            this.comboModeA2.Location = new System.Drawing.Point(229, 33);
            this.comboModeA2.Name = "comboModeA2";
            this.comboModeA2.Size = new System.Drawing.Size(121, 21);
            this.comboModeA2.TabIndex = 1;
            this.comboModeA2.SelectedIndexChanged += new System.EventHandler(this.comboModeA2_SelectedIndexChanged);
            // 
            // comboModeA1
            // 
            this.comboModeA1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModeA1.FormattingEnabled = true;
            this.comboModeA1.Location = new System.Drawing.Point(24, 33);
            this.comboModeA1.Name = "comboModeA1";
            this.comboModeA1.Size = new System.Drawing.Size(121, 21);
            this.comboModeA1.TabIndex = 0;
            this.comboModeA1.SelectedIndexChanged += new System.EventHandler(this.comboModeA1_SelectedIndexChanged);
            // 
            // panelModeA2
            // 
            this.panelModeA2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeA2.Location = new System.Drawing.Point(227, 31);
            this.panelModeA2.Name = "panelModeA2";
            this.panelModeA2.Size = new System.Drawing.Size(125, 25);
            this.panelModeA2.TabIndex = 12;
            this.panelModeA2.Visible = false;
            // 
            // panelModeA1
            // 
            this.panelModeA1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeA1.Location = new System.Drawing.Point(22, 31);
            this.panelModeA1.Name = "panelModeA1";
            this.panelModeA1.Size = new System.Drawing.Size(125, 25);
            this.panelModeA1.TabIndex = 13;
            // 
            // panelModeB1
            // 
            this.panelModeB1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeB1.Location = new System.Drawing.Point(22, 96);
            this.panelModeB1.Name = "panelModeB1";
            this.panelModeB1.Size = new System.Drawing.Size(125, 25);
            this.panelModeB1.TabIndex = 13;
            this.panelModeB1.Visible = false;
            // 
            // panelModeB2
            // 
            this.panelModeB2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeB2.Location = new System.Drawing.Point(227, 96);
            this.panelModeB2.Name = "panelModeB2";
            this.panelModeB2.Size = new System.Drawing.Size(125, 25);
            this.panelModeB2.TabIndex = 14;
            this.panelModeB2.Visible = false;
            // 
            // panelModeADouble
            // 
            this.panelModeADouble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeADouble.Location = new System.Drawing.Point(386, 31);
            this.panelModeADouble.Name = "panelModeADouble";
            this.panelModeADouble.Size = new System.Drawing.Size(125, 25);
            this.panelModeADouble.TabIndex = 15;
            // 
            // panelModeBDouble
            // 
            this.panelModeBDouble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(72)))), ((int)(((byte)(239)))));
            this.panelModeBDouble.Location = new System.Drawing.Point(386, 96);
            this.panelModeBDouble.Name = "panelModeBDouble";
            this.panelModeBDouble.Size = new System.Drawing.Size(125, 25);
            this.panelModeBDouble.TabIndex = 16;
            this.panelModeBDouble.Visible = false;
            // 
            // lbl1LongClick
            // 
            this.lbl1LongClick.Image = null;
            this.lbl1LongClick.Location = new System.Drawing.Point(81, 62);
            this.lbl1LongClick.Name = "lbl1LongClick";
            this.lbl1LongClick.Size = new System.Drawing.Size(36, 34);
            this.lbl1LongClick.TabIndex = 4;
            this.lbl1LongClick.Text = "Long Click";
            // 
            // radModeAdvanced
            // 
            this.radModeAdvanced.AutoSize = true;
            this.radModeAdvanced.Image = null;
            this.radModeAdvanced.Location = new System.Drawing.Point(90, 9);
            this.radModeAdvanced.Name = "radModeAdvanced";
            this.radModeAdvanced.Size = new System.Drawing.Size(74, 17);
            this.radModeAdvanced.TabIndex = 9;
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
            this.radModeBasic.TabIndex = 8;
            this.radModeBasic.TabStop = true;
            this.radModeBasic.Text = "Basic";
            this.radModeBasic.UseVisualStyleBackColor = true;
            this.radModeBasic.CheckedChanged += new System.EventHandler(this.radModeBasic_CheckedChanged);
            // 
            // FlexControlAdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 434);
            this.Controls.Add(this.radModeAdvanced);
            this.Controls.Add(this.radModeBasic);
            this.Controls.Add(this.chkAutoDetect);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.grpButton);
            this.Controls.Add(this.grpKnob);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlexControlAdvancedForm";
            this.Text = "FlexControl - Advanced";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlexControlForm_FormClosing);
            this.grpButton.ResumeLayout(false);
            this.grpButton.PerformLayout();
            this.grpKnob.ResumeLayout(false);
            this.grpKnob.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowDouble)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrowA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picArrow1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBoxTS comboModeA1;
        private System.Windows.Forms.GroupBoxTS grpKnob;
        private System.Windows.Forms.LabelTS lbl1LongClick;
        private System.Windows.Forms.ComboBoxTS comboModeB2;
        private System.Windows.Forms.ComboBoxTS comboModeB1;
        private System.Windows.Forms.ComboBoxTS comboModeA2;
        private System.Windows.Forms.LabelTS lblASingleClick;
        private System.Windows.Forms.LabelTS lblKnobDoubleClick;
        private System.Windows.Forms.ComboBoxTS comboModeADouble;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picArrow1;
        private System.Windows.Forms.ComboBoxTS comboModeBDouble;
        private System.Windows.Forms.Panel panelModeA2;
        private System.Windows.Forms.Panel panelModeA1;
        private System.Windows.Forms.Panel panelModeB1;
        private System.Windows.Forms.Panel panelModeB2;
        private System.Windows.Forms.Panel panelModeADouble;
        private System.Windows.Forms.Panel panelModeBDouble;
        private System.Windows.Forms.PictureBox picArrowA;
        private System.Windows.Forms.PictureBox picArrowDouble;
        private System.Windows.Forms.LabelTS lblDoubleLongClick;
        private System.Windows.Forms.PictureBox picArrowB;
        private System.Windows.Forms.LabelTS lbl2LongClick;
        private System.Windows.Forms.GroupBoxTS grpButton;
        private System.Windows.Forms.ComboBoxTS comboRightDouble;
        private System.Windows.Forms.ComboBoxTS comboRightSingle;
        private System.Windows.Forms.ComboBoxTS comboMidDouble;
        private System.Windows.Forms.ComboBoxTS comboLeftDouble;
        private System.Windows.Forms.ComboBoxTS comboMidSingle;
        private System.Windows.Forms.ComboBoxTS comboLeftSingle;
        private System.Windows.Forms.Panel panelMidSingle;
        private System.Windows.Forms.Panel panelLeftSingle;
        private System.Windows.Forms.Panel panelLeftDouble;
        private System.Windows.Forms.Panel panelMidDouble;
        private System.Windows.Forms.Panel panelRightSingle;
        private System.Windows.Forms.Panel panelRightDouble;
        private System.Windows.Forms.ComboBoxTS comboRightLong;
        private System.Windows.Forms.ComboBoxTS comboMidLong;
        private System.Windows.Forms.ComboBoxTS comboLeftLong;
        private System.Windows.Forms.Panel panelLeftLong;
        private System.Windows.Forms.Panel panelMidLong;
        private System.Windows.Forms.Panel panelRightLong;
        private System.Windows.Forms.LabelTS lblButtonLeft;
        private System.Windows.Forms.LabelTS lblLongClick;
        private System.Windows.Forms.LabelTS lblDoubleClick;
        private System.Windows.Forms.LabelTS lblSingleClick;
        private System.Windows.Forms.LabelTS lblButtonRight;
        private System.Windows.Forms.LabelTS lblButtonMiddle;
        private System.Windows.Forms.ButtonTS btnDefaults;
        private System.Windows.Forms.CheckBoxTS chkAutoDetect;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBoxTS chkAutoClearRITXIT;
        private System.Windows.Forms.CheckBoxTS chkVRT;
        private System.Windows.Forms.RadioButtonTS radModeAdvanced;
        private System.Windows.Forms.RadioButtonTS radModeBasic;
    }
}