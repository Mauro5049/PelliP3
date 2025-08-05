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
            ((System.ComponentModel.ISupportInitialize)(this.songCoverPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // songCoverPlayer
            // 
            this.songCoverPlayer.Location = new System.Drawing.Point(504, 78);
            this.songCoverPlayer.Name = "songCoverPlayer";
            this.songCoverPlayer.Size = new System.Drawing.Size(200, 200);
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
            this.prevSongButton.Location = new System.Drawing.Point(482, 332);
            this.prevSongButton.Name = "prevSongButton";
            this.prevSongButton.Size = new System.Drawing.Size(75, 23);
            this.prevSongButton.TabIndex = 3;
            this.prevSongButton.Text = "<";
            this.prevSongButton.UseVisualStyleBackColor = true;
            this.prevSongButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // nextSongButton
            // 
            this.nextSongButton.Location = new System.Drawing.Point(644, 332);
            this.nextSongButton.Name = "nextSongButton";
            this.nextSongButton.Size = new System.Drawing.Size(75, 23);
            this.nextSongButton.TabIndex = 4;
            this.nextSongButton.Text = ">";
            this.nextSongButton.UseVisualStyleBackColor = true;
            this.nextSongButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // pSongButton
            // 
            this.pSongButton.Location = new System.Drawing.Point(563, 332);
            this.pSongButton.Name = "pSongButton";
            this.pSongButton.Size = new System.Drawing.Size(75, 23);
            this.pSongButton.TabIndex = 5;
            this.pSongButton.Text = "P";
            this.pSongButton.UseVisualStyleBackColor = true;
            this.pSongButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(91, 200);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(295, 23);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "deixe um like se você é um fodinha!!!";
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
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

