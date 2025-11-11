using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static PelliP3.SongUtils;

namespace PelliP3
{
    public partial class SongQueryResult : Form
    {
        public Song SelectedSong { get; private set; }

        public SongQueryResult(Song[] results)
        {
            InitializeComponent();
            InitializeListView();

            foreach (var s in results)
            {
                var item = new ListViewItem(new[]
                {
                    s.Name,
                    s.Band,
                    s.Album,
                    s.Year > 0 ? s.Year.ToString() : "—",
                    s.Duration != TimeSpan.Zero ? s.Duration.ToString(@"m\:ss") : "—"
                });
                item.Tag = s;
                resultList.Items.Add(item);
            }
        }

        private ListView resultList;
        private Button okButton;
        private Button cancelButton;

        private void InitializeListView()
        {
            this.Text = "Select Song Metadata";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            resultList = new ListView
            {
                Dock = DockStyle.Top,
                Height = 300,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false
            };

            resultList.Columns.Add("Title", 180);
            resultList.Columns.Add("Artist", 150);
            resultList.Columns.Add("Album", 150);
            resultList.Columns.Add("Year", 60);
            resultList.Columns.Add("Duration", 80);

            resultList.DoubleClick += (s, e) => SelectAndClose();

            okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Width = 90,
                Height = 30,
                Left = this.ClientSize.Width - 200,
                Top = 320
            };
            okButton.Click += (s, e) => SelectAndClose();

            cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Width = 90,
                Height = 30,
                Left = this.ClientSize.Width - 100,
                Top = 320
            };

            this.Controls.Add(resultList);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void SelectAndClose()
        {
            if (resultList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a song from the list.");
                return;
            }

            SelectedSong = (Song)resultList.SelectedItems[0].Tag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
