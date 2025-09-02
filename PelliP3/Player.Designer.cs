using System.Diagnostics;

namespace PelliP3
{
    partial class mainWindow
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.songTitlePlayer = new System.Windows.Forms.Label();
            this.songArtistPlayer = new System.Windows.Forms.Label();
            this.prevSongButton = new System.Windows.Forms.Button();
            this.nextSongButton = new System.Windows.Forms.Button();
            this.pSongButton = new System.Windows.Forms.Button();
            this.songOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.songProgressBar = new System.Windows.Forms.ProgressBar();
            this.songQueuePanel = new System.Windows.Forms.Panel();
            this.currentTimeLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songCoverPlayer = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songCoverPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // songTitlePlayer
            // 
            this.songTitlePlayer.AutoSize = true;
            this.songTitlePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songTitlePlayer.Location = new System.Drawing.Point(542, 230);
            this.songTitlePlayer.Name = "songTitlePlayer";
            this.songTitlePlayer.Size = new System.Drawing.Size(66, 31);
            this.songTitlePlayer.TabIndex = 1;
            this.songTitlePlayer.Text = "Title";
            // 
            // songArtistPlayer
            // 
            this.songArtistPlayer.AutoSize = true;
            this.songArtistPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songArtistPlayer.Location = new System.Drawing.Point(554, 261);
            this.songArtistPlayer.Name = "songArtistPlayer";
            this.songArtistPlayer.Size = new System.Drawing.Size(40, 17);
            this.songArtistPlayer.TabIndex = 2;
            this.songArtistPlayer.Text = "Artist";
            // 
            // prevSongButton
            // 
            this.prevSongButton.Location = new System.Drawing.Point(456, 322);
            this.prevSongButton.Name = "prevSongButton";
            this.prevSongButton.Size = new System.Drawing.Size(75, 23);
            this.prevSongButton.TabIndex = 3;
            this.prevSongButton.Text = "<";
            this.prevSongButton.UseVisualStyleBackColor = true;
            this.prevSongButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // nextSongButton
            // 
            this.nextSongButton.Location = new System.Drawing.Point(618, 322);
            this.nextSongButton.Name = "nextSongButton";
            this.nextSongButton.Size = new System.Drawing.Size(75, 23);
            this.nextSongButton.TabIndex = 4;
            this.nextSongButton.Text = ">";
            this.nextSongButton.UseVisualStyleBackColor = true;
            this.nextSongButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // pSongButton
            // 
            this.pSongButton.Location = new System.Drawing.Point(537, 322);
            this.pSongButton.Name = "pSongButton";
            this.pSongButton.Size = new System.Drawing.Size(75, 23);
            this.pSongButton.TabIndex = 5;
            this.pSongButton.Text = "|>";
            this.pSongButton.UseVisualStyleBackColor = true;
            this.pSongButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // songOpenFileDialog
            // 
            this.songOpenFileDialog.Filter = ".flac, .mp3, .ogg, .wav|*.flac;*.mp3;*.ogg;*.wav";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // songProgressBar
            // 
            this.songProgressBar.Location = new System.Drawing.Point(478, 284);
            this.songProgressBar.Name = "songProgressBar";
            this.songProgressBar.Size = new System.Drawing.Size(200, 10);
            this.songProgressBar.TabIndex = 7;
            // 
            // songQueuePanel
            // 
            this.songQueuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.songQueuePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.songQueuePanel.Location = new System.Drawing.Point(12, 27);
            this.songQueuePanel.Name = "songQueuePanel";
            this.songQueuePanel.Size = new System.Drawing.Size(416, 318);
            this.songQueuePanel.TabIndex = 8;
            // 
            // currentTimeLabel
            // 
            this.currentTimeLabel.AutoSize = true;
            this.currentTimeLabel.Location = new System.Drawing.Point(554, 301);
            this.currentTimeLabel.Name = "currentTimeLabel";
            this.currentTimeLabel.Size = new System.Drawing.Size(49, 13);
            this.currentTimeLabel.TabIndex = 9;
            this.currentTimeLabel.Text = "00:00:00";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.playlistToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(718, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // playlistToolStripMenuItem
            // 
            this.playlistToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
            this.playlistToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.playlistToolStripMenuItem.Text = "Playlist";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            // 
            // songCoverPlayer
            // 
            this.songCoverPlayer.Image = global::PelliP3.Properties.Resources.defaultAlbumCover;
            this.songCoverPlayer.InitialImage = null;
            this.songCoverPlayer.Location = new System.Drawing.Point(478, 27);
            this.songCoverPlayer.Name = "songCoverPlayer";
            this.songCoverPlayer.Size = new System.Drawing.Size(200, 200);
            this.songCoverPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.songCoverPlayer.TabIndex = 0;
            this.songCoverPlayer.TabStop = false;
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 373);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.songQueuePanel);
            this.Controls.Add(this.songProgressBar);
            this.Controls.Add(this.songCoverPlayer);
            this.Controls.Add(this.songTitlePlayer);
            this.Controls.Add(this.songArtistPlayer);
            this.Controls.Add(this.prevSongButton);
            this.Controls.Add(this.nextSongButton);
            this.Controls.Add(this.pSongButton);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainWindow";
            this.Text = "PelliP3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songCoverPlayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox songCoverPlayer;
        private System.Windows.Forms.Label songTitlePlayer;
        private System.Windows.Forms.Label songArtistPlayer;
        private System.Windows.Forms.Button prevSongButton;
        private System.Windows.Forms.Button nextSongButton;
        private System.Windows.Forms.Button pSongButton;
        private System.Windows.Forms.OpenFileDialog songOpenFileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar songProgressBar;
        private System.Windows.Forms.Panel songQueuePanel;
        private System.Windows.Forms.Label currentTimeLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
    }
}

