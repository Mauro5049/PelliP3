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
            this.songCoverPlayer = new System.Windows.Forms.PictureBox();
            this.songTitlePlayer = new System.Windows.Forms.Label();
            this.songArtistPlayer = new System.Windows.Forms.Label();
            this.prevSongButton = new System.Windows.Forms.Button();
            this.nextSongButton = new System.Windows.Forms.Button();
            this.pSongButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.songOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.songProgressBar = new System.Windows.Forms.ProgressBar();
            this.songQueuePanel = new System.Windows.Forms.Panel();
            this.songQueueEntry = new System.Windows.Forms.Panel();
            this.songQueueEntryCover = new System.Windows.Forms.PictureBox();
            this.songQueueEntryName = new System.Windows.Forms.Label();
            this.songQueueEntryDuration = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.songCoverPlayer)).BeginInit();
            this.songQueuePanel.SuspendLayout();
            this.songQueueEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songQueueEntryCover)).BeginInit();
            this.SuspendLayout();
            // 
            // songCoverPlayer
            // 
            this.songCoverPlayer.Image = global::PelliP3.Properties.Resources.defaultAlbumCover;
            this.songCoverPlayer.InitialImage = null;
            this.songCoverPlayer.Location = new System.Drawing.Point(504, 78);
            this.songCoverPlayer.Name = "songCoverPlayer";
            this.songCoverPlayer.Size = new System.Drawing.Size(200, 200);
            this.songCoverPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.songCoverPlayer.TabIndex = 0;
            this.songCoverPlayer.TabStop = false;
            // 
            // songTitlePlayer
            // 
            this.songTitlePlayer.AutoSize = true;
            this.songTitlePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songTitlePlayer.Location = new System.Drawing.Point(568, 281);
            this.songTitlePlayer.Name = "songTitlePlayer";
            this.songTitlePlayer.Size = new System.Drawing.Size(66, 31);
            this.songTitlePlayer.TabIndex = 1;
            this.songTitlePlayer.Text = "Title";
            // 
            // songArtistPlayer
            // 
            this.songArtistPlayer.AutoSize = true;
            this.songArtistPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songArtistPlayer.Location = new System.Drawing.Point(580, 312);
            this.songArtistPlayer.Name = "songArtistPlayer";
            this.songArtistPlayer.Size = new System.Drawing.Size(40, 17);
            this.songArtistPlayer.TabIndex = 2;
            this.songArtistPlayer.Text = "Artist";
            // 
            // prevSongButton
            // 
            this.prevSongButton.Location = new System.Drawing.Point(482, 373);
            this.prevSongButton.Name = "prevSongButton";
            this.prevSongButton.Size = new System.Drawing.Size(75, 23);
            this.prevSongButton.TabIndex = 3;
            this.prevSongButton.Text = "<";
            this.prevSongButton.UseVisualStyleBackColor = true;
            this.prevSongButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // nextSongButton
            // 
            this.nextSongButton.Location = new System.Drawing.Point(644, 373);
            this.nextSongButton.Name = "nextSongButton";
            this.nextSongButton.Size = new System.Drawing.Size(75, 23);
            this.nextSongButton.TabIndex = 4;
            this.nextSongButton.Text = ">";
            this.nextSongButton.UseVisualStyleBackColor = true;
            this.nextSongButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // pSongButton
            // 
            this.pSongButton.Location = new System.Drawing.Point(563, 373);
            this.pSongButton.Name = "pSongButton";
            this.pSongButton.Size = new System.Drawing.Size(75, 23);
            this.pSongButton.TabIndex = 5;
            this.pSongButton.Text = "|>";
            this.pSongButton.UseVisualStyleBackColor = true;
            this.pSongButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 12);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(27, 23);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "+";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
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
            this.songProgressBar.Location = new System.Drawing.Point(482, 346);
            this.songProgressBar.Name = "songProgressBar";
            this.songProgressBar.Size = new System.Drawing.Size(237, 10);
            this.songProgressBar.TabIndex = 7;
            // 
            // songQueuePanel
            // 
            this.songQueuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.songQueuePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.songQueuePanel.Controls.Add(this.songQueueEntry);
            this.songQueuePanel.Location = new System.Drawing.Point(38, 78);
            this.songQueuePanel.Name = "songQueuePanel";
            this.songQueuePanel.Size = new System.Drawing.Size(416, 318);
            this.songQueuePanel.TabIndex = 8;
            // 
            // songQueueEntry
            // 
            this.songQueueEntry.BackColor = System.Drawing.Color.Silver;
            this.songQueueEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.songQueueEntry.Controls.Add(this.songQueueEntryDuration);
            this.songQueueEntry.Controls.Add(this.songQueueEntryName);
            this.songQueueEntry.Controls.Add(this.songQueueEntryCover);
            this.songQueueEntry.Location = new System.Drawing.Point(14, 14);
            this.songQueueEntry.Name = "songQueueEntry";
            this.songQueueEntry.Size = new System.Drawing.Size(391, 27);
            this.songQueueEntry.TabIndex = 0;
            // 
            // songQueueEntryCover
            // 
            this.songQueueEntryCover.Image = global::PelliP3.Properties.Resources.defaultAlbumCover;
            this.songQueueEntryCover.Location = new System.Drawing.Point(3, 0);
            this.songQueueEntryCover.Name = "songQueueEntryCover";
            this.songQueueEntryCover.Size = new System.Drawing.Size(27, 27);
            this.songQueueEntryCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.songQueueEntryCover.TabIndex = 1;
            this.songQueueEntryCover.TabStop = false;
            // 
            // songQueueEntryName
            // 
            this.songQueueEntryName.AutoSize = true;
            this.songQueueEntryName.Location = new System.Drawing.Point(36, 7);
            this.songQueueEntryName.Name = "songQueueEntryName";
            this.songQueueEntryName.Size = new System.Drawing.Size(32, 13);
            this.songQueueEntryName.TabIndex = 2;
            this.songQueueEntryName.Text = "Song";
            // 
            // songQueueEntryDuration
            // 
            this.songQueueEntryDuration.AutoSize = true;
            this.songQueueEntryDuration.Location = new System.Drawing.Point(339, 7);
            this.songQueueEntryDuration.Name = "songQueueEntryDuration";
            this.songQueueEntryDuration.Size = new System.Drawing.Size(49, 13);
            this.songQueueEntryDuration.TabIndex = 3;
            this.songQueueEntryDuration.Text = "00:00:00";
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.songQueuePanel);
            this.Controls.Add(this.songProgressBar);
            this.Controls.Add(this.songCoverPlayer);
            this.Controls.Add(this.songTitlePlayer);
            this.Controls.Add(this.songArtistPlayer);
            this.Controls.Add(this.prevSongButton);
            this.Controls.Add(this.nextSongButton);
            this.Controls.Add(this.pSongButton);
            this.Controls.Add(this.loadButton);
            this.Name = "mainWindow";
            this.Text = "PelliP3";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.songCoverPlayer)).EndInit();
            this.songQueuePanel.ResumeLayout(false);
            this.songQueueEntry.ResumeLayout(false);
            this.songQueueEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songQueueEntryCover)).EndInit();
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
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.OpenFileDialog songOpenFileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar songProgressBar;
        private System.Windows.Forms.Panel songQueuePanel;
        private System.Windows.Forms.Panel songQueueEntry;
        private System.Windows.Forms.PictureBox songQueueEntryCover;
        private System.Windows.Forms.Label songQueueEntryName;
        private System.Windows.Forms.Label songQueueEntryDuration;
    }
}

