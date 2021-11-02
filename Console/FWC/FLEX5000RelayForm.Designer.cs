//=================================================================
// FLEX5000RelayForm.cs
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
    public partial class FLEX5000RelayForm : System.Windows.Forms.Form
    {
       

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLEX5000RelayForm));
            this.lblTurn = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.lblWhen = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.btnMoreLess = new System.Windows.Forms.Button();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.lstRules = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMoveToTop = new System.Windows.Forms.Button();
            this.btnMoveToEnd = new System.Windows.Forms.Button();
            this.btnTX1 = new System.Windows.Forms.Button();
            this.btnTX2 = new System.Windows.Forms.Button();
            this.btnTX3 = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTurn
            // 
            this.lblTurn.Location = new System.Drawing.Point(16, 16);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(32, 23);
            this.lblTurn.TabIndex = 0;
            this.lblTurn.Text = "Turn";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.comboBox1.Location = new System.Drawing.Point(48, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(48, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Items.AddRange(new object[] {
            "TX1",
            "TX2",
            "TX3"});
            this.comboBox2.Location = new System.Drawing.Point(104, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(48, 21);
            this.comboBox2.TabIndex = 2;
            // 
            // lblWhen
            // 
            this.lblWhen.Location = new System.Drawing.Point(160, 16);
            this.lblWhen.Name = "lblWhen";
            this.lblWhen.Size = new System.Drawing.Size(32, 23);
            this.lblWhen.TabIndex = 3;
            this.lblWhen.Text = "when";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Items.AddRange(new object[] {
            "Power",
            "Band",
            "Mode"});
            this.comboBox3.Location = new System.Drawing.Point(200, 8);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(56, 21);
            this.comboBox3.TabIndex = 4;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.Items.AddRange(new object[] {
            "Is",
            "Is_Not"});
            this.comboBox4.Location = new System.Drawing.Point(264, 8);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(56, 21);
            this.comboBox4.TabIndex = 5;
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.comboBox5.Location = new System.Drawing.Point(328, 8);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(56, 21);
            this.comboBox5.TabIndex = 6;
            // 
            // btnMoreLess
            // 
            this.btnMoreLess.Location = new System.Drawing.Point(392, 8);
            this.btnMoreLess.Name = "btnMoreLess";
            this.btnMoreLess.Size = new System.Drawing.Size(40, 23);
            this.btnMoreLess.TabIndex = 7;
            this.btnMoreLess.Text = "More";
            this.btnMoreLess.Click += new System.EventHandler(this.btnMoreLess_Click);
            // 
            // comboBox6
            // 
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.Items.AddRange(new object[] {
            "Power",
            "Band",
            "Mode"});
            this.comboBox6.Location = new System.Drawing.Point(200, 40);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(56, 21);
            this.comboBox6.TabIndex = 11;
            this.comboBox6.Visible = false;
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            // 
            // comboBox7
            // 
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.Items.AddRange(new object[] {
            "Is",
            "Is_Not"});
            this.comboBox7.Location = new System.Drawing.Point(264, 40);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(56, 21);
            this.comboBox7.TabIndex = 10;
            this.comboBox7.Visible = false;
            // 
            // comboBox8
            // 
            this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox8.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.comboBox8.Location = new System.Drawing.Point(328, 40);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(56, 21);
            this.comboBox8.TabIndex = 9;
            this.comboBox8.Visible = false;
            // 
            // lblAnd
            // 
            this.lblAnd.Location = new System.Drawing.Point(160, 48);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(32, 23);
            this.lblAnd.TabIndex = 8;
            this.lblAnd.Text = "and";
            this.lblAnd.Visible = false;
            // 
            // lstRules
            // 
            this.lstRules.Location = new System.Drawing.Point(24, 88);
            this.lstRules.Name = "lstRules";
            this.lstRules.Size = new System.Drawing.Size(288, 121);
            this.lstRules.TabIndex = 12;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(120, 224);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(32, 224);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(328, 120);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUp.TabIndex = 15;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(328, 152);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDown.TabIndex = 16;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(208, 224);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnMoveToTop
            // 
            this.btnMoveToTop.Location = new System.Drawing.Point(328, 88);
            this.btnMoveToTop.Name = "btnMoveToTop";
            this.btnMoveToTop.Size = new System.Drawing.Size(80, 23);
            this.btnMoveToTop.TabIndex = 18;
            this.btnMoveToTop.Text = "Move To Top";
            this.btnMoveToTop.Click += new System.EventHandler(this.btnMoveToTop_Click);
            // 
            // btnMoveToEnd
            // 
            this.btnMoveToEnd.Location = new System.Drawing.Point(328, 184);
            this.btnMoveToEnd.Name = "btnMoveToEnd";
            this.btnMoveToEnd.Size = new System.Drawing.Size(80, 23);
            this.btnMoveToEnd.TabIndex = 19;
            this.btnMoveToEnd.Text = "Move To End";
            this.btnMoveToEnd.Click += new System.EventHandler(this.btnMoveToEnd_Click);
            // 
            // btnTX1
            // 
            this.btnTX1.BackColor = System.Drawing.Color.Red;
            this.btnTX1.Location = new System.Drawing.Point(320, 232);
            this.btnTX1.Name = "btnTX1";
            this.btnTX1.Size = new System.Drawing.Size(40, 23);
            this.btnTX1.TabIndex = 20;
            this.btnTX1.Text = "TX1";
            this.btnTX1.UseVisualStyleBackColor = false;
            // 
            // btnTX2
            // 
            this.btnTX2.BackColor = System.Drawing.Color.Red;
            this.btnTX2.Location = new System.Drawing.Point(368, 232);
            this.btnTX2.Name = "btnTX2";
            this.btnTX2.Size = new System.Drawing.Size(40, 23);
            this.btnTX2.TabIndex = 21;
            this.btnTX2.Text = "TX2";
            this.btnTX2.UseVisualStyleBackColor = false;
            // 
            // btnTX3
            // 
            this.btnTX3.BackColor = System.Drawing.Color.Red;
            this.btnTX3.Location = new System.Drawing.Point(416, 232);
            this.btnTX3.Name = "btnTX3";
            this.btnTX3.Size = new System.Drawing.Size(40, 23);
            this.btnTX3.TabIndex = 22;
            this.btnTX3.Text = "TX3";
            this.btnTX3.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(232, 264);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 23;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FLEX5000RelayForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(472, 294);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnTX3);
            this.Controls.Add(this.btnTX2);
            this.Controls.Add(this.btnTX1);
            this.Controls.Add(this.btnMoveToEnd);
            this.Controls.Add(this.btnMoveToTop);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lstRules);
            this.Controls.Add(this.comboBox6);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.comboBox8);
            this.Controls.Add(this.lblAnd);
            this.Controls.Add(this.btnMoreLess);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.lblWhen);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblTurn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FLEX5000RelayForm";
            this.Text = "FLEX-5000 Relay Controls";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FLEX5000RelayForm_Closing);
            this.ResumeLayout(false);

        }
        #endregion

       
        private System.Windows.Forms.Label lblTurn;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label lblWhen;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Button btnMoreLess;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.ListBox lstRules;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnMoveToTop;
        private System.Windows.Forms.Button btnMoveToEnd;
        private System.Windows.Forms.Button btnTX1;
        private System.Windows.Forms.Button btnTX2;
        private System.Windows.Forms.Button btnTX3;
        private System.Windows.Forms.Button btnUpdate;
        private System.ComponentModel.Container components = null;

    
    }
}
