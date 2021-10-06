//=================================================================
// FLEX1500DebugForm.designer.cs
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

namespace PowerSDR
{
    partial class FLEX1500DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX1500DebugForm));
            this.btnWrite = new System.Windows.Forms.Button();
            this.txtOpcode = new System.Windows.Forms.TextBox();
            this.lblOpcode = new System.Windows.Forms.Label();
            this.lblParam1 = new System.Windows.Forms.Label();
            this.txtParam1 = new System.Windows.Forms.TextBox();
            this.lblParam2 = new System.Windows.Forms.Label();
            this.txtParam2 = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.chkLED = new System.Windows.Forms.CheckBox();
            this.grpLowLevel = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEEReadFloat = new System.Windows.Forms.Button();
            this.btnEEWrite = new System.Windows.Forms.Button();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.lblEEResult = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.txtEERead = new System.Windows.Forms.TextBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnEERead = new System.Windows.Forms.Button();
            this.lblData = new System.Windows.Forms.Label();
            this.lblNumBytes = new System.Windows.Forms.Label();
            this.txtNumBytes = new System.Windows.Forms.TextBox();
            this.grpI2c = new System.Windows.Forms.GroupBox();
            this.btnI2CRead = new System.Windows.Forms.Button();
            this.btnI2CWrite1 = new System.Windows.Forms.Button();
            this.txtI2CAddr = new System.Windows.Forms.TextBox();
            this.lblI2CResult = new System.Windows.Forms.Label();
            this.lblI2CAddr = new System.Windows.Forms.Label();
            this.txtI2CResult = new System.Windows.Forms.TextBox();
            this.txtI2CByte1 = new System.Windows.Forms.TextBox();
            this.btnI2CWrite2 = new System.Windows.Forms.Button();
            this.lblI2CByte1 = new System.Windows.Forms.Label();
            this.lblI2CByte2 = new System.Windows.Forms.Label();
            this.txtI2CByte2 = new System.Windows.Forms.TextBox();
            this.grpGPIO = new System.Windows.Forms.GroupBox();
            this.btnGPIOWrite1 = new System.Windows.Forms.Button();
            this.lblGPIOResult = new System.Windows.Forms.Label();
            this.txtGPIOResult = new System.Windows.Forms.TextBox();
            this.txtGPIOByte = new System.Windows.Forms.TextBox();
            this.btnGPIORead3 = new System.Windows.Forms.Button();
            this.lblGPIOByte = new System.Windows.Forms.Label();
            this.txtTune = new System.Windows.Forms.TextBox();
            this.btnTune = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnEEPROMRead1 = new System.Windows.Forms.ButtonTS();
            this.buttonTS2 = new System.Windows.Forms.ButtonTS();
            this.buttonTS1 = new System.Windows.Forms.ButtonTS();
            this.buttonTS3 = new System.Windows.Forms.ButtonTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.grpLowLevel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpI2c.SuspendLayout();
            this.grpGPIO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(16, 29);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 0;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // txtOpcode
            // 
            this.txtOpcode.Location = new System.Drawing.Point(103, 33);
            this.txtOpcode.Name = "txtOpcode";
            this.txtOpcode.Size = new System.Drawing.Size(45, 20);
            this.txtOpcode.TabIndex = 1;
            this.txtOpcode.Text = "128";
            // 
            // lblOpcode
            // 
            this.lblOpcode.AutoSize = true;
            this.lblOpcode.Location = new System.Drawing.Point(103, 17);
            this.lblOpcode.Name = "lblOpcode";
            this.lblOpcode.Size = new System.Drawing.Size(45, 13);
            this.lblOpcode.TabIndex = 2;
            this.lblOpcode.Text = "Opcode";
            // 
            // lblParam1
            // 
            this.lblParam1.AutoSize = true;
            this.lblParam1.Location = new System.Drawing.Point(156, 17);
            this.lblParam1.Name = "lblParam1";
            this.lblParam1.Size = new System.Drawing.Size(43, 13);
            this.lblParam1.TabIndex = 4;
            this.lblParam1.Text = "Param1";
            // 
            // txtParam1
            // 
            this.txtParam1.Location = new System.Drawing.Point(154, 33);
            this.txtParam1.Name = "txtParam1";
            this.txtParam1.Size = new System.Drawing.Size(45, 20);
            this.txtParam1.TabIndex = 3;
            this.txtParam1.Text = "1";
            // 
            // lblParam2
            // 
            this.lblParam2.AutoSize = true;
            this.lblParam2.Location = new System.Drawing.Point(208, 17);
            this.lblParam2.Name = "lblParam2";
            this.lblParam2.Size = new System.Drawing.Size(43, 13);
            this.lblParam2.TabIndex = 6;
            this.lblParam2.Text = "Param2";
            // 
            // txtParam2
            // 
            this.txtParam2.Location = new System.Drawing.Point(206, 33);
            this.txtParam2.Name = "txtParam2";
            this.txtParam2.Size = new System.Drawing.Size(45, 20);
            this.txtParam2.TabIndex = 5;
            this.txtParam2.Text = "0";
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(16, 75);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(103, 64);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(37, 13);
            this.lblResult.TabIndex = 9;
            this.lblResult.Text = "Result";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(103, 80);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(45, 20);
            this.txtResult.TabIndex = 8;
            // 
            // chkLED
            // 
            this.chkLED.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLED.AutoSize = true;
            this.chkLED.Location = new System.Drawing.Point(12, 260);
            this.chkLED.Name = "chkLED";
            this.chkLED.Size = new System.Drawing.Size(38, 23);
            this.chkLED.TabIndex = 10;
            this.chkLED.Text = "LED";
            this.chkLED.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkLED.UseVisualStyleBackColor = true;
            this.chkLED.CheckedChanged += new System.EventHandler(this.chkLED_CheckedChanged);
            // 
            // grpLowLevel
            // 
            this.grpLowLevel.Controls.Add(this.btnWrite);
            this.grpLowLevel.Controls.Add(this.txtOpcode);
            this.grpLowLevel.Controls.Add(this.lblResult);
            this.grpLowLevel.Controls.Add(this.lblOpcode);
            this.grpLowLevel.Controls.Add(this.txtResult);
            this.grpLowLevel.Controls.Add(this.txtParam1);
            this.grpLowLevel.Controls.Add(this.btnRead);
            this.grpLowLevel.Controls.Add(this.lblParam1);
            this.grpLowLevel.Controls.Add(this.lblParam2);
            this.grpLowLevel.Controls.Add(this.txtParam2);
            this.grpLowLevel.Location = new System.Drawing.Point(12, 12);
            this.grpLowLevel.Name = "grpLowLevel";
            this.grpLowLevel.Size = new System.Drawing.Size(268, 118);
            this.grpLowLevel.TabIndex = 11;
            this.grpLowLevel.TabStop = false;
            this.grpLowLevel.Text = "Low Level Commands";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEEReadFloat);
            this.groupBox1.Controls.Add(this.btnEEWrite);
            this.groupBox1.Controls.Add(this.txtOffset);
            this.groupBox1.Controls.Add(this.lblEEResult);
            this.groupBox1.Controls.Add(this.lblOffset);
            this.groupBox1.Controls.Add(this.txtEERead);
            this.groupBox1.Controls.Add(this.txtData);
            this.groupBox1.Controls.Add(this.btnEERead);
            this.groupBox1.Controls.Add(this.lblData);
            this.groupBox1.Controls.Add(this.lblNumBytes);
            this.groupBox1.Controls.Add(this.txtNumBytes);
            this.groupBox1.Location = new System.Drawing.Point(12, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 118);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EEPROM";
            // 
            // btnEEReadFloat
            // 
            this.btnEEReadFloat.Location = new System.Drawing.Point(159, 75);
            this.btnEEReadFloat.Name = "btnEEReadFloat";
            this.btnEEReadFloat.Size = new System.Drawing.Size(75, 23);
            this.btnEEReadFloat.TabIndex = 10;
            this.btnEEReadFloat.Text = "Read Float";
            this.btnEEReadFloat.UseVisualStyleBackColor = true;
            this.btnEEReadFloat.Click += new System.EventHandler(this.btnEEReadFloat_Click);
            // 
            // btnEEWrite
            // 
            this.btnEEWrite.Location = new System.Drawing.Point(16, 29);
            this.btnEEWrite.Name = "btnEEWrite";
            this.btnEEWrite.Size = new System.Drawing.Size(75, 23);
            this.btnEEWrite.TabIndex = 0;
            this.btnEEWrite.Text = "Write";
            this.btnEEWrite.UseVisualStyleBackColor = true;
            this.btnEEWrite.Click += new System.EventHandler(this.btnEEWrite_Click);
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(103, 33);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(45, 20);
            this.txtOffset.TabIndex = 1;
            // 
            // lblEEResult
            // 
            this.lblEEResult.AutoSize = true;
            this.lblEEResult.Location = new System.Drawing.Point(103, 64);
            this.lblEEResult.Name = "lblEEResult";
            this.lblEEResult.Size = new System.Drawing.Size(37, 13);
            this.lblEEResult.TabIndex = 9;
            this.lblEEResult.Text = "Result";
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(105, 17);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(35, 13);
            this.lblOffset.TabIndex = 2;
            this.lblOffset.Text = "Offset";
            // 
            // txtEERead
            // 
            this.txtEERead.Location = new System.Drawing.Point(103, 80);
            this.txtEERead.Name = "txtEERead";
            this.txtEERead.Size = new System.Drawing.Size(45, 20);
            this.txtEERead.TabIndex = 8;
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(154, 33);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(45, 20);
            this.txtData.TabIndex = 3;
            // 
            // btnEERead
            // 
            this.btnEERead.Location = new System.Drawing.Point(16, 75);
            this.btnEERead.Name = "btnEERead";
            this.btnEERead.Size = new System.Drawing.Size(75, 23);
            this.btnEERead.TabIndex = 7;
            this.btnEERead.Text = "Read";
            this.btnEERead.UseVisualStyleBackColor = true;
            this.btnEERead.Click += new System.EventHandler(this.btnEERead_Click);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(156, 17);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(30, 13);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "Data";
            // 
            // lblNumBytes
            // 
            this.lblNumBytes.AutoSize = true;
            this.lblNumBytes.Location = new System.Drawing.Point(208, 17);
            this.lblNumBytes.Name = "lblNumBytes";
            this.lblNumBytes.Size = new System.Drawing.Size(58, 13);
            this.lblNumBytes.TabIndex = 6;
            this.lblNumBytes.Text = "Num Bytes";
            // 
            // txtNumBytes
            // 
            this.txtNumBytes.Location = new System.Drawing.Point(206, 33);
            this.txtNumBytes.Name = "txtNumBytes";
            this.txtNumBytes.Size = new System.Drawing.Size(45, 20);
            this.txtNumBytes.TabIndex = 5;
            this.txtNumBytes.Text = "1";
            // 
            // grpI2c
            // 
            this.grpI2c.Controls.Add(this.btnI2CRead);
            this.grpI2c.Controls.Add(this.btnI2CWrite1);
            this.grpI2c.Controls.Add(this.txtI2CAddr);
            this.grpI2c.Controls.Add(this.lblI2CResult);
            this.grpI2c.Controls.Add(this.lblI2CAddr);
            this.grpI2c.Controls.Add(this.txtI2CResult);
            this.grpI2c.Controls.Add(this.txtI2CByte1);
            this.grpI2c.Controls.Add(this.btnI2CWrite2);
            this.grpI2c.Controls.Add(this.lblI2CByte1);
            this.grpI2c.Controls.Add(this.lblI2CByte2);
            this.grpI2c.Controls.Add(this.txtI2CByte2);
            this.grpI2c.Location = new System.Drawing.Point(286, 12);
            this.grpI2c.Name = "grpI2c";
            this.grpI2c.Size = new System.Drawing.Size(268, 118);
            this.grpI2c.TabIndex = 15;
            this.grpI2c.TabStop = false;
            this.grpI2c.Text = "I2C";
            // 
            // btnI2CRead
            // 
            this.btnI2CRead.Location = new System.Drawing.Point(167, 75);
            this.btnI2CRead.Name = "btnI2CRead";
            this.btnI2CRead.Size = new System.Drawing.Size(75, 23);
            this.btnI2CRead.TabIndex = 10;
            this.btnI2CRead.Text = "Read";
            this.btnI2CRead.UseVisualStyleBackColor = true;
            this.btnI2CRead.Click += new System.EventHandler(this.btnI2CRead_Click);
            // 
            // btnI2CWrite1
            // 
            this.btnI2CWrite1.Location = new System.Drawing.Point(16, 29);
            this.btnI2CWrite1.Name = "btnI2CWrite1";
            this.btnI2CWrite1.Size = new System.Drawing.Size(75, 23);
            this.btnI2CWrite1.TabIndex = 0;
            this.btnI2CWrite1.Text = "Write 1";
            this.btnI2CWrite1.UseVisualStyleBackColor = true;
            this.btnI2CWrite1.Click += new System.EventHandler(this.btnI2CWrite1_Click);
            // 
            // txtI2CAddr
            // 
            this.txtI2CAddr.Location = new System.Drawing.Point(103, 33);
            this.txtI2CAddr.Name = "txtI2CAddr";
            this.txtI2CAddr.Size = new System.Drawing.Size(45, 20);
            this.txtI2CAddr.TabIndex = 1;
            this.txtI2CAddr.Text = "E0";
            // 
            // lblI2CResult
            // 
            this.lblI2CResult.AutoSize = true;
            this.lblI2CResult.Location = new System.Drawing.Point(103, 64);
            this.lblI2CResult.Name = "lblI2CResult";
            this.lblI2CResult.Size = new System.Drawing.Size(37, 13);
            this.lblI2CResult.TabIndex = 9;
            this.lblI2CResult.Text = "Result";
            // 
            // lblI2CAddr
            // 
            this.lblI2CAddr.AutoSize = true;
            this.lblI2CAddr.Location = new System.Drawing.Point(103, 17);
            this.lblI2CAddr.Name = "lblI2CAddr";
            this.lblI2CAddr.Size = new System.Drawing.Size(45, 13);
            this.lblI2CAddr.TabIndex = 2;
            this.lblI2CAddr.Text = "Address";
            // 
            // txtI2CResult
            // 
            this.txtI2CResult.Location = new System.Drawing.Point(103, 80);
            this.txtI2CResult.Name = "txtI2CResult";
            this.txtI2CResult.Size = new System.Drawing.Size(45, 20);
            this.txtI2CResult.TabIndex = 8;
            // 
            // txtI2CByte1
            // 
            this.txtI2CByte1.Location = new System.Drawing.Point(154, 33);
            this.txtI2CByte1.Name = "txtI2CByte1";
            this.txtI2CByte1.Size = new System.Drawing.Size(45, 20);
            this.txtI2CByte1.TabIndex = 3;
            this.txtI2CByte1.Text = "4";
            // 
            // btnI2CWrite2
            // 
            this.btnI2CWrite2.Location = new System.Drawing.Point(16, 75);
            this.btnI2CWrite2.Name = "btnI2CWrite2";
            this.btnI2CWrite2.Size = new System.Drawing.Size(75, 23);
            this.btnI2CWrite2.TabIndex = 7;
            this.btnI2CWrite2.Text = "Write 2";
            this.btnI2CWrite2.UseVisualStyleBackColor = true;
            this.btnI2CWrite2.Click += new System.EventHandler(this.btnI2CWrite2_Click);
            // 
            // lblI2CByte1
            // 
            this.lblI2CByte1.AutoSize = true;
            this.lblI2CByte1.Location = new System.Drawing.Point(156, 17);
            this.lblI2CByte1.Name = "lblI2CByte1";
            this.lblI2CByte1.Size = new System.Drawing.Size(34, 13);
            this.lblI2CByte1.TabIndex = 4;
            this.lblI2CByte1.Text = "Byte1";
            // 
            // lblI2CByte2
            // 
            this.lblI2CByte2.AutoSize = true;
            this.lblI2CByte2.Location = new System.Drawing.Point(208, 17);
            this.lblI2CByte2.Name = "lblI2CByte2";
            this.lblI2CByte2.Size = new System.Drawing.Size(34, 13);
            this.lblI2CByte2.TabIndex = 6;
            this.lblI2CByte2.Text = "Byte2";
            // 
            // txtI2CByte2
            // 
            this.txtI2CByte2.Location = new System.Drawing.Point(206, 33);
            this.txtI2CByte2.Name = "txtI2CByte2";
            this.txtI2CByte2.Size = new System.Drawing.Size(45, 20);
            this.txtI2CByte2.TabIndex = 5;
            this.txtI2CByte2.Text = "0";
            // 
            // grpGPIO
            // 
            this.grpGPIO.Controls.Add(this.btnGPIOWrite1);
            this.grpGPIO.Controls.Add(this.lblGPIOResult);
            this.grpGPIO.Controls.Add(this.txtGPIOResult);
            this.grpGPIO.Controls.Add(this.txtGPIOByte);
            this.grpGPIO.Controls.Add(this.btnGPIORead3);
            this.grpGPIO.Controls.Add(this.lblGPIOByte);
            this.grpGPIO.Location = new System.Drawing.Point(286, 136);
            this.grpGPIO.Name = "grpGPIO";
            this.grpGPIO.Size = new System.Drawing.Size(268, 118);
            this.grpGPIO.TabIndex = 16;
            this.grpGPIO.TabStop = false;
            this.grpGPIO.Text = "GPIO";
            // 
            // btnGPIOWrite1
            // 
            this.btnGPIOWrite1.Location = new System.Drawing.Point(16, 29);
            this.btnGPIOWrite1.Name = "btnGPIOWrite1";
            this.btnGPIOWrite1.Size = new System.Drawing.Size(75, 23);
            this.btnGPIOWrite1.TabIndex = 0;
            this.btnGPIOWrite1.Text = "Write 1";
            this.btnGPIOWrite1.UseVisualStyleBackColor = true;
            this.btnGPIOWrite1.Click += new System.EventHandler(this.btnGPIOWrite1_Click);
            // 
            // lblGPIOResult
            // 
            this.lblGPIOResult.AutoSize = true;
            this.lblGPIOResult.Location = new System.Drawing.Point(108, 64);
            this.lblGPIOResult.Name = "lblGPIOResult";
            this.lblGPIOResult.Size = new System.Drawing.Size(37, 13);
            this.lblGPIOResult.TabIndex = 9;
            this.lblGPIOResult.Text = "Result";
            // 
            // txtGPIOResult
            // 
            this.txtGPIOResult.Location = new System.Drawing.Point(106, 80);
            this.txtGPIOResult.Name = "txtGPIOResult";
            this.txtGPIOResult.Size = new System.Drawing.Size(45, 20);
            this.txtGPIOResult.TabIndex = 8;
            // 
            // txtGPIOByte
            // 
            this.txtGPIOByte.Location = new System.Drawing.Point(106, 33);
            this.txtGPIOByte.Name = "txtGPIOByte";
            this.txtGPIOByte.Size = new System.Drawing.Size(45, 20);
            this.txtGPIOByte.TabIndex = 3;
            this.txtGPIOByte.Text = "4";
            // 
            // btnGPIORead3
            // 
            this.btnGPIORead3.Location = new System.Drawing.Point(16, 75);
            this.btnGPIORead3.Name = "btnGPIORead3";
            this.btnGPIORead3.Size = new System.Drawing.Size(75, 23);
            this.btnGPIORead3.TabIndex = 7;
            this.btnGPIORead3.Text = "Read 3";
            this.btnGPIORead3.UseVisualStyleBackColor = true;
            this.btnGPIORead3.Click += new System.EventHandler(this.btnGPIOWrite3_Click);
            // 
            // lblGPIOByte
            // 
            this.lblGPIOByte.AutoSize = true;
            this.lblGPIOByte.Location = new System.Drawing.Point(108, 17);
            this.lblGPIOByte.Name = "lblGPIOByte";
            this.lblGPIOByte.Size = new System.Drawing.Size(28, 13);
            this.lblGPIOByte.TabIndex = 4;
            this.lblGPIOByte.Text = "Byte";
            // 
            // txtTune
            // 
            this.txtTune.Location = new System.Drawing.Point(321, 269);
            this.txtTune.Name = "txtTune";
            this.txtTune.Size = new System.Drawing.Size(100, 20);
            this.txtTune.TabIndex = 17;
            // 
            // btnTune
            // 
            this.btnTune.Location = new System.Drawing.Point(427, 269);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(49, 23);
            this.btnTune.TabIndex = 18;
            this.btnTune.Text = "Tune";
            this.btnTune.UseVisualStyleBackColor = true;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(204, 309);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 53;
            this.toolTip1.SetToolTip(this.numericUpDown1, resources.GetString("numericUpDown1.ToolTip"));
            // 
            // btnEEPROMRead1
            // 
            this.btnEEPROMRead1.Image = null;
            this.btnEEPROMRead1.Location = new System.Drawing.Point(402, 399);
            this.btnEEPROMRead1.Name = "btnEEPROMRead1";
            this.btnEEPROMRead1.Size = new System.Drawing.Size(152, 23);
            this.btnEEPROMRead1.TabIndex = 55;
            this.btnEEPROMRead1.Text = "Record ALL EEPROM";
            this.toolTip1.SetToolTip(this.btnEEPROMRead1, "Downloads and saves a text file of the EEPROM chip\r\nto your database folder. USBE" +
        "EPROMDATA.txt\r\nIt takes almost 1 minute to dump");
            this.btnEEPROMRead1.Click += new System.EventHandler(this.btnEEPROMRead1_Click);
            // 
            // buttonTS2
            // 
            this.buttonTS2.Image = null;
            this.buttonTS2.Location = new System.Drawing.Point(302, 352);
            this.buttonTS2.Name = "buttonTS2";
            this.buttonTS2.Size = new System.Drawing.Size(58, 23);
            this.buttonTS2.TabIndex = 52;
            this.buttonTS2.Text = "Normal";
            this.toolTip1.SetToolTip(this.buttonTS2, "Returns radio back to normal band operation");
            this.buttonTS2.Click += new System.EventHandler(this.buttonTS2_Click);
            // 
            // buttonTS1
            // 
            this.buttonTS1.Image = null;
            this.buttonTS1.Location = new System.Drawing.Point(115, 352);
            this.buttonTS1.Name = "buttonTS1";
            this.buttonTS1.Size = new System.Drawing.Size(181, 23);
            this.buttonTS1.TabIndex = 50;
            this.buttonTS1.Text = "MARS/CAP/SHARES";
            this.toolTip1.SetToolTip(this.buttonTS1, resources.GetString("buttonTS1.ToolTip"));
            this.buttonTS1.Click += new System.EventHandler(this.buttonTS1_Click);
            // 
            // buttonTS3
            // 
            this.buttonTS3.Image = null;
            this.buttonTS3.Location = new System.Drawing.Point(268, 308);
            this.buttonTS3.Name = "buttonTS3";
            this.buttonTS3.Size = new System.Drawing.Size(88, 23);
            this.buttonTS3.TabIndex = 54;
            this.buttonTS3.Text = "Update Turf";
            this.buttonTS3.Click += new System.EventHandler(this.buttonTS3_Click);
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(89, 313);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(112, 16);
            this.labelTS1.TabIndex = 49;
            this.labelTS1.Text = "Select Turf Region:";
            // 
            // FLEX1500DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 386);
            this.Controls.Add(this.btnEEPROMRead1);
            this.Controls.Add(this.buttonTS3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelTS1);
            this.Controls.Add(this.buttonTS2);
            this.Controls.Add(this.buttonTS1);
            this.Controls.Add(this.btnTune);
            this.Controls.Add(this.txtTune);
            this.Controls.Add(this.grpGPIO);
            this.Controls.Add(this.grpI2c);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpLowLevel);
            this.Controls.Add(this.chkLED);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FLEX1500DebugForm";
            this.Text = "FLEX-1500 Debug";
            this.grpLowLevel.ResumeLayout(false);
            this.grpLowLevel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpI2c.ResumeLayout(false);
            this.grpI2c.PerformLayout();
            this.grpGPIO.ResumeLayout(false);
            this.grpGPIO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TextBox txtOpcode;
        private System.Windows.Forms.Label lblOpcode;
        private System.Windows.Forms.Label lblParam1;
        private System.Windows.Forms.TextBox txtParam1;
        private System.Windows.Forms.Label lblParam2;
        private System.Windows.Forms.TextBox txtParam2;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.CheckBox chkLED;
        private System.Windows.Forms.GroupBox grpLowLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEEWrite;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.Label lblEEResult;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.TextBox txtEERead;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnEERead;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblNumBytes;
        private System.Windows.Forms.TextBox txtNumBytes;
        private System.Windows.Forms.GroupBox grpI2c;
        private System.Windows.Forms.Button btnI2CWrite1;
        private System.Windows.Forms.TextBox txtI2CAddr;
        private System.Windows.Forms.Label lblI2CResult;
        private System.Windows.Forms.Label lblI2CAddr;
        private System.Windows.Forms.TextBox txtI2CResult;
        private System.Windows.Forms.TextBox txtI2CByte1;
        private System.Windows.Forms.Button btnI2CWrite2;
        private System.Windows.Forms.Label lblI2CByte1;
        private System.Windows.Forms.Label lblI2CByte2;
        private System.Windows.Forms.TextBox txtI2CByte2;
        private System.Windows.Forms.GroupBox grpGPIO;
        private System.Windows.Forms.Button btnGPIOWrite1;
        private System.Windows.Forms.Label lblGPIOResult;
        private System.Windows.Forms.TextBox txtGPIOResult;
        private System.Windows.Forms.TextBox txtGPIOByte;
        private System.Windows.Forms.Button btnGPIORead3;
        private System.Windows.Forms.Label lblGPIOByte;
        private System.Windows.Forms.TextBox txtTune;
        private System.Windows.Forms.Button btnTune;
        private System.Windows.Forms.Button btnI2CRead;
        private System.Windows.Forms.Button btnEEReadFloat;
        private System.Windows.Forms.ButtonTS buttonTS3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.LabelTS labelTS1;
        private System.Windows.Forms.ButtonTS buttonTS2;
        private System.Windows.Forms.ButtonTS buttonTS1;
        private System.Windows.Forms.ButtonTS btnEEPROMRead1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

