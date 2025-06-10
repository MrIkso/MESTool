using System.Drawing.Drawing2D;
using static MESTool.MesToolProcessor;

namespace MESTool
{
    public partial class TextureViewer : UserControl
    {
        private Image _texture;
        private List<TextureMapUV> _uvEntries;
        private float _zoom = 1.0f;
        private bool showUv = true;

        private SplitContainer _splitContainer;
        private ListView _uvListView;
        private Button _replaceCharButton;
        private ToolTip _toolTip;
        private ushort? _currentToolTipId = null;
        private Brush _checkerboardBrush;
        private TextureMapUV _selectedUvEntry = null;

        private bool _isLayoutInitialized = false;
        private Dictionary<ushort, string> _charTable;
        private bool _isTextureModified = false;

        public Action<object?, EventArgs> TextureModifiedChanged { get; internal set; }

        public TextureViewer()
        {
            InitializeComponent();
            InitializeCustomLayout();

            this.DoubleBuffered = true;

            _toolTip = new ToolTip();
            _toolTip.AutoPopDelay = 5000;
            _toolTip.InitialDelay = 500;
            _toolTip.ReshowDelay = 100;
            _toolTip.ShowAlways = true;

            CreateCheckerboardBrush();
        }

        private void InitializeComponent()
        {

        }

        private void InitializeCustomLayout()
        {
            this.SuspendLayout();

            _splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel2,
            };

            ControlExtensions.SetDoubleBuffered(_splitContainer.Panel1, true);
            _splitContainer.Panel1.AutoScroll = true;
            _splitContainer.Panel1.Paint += Panel1_Paint;
            _splitContainer.Panel1.MouseClick += Panel1_MouseClick;
            _splitContainer.Panel1.MouseMove += Panel1_MouseMove;
            _splitContainer.Panel1.MouseLeave += Panel1_MouseLeave;
            _splitContainer.Panel1.MouseWheel += Panel1_MouseWheel;

            var tableLayoutPanel = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _replaceCharButton = new Button
            {
                Text = "Replace Selected Char...",
                Dock = DockStyle.Top,
                Enabled = false,
                AutoSize = true,

                Margin = new Padding(3, 3, 3, 3)
            };
            _replaceCharButton.Click += ReplaceCharButton_Click;

            _uvListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };
            _uvListView.Columns.Add("ID", 50);
            _uvListView.Columns.Add("Hex", 65);
            _uvListView.Columns.Add("Char", 50);
            _uvListView.SelectedIndexChanged += UvListView_SelectedIndexChanged;

            tableLayoutPanel.Controls.Add(_replaceCharButton, 0, 0); // Кнопка в першому рядку (індекс 0)
            tableLayoutPanel.Controls.Add(_uvListView, 0, 1);       // Список у другому рядку (індекс 1)

            _splitContainer.Panel2.Controls.Add(tableLayoutPanel);

