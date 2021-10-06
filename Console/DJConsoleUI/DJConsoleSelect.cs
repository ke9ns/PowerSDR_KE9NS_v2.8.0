using System;
using System.Windows.Forms;

namespace PowerSDR.DJConsoleUI
{
    public partial class DJConsoleSelect : Form
    {
        private Console m_parent;

        private DJConsoleMK2Config ConfigWindowMK2;
        private DJConsoleMP3e2Config ConfigWindowMP3e2;
        private DJConsoleMP3LEConfig ConfigWindowMP3LE;

        public DJConsoleSelect(Console console)
        {
            m_parent = console;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (m_parent.DJConsoleObj.connectedConsoles.Count > 0)
            {
                cbConsoleSelect.DataSource = new BindingSource(m_parent.DJConsoleObj.connectedConsoles, null);
                cbConsoleSelect.DisplayMember = "Value";
                cbConsoleSelect.ValueMember = "Key";
                this.cbConsoleSelect.SelectedIndexChanged += new System.EventHandler(this.cbConsoleSelect_SelectedIndexChanged);
                //                cbConsoleSelect.SelectedValue = m_parent.DJConsoleObj.SelectedConsole;
                this.Show();
            }
            else
            {
                MessageBox.Show("Sorry, no compatible device detected", "Error");
                this.Dispose();
            }

            //if (m_parent.DJConsoleObj.connectedConsoles.Count > 0)
            //{
            //    cbConsoleSelect.DataSource = new BindingSource(m_parent.DJConsoleObj.connectedConsoles, null);
            //    cbConsoleSelect.DisplayMember = "Value";
            //    cbConsoleSelect.ValueMember = "Key";
            //    this.cbConsoleSelect.SelectedIndexChanged += new System.EventHandler(this.cbConsoleSelect_SelectedIndexChanged);
            //    //cbConsoleSelect.Items.Contains(m_parent.DJConsoleObj.SelectedConsole);
            //    cbConsoleSelect.SelectedValue= m_parent.DJConsoleObj.SelectedConsole;
            //    this.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry, no compatible device detected", "Error");
            //    this.Dispose();
            //}
        }


        private void btnConfigure_Click(object sender, EventArgs e)
        {

            if (m_parent.DJConsoleObj.SelectedConsole == 0)
            {
                if (ConfigWindowMP3e2 == null)
                {
                    ConfigWindowMP3e2 = new DJConsoleMP3e2Config(m_parent);
                    ConfigWindowMP3e2.Show();
                    ConfigWindowMP3e2.Focus();
                    ConfigWindowMP3e2.FormClosed += new FormClosedEventHandler(ConfigWindowMP3e2Closed);
                }
                return;
            }


            if (m_parent.DJConsoleObj.SelectedConsole == 1)
            {
                if (ConfigWindowMK2 == null)
                {
                    ConfigWindowMK2 = new DJConsoleMK2Config(m_parent);
                    ConfigWindowMK2.Show();
                    ConfigWindowMK2.Focus();
                    ConfigWindowMK2.FormClosed += new FormClosedEventHandler(ConfigWindowMK2Closed);
                }
                return;
            }

            if (m_parent.DJConsoleObj.SelectedConsole == 2)
            {
                if (ConfigWindowMP3LE == null)
                {
                    ConfigWindowMP3LE = new DJConsoleMP3LEConfig(m_parent);
                    ConfigWindowMP3LE.Show();
                    ConfigWindowMP3LE.Focus();
                    ConfigWindowMP3LE.FormClosed += new FormClosedEventHandler(ConfigWindowMP3LEClosed);
                }
                return;

            }

            else
            {
                MessageBox.Show("Please select a Console", "Error");
            }


            //if ((m_parent.DJConsoleObj.SelectedConsole == 2) | (m_parent.DJConsoleObj.SelectedConsole == 3))
            //{
            //    if (ConfigWindowMK2 == null)
            //    {
            //        ConfigWindowMK2 = new DJConsoleMK2Config(m_parent);
            //        ConfigWindowMK2.Show();
            //        ConfigWindowMK2.Focus();
            //        ConfigWindowMK2.FormClosed += new FormClosedEventHandler(ConfigWindowMK2Closed);
            //    }
            //    return;
            //}

            //if ((m_parent.DJConsoleObj.SelectedConsole == 0) | (m_parent.DJConsoleObj.SelectedConsole == 1))
            //{
            //    if (ConfigWindowMP3e2 == null)
            //    {
            //        ConfigWindowMP3e2 = new DJConsoleMP3e2Config(m_parent);
            //        ConfigWindowMP3e2.Show();
            //        ConfigWindowMP3e2.Focus();
            //        ConfigWindowMP3e2.FormClosed += new FormClosedEventHandler(ConfigWindowMP3e2Closed);
            //    }
            //    return;
            //}

            //else
            //{
            //    MessageBox.Show("Please select a Console","Error");
            //}

        }

        private void ConfigWindowMK2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMK2 != null)
            {
                ConfigWindowMK2 = null;
            }
        }

        private void ConfigWindowMP3e2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3e2 != null)
            {
                ConfigWindowMP3e2 = null;
            }
        }

        private void ConfigWindowMP3LEClosed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3e2 != null)
            {
                ConfigWindowMP3e2 = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbConsoleSelect.SelectedItem != null)
            {
                m_parent.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                m_parent.DJConsoleObj.Reload();
            }
            this.Close();
        }

        private void cbConsoleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConsoleSelect.SelectedItem != null)
            {
                m_parent.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                m_parent.DJConsoleObj.Reload();
            }
        }


    }
}
