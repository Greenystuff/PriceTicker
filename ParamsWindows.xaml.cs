using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PriceTicker
{
    /// <summary>
    /// Logique d'interaction pour ParamsWindows.xaml
    /// </summary>
    public partial class ParamsWindows : Window
    {

        public static ParamsWindows? gui;

        public ParamsWindows()
        {
            InitializeComponent();
            gui = this;
            IpAdressTxt.Text = Properties.Settings.Default.ClientAdresseIP;
            PortTxt.Text = Properties.Settings.Default.ClientPort;
        }
    }
}
