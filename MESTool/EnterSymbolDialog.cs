namespace MESTool
{
    public partial class EnterSymbolDialog : Form
    {
        public EnterSymbolDialog(string oldCharU)
        {
            InitializeComponent();
            textBox1.Text = oldCharU;
            this.AcceptButton = okBtn;
            this.CancelButton = cancelBtn;
        }

        public string Symbol
        {
            get { return textBox1.Text; }
        }
    }
}
