using System;
using System.Windows.Forms;
using System.Drawing;


namespace ExtensionMethods
{
    public static class Extension
    {

        delegate object UseRichTextBox();

        public static void addLine(this RichTextBox rb)
        {

            Chat_Monkeyz.Program.myForm.Invoke(new UseRichTextBox(() =>
            {
                String str = "______________________________________________________________________";
                int start = rb.TextLength + 1;
                int length = str.Length + 1;


                rb.AppendText(str);

                rb.SelectionStart = start;
                rb.SelectionLength = length;
                rb.SelectionColor = Color.Red;
                return "";
            }));

            rb.resetStyle();

        }

        public static void addMessage(this RichTextBox rb, String text, Chat_Monkeyz.Chat.MessageType ms)
        {
            Chat_Monkeyz.Program.myForm.Invoke(new UseRichTextBox(() =>
            {
                int start = rb.TextLength;
                int length = text.Length;

                rb.AppendText(text);

                if (ms.HasFlag(Chat_Monkeyz.Chat.MessageType.Mention))
                {
                    rb.SelectionStart = start;
                    rb.SelectionLength = length;
                    rb.SelectionFont = new Font(rb.SelectionFont, FontStyle.Bold);

                    rb.resetStyle();
                }

                return "";
            }));
        }


        public static void resetStyle(this RichTextBox rb)
        {
            Chat_Monkeyz.Program.myForm.Invoke(new UseRichTextBox(() =>
            {
                rb.SelectionStart = rb.TextLength;
                rb.SelectionLength = 0;
                rb.SelectionColor = Chat_Monkeyz.Program.currentTheme.fontcolor;
                rb.SelectionBackColor = Chat_Monkeyz.Program.currentTheme.backcolor;
                rb.SelectionFont = Chat_Monkeyz.Program.currentTheme.font;
                return "";
            }));
        }



        public static String getRTF(this RichTextBox rb)
        {
            return (String)Chat_Monkeyz.Program.myForm.Invoke(new UseRichTextBox(() =>
            {
                return rb.Rtf;
            }));
        }

        public static void setRTF(this RichTextBox rb, String rtf)
        {
            Chat_Monkeyz.Program.myForm.Invoke(new UseRichTextBox(() =>
            {
                rb.Rtf = rtf;
                return "";
            }));
        }



    }
}