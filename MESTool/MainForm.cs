using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static MESTool.MesToolProcessor;

namespace MESTool
{
    public partial class MainForm : Form
    {
        private string _currentMesPath;
        private string _dumpFolderPath;
        private string _currentlySelectedFilePath;
        private string _resourcesPath;

        private TextBox textBoxContent;
        private TextureViewer textureViewer;
        private MesToolProcessor _mesProcessor;

        public MainForm()
        {
            InitializeComponent();
            InitializeDynamicControls();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _resourcesPath = Path.Combine(exeDirectory, "Resources");

                _mesProcessor = new MesToolProcessor();
                _mesProcessor.Plat = Platform.PC;

                PopulateCharTableComboBox();
                PopulateCharGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical initialization error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void InitializeDynamicControls()
        {
            textBoxContent = new TextBox
            {
                Name = "textBoxContent",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9.75f),
                Visible = true
            };
            textBoxContent.Leave += TextBoxContent_Leave;

            textureViewer = new TextureViewer
            {
                Name = "textureViewer",
                Dock = DockStyle.Fill,
                Visible = false
            };

            textureViewer.TextureModifiedChanged += TextureViewer_TextureModifiedChanged;

            this.splitContainer1.Panel2.Controls.Add(textBoxContent);
            this.splitContainer1.Panel2.Controls.Add(textureViewer);
        }

        private void MenuFileOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "MES files (*.mes)|*.mes" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _currentMesPath = ofd.FileName;
                    string sourceDirectory = Path.GetDirectoryName(_currentMesPath);
                    string unpackedFolderName = Path.GetFileNameWithoutExtension(_currentMesPath);

                    _dumpFolderPath = Path.Combine(sourceDirectory, unpackedFolderName);

