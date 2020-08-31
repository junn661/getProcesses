using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace getProcess
{
    public partial class Form1 : Form
    {
        bool chkWhenBlank = false;
        public Form1()
        {
            InitializeComponent();
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.Items.Clear();
            TopMost = checkBox2.Checked;
        }

        private System.Collections.Generic.IEnumerable<string> a;
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
                foreach (var item in Process.GetProcesses())
                {
                    listView1.Items.Add(item.ProcessName);
                }

                if (listView1.Items.Count != 0 && topI <= listView1.Items.Count)
                {
                    listView1.TopItem = listView1.Items[topI];
                }

                return;
            }

            a = from proc in Process.GetProcesses()
                where proc.ProcessName.Contains(textBox1.Text)
                select proc.ProcessName;

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
            foreach (string item in a)
            {
                listView1.Items.Add(item);
            }

            if (listView1.Items.Count != 0 && topI <= listView1.Items.Count)
            {
                listView1.TopItem = listView1.Items[topI];
            }

            if (selecteditem != -1 && listView1.Items.Count >= selecteditem && listView1.Items.Count != 0)
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
            foreach (Process item in Process.GetProcessesByName(textBox1.Text))
            {
                item.Kill();
            }
            tick();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
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
    }
}