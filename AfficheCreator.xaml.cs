﻿using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
   
    public partial class AfficheCreator : Window
    {

        List<Models.PcGamer> productList = new List<Models.PcGamer>();

        public AfficheCreator()
        {
            InitializeComponent();
            ScrapWebsite();
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

        private void ScrapWebsite()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressTextBlock.Visibility = Visibility.Visible;
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
            productList.Clear();
            var ProductsWebAdress = @"https://www.cybertek.fr/univers-gamer/pc-assembles";
            HtmlWeb webPageAllProducts = new HtmlWeb();
            var ProductsHtmlDoc = webPageAllProducts.Load(ProductsWebAdress);

            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Chargement en cours... 0 %"));
            HtmlNodeCollection listArticlesNodes = ProductsHtmlDoc.DocumentNode.SelectNodes("//article");
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
                    //Debug.WriteLine("Nom du PC : " + productName);

                    Product.setname(productName);
                }
                //Debug.WriteLine("Lien Internet : " + productLink);
                Product.setWebLink(@"https://www.cybertek.fr" + productLink);
                string Product_Id = productLink;

                int indexProductId = Product_Id.IndexOf(".aspx");
                if (indexProductId > 0)
                {
                    List<long> productIdTemp = new List<long>();
                    Product_Id = Product_Id.Substring(0, indexProductId);
                    productIdTemp = FindIdNumbers(Product_Id);
                    Product_Id = productIdTemp.Last().ToString();
                }

                XmlTextReader ProduitsXml = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD\\Temp_xml\\Produit.xml");

                String? IdJaja = null;

                while (ProduitsXml.Read())
                {

                    if (ProduitsXml.NodeType == XmlNodeType.Element && ProduitsXml.Name == "produit_id")
                    {
                        string temp = ProduitsXml.ReadElementContentAsString();
                        if (temp == Product_Id)
                        {
                            ProduitsXml.ReadToNextSibling("num_produit");
                            IdJaja = ProduitsXml.ReadElementContentAsString();
                            Product.setIdJaja(IdJaja);
                        }
                        
                    }
                }

                var ProductWebAdress = @"https://www.cybertek.fr" + productLink;
                HtmlWeb webPageProduct = new HtmlWeb();
                HtmlDocument ProductHtmlDoc = webPageProduct.Load(ProductWebAdress);
                HtmlNodeCollection listDescNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='pcgamer__caracteristiques__fiche-pc']");
                HtmlNodeCollection prixBarreNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='price-gaming-page']");
                
                if (prixBarreNodes[0].InnerHtml.Contains("prix_total_sans_remise"))
                {
                    HtmlNodeCollection prixNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='prix_total_sans_remise']");
                    string prix = prixNodes[0].InnerText.Replace("€", ",");
                    Product.setPrix(Decimal.Parse(prix));
                }

                if (prixBarreNodes[0].InnerHtml.Contains("prix-config-barre-sans-option"))
                {
                    HtmlNodeCollection prixBarreSpanNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@id='prix-config-barre-sans-option']");
                    string prixBarreStr = prixBarreSpanNodes[0].InnerText.Replace("€", ",");
                    Product.setPrixBarre(Decimal.Parse(prixBarreStr));
                }

                


                int caracNbr = listDescNodes.Count;
                

                for (int i2 = 0; i2 < caracNbr; i2++)
                {

                    var listNodes = listDescNodes[i2].Descendants("li");

                    foreach (var listNode in listNodes) {
                        string categorie = "";
                        string caracteristiqueDesc = "";
                        string caracteristique = "";

                        if (!listNode.InnerHtml.StartsWith("<a"))
                        {
                            categorie = HttpUtility.HtmlDecode(listNode.Descendants("strong").First().InnerText);
                            caracteristique = HttpUtility.HtmlDecode(listNode.InnerText).Replace(categorie, "");
                            

                            if (categorie.StartsWith("Sans sys"))
                            {
                                Product.setSystemeExploitation(caracteristique);
                            }
                            if (categorie.StartsWith("Disque"))
                            {
                                Product.setDisqueSsd(caracteristique);
                            }
                            if (categorie.StartsWith("Mémoire PC") || categorie.StartsWith("Memoire PC"))
                            {
                                Product.setRam(caracteristique);
                            }
                            if (categorie.StartsWith("Carte graphique") || categorie.StartsWith("Carte Graphique") || categorie.StartsWith("Composant"))
                            {
                                Product.setCarteGraphique(caracteristique);
                            }
                            if(categorie.StartsWith("Processeur"))
                            {
                                Product.setProcesseur(categorie + caracteristique);
                            }
                            if (categorie.StartsWith("Alimentation"))
                            {
                                Product.setAlimentation(caracteristique);
                            }


                            //Debug.WriteLine("Catégorie non Link : " + categorie);
                            //Debug.WriteLine("Caractéristique non link : " + categorie + caracteristique);
                        }else
                        {
                            categorie = listNode.FirstChild.Attributes["href"].Value.Replace("../../", "");
                            int index = categorie.IndexOf("/");
                            if (index > 0)
                            {
                                
                                categorie = categorie.Substring(0, index); // or index + 1 to keep slash
                            }
                            

                            string[] tempArray = listNode.FirstChild.OuterHtml.Split("<strong>");
                            caracteristiqueDesc = tempArray[1];
                            int indexCaracDesc = caracteristiqueDesc.IndexOf("</strong>");
                            if (indexCaracDesc > 0)
                            {
                                caracteristiqueDesc = HttpUtility.HtmlDecode(caracteristiqueDesc.Substring(0, indexCaracDesc)); // or index + 1 to keep slash
                            }

                            string[] tempArray2 = listNode.FirstChild.OuterHtml.Split("</strong>");
                            caracteristique = tempArray2[1];
                            int indexCarac = caracteristique.IndexOf("</a>");
                            if (indexCarac > 0)
                            {
                                caracteristique = HttpUtility.HtmlDecode(caracteristique.Substring(0, indexCarac)); // or index + 1 to keep slash
                            }

                            switch (categorie)
                            {
                                case "boitier-pc":
                                    Product.setBoitier(caracteristiqueDesc + caracteristique);
                                    break;

                                case "accessoire-boitier":
                                    Product.setAccessoireBoitier(caracteristiqueDesc + caracteristique);
                                    break;

                                case "ventilateur-boitier":
                                    Product.setVentilateurBoitier(caracteristiqueDesc + caracteristique);
                                    break;

                                case "carte-mere":
                                    Product.setCarteMere(caracteristiqueDesc + caracteristique);
                                    break;

                                case "processeur":
                                    Product.setProcesseur(caracteristiqueDesc + caracteristique);
                                    break;

                                case "ventilateur-cpu":
                                    Product.setVentirad(caracteristiqueDesc + caracteristique);
                                    break;

                                case "disque-ssd":
                                    Product.setDisqueSsd(caracteristiqueDesc +  caracteristique);
                                    break;

                                case "disque-dur-interne-3-5":
                                    Product.setDisqueSupplementaire(caracteristiqueDesc + caracteristique);
                                    break;

                                case "memoire-pc":
                                    Product.setRam(caracteristiqueDesc + caracteristique);
                                    break;

                                case "watercooling":
                                    Product.setWaterCooling(caracteristiqueDesc + caracteristique);
                                    break;

                                case "carte-graphique":
                                    Product.setCarteGraphique(caracteristiqueDesc + caracteristique);
                                    break;

                                case "accessoire-carte-graphique":
                                    Product.setAccessoireCarteGraphique(caracteristiqueDesc + caracteristique);
                                    break;

                                case "carte-reseau":
                                    Product.setCarteReseau(caracteristiqueDesc + caracteristique);
                                    break;

                                case "alimentation":
                                    Product.setAlimentation(caracteristiqueDesc + caracteristique);
                                    break;

                                case "accessoire-alimentation":
                                    Product.setAccessoireAlimentation(caracteristiqueDesc + caracteristique);
                                    break;

                                case "logiciel-systeme-exploitation":
                                    Product.setSystemeExploitation(caracteristique);
                                    break;

                                case "integration-logicielle":
                                    Product.setSystemeExploitation(caracteristique);
                                    break;


                            }


                            //Debug.WriteLine("Catégorie : " + categorie);
                            
                            //Debug.WriteLine("Caractéristique : " + caracteristiqueDesc + caracteristique);
                        }

                        
                    }

                }

                int nbrPc = i + 1;
                string resultat = "PC N°" + nbrPc + "\r" + Product.getAllCaracteristiques();
                Debug.WriteLine(resultat);
                
                productList.Add(Product);

                float valeur = 100*i/productNbr;
                int roundValeur = (int)Math.Round(valeur);

                worker.ReportProgress(roundValeur, String.Format("Chargement en cours... " + valeur + " %", i+2));
                
            }

            Debug.WriteLine("Nombre d'éléments : " + productList.Count);
            Dispatcher.Invoke(new Action(() =>
            {
                ConfigGroupDataGrid.AutoGenerateColumns = false;
                

                IEnumerable _bind = productList.Select(product => new
                {
                    
                    id = product.getIdJaja(),
                    name = product.getName(),
                    prix = product.getPrix() + " €",
                    boitier = product.getBoitier(),
                    carteMere = product.getCarteMere(),
                    processeur = product.getProcesseur(),
                    carteGraphique = product.getCarteGraphique(),
                    ram = product.getRam(),

                });


                ConfigGroupDataGrid.ItemsSource = _bind;
            }), DispatcherPriority.SystemIdle);
            
            worker.ReportProgress(100, "Mise à jour terminée !");

        }

        private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Value = 0;
            ProgressBar.Visibility = Visibility.Hidden;
            ProgressTextBlock.Text = "";
            ProgressTextBlock.Visibility = Visibility.Hidden;
        }

        public static List<long> FindIdNumbers(string str)
        {
            var nums = new List<long>();
            var start = -1;
            for (int i = 0; i < str.Length; i++)
            {
                if (start < 0 && Char.IsDigit(str[i]))
                {
                    start = i;
                }
                else if (start >= 0 && !Char.IsDigit(str[i]))
                {
                    nums.Add(long.Parse(str.Substring(start, i - start)));
                    start = -1;
                }
            }
            if (start >= 0)
                nums.Add(long.Parse(str.Substring(start, str.Length - start)));
            return nums;
        }

        private void OpenURLinNav(object sender, RoutedEventArgs e)
        {
            string IdJajaSelected = ConfigGroupDataGrid.SelectedItem.ToString();
            List<long> IdJajaSelectedList = new List<long>();
            IdJajaSelectedList = FindIdNumbers(IdJajaSelected);
            IdJajaSelected = IdJajaSelectedList.First().ToString();

            for (int i = 0; i < productList.Count; i++)
            {
                string idJajaRecherche = productList[i].getIdJaja();
                if (idJajaRecherche.Contains(IdJajaSelected))
                {
                    string url = productList[i].getWebLink();
                    var sInfo = new ProcessStartInfo(url)
                    {
                        UseShellExecute = true,
                    };
                    Process.Start(sInfo);
                    break;
                }
            }

            
        }

        private void CraftPoster(string Libelle)
        {
            Debug.WriteLine("Création de l'affiche...");
            string PatronAffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            string AffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(PatronAffichePath);

            StringFormat IntroFormat = new();
            IntroFormat.Alignment = StringAlignment.Center;
            IntroFormat.LineAlignment = StringAlignment.Near;
            System.Drawing.Rectangle rectIntro = new(0, 55, bitmap.Width, 40);

            StringFormat LibelleFormat = new();
            LibelleFormat.Alignment = StringAlignment.Center;
            LibelleFormat.LineAlignment = StringAlignment.Near;
            System.Drawing.Rectangle rectLibelle = new(0, 94, bitmap.Width, 100);

            StringFormat CategorieBoitierFormat = new();
            CategorieBoitierFormat.Alignment = StringAlignment.Far;
            CategorieBoitierFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieBoitier = new(-110, 225, bitmap.Width/2, 40);

            StringFormat CategorieCMFormat = new();
            CategorieCMFormat.Alignment = StringAlignment.Far;
            CategorieCMFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieCM = new(-110, 295, bitmap.Width / 2, 40);

            StringFormat CategorieProcFormat = new();
            CategorieProcFormat.Alignment = StringAlignment.Far;
            CategorieProcFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieProc = new(-110, 365, bitmap.Width / 2, 40);

            StringFormat CategorieRamFormat = new();
            CategorieRamFormat.Alignment = StringAlignment.Far;
            CategorieRamFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieRam = new(-110, 435, bitmap.Width / 2, 40);

            StringFormat CategorieCGFormat = new();
            CategorieCGFormat.Alignment = StringAlignment.Far;
            CategorieCGFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieCG = new(-110, 505, bitmap.Width / 2, 40);

            StringFormat CategorieStockageFormat = new();
            CategorieStockageFormat.Alignment = StringAlignment.Far;
            CategorieStockageFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieStockage = new(-110, 575, bitmap.Width / 2, 40);

            StringFormat CategorieAlimFormat = new();
            CategorieAlimFormat.Alignment = StringAlignment.Far;
            CategorieAlimFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieAlim = new(-110, 645, bitmap.Width / 2, 40);

            StringFormat CategorieOSFormat = new();
            CategorieOSFormat.Alignment = StringAlignment.Far;
            CategorieOSFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCategorieOS = new(-110, 715, bitmap.Width / 2, 40);

            StringFormat CaracBoitierFormat = new();
            CaracBoitierFormat.Alignment = StringAlignment.Near;
            CaracBoitierFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracBoitier = new(320, 225, bitmap.Width / 2, 40);

            StringFormat CaracCMFormat = new();
            CaracCMFormat.Alignment = StringAlignment.Near;
            CaracCMFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracCM = new(320, 295, bitmap.Width / 2, 40);

            StringFormat CaracProcFormat = new();
            CaracProcFormat.Alignment = StringAlignment.Near;
            CaracProcFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracProc = new(320, 365, bitmap.Width / 2, 40);

            StringFormat CaracRamFormat = new();
            CaracRamFormat.Alignment = StringAlignment.Near;
            CaracRamFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracRam = new(320, 435, bitmap.Width / 2, 40);

            StringFormat CaracCGFormat = new();
            CaracCGFormat.Alignment = StringAlignment.Near;
            CaracCGFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracCG = new(320, 505, bitmap.Width / 2, 40);

            StringFormat CaracStockageFormat = new();
            CaracStockageFormat.Alignment = StringAlignment.Near;
            CaracStockageFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracStockage = new(320, 575, bitmap.Width / 2, 40);

            StringFormat CaracAlimFormat = new();
            CaracAlimFormat.Alignment = StringAlignment.Near;
            CaracAlimFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracAlim = new(320, 645, bitmap.Width / 2, 40);

            StringFormat CaracOSFormat = new();
            CaracOSFormat.Alignment = StringAlignment.Near;
            CaracOSFormat.LineAlignment = StringAlignment.Center;
            System.Drawing.Rectangle rectCaracOS = new(320, 715, bitmap.Width / 2, 40);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                PrivateFontCollection privateFonts = new PrivateFontCollection(); 
                privateFonts.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\CenturyGothic.ttf");
                privateFonts.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\Mass_Effect.ttf");

                Font IntroFont = new(privateFonts.Families[0], 25, GraphicsUnit.Point);
                Font NameFont = new(privateFonts.Families[1], 47, GraphicsUnit.Point);
                Font CategorieFont = new(privateFonts.Families[0], 14, System.Drawing.FontStyle.Regular);
                Font CaracFont = new(privateFonts.Families[0], 20, System.Drawing.FontStyle.Bold);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                
                graphics.DrawString("PC monté", IntroFont, System.Drawing.Brushes.Black, rectIntro, IntroFormat);
                graphics.DrawString(Libelle, NameFont, System.Drawing.Brushes.Black, rectLibelle, LibelleFormat);
                graphics.DrawString("Boîtier", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieBoitier, CategorieBoitierFormat);
                graphics.DrawString("Carte mère", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieCM, CategorieCMFormat);
                graphics.DrawString("Processeur", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieProc, CategorieProcFormat);
                graphics.DrawString("Mémoire vive", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieRam, CategorieRamFormat);
                graphics.DrawString("Carte graphique", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieCG, CategorieCGFormat);
                graphics.DrawString("Stockage", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieStockage, CategorieStockageFormat);
                graphics.DrawString("Alimentation", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieAlim, CategorieAlimFormat);
                graphics.DrawString("Système d'exploitation", CategorieFont, System.Drawing.Brushes.Gray, rectCategorieOS, CategorieOSFormat);

                graphics.DrawString("Lian-Li ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracBoitier, CaracBoitierFormat);
                graphics.DrawString("MSI B560 ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracCM, CaracCMFormat);
                graphics.DrawString("i7-7700K de ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracProc, CaracProcFormat);
                graphics.DrawString("2x128 Go ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracRam, CaracRamFormat);
                graphics.DrawString("3090Ti de ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracCG, CaracCGFormat);
                graphics.DrawString("SSD de ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracStockage, CaracStockageFormat);
                graphics.DrawString("M-RED ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracAlim, CaracAlimFormat);
                graphics.DrawString("Windows 10 ta mère !!", CaracFont, System.Drawing.Brushes.Black, rectCaracOS, CaracOSFormat);



                graphics.Dispose();
            }

                using (MemoryStream memory = new())
                {
                    Bitmap bm = new(bitmap);
                    bitmap.Dispose();
                    using (FileStream fs = new(AffichePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        bm.Save(memory, ImageFormat.Bmp);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                        fs.Dispose();

                    }
                    bm.Dispose();
                    memory.Close();
                    memory.Dispose();
                    bitmap.Dispose();
                    Debug.WriteLine("Affiche crée avec succès !");
                }
                
        }

        private void ValiderAffiche(object sender, RoutedEventArgs e)
        {
            
            CraftPoster("CRONOS");
        }
    }

        
    }