                    try
                    {
                        _mesProcessor.Dump(_currentMesPath, _dumpFolderPath);
                        LoadFileList(_dumpFolderPath);
                        MessageBox.Show($"File unpacked successfully into the '{_dumpFolderPath}' folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error while unpacking the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PlatformMenuItem_Click(object sender, EventArgs e)
        {

            if (sender is ToolStripMenuItem clickedItem)
            {
                Platform selectedPlatform;
                if (clickedItem == pCToolStripMenuItem)
                    selectedPlatform = Platform.PC;
                else if (clickedItem == pS3ToolStripMenuItem)
                    selectedPlatform = Platform.PS3;
                else if (clickedItem == x360ToolStripMenuItem)
                    selectedPlatform = Platform.X360;
                else if (clickedItem == wiiUToolStripMenuItem)
                    selectedPlatform = Platform.WiiU;
                else
                    return;

                SetPlatform(selectedPlatform);
            }
        }

        private void SaveTexture()
        {
            Bitmap? modifiedTexture = textureViewer.GetModifiedTexture();
            if (modifiedTexture == null)
            {
                MessageBox.Show("No texture is currently loaded to be saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "DirectDraw Surface (*.dds)|*.dds";
                sfd.Title = "Save Texture";
                sfd.FileName = "Texture_modified.dds";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DDSUtil.SaveTextureAsDds(modifiedTexture, sfd.FileName);
                        MessageBox.Show("Texture saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save texture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SetPlatform(Platform platform)
        {

            if (_mesProcessor != null)
            {
                _mesProcessor.Plat = platform;
            }

            pCToolStripMenuItem.Checked = false;
            pS3ToolStripMenuItem.Checked = false;
            x360ToolStripMenuItem.Checked = false;
            wiiUToolStripMenuItem.Checked = false;

            switch (platform)
            {
                case Platform.PC:
                    pCToolStripMenuItem.Checked = true;
                    break;
                case Platform.PS3:
                    pS3ToolStripMenuItem.Checked = true;
                    break;
                case Platform.X360:
                    x360ToolStripMenuItem.Checked = true;
                    break;
                case Platform.WiiU:
                    wiiUToolStripMenuItem.Checked = true;
                    break;
            }
        }

        private void LoadFileList(string folderPath)
        {
            listBoxEntries.Items.Clear();
            textBoxContent.Clear();
            _currentlySelectedFilePath = null;
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                listBoxEntries.Items.Add(Path.GetFileName(file));
            }
        }

        private void ListBoxEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEntries.SelectedItem == null)
            {
                return;
            }

            if (textureViewer.Visible && textureViewer.IsTextureModified())
            {
                var result = MessageBox.Show("You have unsaved changes in the texture viewer. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveTexture();
                    SaveCharsetTabe(textureViewer.GetCharTable());

                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            SaveChangesToCurrentFile();

            string selectedFileName = listBoxEntries.SelectedItem.ToString();
            _currentlySelectedFilePath = Path.Combine(_dumpFolderPath, selectedFileName);
            string extension = Path.GetExtension(selectedFileName).ToLower();

            if (extension == ".dds" || extension == ".wtb" || extension == ".png" || extension == ".jpg")
            {
                SwitchToView(textureViewer);
                try
                {
                    Bitmap texture = DDSUtil.LoadTexture(_currentlySelectedFilePath);
                    TextureMapInfo? textureMapInfo = _mesProcessor.LoadTextureMapInfo(_dumpFolderPath);
                    textureViewer.SetCharTable(_mesProcessor.GetCharTableForDisplay());
                    textureViewer.SetTexture(texture, textureMapInfo!.UVTable.Entries);

                    textureViewer.ShowUV(showUVToolStripMenuItem.Checked);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    MessageBox.Show($"Could not load texture '{selectedFileName}':\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textureViewer.Clear();
                }
            }
            else
            {
                textureViewer.Clear();
                SwitchToView(textBoxContent);
                try
                {
                    textBoxContent.Text = File.ReadAllText(_currentlySelectedFilePath, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not read file '{selectedFileName}':\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxContent.Text = "";
                }
            }
        }

        private void SwitchToView(Control viewToShow)
        {
            foreach (Control control in this.splitContainer1.Panel2.Controls)
            {
                control.Visible = false;
            }

            if (viewToShow != null)
            {
                viewToShow.Visible = true;
            }
        }

        private void TextBoxContent_Leave(object sender, EventArgs e)
        {
            SaveChangesToCurrentFile();
        }

        private void SaveChangesToCurrentFile()
        {
            if (string.IsNullOrEmpty(_currentlySelectedFilePath) || !File.Exists(_currentlySelectedFilePath))
            {
                return;
            }

            string extension = Path.GetExtension(_currentlySelectedFilePath).ToLower();
            if (extension == ".dds" || extension == ".wtb")
            {
                return;
            }

            try
            {
                string currentFileContent = File.ReadAllText(_currentlySelectedFilePath, Encoding.UTF8);
                if (currentFileContent != textBoxContent.Text)
                {
                    File.WriteAllText(_currentlySelectedFilePath, textBoxContent.Text, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save changes to {_currentlySelectedFilePath}: {ex.Message}");
            }
        }

        private void MenuFileRebuild_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_dumpFolderPath) || !Directory.Exists(_dumpFolderPath))
            {
                MessageBox.Show("First, open and unpack a .mes file.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveChangesToCurrentFile();
            SaveAndRebuildFromFolder(_dumpFolderPath, Path.GetFileName(_currentMesPath));
        }

        private void MenuFileRebuildFromFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.ValidateNames = false;
                fbd.CheckFileExists = false;
                fbd.CheckPathExists = true;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _dumpFolderPath = Path.GetDirectoryName(fbd.FileName);
                    if (!File.Exists(Path.Combine(_dumpFolderPath, "Texts.txt")))
                    {
                        MessageBox.Show("The selected folder does not contain Texts.txt. Please specify the correct folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    LoadFileList(_dumpFolderPath);
                }
            }
        }

        private void SaveAndRebuildFromFolder(string folderPath, string defaultFileName)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "MES files (*.mes)|*.mes", FileName = defaultFileName })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _mesProcessor.Create(folderPath, sfd.FileName);
                        MessageBox.Show("File rebuilt successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error while rebuilding the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveChangesToCurrentFile();
            Close();
        }

        private void showUVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showUVToolStripMenuItem.Checked = !showUVToolStripMenuItem.Checked;

            textureViewer.ShowUV(showUVToolStripMenuItem.Checked);
        }

        private void PopulateCharTableComboBox()
        {
            charTablesComboBox.Items.Clear();

            if (Directory.Exists(_resourcesPath))
            {
                var charTableFiles = Directory.GetFiles(_resourcesPath, "*.txt");

                if (charTableFiles.Length > 0)
                {
                    foreach (var filePath in charTableFiles)
                    {
                        charTablesComboBox.Items.Add(Path.GetFileName(filePath));
                    }
                    charTablesComboBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show(
                        $"No character map files (*.txt) found in the 'Resources' folder.",
                        "Warning: No CharTables",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            else
            {
                MessageBox.Show(
                   $"The 'Resources' folder was not found.\nPlease create it next to the application executable.",
                   "Error: Folder Not Found",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error
               );
            }
        }

        private void PopulateCharGridView()
        {
            var charTableData = _mesProcessor.GetCharTableForDisplay();

            var charEntries = new List<CharMapEntry>();
            foreach (var kvp in charTableData)
            {
                charEntries.Add(new CharMapEntry
                {
                    SortKey = kvp.Key,
                    DecCode = kvp.Key.ToString(),
                    HexCode = $"{kvp.Key:X4}",
                    Character = kvp.Value.Replace("\n", "\\n").Replace("\r", "")
                });
            }

            charDataGridView.DataSource = null;
            charDataGridView.Columns.Clear();
            charDataGridView.AutoGenerateColumns = false;
            charDataGridView.RowHeadersVisible = false;

            charDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SortKeyColumn",
                DataPropertyName = "SortKey",
                Visible = false
            });

            charDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DecCodeColumn",
                DataPropertyName = "DecCode",
                HeaderText = "DEC",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } // Вирівнювання
            });

            charDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HexCodeColumn",
                DataPropertyName = "HexCode",
                HeaderText = "HEX",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            charDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CharacterColumn",
                DataPropertyName = "Character",
                HeaderText = "Character",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            charDataGridView.DataSource = new BindingList<CharMapEntry>(charEntries);
        }

        private void SaveCharsetTabe(Dictionary<ushort, string> charsetTabe)
        {

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title = "Save Character Table";
                sfd.FileName = "CharTable.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileContent = DictionaryUtils.GenerateCharTableContent(charsetTabe);

                        if (string.IsNullOrEmpty(fileContent))
                        {
                            MessageBox.Show("Character map is empty. Nothing to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        File.WriteAllText(sfd.FileName, fileContent, Encoding.UTF8);

                        MessageBox.Show("Character table saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while saving the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveTextureFromWtb()
        {
            string extension = Path.GetExtension(_currentlySelectedFilePath).ToLower();
            if (extension != ".wtb")
            {
                MessageBox.Show("Please select a valid WTB file to save the texture.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] ddsData = DDSUtil.ExtractDdsFromWtb(File.ReadAllBytes(_currentlySelectedFilePath));

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "DirectDraw Surface (*.dds)|*.dds";
                sfd.Title = "Save Texture";
                sfd.FileName = "Texture.dds";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                       File.WriteAllBytes(sfd.FileName, ddsData);
                       MessageBox.Show("Texture saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save texture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveCharsetDataBtn_Click(object sender, EventArgs e)
        {

            if (charDataGridView.DataSource == null)
            {
                MessageBox.Show("There is no character map data to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title = "Save Character Table";
                sfd.FileName = "CharTable.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var charEntries = (charDataGridView.DataSource as BindingList<CharMapEntry>);
                        if (charEntries == null)
                        {
                            throw new Exception("Could not retrieve data source from the grid.");
                        }

                        var sortedEntries = charEntries.OrderBy(entry => entry.SortKey);

                        StringBuilder fileContent = new StringBuilder();
                        foreach (var entry in sortedEntries)
                        {
                            string hexCode = entry.SortKey.ToString("X");
                            string characterValue = entry.Character.Replace("\\n", "\n");

                            // Формуємо рядок "КОД=СИМВОЛ"
                            fileContent.AppendLine($"{hexCode}={characterValue}");
                        }

                        File.WriteAllText(sfd.FileName, fileContent.ToString(), Encoding.UTF8);
                        MessageBox.Show("Character table saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while saving the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"MesTool GUI by MrIkso. Based on MESTool by gdkchan\nVersion 1.0.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void charTablesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (charTablesComboBox.SelectedItem == null)
            {
                return;
            }

            string selectedFileName = charTablesComboBox.SelectedItem.ToString();
            string fullPath = Path.Combine(_resourcesPath, selectedFileName);

            ReloadCharTable(fullPath);
        }

        private void ReloadCharTable(string charTablePath)
        {
            try
            {
                if (!File.Exists(charTablePath))
                {
                    throw new FileNotFoundException($"The selected CharTable file was not found: {charTablePath}");
                }

                string charTableContent = File.ReadAllText(charTablePath, Encoding.UTF8);

                _mesProcessor.InitializeCharTable(charTableContent);

                SetPlatform(GetCurrentSelectedPlatform());
                PopulateCharGridView();

                if (textureViewer.Visible)
                {
                    textureViewer.SetCharTable(_mesProcessor.GetCharTableForDisplay());
                }

                Console.WriteLine($"Successfully reloaded CharTable from: {Path.GetFileName(charTablePath)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load character map:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Platform GetCurrentSelectedPlatform()
        {
            if (pS3ToolStripMenuItem.Checked) return Platform.PS3;
            if (x360ToolStripMenuItem.Checked) return Platform.X360;
            if (wiiUToolStripMenuItem.Checked) return Platform.WiiU;
            return Platform.PC;
        }

        private void saveTextureAsDDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTextureFromWtb();
        }

        private void exportModifiedTextureAndCharTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTexture();
            SaveCharsetTabe(textureViewer.GetCharTable());
        }
        private void TextureViewer_TextureModifiedChanged(object? sender, EventArgs e)
        {
            exportModifiedTextureAndCharTableToolStripMenuItem.Enabled = textureViewer.IsTextureModified();
        }

    }
    public class CharMapEntry
    {
        public ushort SortKey { get; set; }
        public string DecCode { get; set; }
        public string HexCode { get; set; }
        public string Character { get; set; }
    }

}