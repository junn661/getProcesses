using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace getProcess
{
    public partial class Form1 : Form
    {
        bool chkWhenBlank = false;
        private IEnumerable<Process> a;
        private List<int> pId = new List<int>();

        public Form1()
        {
            InitializeComponent();
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.Items.Clear();
            TopMost = checkBox2.Checked;
        }

        void tick()
        {
            if (!checkBox1.Checked) return;

            int topI = 0;
            if (listView1.TopItem != null)
            {
                topI = listView1.TopItem.Index;
            }

            if (textBox1.Text == "")
            {
                if (!chkWhenBlank) return;
                if (Process.GetProcesses().Count() == listView1.Items.Count) return;

                listView1.Items.Clear();
                pId.Clear();
                foreach (var item in Process.GetProcesses())
                {
                    listView1.Items.Add(item.ProcessName);
                    pId.Add(item.Id);
                }

                if (listView1.Items.Count != 0 && topI <= listView1.Items.Count)
                {
                    listView1.TopItem = listView1.Items[topI];
                }

                return;
            }

            a = from proc in Process.GetProcesses()
                where proc.ProcessName.Contains(textBox1.Text) && !proc.ProcessName.Contains("Idle")
                orderby proc.ProcessName, proc.StartTime
                select proc;

            if (a.Count() == listView1.Items.Count) return;

            int selecteditem = -1;
            if (listView1.SelectedItems.Count != 0)
            {
                selecteditem = listView1.SelectedItems[0].Index;
            }
            else
            {
                selecteditem = -1;
            }

            listView1.Items.Clear();
            pId.Clear();
            foreach (Process item in a)
            {
                listView1.Items.Add(item.ProcessName);
                pId.Add(item.Id);
            }

            if (listView1.Items.Count != 0 && topI <= listView1.Items.Count)
            {
                listView1.TopItem = listView1.Items[topI];
            }

            if (selecteditem != -1 && listView1.Items.Count > selecteditem && listView1.Items.Count != 0)
            {
                listView1.Items[selecteditem].Selected = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process proc;

            foreach (int item in pId)
            {
                try
                {
                    proc = Process.GetProcessById(item);
                    proc.Kill();
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "System.ComponentModel.Win32Exception" || ex.GetType().FullName == "System.ArgumentException")
                    {
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            tick();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            //textBox1.SelectAll();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            tick();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            textBox1.Text = listView1.SelectedItems[0].Text;
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.Width - 15;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            if (listView1.SelectedItems.Count == 0)
            {
                miKill.Enabled = false;
                miCopy.Enabled = false;
            }
            else
            {
                miKill.Enabled = true;
                miCopy.Enabled = true;
            }
        }

        private void miKill_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0 || listView1.SelectedItems.Count > 1) return;

            Process proc;
            try
            {
                proc = Process.GetProcessById(pId[listView1.SelectedItems[0].Index]);
            }
            catch (Exception)
            {
                MessageBox.Show("액세스가 거부되었습니다.");
                tick();
                return;
            }

            if (proc.ProcessName != listView1.SelectedItems[0].Text) return;
            proc.Kill();
            //Text = pId[listView1.SelectedItems[0].Index].ToString() + ", " + proc.ProcessName;
            tick();
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            textBox1.Text = listView1.SelectedItems[0].Text;
            tick();
        }
    }
}