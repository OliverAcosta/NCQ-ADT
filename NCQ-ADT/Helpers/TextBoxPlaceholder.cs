
namespace NCQ_ADT.Helpers
{
    public class TextBoxPlaceholder
    {
        private readonly TextBox textbox;
        private string placeholder;
        public bool Has { get { return textbox.Text == placeholder; } }

        public TextBoxPlaceholder(TextBox txt, string placeholder = "Escribe aquí...") { 
            this.textbox = txt;
            this.placeholder = placeholder;
            init();
        }

        public void SetPlaceHolder(string str)
        {
            this.textbox.Text = string.Empty;
            this.placeholder = str;
            this.SetRule();
        }
        private void init()
        {
            textbox.Text = placeholder;
            textbox.Enter += Enter;
            textbox.Leave += Leave;
        }
        private void SetRule()
        {
            if (string.IsNullOrWhiteSpace(textbox.Text))
            {
                textbox.Text = placeholder;
                textbox.ForeColor = Color.Gray;
            }
        }
        private void Leave(object sender, EventArgs e)
        {
            SetRule();
        }

        private void Enter(object sender, EventArgs e)
        {
            if (textbox.Text == placeholder)
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }
        }
    }
}