            this.Controls.Add(_splitContainer);
            this.ResumeLayout(false);

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!_isLayoutInitialized && this.Width > 0 && _splitContainer != null)
            {
                if (this.Width > 260)
                {
                    _splitContainer.SplitterDistance = this.Width - 250;
                }
                _isLayoutInitialized = true;
            }
        }

        private void CreateCheckerboardBrush()
        {
            int cellSize = 8;
            Bitmap checkerboard = new Bitmap(cellSize * 2, cellSize * 2);
            using (Graphics g = Graphics.FromImage(checkerboard))
            {
                g.FillRectangle(Brushes.LightGray, 0, 0, cellSize, cellSize);
                g.FillRectangle(Brushes.White, cellSize, 0, cellSize, cellSize);
                g.FillRectangle(Brushes.White, 0, cellSize, cellSize, cellSize);
                g.FillRectangle(Brushes.LightGray, cellSize, cellSize, cellSize, cellSize);
            }
            _checkerboardBrush = new TextureBrush(checkerboard);
        }

        public void SetTexture(Image texture, List<TextureMapUV> uvEntries)
        {
            if (_texture != null)
            {
                _texture.Dispose();
            }
            _texture = texture;
            _uvEntries = uvEntries;
            _selectedUvEntry = null;

            PopulateUvListView();

            _splitContainer.Panel1.Invalidate();
        }

        public void SetCharTable(Dictionary<ushort, string> charTable)
        {
            _charTable = charTable;
            if (_uvListView.Items.Count > 0)
            {
                PopulateUvListView();
            }
        }

        public void ShowUV(bool show)
        {
            this.showUv = show;
            _splitContainer.Panel2Collapsed = !show;
            if (!show)
            {
                _toolTip.Hide(_splitContainer.Panel1);
                _currentToolTipId = null;
                _selectedUvEntry = null;
            }
            _splitContainer.Panel1.Invalidate();
        }

        public void Clear()
        {
            if (_texture != null)
            {
                _texture.Dispose();
                _texture = null;
            }
            _uvEntries = null;

            _selectedUvEntry = null;
            _splitContainer.Panel1.Invalidate();
            this.Invalidate();
        }

        public void ZoomIn()
        {
            _zoom *= 1.2f;
            _splitContainer.Panel1.Invalidate();
        }

        public void ZoomOut()
        {
            _zoom /= 1.2f;
            if (_zoom < 0.1f) _zoom = 0.1f;
            _splitContainer.Panel1.Invalidate();
        }

        private void PopulateUvListView()
        {
            _uvListView.Items.Clear();
            if (_uvEntries == null) return;

            var listItems = _uvEntries.OrderBy(uv => uv.Id).Select(uv =>
            {
                string? character = "";
                if (_charTable != null && _charTable.TryGetValue(uv.Id, out string? foundChar))
                {

                    character = foundChar!.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
                }

                var item = new ListViewItem(uv.Id.ToString());
                item.SubItems.Add($"0x{uv.Id:X4}");
                item.SubItems.Add(character);
                item.Tag = uv;
                return item;
            }).ToArray();

            _uvListView.Items.AddRange(listItems);
        }

        private void UvListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            TootgleReplaceButtonState();
            if (_uvListView.SelectedItems.Count > 0)
            {
                _selectedUvEntry = _uvListView.SelectedItems[0].Tag as TextureMapUV;
            }
            else
            {
                _selectedUvEntry = null;
            }
            _splitContainer.Panel1.Invalidate();
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
        }

        private void Panel1_MouseLeave(object sender, EventArgs e)
        {
            _currentToolTipId = null;
            _toolTip.Hide(_splitContainer.Panel1);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!showUv || _texture == null || _uvEntries == null) return;

            TextureMapUV foundUv = FindUvAtMousePosition(e.Location);

            if (foundUv != null)
            {
                if (_currentToolTipId != foundUv.Id)
                {
                    _currentToolTipId = foundUv.Id;
                    _toolTip.SetToolTip(_splitContainer.Panel1, $"ID: {foundUv.Id} (0x{foundUv.Id:X4})");
                }
            }
            else
            {
                if (_currentToolTipId != null)
                {
                    _currentToolTipId = null;
                    _toolTip.Hide(_splitContainer.Panel1);
                }
            }
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            _currentToolTipId = null;
            _toolTip.Hide(this);
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!showUv || _texture == null || _uvEntries == null) return;

            TextureMapUV clickedUv = FindUvAtMousePosition(e.Location);

            if (clickedUv != null)
            {
                SelectUvInListView(clickedUv.Id);

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem replaceItem = new ToolStripMenuItem("Replace Character", null, (s, args) => ReplaceTextureChar());

                    contextMenu.Items.Add(replaceItem);
                    contextMenu.Show(_splitContainer.Panel1, e.Location);
                }
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_texture == null)
            {
                e.Graphics.Clear(_splitContainer.Panel1.BackColor);
                return;
            }

            if (_checkerboardBrush is TextureBrush textureBrush)
            {

                Matrix originalBrushTransform = textureBrush.Transform;

                textureBrush.TranslateTransform(
                    _splitContainer.Panel1.AutoScrollPosition.X,
                    _splitContainer.Panel1.AutoScrollPosition.Y
                );

                e.Graphics.FillRectangle(textureBrush, e.ClipRectangle);

                textureBrush.Transform = originalBrushTransform;
            }
            else
            {

                e.Graphics.Clear(_splitContainer.Panel1.BackColor);
            }

            _splitContainer.Panel1.AutoScrollMinSize = new Size(
                (int)(_texture.Width * _zoom),
                (int)(_texture.Height * _zoom)
            );
            e.Graphics.TranslateTransform(_splitContainer.Panel1.AutoScrollPosition.X, _splitContainer.Panel1.AutoScrollPosition.Y);
            e.Graphics.ScaleTransform(_zoom, _zoom);
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            e.Graphics.DrawImage(_texture, 0, 0);

            if (showUv && _uvEntries != null)
            {

                using (Pen pen = new Pen(Color.Red, 1.0f / _zoom))
                {
                    foreach (var uv in _uvEntries)
                    {
                        if (uv != _selectedUvEntry)
                        {
                            DrawUvRect(e.Graphics, uv, pen);
                        }
                    }
                }

                if (_selectedUvEntry != null)
                {
                    Color highlightFillColor = Color.FromArgb(100, SystemColors.Highlight);
                    using (Brush selectedBrush = new SolidBrush(highlightFillColor))
                    {
                        FillUvRect(e.Graphics, _selectedUvEntry, selectedBrush);
                    }
                    using (Pen selectedPen = new Pen(SystemColors.Highlight, 1.5f / _zoom))
                    {
                        DrawUvRect(e.Graphics, _selectedUvEntry, selectedPen);
                    }
                }
            }
        }

        private void ReplaceCharButton_Click(object sender, EventArgs e)
        {
            ReplaceTextureChar();
        }

        private bool ReplaceTextureChar()
        {
            if (_selectedUvEntry == null || _texture == null)
            {
                MessageBox.Show("Please select a texture from the list first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.png;*.bmp;*.jpg)|*.png;*.bmp;*.jpg|All Files (*.*)|*.*";
                ofd.Title = "Select New Texture Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image newCharImage = Image.FromFile(ofd.FileName))
                        {
                           
                            string inputChar = ShowCharInputDialog();
                           // if (inputChar == null)
                               // return false;

                            ReplaceCharacterOnTexture(newCharImage);

                            if (_charTable != null)
                            {
                                _charTable[_selectedUvEntry.Id] = inputChar;
                                PopulateUvListView();
                                SelectUvInListView(_selectedUvEntry.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to replace texture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }
        
        private string ShowCharInputDialog()
        {
            string oldChar = string.Empty;
            if (_charTable != null && _charTable.TryGetValue(_selectedUvEntry.Id, out string? oldCharU))
                oldChar = oldCharU;

            using (EnterSymbolDialog enterSymbolDialog = new EnterSymbolDialog(oldChar))
            {
                if (enterSymbolDialog.ShowDialog() == DialogResult.OK)
                {
                    return enterSymbolDialog.Symbol;
                }
            }
            return oldChar;

        }

        private void ReplaceCharacterOnTexture(Image newCharImage)
        {
            float rectX = Math.Min(_selectedUvEntry.StartX, _selectedUvEntry.EndX) * _texture.Width;
            float rectY = Math.Min(_selectedUvEntry.StartY, _selectedUvEntry.EndY) * _texture.Height;
            float rectWidth = Math.Abs(_selectedUvEntry.EndX - _selectedUvEntry.StartX) * _texture.Width;
            float rectHeight = Math.Abs(_selectedUvEntry.EndY - _selectedUvEntry.StartY) * _texture.Height;

            RectangleF targetRect = new RectangleF(rectX, rectY, rectWidth, rectHeight);

            if (targetRect.Width <= 0 || targetRect.Height <= 0)
            {
                MessageBox.Show("The selected UV area has zero width or height.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Graphics g = Graphics.FromImage(_texture))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.CompositingMode = CompositingMode.SourceCopy;

                using (var transparentBrush = new SolidBrush(Color.Transparent))
                {
                    g.FillRectangle(transparentBrush, targetRect);
                }

                g.CompositingMode = CompositingMode.SourceOver;
                g.DrawImage(newCharImage, targetRect);
            }

            _splitContainer.Panel1.Invalidate();
            _isTextureModified = true;
            TextureModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        private TextureMapUV FindUvAtMousePosition(Point mouseLocation)
        {
            Matrix inverseTransform = new Matrix();
            inverseTransform.Translate(_splitContainer.Panel1.AutoScrollPosition.X, _splitContainer.Panel1.AutoScrollPosition.Y);
            inverseTransform.Scale(_zoom, _zoom);
            inverseTransform.Invert();

            Point[] mousePoint = { mouseLocation };
            inverseTransform.TransformPoints(mousePoint);
            PointF imageCoords = mousePoint[0];


            for (int i = _uvEntries.Count - 1; i >= 0; i--)
            {
                var uv = _uvEntries[i];
                var uvRect = new RectangleF(
                    uv.StartX * _texture.Width,
                    uv.StartY * _texture.Height,
                    (uv.EndX - uv.StartX) * _texture.Width,
                    (uv.EndY - uv.StartY) * _texture.Height
                );
                if (uvRect.Contains(imageCoords))
                {
                    return uv;
                }
            }

            return null;
        }

        public bool IsTextureModified()
        {
            return _isTextureModified;
        }

        private void SelectUvInListView(ushort uvId)
        {
            foreach (ListViewItem item in _uvListView.Items)
            {
                if (item.Tag is TextureMapUV uv && uv.Id == uvId)
                {
                    _uvListView.SelectedIndexChanged -= UvListView_SelectedIndexChanged;
                    _uvListView.SelectedItems.Clear();
                    item.Selected = true;
                    item.Focused = true;
                    item.EnsureVisible();

                    _uvListView.SelectedIndexChanged += UvListView_SelectedIndexChanged;
                    _selectedUvEntry = uv;
                    _splitContainer.Panel1.Invalidate();

                    break;
                }
            }
            TootgleReplaceButtonState();
        }

        private void TootgleReplaceButtonState()
        {
            if (_uvListView.SelectedItems.Count > 0)
            {
                _replaceCharButton.Enabled = true;
            }
            else
            {
                _replaceCharButton.Enabled = false;
            }
        }

        private void FillUvRect(Graphics g, TextureMapUV uv, Brush brush)
        {
            float x = uv.StartX * _texture.Width;
            float y = uv.StartY * _texture.Height;
            float width = (uv.EndX - uv.StartX) * _texture.Width;
            float height = (uv.EndY - uv.StartY) * _texture.Height;

            float rectX = Math.Min(uv.StartX, uv.EndX) * _texture.Width;
            float rectY = Math.Min(uv.StartY, uv.EndY) * _texture.Height;
            float rectWidth = Math.Abs(width);
            float rectHeight = Math.Abs(height);

            if (rectWidth > 0 && rectHeight > 0)
            {
                g.FillRectangle(brush, rectX, rectY, rectWidth, rectHeight);
            }
        }

        private void DrawUvRect(Graphics g, TextureMapUV uv, Pen pen)
        {
            float x = uv.StartX * _texture.Width;
            float y = uv.StartY * _texture.Height;

            float width = (uv.EndX - uv.StartX) * _texture.Width;
            float height = (uv.EndY - uv.StartY) * _texture.Height;

            float rectX = Math.Min(uv.StartX, uv.EndX) * _texture.Width;
            float rectY = Math.Min(uv.StartY, uv.EndY) * _texture.Height;
            float rectWidth = Math.Abs(width);
            float rectHeight = Math.Abs(height);

            if (rectWidth > 0 && rectHeight > 0)
            {
                g.DrawRectangle(pen, rectX, rectY, rectWidth, rectHeight);
            }
        }

        public Bitmap? GetModifiedTexture()
        {
            if (_texture is Bitmap bmp)
            {
                return bmp;
            }

            return _texture != null ? new Bitmap(_texture) : null;
        }

        public Dictionary<ushort, string> GetCharTable()
        {
            return _charTable ?? new Dictionary<ushort, string>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _toolTip?.Dispose();
                _checkerboardBrush?.Dispose();
                _texture?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}