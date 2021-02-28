using System;
using System.Windows.Forms;

namespace Chat_Monkeyz
{
    public static class Prompt
    {
        public static String ShowDialog(String text, String caption)
        {
            Form prompt = new Form();
            prompt.Width = 250;
            prompt.Height = 150;
            prompt.Text = caption;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;
            prompt.KeyPreview = true;

            prompt.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Escape) prompt.Close(); };

            Label textLabel = new Label() { Left = 10, Top = 20, Text = text, Width = 250 };
            Button confirmation = new Button() { Text = "Connect", Left = 70, Width = 80, Top = 80 };
            TextBox textBox = new TextBox() { Left = 40, Top = 50, Width = 150 };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();


            return textBox.Text;
        }
    }
}
