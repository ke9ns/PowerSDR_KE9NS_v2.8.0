//=================================================================
// scan.cs
// created by Darrin Kohn ke9ns
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
//
//=================================================================

//=================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;                    // ke9ns add for stringbuilder
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerSDR
{
    public class ScanControl : System.Windows.Forms.Form
    {



        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";


        private System.Windows.Forms.GroupBoxTS grpPlayback;
        private System.Windows.Forms.GroupBox grpPlaylist;
        private System.Windows.Forms.MainMenu mainMenu1;
        private Button button1;
        private Button btnCustomList;
        private Button btnBandstack;
        private Button btnGroupMemory;
        public TextBox lowFBox;
        private TextBox textBox3;
        private Label label2;
        public TextBox highFBox;
        private Label label4;
        private Label label5;
        public CheckBoxTS chkAlwaysOnTop;
        private GroupBoxTS grpGenCustomTitleText;
        private Label label6;
        private CheckBoxTS chkBoxSQLBRK;
        private ComboBoxTS comboMemGroupName;
        public DataGridView dataGridView2;
        public TextBox currFBox;
        private Label label3;
        private Label label7;
        private Label label8;
        private Label label9;
        private ComboBoxTS comboBoxTS1;
        private Button pausebtn;
        private NumericUpDownTS udspeedBox;
        private NumericUpDownTS udspeedBox1;
        private Label label10;
        private NumericUpDownTS udstepBox;
        private OpenFileDialog openFileDialog2;
        private ToolTip toolTip1;
        private NumericUpDownTS udPauseLength;
        private Label label11;
        private Label label12;
        private GroupBoxTS groupBoxTS2;
        private LabelTS labelTS27;
        private LabelTS labelTS23;
        public CheckBoxTS chkBoxIdent;
        private Label label13;
        public NumericUpDownTS udIDTimer;
        public NumericUpDownTS udIDGap;
        public NumericUpDownTS udIDThres;
        public Button button2;
        public CheckBoxTS checkBoxSWR;
        private Label label14;
        public NumericUpDownTS numericSWRTest;
        private Label label15;
        private Label label1;
        public TextBox textBox1;
        private Button btnGroupMemory1;
        private CheckBoxTS chkBoxLoop;
        private CheckBoxTS chkBoxSQLBRKWait;
        private Button button_reset;
        private IContainer components;


        #region Constructor and Destructor

        public ScanControl(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "ScanForm", true);



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.currFBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pausebtn = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.udIDGap = new System.Windows.Forms.NumericUpDownTS();
            this.udIDThres = new System.Windows.Forms.NumericUpDownTS();
            this.udIDTimer = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxIdent = new System.Windows.Forms.CheckBoxTS();
            this.udPauseLength = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxSQLBRK = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxLoop = new System.Windows.Forms.CheckBoxTS();
            this.btnGroupMemory1 = new System.Windows.Forms.Button();
            this.numericSWRTest = new System.Windows.Forms.NumericUpDownTS();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGroupMemory = new System.Windows.Forms.Button();
            this.btnCustomList = new System.Windows.Forms.Button();
            this.btnBandstack = new System.Windows.Forms.Button();
            this.checkBoxSWR = new System.Windows.Forms.CheckBoxTS();
            this.button1 = new System.Windows.Forms.Button();
            this.chkBoxSQLBRKWait = new System.Windows.Forms.CheckBoxTS();
            this.button_reset = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBoxTS2 = new System.Windows.Forms.GroupBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.label13 = new System.Windows.Forms.Label();
            this.labelTS27 = new System.Windows.Forms.LabelTS();
            this.labelTS23 = new System.Windows.Forms.LabelTS();
            this.comboMemGroupName = new System.Windows.Forms.ComboBoxTS();
            this.grpGenCustomTitleText = new System.Windows.Forms.GroupBoxTS();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.udspeedBox = new System.Windows.Forms.NumericUpDownTS();
            this.udstepBox = new System.Windows.Forms.NumericUpDownTS();
            this.udspeedBox1 = new System.Windows.Forms.NumericUpDownTS();
            this.comboBoxTS1 = new System.Windows.Forms.ComboBoxTS();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.highFBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lowFBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDThres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPauseLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSWRTest)).BeginInit();
            this.groupBoxTS2.SuspendLayout();
            this.grpGenCustomTitleText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udspeedBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udstepBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udspeedBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(18, 10);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(608, 104);
            this.textBox3.TabIndex = 9;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            this.textBox3.MouseEnter += new System.EventHandler(this.ScanControl_MouseEnter);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.Location = new System.Drawing.Point(385, 153);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView2.Size = new System.Drawing.Size(254, 94);
            this.dataGridView2.TabIndex = 76;
            this.dataGridView2.Visible = false;
            // 
            // currFBox
            // 
            this.currFBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.currFBox.BackColor = System.Drawing.Color.LightYellow;
            this.currFBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.currFBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currFBox.HideSelection = false;
            this.currFBox.Location = new System.Drawing.Point(12, 133);
            this.currFBox.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.currFBox.MaxLength = 10000000;
            this.currFBox.MinimumSize = new System.Drawing.Size(300, 100);
            this.currFBox.Multiline = true;
            this.currFBox.Name = "currFBox";
            this.currFBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.currFBox.Size = new System.Drawing.Size(620, 193);
            this.currFBox.TabIndex = 77;
            this.currFBox.TabStop = false;
            this.currFBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.currFBox_MouseDown);
            this.currFBox.MouseEnter += new System.EventHandler(this.ScanControl_MouseEnter);
            this.currFBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.currFBox_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(9, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Index";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(220, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 79;
            this.label7.Text = "Frequency";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label8.Location = new System.Drawing.Point(352, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 80;
            this.label8.Text = "Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(448, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 13);
            this.label9.TabIndex = 81;
            this.label9.Text = "Squelch, Signal, Remarks";
            // 
            // pausebtn
            // 
            this.pausebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pausebtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pausebtn.Location = new System.Drawing.Point(18, 546);
            this.pausebtn.Name = "pausebtn";
            this.pausebtn.Size = new System.Drawing.Size(81, 23);
            this.pausebtn.TabIndex = 66;
            this.pausebtn.Text = "Pause Scan";
            this.toolTip1.SetToolTip(this.pausebtn, "Turns Yellow if \"Wait on Squelch Break\" ON or \"Pause on Squelch Break\" and squelc" +
        "h Breaks.\r\n\r\nClick the Yellow \"Pause Scan\" button to jump past an occupied chann" +
        "el and continue scanning.\r\n\r\n");
            this.pausebtn.UseVisualStyleBackColor = false;
            this.pausebtn.Click += new System.EventHandler(this.pausebtn_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 12000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 40;
            // 
            // udIDGap
            // 
            this.udIDGap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udIDGap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udIDGap.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udIDGap.Location = new System.Drawing.Point(124, 36);
            this.udIDGap.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.udIDGap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udIDGap.Name = "udIDGap";
            this.udIDGap.Size = new System.Drawing.Size(55, 22);
            this.udIDGap.TabIndex = 97;
            this.toolTip1.SetToolTip(this.udIDGap, "Sets Threshold to detect signals above the noise floor.");
            this.udIDGap.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // udIDThres
            // 
            this.udIDThres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udIDThres.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udIDThres.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udIDThres.Location = new System.Drawing.Point(133, 8);
            this.udIDThres.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udIDThres.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udIDThres.Name = "udIDThres";
            this.udIDThres.Size = new System.Drawing.Size(45, 22);
            this.udIDThres.TabIndex = 96;
            this.toolTip1.SetToolTip(this.udIDThres, "Sets Threshold to detect signals above the noise floor.");
            this.udIDThres.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // udIDTimer
            // 
            this.udIDTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udIDTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udIDTimer.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udIDTimer.Location = new System.Drawing.Point(265, 8);
            this.udIDTimer.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udIDTimer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udIDTimer.Name = "udIDTimer";
            this.udIDTimer.Size = new System.Drawing.Size(53, 22);
            this.udIDTimer.TabIndex = 95;
            this.toolTip1.SetToolTip(this.udIDTimer, "How long to keep the detected peak signal on the screen.\r\nBased on how fast your " +
        "refreshing your Display.");
            this.udIDTimer.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // chkBoxIdent
            // 
            this.chkBoxIdent.Image = null;
            this.chkBoxIdent.Location = new System.Drawing.Point(6, 19);
            this.chkBoxIdent.Name = "chkBoxIdent";
            this.chkBoxIdent.Size = new System.Drawing.Size(58, 19);
            this.chkBoxIdent.TabIndex = 89;
            this.chkBoxIdent.Text = "ON";
            this.toolTip1.SetToolTip(this.chkBoxIdent, "Check to turn on Peak signal identify in the Panadapter");
            this.chkBoxIdent.CheckedChanged += new System.EventHandler(this.chkBoxIdent_CheckedChanged);
            // 
            // udPauseLength
            // 
            this.udPauseLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udPauseLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udPauseLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPauseLength.Location = new System.Drawing.Point(118, 546);
            this.udPauseLength.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udPauseLength.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPauseLength.Name = "udPauseLength";
            this.udPauseLength.Size = new System.Drawing.Size(63, 22);
            this.udPauseLength.TabIndex = 92;
            this.toolTip1.SetToolTip(this.udPauseLength, "Set the Pause on Squelch Break Time (in Seconds).\r\n\r\n0=OFF (pause forever)");
            this.udPauseLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkBoxSQLBRK
            // 
            this.chkBoxSQLBRK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSQLBRK.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxSQLBRK.Image = null;
            this.chkBoxSQLBRK.Location = new System.Drawing.Point(112, 495);
            this.chkBoxSQLBRK.Name = "chkBoxSQLBRK";
            this.chkBoxSQLBRK.Size = new System.Drawing.Size(104, 31);
            this.chkBoxSQLBRK.TabIndex = 62;
            this.chkBoxSQLBRK.Text = "Pause on Squelch Break";
            this.toolTip1.SetToolTip(this.chkBoxSQLBRK, resources.GetString("chkBoxSQLBRK.ToolTip"));
            this.chkBoxSQLBRK.CheckedChanged += new System.EventHandler(this.chkBoxSQLBRK_CheckedChanged);
            // 
            // chkBoxLoop
            // 
            this.chkBoxLoop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxLoop.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxLoop.Image = null;
            this.chkBoxLoop.Location = new System.Drawing.Point(175, 101);
            this.chkBoxLoop.Name = "chkBoxLoop";
            this.chkBoxLoop.Size = new System.Drawing.Size(60, 26);
            this.chkBoxLoop.TabIndex = 95;
            this.chkBoxLoop.Text = "Loop";
            this.toolTip1.SetToolTip(this.chkBoxLoop, "Frequency Lo-Hi Scan will Loop forever (instead of 1 time through)");
            // 
            // btnGroupMemory1
            // 
            this.btnGroupMemory1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGroupMemory1.Enabled = false;
            this.btnGroupMemory1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnGroupMemory1.Location = new System.Drawing.Point(14, 44);
            this.btnGroupMemory1.Name = "btnGroupMemory1";
            this.btnGroupMemory1.Size = new System.Drawing.Size(115, 23);
            this.btnGroupMemory1.TabIndex = 105;
            this.btnGroupMemory1.Text = "SWL Scan (RX)";
            this.toolTip1.SetToolTip(this.btnGroupMemory1, "First Select a SWL GROUP (to the Right)\r\nThen Click this Button to start scanning" +
        "");
            this.btnGroupMemory1.UseVisualStyleBackColor = true;
            this.btnGroupMemory1.Click += new System.EventHandler(this.btnGroupMemory1_Click);
            // 
            // numericSWRTest
            // 
            this.numericSWRTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericSWRTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericSWRTest.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSWRTest.Location = new System.Drawing.Point(11, 127);
            this.numericSWRTest.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericSWRTest.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSWRTest.Name = "numericSWRTest";
            this.numericSWRTest.Size = new System.Drawing.Size(34, 22);
            this.numericSWRTest.TabIndex = 98;
            this.toolTip1.SetToolTip(this.numericSWRTest, "Record up to (5) SWR plots on the same ANT and same Band\r\nand visually compare re" +
        "sults.\r\n\r\nPanadapter:  You can view up to 5 RUN\'s at one time.\r\nPanafall: You ca" +
        "n only view 1 RUN at a time. \r\n\r\n");
            this.numericSWRTest.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSWRTest.ValueChanged += new System.EventHandler(this.numericSWRTest_ValueChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button2.Location = new System.Drawing.Point(52, 103);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 48);
            this.button2.TabIndex = 92;
            this.button2.Text = "SWR Scan (TX)";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            // 
            // btnGroupMemory
            // 
            this.btnGroupMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGroupMemory.Enabled = false;
            this.btnGroupMemory.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnGroupMemory.Location = new System.Drawing.Point(14, 20);
            this.btnGroupMemory.Name = "btnGroupMemory";
            this.btnGroupMemory.Size = new System.Drawing.Size(115, 23);
            this.btnGroupMemory.TabIndex = 5;
            this.btnGroupMemory.Text = "Memory Scan (RX)";
            this.toolTip1.SetToolTip(this.btnGroupMemory, resources.GetString("btnGroupMemory.ToolTip"));
            this.btnGroupMemory.UseVisualStyleBackColor = true;
            this.btnGroupMemory.Click += new System.EventHandler(this.btnGroupMemory_Click);
            // 
            // btnCustomList
            // 
            this.btnCustomList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCustomList.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCustomList.Location = new System.Drawing.Point(496, 49);
            this.btnCustomList.Name = "btnCustomList";
            this.btnCustomList.Size = new System.Drawing.Size(108, 23);
            this.btnCustomList.TabIndex = 3;
            this.btnCustomList.Text = "Cstm List Start";
            this.toolTip1.SetToolTip(this.btnCustomList, resources.GetString("btnCustomList.ToolTip"));
            this.btnCustomList.UseVisualStyleBackColor = true;
            this.btnCustomList.Click += new System.EventHandler(this.btnCustomList_Click);
            // 
            // btnBandstack
            // 
            this.btnBandstack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBandstack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBandstack.Location = new System.Drawing.Point(496, 20);
            this.btnBandstack.Name = "btnBandstack";
            this.btnBandstack.Size = new System.Drawing.Size(108, 23);
            this.btnBandstack.TabIndex = 4;
            this.btnBandstack.Text = "BandStack Start";
            this.toolTip1.SetToolTip(this.btnBandstack, "Click to start scanning your current BandStack.");
            this.btnBandstack.UseVisualStyleBackColor = true;
            this.btnBandstack.Click += new System.EventHandler(this.btnBandstack_Click);
            // 
            // checkBoxSWR
            // 
            this.checkBoxSWR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSWR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxSWR.Image = null;
            this.checkBoxSWR.Location = new System.Drawing.Point(6, 87);
            this.checkBoxSWR.Name = "checkBoxSWR";
            this.checkBoxSWR.Size = new System.Drawing.Size(214, 17);
            this.checkBoxSWR.TabIndex = 95;
            this.checkBoxSWR.Text = "Display Recorded SWR Plot(s) to Pan";
            this.toolTip1.SetToolTip(this.checkBoxSWR, "Hit F1 for more HELP\r\n\r\nDisplay SWR Plot(s) to Panadapter display area\r\n\r\nRecords" +
        " SWR to SWR_PLOTS Folder (right click on SWR SCAN button to open)");
            this.checkBoxSWR.CheckedChanged += new System.EventHandler(this.checkBoxSWR_CheckedChanged);
            this.checkBoxSWR.MouseEnter += new System.EventHandler(this.checkBoxSWR_MouseEnter);
            this.checkBoxSWR.MouseLeave += new System.EventHandler(this.checkBoxSWR_MouseLeave);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(115, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "Low-Hi Scan (RX)";
            this.toolTip1.SetToolTip(this.button1, "Click here to start scanning from Low Freq to High Freq.\r\n\r\nYou can manually chan" +
        "ge the Low and High Freq Edges.\r\n\r\nLoop checkbox for continous Scan");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button5_Click);
            // 
            // chkBoxSQLBRKWait
            // 
            this.chkBoxSQLBRKWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSQLBRKWait.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxSQLBRKWait.Image = null;
            this.chkBoxSQLBRKWait.Location = new System.Drawing.Point(12, 495);
            this.chkBoxSQLBRKWait.Name = "chkBoxSQLBRKWait";
            this.chkBoxSQLBRKWait.Size = new System.Drawing.Size(104, 31);
            this.chkBoxSQLBRKWait.TabIndex = 95;
            this.chkBoxSQLBRKWait.Text = "Wait on Squelch Break";
            this.toolTip1.SetToolTip(this.chkBoxSQLBRKWait, resources.GetString("chkBoxSQLBRKWait.ToolTip"));
            this.chkBoxSQLBRKWait.CheckedChanged += new System.EventHandler(this.chkBoxSQLBRKWait_CheckedChanged);
            // 
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_reset.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_reset.Location = new System.Drawing.Point(334, 85);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(80, 23);
            this.button_reset.TabIndex = 106;
            this.button_reset.Text = "Edge RST";
            this.toolTip1.SetToolTip(this.button_reset, "Click to Reset Low-High Band Edges");
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(115, 530);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 92;
            this.label11.Text = "Pause Length (Sec)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label12.Location = new System.Drawing.Point(100, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 93;
            this.label12.Text = "Group";
            // 
            // groupBoxTS2
            // 
            this.groupBoxTS2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxTS2.Controls.Add(this.udIDGap);
            this.groupBoxTS2.Controls.Add(this.udIDThres);
            this.groupBoxTS2.Controls.Add(this.chkAlwaysOnTop);
            this.groupBoxTS2.Controls.Add(this.label13);
            this.groupBoxTS2.Controls.Add(this.udIDTimer);
            this.groupBoxTS2.Controls.Add(this.labelTS27);
            this.groupBoxTS2.Controls.Add(this.labelTS23);
            this.groupBoxTS2.Controls.Add(this.chkBoxIdent);
            this.groupBoxTS2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBoxTS2.Location = new System.Drawing.Point(222, 500);
            this.groupBoxTS2.Name = "groupBoxTS2";
            this.groupBoxTS2.Size = new System.Drawing.Size(410, 70);
            this.groupBoxTS2.TabIndex = 94;
            this.groupBoxTS2.TabStop = false;
            this.groupBoxTS2.Text = "Sig Ident";
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(300, 42);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 59;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(197, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 95;
            this.label13.Text = "Persistance:";
            // 
            // labelTS27
            // 
            this.labelTS27.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelTS27.Image = null;
            this.labelTS27.Location = new System.Drawing.Point(70, 36);
            this.labelTS27.Name = "labelTS27";
            this.labelTS27.Size = new System.Drawing.Size(51, 18);
            this.labelTS27.TabIndex = 91;
            this.labelTS27.Text = "Hz Gap:";
            // 
            // labelTS23
            // 
            this.labelTS23.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelTS23.Image = null;
            this.labelTS23.Location = new System.Drawing.Point(70, 8);
            this.labelTS23.Name = "labelTS23";
            this.labelTS23.Size = new System.Drawing.Size(63, 18);
            this.labelTS23.TabIndex = 89;
            this.labelTS23.Text = "dBm Thres:";
            // 
            // comboMemGroupName
            // 
            this.comboMemGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboMemGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMemGroupName.DropDownWidth = 112;
            this.comboMemGroupName.Location = new System.Drawing.Point(404, 305);
            this.comboMemGroupName.Name = "comboMemGroupName";
            this.comboMemGroupName.Size = new System.Drawing.Size(223, 21);
            this.comboMemGroupName.TabIndex = 64;
            this.comboMemGroupName.Visible = false;
            this.comboMemGroupName.SelectedIndexChanged += new System.EventHandler(this.comboMemGroupName_SelectedIndexChanged);
            // 
            // grpGenCustomTitleText
            // 
            this.grpGenCustomTitleText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpGenCustomTitleText.Controls.Add(this.button_reset);
            this.grpGenCustomTitleText.Controls.Add(this.chkBoxLoop);
            this.grpGenCustomTitleText.Controls.Add(this.btnGroupMemory1);
            this.grpGenCustomTitleText.Controls.Add(this.textBox1);
            this.grpGenCustomTitleText.Controls.Add(this.label1);
            this.grpGenCustomTitleText.Controls.Add(this.label15);
            this.grpGenCustomTitleText.Controls.Add(this.label14);
            this.grpGenCustomTitleText.Controls.Add(this.numericSWRTest);
            this.grpGenCustomTitleText.Controls.Add(this.button2);
            this.grpGenCustomTitleText.Controls.Add(this.udspeedBox);
            this.grpGenCustomTitleText.Controls.Add(this.udstepBox);
            this.grpGenCustomTitleText.Controls.Add(this.udspeedBox1);
            this.grpGenCustomTitleText.Controls.Add(this.comboBoxTS1);
            this.grpGenCustomTitleText.Controls.Add(this.label6);
            this.grpGenCustomTitleText.Controls.Add(this.btnGroupMemory);
            this.grpGenCustomTitleText.Controls.Add(this.label4);
            this.grpGenCustomTitleText.Controls.Add(this.highFBox);
            this.grpGenCustomTitleText.Controls.Add(this.btnCustomList);
            this.grpGenCustomTitleText.Controls.Add(this.btnBandstack);
            this.grpGenCustomTitleText.Controls.Add(this.checkBoxSWR);
            this.grpGenCustomTitleText.Controls.Add(this.label2);
            this.grpGenCustomTitleText.Controls.Add(this.label5);
            this.grpGenCustomTitleText.Controls.Add(this.lowFBox);
            this.grpGenCustomTitleText.Controls.Add(this.button1);
            this.grpGenCustomTitleText.Controls.Add(this.label10);
            this.grpGenCustomTitleText.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.grpGenCustomTitleText.Location = new System.Drawing.Point(12, 332);
            this.grpGenCustomTitleText.Name = "grpGenCustomTitleText";
            this.grpGenCustomTitleText.Size = new System.Drawing.Size(620, 162);
            this.grpGenCustomTitleText.TabIndex = 61;
            this.grpGenCustomTitleText.TabStop = false;
            this.grpGenCustomTitleText.Text = "Scan Type";
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(139, 44);
            this.textBox1.MaxLength = 20;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(232, 24);
            this.textBox1.TabIndex = 104;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxTS2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 103;
            this.label1.Text = "\"SWL\" Group to Scan";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 13);
            this.label15.TabIndex = 101;
            this.label15.Text = "Run#";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(549, 113);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 99;
            this.label14.Text = "(mSec)";
            // 
            // udspeedBox
            // 
            this.udspeedBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udspeedBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udspeedBox.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udspeedBox.Location = new System.Drawing.Point(392, 20);
            this.udspeedBox.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.udspeedBox.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udspeedBox.Name = "udspeedBox";
            this.udspeedBox.Size = new System.Drawing.Size(80, 22);
            this.udspeedBox.TabIndex = 89;
            this.udspeedBox.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // udstepBox
            // 
            this.udstepBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udstepBox.DecimalPlaces = 1;
            this.udstepBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udstepBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udstepBox.Location = new System.Drawing.Point(470, 129);
            this.udstepBox.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.udstepBox.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.udstepBox.Name = "udstepBox";
            this.udstepBox.Size = new System.Drawing.Size(58, 22);
            this.udstepBox.TabIndex = 91;
            this.udstepBox.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // udspeedBox1
            // 
            this.udspeedBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udspeedBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udspeedBox1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udspeedBox1.Location = new System.Drawing.Point(538, 129);
            this.udspeedBox1.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.udspeedBox1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udspeedBox1.Name = "udspeedBox1";
            this.udspeedBox1.Size = new System.Drawing.Size(63, 22);
            this.udspeedBox1.TabIndex = 90;
            this.udspeedBox1.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // comboBoxTS1
            // 
            this.comboBoxTS1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxTS1.DropDownWidth = 112;
            this.comboBoxTS1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.comboBoxTS1.Location = new System.Drawing.Point(139, 20);
            this.comboBoxTS1.Name = "comboBoxTS1";
            this.comboBoxTS1.Size = new System.Drawing.Size(234, 21);
            this.comboBoxTS1.TabIndex = 65;
            this.comboBoxTS1.SelectedIndexChanged += new System.EventHandler(this.comboBoxTS1_SelectedIndexChanged);
            this.comboBoxTS1.TextUpdate += new System.EventHandler(this.comboBoxTS1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "\"Memory\" Group to Scan";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(471, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Step in Khz";
            // 
            // highFBox
            // 
            this.highFBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.highFBox.BackColor = System.Drawing.Color.LightYellow;
            this.highFBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highFBox.Location = new System.Drawing.Point(324, 127);
            this.highFBox.Name = "highFBox";
            this.highFBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.highFBox.Size = new System.Drawing.Size(138, 24);
            this.highFBox.TabIndex = 21;
            this.highFBox.Click += new System.EventHandler(this.highFBox_Click);
            this.highFBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.highFBox_KeyDown);
            this.highFBox.MouseLeave += new System.EventHandler(this.highFBox_MouseLeave);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Low Edge --  (MHz)  --  High Edge";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Scan Speed (in mSec)";
            // 
            // lowFBox
            // 
            this.lowFBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lowFBox.BackColor = System.Drawing.Color.LightYellow;
            this.lowFBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowFBox.Location = new System.Drawing.Point(175, 127);
            this.lowFBox.Name = "lowFBox";
            this.lowFBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lowFBox.Size = new System.Drawing.Size(139, 24);
            this.lowFBox.TabIndex = 6;
            this.lowFBox.Click += new System.EventHandler(this.lowFBox_Click);
            this.lowFBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lowFBox_KeyDown);
            this.lowFBox.MouseLeave += new System.EventHandler(this.lowFBox_MouseLeave);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(538, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 90;
            this.label10.Text = "Scan Speed";
            // 
            // ScanControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(644, 577);
            this.Controls.Add(this.groupBoxTS2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.udPauseLength);
            this.Controls.Add(this.pausebtn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboMemGroupName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currFBox);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.chkBoxSQLBRK);
            this.Controls.Add(this.grpGenCustomTitleText);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.chkBoxSQLBRKWait);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(590, 458);
            this.Name = "ScanControl";
            this.Text = "Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScanControl_FormClosing);
            this.Load += new System.EventHandler(this.ScanControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanControl_KeyDown);
            this.MouseEnter += new System.EventHandler(this.ScanControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ScanControl_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDThres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udIDTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPauseLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSWRTest)).EndInit();
            this.groupBoxTS2.ResumeLayout(false);
            this.groupBoxTS2.PerformLayout();
            this.grpGenCustomTitleText.ResumeLayout(false);
            this.grpGenCustomTitleText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udspeedBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udstepBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udspeedBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }





        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion




        //====================================================================================================
        int NamesTot = 0; // total number of unique Group names found in the memory list (no repeats)
        string[] Names = new string[1000]; // all the Group names found in the memory list (no repeats)

        StringBuilder sb = new StringBuilder();

        private void ScanControl_Load(object sender, EventArgs e)
        {

            comboMemGroupName.DataSource = console.MemoryList.List; // upon loading, load up the current memory listing into the combobox
            comboMemGroupName.DisplayMember = "Group";
            comboMemGroupName.ValueMember = "Group";

            dataGridView2.DataSource = console.MemoryList.List;   // ke9ns get list of memories from memorylist.cs is where the file is opened and saved
            Debug.WriteLine("Rows Count " + dataGridView2.Rows.Count);
            memcount = dataGridView2.Rows.Count;

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {

                for (int y = 0; y < NamesTot; y++) // recheck all prior names found 
                {
                    if (dataGridView2[0, i].Value.ToString() == Names[y]) // check if index matches name
                    {
                        goto rt1;
                    }
                }

                comboBoxTS1.Items.Add(dataGridView2[0, i].Value.ToString());  // accumulate a combobox list of Group Memory names (no repeats)

                Names[NamesTot] = dataGridView2[0, i].Value.ToString(); // save new name for list
                NamesTot++;
                continue;

            rt1:
                Debug.WriteLine("Scanner Load groups: Found repeat ");

            } // for i loop

            // comboBoxTS1.DataSource = Names;


        } // ScanControl_Load

        private int band_index;
        public static byte ScanStop = 1; // controlled from console 0=run, 1=stop

        public static byte ScanRST = 0; // 1= pick up where you left off, 0=reset back to low_freq
        private string last_band;                           // Used in bandstacking algorithm

        public static bool ScanPause = false; // pause = true
        public static bool ScanRun = false; // run = true
        //=======================================================================================
        // ke9ns add    scan just the Band stacking reg 
        private void btnBandstack_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;

            if (ScanRun == false) // if stopped
            {
                try
                {
                    // check to see if you are on a band button
                    for (nnn = 0; nnn < 41; nnn++)   // total number of possible Bands
                    {
                        if (band_list[nnn] == console.last_band) break; // this is the current band_list index 
                        if (nnn >= 40)
                        {
                            Debug.WriteLine("console.last_Band doesnt match anything");
                            return;
                        }
                    }
                }
                catch (Exception q)
                {
                    Debug.WriteLine("console.last_Band " + q);
                    return;
                }

                ScanRun = true; // start up the scanner

                currFBox.Text = "";
                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }



                UpdateText2();

                Thread t = new Thread(new ThreadStart(SCAN2));
                t.Name = "Bandstack memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }

        } // btnBandstack_Click

        public string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };



        public int nnn = 0; // 0-41 based on last_band
        public double[] freq2 = new double[20];

        public string[] filter1 = new string[20];
        public string[] filter2 = new string[20]; // ke9ns add   F4 would indicate a unlocked bandstack memory, but F4L would indicate its a locked bandstank memory
        public string[] mode1 = new string[20];

        //========================================================================================
        //  BANDSTACK CURRFBOX text update
        void UpdateText2()
        {
            string filter, mode;
            double freq;

            for (nnn = 0; nnn < 41; nnn++) // total number of possible Bands
            {
                if (band_list[nnn] == console.last_band) break; // this is the current band_list index 

            }

            string temp1 = "";

            memtotal = 0;

            for (int ii = 0; ii < console.band_stacks[nnn]; ii++)
            {

                if (DB.GetBandStack(band_list[nnn], ii, out mode, out filter, out freq))
                {

                    string freq3 = freq.ToString("###0.000000"); // was N6 4 less than having index numbers

                    string name = console.last_band.ToString();

                    string mm = "BandStack Memories";

                    if (memsignal[ii] == null) memsignal[ii] = " ";

                    //  temp1 = temp1 + (ii + 1).ToString().PadLeft(2)       + ": " + freq3.PadLeft(12).Substring(0, 12) + " , " + name.PadRight(20).Substring(0, 20) + " , " + memsignal[ii].PadRight(20).Substring(0,20) + "\r\n";
                    temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq3.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long

                    memIndex[memtotal] = ii;
                    memtotal++; // test1

                } // if bandstack available for band
                else
                {
                    Debug.WriteLine("no bandstack for band " + band_list[nnn]);
                    break;
                }

            } // for


            currFBox.Text = temp1;

        } // UpdateText2()  BANDSTACK 

        //===========================================================================
        // Thread bandstack scanner
        private void SCAN2()
        {

            scantype = 2;

            ST2.Reset();
            ST3.Reset();

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnBandstack.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            Debug.WriteLine("CONSOLE LAST BAND " + console.last_band + " , " + console.RX1Band);


            last_band = console.last_band; // get current band stack you are viewing now

            do // ScanRun
            {

                for (; ; )
                {

                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    incbandstack(); // go to next bandstack memory

                    currFBox.SelectionStart = band_index * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;
                    //-------------------------------------------------------
                    // timer
                    do // scan speed and scanPause
                    {
                        Thread.Sleep(50);

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;
                        }

                        if (SIG > lastSIG)
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[band_index] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", Squelch Break";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText2(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;

                                }
                                break;

                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText2(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;

                                }
                                break;

                            }
                        }
                        else
                        {
                            memsignal[band_index] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText2(); // update currFBox text

                    currFBox.SelectionStart = band_index * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {

                        Thread.Sleep(50);

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer

                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH ");
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    };

                    pausebtn.BackColor = SystemColors.ControlLight;


                } // FOR ;; loop

            } while (ScanRun == true); // ScanStopped so leave thread


        RT2: ST2.Stop();
            ST3.Stop();
            Debug.WriteLine("SCANTOP0"); // scanner done
            btnBandstack.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;
            //   scantype = 0;

        } // SCAN2   BANDSTACK memory scanner





        //================================================================================================
        // increment through the bandstack (as though you were clicking on the same band button over and over
        public void incbandstack()
        {
            string filter, mode;
            double freq;


            if (last_band.Equals("160M"))
            {
                console.band_160m_index = (console.band_160m_index + 1) % console.band_160m_register;
                band_index = console.band_160m_index;
            }
            else if (last_band.Equals("80M"))
            {
                console.band_80m_index = (console.band_80m_index + 1) % console.band_80m_register;
                band_index = console.band_80m_index;
            }
            else if (last_band.Equals("60M"))
            {
                console.band_60m_index = (console.band_60m_index + 1) % console.band_60m_register;
                band_index = console.band_60m_index;
            }
            else if (last_band.Equals("40M"))
            {
                console.band_40m_index = (console.band_40m_index + 1) % console.band_40m_register;
                band_index = console.band_40m_index;
            }
            else if (last_band.Equals("30M"))
            {
                console.band_30m_index = (console.band_30m_index + 1) % console.band_30m_register;
                band_index = console.band_30m_index;
            }
            else if (last_band.Equals("20M"))
            {
                console.band_20m_index = (console.band_20m_index + 1) % console.band_20m_register;
                band_index = console.band_20m_index;
            }
            else if (last_band.Equals("17M"))
            {
                console.band_17m_index = (console.band_17m_index + 1) % console.band_17m_register;
                band_index = console.band_17m_index;
            }
            else if (last_band.Equals("15M"))
            {
                console.band_15m_index = (console.band_15m_index + 1) % console.band_15m_register;
                band_index = console.band_15m_index;
            }
            else if (last_band.Equals("12M"))
            {
                console.band_12m_index = (console.band_12m_index + 1) % console.band_12m_register;
                band_index = console.band_12m_index;
            }
            else if (last_band.Equals("10M"))
            {
                console.band_10m_index = (console.band_10m_index + 1) % console.band_10m_register;
                band_index = console.band_10m_index;
            }
            else if (last_band.Equals("6M"))
            {
                console.band_6m_index = (console.band_6m_index + 1) % console.band_6m_register;
                band_index = console.band_6m_index;
            }
            else if (last_band.Equals("2M"))
            {
                console.band_2m_index = (console.band_2m_index + 1) % console.band_2m_register;
                band_index = console.band_2m_index;
            }
            else if (last_band.Equals("WWV"))
            {
                console.band_wwv_index = (console.band_wwv_index + 1) % console.band_wwv_register;
                band_index = console.band_wwv_index;
            }
            else if (last_band.Equals("GEN"))
            {
                console.band_gen_index = (console.band_gen_index + 1) % console.band_gen_register;
                band_index = console.band_gen_index;
            }
            else if (last_band.Equals("VHF0"))
            {
                console.band_vhf0_index = (console.band_vhf0_index + 1) % console.band_vhf0_register;
                band_index = console.band_vhf0_index;
            }
            else if (last_band.Equals("VHF1"))
            {
                console.band_vhf1_index = (console.band_vhf1_index + 1) % console.band_vhf1_register;
                band_index = console.band_vhf1_index;
            }
            else if (last_band.Equals("VHF2"))
            {
                console.band_vhf2_index = (console.band_vhf2_index + 1) % console.band_vhf2_register;
                band_index = console.band_vhf2_index;
            }
            else if (last_band.Equals("VHF3"))
            {
                console.band_vhf3_index = (console.band_vhf3_index + 1) % console.band_vhf3_register;
                band_index = console.band_vhf3_index;
            }
            else if (last_band.Equals("VHF4"))
            {
                console.band_vhf4_index = (console.band_vhf4_index + 1) % console.band_vhf4_register;
                band_index = console.band_vhf4_index;
            }
            else if (last_band.Equals("VHF5"))
            {
                console.band_vhf5_index = (console.band_vhf5_index + 1) % console.band_vhf5_register;
                band_index = console.band_vhf5_index;
            }
            else if (last_band.Equals("VHF6"))
            {
                console.band_vhf6_index = (console.band_vhf6_index + 1) % console.band_vhf6_register;
                band_index = console.band_vhf6_index;
            }
            else if (last_band.Equals("VHF7"))
            {
                console.band_vhf7_index = (console.band_vhf7_index + 1) % console.band_vhf7_register;
                band_index = console.band_vhf7_index;
            }
            else if (last_band.Equals("VHF8"))
            {
                console.band_vhf8_index = (console.band_vhf8_index + 1) % console.band_vhf8_register;
                band_index = console.band_vhf8_index;
            }
            else if (last_band.Equals("VHF9"))
            {
                console.band_vhf9_index = (console.band_vhf9_index + 1) % console.band_vhf9_register;
                band_index = console.band_vhf9_index;
            }
            else if (last_band.Equals("VHF10"))
            {
                console.band_vhf10_index = (console.band_vhf10_index + 1) % console.band_vhf10_register;
                band_index = console.band_vhf10_index;
            }
            else if (last_band.Equals("VHF11"))
            {
                console.band_vhf11_index = (console.band_vhf11_index + 1) % console.band_vhf11_register;
                band_index = console.band_vhf11_index;
            }
            else if (last_band.Equals("VHF12"))
            {
                console.band_vhf12_index = (console.band_vhf12_index + 1) % console.band_vhf12_register;
                band_index = console.band_vhf12_index;
            }
            else if (last_band.Equals("VHF13"))
            {
                console.band_vhf13_index = (console.band_vhf13_index + 1) % console.band_vhf13_register;
                band_index = console.band_vhf13_index;
            }
            else if (last_band.Equals("LMF"))
            {
                console.band_LMF_index = (console.band_LMF_index + 1) % console.band_LMF_register;
                band_index = console.band_LMF_index;
            }
            else if (last_band.Equals("120M"))
            {
                console.band_120m_index = (console.band_120m_index + 1) % console.band_120m_register;
                band_index = console.band_120m_index;
            }
            else if (last_band.Equals("90M"))
            {
                console.band_90m_index = (console.band_90m_index + 1) % console.band_90m_register;
                band_index = console.band_90m_index;
            }
            else if (last_band.Equals("61M"))
            {
                console.band_61m_index = (console.band_61m_index + 1) % console.band_61m_register;
                band_index = console.band_61m_index;
            }
            else if (last_band.Equals("49M"))
            {
                console.band_49m_index = (console.band_49m_index + 1) % console.band_49m_register;
                band_index = console.band_49m_index;
            }
            else if (last_band.Equals("41M"))
            {
                console.band_41m_index = (console.band_41m_index + 1) % console.band_41m_register;
                band_index = console.band_41m_index;
            }
            else if (last_band.Equals("31M"))
            {
                console.band_31m_index = (console.band_31m_index + 1) % console.band_31m_register;
                band_index = console.band_31m_index;
            }
            else if (last_band.Equals("25M"))
            {
                console.band_25m_index = (console.band_25m_index + 1) % console.band_25m_register;
                band_index = console.band_25m_index;
            }
            else if (last_band.Equals("22M"))
            {
                console.band_22m_index = (console.band_22m_index + 1) % console.band_22m_register;
                band_index = console.band_22m_index;
            }
            else if (last_band.Equals("19M"))
            {
                console.band_19m_index = (console.band_19m_index + 1) % console.band_19m_register;
                band_index = console.band_19m_index;
            }
            else if (last_band.Equals("16M"))
            {
                console.band_16m_index = (console.band_16m_index + 1) % console.band_16m_register;
                band_index = console.band_16m_index;
            }
            else if (last_band.Equals("14M"))
            {
                console.band_14m_index = (console.band_14m_index + 1) % console.band_14m_register;
                band_index = console.band_16m_index;
            }
            else if (last_band.Equals("13M"))
            {
                console.band_13m_index = (console.band_13m_index + 1) % console.band_13m_register;
                band_index = console.band_13m_index;
            }
            else if (last_band.Equals("11M"))
            {
                console.band_11m_index = (console.band_11m_index + 1) % console.band_11m_register;
                band_index = console.band_11m_index;
            }
            else
            {
                return;
            }

            //---------------------------------------------------------------


            if (DB.GetBandStack(last_band, band_index, out mode, out filter, out freq))
            {
                if (filter.Contains("@"))
                {
                    filter = filter.Substring(0, (filter.Length) - 1); // ke9ns add for bandstack lockout
                }

                console.SetBand(mode, filter, freq);
            }


            console.UpdateWaterfallLevelValues();


        }  // incbandstack




        public int xxx = 0;
        //======================================================================== 
        public void updateindex()
        {
            switch (console.RX1Band)
            {
                case Band.B160M:
                    console.band_160m_index = xxx;

                    break;
                case Band.B80M:
                    console.band_80m_index = xxx;

                    break;
                case Band.B60M:
                    console.band_60m_index = xxx;

                    break;
                case Band.B40M:
                    console.band_40m_index = xxx;
                    break;
                case Band.B30M:
                    console.band_30m_index = xxx;
                    break;
                case Band.B20M:
                    console.band_20m_index = xxx;
                    break;
                case Band.B17M:
                    console.band_17m_index = xxx;
                    break;
                case Band.B15M:
                    console.band_15m_index = xxx;
                    break;
                case Band.B12M:
                    console.band_12m_index = xxx;
                    break;
                case Band.B10M:
                    console.band_10m_index = xxx;
                    break;
                case Band.B6M:
                    console.band_6m_index = xxx;
                    break;
                case Band.B2M:
                    console.band_2m_index = xxx;
                    break;
                case Band.WWV:
                    console.band_wwv_index = xxx;
                    break;
                case Band.GEN:
                    console.band_gen_index = xxx;
                    break;


                case Band.VHF0:
                    console.band_vhf0_index = xxx;
                    break;
                case Band.VHF1:
                    console.band_vhf1_index = xxx;
                    break;
                case Band.VHF2:
                    console.band_vhf2_index = xxx;
                    break;
                case Band.VHF3:
                    console.band_vhf3_index = xxx;
                    break;
                case Band.VHF4:
                    console.band_vhf4_index = xxx;
                    break;
                case Band.VHF5:
                    console.band_vhf5_index = xxx;
                    break;
                case Band.VHF6:
                    console.band_vhf6_index = xxx;
                    break;
                case Band.VHF7:
                    console.band_vhf7_index = xxx;
                    break;
                case Band.VHF8:
                    console.band_vhf8_index = xxx;
                    break;
                case Band.VHF9:
                    console.band_vhf9_index = xxx;
                    break;
                case Band.VHF10:
                    console.band_vhf10_index = xxx;
                    break;
                case Band.VHF11:
                    console.band_vhf11_index = xxx;
                    break;
                case Band.VHF12:
                    console.band_vhf12_index = xxx;
                    break;
                case Band.VHF13:
                    console.band_vhf13_index = xxx;
                    break;



                case Band.BLMF:                                                                     // ke9ns add down below vhf
                    console.band_LMF_index = xxx;
                    break;
                case Band.B120M:
                    console.band_120m_index = xxx;
                    break;
                case Band.B90M:
                    console.band_90m_index = xxx;
                    break;
                case Band.B61M:
                    console.band_61m_index = xxx;
                    break;
                case Band.B49M:
                    console.band_49m_index = xxx;
                    break;
                case Band.B41M:
                    console.band_41m_index = xxx;
                    break;
                case Band.B31M:
                    console.band_31m_index = xxx;
                    break;
                case Band.B25M:
                    console.band_25m_index = xxx;
                    break;
                case Band.B22M:
                    console.band_22m_index = xxx;
                    break;

                case Band.B19M:
                    console.band_19m_index = xxx;
                    break;

                case Band.B16M:
                    console.band_16m_index = xxx;
                    break;
                case Band.B14M:
                    console.band_14m_index = xxx;
                    break;

                case Band.B13M:
                    console.band_13m_index = xxx;
                    break;

                case Band.B11M:
                    console.band_11m_index = xxx;
                    break;

            } // switch rx1band

        } // updateindex


        //=======================================================================================================================
        private void ScanControl_FormClosing(object sender, FormClosingEventArgs e)
        {

            Debug.WriteLine("==========CLOSING SCANNER============");

            ScanRun = false;
            ScanPause = false;
            ScanStop = 1;

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "ScanForm");
            //  console.MemoryList.Save();



        } // ScanControl_FormClosing


        MemoryRecord[] m1 = new MemoryRecord[1000];

        int[] memIndex = new int[1000]; // holder for memories that match the group name
        int memtotal = 0; // total matching group name memories found
        int memcount = 0; // total memories found
        string[] memsignal = new string[1000]; // db signal and sql brk


        //=======================================================================================================================
        // Group memory scanner. Scanning only frequencies in 1 group name
        private void btnGroupMemory_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;

            if (console.MemoryList.List.Count == 0) return; // nothing in the list, exit
            if (comboMemGroupName.Items.Count == 0) return;
            memcount = comboMemGroupName.Items.Count; // total number of memories listed

            Debug.WriteLine("memory list7 " + memcount);

            if (ScanRun == false) // if stoppedchange
            {
                currFBox.Text = "";

                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText();  // upate currFBox text

                ScanRun = true; // start up the scanner

                Thread t = new Thread(new ThreadStart(SCAN1));
                t.Name = "Group memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }

        } // button6_Click


        //=======================================================================================================================
        // Group memory scanner. Scanning only frequencies in 1 group name
        private void btnGroupMemory1_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            ScanPause = false;
            bandSwlupdate1(); // determine if there are any SWL stations ON the air that contain the name you just typed int the combobox of the scanner 


            if (swl_number < 1) return; // did not find any stations matching so return

            memcount = swl_number; // total number of memories listed


            Debug.WriteLine("memory list77 " + memcount);


            if (ScanRun == false) // if stoppedchange
            {
                currFBox.Text = "";

                for (int y = 0; y < 200; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText1();  // upate currFBox text

                ScanRun = true; // start up the scanner

                Thread t = new Thread(new ThreadStart(SCAN6));
                t.Name = "Group memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }

        } // btnGroupMemory1_Click


        MemoryRecord recordToRestore; // holder to select group name
        string Gname; // name of group of memories to scan
        string Gname1; // name of group of SWL to scan

        //==========================================================================================
        // ke9ns combobox to display ALL the group names from the memory listing
        private void comboMemGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMemGroupName.Items.Count == 0 || comboMemGroupName.SelectedItem == null) return;

            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list


        } //  comboMemGroupName_SelectedIndexChanged


        //==========================================================================================
        // ke9ns combobox to display all the unique (SUB) group names from the memory listing
        private void comboBoxTS1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScanPause = false;
            ScanRun = false;
            scantype = 1;


            if (comboBoxTS1.Text == "")
            {
                Gname = "";
                memcount = 0;
                return;
            }


            textBox1.Text = "";


            Debug.WriteLine("[[[[[[[[[[[[[[[[[[[COMBOBOX EVENT]]]]]]]]]]]]]]]");

            //  if (comboBoxTS1.Items.Count == 0 || comboBoxTS1.SelectedItem == null) return;
            if (console.initializing == true) return;

            //  Gname = comboBoxTS1.SelectedItem.ToString();
            Gname = comboBoxTS1.Text;

            Debug.WriteLine("SELECTED GROUP NAME " + Gname);

            if (console.MemoryList.List.Count == 0) return; // nothing in the list, exit
            if (comboMemGroupName.Items.Count == 0) return;

            memcount = comboMemGroupName.Items.Count; // total number of memories listed

            Debug.WriteLine("memory list8 " + memcount);


            UpdateText(); // upate currFBox text

            Debug.WriteLine("memory list8a " + memcount);

            btnGroupMemory.Enabled = true;
            btnGroupMemory1.Enabled = false;

            Debug.WriteLine("memory list8b " + memcount);

        } // comboBoxTS1_SelectedIndexChanged


        private void comboBoxTS2_SelectedIndexChanged(object sender, KeyEventArgs e)
        {
            ScanPause = false;
            ScanRun = false;
            scantype = 1;

            //  comboBoxTS1.SelectedIndexChanged -= comboBoxTS1_SelectedIndexChanged;  // ke9ns turn off checkchanged temporarily   
            comboBoxTS1.Text = "";
            //  comboBoxTS1.SelectedIndexChanged += comboBoxTS1_SelectedIndexChanged;  //


            //    Debug.WriteLine("SWL TEXT TYPED" + textBox1.Text);


            if (e.KeyCode != Keys.Enter) return;

            //    Debug.WriteLine("SWL TEXT TYPED and contains a CR");

            Gname1 = textBox1.Text;

            //   Debug.WriteLine("SELECTED SWL NAME " + Gname1);

            //     Debug.WriteLine("SWL checking SWL listing");

            bandSwlupdate1(); // determine if there are any SWL stations ON the air that contain the name you just typed int the combobox of the scanner 

            Debug.WriteLine("[[[[[[[[[[[[[[[[[[[SWL List compiled]]]]]]]]]]]]]]]");

            if (console.initializing == true) return;

            Debug.WriteLine("SWL found size " + swl_number);

            if (swl_number < 1) return; // did not find any stations matching so return

            memcount = swl_number; // total number of memories listed

            UpdateText1(); // upate currFBox text

            btnGroupMemory1.Enabled = true;
            btnGroupMemory.Enabled = false;
        }
        //==========================================================================================
        // ke9ns combobox to display all the unique (SUB) group names from the memory listing
        private void comboBoxTS2_SelectedIndexChanged(object sender, EventArgs e)
        {


        } // comboBoxTS1_SelectedIndexChanged

        //========================================================================================
        //  Lookup SWL table and match user input to the SWL list and update currFBox text screen
        void UpdateText1()
        {

            memtotal = 0;

            string temp1 = "";

            Debug.WriteLine("1UPDATE LIST " + Gname1);

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {
                int zz = swl_index[i];

                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;
                //     double hh = Convert.ToDouble();  // SWL "RXFREQ"  convert to hz
                string freq = hh.ToString("###0.000000");    //  freq of SWL

                string name = SpotControl.SWL_Loc[zz];    // name of SWL
                string mm = SpotControl.SWL_Station[zz];  // GROUP of SWL

                if (memsignal[memtotal] == null) memsignal[memtotal] = " ";

                temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long

                memIndex[memtotal] = i;
                memtotal++;

            } // for i loop

            currFBox.Text = temp1;

        } // UpdateText1()  SWL 



        //===================================================================================
        //===================================================================================
        //===================================================================================

        int[] swl_index = new int[20000];
        int swl_number = 0;

        void bandSwlupdate1()
        {

            int iii = 0;

            //   Debug.WriteLine("swl index size= " + SpotControl.SWL_Index1);

            Gname1 = Gname1.TrimEnd('\r', '\n');

            DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            SpotControl.UTCNEW1 = Convert.ToInt16(UTCD.ToString("HHmm")); // convert 24hr UTC to int

            iii = 0;

            int ii = 0;
            try
            {

                for (ii = 0; ii < SpotControl.SWL_Index1; ii++) // check all spots to see which ones are on at this particular time and day
                {
                    // station check 

                    if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(SpotControl.SWL_Station[ii], Gname1, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                    {

                        //   Debug.WriteLine("SCAN SWL FREQ " + SpotControl.SWL_Freq[ii] + " , " + ii + " , " + SpotControl.SWL_Station[ii]);

                        // station check days on air and time on air
                        if (
                            ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                            ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                             )
                        {
                            //   Debug.WriteLine("station found " + SpotControl.SWL_Freq[ii] + " , "+ SpotControl.SWL_Day1[ii]);

                            swl_index[iii++] = ii; // keep track of frequencies on at the moment

                            if (SpotControl.SWL_Station[ii].Length > 20) SpotControl.SWL_Station[ii] = SpotControl.SWL_Station[ii].Substring(0, 20);
                            SpotControl.SWL_Mode[ii] = "USB";

                            //      bigmessage += (String.Format("{0:00.000000}", (double)(SpotControl.SWL_Freq[ii]) / 1000000.0) +
                            //           "  " + SpotControl.SWL_Station[ii].PadRight(25, ' ') + " " + SpotControl.SWL_Loc[ii].PadRight(3, ' ') +
                            //         " " + SpotControl.SWL_TimeN[ii].ToString().PadLeft(4, '0') + ":" + SpotControl.SWL_TimeF[ii].ToString().PadLeft(4, '0') +
                            //        " " + "\r\n");


                        } // check time
                    } // text search to narrow down


                } // for loop through SWL_Index
            }
            catch (Exception e)
            {

                Debug.WriteLine("SWL Load problem " + ii + " , " + e);
            }

            Debug.WriteLine("SWL DONE " + (iii - 1));


            swl_number = iii - 1; // total swl stations found that contain the Gname1 swl name



        } // bandSwlupdate







        int linelength = 84; // length of a line in the currFbox
        //========================================================================================
        //  Lookup MEMORY table and match user input to the MEMORY list and update currFBox text screen
        void UpdateText()
        {

            memtotal = 0;

            string temp1 = "";

            Debug.WriteLine("UPDATE LIST " + Gname);

            for (int i = 0; i < memcount; i++) // find all the memories with the same group name
            {

                //  if ( (dataGridView2[0, i].Value.ToString()).Equals(Gname, StringComparison.InvariantCultureIgnoreCase) == true) // check if index matches name
                //    if (dataGridView2[0, i].Value.ToString() == Gname) // check if index matches name

                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                {
                    bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .155  

                    double hh = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz
                    string freq = hh.ToString("###0.000000");    //  freq of memory  dataGridView2[1, i].Value.ToString();
                    string name = dataGridView2[2, i].Value.ToString(); // name of memory
                    string mm = dataGridView2[0, i].Value.ToString();  // GROUP of MEMORY

                    //  string comment = dataGridView2["comments", i].Value.ToString(); // comments of memory
                    //  int hh = (int)(Convert.ToDouble(SpotForm.dataGridView2[1, ii].Value) * 1000000);  // MEMORY "RXFREQ"  convert to hz
                    // string ll = (string)SpotForm.dataGridView2[2, holder2[ii]].Value;  // Name of MEMORY
                    //  DSPMode nn = (DSPMode)SpotForm.dataGridView2[3, holder2[ii]].Value;  // DSPMODE of MEMORY

                    //  Debug.WriteLine("UPDATE LIST A ");

                    if (memsignal[memtotal] == null) memsignal[memtotal] = " ";

                    //   Debug.WriteLine("UPDATE LIST B " + memsignal[memtotal]);

                  //  bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .155  

                    string Y;
                    if (scan == true) Y = " "; // .155
                    else Y = "X";

                    temp1 = temp1 + (memtotal + 1).ToString().PadLeft(2) + ":" + Y + " " + mm.PadRight(20).Substring(0, 20) + ", " + freq.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[memtotal].PadRight(19).Substring(0, 19) + "\r\n"; // 74 char long

                    memIndex[memtotal] = i;
                    memtotal++;
                    //  Debug.WriteLine("Found Group name match at index " + i);
                }

            } // for i loop

            currFBox.Text = temp1;

        } // UpdateText()  MEMORY 


        public static int SQL = 0;  // updated by console routine picSquelch_Paint
        public static int SIG = 0;

        Stopwatch ST2 = new Stopwatch();
        Stopwatch ST3 = new Stopwatch();

        int scantype = 0;  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
        bool scanstop = false;

        //==========================================================================================
        // thread scans selected "Memory" Group name frequencies only
        private void SCAN1()
        {
            ST2.Reset();
            ST3.Reset();

            scantype = 1;

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnGroupMemory.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            ST3.Reset();

            int x = 0;

            do // ScanRun
            {
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2;

                }
                Debug.WriteLine("START OF LOOP");

                for (x = 0; x < memtotal; x++) // go through list of MEMORIES you found
                {
                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    comboMemGroupName.SelectedIndex = memIndex[x];
                    recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                    if (recordToRestore.Scan == false) continue; // ke9ns add .155

                    Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                    console.RecallMemory(recordToRestore);


                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;


                    //-------------------------------------------------------
                    // SPEED TIMER and PAUSE
                    do
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;  // turn off this thread now
                        }

                        if (SIG > lastSIG)  // CHECK SQUELCH and SIGNAL levels
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }


                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", SQL BRK";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else // if 0
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }

                                break; // break out of the while loop
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer

                                }
                                else // if 0
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break; // break out of the while loop

                            }


                        } // ScanStop == 1
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-----------------------------------------------------BREAK comes here--

                    UpdateText(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {
                        Thread.Sleep(50);

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false; //.219
                            scanstop = true;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH " + memtotal);
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    }; //  while (ScanPause == true)  // wait here in in pause


                    pausebtn.BackColor = SystemColors.ControlLight;

                    Debug.WriteLine("END OF LOOP " + memtotal + " , " + x);

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }

                } // for (x = 0; x < memtotal; x++)   FOR memtotal loop



                Debug.WriteLine("END OF LOOP1 " + memtotal + " , " + x);
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2;

                }
            } while (ScanRun == true); // ScanStopped so leave thread

        RT2:
            Debug.WriteLine("SCANTOP0"); // scanner done
            ST2.Stop();
            ST3.Stop();

            btnGroupMemory.BackColor = SystemColors.ControlLight;
            //   scantype = 0;

        } // SCAN1()


        //==========================================================================================
        // thread scans selected SWL Group name frequencies only
        private void SCAN6()
        {
            ST2.Reset();
            ST3.Reset();

            scantype = 1;

            Debug.WriteLine("6SCANSTOP = " + ScanStop);
            btnGroupMemory1.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            ST3.Reset();

            int x = 0;

            do // ScanRun
            {
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2A;

                }
                Debug.WriteLine("START OF LOOP");

                for (x = 0; x < memtotal; x++) // go through list of MEMORIES you found
                {
                    Thread.Sleep(50);

                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }

                    int zz = swl_index[x];

                    //   double hh = (double)SpotControl.SWL_Freq[zz];


                    //   string name = SpotControl.SWL_Loc[zz];    // name of SWL
                    //  string mm = SpotControl.SWL_Station[zz];  // GROUP of SWL

                    //    comboMemGroupName.SelectedIndex = memIndex[x];
                    //   recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list
                    //    Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                    //    console.RecallMemory(recordToRestore);

                    console.VFOAFreq = SpotControl.SWL_Freq[zz] / 1000000.0; // convert to mhz
                    console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    Debug.WriteLine("SELECT FREQ: " + console.VFOAFreq);

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;


                    //-------------------------------------------------------
                    // SPEED TIMER and PAUSE
                    do
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }
                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2A;  // turn off this thread now
                        }

                        if (SIG > lastSIG)  // CHECK SQUELCH and SIGNAL levels
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }


                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", SQL BRK";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2A;
                                }
                                break;
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;

                                UpdateText(); // update currFBox text

                                currFBox.SelectionStart = x * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED " + memtotal);
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2A;
                                }

                                break;
                            }
                        }
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText1(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2A;

                    }

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {
                        Thread.Sleep(50);

                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2A;

                        }

                        
                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            scanstop = true;                       // .221
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH " + memtotal);
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    }; // while PAUSE

                    if (scanstop == true) //.221
                    {
                        scanstop = false;  // reset
                        goto RT2A;

                    }
                    pausebtn.BackColor = SystemColors.ControlLight;

                    Debug.WriteLine("END OF LOOP " + memtotal + " , " + x);

                } // FOR memtotal loop

                Debug.WriteLine("END OF LOOP1 " + memtotal + " , " + x);
                if (scanstop == true)
                {
                    scanstop = false;  // reset
                    goto RT2A;

                }
            } while (ScanRun == true); // ScanStopped so leave thread

        RT2A:
            Debug.WriteLine("SCANTOP0"); // scanner done
            ST2.Stop();
            ST3.Stop();

            btnGroupMemory1.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;

            //   scantype = 0;

        } // SCAN6()






        //===========================================================================================
       
        public static byte SP5_Active = 0; // ke9ns: 1= running, 0=off
        double freq1 = 0.0;
        public static double freq_Low = 0.0; // ke9ns low and high get automatically filled by the console.cs routine as the band changes
        public static double freq_High = 0.0;

        public static double freq_Low1 = 0.0; // ke9ns low and high get automatically filled by the console.cs routine as the band changes
        public static double freq_High1 = 0.0;// but these cannot be changed by the user

        public static double freq_Last = 0.0;

        public static bool ViewSWRPlot = false; // true = view SWR plot on display
        public static bool RunSWRPlot = false; // true = run a new plot
                                               //  public static double[,,] SWR_READ = new double[10, 40, 5000]; // Ant=1,2,3, band=1-30,freq slot = swr on band  (now found in console.cs)
        public static int SWR_STEP = 1; // 1k step for SWR
        public static int SWR_SLOT = 0; // slot in band (example 3.5 to 4.0  Slot0 = 3.500 Slot1 = 3.501 (1 SLOT = 1 KHZ)

        // public static double[] SWR_Freq = new double[1000];


        // SWR PLOT BUTTON2
        // read freq_low and Freq_high
        // turn off ATU
        // check for TX auth on freq, if not go to next freq, otherwise TUNE and check SWR, save in array
        private void button2_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            switch (console.CurrentModel)
            {

                case Model.FLEX5000:
                case Model.FLEX3000:
                    if (console.PowerOn == false)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Your Flex Radio Must be Running, Turn ON first",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return;
                    }
                    if (console.VFOAFreq > 54.0)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "No SWR detection above 6m.",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show(new Form { TopMost = true }, "Your Flex Radio does not have an SWR circuit",
                    " ERROR ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                    break;
            }


            Debug.WriteLine("RUN SWR PLOT click    ");

            ScanStop = 0; // reset scan

            if (SP5_Active == 0)
            {

                SP5_Active = 1;

                // see console routine  if (rx1_band != old_band || initializing) for setting low and high settings

                Thread t = new Thread(new ThreadStart(SWR_SCANNER));


                t.Name = "SWR Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Debug.WriteLine("good    ");

            } // SP_active = 0;
            else
            {

                SP5_Active = 0;
                Debug.WriteLine("OFF   ");

            } // SP_Active = 1



        } // button2_Click

        //===================================================================================================
        private async void SWR_SCANNER()
        {

            Stopwatch x1 = new Stopwatch();
            // TEST
            int TestRun = 0; // numericSWRTest = 1- 5

            // Band
            Band BAND1 = console.RX1Band; // B160M = 1, B80M = 2,B60M = 3,

            // ANT
            FWCAnt ANT1 = console.GetRX1Ant(BAND1); //ANT1 = 1, ANT2 = 2, ANT3 = 3, RX1IN=4, RX2IN=5, RX1TAP=6, SIG_GEN=7, VHF=8, UHF=9,

            currFBox.Text = "Antenna = " + ANT1 + " , Band= " + BAND1 + "\r\n";



            scantype = 4; // low to high scan

            freq1 = freq_Low;

            //lblTUNE.Text = "Tune: " + ptbTune.Value.ToString();
            var OLD_TUNE_LEVEL = console.TunePower;
            console.TunePower = 11;
            // console.ptbTune.Value = 10; // set power level to 10watts

            try
            {
                TestRun = (int)numericSWRTest.Value;

            }
            catch (Exception)
            {
                TestRun = 1;
            }

            if (TestRun > 5)
            {
                TestRun = 1;
                numericSWRTest.Value = 1;
            }

            try
            {
                step = (int)udstepBox.Value;   // get just whole number. Convert.ToDouble(udstepBox.Text) / 1000;
                SWR_STEP = (int)udstepBox.Value;

                step = step / 1000; // convert to KHZ  (1khz : step= .001)
            }
            catch (Exception)
            {
                step = 0.002; // 2 khz
                SWR_STEP = 2;
            }

            try
            {
                speed = (int)udspeedBox1.Value;  // Convert.ToInt16(udspeedBox.Text);

            }
            catch (Exception)
            {
                speed = 300; // 300msec

            }

            Debug.WriteLine("SWR_Scanner STEP " + step + " ,SPEED " + speed + " ,LOW " + freq_Low + " ,HIGH " + freq_High + " ,RST " + ScanRST + " ,LAST " + freq_Last + ", Test: " + TestRun);


            double ii = freq_Low;


            if (ScanRST == 1)
            {
                ii = freq_Last;
            }


            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 0] = Math.Round(freq_Low1, 3); // first entry is LOW freq of this SWR PLOT (SLOT size is always 1khz)
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 1] = Math.Round(freq_High1, 3); // 2nd entry is HIGH freq of this SWR PLOT (SLOT size is always 1khz)
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 2] = Math.Round(step, 3); // .001 = 1khz first entry is LOW freq of this SWR PLOT (SLOT size is always 1khz)

            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 3] = TestRun;  //TEST#
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 4] = 4; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 5] = 5; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 6] = 6; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 7] = 7; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 8] = 8; // reserved
            console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, 9] = 9; // reserved

            SWR_SLOT = (int)(((decimal)ii - (decimal)freq_Low1) * 1000) + 10; // find first SLOT in current LOW freq (example 3.5 - 4.0, but LOW = 3.6, so SLOT = 100 for 3.6 (1khz = 1 slot)

            //   Debug.WriteLine("SWR_SLOT SCAN " + SWR_SLOT);

            ScanStop = 0;

            for (int x = 10; x < SWR_SLOT; x++) // fill low end of SWR plot
            {
                if (console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, x] < 1) console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, x] = 0.5; // this is so the storage file will see the data gap and not ignore data after the gap
                                                                                                                                      //  Debug.WriteLine("SWR FILL " + x);

            }

            System.IO.Directory.CreateDirectory(console.AppDataPath + "SWR_PLOTS"); // ke9ns create sub directory

            string file_name = console.AppDataPath + "SWR_PLOTS\\SWR_TEST#" + TestRun.ToString() + "-" + ANT1.ToString() + "-" + BAND1.ToString() + "_" + SWRName() + ".csv"; // save data for my mods .166

            FileStream stream4 = new FileStream(file_name, FileMode.Create); // open   file .166
            BinaryWriter writer4 = new BinaryWriter(stream4);

            byte[] textb = Encoding.ASCII.GetBytes("Test Run=," + TestRun.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("Antenna Source=," + ANT1.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("Band=," + BAND1.ToString() + Environment.NewLine);
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes(Environment.NewLine); // .166
            writer4.Write(textb);

            textb = Encoding.ASCII.GetBytes("SLOT," + "Frequency (Mhz)," + "SWR" + Environment.NewLine); // .166
            writer4.Write(textb);


            for (; ii <= freq_High; ii = ii + step, SWR_SLOT = SWR_SLOT + (int)(udstepBox.Value))
            {

                // find if you are authorized to TX on this freq
                if (console.CheckValidTXFreq(console.current_region, ii, console.dsp.GetDSPTX(0).CurrentDSPMode) == false) continue;

                console.chkTUN.Checked = true; // TX 

                if (SWR_SLOT == 10)
                {

                    for (int x9 = 0; x9 < 4; x9++)
                    {
                        if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break;
                        await Task.Delay(speed / 10).ConfigureAwait(false);

                    }

                }

                x1.Restart(); // restart timer


                console.VFOAFreq = ii; // convert to MHZ

                for (int x9 = 0; x9 < 10; x9++)
                {
                    if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break; // allow you to check if user stopped scanner
                    await Task.Delay(speed / 10).ConfigureAwait(false);
                }

                x1.Stop();
                double SWR1 = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("Freq & SWR " + ii + " , " + SWR1);

                if (SWR1 >= 25) // try 1 more time before displaying SWR
                {
                    for (int q = 0; q < 5; q++) // ke9ns .184
                    {
                        for (int x9 = 0; x9 < 3; x9++)
                        {
                            if (ScanStop == 1 && chkBoxSQLBRK.Checked == true) break;
                            await Task.Delay(speed / 10).ConfigureAwait(false);

                        }
                        SWR1 = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                        if (SWR1 < 25) break;
                    } // for loop

                } // if SWR1 >=25


                double temp9 = (Math.Round(SWR1, 1));

                Debug.WriteLine("SLOT= " + SWR_SLOT + " , " + ii.ToString("###0.000") + " , " + temp9.ToString() + " , " + (int)ANT1 + " , " + (int)BAND1);


                console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, SWR_SLOT] = temp9;

                for (int i = 1; i < SWR_STEP; i++) // fill in empty slots with identical data to the last valid slot SWR
                {
                    console.SWR_READ[TestRun, (int)ANT1, (int)BAND1, SWR_SLOT + i] = temp9;

                } // if (SWR_STEP > 1)

                currFBox.AppendText(SWR_SLOT + " , " + ii.ToString("###0.000000") + " , " + temp9 + "\r\n");
                textb = Encoding.ASCII.GetBytes(SWR_SLOT.ToString() + " , " + ii.ToString("###0.000000") + " , " + temp9 + Environment.NewLine); // .166
                writer4.Write(textb);

                if (SP5_Active == 0) break;

            } // for loop

            console.chkTUN.Checked = false; // TX OFF 
            console.TunePower = OLD_TUNE_LEVEL;

            currFBox.AppendText("ABOVE ^ # " + TestRun + ", Antenna = " + ANT1 + " , Band= " + BAND1 + "\r\n");
            textb = Encoding.ASCII.GetBytes("ABOVE ^ # " + TestRun.ToString() + ", Antenna =," + ANT1.ToString() + " , Band= ," + BAND1.ToString() + Environment.NewLine); // .166
            writer4.Write(textb);
            Debug.WriteLine("SWR_Scanner STOPPED");

            if (ii >= freq_High)
            {
                ScanRST = 0; // reset back to start
                ii = freq_Low;

                Debug.WriteLine("SWR_SCANNER FINISHED ");

            }
            else
            {
                ScanRST = 1; // leave off where you left off
                freq_Last = ii + (step * 2); // need to jump past last signal that breaks squelch otherwise you cant move anymore
            }

            writer4.Close();    // close  file
            stream4.Close();   // close stream


        } // SWR_SCANNER (END Thread)


        public string SWRName()
        {
            string temp = DateTime.Now.ToString();                     // Date and time
            temp = temp.Replace("/", "-");
            temp = temp.Replace(":", "_");

            return temp;


        } // SWRName()


        // FREQ SCANNER BUTTON
        private void button5_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset(); // shut down memory scan


            Debug.WriteLine("click    ");

            ScanStop = 0; // reset scan

            if (SP5_Active == 0)
            {

                SP5_Active = 1;

                // see console routine  if (rx1_band != old_band || initializing) for setting low and high settings

                Thread t = new Thread(new ThreadStart(SCANNER));


                t.Name = "Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                Trace.WriteLine("good    ");

            } // SP_active = 0;
            else
            {

                SP5_Active = 0;
                Trace.WriteLine("OFF   ");

            } // SP_Active = 1




        } // button5_Click



        //===============================================================================
        //===============================================================================
        // ke9ns SCANNER thread from low to high frequency selected on the scanner panel

        double step = 0.0001;
        int speed = 50;


        private async void SCANNER()
        {

            Stopwatch x1 = new Stopwatch();

        LOOP1: scantype = 4;

            freq1 = freq_Low;

            try
            {
                step = (double)udstepBox.Value;   //Convert.ToDouble(udstepBox.Text) / 1000;
                step = step / 1000; // convert to KHZ
            }
            catch (Exception)
            {
                step = 0.0001; // 1 khz
            }

            try
            {
                speed = (int)udspeedBox1.Value;  // Convert.ToInt16(udspeedBox.Text);

            }
            catch (Exception)
            {
                speed = 50; // 50msec
            }

            Debug.WriteLine("Scanner STEP " + step + " ,SPEED " + speed + " ,LOW " + freq_Low + " ,HIGH " + freq_High + " ,RST " + ScanRST + " ,LAST " + freq_Last);



            //   Trace.WriteLine("good1   ");

            double ii = freq_Low;

            if (ScanRST == 1)
            {
                ii = freq_Last;
            }


            for (; ii <= freq_High; ii = ii + step)
            {

                currFBox.Text = ii.ToString("f6");

                x1.Restart();

                console.VFOAFreq = ii; // convert to MHZ

                for (int x9 = 0; x9 < 10; x9++) // divide up step time into 10 parts, so I can use await timer
                {
                    if (chkBoxSQLBRK.Checked == true)
                    {
                        if (ScanStop == 1) // if console says squelch break
                        {
                            if (udPauseLength.Value == 0)
                            {
                                break; // break out of the delay loop
                            }
                            else
                            {
                                if (SP5_Active == 0) break;
                                Debug.WriteLine("PAUSE " + x9);
                                pausebtn.BackColor = Color.Yellow;
                                await Task.Delay((int)udPauseLength.Value * 1000).ConfigureAwait(false); // pause
                                ScanStop = 0;
                                pausebtn.BackColor = SystemColors.ControlLight;
                                break;
                            }
                        }
                    }
                    else if (chkBoxSQLBRKWait.Checked == true)
                    {
                        if (ScanStop == 1) // if console says squelch break
                        {
                            if (udPauseLength.Value == 0)
                            {
                                break; // break out of the delay loop
                            }
                            else
                            {
                                if (SP5_Active == 0) break;
                                Debug.WriteLine("PAUSE " + x9);
                                pausebtn.BackColor = Color.Yellow;
                                await Task.Delay((int)udPauseLength.Value * 1000).ConfigureAwait(false); // pause
                                ScanStop = 0;
                                pausebtn.BackColor = SystemColors.ControlLight;

                            }
                        }
                    }
                    await Task.Delay(speed / 10).ConfigureAwait(false);

                } // 10x loop

                if (chkBoxSQLBRK.Checked == true)
                {
                    if (ScanStop == 1) // if console says squelch break
                    {
                        if (udPauseLength.Value == 0)
                        {
                            ScanStop = 0;
                            break; // break out of the freq scanner loop
                        }
                    }
                }
                else if (chkBoxSQLBRKWait.Checked == true)
                {
                    if (ScanStop == 1) // if console says squelch break
                    {
                        if (udPauseLength.Value == 0)
                        {
                            ScanStop = 0;
                            break; // break out of the freq scanner loop
                        }
                    }
                }


                x1.Stop();

                Debug.WriteLine("TIME " + x1.ElapsedMilliseconds);

                if (SP5_Active == 0) break;

            } // for loop


            Debug.WriteLine("Scanner1 ");

            if (ii >= freq_High)
            {
                ScanRST = 0; // reset back to start
                ii = freq_Low;

                Debug.WriteLine("SCANNER FINISHED ");

                if (chkBoxLoop.Checked == true)
                {
                    SP5_Active = 1;
                    goto LOOP1;
                }


            }
            else
            {
                ScanRST = 1; // leave off where you left off
                freq_Last = ii + (step * 2); // need to jump past last signal that breaks squelch otherwise you cant move anymore
            }


            //   scantype = 0;

        } // SCANNER


        //===============================================================================
        //===============================================================================


        // ke9ns override band edge setting 
        private void lowFBox_Click(object sender, EventArgs e)
        {
            double freq2 = 0.0;
            ScanRST = 0;
            try
            {
                freq2 = Convert.ToDouble(lowFBox.Text);

                if (freq2 < freq_Low1) freq2 = freq_Low1;

            }
            catch (Exception)
            {
                freq2 = freq_Low1;


            }

            freq_Low = freq2;
            lowFBox.Text = freq_Low.ToString("f6");

            if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // lowFBox_Click

        private void lowFBox_MouseLeave(object sender, EventArgs e)
        {
            double freq2 = 0.0;
            ScanRST = 0;
            try
            {
                freq2 = Convert.ToDouble(lowFBox.Text);

                if (freq2 < freq_Low1) freq2 = freq_Low1;

            }
            catch (Exception)
            {
                freq2 = freq_Low1;


            }


            freq_Low = freq2;
            lowFBox.Text = freq_Low.ToString("f6");

            if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // lowFBox_MouseLeave

        private void highFBox_Click(object sender, EventArgs e)
        {
            double freq3 = 0.0;
            ScanRST = 0;
            try
            {
                freq3 = Convert.ToDouble(highFBox.Text);
                if (freq3 > freq_High1) freq3 = freq_High1;
            }
            catch (Exception)
            {
                freq3 = freq_High1;


            }

            freq_High = freq3;
            highFBox.Text = freq_High.ToString("f6");

            if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band


        } // highFBox_Click

        private void highFBox_MouseLeave(object sender, EventArgs e)
        {
            double freq3 = 0.0;
            ScanRST = 0;
            try
            {
                freq3 = Convert.ToDouble(highFBox.Text);
                if (freq3 > freq_High1) freq3 = freq_High1;
            }
            catch (Exception)
            {
                freq3 = freq_High1;


            }

            freq_High = freq3;
            highFBox.Text = freq_High.ToString("f6");

            if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band

        } // highFBox_MouseLeave

        private void lowFBox_KeyDown(object sender, KeyEventArgs e)
        {
            double freq2 = 0.0;

            if (e.KeyData == Keys.Enter)
            {
                ScanRST = 0;
                try
                {
                    freq2 = Convert.ToDouble(lowFBox.Text);

                    if (freq2 < freq_Low1) freq2 = freq_Low1;

                }
                catch (Exception)
                {
                    freq2 = freq_Low1;


                }

                freq_Low = freq2;
                lowFBox.Text = freq_Low.ToString("f6");

                if (console.RX1Band >= 0) console.SLowScan[(int)console.RX1Band] = lowFBox.Text; // ke9ns .186: save low/high of scanner for each band

            } // wait for enter key

        } // lowFBox_KeyDown

        private void highFBox_KeyDown(object sender, KeyEventArgs e)
        {
            double freq3 = 0.0;

            if (e.KeyData == Keys.Enter)
            {
                ScanRST = 0;
                try
                {
                    freq3 = Convert.ToDouble(highFBox.Text);
                    if (freq3 > freq_High1) freq3 = freq_High1;
                }
                catch (Exception)
                {
                    freq3 = freq_High1;


                }

                freq_High = freq3;
                highFBox.Text = freq_High.ToString("f6");

                if (console.RX1Band >= 0) console.SHighScan[(int)console.RX1Band] = highFBox.Text; // ke9ns .186: save low/high of scanner for each band

            } // wait for enter key

        }// highFBox_KeyDown

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



        string customscannerlist = ""; // ke9ns file name of customscannerlist


        //=========================================================================================
        // ke9ns add   select scanner file name (text file freq in hz, name)
        string[] customString = new string[200]; // name
        string[] customFilter = new string[200]; // filter
        string[] customMode = new string[200]; // mode
        double[] customMem = new double[200]; // list of custom memory frequency

        public static FileStream stream2;          // for reading SWL.csv file
        public static BinaryReader reader2;

        public static int custSize = 0; // size on custom freq list

        private void btnCustomList_Click(object sender, EventArgs e)
        {

            ScanPause = false;
           
            comboBoxTS1.Text = "";
            textBox1.Text = "";
            Gname = "";
            memcount = 0;
            memtotal = 0;  //.226
           

            ST3.Stop();
            ST3.Reset();
           

            if (ScanRun == false) // if stopped
            {
                string filePath = console.AppDataPath + "CustomScannerList\\";


                if (!Directory.Exists(filePath))
                {
                    //   Debug.WriteLine("no CustomScannerList folder file found");
                    System.IO.Directory.CreateDirectory(console.AppDataPath + "CustomScannerList"); // ke9ns create sub directory
                                                                                                    //  Debug.WriteLine("CustomScannerList created");

                }

                openFileDialog2.InitialDirectory = String.Empty;
                openFileDialog2.InitialDirectory = filePath; // ke9ns  file to quickplay subfolder but could also be wave_folder;


                DialogResult result1 = openFileDialog2.ShowDialog();

                if (result1 == DialogResult.OK) // Test result.
                {
                    //    Debug.WriteLine("file selected1 " + result);
                    //  Debug.WriteLine("file selected2 " + openFileDialog2.FileName);

                    customscannerlist = openFileDialog2.FileName; // pass file name to wave file
                }
                else
                {
                    customscannerlist = null;
                    return; // if you dont select a file then no scanning
                }

                stream2 = new FileStream(customscannerlist, FileMode.Open); // open  file
                reader2 = new BinaryReader(stream2, Encoding.ASCII);

                var result = new StringBuilder();

                Debug.WriteLine("OPEN THE FILE ");

                custSize = 0; // new size of custom freq list

                int x = 0;
                for (; ; )
                {

                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if ((newChar == '\r'))  // 0x0d LF
                        {

                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            string[] values = result.ToString().Split(','); // split line up into segments divided by ,

                            Debug.WriteLine("CUSTOM STRING " + values[0]);
                            Debug.WriteLine("CUSTOM MEM " + values[1]);

                            Debug.WriteLine("CUSTOM MEM " + values[2]);

                            Debug.WriteLine("CUSTOM MEM " + values[3]);

                            customString[x] = values[0];                 // name
                            customMem[x] = Convert.ToDouble(values[1]);  // freq
                            customMode[x] = values[2];                   // mode = LSB,  USB,DSB,CWL,CWU,FM,	AM,	DIGU,SPEC,	DIGL,	SAM, DRM
                            customFilter[x] = values[3];                 // filter =  F1,F2,F3,	F4,	F5,	F6,	F7,	F8,	F9,	F10,VAR1,VAR2



                            result.Clear();


                            x++; // get next line
                        }
                        else
                        {

                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        // x--;
                        Debug.WriteLine("END OF STREAM ");
                        break; // done with file
                    }
                    catch (Exception f)
                    {
                        Debug.WriteLine("GET CHAR EXCEPTION " + f);
                        // x--;
                        break;
                    }

                    if (x > 100) break; // only allow 100 freq in list


                } // for loop 
              
                reader2.Close();    // close  file
                stream2.Close();   // close stream
               
                custSize = x; // new size of custom freq list

                ScanRun = true; // start up the scanner

                currFBox.Text = "";
                for (int y = 0; y < 100; y++)
                {
                    memsignal[y] = null;
                }

                UpdateText3();

                Thread t = new Thread(new ThreadStart(SCAN3));
                t.Name = "Custom memory Scanner Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }
            else
            {
                ScanRun = false; // turn off scanner

            }


        } // btnCustomList_Click

        //========================================================================================
        //  CUSTOM MEMORY LIST CURRFBOX text update
        void UpdateText3()
        {

            string temp1 = "";

           
            for (int ii = 0; ii < custSize; ii++)
            {
               
                string freq3 = customMem[ii].ToString("###0.000000"); // was N6 4 less than having index numbers
                string name = customString[ii];
                string mm = "Custom Memory List";

                if (memsignal[ii] == "") memsignal[ii] = " ";
                if (memsignal[ii] == null) memsignal[ii] = " ";

                temp1 = temp1 + (ii + 1).ToString().PadLeft(2) + ": " + mm.PadRight(20).Substring(0, 20) + ", " + freq3.PadLeft(12).Substring(0, 12) + ", " + name.PadRight(20).Substring(0, 20) + ", " + memsignal[ii].PadRight(20).Substring(0, 20) + "\r\n"; // 74 char long //.226 fix ii
              

            } // for


            currFBox.Text = temp1;

        } // UpdateText3()  CUSTOM MEMORY LIST


        //===========================================================================
        // Thread CUSTOM LIST MEMORY scanner
        private void SCAN3()
        {
            scantype = 3;

            ST2.Reset();
            ST3.Reset();

            Debug.WriteLine("SCANSTOP = " + ScanStop);
            btnCustomList.BackColor = Color.LightGreen;

            int lastSIG = 0;
            int lastSQL = 0;

            string filter, mode;
            double freq;


            do // ScanRun
            {

                for (int x = 0; x < custSize; x++)
                {

                    Thread.Sleep(50);
                    if (scanstop == true)
                    {
                        scanstop = false;  // reset
                        goto RT2;

                    }
                    try
                    {
                        speed = (int)udspeedBox.Value;  // Convert.ToInt16(udspeedBox.Text);
                        Debug.WriteLine("SPEED " + speed);
                    }
                    catch (Exception)
                    {
                        speed = 50; // 50msec
                    }


                    // go to next bandstack memory

                    freq = customMem[x];
                    filter = customFilter[x];   // "LAST";
                    mode = customMode[x];        // "LAST";

                    Debug.WriteLine("CUSTOM BAND: " + freq + " , " + filter + " , " + mode);

                    // filter = F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,VAR1,VAR2
                    // mode = LSB,USB,DSB,CWL,CWU,FM,AM,DIGU,SPEC,DIGL,SAM,DRM


                    console.SetBand(mode, filter, freq);
                    console.UpdateWaterfallLevelValues();

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;
                    currFBox.ScrollToCaret(); // keep highlighted line visable

                    ST2.Restart(); // restart timer over again

                    ScanStop = 0; // reset squelch

                    lastSIG = -400;
                    lastSQL = -400;
                    //-------------------------------------------------------
                    // timer
                    do // scan speed and scanPause
                    {
                        Thread.Sleep(50);
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                        if (ScanRun == false)
                        {
                            ScanPause = false;
                            goto RT2;
                        }

                        if (SIG > lastSIG)
                        {
                            lastSIG = SIG;
                        }

                        if (SQL > lastSQL)
                        {
                            lastSQL = SQL;
                        }

                        if ((ScanStop == 1)) // if console detected squelch open
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", Squelch Break";

                            if ((chkBoxSQLBRK.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText3(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break;
                            }
                            else if ((chkBoxSQLBRKWait.Checked == true) && (ScanPause == false)) // if stop on squelch break, then stop now
                            {
                                ScanPause = true;
                                UpdateText3(); // update currFBox text

                                currFBox.SelectionStart = band_index * linelength; // i * linelength
                                currFBox.SelectionLength = linelength;

                                if (udPauseLength.Value > 0)
                                {
                                    Debug.WriteLine("ST3 TIMER STARTED ");
                                    ST3.Restart(); // start the pause timer
                                }
                                else
                                {
                                    ScanPause = false;
                                    ScanRun = false;
                                    goto RT2;
                                }
                                break;
                            }

                        }
                        else
                        {
                            memsignal[x] = lastSQL.ToString().PadLeft(4) + ", " + lastSIG.ToString().PadLeft(4) + ", ";
                        }
                        if (scanstop == true)
                        {
                            scanstop = false;  // reset
                            goto RT2;

                        }
                    } while ((ST2.ElapsedMilliseconds < speed) || (ScanPause == true));

                    //-------------------------------------------------------

                    UpdateText3(); // update currFBox text

                    currFBox.SelectionStart = x * linelength; // i * linelength
                    currFBox.SelectionLength = linelength;

                    if (SP5_Active == 1)
                    {
                        ScanRun = false;
                        ScanPause = false;
                        Debug.WriteLine("SCANSTOP, another scanner started");
                        break;
                    }


                    //-------------------------------------------------------
                    // CHECK For PAUSE
                    while (ScanPause == true)  // wait here in in pause
                    {

                        Thread.Sleep(50);

                        if (ScanRun == false)
                        {
                            Debug.WriteLine("SCANSTOP, Group scanner turned back off");
                            ScanPause = false;
                            break;
                        }

                        if (ST3.ElapsedMilliseconds > ((long)udPauseLength.Value * 1000))
                        {
                            ST3.Stop(); // stop the pause timer
                            if (chkBoxSQLBRKWait.Checked == true && ScanStop == 1)
                            {
                                ScanPause = true;
                                ScanStop = 0;
                                ST3.Restart();

                            }
                            else ScanPause = false;

                            Debug.WriteLine("ST3 TIMER REACHED PAUSELENGTH ");
                        }

                        if (ScanPause == true) pausebtn.BackColor = Color.Yellow;
                        else pausebtn.BackColor = SystemColors.ControlLight;

                    };

                    pausebtn.BackColor = SystemColors.ControlLight;

                } // FOR custSize loop

            } while (ScanRun == true); // ScanStopped so leave thread

        RT2: ST2.Stop();
            ST3.Stop();

            Debug.WriteLine("SCANTOP0"); // scanner done
            btnCustomList.BackColor = SystemColors.ControlLight;
            pausebtn.BackColor = SystemColors.ControlLight;
            //   scantype = 0;


        } // SCAN3  CUSTOMER LIST MEMORY SCANNER



        //==========================================================================================
        // Pause button
        private void pausebtn_Click(object sender, EventArgs e)
        {
            ST3.Stop();
            ST3.Reset();

            if (ScanRun == true)
            {
                if (ScanPause == false)
                {
                    ScanPause = true;

                }
                else
                {

                    ScanPause = false;
                    ScanStop = 0; // undo the squelch break if you unpause
                }
            }
        }

        int yyy = 0;
        int iii = 0;

        //==================================================================================
        // ke9ns left click to select this memory for vfoA
        private void currFBox_MouseUp(object sender, MouseEventArgs e)
        {
            string filter, mode;
            double freq;

            currFBox.ShortcutsEnabled = false;

            if (e.Button == MouseButtons.Left) // VFOA
            {
                Debug.WriteLine("LEFT CLICK: " );

                scanstop = true;

                if (scantype == 1)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }
                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scann screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                           comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list
                          
                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemory(recordToRestore);

                            return;
                        }

                        int counter = 0;

                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {
                              
                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {
                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        console.SetBand(mode, filter, freq);
                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOAFreq = hh;
                                console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
                else if (scantype == 2)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        Debug.WriteLine("index at start of click " + console.iii);


                        console.iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("index after click " + console.iii);

                        yyy = 0;


                        if (xxx > console.band_stacks[nnn])
                        {
                            return;

                        }


                        DB.GetBandStack(band_list[nnn], xxx, out mode, out filter, out freq);

                        // updateindex();

                        console.SetBand(mode, filter, freq);

                        console.UpdateWaterfallLevelValues();
                    }
                    catch
                    {
                        Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                        if (yyy == 1)
                        {
                            updateindex();

                            //   console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                            console.UpdateWaterfallLevelValues();
                        }

                    }


                } // scantype = 2 (bandstack)



            } // LEFT CLICK MOUSE VFOA
            else if (e.Button == MouseButtons.Right) // .222 VFOB RX2
            {
                Debug.WriteLine("RIGHT CLICK: ");

                scanstop = true;

                if (scantype == 1)
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }

                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scann screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                            comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemoryB(recordToRestore);


                            return;
                        }

                        int counter = 0;

                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {

                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {

                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        console.SetBand2(mode, filter, freq); // .222 rx2
                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOBFreq = hh;
                              //  console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value


                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
                else if (scantype == 2)
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        Debug.WriteLine("index at start of click " + console.iii);


                        console.iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("index after click " + console.iii);

                        yyy = 0;


                        if (xxx > console.band_stacks[nnn])
                        {
                            return;

                        }


                        DB.GetBandStack(band_list[nnn], xxx, out mode, out filter, out freq);

                        // updateindex();

                        console.SetBand2(mode, filter, freq); // .222 rX2

                        console.UpdateWaterfallLevelValues();
                    }
                    catch
                    {
                        Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                        if (yyy == 1)
                        {
                            updateindex();

                            //   console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                            console.UpdateWaterfallLevelValues();
                        }

                    }


                } // scantype = 2 (bandstack)



            } // RIGHT CLICK MOUSE VFOB RX2
            else if (e.Button == MouseButtons.Middle) // .226 only for memory channel scan toggle ON/OFF
            {
              
                if (comboBoxTS1.Text == "") return;
                if (textBox1.Text != "") return; // only for memory channel scan toggle ON/OFF

                    scanstop = true;

                if (scantype == 1)  // 1=memory, 2=band stack, 3= custom, 4= low-high, 5=SWL
                {
                    try
                    {
                        int ii = currFBox.GetCharIndexFromPosition(e.Location);

                        xxx = (ii / linelength); //find row 

                        // if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                        Debug.WriteLine("1xxx " + xxx + " , " + ii);


                        currFBox.SelectionStart = (xxx * linelength);
                        currFBox.SelectionLength = linelength;

                        currFBox.ScrollToCaret(); // keep highlighted line visable

                        Debug.WriteLine("index at start of click " + iii);


                        iii = xxx; // update new position in bandstack for checking if its locked

                        Debug.WriteLine("memcount " + memcount + " , " + memtotal + " , " + memIndex[xxx]);

                        yyy = 0;

                        if (textBox1.Text == "")  // MEMORY
                        {
                            if (iii > memIndex[memtotal])
                            {
                                Debug.WriteLine("clicked beyond index length " + memIndex[memtotal]);
                                return;
                            }
                        }
                        else // SWL
                        {
                            if (iii > memtotal)
                            {
                                Debug.WriteLine("1clicked beyond index length " + memtotal);
                                return;
                            }

                        }

                        if (comboBoxTS1.Text != "") //.221 add so clicking on memory in the scan screen will pull up all memory parameters
                        {
                            Debug.WriteLine("MEMORY CLICK. restore memory " + xxx);

                            comboMemGroupName.SelectedIndex = memIndex[xxx];
                            recordToRestore = new MemoryRecord((MemoryRecord)comboMemGroupName.SelectedItem); // ke9ns   you select index in the combo pulldown list

                            Debug.WriteLine("CHANGE MEMORY TO " + recordToRestore.RXFreq);
                            console.RecallMemory(recordToRestore);

                         
                        }

                        int counter = 0;

                       
                        for (int i = 0; i < memcount; i++) // find all the memories with the same group name
                        {

                            if (textBox1.Text == "")  // MEMORY
                            {

                                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf((dataGridView2[0, i].Value.ToString()), Gname, CompareOptions.IgnoreCase) >= 0) // Gname must be contains in MEMORY (partial or full) and case insensitive)
                                {
                                   
                                    freq = Convert.ToDouble(dataGridView2[1, i].Value);  // MEMORY "RXFREQ"  convert to hz

                                    mode = dataGridView2[3, i].Value.ToString();  // DSPMODE of MEMORY

                                    filter = dataGridView2[20, i].Value.ToString();

                                    // you got a match to your GROUP name so check the line #
                                    if (counter == iii)
                                    {
                                        bool scan = (bool)dataGridView2["Scan", i].Value; // ke9ns add .226 
                                       
                                        if (scan == true) dataGridView2["Scan", i].Value = false; // ke9ns add .226 change value here (in memoryForm.cs)
                                        else dataGridView2["Scan", i].Value = true;
                                        
                                        console.SetBand(mode, filter, freq);

                                        UpdateText(); // update memory currfbox.text .226

                                        
                                        return;
                                    }
                                    counter++;
                                }
                            }
                            else // SWL 
                            {
                                int zz = swl_index[iii];

                                double hh = (double)SpotControl.SWL_Freq[zz] / 1000000.0;

                                console.VFOAFreq = hh;
                                console.tempVFOAFreq = console.VFOAFreq; // ke9ns add  CTUN operation changed freq so update temp value

                                return;
                            }

                        } // for i loop

                    }
                    catch (Exception)
                    {


                    }

                    Debug.WriteLine(" did not find a match ");
                    return;

                } // scantype = 1
               


            } // Wheel CLICK MOUSE (disable in memory scan)



        } // currFBox_MouseUp

        private void currFBox_MouseDown(object sender, MouseEventArgs e)
        {
            currFBox.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        } // currFBox_MouseDown

        private void chkBoxIdent_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
        }

        private void checkBoxSWR_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
        }

        private void checkBoxSWR_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }


        //=========================================
        // for F1 help sceen
        private void ScanControl_KeyDown(object sender, KeyEventArgs e) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c) 
        {
            Debug.WriteLine("F1 key2");



            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {


                Debug.WriteLine("F1 key");

                if (MouseIsOverControl(checkBoxSWR) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/w5j6jh6c0_g?rel=0&amp";
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.SWRScanner.Text;
                }
                else if (MouseIsOverControl(button2) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/w5j6jh6c0_g?rel=0&amp";
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.SWRScanner.Text;
                }



            } // if (e.KeyCode == Keys.F1)




        } // SpotControl_KeyDown


        public bool MouseIsOverControl(Control c) // ke9ns keypreview must be TRUE
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }


        private void ScanControl_MouseEnter(object sender, EventArgs e)
        {
            Console.HELPSWR = true;
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
        }

        private void ScanControl_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPSWR = false;
        }

        private void numericSWRTest_ValueChanged(object sender, EventArgs e)
        {
            console.SWR_TESTRUN = (int)numericSWRTest.Value;

        } // numericSWRTest_ValueChanged

        private void checkBoxSWR_CheckedChanged(object sender, EventArgs e)
        {
            if (console.VFOAFreq > 54) checkBoxSWR.Checked = false; // .226 no swr above 6m
        }

        private void chkBoxSQLBRKWait_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxSQLBRKWait.Checked == true)
            {
                chkBoxSQLBRK.Checked = false;

            }
        }

        private void chkBoxSQLBRK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxSQLBRK.Checked == true)
            {
                chkBoxSQLBRKWait.Checked = false;

            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {

            MouseEventArgs me = (MouseEventArgs)e;


            if (me.Button == System.Windows.Forms.MouseButtons.Right) // right mouse click
            {

                string filePath = console.AppDataPath + @"SWR_PLOTS\";

                Debug.WriteLine("filepath: " + filePath);

                if (!Directory.Exists(filePath))
                {

                    Debug.WriteLine("problem folder found");
                    return;

                }


                string argument = @filePath;                     //@"/select, " + filePath;

                System.Diagnostics.Process.Start("explorer.exe", argument);

            }// right mouse click

        }

        private void button_reset_Click(object sender, EventArgs e) // ke9ns add .186
        {
            if (console.RX1Band >= 0)
            {
                console.SLowScan[(int)console.RX1Band] = " ";
                console.SHighScan[(int)console.RX1Band] = " ";

                console.scanUpdate = true; // force a simulated band button click
                console.RX1Band = console.RX1Band;

                lowFBox.Invalidate();
                highFBox.Invalidate();
            }

        } // button_reset_Click

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBoxTS1.Text = "";

        }
    } // scancontrol


} // powersdr
