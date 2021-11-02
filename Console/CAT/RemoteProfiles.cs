using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for RemoteProfiles.
    /// </summary>
    public partial class RemoteProfiles : System.Windows.Forms.Form
    {
       
        /// <summary>
        /// Required designer variable.
        /// </summary>
      
        private XmlDocument pdoc;
       
        private Console console;
        private CATParser parser;
        private string model;
        private string profile;
       
        private bool started = false;
        private bool updating = false;

        public RemoteProfiles(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();


            console = c;
            parser = new CATParser(console);
            model = console.CurrentModel.ToString().ToLower();
            if (model.StartsWith("s"))
                this.Text = "Remote Profiles for an " + model.ToUpper();
            else
                this.Text = "Remote Profiles for a " + model.ToUpper();

            //			GetProfiles();
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


        /// <summary>
        /// 
        /// </summary>
        private void GetProfiles()
        {
            pdoc = new XmlDocument();

            if (File.Exists(Application.StartupPath + "\\command.xml"))
            {
                pdoc.Load(Application.StartupPath + "\\command.xml");
                DisplayProfileNames();
                started = true;
            }
            else
            {
                MessageBox.Show("Unable to locate command.xml", "File Missing", MessageBoxButtons.OK);
                Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayProfileNames()
        {
            XPathNavigator nav = pdoc.CreateNavigator();
            XPathNodeIterator itr = nav.Select("profiles/profile[@radio='" + model.ToLower() + "']");
            if (itr.Count > 0)
            {
                cboProfiles.Items.Clear();
                while (itr.MoveNext())
                {
                    cboProfiles.Items.Add(itr.Current.GetAttribute("name", "").ToUpper());
                }
            }
            if (cboProfiles.Items.Count >= 0)
                cboProfiles.SelectedIndex = 0;

        }


        private void ReadProfile()
        {
            string ans = "";
            XPathNavigator nav = pdoc.CreateNavigator();
            XPathNodeIterator itr = nav.Select("profiles/profile[@name='" + profile + "' and @radio='" + model + "']/command");
            if (itr.Count > 0)
            {
                while (itr.MoveNext())
                {
                    ans = parser.Get(itr.Current.ToString());
                }
            }
        }

     

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void cboProfiles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            profile = cboProfiles.SelectedItem.ToString().ToLower();
            if (started && !updating)
                ReadProfile();
        }

        private void RemoteProfiles_Activated(object sender, System.EventArgs e)
        {
            updating = true;
            GetProfiles();
            updating = false;
        }

    }
}
