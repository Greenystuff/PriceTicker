using HtmlAgilityPack;
using PriceTicker.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PriceTicker
{
    internal class Scraping
    {
        DatabaseManager databaseManager = new DatabaseManager();
        DataBaseUpdater dataBaseUpdater = new DataBaseUpdater();
        TicketCrafter ticketCrafter = new TicketCrafter();
        List<Models.PcGamer> productList = new List<Models.PcGamer>();
        private BackgroundWorker? worker = null;

        public void ScrapWebsite()
        {

            if (!databaseManager.CheckIfTableContainsData("PcGamer"))
            {
                AfficheCreator.gui.ProgressBar.Visibility = Visibility.Visible;
                AfficheCreator.gui.ProgressTextBlock.Visibility = Visibility.Visible;
                worker = new();
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
            else
            {
                List<Models.PcGamer> ProductsInDb = new();
                List<int> newIdsPCSaved = new();
                newIdsPCSaved = databaseManager.SelectAllIdPcGamer();

                for (int i = 0; i < newIdsPCSaved.Count; i++)
                {
                    ProductsInDb.Add(databaseManager.SelectPcGamerByID(newIdsPCSaved[i]));
                }

                var sortedProducts = ProductsInDb.OrderBy(c => c.getPrix());

                Debug.WriteLine("Nombre d'éléments : " + ProductsInDb.Count);

                AfficheCreator.gui.Dispatcher.Invoke(new Action(() =>
                {
                    AfficheCreator.gui.ConfigGroupDataGrid.AutoGenerateColumns = false;
                    AfficheCreator.gui.ProductCountTxt.Text = "Nombre de produits : " + ProductsInDb.Count.ToString();

                    IEnumerable _bind = sortedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        prixBarre = product.getPrixBarre(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    DateTime LastUpdate = Properties.Settings.Default.LastUpdateDate;
                    if (LastUpdate.Date.ToShortDateString() != "01/01/0001")
                    {
                        TimeSpan Interval = DateTime.Now.Subtract(LastUpdate);
                        if (Interval.Days == 0 && Interval.Hours == 0 && Interval.Minutes == 0)
                        {
                            AfficheCreator.gui.LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Seconds + " secondes)";
                        }
                        if (Interval.Days == 0 && Interval.Hours == 0 && Interval.Minutes != 0)
                        {
                            AfficheCreator.gui.LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Minutes + " minutes)";
                        }
                        if (Interval.Days == 0 && Interval.Hours != 0)
                        {
                            AfficheCreator.gui.LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Hours + " heures et " + Interval.Minutes + " minutes)";
                        }
                        if (Interval.Days != 0)
                        {
                            AfficheCreator.gui.LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Days + " jours, " + Interval.Hours + " heures et " + Interval.Minutes + " minutes)";
                        }
                    }

                    AfficheCreator.gui.ConfigGroupDataGrid.ItemsSource = _bind;
                }), DispatcherPriority.SystemIdle);

            }

        }

        public void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            AfficheCreator.gui.ProgressBar.Value = e.ProgressPercentage;
            AfficheCreator.gui.ProgressTextBlock.Text = (string)e.UserState;
        }

        public void worker_DoWork(object? sender, DoWorkEventArgs e)
        {

            productList.Clear();
            var ProductsWebAdress = @"https://www.cybertek.fr/univers-gamer/pc-assembles";
            HtmlWeb webPageAllProducts = new HtmlWeb();
            var ProductsHtmlDoc = webPageAllProducts.Load(ProductsWebAdress);

            worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Chargement en cours... 0 %"));
            HtmlNodeCollection listArticlesNodes = ProductsHtmlDoc.DocumentNode.SelectNodes("//article");
            int productNbr = listArticlesNodes.Count;
            for (int i = 0; i < productNbr; i++)
            {

                if (worker.CancellationPending)
                {
                    break;
                }

                var h2Nodes = listArticlesNodes[i].Descendants("h2");
                var productLink = listArticlesNodes[i].SelectSingleNode("a").Attributes["href"].Value;

                int configID = int.Parse(listArticlesNodes[i].Attributes["data-id"].Value);
                //Debug.WriteLine("ID unique du PC : " + configID);


                Models.PcGamer Product = new();

                Product.setIdConfig(configID);

                foreach (var h2node in h2Nodes)
                {
                    string productName = h2node.InnerText.Replace("pc gamer", "").Replace("\n", "").Replace("\r", "").Trim();
                    productName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(productName.ToLower());
                    //Debug.WriteLine("Nom du PC : " + productName);

                    Product.setName(productName);
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

                var ProductWebAdress = @"https://www.cybertek.fr" + productLink;
                HtmlWeb webPageProduct = new HtmlWeb();
                HtmlDocument ProductHtmlDoc = webPageProduct.Load(ProductWebAdress);
                HtmlNodeCollection listDescNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='pcgamer__caracteristiques__fiche-pc']");
                HtmlNodeCollection prixBarreNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='price-gaming-page']");

                if (prixBarreNodes != null)
                {
                    if (prixBarreNodes.Last().InnerHtml.Contains("prix_total_sans_remise"))
                    {
                        HtmlNodeCollection prixNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@class='prix_total_sans_remise']");
                        string prix = prixNodes[0].InnerText.Replace("€", ",");
                        Product.setPrix(Decimal.Parse(prix));
                    }

                    if (prixBarreNodes.Last().InnerHtml.Contains("prix-config-barre-sans-option"))
                    {
                        HtmlNodeCollection prixBarreSpanNodes = ProductHtmlDoc.DocumentNode.SelectNodes("//*[@id='prix-config-barre-sans-option']");
                        string prixBarreStr = prixBarreSpanNodes[0].InnerText.Replace("€", ",");
                        Product.setPrixBarre(Decimal.Parse(prixBarreStr));
                    }

                    int caracNbr = listDescNodes.Count;
                    for (int i2 = 0; i2 < caracNbr; i2++)
                    {

                        var listNodes = listDescNodes[i2].Descendants("li");

                        foreach (var listNode in listNodes)
                        {
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
                                if (categorie.StartsWith("Processeur"))
                                {
                                    Product.setProcesseur(categorie + caracteristique);
                                }
                                if (categorie.StartsWith("Alimentation"))
                                {
                                    Product.setAlimentation(caracteristique);
                                }


                                //Debug.WriteLine("Catégorie non Link : " + categorie);
                                //Debug.WriteLine("Caractéristique non link : " + categorie + caracteristique);
                            }
                            else
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
                                        Product.setDisqueSsd(caracteristiqueDesc + caracteristique);
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

                }

                Settings.Default.Save();

                float valeur = 100 * i / productNbr;
                int roundValeur = (int)Math.Round(valeur);

                worker.ReportProgress(roundValeur, String.Format("Chargement en cours... " + valeur + " %", i + 2));


            }

            if (!worker.CancellationPending)
            {

                // La base de données ne contient pas de données donc on INSERT toutes les configs.
                if (!databaseManager.CheckIfTableContainsData("PcGamer"))
                {
                    dataBaseUpdater.insertPcAndComposantsInDb(productList);
                }
                else
                {
                    // Cas où la base de données contient déjà des données.
                    List<int> IdsPCSaved = new();
                    List<int> IdsPCWeb = new();
                    IdsPCSaved = databaseManager.SelectAllIdPcGamer();

                    for (int i = 0; i < productList.Count; i++)
                    {
                        IdsPCWeb.Add(productList[i].getIdConfig());
                    }

                    // Vérifie si chacune des configs relevées sur internet sont présentes dans la base de données. Sinon on INSERT les nouvelles.
                    dataBaseUpdater.CompareWebwithDbAndInsertNews(productList, IdsPCSaved);
                    //Test :
                    //IdsPCWeb.RemoveAt(IdsPCWeb.Count - 1);
                    // Vérifie si une config a disparu d'internet, et dans ce cas il faudra la virer de la table pour l'archiver dans la table des archives.
                    for (int i = 0; i < IdsPCSaved.Count; i++)
                    {
                        if (!IdsPCWeb.Contains(IdsPCSaved[i]))
                        {
                            Debug.WriteLine("Le PC \"" + databaseManager.SelectPcGamerByID(IdsPCSaved[i]).getName() + "\" n'existe plus sur internet mais est présent dans la base de données. Archivage de la config... \r");
                            bool isArchived = databaseManager.ArchiveConfigByID(IdsPCSaved[i]);
                            if (isArchived)
                            {
                                databaseManager.DeleteConfigByID(IdsPCSaved[i]);
                            }
                        }
                    }

                    // Vérifie si un composant a changé.
                    dataBaseUpdater.CompareComposantsWithWeb(IdsPCSaved, IdsPCWeb, productList);
                }

                List<Models.PcGamer> ProductsInDb = new();
                List<int> newIdsPCSaved = new();
                newIdsPCSaved = databaseManager.SelectAllIdPcGamer();

                for (int i = 0; i < newIdsPCSaved.Count; i++)
                {
                    ProductsInDb.Add(databaseManager.SelectPcGamerByID(newIdsPCSaved[i]));
                }

                var sortedProducts = ProductsInDb.OrderBy(c => c.getPrix());

                Debug.WriteLine("Nombre d'éléments : " + ProductsInDb.Count);

                AfficheCreator.gui.Dispatcher.Invoke(new Action(() =>
                {
                    AfficheCreator.gui.ConfigGroupDataGrid.AutoGenerateColumns = false;
                    AfficheCreator.gui.ProductCountTxt.Text = "Nombre de produits : " + ProductsInDb.Count.ToString();

                    IEnumerable _bind = sortedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        prixBarre = product.getPrixBarre(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    AfficheCreator.gui.ConfigGroupDataGrid.ItemsSource = _bind;
                    Settings.Default.LastUpdateDate = DateTime.Now;
                    Settings.Default.Save();
                    DateTime LastUpdate = Settings.Default.LastUpdateDate;
                    int timeDifference = (int)Math.Round(DateTime.Now.Subtract(LastUpdate).TotalSeconds);

                    AfficheCreator.gui.LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + timeDifference + " secondes)";
                }), DispatcherPriority.SystemIdle);

                worker.ReportProgress(100, "Mise à jour terminée !");

            }
            else
            {
                Debug.WriteLine("Tâche asynchrone annulée !!");
                worker.Dispose();
                worker = null;
            }

        }

        public void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (worker != null)
            {
                ticketCrafter.CraftPoster(productList[0].getName(), productList[0].getBoitier(), productList[0].getCarteMere(), productList[0].getProcesseur(), productList[0].getRam(), productList[0].getCarteGraphique(), productList[0].getDisqueSsd(), productList[0].getAlimentation(), productList[0].getSystemeExploitation(), productList[0].getPrixBarre().ToString(), productList[0].getPrix().ToString());

                Uri AfficheUri = new(AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp", UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = AfficheUri;
                _image.EndInit();
                AfficheCreator.gui.imgAffiche.Source = _image;


                AfficheCreator.gui.ProgressBar.Value = 0;
                AfficheCreator.gui.ProgressBar.Visibility = Visibility.Hidden;
                AfficheCreator.gui.ProgressTextBlock.Text = "";
                AfficheCreator.gui.ProgressTextBlock.Visibility = Visibility.Hidden;
            }
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

    }
}
