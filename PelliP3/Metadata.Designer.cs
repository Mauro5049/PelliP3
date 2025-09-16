namespace PelliP3
{
    partial class Metadata
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.songNameEdit = new System.Windows.Forms.TextBox();
            this.artistNameEdit = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.yearAlbumEdit = new System.Windows.Forms.MaskedTextBox();
            this.songCoverEditor = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.songCoverEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // songNameEdit
            // 
            this.songNameEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.songNameEdit.Location = new System.Drawing.Point(141, 218);
            this.songNameEdit.Name = "songNameEdit";
            this.songNameEdit.Size = new System.Drawing.Size(100, 38);
            this.songNameEdit.TabIndex = 2;
            this.songNameEdit.Text = "Title";
            // 
            // artistNameEdit
            // 
            this.artistNameEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.artistNameEdit.Location = new System.Drawing.Point(141, 262);
            this.artistNameEdit.Name = "artistNameEdit";
            this.artistNameEdit.Size = new System.Drawing.Size(100, 23);
            this.artistNameEdit.TabIndex = 3;
            this.artistNameEdit.Text = "Artist";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(303, 415);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // yearAlbumEdit
            // 
            this.yearAlbumEdit.Location = new System.Drawing.Point(141, 291);
            this.yearAlbumEdit.Mask = "9999";
            this.yearAlbumEdit.Name = "yearAlbumEdit";
            this.yearAlbumEdit.Size = new System.Drawing.Size(100, 20);
            this.yearAlbumEdit.TabIndex = 5;
            this.yearAlbumEdit.ValidatingType = typeof(System.DateTime);
            // 
            // songCoverEditor
            // 
            this.songCoverEditor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.songCoverEditor.Image = global::PelliP3.Properties.Resources.defaultAlbumCover;
            this.songCoverEditor.InitialImage = null;
            this.songCoverEditor.Location = new System.Drawing.Point(90, 12);
            this.songCoverEditor.Name = "songCoverEditor";
            this.songCoverEditor.Size = new System.Drawing.Size(200, 200);
            this.songCoverEditor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.songCoverEditor.TabIndex = 1;
            this.songCoverEditor.TabStop = false;
            this.songCoverEditor.Click += new System.EventHandler(this.songCoverEditor_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "\".png\"|*.png|\"Pênis\"|.jpg";
            // 
            // Metadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 450);
            this.Controls.Add(this.yearAlbumEdit);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.artistNameEdit);
            this.Controls.Add(this.songNameEdit);
            this.Controls.Add(this.songCoverEditor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Metadata";
            this.Text = "Metadata Editor";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Metadata_Load);
            ((System.ComponentModel.ISupportInitialize)(this.songCoverEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox songCoverEditor;
        private System.Windows.Forms.TextBox songNameEdit;
        private System.Windows.Forms.TextBox artistNameEdit;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.MaskedTextBox yearAlbumEdit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}