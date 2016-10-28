namespace Kemori.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.dlPathTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dlPathButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mangaList = new System.Windows.Forms.ListBox();
            this.chapterList = new System.Windows.Forms.CheckedListBox();
            this.chapterSelectAll = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.downloadProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.downloadProgressLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.chapterLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mangaLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlPathTextbox
            // 
            this.dlPathTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dlPathTextbox.Location = new System.Drawing.Point(102, 12);
            this.dlPathTextbox.Name = "dlPathTextbox";
            this.dlPathTextbox.ReadOnly = true;
            this.dlPathTextbox.Size = new System.Drawing.Size(462, 20);
            this.dlPathTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Path:";
            // 
            // dlPathButton
            // 
            this.dlPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dlPathButton.Location = new System.Drawing.Point(570, 10);
            this.dlPathButton.Name = "dlPathButton";
            this.dlPathButton.Size = new System.Drawing.Size(28, 23);
            this.dlPathButton.TabIndex = 3;
            this.dlPathButton.Text = "...";
            this.dlPathButton.UseVisualStyleBackColor = true;
            this.dlPathButton.Click += new System.EventHandler(this.dlPathButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Search:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "MangaFox"});
            this.comboBox1.Location = new System.Drawing.Point(477, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(102, 38);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(369, 21);
            this.comboBox2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Manga List:";
            // 
            // mangaList
            // 
            this.mangaList.FormattingEnabled = true;
            this.mangaList.Location = new System.Drawing.Point(18, 83);
            this.mangaList.Name = "mangaList";
            this.mangaList.Size = new System.Drawing.Size(284, 147);
            this.mangaList.TabIndex = 9;
            // 
            // chapterList
            // 
            this.chapterList.FormattingEnabled = true;
            this.chapterList.Location = new System.Drawing.Point(308, 83);
            this.chapterList.Name = "chapterList";
            this.chapterList.Size = new System.Drawing.Size(290, 154);
            this.chapterList.TabIndex = 10;
            // 
            // chapterSelectAll
            // 
            this.chapterSelectAll.AutoSize = true;
            this.chapterSelectAll.Location = new System.Drawing.Point(308, 65);
            this.chapterSelectAll.Name = "chapterSelectAll";
            this.chapterSelectAll.Size = new System.Drawing.Size(82, 17);
            this.chapterSelectAll.TabIndex = 11;
            this.chapterSelectAll.Text = "Chapter List";
            this.chapterSelectAll.UseVisualStyleBackColor = true;
            this.chapterSelectAll.CheckedChanged += new System.EventHandler(this.chapterSelectAll_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadProgressBar,
            this.toolStripSeparator1,
            this.downloadProgressLabel,
            this.toolStripSeparator2,
            this.chapterLabel,
            this.toolStripSeparator3,
            this.mangaLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 716);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(610, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // downloadProgressBar
            // 
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.Size = new System.Drawing.Size(100, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // downloadProgressLabel
            // 
            this.downloadProgressLabel.Name = "downloadProgressLabel";
            this.downloadProgressLabel.Size = new System.Drawing.Size(52, 22);
            this.downloadProgressLabel.Text = "Progress";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // chapterLabel
            // 
            this.chapterLabel.Name = "chapterLabel";
            this.chapterLabel.Size = new System.Drawing.Size(49, 22);
            this.chapterLabel.Text = "Chapter";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // mangaLabel
            // 
            this.mangaLabel.Name = "mangaLabel";
            this.mangaLabel.Size = new System.Drawing.Size(44, 22);
            this.mangaLabel.Text = "Manga";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 741);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.chapterSelectAll);
            this.Controls.Add(this.chapterList);
            this.Controls.Add(this.mangaList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dlPathButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dlPathTextbox);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dlPathTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button dlPathButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox mangaList;
        private System.Windows.Forms.CheckedListBox chapterList;
        private System.Windows.Forms.CheckBox chapterSelectAll;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripProgressBar downloadProgressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel downloadProgressLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel chapterLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel mangaLabel;
    }
}