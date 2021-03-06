﻿/*
 * Kemori - An open source and community friendly manga downloader
 * Copyright (C) 2016  GGG KILLER
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            this.dlPathTextbox = new System.Windows.Forms.TextBox();
            this.dlPathButton = new System.Windows.Forms.Button();
            this.cbConnectors = new System.Windows.Forms.ComboBox();
            this.chapterSelectAll = new System.Windows.Forms.CheckBox();
            this.dlButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ssLoadProgress = new System.Windows.Forms.StatusStrip();
            this.lblLoadProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbLoadPorgress = new System.Windows.Forms.ToolStripProgressBar();
            this.updateAllBtn = new System.Windows.Forms.Button();
            this.dgvJobs = new System.Windows.Forms.DataGridView();
            this.Manga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Chapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Progress = new GUtils.UI.Components.DataGridView.DataGridViewProgressColumn();
            this.mangaList = new Kemori.UI.Components.MangaListBox(this.components);
            this.chList = new Kemori.UI.Components.ChapterListView(this.components);
            this.chNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bookmarkCB = new Kemori.UI.Components.BookmarkComboBox(this.components);
            this.btnBookmarkAction = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.ssLoadProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 13);
            label1.TabIndex = 1;
            label1.Text = "Download Path:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(13, 41);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 13);
            label2.TabIndex = 4;
            label2.Text = "Search:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(0, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(62, 13);
            label3.TabIndex = 8;
            label3.Text = "Manga List:";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(15, 286);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(80, 13);
            label5.TabIndex = 20;
            label5.Text = "Download jobs:";
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
            // dlPathButton
            // 
            this.dlPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dlPathButton.Location = new System.Drawing.Point(570, 10);
            this.dlPathButton.Name = "dlPathButton";
            this.dlPathButton.Size = new System.Drawing.Size(28, 23);
            this.dlPathButton.TabIndex = 3;
            this.dlPathButton.Text = "...";
            this.dlPathButton.UseVisualStyleBackColor = true;
            this.dlPathButton.Click += new System.EventHandler(this.DlPathButton_Click);
            // 
            // cbConnectors
            // 
            this.cbConnectors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConnectors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnectors.FormattingEnabled = true;
            this.cbConnectors.Location = new System.Drawing.Point(403, 38);
            this.cbConnectors.Name = "cbConnectors";
            this.cbConnectors.Size = new System.Drawing.Size(121, 21);
            this.cbConnectors.TabIndex = 6;
            this.cbConnectors.SelectedIndexChanged += new System.EventHandler(this.CbConnectors_SelectedIndexChanged);
            // 
            // chapterSelectAll
            // 
            this.chapterSelectAll.AutoSize = true;
            this.chapterSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.chapterSelectAll.Location = new System.Drawing.Point(0, 0);
            this.chapterSelectAll.Name = "chapterSelectAll";
            this.chapterSelectAll.Size = new System.Drawing.Size(292, 17);
            this.chapterSelectAll.TabIndex = 11;
            this.chapterSelectAll.Text = "Chapter List";
            this.chapterSelectAll.UseVisualStyleBackColor = true;
            this.chapterSelectAll.CheckedChanged += new System.EventHandler(this.ChapterSelectAll_CheckedChanged);
            // 
            // dlButton
            // 
            this.dlButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dlButton.Location = new System.Drawing.Point(19, 496);
            this.dlButton.Name = "dlButton";
            this.dlButton.Size = new System.Drawing.Size(571, 23);
            this.dlButton.TabIndex = 12;
            this.dlButton.Text = "Download";
            this.dlButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Connector:";
            // 
            // ssLoadProgress
            // 
            this.ssLoadProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLoadProgress,
            this.pbLoadPorgress});
            this.ssLoadProgress.Location = new System.Drawing.Point(0, 526);
            this.ssLoadProgress.Name = "ssLoadProgress";
            this.ssLoadProgress.Size = new System.Drawing.Size(610, 22);
            this.ssLoadProgress.TabIndex = 15;
            this.ssLoadProgress.Text = "statusStrip1";
            this.ssLoadProgress.Visible = false;
            // 
            // lblLoadProgress
            // 
            this.lblLoadProgress.Name = "lblLoadProgress";
            this.lblLoadProgress.Size = new System.Drawing.Size(56, 17);
            this.lblLoadProgress.Text = "<action>";
            // 
            // pbLoadPorgress
            // 
            this.pbLoadPorgress.Name = "pbLoadPorgress";
            this.pbLoadPorgress.Size = new System.Drawing.Size(100, 16);
            // 
            // updateAllBtn
            // 
            this.updateAllBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateAllBtn.Location = new System.Drawing.Point(530, 36);
            this.updateAllBtn.Name = "updateAllBtn";
            this.updateAllBtn.Size = new System.Drawing.Size(68, 23);
            this.updateAllBtn.TabIndex = 16;
            this.updateAllBtn.Text = "Update All";
            this.updateAllBtn.UseVisualStyleBackColor = true;
            this.updateAllBtn.Click += new System.EventHandler(this.UpdateAllBtn_ClickAsync);
            // 
            // dgvJobs
            // 
            this.dgvJobs.AllowUserToAddRows = false;
            this.dgvJobs.AllowUserToDeleteRows = false;
            this.dgvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJobs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJobs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Manga,
            this.Chapter,
            this.Progress});
            this.dgvJobs.Location = new System.Drawing.Point(18, 305);
            this.dgvJobs.Name = "dgvJobs";
            this.dgvJobs.ReadOnly = true;
            this.dgvJobs.Size = new System.Drawing.Size(569, 176);
            this.dgvJobs.TabIndex = 19;
            // 
            // Manga
            // 
            this.Manga.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Manga.HeaderText = "Manga";
            this.Manga.Name = "Manga";
            this.Manga.ReadOnly = true;
            // 
            // Chapter
            // 
            this.Chapter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Chapter.HeaderText = "Chapter";
            this.Chapter.Name = "Chapter";
            this.Chapter.ReadOnly = true;
            // 
            // Progress
            // 
            this.Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Progress.HeaderText = "Progress";
            this.Progress.Name = "Progress";
            this.Progress.ReadOnly = true;
            // 
            // mangaList
            // 
            this.mangaList.Connector = null;
            this.mangaList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mangaList.FormattingEnabled = true;
            this.mangaList.Location = new System.Drawing.Point(0, 16);
            this.mangaList.Name = "mangaList";
            this.mangaList.SelectedItem = null;
            this.mangaList.Size = new System.Drawing.Size(286, 199);
            this.mangaList.TabIndex = 21;
            // 
            // chList
            // 
            this.chList.CheckBoxes = true;
            this.chList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNameHeader});
            this.chList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.chList.LabelWrap = false;
            this.chList.Location = new System.Drawing.Point(0, 16);
            this.chList.MultiSelect = false;
            this.chList.Name = "chList";
            this.chList.Size = new System.Drawing.Size(292, 199);
            this.chList.TabIndex = 22;
            this.chList.UseCompatibleStateImageBehavior = false;
            this.chList.View = System.Windows.Forms.View.Details;
            // 
            // chNameHeader
            // 
            this.chNameHeader.Text = "Chapter";
            // 
            // bookmarkCB
            // 
            this.bookmarkCB.FormattingEnabled = true;
            this.bookmarkCB.KeystrokeInterval = 600;
            this.bookmarkCB.Location = new System.Drawing.Point(63, 38);
            this.bookmarkCB.Name = "bookmarkCB";
            this.bookmarkCB.SearchTerm = null;
            this.bookmarkCB.SelectedItem = null;
            this.bookmarkCB.Size = new System.Drawing.Size(237, 21);
            this.bookmarkCB.TabIndex = 23;
            // 
            // btnBookmarkAction
            // 
            this.btnBookmarkAction.Location = new System.Drawing.Point(307, 36);
            this.btnBookmarkAction.Name = "btnBookmarkAction";
            this.btnBookmarkAction.Size = new System.Drawing.Size(25, 23);
            this.btnBookmarkAction.TabIndex = 24;
            this.btnBookmarkAction.Text = "+";
            this.btnBookmarkAction.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(16, 66);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mangaList);
            this.splitContainer1.Panel1.Controls.Add(label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chList);
            this.splitContainer1.Panel2.Controls.Add(this.chapterSelectAll);
            this.splitContainer1.Size = new System.Drawing.Size(582, 215);
            this.splitContainer1.SplitterDistance = 286;
            this.splitContainer1.TabIndex = 25;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 548);
            this.Controls.Add(this.btnBookmarkAction);
            this.Controls.Add(this.bookmarkCB);
            this.Controls.Add(label5);
            this.Controls.Add(this.dgvJobs);
            this.Controls.Add(this.updateAllBtn);
            this.Controls.Add(this.ssLoadProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dlButton);
            this.Controls.Add(this.cbConnectors);
            this.Controls.Add(label2);
            this.Controls.Add(this.dlPathButton);
            this.Controls.Add(label1);
            this.Controls.Add(this.dlPathTextbox);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(626, 587);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Kemori";
            this.Load += new System.EventHandler(this.MainForm_LoadAsync);
            this.ssLoadProgress.ResumeLayout(false);
            this.ssLoadProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dlPathTextbox;
        private System.Windows.Forms.Button dlPathButton;
        private System.Windows.Forms.ComboBox cbConnectors;
        private System.Windows.Forms.CheckBox chapterSelectAll;
        private System.Windows.Forms.Button dlButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip ssLoadProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblLoadProgress;
        private System.Windows.Forms.ToolStripProgressBar pbLoadPorgress;
        private System.Windows.Forms.Button updateAllBtn;
        private System.Windows.Forms.DataGridView dgvJobs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Manga;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chapter;
        private GUtils.UI.Components.DataGridView.DataGridViewProgressColumn Progress;
        private UI.Components.MangaListBox mangaList;
        private UI.Components.ChapterListView chList;
        private System.Windows.Forms.ColumnHeader chNameHeader;
        private UI.Components.BookmarkComboBox bookmarkCB;
        private System.Windows.Forms.Button btnBookmarkAction;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}