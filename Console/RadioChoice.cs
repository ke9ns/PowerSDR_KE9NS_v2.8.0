//=================================================================
// RadioChoice.cs
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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PowerSDR
{
    public partial class RadioChoice : Form
    {
        #region Variable Declaration

        private Console console;
        private bool closing = false;

        #endregion

        #region Constructor

        public RadioChoice(Console c)
        {
            InitializeComponent();

            console = c;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.VirtualMode = true;
            dataGridView1.DataSource = RadiosAvailable.RadioList;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns["AccessObj"].Visible = false;
            dataGridView1.Columns["Present"].Visible = false;
            //dataGridView1.Columns["Nickname"].

            // Add a button column. 
            DataGridViewDisableButtonColumn btnColumnUse =
                new DataGridViewDisableButtonColumn();
            btnColumnUse.HeaderText = "";
            btnColumnUse.Name = "Use";
            btnColumnUse.Text = "Use";
            btnColumnUse.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(btnColumnUse);


            // Add a "remove" button column
            DataGridViewColoredTextButtonColumn btnColumnRemove =
                new DataGridViewColoredTextButtonColumn();
            btnColumnRemove.HeaderText = "";
            btnColumnRemove.Name = "Remove";
            btnColumnRemove.Text = "X";
            btnColumnRemove.Width = 26;
            btnColumnRemove.UseColumnTextForButtonValue = true;
            btnColumnRemove.CellTemplate.Style.Font = new Font("Arial", 10.0f, FontStyle.Bold);
            btnColumnRemove.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            btnColumnRemove.CellTemplate.Style.ForeColor = Color.Red;

            dataGridView1.Columns.Add(btnColumnRemove);


            Thread t = new Thread(new ThreadStart(PollRadios));
            t.Name = "Poll Radios Thread";
            t.Priority = ThreadPriority.Normal;
            t.IsBackground = true;
            t.Start();

            timer_button_update.Enabled = true;
        }

        #endregion

        #region Misc Routines

        private void PollRadios()
        {
            bool pal_init = false;

            while (!closing)
            {
                if (!pal_init)
                    pal_init = Pal.Init();
                else
                    pal_init = RadiosAvailable.ScanPal();

                RadiosAvailable.Scan1500();
                Thread.Sleep(1000);
            }
        }

        private void UpdateButtons()
        {
            if (dataGridView1 == null || dataGridView1.IsDisposed) return;

            bool changed = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewDisableButtonCell buttonCell =
                    (DataGridViewDisableButtonCell)row.Cells["Use"];

                bool old_val = buttonCell.Enabled;
                buttonCell.Enabled = (bool)row.Cells["Present"].Value;
                if (old_val != buttonCell.Enabled)
                    changed = true;
            }

            if (changed)
                dataGridView1.Invalidate();
        }

        #endregion

        #region Event Handlers

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Radio r = RadiosAvailable.RadioList[e.RowIndex];
            //DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (e.ColumnIndex == dataGridView1.Columns["Use"].Index) // Use clicked
            {
                if (r.Present)
                {
                    //MessageBox.Show("Use " + r.Model.ToString() + " " + r.SerialNumber);

                    switch (r.Model)
                    {
                        case Model.FLEX5000:
                        case Model.FLEX3000:
                            Pal.SelectDevice((uint)r.AccessObj);
                            break;
                        case Model.FLEX1500:
                            Flex1500.SetActiveRadio((IntPtr)r.AccessObj);
                            break;
                        case Model.SDR1000:
                        case Model.SOFTROCK40:
                        case Model.DEMO:
                            break;
                    }

                    console.DBFileName = console.AppDataPath + r.GetDBFilename1(); // ke9ns mod 
                    console.RadioToUse = r;

                    this.Close();
                }
            }

            if (e.ColumnIndex == dataGridView1.Columns["Remove"].Index) // Remove clicked
            {
                if (r.Model == Model.DEMO) return;

                DialogResult dr = MessageBox.Show("Are you sure you want to remove " + r.Model.ToString() + ": " + r.SerialNumber + " from the list?",
                    "Remove radio?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.No) return;

                Master.RemoveRadio(r);
                RadiosAvailable.RadioList.Remove(r);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridView1.Columns["Present"].Index)
            {
                DataGridViewDisableButtonCell buttonCell =
                    (DataGridViewDisableButtonCell)dataGridView1.
                    Rows[e.RowIndex].Cells["Use"];

                buttonCell.Enabled =
                    (bool)dataGridView1.Rows[e.RowIndex].Cells["Present"].Value;

                dataGridView1.Invalidate();
            }
        }

        private void timer_button_update_Tick(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void RadioChoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_button_update.Enabled = false;
            closing = true;

            // add any new radios found to the master list
            foreach (Radio r in RadiosAvailable.RadioList)
                Master.AddRadio(r);

            Master.Commit();

            // handle when user closes form without making a selection
            if (console.RadioToUse == null)
            {
                this.Hide();
                Thread.Sleep(50);
                Process.GetCurrentProcess().Kill(); // program ends here
                return; // this is here only for clarity
            }
        }

        private void btnAddLegacy_Click(object sender, EventArgs e)
        {
            LegacyForm legacy = new LegacyForm();
            legacy.ShowDialog();
        }

        #endregion
    }

    #region Supporting Classes

    #region DataGridViewColoredTextButtonColumn

    public class DataGridViewColoredTextButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewColoredTextButtonColumn()
        {
            this.CellTemplate = new DataGridViewColoredTextButtonCell();
        }
    }

    public class DataGridViewColoredTextButtonCell : DataGridViewButtonCell
    {
        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewColoredTextButtonCell cell =
                (DataGridViewColoredTextButtonCell)base.Clone();
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewColoredTextButtonCell()
        {

        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                elementState, value, formattedValue, errorText,
                cellStyle, advancedBorderStyle, paintParts);

            if (this.RowIndex < 0) return;

            // Calculate the area in which to draw the button.
            Rectangle buttonArea = cellBounds;
            Rectangle buttonAdjustment =
                this.BorderWidths(advancedBorderStyle);
            buttonArea.X += buttonAdjustment.X;
            buttonArea.Y += buttonAdjustment.Y;
            buttonArea.Height -= buttonAdjustment.Height;
            buttonArea.Width -= buttonAdjustment.Width;

            // Draw the disabled button text. 
            if (this.FormattedValue is String)
            {
                TextRenderer.DrawText(graphics,
                    (string)this.FormattedValue,
                    this.Style.Font,
                    buttonArea, this.Style.ForeColor);
            }
        }
    }

    #endregion

    #region DataGridViewDisableButtonColumn

    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,  
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.                
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text. 
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class 
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }

    #endregion

    #endregion
}
