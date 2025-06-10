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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            openUnpackedFolderToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveTextureAsDDSToolStripMenuItem = new ToolStripMenuItem();
            exportModifiedTextureAndCharTableToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            platformMenuItem = new ToolStripMenuItem();
            pCToolStripMenuItem = new ToolStripMenuItem();
            pS3ToolStripMenuItem = new ToolStripMenuItem();
            x360ToolStripMenuItem = new ToolStripMenuItem();
            wiiUToolStripMenuItem = new ToolStripMenuItem();
            viewStripMenuItem = new ToolStripMenuItem();
            showUVToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            listBoxEntries = new ListBox();
            tabControl1 = new TabControl();
            contentPage = new TabPage();
            charPage = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            saveCharsetDataBtn = new Button();
            languageComboBox = new ComboBox();
            label1 = new Label();
            charDataGridView = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            label2 = new Label();
            charTablesComboBox = new ComboBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            contentPage.SuspendLayout();
            charPage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)charDataGridView).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, platformMenuItem, viewStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(882, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, openUnpackedFolderToolStripMenuItem, saveToolStripMenuItem, saveTextureAsDDSToolStripMenuItem, exportModifiedTextureAndCharTableToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(354, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += MenuFileOpen_Click;
            // 
            // openUnpackedFolderToolStripMenuItem
            // 
            openUnpackedFolderToolStripMenuItem.Name = "openUnpackedFolderToolStripMenuItem";
            openUnpackedFolderToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.G;
            openUnpackedFolderToolStripMenuItem.Size = new Size(354, 26);
            openUnpackedFolderToolStripMenuItem.Text = "Open unpacked folder";
            openUnpackedFolderToolStripMenuItem.Click += MenuFileRebuildFromFolder_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(354, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += MenuFileRebuild_Click;
            // 
            // saveTextureAsDDSToolStripMenuItem
            // 
            saveTextureAsDDSToolStripMenuItem.Name = "saveTextureAsDDSToolStripMenuItem";
            saveTextureAsDDSToolStripMenuItem.Size = new Size(354, 26);
            saveTextureAsDDSToolStripMenuItem.Text = "Save Texture as DDS";
            saveTextureAsDDSToolStripMenuItem.Click += saveTextureAsDDSToolStripMenuItem_Click;
            // 
            // exportModifiedTextureAndCharTableToolStripMenuItem
            // 
            exportModifiedTextureAndCharTableToolStripMenuItem.Enabled = false;
            exportModifiedTextureAndCharTableToolStripMenuItem.Name = "exportModifiedTextureAndCharTableToolStripMenuItem";
            exportModifiedTextureAndCharTableToolStripMenuItem.Size = new Size(354, 26);
            exportModifiedTextureAndCharTableToolStripMenuItem.Text = "Export Modified Texture and Char Table";
            exportModifiedTextureAndCharTableToolStripMenuItem.Click += exportModifiedTextureAndCharTableToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(354, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // platformMenuItem
            // 
            platformMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pCToolStripMenuItem, pS3ToolStripMenuItem, x360ToolStripMenuItem, wiiUToolStripMenuItem });
            platformMenuItem.Name = "platformMenuItem";
            platformMenuItem.Size = new Size(80, 24);
            platformMenuItem.Text = "Platform";
            // 
            // pCToolStripMenuItem
            // 
            pCToolStripMenuItem.Checked = true;
            pCToolStripMenuItem.CheckState = CheckState.Checked;
            pCToolStripMenuItem.Name = "pCToolStripMenuItem";
            pCToolStripMenuItem.Size = new Size(125, 26);
            pCToolStripMenuItem.Text = "PC";
            pCToolStripMenuItem.Click += PlatformMenuItem_Click;
            // 
            // pS3ToolStripMenuItem
            // 
            pS3ToolStripMenuItem.Name = "pS3ToolStripMenuItem";
            pS3ToolStripMenuItem.Size = new Size(125, 26);
            pS3ToolStripMenuItem.Text = "PS3";
            pS3ToolStripMenuItem.Click += PlatformMenuItem_Click;
            // 
            // x360ToolStripMenuItem
            // 
            x360ToolStripMenuItem.Name = "x360ToolStripMenuItem";
            x360ToolStripMenuItem.Size = new Size(125, 26);
            x360ToolStripMenuItem.Text = "X360";
            x360ToolStripMenuItem.Click += PlatformMenuItem_Click;
            // 
            // wiiUToolStripMenuItem
            // 
            wiiUToolStripMenuItem.Name = "wiiUToolStripMenuItem";
            wiiUToolStripMenuItem.Size = new Size(125, 26);
            wiiUToolStripMenuItem.Text = "WiiU";
            wiiUToolStripMenuItem.Click += PlatformMenuItem_Click;
            // 
            // viewStripMenuItem
            // 
            viewStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showUVToolStripMenuItem });
            viewStripMenuItem.Name = "viewStripMenuItem";
            viewStripMenuItem.Size = new Size(55, 24);
            viewStripMenuItem.Text = "View";
            // 
            // showUVToolStripMenuItem
            // 
            showUVToolStripMenuItem.Name = "showUVToolStripMenuItem";
            showUVToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            showUVToolStripMenuItem.Size = new Size(204, 26);
            showUVToolStripMenuItem.Text = "Show UV";
            showUVToolStripMenuItem.Click += showUVToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 2);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBoxEntries);
            splitContainer1.Size = new Size(862, 524);
            splitContainer1.SplitterDistance = 285;
            splitContainer1.TabIndex = 1;
            // 
            // listBoxEntries
            // 
            listBoxEntries.Dock = DockStyle.Fill;
            listBoxEntries.FormattingEnabled = true;
            listBoxEntries.Location = new Point(0, 0);
            listBoxEntries.Margin = new Padding(3, 2, 3, 2);
            listBoxEntries.Name = "listBoxEntries";
            listBoxEntries.Size = new Size(285, 524);
            listBoxEntries.TabIndex = 0;
            listBoxEntries.SelectedIndexChanged += ListBoxEntries_SelectedIndexChanged;
            // 
            // tabControl1
            // 
            tableLayoutPanel2.SetColumnSpan(tabControl1, 2);
            tabControl1.Controls.Add(contentPage);
            tabControl1.Controls.Add(charPage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 38);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(876, 561);
            tabControl1.TabIndex = 2;
            // 
            // contentPage
            // 
            contentPage.Controls.Add(splitContainer1);
            contentPage.Location = new Point(4, 29);
            contentPage.Margin = new Padding(3, 2, 3, 2);
            contentPage.Name = "contentPage";
            contentPage.Padding = new Padding(3, 2, 3, 2);
            contentPage.Size = new Size(868, 528);
            contentPage.TabIndex = 0;
            contentPage.Text = "Unpaked Content";
            contentPage.UseVisualStyleBackColor = true;
            // 
            // charPage
            // 
            charPage.Controls.Add(tableLayoutPanel1);
            charPage.Location = new Point(4, 29);
            charPage.Margin = new Padding(3, 2, 3, 2);
            charPage.Name = "charPage";
            charPage.Padding = new Padding(3, 2, 3, 2);
            charPage.Size = new Size(868, 528);
            charPage.TabIndex = 1;
            charPage.Text = "Char Table";
            charPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(saveCharsetDataBtn, 2, 0);
            tableLayoutPanel1.Controls.Add(languageComboBox, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(charDataGridView, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 2);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(862, 524);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // saveCharsetDataBtn
            // 
            saveCharsetDataBtn.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            saveCharsetDataBtn.AutoSize = true;
            saveCharsetDataBtn.Location = new Point(778, 2);
            saveCharsetDataBtn.Margin = new Padding(3, 2, 3, 2);
            saveCharsetDataBtn.Name = "saveCharsetDataBtn";
            saveCharsetDataBtn.Size = new Size(81, 41);
            saveCharsetDataBtn.TabIndex = 0;
            saveCharsetDataBtn.Text = "Save";
            saveCharsetDataBtn.UseVisualStyleBackColor = true;
            saveCharsetDataBtn.Click += saveCharsetDataBtn_Click;
            // 
            // languageComboBox
            // 
            languageComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.FormattingEnabled = true;
            languageComboBox.Location = new Point(175, 8);
            languageComboBox.Margin = new Padding(3, 2, 3, 2);
            languageComboBox.Name = "languageComboBox";
            languageComboBox.Size = new Size(597, 28);
            languageComboBox.TabIndex = 1;
            languageComboBox.Visible = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(3, 12);
            label1.Name = "label1";
            label1.Size = new Size(166, 20);
            label1.TabIndex = 2;
            label1.Text = "Select Target Language:";
            label1.Visible = false;
            // 
            // charDataGridView
            // 
            charDataGridView.BorderStyle = BorderStyle.None;
            charDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(charDataGridView, 3);
            charDataGridView.Dock = DockStyle.Fill;
            charDataGridView.Location = new Point(3, 47);
            charDataGridView.Margin = new Padding(3, 2, 3, 2);
            charDataGridView.Name = "charDataGridView";
            charDataGridView.RowHeadersWidth = 51;
            charDataGridView.RowTemplate.Height = 24;
            charDataGridView.Size = new Size(856, 501);
            charDataGridView.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(tabControl1, 0, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 0);
            tableLayoutPanel2.Controls.Add(charTablesComboBox, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 28);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(882, 601);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(3, 8);
            label2.Name = "label2";
            label2.Size = new Size(310, 20);
            label2.TabIndex = 3;
            label2.Text = "Select CharTable for Decoding\\Encoding text:";
            // 
            // charTablesComboBox
            // 
            charTablesComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            charTablesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            charTablesComboBox.FormattingEnabled = true;
            charTablesComboBox.Location = new Point(319, 4);
            charTablesComboBox.Margin = new Padding(3, 4, 3, 4);
            charTablesComboBox.Name = "charTablesComboBox";
            charTablesComboBox.Size = new Size(560, 28);
            charTablesComboBox.TabIndex = 4;
            charTablesComboBox.SelectedIndexChanged += charTablesComboBox_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 629);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MES Tool GUI";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            contentPage.ResumeLayout(false);
            charPage.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)charDataGridView).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        private ToolStripMenuItem saveTextureAsDDSToolStripMenuItem;
        private ToolStripMenuItem exportModifiedTextureAndCharTableToolStripMenuItem;
    }
}