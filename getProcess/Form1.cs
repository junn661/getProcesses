using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace getProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.Items.Clear();
        }

        void tick()
        {
            if (textBox1.Text == "") return;
            if (checkBox1.Checked)
            {
                if (Process.GetProcesses().Length == listView1.Items.Count) return;
                int topI = 0;
                if (listView1.TopItem != null)
                {
                    topI = listView1.TopItem.Index;
                }

                listView1.Items.Clear();
                foreach (Process item in Process.GetProcesses())
                {
                    if (item.ProcessName.Contains(textBox1.Text))
                    {
                        listView1.Items.Add(item.ProcessName);
                    }
                }

                if (listView1.Items.Count != 0)
                {
                    listView1.TopItem = listView1.Items[topI];
                }
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
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            textBox1.Text = listView1.SelectedItems[0].Text;
        }
    }
}