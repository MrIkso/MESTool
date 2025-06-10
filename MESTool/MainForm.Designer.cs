namespace MESTool
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openUnpackedFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.platformMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x360ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wiiUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showUVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBoxEntries = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contentPage = new System.Windows.Forms.TabPage();
            this.charPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.saveCharsetDataBtn = new System.Windows.Forms.Button();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.charDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.charTablesComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.contentPage.SuspendLayout();
            this.charPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.charDataGridView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.platformMenuItem,
            this.viewStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(882, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openUnpackedFolderToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 26);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.MenuFileOpen_Click);
            // 
            // openUnpackedFolderToolStripMenuItem
            // 
            this.openUnpackedFolderToolStripMenuItem.Name = "openUnpackedFolderToolStripMenuItem";
            this.openUnpackedFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.openUnpackedFolderToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.openUnpackedFolderToolStripMenuItem.Text = "Open unpacked folder";
            this.openUnpackedFolderToolStripMenuItem.Click += new System.EventHandler(this.MenuFileRebuildFromFolder_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.MenuFileRebuild_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // platformMenuItem
            // 
            this.platformMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pCToolStripMenuItem,
            this.pS3ToolStripMenuItem,
            this.x360ToolStripMenuItem,
            this.wiiUToolStripMenuItem});
            this.platformMenuItem.Name = "platformMenuItem";
            this.platformMenuItem.Size = new System.Drawing.Size(80, 26);
            this.platformMenuItem.Text = "Platform";
            // 
            // pCToolStripMenuItem
            // 
            this.pCToolStripMenuItem.Checked = true;
            this.pCToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pCToolStripMenuItem.Name = "pCToolStripMenuItem";
            this.pCToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.pCToolStripMenuItem.Text = "PC";
            this.pCToolStripMenuItem.Click += new System.EventHandler(this.PlatformMenuItem_Click);
            // 
            // pS3ToolStripMenuItem
            // 
            this.pS3ToolStripMenuItem.Name = "pS3ToolStripMenuItem";
            this.pS3ToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.pS3ToolStripMenuItem.Text = "PS3";
            this.pS3ToolStripMenuItem.Click += new System.EventHandler(this.PlatformMenuItem_Click);
            // 
            // x360ToolStripMenuItem
            // 
            this.x360ToolStripMenuItem.Name = "x360ToolStripMenuItem";
            this.x360ToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.x360ToolStripMenuItem.Text = "X360";
            this.x360ToolStripMenuItem.Click += new System.EventHandler(this.PlatformMenuItem_Click);
            // 
            // wiiUToolStripMenuItem
            // 
            this.wiiUToolStripMenuItem.Name = "wiiUToolStripMenuItem";
            this.wiiUToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.wiiUToolStripMenuItem.Text = "WiiU";
            this.wiiUToolStripMenuItem.Click += new System.EventHandler(this.PlatformMenuItem_Click);
            // 
            // viewStripMenuItem
            // 
            this.viewStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showUVToolStripMenuItem});
            this.viewStripMenuItem.Name = "viewStripMenuItem";
            this.viewStripMenuItem.Size = new System.Drawing.Size(55, 26);
            this.viewStripMenuItem.Text = "View";
            // 
            // showUVToolStripMenuItem
            // 
            this.showUVToolStripMenuItem.Name = "showUVToolStripMenuItem";
            this.showUVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.showUVToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.showUVToolStripMenuItem.Text = "Show UV";
            this.showUVToolStripMenuItem.Click += new System.EventHandler(this.showUVToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxEntries);
            this.splitContainer1.Size = new System.Drawing.Size(862, 397);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.TabIndex = 1;
            // 
            // listBoxEntries
            // 
            this.listBoxEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxEntries.FormattingEnabled = true;
            this.listBoxEntries.ItemHeight = 16;
            this.listBoxEntries.Location = new System.Drawing.Point(0, 0);
            this.listBoxEntries.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxEntries.Name = "listBoxEntries";
            this.listBoxEntries.Size = new System.Drawing.Size(285, 397);
            this.listBoxEntries.TabIndex = 0;
            this.listBoxEntries.SelectedIndexChanged += new System.EventHandler(this.ListBoxEntries_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.contentPage);
            this.tabControl1.Controls.Add(this.charPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 32);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(876, 439);
            this.tabControl1.TabIndex = 2;
            // 
            // contentPage
            // 
            this.contentPage.Controls.Add(this.splitContainer1);
            this.contentPage.Location = new System.Drawing.Point(4, 25);
            this.contentPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.contentPage.Name = "contentPage";
            this.contentPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.contentPage.Size = new System.Drawing.Size(868, 401);
            this.contentPage.TabIndex = 0;
            this.contentPage.Text = "Unpaked Content";
            this.contentPage.UseVisualStyleBackColor = true;
            // 
            // charPage
            // 
            this.charPage.Controls.Add(this.tableLayoutPanel1);
            this.charPage.Location = new System.Drawing.Point(4, 25);
            this.charPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.charPage.Name = "charPage";
            this.charPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.charPage.Size = new System.Drawing.Size(868, 410);
            this.charPage.TabIndex = 1;
            this.charPage.Text = "Char Table";
            this.charPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.saveCharsetDataBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.languageComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.charDataGridView, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(862, 406);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // saveCharsetDataBtn
            // 
            this.saveCharsetDataBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCharsetDataBtn.AutoSize = true;
            this.saveCharsetDataBtn.Location = new System.Drawing.Point(778, 2);
            this.saveCharsetDataBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saveCharsetDataBtn.Name = "saveCharsetDataBtn";
            this.saveCharsetDataBtn.Size = new System.Drawing.Size(81, 33);
            this.saveCharsetDataBtn.TabIndex = 0;
            this.saveCharsetDataBtn.Text = "Save";
            this.saveCharsetDataBtn.UseVisualStyleBackColor = true;
            this.saveCharsetDataBtn.Click += new System.EventHandler(this.saveCharsetDataBtn_Click);
            // 
            // languageComboBox
            // 
            this.languageComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Location = new System.Drawing.Point(164, 6);
            this.languageComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(608, 24);
            this.languageComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Target Language:";
            // 
            // charDataGridView
            // 
            this.charDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.charDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.charDataGridView, 3);
            this.charDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.charDataGridView.Location = new System.Drawing.Point(3, 39);
            this.charDataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.charDataGridView.Name = "charDataGridView";
            this.charDataGridView.RowHeadersWidth = 51;
            this.charDataGridView.RowTemplate.Height = 24;
            this.charDataGridView.Size = new System.Drawing.Size(856, 401);
            this.charDataGridView.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.charTablesComboBox, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(882, 473);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(279, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select CharTable for Decoding\\Encoding text:";
            // 
            // charTablesComboBox
            // 
            this.charTablesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.charTablesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.charTablesComboBox.FormattingEnabled = true;
            this.charTablesComboBox.Location = new System.Drawing.Point(288, 3);
            this.charTablesComboBox.Name = "charTablesComboBox";
            this.charTablesComboBox.Size = new System.Drawing.Size(591, 24);
            this.charTablesComboBox.TabIndex = 4;
            this.charTablesComboBox.SelectedIndexChanged += new System.EventHandler(this.charTablesComboBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 503);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MES Tool GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.contentPage.ResumeLayout(false);
            this.charPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.charDataGridView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openUnpackedFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxEntries;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showUVToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage contentPage;
        private System.Windows.Forms.TabPage charPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button saveCharsetDataBtn;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView charDataGridView;
        private System.Windows.Forms.ToolStripMenuItem platformMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pS3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x360ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wiiUToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox charTablesComboBox;
    }
}