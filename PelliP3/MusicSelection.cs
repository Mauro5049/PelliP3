using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PelliP3
{
    public partial class MusicSelection : Form
    {
        private void checkEmpty()
        {
            if (Properties.Settings.Default.folderScan == String.Empty || !Directory.Exists(Properties.Settings.Default.folderScan))
            {
                MusicSelection newWindow = new MusicSelection();
                newWindow.Show();
            }
        }

        public MusicSelection()
        {
            InitializeComponent();
            this.FormClosing += (s, e) => checkEmpty();
           
        }

        private void MusicSelection_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.folderScan = textBox1.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
