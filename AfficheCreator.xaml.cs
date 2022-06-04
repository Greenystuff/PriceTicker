using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace PriceTicker
{
    /// <summary>
    /// Logique d'interaction pour AfficheCreator.xaml
    /// </summary>
    public partial class AfficheCreator : Window
    {

        List<Models.PcGamer> productList = new List<Models.PcGamer>();

        public AfficheCreator()
        {
            InitializeComponent();
        }

        public static String code(string Url)
        {

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

        private void FindHTMLCode(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();

        }

        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            ProgressTextBlock.Text = (string)e.UserState;
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {

            var ProductsWebAdress = @"https://www.cybertek.fr/univers-gamer/pc-assembles";
            HtmlWeb webPageAllProducts = new HtmlWeb();
            var ProductsHtmlDoc = webPageAllProducts.Load(ProductsWebAdress);

            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Chargement en cours... 1."));
            var listArticlesNodes = ProductsHtmlDoc.DocumentNode.SelectNodes("//article");
            int productNbr = listArticlesNodes.Count;
            for (int i = 0; i < productNbr; i++)
            {
                var h2Nodes = listArticlesNodes[i].Descendants("h2");
                var productLink = listArticlesNodes[i].SelectSingleNode("a").Attributes["href"].Value;
                Models.PcGamer Product = new();

                foreach (var h2node in h2Nodes)
                {
                    string productName = h2node.InnerText.Replace("pc gamer", "").Replace("\n", "").Replace("\r", "").Trim();
                    productName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(productName.ToLower());
                    Debug.WriteLine("Nom du PC : " + productName);

                    Product.setname(productName);
                }
                Debug.WriteLine("Lien Internet : " + productLink);
                Product.setWebLink(productLink);
                productList.Add(Product);

                var ProductWebAdress = @"https://www.cybertek.fr" + productLink;
                HtmlWeb webPageProduct = new HtmlWeb();
                var ProductHtmlDoc = webPageProduct.Load(ProductWebAdress);
                worker.ReportProgress((i + 1) * (productNbr/15), String.Format("Chargement en cours... {0}.", i+2));
                
            }

            Debug.WriteLine("Nombre d'éléments : " + productList.Count);
            worker.ReportProgress(100, "Mise à jour terminée !");

        }

        private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("PriceTicker est à jour !");
            ProgressBar.Value = 0;
            ProgressBar.Visibility = Visibility.Hidden;
            ProgressTextBlock.Text = "";
        }
    }
}
