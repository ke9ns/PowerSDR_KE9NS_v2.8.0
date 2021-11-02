//=================================================================
// logon.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class LogOn : System.Windows.Forms.Form
    {
        Console console;
        string data;
        int count;

        public LogOn(Console c)
        {
            InitializeComponent();
            console = c;

            StreamReader sr = File.OpenText("extended.edf");
            data = sr.ReadLine();
            sr.Close();

            count = 4;
        }

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


        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            Encryption enc = new Encryption("FlexRadio Systems", "PowerSDR");
            string text = enc.Encrypt(txtUser.Text + "/" + txtPass.Text);
            if (text == data)
            {
                MessageBox.Show("Access Granted");
                console.Extended = true;

                ArrayList a = new ArrayList();
                a.Add("extended/" + text);
                DB.SaveVars("State", ref a);

                this.Close();
                return;
            }
            else
            {
                count--;
                if (count == 0)
                {
                    MessageBox.Show("Access Denied");
                    this.Close();
                    return;
                }
                statusBar1.Text = "Invalid User/Pass.  " + count.ToString() + " tries left.";
            }

        } //  btnSubmit_Click



    } // logon

    public class Encryption
    {
        public Encryption(string k, string v)
        {
            key = k.PadRight(24, '0');
            vector = v.PadRight(8, '0');
        }

        private TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

        private string key;
        private string vector;

        private Byte[] Key
        {
            get { return Encoding.Default.GetBytes(key); }
        }

        private Byte[] Vector
        {
            get { return Encoding.Default.GetBytes(vector); }
        }

        public string Encrypt(string text)
        {
            return Transform(text, des.CreateEncryptor(Key, Vector));
        }

        public string Decrypt(string encryptedText)
        {
            return Transform(encryptedText, des.CreateDecryptor(Key, Vector));
        }

        private string Transform(string Text, ICryptoTransform CryptoTransform)
        {
            MemoryStream stream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(stream, CryptoTransform, CryptoStreamMode.Write);
            Byte[] input = Encoding.Default.GetBytes(Text);

            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.Default.GetString(stream.ToArray());
        }
    } // encryption
}
