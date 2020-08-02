using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

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

        void main()
        {
            if (!(textBox1.Text == ""))
            {
                if (checkBox1.Checked)
                {
                    if (Process.GetProcesses().Length != listView1.Items.Count)
                    {
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

                        listView1.TopItem = listView1.Items[topI];
                        ;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            main();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                TopMost = true;
            }
            else
            {
                TopMost = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            main();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}