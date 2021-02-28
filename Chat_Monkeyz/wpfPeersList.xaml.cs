using System;
using System.Windows;
using System.Windows.Controls;


namespace Chat_Monkeyz
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }


        //holy shit
        public void UpdateBackgroundColor(System.Drawing.Color c)
        {
            System.Windows.Media.BrushConverter bc = new System.Windows.Media.BrushConverter();
            System.Windows.Media.Brush b = (System.Windows.Media.Brush)bc.ConvertFrom(System.Drawing.ColorTranslator.ToHtml(c));
            listPeers.Background = b;
        }
    }
}
