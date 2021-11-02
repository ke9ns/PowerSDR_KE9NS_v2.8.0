//=================================================================
// DiversityForm.cs
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
    /// <summary>
    /// Summary description for DiversityForm.
    /// </summary>
    public partial class DiversityForm : System.Windows.Forms.Form
    {
      

        public DiversityForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //

            InitializeComponent();
            console = c;
            console.RadarColorUpdate = true;

            Common.RestoreForm(this, "DiversityForm", true); // ke9ns: was false

            this.TopMost = chkAlwaysOnTop.Checked;


        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityForm));
            this.picRadar = new System.Windows.Forms.PictureBox();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.chkLockAngle = new System.Windows.Forms.CheckBox();
            this.chkLockR = new System.Windows.Forms.CheckBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnBump180 = new System.Windows.Forms.ButtonTS();
            this.btnBump45 = new System.Windows.Forms.ButtonTS();
            this.udAngle = new System.Windows.Forms.NumericUpDownTS();
            this.udR = new System.Windows.Forms.NumericUpDownTS();
            ((System.ComponentModel.ISupportInitialize)(this.picRadar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udR)).BeginInit();
            this.SuspendLayout();
            // 
            // picRadar
            // 
            this.picRadar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picRadar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.picRadar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picRadar.Location = new System.Drawing.Point(0, 0);
            this.picRadar.Name = "picRadar";
            this.picRadar.Size = new System.Drawing.Size(303, 283);
            this.picRadar.TabIndex = 0;
            this.picRadar.TabStop = false;
            this.picRadar.Paint += new System.Windows.Forms.PaintEventHandler(this.picRadar_Paint);
            this.picRadar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseDown);
            this.picRadar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseMove);
            this.picRadar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseUp);
            this.picRadar.Resize += new System.EventHandler(this.picRadar_Resize);
            // 
            // chkAuto
            // 
            this.chkAuto.Enabled = false;
            this.chkAuto.Location = new System.Drawing.Point(12, 453);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(48, 24);
            this.chkAuto.TabIndex = 1;
            this.chkAuto.Text = "Auto";
            this.chkAuto.Visible = false;
            // 
            // chkLockAngle
            // 
            this.chkLockAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkLockAngle.ForeColor = System.Drawing.Color.White;
            this.chkLockAngle.Location = new System.Drawing.Point(7, 289);
            this.chkLockAngle.Name = "chkLockAngle";
            this.chkLockAngle.Size = new System.Drawing.Size(80, 24);
            this.chkLockAngle.TabIndex = 2;
            this.chkLockAngle.Text = "Lock Angle";
            this.chkLockAngle.CheckedChanged += new System.EventHandler(this.chkLockAngle_CheckedChanged);
            // 
            // chkLockR
            // 
            this.chkLockR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkLockR.ForeColor = System.Drawing.Color.White;
            this.chkLockR.Location = new System.Drawing.Point(93, 289);
            this.chkLockR.Name = "chkLockR";
            this.chkLockR.Size = new System.Drawing.Size(104, 24);
            this.chkLockR.TabIndex = 3;
            this.chkLockR.Text = "Lock Magnitude";
            this.chkLockR.CheckedChanged += new System.EventHandler(this.chkLockR_CheckedChanged);
            // 
            // btnSync
            // 
            this.btnSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSync.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSync.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSync.FlatAppearance.BorderSize = 0;
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.ForeColor = System.Drawing.Color.White;
            this.btnSync.Location = new System.Drawing.Point(2, 319);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(56, 26);
            this.btnSync.TabIndex = 47;
            this.btnSync.Text = "Sync";
            this.btnSync.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnSync, "Click to SYNC up both RX1 and RX2 receivers");
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // chkEnable
            // 
            this.chkEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnable.BackColor = System.Drawing.Color.Transparent;
            this.chkEnable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkEnable.FlatAppearance.BorderSize = 0;
            this.chkEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnable.ForeColor = System.Drawing.Color.White;
            this.chkEnable.Location = new System.Drawing.Point(64, 319);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(56, 26);
            this.chkEnable.TabIndex = 48;
            this.chkEnable.Text = "Enable";
            this.chkEnable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.chkEnable, resources.GetString("chkEnable.ToolTip"));
            this.chkEnable.UseVisualStyleBackColor = false;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(126, 319);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(56, 26);
            this.btnReset.TabIndex = 51;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.btnReset, "Reset the cursor back to CENTER.\r\nIf ESC is ENABLED, a CENTER cursor bypasses ESC" +
        ".\r\nMove cursor to filter noise or enhance a signal");
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.ForeColor = System.Drawing.Color.White;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(203, 289);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 52;
            this.chkAlwaysOnTop.Text = "Always on Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.ChkAlwaysOnTop_CheckedChanged);
            // 
            // btnBump180
            // 
            this.btnBump180.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBump180.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBump180.FlatAppearance.BorderSize = 0;
            this.btnBump180.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBump180.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBump180.ForeColor = System.Drawing.Color.White;
            this.btnBump180.Image = null;
            this.btnBump180.Location = new System.Drawing.Point(246, 319);
            this.btnBump180.Name = "btnBump180";
            this.btnBump180.Size = new System.Drawing.Size(39, 23);
            this.btnBump180.TabIndex = 50;
            this.btnBump180.Text = "180";
            this.btnBump180.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBump180.UseVisualStyleBackColor = true;
            this.btnBump180.Click += new System.EventHandler(this.btnBump180_Click);
            // 
            // btnBump45
            // 
            this.btnBump45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBump45.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBump45.FlatAppearance.BorderSize = 0;
            this.btnBump45.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBump45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBump45.ForeColor = System.Drawing.Color.White;
            this.btnBump45.Image = null;
            this.btnBump45.Location = new System.Drawing.Point(188, 319);
            this.btnBump45.Name = "btnBump45";
            this.btnBump45.Size = new System.Drawing.Size(39, 23);
            this.btnBump45.TabIndex = 49;
            this.btnBump45.Text = "45";
            this.btnBump45.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBump45.UseVisualStyleBackColor = true;
            this.btnBump45.Click += new System.EventHandler(this.btnBump45_Click);
            // 
            // udAngle
            // 
            this.udAngle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.udAngle.DecimalPlaces = 3;
            this.udAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udAngle.Location = new System.Drawing.Point(271, 352);
            this.udAngle.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udAngle.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            -2147483648});
            this.udAngle.Name = "udAngle";
            this.udAngle.Size = new System.Drawing.Size(56, 20);
            this.udAngle.TabIndex = 6;
            this.udAngle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udAngle.ValueChanged += new System.EventHandler(this.udTheta_ValueChanged);
            // 
            // udR
            // 
            this.udR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.udR.DecimalPlaces = 3;
            this.udR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udR.Location = new System.Drawing.Point(248, 352);
            this.udR.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udR.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.udR.Name = "udR";
            this.udR.Size = new System.Drawing.Size(0, 20);
            this.udR.TabIndex = 5;
            this.udR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udR.ValueChanged += new System.EventHandler(this.udR_ValueChanged);
            // 
            // DiversityForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(303, 347);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnBump180);
            this.Controls.Add(this.btnBump45);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.udAngle);
            this.Controls.Add(this.udR);
            this.Controls.Add(this.chkLockR);
            this.Controls.Add(this.chkLockAngle);
            this.Controls.Add(this.chkAuto);
            this.Controls.Add(this.picRadar);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "DiversityForm";
            this.Text = "Enhanced Signal Clarity ™";
            this.toolTip1.SetToolTip(this, "Hit F2 for Video showing ESC in operation");
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DiversityForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiversityForm_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DiversityForm_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiversityForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picRadar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udR)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.PictureBox picRadar;
        private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.CheckBox chkLockAngle;
        private System.Windows.Forms.NumericUpDownTS udR;
        private System.Windows.Forms.NumericUpDownTS udAngle;
        private System.Windows.Forms.CheckBox chkLockR;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.ButtonTS btnBump45;
        private System.Windows.Forms.ButtonTS btnBump180;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;

      
    }
}
