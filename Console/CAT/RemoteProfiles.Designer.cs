
namespace PowerSDR
{
    /// <summary>
    /// Summary description for RemoteProfiles.
    /// </summary>
    public partial class RemoteProfiles : System.Windows.Forms.Form
    {
          
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteProfiles));
            this.btnClose = new System.Windows.Forms.Button();
            this.cboProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(272, 112);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 24);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboProfiles
            // 
            this.cboProfiles.Location = new System.Drawing.Point(16, 48);
            this.cboProfiles.Name = "cboProfiles";
            this.cboProfiles.Size = new System.Drawing.Size(328, 21);
            this.cboProfiles.TabIndex = 1;
            this.cboProfiles.SelectedIndexChanged += new System.EventHandler(this.cboProfiles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Profile Name";
            // 
            // RemoteProfiles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(360, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboProfiles);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteProfiles";
            this.Activated += new System.EventHandler(this.RemoteProfiles_Activated);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnClose;

        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.ComboBox cboProfiles;

        private System.Windows.Forms.Label label1;

    }
}
