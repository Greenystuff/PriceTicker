using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
            int nbrElements = 0;
            var html = @"https://www.cybertek.fr/univers-gamer/pc-assembles";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);

            var listArticlesNodes = htmlDoc.DocumentNode.SelectNodes("//article");


            foreach (var articleNode in listArticlesNodes)
            {
                var h2Nodes = articleNode.Descendants("h2");
                var aNode = articleNode.SelectSingleNode("a").Attributes["href"].Value;
                Models.PcGamer Product = new();

                foreach (var h2node in h2Nodes)
                {
                    var span = h2node.Element("span").FirstChild;
                    string pcName = h2node.InnerText.Replace("pc gamer", "").Replace("\n", "").Replace("\r", "").Trim();
                    pcName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pcName.ToLower());
                    Debug.WriteLine("Nom du PC : " + pcName);
                    
                    Product.setname(pcName);
                }
                Debug.WriteLine("Lien Internet : " + aNode);
                Product.setWebLink(aNode);
                productList.Add(Product);
            }

            Debug.WriteLine("Nombre d'éléments : " + productList.Count);

        }

    }
}
