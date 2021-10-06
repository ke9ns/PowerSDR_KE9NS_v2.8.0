//=================================================================
// AboutForm.cs
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PowerSDR
{
    partial class AboutForm : Form
    {
        public AboutForm()
        {
          
            InitializeComponent();
            this.Text = String.Format( "About FlexRadio Systems™ {0}™", AssemblyTitle);
            this.labelProductName.Text = String.Format( "Application: FlexRadio Systems™ {0}™", AssemblyProduct);
            this.labelVersion.Text = String.Format( "Version: {0}", AssemblyVersion);
            this.labelCopyright.Text = String.Format( "{0}", AssemblyCopyright);
            this.labelCompanyName.Text = String.Format( "{0} is a registered trademark of Bronze Bear Communications, Inc.", AssemblyCompany);
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void labelProductName_Click(object sender, EventArgs e)
        {

        }

        private void labelCopyright_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelVersion_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // System.Diagnostics.Process.Start("IExplore.exe", e.LinkText);



            var result = new StringBuilder(Environment.ExpandEnvironmentVariables("%userprofile%"));

            try
            {

                System.Diagnostics.Process.Start(e.LinkText);    // HTTP
            }
            catch
            {
                try
                {
                    var link = e.LinkText.Replace("file://%userprofile%", ""); //file
                    link = link.Replace("%20", " ");

                    result.Append(link);

                    Debug.WriteLine("link2 " + result.ToString());

                    Process.Start("explorer.exe", result.ToString());
                }
                catch
                {

                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {

            /*
                     <?xml version="1.0" encoding = "utf-8"?>
                     <powersdr>
                         <version>2.8.0.28</version>
                        <url>https://ke9ns.com/flexpage.html/</url>
                     </powersdr>
                */

            string downloadUrl = "";
            Version newVersion = null;
            string xmlUrl = "https://ke9ns.com/update.xml";
            XmlTextReader reader = null;

            try
            {
                Debug.WriteLine("HERE0");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
                // ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback((s, ce, ch, ssl) => true); // if you want to validate any ssl good or bad
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(xmlUrl);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                reader = new XmlTextReader(webResponse.GetResponseStream());
                //  reader = new XmlTextReader(xmlUrl);
                Debug.WriteLine("HERE1");

                reader.MoveToContent();

                string elementName = "";
                Debug.WriteLine("HERE2");

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "powersdr"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        downloadUrl = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                if (reader != null) reader.Close();
                MessageBox.Show("Failed to get update information. " + e1,
                 "Update Error",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);

                return;
            }
            finally
            {
                if (reader != null) reader.Close();
            }


            Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; // ke9ns this is your current installed version

            if (appVersion.CompareTo(newVersion) < 0)
            {
                DialogResult dr = MessageBox.Show(
                    "Version " + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + " of ke9ns PowerSDR is available for download, would you like to download it?",

                    "This is Your currently installed version: " + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.No) return;
                else if (dr == DialogResult.Yes)
                {

                    MessageBox.Show("OK. Make sure to make a Database backup at Setup->Export Database before you install the update.",
                    "Database Backup Request",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start("explorer.exe", downloadUrl);

                }

            }
            else
            {
                MessageBox.Show("PowerSDR ke9ns Version: " + appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision + " is up to date!",
                "No need to Update",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }

        } // okButton_Click

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    } // class about
}
