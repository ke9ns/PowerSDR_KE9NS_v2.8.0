//=================================================================
// Spot.cs
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


namespace PowerSDR
{


    //=======================================================================================
    public partial class SpotControl : System.Windows.Forms.Form
    {

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SWLbutton = new System.Windows.Forms.Button();
            this.SSBbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.nodeBox1 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.RichTextBox();
            this.callBox = new System.Windows.Forms.TextBox();
            this.portBox2 = new System.Windows.Forms.TextBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusBoxSWL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTrack = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.SWLbutton2 = new System.Windows.Forms.Button();
            this.btnBeacon = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.checkBoxTone = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.DXPost = new System.Windows.Forms.Button();
            this.textBoxDXCall = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.RotorHead = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.chkFLayerON = new System.Windows.Forms.CheckBoxTS();
            this.chkDLayerON = new System.Windows.Forms.CheckBoxTS();
            this.txtLoTWpass = new System.Windows.Forms.TextBoxTS();
            this.chkBoxBeacon = new System.Windows.Forms.CheckBoxTS();
            this.chkVoacap = new System.Windows.Forms.CheckBoxTS();
            this.chkDXOn = new System.Windows.Forms.CheckBoxTS();
            this.chkMapOn = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxBandText = new System.Windows.Forms.CheckBoxTS();
            this.numBeamHeading = new System.Windows.Forms.NumericUpDownTS();
            this.chkISS = new System.Windows.Forms.CheckBoxTS();
            this.chkMoon = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxContour = new System.Windows.Forms.CheckBoxTS();
            this.tbPanPower = new System.Windows.Forms.TrackBarTS();
            this.chkBoxAnt = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxDIG = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxMUF = new System.Windows.Forms.CheckBoxTS();
            this.udDisplayWWV = new System.Windows.Forms.NumericUpDownTS();
            this.checkBoxWWV = new System.Windows.Forms.CheckBoxTS();
            this.numericUpDownTS1 = new System.Windows.Forms.NumericUpDownTS();
            this.BoxBFScan = new System.Windows.Forms.CheckBoxTS();
            this.BoxBScan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxBeam = new System.Windows.Forms.CheckBoxTS();
            this.udDisplayLong = new System.Windows.Forms.NumericUpDownTS();
            this.udDisplayLat = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxMem = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxPan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxSSB = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxCW = new System.Windows.Forms.CheckBoxTS();
            this.chkMapBand = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCountry = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCall = new System.Windows.Forms.CheckBoxTS();
            this.chkPanMode = new System.Windows.Forms.CheckBoxTS();
            this.chkGrayLine = new System.Windows.Forms.CheckBoxTS();
            this.chkSUN = new System.Windows.Forms.CheckBoxTS();
            this.hkBoxSpotRX2 = new System.Windows.Forms.CheckBoxTS();
            this.hkBoxSpotBand = new System.Windows.Forms.CheckBoxTS();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.menuItem2 = new System.Windows.Forms.ToolStripTextBox();
            this.mnuSpotOptions = new System.Windows.Forms.MenuItem();
            this.chkTimeServer1 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer2 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer3 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer4 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer5 = new System.Windows.Forms.MenuItem();
            this.menuTimeServers = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuSpotAge = new System.Windows.Forms.MenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.chkBoxWrld = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxNA = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.chkDXMode = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeamHeading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).BeginInit();
            this.SuspendLayout();
            // 
            // SWLbutton
            // 
            this.SWLbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SWLbutton.Location = new System.Drawing.Point(615, 368);
            this.SWLbutton.Name = "SWLbutton";
            this.SWLbutton.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton.TabIndex = 2;
            this.SWLbutton.Text = "Spot SWL";
            this.toolTip1.SetToolTip(this.SWLbutton, resources.GetString("SWLbutton.ToolTip"));
            this.SWLbutton.UseVisualStyleBackColor = false;
            this.SWLbutton.Click += new System.EventHandler(this.SWLbutton_Click);
            // 
            // SSBbutton
            // 
            this.SSBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SSBbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SSBbutton.Location = new System.Drawing.Point(12, 470);
            this.SSBbutton.Name = "SSBbutton";
            this.SSBbutton.Size = new System.Drawing.Size(75, 23);
            this.SSBbutton.TabIndex = 1;
            this.SSBbutton.Text = "Spot DX";
            this.toolTip1.SetToolTip(this.SSBbutton, "Click to Turn On/Off Dx Cluster Spotting (on both this DX Spotting window and Pan" +
        "adapter)\r\nRequires Internet to work.\r\n");
            this.SSBbutton.UseVisualStyleBackColor = false;
            this.SSBbutton.Click += new System.EventHandler(this.spotSSB_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.DetectUrls = false;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(12, 143);
            this.textBox1.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.textBox1.MaxLength = 1000000;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(759, 168);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "";
            this.toolTip1.SetToolTip(this.textBox1, "\r\n");
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseEnter += new System.EventHandler(this.SpotControl_MouseEnter);
            this.textBox1.MouseHover += new System.EventHandler(this.textBox1_MouseHover);
            this.textBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseMove);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // nodeBox1
            // 
            this.nodeBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeBox1.Location = new System.Drawing.Point(658, 366);
            this.nodeBox1.MaxLength = 50;
            this.nodeBox1.Name = "nodeBox1";
            this.nodeBox1.Size = new System.Drawing.Size(84, 22);
            this.nodeBox1.TabIndex = 6;
            this.nodeBox1.Text = "spider.ham-radio-deluxe.com";
            this.toolTip1.SetToolTip(this.nodeBox1, "Enter in a DX Cluster URL address here");
            this.nodeBox1.Visible = false;
            this.nodeBox1.TextChanged += new System.EventHandler(this.nodeBox_TextChanged);
            this.nodeBox1.Leave += new System.EventHandler(this.nodeBox_Leave);
            this.nodeBox1.MouseEnter += new System.EventHandler(this.nodeBox_MouseEnter);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(298, 7);
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.textBox3.Size = new System.Drawing.Size(473, 130);
            this.textBox3.TabIndex = 8;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            this.textBox3.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textBox3_LinkClicked);
            this.textBox3.MouseEnter += new System.EventHandler(this.SpotControl_MouseEnter);
            // 
            // callBox
            // 
            this.callBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.callBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callBox.Location = new System.Drawing.Point(682, 470);
            this.callBox.MaxLength = 20;
            this.callBox.Name = "callBox";
            this.callBox.ShortcutsEnabled = false;
            this.callBox.Size = new System.Drawing.Size(87, 22);
            this.callBox.TabIndex = 5;
            this.callBox.Text = "Callsign";
            this.toolTip1.SetToolTip(this.callBox, resources.GetString("callBox.ToolTip"));
            this.callBox.Click += new System.EventHandler(this.callBox_Click);
            this.callBox.TextChanged += new System.EventHandler(this.callBox_TextChanged);
            this.callBox.Leave += new System.EventHandler(this.callBox_Leave);
            this.callBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.callBox_MouseDown);
            this.callBox.MouseEnter += new System.EventHandler(this.callBox_MouseEnter);
            // 
            // portBox2
            // 
            this.portBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBox2.Location = new System.Drawing.Point(696, 368);
            this.portBox2.MaxLength = 7;
            this.portBox2.Name = "portBox2";
            this.portBox2.Size = new System.Drawing.Size(56, 22);
            this.portBox2.TabIndex = 7;
            this.portBox2.Text = "0";
            this.toolTip1.SetToolTip(this.portBox2, "Enter in Dx Cluster URL Port# here");
            this.portBox2.Visible = false;
            this.portBox2.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            this.portBox2.Leave += new System.EventHandler(this.portBox_Leave);
            this.portBox2.MouseEnter += new System.EventHandler(this.portBox_MouseEnter);
            // 
            // statusBox
            // 
            this.statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(13, 338);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(156, 22);
            this.statusBox.TabIndex = 11;
            this.statusBox.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBox, "Click to Test connection\r\nIf it goes back to \"Spotting\" then the connection is go" +
        "od");
            this.statusBox.Click += new System.EventHandler(this.statusBox_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(101, 470);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pause";
            this.toolTip1.SetToolTip(this.button1, "Click to toggle Pause updates to this Spotter screen\r\n\r\nAlso mouse over the Spott" +
        "er screen will also Pause updates to screen\r\n\r\n");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.TextChanged += new System.EventHandler(this.button1_TextChanged);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusBoxSWL
            // 
            this.statusBoxSWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBoxSWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBoxSWL.Location = new System.Drawing.Point(614, 340);
            this.statusBoxSWL.Name = "statusBoxSWL";
            this.statusBoxSWL.Size = new System.Drawing.Size(156, 22);
            this.statusBoxSWL.TabIndex = 16;
            this.statusBoxSWL.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBoxSWL, "Status of ShortWave spotter list transfer to PowerSDR memory\r\n");
            this.statusBoxSWL.Click += new System.EventHandler(this.statusBoxSWL_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(13, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Status of DX Cluster";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(612, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Status of Space WX & SWL Spot";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 13000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 20;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrack.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.Location = new System.Drawing.Point(273, 394);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(60, 23);
            this.btnTrack.TabIndex = 62;
            this.btnTrack.Text = "Track/Map";
            this.toolTip1.SetToolTip(this.btnTrack, resources.GetString("btnTrack.ToolTip"));
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            this.btnTrack.MouseEnter += new System.EventHandler(this.btnTrack_MouseEnter);
            this.btnTrack.MouseLeave += new System.EventHandler(this.btnTrack_MouseLeave);
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBox.Location = new System.Drawing.Point(687, 368);
            this.nameBox.MaxLength = 20;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(46, 22);
            this.nameBox.TabIndex = 64;
            this.nameBox.Text = "name";
            this.toolTip1.SetToolTip(this.nameBox, "Enter Your Call sign to login to the DX Cluster here");
            this.nameBox.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 44);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Size = new System.Drawing.Size(280, 93);
            this.dataGridView1.TabIndex = 72;
            this.toolTip1.SetToolTip(this.dataGridView1, "Enter DX address : port#\r\nExample:  wb8zrl.no-ip.org:7300\r\n");
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.MouseEnter += new System.EventHandler(this.SpotControl_MouseEnter);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView2.Location = new System.Drawing.Point(498, 143);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView2.Size = new System.Drawing.Size(254, 94);
            this.dataGridView2.TabIndex = 75;
            this.toolTip1.SetToolTip(this.dataGridView2, "memories");
            this.dataGridView2.Visible = false;
            // 
            // SWLbutton2
            // 
            this.SWLbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SWLbutton2.Location = new System.Drawing.Point(696, 368);
            this.SWLbutton2.Name = "SWLbutton2";
            this.SWLbutton2.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton2.TabIndex = 76;
            this.SWLbutton2.Text = "SWL list";
            this.toolTip1.SetToolTip(this.SWLbutton2, resources.GetString("SWLbutton2.ToolTip"));
            this.SWLbutton2.UseVisualStyleBackColor = false;
            this.SWLbutton2.Click += new System.EventHandler(this.SWLbutton2_Click);
            // 
            // btnBeacon
            // 
            this.btnBeacon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBeacon.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBeacon.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBeacon.Location = new System.Drawing.Point(182, 470);
            this.btnBeacon.Name = "btnBeacon";
            this.btnBeacon.Size = new System.Drawing.Size(85, 23);
            this.btnBeacon.TabIndex = 85;
            this.btnBeacon.Text = "NCDXF Beacon";
            this.toolTip1.SetToolTip(this.btnBeacon, resources.GetString("btnBeacon.ToolTip"));
            this.btnBeacon.UseVisualStyleBackColor = false;
            this.btnBeacon.Click += new System.EventHandler(this.btnBeacon_Click);
            // 
            // btnTime
            // 
            this.btnTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTime.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTime.Location = new System.Drawing.Point(513, 324);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 89;
            this.btnTime.Text = "Time Sync";
            this.toolTip1.SetToolTip(this.btnTime, resources.GetString("btnTime.ToolTip"));
            this.btnTime.UseVisualStyleBackColor = false;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // checkBoxTone
            // 
            this.checkBoxTone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTone.AutoSize = true;
            this.checkBoxTone.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBoxTone.Location = new System.Drawing.Point(561, 377);
            this.checkBoxTone.Name = "checkBoxTone";
            this.checkBoxTone.Size = new System.Drawing.Size(46, 17);
            this.checkBoxTone.TabIndex = 93;
            this.checkBoxTone.TabStop = true;
            this.checkBoxTone.Text = "Tick";
            this.toolTip1.SetToolTip(this.checkBoxTone, resources.GetString("checkBoxTone.ToolTip"));
            this.checkBoxTone.UseVisualStyleBackColor = true;
            this.checkBoxTone.CheckedChanged += new System.EventHandler(this.checkBoxTone_CheckedChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(532, 201);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(46, 20);
            this.textBox2.TabIndex = 94;
            this.textBox2.Text = "0";
            this.toolTip1.SetToolTip(this.textBox2, "Length of Tone in mSec\r\n");
            this.textBox2.Visible = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Location = new System.Drawing.Point(297, 163);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 23);
            this.button2.TabIndex = 99;
            this.button2.Text = "Pause";
            this.toolTip1.SetToolTip(this.button2, "Click to Pause the DX Text window (if spots are coming through too fast)\r\nUpdates" +
        " to the Panadapter will still occur");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            // 
            // DXPost
            // 
            this.DXPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DXPost.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.DXPost.Enabled = false;
            this.DXPost.Location = new System.Drawing.Point(13, 369);
            this.DXPost.Name = "DXPost";
            this.DXPost.Size = new System.Drawing.Size(64, 23);
            this.DXPost.TabIndex = 100;
            this.DXPost.Text = "Callout";
            this.toolTip1.SetToolTip(this.DXPost, resources.GetString("DXPost.ToolTip"));
            this.DXPost.UseVisualStyleBackColor = false;
            this.DXPost.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DXPost_MouseUp);
            // 
            // textBoxDXCall
            // 
            this.textBoxDXCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxDXCall.Enabled = false;
            this.textBoxDXCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDXCall.Location = new System.Drawing.Point(82, 370);
            this.textBoxDXCall.Name = "textBoxDXCall";
            this.textBoxDXCall.Size = new System.Drawing.Size(87, 22);
            this.textBoxDXCall.TabIndex = 101;
            this.textBoxDXCall.Text = "DX Callsign";
            this.toolTip1.SetToolTip(this.textBoxDXCall, resources.GetString("textBoxDXCall.ToolTip"));
            this.textBoxDXCall.Click += new System.EventHandler(this.textBoxDXCall_Click);
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(480, 471);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(37, 20);
            this.textBox4.TabIndex = 102;
            this.textBox4.Text = "0";
            this.toolTip1.SetToolTip(this.textBox4, "Moons Ecliptic Longitude\r\n");
            this.textBox4.Visible = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Location = new System.Drawing.Point(628, 410);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(48, 19);
            this.button3.TabIndex = 108;
            this.button3.Text = "Rotor°";
            this.toolTip1.SetToolTip(this.button3, "Click to Move Rotor to Selected Heading.\r\n\r\nYou can also click on the beam headin" +
        "g in the DX spot list");
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // RotorHead
            // 
            this.RotorHead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RotorHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotorHead.Location = new System.Drawing.Point(520, 410);
            this.RotorHead.Name = "RotorHead";
            this.RotorHead.Size = new System.Drawing.Size(37, 20);
            this.RotorHead.TabIndex = 109;
            this.RotorHead.Text = "0°";
            this.toolTip1.SetToolTip(this.RotorHead, resources.GetString("RotorHead.ToolTip"));
            this.RotorHead.Visible = false;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Location = new System.Drawing.Point(190, 413);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(57, 23);
            this.button4.TabIndex = 117;
            this.button4.Text = "LoTW";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button4_MouseDown);
            // 
            // chkFLayerON
            // 
            this.chkFLayerON.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFLayerON.Enabled = false;
            this.chkFLayerON.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkFLayerON.Image = null;
            this.chkFLayerON.Location = new System.Drawing.Point(270, 420);
            this.chkFLayerON.Name = "chkFLayerON";
            this.chkFLayerON.Size = new System.Drawing.Size(84, 20);
            this.chkFLayerON.TabIndex = 122;
            this.chkFLayerON.Text = "foF2-Layer";
            this.toolTip1.SetToolTip(this.chkFLayerON, resources.GetString("chkFLayerON.ToolTip"));
            this.chkFLayerON.CheckedChanged += new System.EventHandler(this.chkFLayerON_CheckedChanged);
            // 
            // chkDLayerON
            // 
            this.chkDLayerON.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDLayerON.Enabled = false;
            this.chkDLayerON.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkDLayerON.Image = null;
            this.chkDLayerON.Location = new System.Drawing.Point(270, 437);
            this.chkDLayerON.Name = "chkDLayerON";
            this.chkDLayerON.Size = new System.Drawing.Size(75, 20);
            this.chkDLayerON.TabIndex = 121;
            this.chkDLayerON.Text = "D-Layer";
            this.toolTip1.SetToolTip(this.chkDLayerON, resources.GetString("chkDLayerON.ToolTip"));
            this.chkDLayerON.CheckedChanged += new System.EventHandler(this.chkDLayerON_CheckedChanged_1);
            // 
            // txtLoTWpass
            // 
            this.txtLoTWpass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLoTWpass.Location = new System.Drawing.Point(682, 441);
            this.txtLoTWpass.MaxLength = 20;
            this.txtLoTWpass.Name = "txtLoTWpass";
            this.txtLoTWpass.PasswordChar = '.';
            this.txtLoTWpass.ShortcutsEnabled = false;
            this.txtLoTWpass.Size = new System.Drawing.Size(86, 20);
            this.txtLoTWpass.TabIndex = 116;
            this.txtLoTWpass.Text = "password LoTW";
            this.toolTip1.SetToolTip(this.txtLoTWpass, "LoTW password (this is permanently saved)");
            this.txtLoTWpass.UseSystemPasswordChar = true;
            this.txtLoTWpass.Visible = false;
            this.txtLoTWpass.TextChanged += new System.EventHandler(this.txtLoTWpass_TextChanged);
            // 
            // chkBoxBeacon
            // 
            this.chkBoxBeacon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxBeacon.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxBeacon.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxBeacon.Image = null;
            this.chkBoxBeacon.Location = new System.Drawing.Point(182, 390);
            this.chkBoxBeacon.Name = "chkBoxBeacon";
            this.chkBoxBeacon.Size = new System.Drawing.Size(85, 24);
            this.chkBoxBeacon.TabIndex = 114;
            this.chkBoxBeacon.Text = "Spot Beacon";
            this.toolTip1.SetToolTip(this.chkBoxBeacon, "Check to display Beacon Spots\r\n");
            // 
            // chkVoacap
            // 
            this.chkVoacap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkVoacap.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkVoacap.Image = null;
            this.chkVoacap.Location = new System.Drawing.Point(615, 169);
            this.chkVoacap.Name = "chkVoacap";
            this.chkVoacap.Size = new System.Drawing.Size(77, 17);
            this.chkVoacap.TabIndex = 113;
            this.chkVoacap.Text = "VoaOn";
            this.toolTip1.SetToolTip(this.chkVoacap, "voacap on");
            this.chkVoacap.Visible = false;
            // 
            // chkDXOn
            // 
            this.chkDXOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDXOn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkDXOn.Image = null;
            this.chkDXOn.Location = new System.Drawing.Point(615, 192);
            this.chkDXOn.Name = "chkDXOn";
            this.chkDXOn.Size = new System.Drawing.Size(77, 17);
            this.chkDXOn.TabIndex = 112;
            this.chkDXOn.Text = "DxOn";
            this.toolTip1.SetToolTip(this.chkDXOn, "dx on\r\n");
            this.chkDXOn.Visible = false;
            // 
            // chkMapOn
            // 
            this.chkMapOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapOn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapOn.Image = null;
            this.chkMapOn.Location = new System.Drawing.Point(520, 169);
            this.chkMapOn.Name = "chkMapOn";
            this.chkMapOn.Size = new System.Drawing.Size(77, 17);
            this.chkMapOn.TabIndex = 111;
            this.chkMapOn.Text = "MapOn";
            this.toolTip1.SetToolTip(this.chkMapOn, "map on\r\n");
            this.chkMapOn.Visible = false;
            // 
            // chkBoxBandText
            // 
            this.chkBoxBandText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxBandText.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxBandText.Image = null;
            this.chkBoxBandText.Location = new System.Drawing.Point(417, 425);
            this.chkBoxBandText.Name = "chkBoxBandText";
            this.chkBoxBandText.Size = new System.Drawing.Size(113, 20);
            this.chkBoxBandText.TabIndex = 106;
            this.chkBoxBandText.Text = "BandText to Pan";
            this.toolTip1.SetToolTip(this.chkBoxBandText, "Check to Show BandText on Panadapter display\r\n\r\nAlso, can Right Click on VFO Band" +
        " Text area to Toggle this on/off\r\n");
            // 
            // numBeamHeading
            // 
            this.numBeamHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numBeamHeading.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBeamHeading.Location = new System.Drawing.Point(580, 410);
            this.numBeamHeading.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numBeamHeading.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeamHeading.Name = "numBeamHeading";
            this.numBeamHeading.Size = new System.Drawing.Size(42, 20);
            this.numBeamHeading.TabIndex = 105;
            this.toolTip1.SetToolTip(this.numBeamHeading, "Enter Angle to Point Antenna.\r\n\r\nRight Click on Angle to MOVE Antenna to new Posi" +
        "tion.\r\n\r\nChanging the value will stop the Antenna.\r\n\r\nClick Rotor button to Move" +
        " Antenna to new Position");
            this.numBeamHeading.Value = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.numBeamHeading.Visible = false;
            this.numBeamHeading.ValueChanged += new System.EventHandler(this.numBeamHeading_ValueChanged);
            // 
            // chkISS
            // 
            this.chkISS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkISS.Checked = true;
            this.chkISS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkISS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkISS.Image = null;
            this.chkISS.Location = new System.Drawing.Point(367, 325);
            this.chkISS.Name = "chkISS";
            this.chkISS.Size = new System.Drawing.Size(46, 20);
            this.chkISS.TabIndex = 104;
            this.chkISS.Text = "ISS";
            this.toolTip1.SetToolTip(this.chkISS, resources.GetString("chkISS.ToolTip"));
            this.chkISS.CheckedChanged += new System.EventHandler(this.chkISS_CheckedChanged);
            // 
            // chkMoon
            // 
            this.chkMoon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMoon.Checked = true;
            this.chkMoon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMoon.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMoon.Image = null;
            this.chkMoon.Location = new System.Drawing.Point(320, 325);
            this.chkMoon.Name = "chkMoon";
            this.chkMoon.Size = new System.Drawing.Size(58, 20);
            this.chkMoon.TabIndex = 103;
            this.chkMoon.Text = "Moon";
            this.toolTip1.SetToolTip(this.chkMoon, resources.GetString("chkMoon.ToolTip"));
            this.chkMoon.CheckedChanged += new System.EventHandler(this.chkMoon_CheckedChanged);
            // 
            // chkBoxContour
            // 
            this.chkBoxContour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxContour.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxContour.Image = null;
            this.chkBoxContour.Location = new System.Drawing.Point(349, 416);
            this.chkBoxContour.Name = "chkBoxContour";
            this.chkBoxContour.Size = new System.Drawing.Size(70, 20);
            this.chkBoxContour.TabIndex = 98;
            this.chkBoxContour.Text = "Contour";
            this.toolTip1.SetToolTip(this.chkBoxContour, resources.GetString("chkBoxContour.ToolTip"));
            this.chkBoxContour.CheckedChanged += new System.EventHandler(this.chkBoxContour_CheckedChanged);
            // 
            // tbPanPower
            // 
            this.tbPanPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPanPower.AutoSize = false;
            this.tbPanPower.Location = new System.Drawing.Point(343, 457);
            this.tbPanPower.Maximum = 1500;
            this.tbPanPower.Minimum = 1;
            this.tbPanPower.Name = "tbPanPower";
            this.tbPanPower.Size = new System.Drawing.Size(66, 18);
            this.tbPanPower.TabIndex = 97;
            this.tbPanPower.TickFrequency = 90;
            this.toolTip1.SetToolTip(this.tbPanPower, "HIT F2 to watch video showing how to use VOACAP\r\n\r\nVOACAP: 400 Watts");
            this.tbPanPower.Value = 400;
            this.tbPanPower.Scroll += new System.EventHandler(this.tbPanPower_Scroll);
            this.tbPanPower.MouseEnter += new System.EventHandler(this.tbPanPower_MouseEnter);
            this.tbPanPower.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbPanPower_MouseUp);
            // 
            // chkBoxAnt
            // 
            this.chkBoxAnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxAnt.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxAnt.Image = null;
            this.chkBoxAnt.Location = new System.Drawing.Point(349, 432);
            this.chkBoxAnt.Name = "chkBoxAnt";
            this.chkBoxAnt.Size = new System.Drawing.Size(55, 24);
            this.chkBoxAnt.TabIndex = 96;
            this.chkBoxAnt.Text = "Beam";
            this.toolTip1.SetToolTip(this.chkBoxAnt, resources.GetString("chkBoxAnt.ToolTip"));
            this.chkBoxAnt.CheckedChanged += new System.EventHandler(this.chkBoxAnt_CheckedChanged);
            // 
            // chkBoxDIG
            // 
            this.chkBoxDIG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxDIG.Checked = true;
            this.chkBoxDIG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxDIG.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxDIG.Image = null;
            this.chkBoxDIG.Location = new System.Drawing.Point(182, 367);
            this.chkBoxDIG.Name = "chkBoxDIG";
            this.chkBoxDIG.Size = new System.Drawing.Size(85, 24);
            this.chkBoxDIG.TabIndex = 70;
            this.chkBoxDIG.Text = "Spot Digital";
            this.toolTip1.SetToolTip(this.chkBoxDIG, "Show Digital spots when checked (like RTTY, PSK, FT8, etc)\r\n");
            // 
            // checkBoxMUF
            // 
            this.checkBoxMUF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMUF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxMUF.Image = null;
            this.checkBoxMUF.Location = new System.Drawing.Point(343, 394);
            this.checkBoxMUF.Name = "checkBoxMUF";
            this.checkBoxMUF.Size = new System.Drawing.Size(75, 20);
            this.checkBoxMUF.TabIndex = 95;
            this.checkBoxMUF.Text = "VOACAP";
            this.toolTip1.SetToolTip(this.checkBoxMUF, resources.GetString("checkBoxMUF.ToolTip"));
            this.checkBoxMUF.CheckedChanged += new System.EventHandler(this.checkBoxMUF_CheckedChanged);
            this.checkBoxMUF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxMUF_MouseDown);
            // 
            // udDisplayWWV
            // 
            this.udDisplayWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayWWV.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Location = new System.Drawing.Point(513, 376);
            this.udDisplayWWV.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udDisplayWWV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Name = "udDisplayWWV";
            this.udDisplayWWV.Size = new System.Drawing.Size(39, 20);
            this.udDisplayWWV.TabIndex = 91;
            this.toolTip1.SetToolTip(this.udDisplayWWV, resources.GetString("udDisplayWWV.ToolTip"));
            this.udDisplayWWV.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBoxWWV
            // 
            this.checkBoxWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWWV.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxWWV.Image = null;
            this.checkBoxWWV.Location = new System.Drawing.Point(513, 350);
            this.checkBoxWWV.Name = "checkBoxWWV";
            this.checkBoxWWV.Size = new System.Drawing.Size(98, 24);
            this.checkBoxWWV.TabIndex = 90;
            this.checkBoxWWV.Text = "Use WWV HF";
            this.toolTip1.SetToolTip(this.checkBoxWWV, resources.GetString("checkBoxWWV.ToolTip"));
            this.checkBoxWWV.CheckedChanged += new System.EventHandler(this.checkBoxWWV_CheckedChanged);
            // 
            // numericUpDownTS1
            // 
            this.numericUpDownTS1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownTS1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Location = new System.Drawing.Point(417, 470);
            this.numericUpDownTS1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTS1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Name = "numericUpDownTS1";
            this.numericUpDownTS1.Size = new System.Drawing.Size(39, 20);
            this.numericUpDownTS1.TabIndex = 88;
            this.toolTip1.SetToolTip(this.numericUpDownTS1, "Which Band to Start Slow Beacaon Scan on:\r\n1=14.1mhz\r\n2=18.11mhz\r\n3=21.15mhz\r\n4=2" +
        "4.93mhz\r\n5=28.2mhz\r\n");
            this.numericUpDownTS1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.ValueChanged += new System.EventHandler(this.numericUpDownTS1_ValueChanged);
            // 
            // BoxBFScan
            // 
            this.BoxBFScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBFScan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BoxBFScan.Image = null;
            this.BoxBFScan.Location = new System.Drawing.Point(343, 477);
            this.BoxBFScan.Name = "BoxBFScan";
            this.BoxBFScan.Size = new System.Drawing.Size(87, 24);
            this.BoxBFScan.TabIndex = 87;
            this.BoxBFScan.Text = "Slow Scan";
            this.toolTip1.SetToolTip(this.BoxBFScan, resources.GetString("BoxBFScan.ToolTip"));
            this.BoxBFScan.CheckedChanged += new System.EventHandler(this.BoxBFScan_CheckedChanged);
            // 
            // BoxBScan
            // 
            this.BoxBScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBScan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BoxBScan.Image = null;
            this.BoxBScan.Location = new System.Drawing.Point(273, 477);
            this.BoxBScan.Name = "BoxBScan";
            this.BoxBScan.Size = new System.Drawing.Size(81, 24);
            this.BoxBScan.TabIndex = 86;
            this.BoxBScan.Text = "Fast Scan";
            this.toolTip1.SetToolTip(this.BoxBScan, "Check to Scan all 18 Beacon Stations 5 Frequecies at each 10 second Interval\r\nPow" +
        "erSDR will move across all 5 Beacon Frequencies in 1 sec intervals \r\n\r\nTotal bea" +
        "con map is compled in 3 minutes.\r\n\r\n");
            this.BoxBScan.CheckedChanged += new System.EventHandler(this.BoxBScan_CheckedChanged);
            // 
            // chkBoxBeam
            // 
            this.chkBoxBeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxBeam.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxBeam.Image = null;
            this.chkBoxBeam.Location = new System.Drawing.Point(417, 402);
            this.chkBoxBeam.Name = "chkBoxBeam";
            this.chkBoxBeam.Size = new System.Drawing.Size(88, 20);
            this.chkBoxBeam.TabIndex = 83;
            this.chkBoxBeam.Text = "Map Beam°";
            this.toolTip1.SetToolTip(this.chkBoxBeam, "Check To Show Beam heading on map in (deg)\r\n");
            this.chkBoxBeam.CheckedChanged += new System.EventHandler(this.chkBoxBeam_CheckedChanged);
            // 
            // udDisplayLong
            // 
            this.udDisplayLong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLong.DecimalPlaces = 2;
            this.udDisplayLong.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLong.Location = new System.Drawing.Point(614, 471);
            this.udDisplayLong.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.udDisplayLong.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.udDisplayLong.Name = "udDisplayLong";
            this.udDisplayLong.Size = new System.Drawing.Size(62, 20);
            this.udDisplayLong.TabIndex = 80;
            this.toolTip1.SetToolTip(this.udDisplayLong, "Enter Longitude in deg (-180 to 180) for Beam Heading\r\n- for West of 0 GMT line\r\n" +
        "+ for East of 0 GMT line\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to " +
        "\r\ntoggle Lat/Long map");
            this.udDisplayLong.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLong.ValueChanged += new System.EventHandler(this.udDisplayLong_ValueChanged);
            // 
            // udDisplayLat
            // 
            this.udDisplayLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLat.DecimalPlaces = 2;
            this.udDisplayLat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLat.Location = new System.Drawing.Point(538, 471);
            this.udDisplayLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.udDisplayLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.udDisplayLat.Name = "udDisplayLat";
            this.udDisplayLat.Size = new System.Drawing.Size(58, 20);
            this.udDisplayLat.TabIndex = 79;
            this.toolTip1.SetToolTip(this.udDisplayLat, "Enter Latitude in deg (90 to -90) for Beam Heading\r\n+ for Northern Hemisphere\r\n- " +
        "for Southern Hemisphere\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to \r" +
        "\ntoggle Lat/Long map");
            this.udDisplayLat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLat.ValueChanged += new System.EventHandler(this.udDisplayLat_ValueChanged);
            // 
            // chkBoxMem
            // 
            this.chkBoxMem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxMem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxMem.Image = null;
            this.chkBoxMem.Location = new System.Drawing.Point(417, 445);
            this.chkBoxMem.Name = "chkBoxMem";
            this.chkBoxMem.Size = new System.Drawing.Size(123, 20);
            this.chkBoxMem.TabIndex = 74;
            this.chkBoxMem.Text = "MEMORIES to Pan";
            this.toolTip1.SetToolTip(this.chkBoxMem, resources.GetString("chkBoxMem.ToolTip"));
            this.chkBoxMem.CheckedChanged += new System.EventHandler(this.chkBoxMem_CheckedChanged);
            // 
            // chkBoxPan
            // 
            this.chkBoxPan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxPan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxPan.Image = null;
            this.chkBoxPan.Location = new System.Drawing.Point(417, 384);
            this.chkBoxPan.Name = "chkBoxPan";
            this.chkBoxPan.Size = new System.Drawing.Size(100, 20);
            this.chkBoxPan.TabIndex = 71;
            this.chkBoxPan.Text = "Map just Pan";
            this.toolTip1.SetToolTip(this.chkBoxPan, "Show Country or Calls on Map for just the Panadapter freq you are viewing.\r\n");
            this.chkBoxPan.CheckedChanged += new System.EventHandler(this.chkBoxPan_CheckedChanged);
            // 
            // chkBoxSSB
            // 
            this.chkBoxSSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSSB.Checked = true;
            this.chkBoxSSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxSSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxSSB.Image = null;
            this.chkBoxSSB.Location = new System.Drawing.Point(182, 346);
            this.chkBoxSSB.Name = "chkBoxSSB";
            this.chkBoxSSB.Size = new System.Drawing.Size(85, 24);
            this.chkBoxSSB.TabIndex = 69;
            this.chkBoxSSB.Text = "Spot Phone";
            this.toolTip1.SetToolTip(this.chkBoxSSB, "Show SSB,FM,AM spots when checked\r\n");
            // 
            // chkBoxCW
            // 
            this.chkBoxCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxCW.Checked = true;
            this.chkBoxCW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxCW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxCW.Image = null;
            this.chkBoxCW.Location = new System.Drawing.Point(182, 323);
            this.chkBoxCW.Name = "chkBoxCW";
            this.chkBoxCW.Size = new System.Drawing.Size(85, 24);
            this.chkBoxCW.TabIndex = 68;
            this.chkBoxCW.Text = "Spot CW";
            this.toolTip1.SetToolTip(this.chkBoxCW, "Show CW spots when checked\r\n");
            // 
            // chkMapBand
            // 
            this.chkMapBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapBand.Checked = true;
            this.chkMapBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapBand.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapBand.Image = null;
            this.chkMapBand.Location = new System.Drawing.Point(417, 367);
            this.chkMapBand.Name = "chkMapBand";
            this.chkMapBand.Size = new System.Drawing.Size(113, 20);
            this.chkMapBand.TabIndex = 67;
            this.chkMapBand.Text = "Map just Band";
            this.toolTip1.SetToolTip(this.chkMapBand, "Show Country or Calls on Map for the Band you are on.\r\n");
            this.chkMapBand.CheckedChanged += new System.EventHandler(this.chkMapBand_CheckedChanged);
            // 
            // chkMapCountry
            // 
            this.chkMapCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCountry.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapCountry.Image = null;
            this.chkMapCountry.Location = new System.Drawing.Point(417, 327);
            this.chkMapCountry.Name = "chkMapCountry";
            this.chkMapCountry.Size = new System.Drawing.Size(88, 20);
            this.chkMapCountry.TabIndex = 66;
            this.chkMapCountry.Text = "Map Country";
            this.toolTip1.SetToolTip(this.chkMapCountry, "Show Dx spot Countries on Map\r\n");
            this.chkMapCountry.CheckedChanged += new System.EventHandler(this.chkMapCountry_CheckedChanged);
            // 
            // chkMapCall
            // 
            this.chkMapCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCall.Checked = true;
            this.chkMapCall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapCall.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapCall.Image = null;
            this.chkMapCall.Location = new System.Drawing.Point(417, 344);
            this.chkMapCall.Name = "chkMapCall";
            this.chkMapCall.Size = new System.Drawing.Size(88, 20);
            this.chkMapCall.TabIndex = 65;
            this.chkMapCall.Text = "Map Calls";
            this.toolTip1.SetToolTip(this.chkMapCall, "Show DX Spot Call signs on Map");
            this.chkMapCall.CheckedChanged += new System.EventHandler(this.chkMapCall_CheckedChanged);
            // 
            // chkPanMode
            // 
            this.chkPanMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPanMode.Checked = true;
            this.chkPanMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPanMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkPanMode.Image = null;
            this.chkPanMode.Location = new System.Drawing.Point(273, 369);
            this.chkPanMode.Name = "chkPanMode";
            this.chkPanMode.Size = new System.Drawing.Size(148, 20);
            this.chkPanMode.TabIndex = 63;
            this.chkPanMode.Text = "Special PanaFall Mode\r\n";
            this.toolTip1.SetToolTip(this.chkPanMode, "When Checked, will Display RX1 in Panafall mode, with a small waterfall for bette" +
        "r viewing of the map\r\nThis is equivalent to 80/20 Panafall");
            this.chkPanMode.CheckedChanged += new System.EventHandler(this.chkPanMode_CheckedChanged);
            // 
            // chkGrayLine
            // 
            this.chkGrayLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGrayLine.Checked = true;
            this.chkGrayLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGrayLine.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkGrayLine.Image = null;
            this.chkGrayLine.Location = new System.Drawing.Point(273, 350);
            this.chkGrayLine.Name = "chkGrayLine";
            this.chkGrayLine.Size = new System.Drawing.Size(105, 17);
            this.chkGrayLine.TabIndex = 61;
            this.chkGrayLine.Text = "GrayLine Track";
            this.toolTip1.SetToolTip(this.chkGrayLine, "GrayLine will show on Panadapter Display\r\nBut only when you run the Tracking Worl" +
        "d Map (click on the Track/Map button)");
            this.chkGrayLine.CheckedChanged += new System.EventHandler(this.chkGrayLine_CheckedChanged);
            // 
            // chkSUN
            // 
            this.chkSUN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSUN.Checked = true;
            this.chkSUN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSUN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkSUN.Image = null;
            this.chkSUN.Location = new System.Drawing.Point(273, 325);
            this.chkSUN.Name = "chkSUN";
            this.chkSUN.Size = new System.Drawing.Size(46, 20);
            this.chkSUN.TabIndex = 60;
            this.chkSUN.Text = "Sun";
            this.toolTip1.SetToolTip(this.chkSUN, "True (Zenith) position of the Sun will show on Panadapter screen (including space" +
        " weather)\r\nWhen you Click on the TRACK/Map button to activate the World Map\r\n");
            this.chkSUN.CheckedChanged += new System.EventHandler(this.chkSUN_CheckedChanged);
            // 
            // hkBoxSpotRX2
            // 
            this.hkBoxSpotRX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hkBoxSpotRX2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.hkBoxSpotRX2.Image = null;
            this.hkBoxSpotRX2.Location = new System.Drawing.Point(111, 402);
            this.hkBoxSpotRX2.Name = "hkBoxSpotRX2";
            this.hkBoxSpotRX2.Size = new System.Drawing.Size(78, 19);
            this.hkBoxSpotRX2.TabIndex = 119;
            this.hkBoxSpotRX2.Text = "RX2 Band ";
            this.toolTip1.SetToolTip(this.hkBoxSpotRX2, "Only Show DX Spots on RX2 Band");
            this.hkBoxSpotRX2.CheckedChanged += new System.EventHandler(this.hkBoxSpotRX2_CheckedChanged);
            // 
            // hkBoxSpotBand
            // 
            this.hkBoxSpotBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hkBoxSpotBand.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.hkBoxSpotBand.Image = null;
            this.hkBoxSpotBand.Location = new System.Drawing.Point(12, 402);
            this.hkBoxSpotBand.Name = "hkBoxSpotBand";
            this.hkBoxSpotBand.Size = new System.Drawing.Size(103, 19);
            this.hkBoxSpotBand.TabIndex = 118;
            this.hkBoxSpotBand.Text = "RX1 Band Only\r\n";
            this.toolTip1.SetToolTip(this.hkBoxSpotBand, "Only Show DX Spots on RX1 Band");
            this.hkBoxSpotBand.CheckedChanged += new System.EventHandler(this.hkBoxSpotBand_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(537, 449);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Your Lat and Long (+/- deg)\r\n";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(693, 449);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 82;
            this.label4.Text = "Your Call sign";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(541, 434);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "Setup->CAT Control->DDUtil , for Rotor Control";
            // 
            // menuItem2
            // 
            this.menuItem2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(100, 23);
            // 
            // mnuSpotOptions
            // 
            this.mnuSpotOptions.Index = 1;
            this.mnuSpotOptions.Text = "VOACAP Override";
            this.mnuSpotOptions.Click += new System.EventHandler(this.mnuSpotOptions_Click);
            // 
            // chkTimeServer1
            // 
            this.chkTimeServer1.Index = 0;
            this.chkTimeServer1.Text = "utcnist.colorado.edu";
            this.chkTimeServer1.Click += new System.EventHandler(this.chkTimeServer1_Click);
            // 
            // chkTimeServer2
            // 
            this.chkTimeServer2.Index = 1;
            this.chkTimeServer2.Text = "utcnist2.colorado.edu";
            this.chkTimeServer2.Click += new System.EventHandler(this.chkTimeServer2_Click);
            // 
            // chkTimeServer3
            // 
            this.chkTimeServer3.Index = 2;
            this.chkTimeServer3.Text = "time-c.nist.gov";
            this.chkTimeServer3.Click += new System.EventHandler(this.chkTimeServer3_Click);
            // 
            // chkTimeServer4
            // 
            this.chkTimeServer4.Index = 3;
            this.chkTimeServer4.Text = "time-b.nist.gov";
            this.chkTimeServer4.Click += new System.EventHandler(this.chkTimeServer4_Click);
            // 
            // chkTimeServer5
            // 
            this.chkTimeServer5.Index = 4;
            this.chkTimeServer5.Text = "time-a.nist.gov";
            this.chkTimeServer5.Click += new System.EventHandler(this.chkTimeServer5_Click);
            // 
            // menuTimeServers
            // 
            this.menuTimeServers.Index = 2;
            this.menuTimeServers.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.chkTimeServer1,
            this.chkTimeServer2,
            this.chkTimeServer3,
            this.chkTimeServer4,
            this.chkTimeServer5});
            this.menuTimeServers.Text = "Time Servers List";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "Decoders";
            this.menuItem1.Visible = false;
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.Text = "";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSpotAge,
            this.mnuSpotOptions,
            this.menuTimeServers,
            this.menuItem1,
            this.menuItem3});
            // 
            // mnuSpotAge
            // 
            this.mnuSpotAge.Index = 0;
            this.mnuSpotAge.Text = "Spotter Settings";
            this.mnuSpotAge.Click += new System.EventHandler(this.MenuItem4_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(587, 413);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 107;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(559, 413);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 13);
            this.label7.TabIndex = 110;
            this.label7.Text = "<--";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(13, 7);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(279, 31);
            this.richTextBox1.TabIndex = 120;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "Additional DX telnet addresses: http://www.dxcluster.info/telnet/index.php.\n";
            // 
            // chkBoxWrld
            // 
            this.chkBoxWrld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxWrld.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxWrld.Image = null;
            this.chkBoxWrld.Location = new System.Drawing.Point(12, 441);
            this.chkBoxWrld.Name = "chkBoxWrld";
            this.chkBoxWrld.Size = new System.Drawing.Size(215, 20);
            this.chkBoxWrld.TabIndex = 78;
            this.chkBoxWrld.Text = "Exclude North American Spotters";
            this.chkBoxWrld.CheckedChanged += new System.EventHandler(this.chkBoxWrld_CheckedChanged);
            // 
            // chkBoxNA
            // 
            this.chkBoxNA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxNA.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxNA.Image = null;
            this.chkBoxNA.Location = new System.Drawing.Point(12, 424);
            this.chkBoxNA.Name = "chkBoxNA";
            this.chkBoxNA.Size = new System.Drawing.Size(177, 19);
            this.chkBoxNA.TabIndex = 77;
            this.chkBoxNA.Text = "North American Spotters only";
            this.chkBoxNA.CheckedChanged += new System.EventHandler(this.chkBoxNA_CheckedChanged);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(682, 397);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 24);
            this.chkAlwaysOnTop.TabIndex = 58;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // chkDXMode
            // 
            this.chkDXMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDXMode.Checked = true;
            this.chkDXMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDXMode.Image = null;
            this.chkDXMode.Location = new System.Drawing.Point(682, 470);
            this.chkDXMode.Name = "chkDXMode";
            this.chkDXMode.Size = new System.Drawing.Size(91, 24);
            this.chkDXMode.TabIndex = 59;
            this.chkDXMode.Text = "Parse \"DX Spot\" Mode";
            this.chkDXMode.UseVisualStyleBackColor = true;
            this.chkDXMode.Visible = false;
            // 
            // SpotControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(784, 502);
            this.Controls.Add(this.chkDLayerON);
            this.Controls.Add(this.txtLoTWpass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.chkBoxBeacon);
            this.Controls.Add(this.chkVoacap);
            this.Controls.Add(this.chkDXOn);
            this.Controls.Add(this.chkMapOn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RotorHead);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkBoxBandText);
            this.Controls.Add(this.numBeamHeading);
            this.Controls.Add(this.chkISS);
            this.Controls.Add(this.chkMoon);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBoxDXCall);
            this.Controls.Add(this.DXPost);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.tbPanPower);
            this.Controls.Add(this.chkBoxAnt);
            this.Controls.Add(this.chkBoxDIG);
            this.Controls.Add(this.checkBoxMUF);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.checkBoxTone);
            this.Controls.Add(this.udDisplayWWV);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.numericUpDownTS1);
            this.Controls.Add(this.BoxBFScan);
            this.Controls.Add(this.BoxBScan);
            this.Controls.Add(this.btnBeacon);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkBoxBeam);
            this.Controls.Add(this.SWLbutton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.udDisplayLong);
            this.Controls.Add(this.udDisplayLat);
            this.Controls.Add(this.chkBoxWrld);
            this.Controls.Add(this.chkBoxNA);
            this.Controls.Add(this.SWLbutton2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.chkBoxMem);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkBoxPan);
            this.Controls.Add(this.chkBoxSSB);
            this.Controls.Add(this.chkBoxCW);
            this.Controls.Add(this.chkMapBand);
            this.Controls.Add(this.chkMapCountry);
            this.Controls.Add(this.chkMapCall);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.chkPanMode);
            this.Controls.Add(this.chkGrayLine);
            this.Controls.Add(this.chkSUN);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBoxSWL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.portBox2);
            this.Controls.Add(this.callBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.nodeBox1);
            this.Controls.Add(this.SSBbutton);
            this.Controls.Add(this.chkDXMode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.hkBoxSpotRX2);
            this.Controls.Add(this.hkBoxSpotBand);
            this.Controls.Add(this.chkBoxContour);
            this.Controls.Add(this.chkFLayerON);
            this.Controls.Add(this.checkBoxWWV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(800, 1000);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "SpotControl";
            this.Text = "DX / SWL Spotter";
            this.Deactivate += new System.EventHandler(this.SpotControl_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpotControl_FormClosing);
            this.Load += new System.EventHandler(this.SpotControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpotControl_KeyDown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SpotControl_Layout);
            this.MouseEnter += new System.EventHandler(this.SpotControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.SpotControl_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeamHeading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        } //initializecomponents
        #endregion

        public System.Windows.Forms.Button SWLbutton;
        public System.Windows.Forms.Button SSBbutton;
        public System.Windows.Forms.RichTextBox textBox1;
        public System.Windows.Forms.TextBox nodeBox1;
        private System.Windows.Forms.RichTextBox textBox3;
        public System.Windows.Forms.TextBox callBox;
        public System.Windows.Forms.TextBox portBox2;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox statusBoxSWL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
        public System.Windows.Forms.CheckBoxTS chkDXMode;
        public System.Windows.Forms.CheckBoxTS chkSUN;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.CheckBoxTS chkGrayLine;
        public System.Windows.Forms.Button btnTrack;
        public System.Windows.Forms.CheckBoxTS chkPanMode;
        public System.Windows.Forms.TextBox nameBox;
        public System.Windows.Forms.CheckBoxTS chkMapCall;
        public System.Windows.Forms.CheckBoxTS chkMapCountry;
        public System.Windows.Forms.CheckBoxTS chkMapBand;
        public System.Windows.Forms.CheckBoxTS chkBoxCW;
        public System.Windows.Forms.CheckBoxTS chkBoxSSB;
        public System.Windows.Forms.CheckBoxTS chkBoxDIG;
        public System.Windows.Forms.CheckBoxTS chkBoxPan;
        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.CheckBoxTS chkBoxMem;
        public System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button SWLbutton2;
        public System.Windows.Forms.CheckBoxTS chkBoxNA;
        public System.Windows.Forms.CheckBoxTS chkBoxWrld;
        public System.Windows.Forms.NumericUpDownTS udDisplayLat;
        public System.Windows.Forms.NumericUpDownTS udDisplayLong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBoxTS chkBoxBeam;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBeacon;
        public System.Windows.Forms.CheckBoxTS BoxBScan;
        public System.Windows.Forms.CheckBoxTS BoxBFScan;
        private System.Windows.Forms.NumericUpDownTS numericUpDownTS1;
        private System.Windows.Forms.Button btnTime;
        public System.Windows.Forms.CheckBoxTS checkBoxWWV;
        private System.Windows.Forms.NumericUpDownTS udDisplayWWV;

        private System.Windows.Forms.RadioButton checkBoxTone;
        private System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.CheckBoxTS checkBoxMUF;
        public System.Windows.Forms.CheckBoxTS chkBoxAnt;
        public System.Windows.Forms.TrackBarTS tbPanPower;
        public PowerSDR.DSP dsp;
        public System.Windows.Forms.CheckBoxTS chkDLayerON;
        public System.Windows.Forms.CheckBoxTS chkFLayerON;

        public System.Windows.Forms.CheckBoxTS chkBoxContour;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button DXPost;
        private System.Windows.Forms.TextBox textBoxDXCall;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.CheckBoxTS chkMoon;
        public System.Windows.Forms.CheckBoxTS chkISS;
        public System.Windows.Forms.NumericUpDownTS numBeamHeading;
        public System.Windows.Forms.CheckBoxTS chkBoxBandText;
        private System.Windows.Forms.ToolStripTextBox menuItem2;
        public System.Windows.Forms.MenuItem mnuSpotOptions;
        private System.Windows.Forms.MenuItem chkTimeServer1;
        private System.Windows.Forms.MenuItem chkTimeServer2;
        private System.Windows.Forms.MenuItem chkTimeServer3;
        private System.Windows.Forms.MenuItem chkTimeServer4;
        private System.Windows.Forms.MenuItem chkTimeServer5;
        private System.Windows.Forms.MenuItem menuTimeServers;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.TextBox RotorHead;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.CheckBoxTS chkMapOn;
        public System.Windows.Forms.CheckBoxTS chkDXOn;
        public System.Windows.Forms.CheckBoxTS chkVoacap;
        public System.Windows.Forms.CheckBoxTS chkBoxBeacon;
        public System.Windows.Forms.TextBoxTS txtLoTWpass;
        public System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuItem mnuSpotAge;
        public System.Windows.Forms.CheckBoxTS hkBoxSpotBand;
        public System.Windows.Forms.CheckBoxTS hkBoxSpotRX2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private TimeProc timeProcPeriodic;



    } // Spotcontrol




} // powersdr
