﻿using System;
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
            TopMost = checkBox2.Checked;
        }

        void tick()
        {
            if (textBox1.Text == "") return;
            if (!checkBox1.Checked) return;
            if (Process.GetProcesses().Length == listView1.Items.Count) return;

            int topI = 0;
            if (listView1.TopItem != null)
            {
                topI = listView1.TopItem.Index;
            }

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
            foreach (Process item in Process.GetProcesses())
            {
                if (item.ProcessName.Contains(textBox1.Text))
                {
                    listView1.Items.Add(item.ProcessName);
                }
            }

            if (listView1.Items.Count != 0 && topI <= listView1.Items.Count)
            {
                listView1.TopItem = listView1.Items[topI];
            }

            if (selecteditem != -1 && listView1.Items.Count >= selecteditem)
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
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            textBox1.Text = listView1.SelectedItems[0].Text;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }
    }
}