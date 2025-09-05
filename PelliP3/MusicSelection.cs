using System;
using System.IO;
using System.Windows.Forms;

namespace PelliP3
{
    public partial class MusicSelection : Form
    {
        public MusicSelection()
        {
            InitializeComponent();
            this.FormClosing += MusicSelection_FormClosing;
        }

        private void MusicSelection_Load(object sender, EventArgs e)
        {
            var existingPath = Properties.Settings.Default.folderScan;
            if (!string.IsNullOrEmpty(existingPath))
            {
                textBox1.Text = existingPath;
            }
        }

        private void MusicSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.folderScan) ||
                !Directory.Exists(Properties.Settings.Default.folderScan))
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    MessageBox.Show("Please select a valid music folder.", "Folder Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folderPath = textBox1.Text?.Trim();

            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please enter a folder path.", "Invalid Path",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("The specified folder does not exist.", "Invalid Path",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Properties.Settings.Default.folderScan = folderPath;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && Directory.Exists(textBox1.Text))
            {
                folderBrowserDialog1.SelectedPath = textBox1.Text;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}