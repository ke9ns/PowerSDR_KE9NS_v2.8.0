//=================================================================
// production_debug.cs
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
    /// Summary description for production_debug.
    /// </summary>
    public partial class ProductionDebug : System.Windows.Forms.Form
    {
       

        #region Constructor and Destructor

        public ProductionDebug(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            console = c;
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

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductionDebug));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radPIOSetOdd = new System.Windows.Forms.RadioButton();
            this.radPIOSetEven = new System.Windows.Forms.RadioButton();
            this.radPIOSetClearAll = new System.Windows.Forms.RadioButton();
            this.radPIORotate = new System.Windows.Forms.RadioButton();
            this.radPIOOff = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radPIOSetOdd);
            this.groupBox1.Controls.Add(this.radPIOSetEven);
            this.groupBox1.Controls.Add(this.radPIOSetClearAll);
            this.groupBox1.Controls.Add(this.radPIORotate);
            this.groupBox1.Controls.Add(this.radPIOOff);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PIO Test";
            // 
            // radPIOSetOdd
            // 
            this.radPIOSetOdd.Location = new System.Drawing.Point(16, 120);
            this.radPIOSetOdd.Name = "radPIOSetOdd";
            this.radPIOSetOdd.Size = new System.Drawing.Size(104, 24);
            this.radPIOSetOdd.TabIndex = 5;
            this.radPIOSetOdd.Text = "Set Odd Bits";
            this.radPIOSetOdd.CheckedChanged += new System.EventHandler(this.radPIOSetOdd_CheckedChanged);
            // 
            // radPIOSetEven
            // 
            this.radPIOSetEven.Location = new System.Drawing.Point(16, 96);
            this.radPIOSetEven.Name = "radPIOSetEven";
            this.radPIOSetEven.Size = new System.Drawing.Size(104, 24);
            this.radPIOSetEven.TabIndex = 4;
            this.radPIOSetEven.Text = "Set Even Bits";
            this.radPIOSetEven.CheckedChanged += new System.EventHandler(this.radPIOSetEven_CheckedChanged);
            // 
            // radPIOSetClearAll
            // 
            this.radPIOSetClearAll.Location = new System.Drawing.Point(16, 72);
            this.radPIOSetClearAll.Name = "radPIOSetClearAll";
            this.radPIOSetClearAll.Size = new System.Drawing.Size(112, 24);
            this.radPIOSetClearAll.TabIndex = 2;
            this.radPIOSetClearAll.Text = "Set/Clear All Bits";
            this.radPIOSetClearAll.CheckedChanged += new System.EventHandler(this.radPIOSetClearAll_CheckedChanged);
            // 
            // radPIORotate
            // 
            this.radPIORotate.Location = new System.Drawing.Point(16, 48);
            this.radPIORotate.Name = "radPIORotate";
            this.radPIORotate.Size = new System.Drawing.Size(104, 24);
            this.radPIORotate.TabIndex = 1;
            this.radPIORotate.Text = "Rotate High Bit";
            this.radPIORotate.CheckedChanged += new System.EventHandler(this.radPIORotate_CheckedChanged);
            // 
            // radPIOOff
            // 
            this.radPIOOff.Checked = true;
            this.radPIOOff.Location = new System.Drawing.Point(16, 24);
            this.radPIOOff.Name = "radPIOOff";
            this.radPIOOff.Size = new System.Drawing.Size(104, 24);
            this.radPIOOff.TabIndex = 0;
            this.radPIOOff.TabStop = true;
            this.radPIOOff.Text = "Off";
            this.radPIOOff.CheckedChanged += new System.EventHandler(this.radPIOOff_CheckedChanged);
            // 
            // ProductionDebug
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductionDebug";
            this.Text = "Production Debug";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ProductionDebug_Closing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

       

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radPIOOff;
        private System.Windows.Forms.RadioButton radPIORotate;
        private System.Windows.Forms.RadioButton radPIOSetClearAll;
        private System.Windows.Forms.RadioButton radPIOSetEven;
        private System.Windows.Forms.RadioButton radPIOSetOdd;
        private System.ComponentModel.Container components = null;

       
    }
}
