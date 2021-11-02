using System.Data;
using System.Windows.Forms;

namespace PowerSDR
{
   
    public partial class CATTester : System.Windows.Forms.Form
    {

      
        private Console console;
        private CATParser parser;
       
        private DataSet ds;
       
     
        public CATTester(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            parser = new CATParser(console);
            ds = new DataSet();
            Setup();
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

        private void Setup()
        {
            ds.ReadXml(Application.StartupPath + "\\CATStructs.xml");
            dataGrid1.DataSource = ds;
            txtInput.Focus();
        }

    


        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void txtInput_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckText();
            }
        }

        private void ExecuteCommand()
        {
            string answer = parser.Get(txtInput.Text);
            txtResult.Text = answer;
            txtInput.Clear();

        }

        private void btnExecute_Click(object sender, System.EventArgs e)
        {
            CheckText();
        }

        private void CheckText()
        {
            if (!txtInput.Text.EndsWith(";")) txtInput.Text += ";";
            ExecuteCommand();
        }



    }
}
