

namespace PowerSDR
{
   
    public partial class CATTester : System.Windows.Forms.Form
    {


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CATTester));
            this.btnExit = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBoxTS();
            this.txtResult = new System.Windows.Forms.TextBoxTS();
            this.label1 = new System.Windows.Forms.LabelTS();
            this.label2 = new System.Windows.Forms.LabelTS();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.btnExecute = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(440, 336);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(120, 240);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(168, 20);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyUp);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(120, 280);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(392, 20);
            this.txtResult.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Image = null;
            this.label1.Location = new System.Drawing.Point(16, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "CAT Command";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Image = null;
            this.label2.Location = new System.Drawing.Point(16, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "CAT Response";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // dataGrid1
            // 
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(8, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(512, 224);
            this.dataGrid1.TabIndex = 6;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(312, 240);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 7;
            this.btnExecute.Text = "Execute";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // CATTester
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 382);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CATTester";
            this.Text = "CAT Command Tester";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion



        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBoxTS txtInput;
        private System.Windows.Forms.TextBoxTS txtResult;

        private System.Windows.Forms.LabelTS label1;
        private System.Windows.Forms.LabelTS label2;

        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Button btnExecute;


        private System.ComponentModel.Container components = null;





    }
}
