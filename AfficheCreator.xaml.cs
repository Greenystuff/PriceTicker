using HtmlAgilityPack;
using PriceTicker.Properties;
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
using System.Net.Cache;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PriceTicker
{
   
    public partial class AfficheCreator : Window
    {

        List<Models.PcGamer> productList = new List<Models.PcGamer>();
        DatabaseManager databaseManager = new DatabaseManager();

        public AfficheCreator()
        {
            InitializeComponent();
            databaseManager.CreateDbFile();
            databaseManager.CreateDbConnection();
            databaseManager.CreateTables();
            databaseManager.CloseDbConnection();
            ScrapWebsite();
        }

        private void ScrapWebsite()
        {
            
            if (!databaseManager.CheckIfTableContainsData("PcGamer"))
            {
                ProgressBar.Visibility = Visibility.Visible;
                ProgressTextBlock.Visibility = Visibility.Visible;
                BackgroundWorker worker = new();
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();
            }else
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
                
                Dispatcher.Invoke(new Action(() =>
                {
                    ConfigGroupDataGrid.AutoGenerateColumns = false;
                    ProductCountTxt.Text = "Nombre de produits : " + ProductsInDb.Count.ToString();

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

                    ConfigGroupDataGrid.ItemsSource = _bind;
                }), DispatcherPriority.SystemIdle);

            }

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

                float valeur = 100*i/productNbr;
                int roundValeur = (int)Math.Round(valeur);

                worker.ReportProgress(roundValeur, String.Format("Chargement en cours... " + valeur + " %", i+2));
                
            }
            
            // La base de données ne contient pas de données donc on INSERT toutes les configs.
            if (!databaseManager.CheckIfTableContainsData("PcGamer"))
            {
                insertPcAndComposantsInDb(productList);
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
                CompareWebwithDbAndInsertNews(productList, IdsPCSaved);

                // Vérifie si une config a disparu d'internet, et dans ce cas il faudra la virer de la table pour l'archiver dans la table des archives.
                for (int i = 0; i < IdsPCSaved.Count; i++) 
                {
                    if (!IdsPCWeb.Contains(IdsPCSaved[i]))
                    {
                        Debug.WriteLine("Le PC \"" + databaseManager.SelectPcGamerByID(IdsPCSaved[i]).getName() + "\" n'existe plus sur internet mais es tprésent dans la base de données. Archivage de la config... \r");
                        databaseManager.DeleteByID(IdsPCSaved[i]);
                        //Faire le code pour achiver la config parce qu'elle a disparut d'internet.
                    }
                }

                // Vérifie si un composant a changé.
                CompareComposantsWithWeb(IdsPCSaved, IdsPCWeb, productList);
            }

            List<Models.PcGamer> ProductsInDb = new();
            List<int> newIdsPCSaved = new();
            newIdsPCSaved = databaseManager.SelectAllIdPcGamer();

            for(int i = 0; i < newIdsPCSaved.Count; i++)
            {
                ProductsInDb.Add(databaseManager.SelectPcGamerByID(newIdsPCSaved[i]));
            }

            var sortedProducts = ProductsInDb.OrderBy(c => c.getPrix());

            Debug.WriteLine("Nombre d'éléments : " + ProductsInDb.Count);
            
            Dispatcher.Invoke(new Action(() =>
            {
                ConfigGroupDataGrid.AutoGenerateColumns = false;
                ProductCountTxt.Text = "Nombre de produits : " + ProductsInDb.Count.ToString();

                IEnumerable _bind = sortedProducts.Select(product => new
                {
                    
                    name = product.getName(),
                    prix = product.getPrix(),
                    prixBarre = product.getPrixBarre(),
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
            CraftPoster(productList[0].getName(), productList[0].getBoitier(), productList[0].getCarteMere(), productList[0].getProcesseur(), productList[0].getRam(), productList[0].getCarteGraphique(), productList[0].getDisqueSsd(), productList[0].getAlimentation(), productList[0].getSystemeExploitation(), productList[0].getPrixBarre().ToString(), productList[0].getPrix().ToString());

            Uri AfficheUri = new(AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp", UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = AfficheUri;
            _image.EndInit();
            imgAffiche.Source = _image;


            ProgressBar.Value = 0;
            ProgressBar.Visibility = Visibility.Hidden;
            ProgressTextBlock.Text = "";
            ProgressTextBlock.Visibility = Visibility.Hidden;
        }

        public void insertPcAndComposantsInDb(List<Models.PcGamer> productList)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                databaseManager.InsertPCGamer(
                    productList[i].getIdConfig(),
                    productList[i].getName(),
                    productList[i].getPrix().ToString(),
                    productList[i].getPrixBarre().ToString(),
                    productList[i].getWebLink(),
                    DateTime.Now.Date);

                // Insertion des Composants
                if (productList[i].getBoitier() != "")
                {
                    databaseManager.InsertComposants("Boîtier", productList[i].getBoitier());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Boîtier", productList[i].getBoitier()),
                            "Boîtier",
                            productList[i].getBoitier()
                        );
                }

                if (productList[i].getAccessoireBoitier() != "")
                {
                    databaseManager.InsertComposants("Accessoire de boîtier", productList[i].getAccessoireBoitier());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Accessoire de boîtier", productList[i].getAccessoireBoitier()),
                            "Accessoire de boîtier",
                            productList[i].getAccessoireBoitier()
                        );

                }
                if (productList[i].getVentilateurBoitier() != "")
                {
                    databaseManager.InsertComposants("Ventilateurs", productList[i].getVentilateurBoitier());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Ventilateurs", productList[i].getVentilateurBoitier()),
                            "Ventilateurs",
                            productList[i].getVentilateurBoitier()
                        );

                }
                if (productList[i].getProcesseur() != "")
                {
                    databaseManager.InsertComposants("Processeur", productList[i].getProcesseur());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Processeur", productList[i].getProcesseur()),
                            "Processeur",
                            productList[i].getProcesseur()
                        );

                }
                if (productList[i].getVentirad() != "")
                {
                    databaseManager.InsertComposants("Ventirad", productList[i].getVentirad());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Ventirad", productList[i].getVentirad()),
                            "Ventirad",
                            productList[i].getVentirad()
                        );

                }
                if (productList[i].getWaterCooling() != "")
                {
                    databaseManager.InsertComposants("Watercooling", productList[i].getWaterCooling());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Watercooling", productList[i].getWaterCooling()),
                            "Watercooling",
                            productList[i].getWaterCooling()
                        );

                }
                if (productList[i].getCarteMere() != "")
                {
                    databaseManager.InsertComposants("Carte mère", productList[i].getCarteMere());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Carte mère", productList[i].getCarteMere()),
                            "Carte mère",
                            productList[i].getCarteMere()
                        );

                }
                if (productList[i].getCarteGraphique() != "")
                {
                    databaseManager.InsertComposants("Carte Graphique", productList[i].getCarteGraphique());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Carte Graphique", productList[i].getCarteGraphique()),
                            "Carte Graphique",
                            productList[i].getCarteGraphique()
                        );

                }
                if (productList[i].getAccessoireCarteGraphique() != "")
                {
                    databaseManager.InsertComposants("Accessoire de Carte Graphique", productList[i].getAccessoireCarteGraphique());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Accessoire de Carte Graphique", productList[i].getAccessoireCarteGraphique()),
                            "Accessoire de Carte Graphique",
                            productList[i].getAccessoireCarteGraphique()
                        );

                }
                if (productList[i].getRam() != "")
                {
                    databaseManager.InsertComposants("Mémoire vive", productList[i].getRam());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Mémoire vive", productList[i].getRam()),
                            "Mémoire vive",
                            productList[i].getRam()
                        );

                }
                if (productList[i].getDisqueSsd() != "")
                {
                    databaseManager.InsertComposants("SSD", productList[i].getDisqueSsd());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("SSD", productList[i].getDisqueSsd()),
                            "SSD",
                            productList[i].getDisqueSsd()
                        );

                }
                if (productList[i].getDisqueSupplementaire() != "")
                {
                    databaseManager.InsertComposants("HDD", productList[i].getDisqueSupplementaire());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("HDD", productList[i].getDisqueSupplementaire()),
                            "HDD",
                            productList[i].getDisqueSupplementaire()
                        );

                }
                if (productList[i].getCarteReseau() != "")
                {
                    databaseManager.InsertComposants("Carte réseau", productList[i].getCarteReseau());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Carte réseau", productList[i].getCarteReseau()),
                            "Carte réseau",
                            productList[i].getCarteReseau()
                        );

                }
                if (productList[i].getAlimentation() != "")
                {
                    databaseManager.InsertComposants("Alimentation", productList[i].getAlimentation());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Alimentation", productList[i].getAlimentation()),
                            "Alimentation",
                            productList[i].getAlimentation()
                        );

                }
                if (productList[i].getAccessoireAlimentation() != "")
                {
                    databaseManager.InsertComposants("Accessoire alimentation", productList[i].getAccessoireAlimentation());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Accessoire alimentation", productList[i].getAccessoireAlimentation()),
                            "Accessoire alimentation",
                            productList[i].getAccessoireAlimentation()
                        );

                }
                if (productList[i].getSystemeExploitation() != "")
                {
                    databaseManager.InsertComposants("Système exploitation", productList[i].getSystemeExploitation());
                    databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                            productList[i].getName(),
                            databaseManager.GetLastIDPcGamerComposants("Système exploitation", productList[i].getSystemeExploitation()),
                            "Système exploitation",
                            productList[i].getSystemeExploitation()
                        );

                }

            }
        }

        public void CompareComposantsWithWeb(List<int> IdsPCSaved, List<int> IdsPCWeb, List<Models.PcGamer> productList)
        {
            for (int i = 0; i < IdsPCWeb.Count; i++)
            {

                if (IdsPCSaved.Contains(IdsPCWeb[i]))
                {
                    Models.PcGamer PcGamerSaved = new();
                    Models.PcGamer PcWeb = new();
                    PcGamerSaved = databaseManager.SelectPcGamerByID(IdsPCWeb[i]);

                    for (int j = 0; j < productList.Count; j++)
                    {
                        if (productList[j].getIdConfig() == IdsPCWeb[i])
                        {
                            PcWeb = productList[j];
                        }
                    }

                    //Si une caractéristique a changé, il faut archiver la config et mettre à jour la config actuelle.
                    if (PcGamerSaved.getName() != PcWeb.getName())
                    {
                        databaseManager.UpdatePcGamerByID(IdsPCWeb[i], "Name", PcWeb.getName());
                    }
                    if (PcGamerSaved.getPrix() != PcWeb.getPrix())
                    {
                        databaseManager.UpdatePcGamerByID(IdsPCWeb[i], "Prix", PcWeb.getPrix().ToString());
                    }
                    if (PcGamerSaved.getPrixBarre() != PcWeb.getPrixBarre())
                    {
                        databaseManager.UpdatePcGamerByID(IdsPCWeb[i], "PrixBarre", PcWeb.getPrixBarre().ToString());
                    }
                    //Test
                    //PcWeb.setBoitier("Caca");
                    if (PcGamerSaved.getBoitier() != PcWeb.getBoitier())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getBoitier());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le boîtier \"" + PcGamerSaved.getBoitier() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getBoitier() + "\"\r"
                                + "Ce boîtier existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Boîtier", PcWeb.getBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("Le boîtier " + PcGamerSaved.getBoitier() + " a changé. Le nouveau est : " + PcWeb.getBoitier() + "\r"
                                + "Ce boîtier n'existe pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Boîtier", PcWeb.getBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getBoitier());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Boîtier", PcWeb.getBoitier());
                        }
                    }
                    if (PcGamerSaved.getAccessoireBoitier() != PcWeb.getAccessoireBoitier())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireBoitier());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("L'accessoire de boîtier \"" + PcGamerSaved.getAccessoireBoitier() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getAccessoireBoitier() + "\"\r"
                                + "Cet accessoire de boîtier existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de boîtier", PcWeb.getAccessoireBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("L'Accessoire de boîtier " + PcGamerSaved.getAccessoireBoitier() + " a changé. Le nouveau est : " + PcWeb.getAccessoireBoitier() + "\r"
                                + "Cet accessoire de boîtier n'existe pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire de boîtier", PcWeb.getAccessoireBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireBoitier());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de boîtier", PcWeb.getAccessoireBoitier());
                        }
                    }
                    if (PcGamerSaved.getVentilateurBoitier() != PcWeb.getVentilateurBoitier())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getVentilateurBoitier());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Les ventilateurs de boîtier \"" + PcGamerSaved.getVentilateurBoitier() + "\" ont changé dans la config \"" + PcGamerSaved.getName() + "\". Les nouveaux sont : \"" + PcWeb.getVentilateurBoitier() + "\"\r"
                                + "Ces ventilateurs de boîtier existent déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventilateurs", PcWeb.getVentilateurBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("Les ventilateurs de boîtier " + PcGamerSaved.getVentilateurBoitier() + " ont changé. Les nouveaux sont : " + PcWeb.getVentilateurBoitier() + "\r"
                                + "Ces ventilateurs de boîtier n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Ventilateurs", PcWeb.getVentilateurBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getVentilateurBoitier());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventilateurs", PcWeb.getVentilateurBoitier());
                        }
                    }
                    if (PcGamerSaved.getProcesseur() != PcWeb.getProcesseur())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getProcesseur());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le Processeur \"" + PcGamerSaved.getProcesseur() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getProcesseur() + "\"\r"
                                + "Ce Processeur existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Processeur", PcWeb.getProcesseur());
                        }
                        else
                        {
                            Debug.WriteLine("Le Processeur " + PcGamerSaved.getProcesseur() + " a changé. Le nouveau est : " + PcWeb.getProcesseur() + "\r"
                                + "Ce Processeur n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Processeur", PcWeb.getProcesseur());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getProcesseur());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Processeur", PcWeb.getProcesseur());
                        }
                    }
                    if (PcGamerSaved.getVentirad() != PcWeb.getVentirad())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getVentirad());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le Ventirad \"" + PcGamerSaved.getVentirad() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getVentirad() + "\"\r"
                                + "Ce Ventirad existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventirad", PcWeb.getVentirad());
                        }
                        else
                        {
                            Debug.WriteLine("Le Ventirad " + PcGamerSaved.getVentirad() + " a changé. Le nouveau est : " + PcWeb.getVentirad() + "\r"
                                + "Ce Ventirad n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Ventirad", PcWeb.getVentirad());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getVentirad());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventirad", PcWeb.getVentirad());
                        }
                    }
                    if (PcGamerSaved.getWaterCooling() != PcWeb.getWaterCooling())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getWaterCooling());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le Watercooling \"" + PcGamerSaved.getWaterCooling() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getWaterCooling() + "\"\r"
                                + "Ce Watercooling existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Watercooling", PcWeb.getWaterCooling());
                        }
                        else
                        {
                            Debug.WriteLine("Le Watercooling " + PcGamerSaved.getWaterCooling() + " a changé. Le nouveau est : " + PcWeb.getWaterCooling() + "\r"
                                + "Ce Watercooling n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Watercooling", PcWeb.getWaterCooling());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getWaterCooling());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Watercooling", PcWeb.getWaterCooling());
                        }
                    }
                    if (PcGamerSaved.getCarteMere() != PcWeb.getCarteMere())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteMere());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("La Carte mère \"" + PcGamerSaved.getCarteMere() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". La nouvelle est : \"" + PcWeb.getCarteMere() + "\"\r"
                                + "Cette Carte mère existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte mère", PcWeb.getCarteMere());
                        }
                        else
                        {
                            Debug.WriteLine("La Carte mère " + PcGamerSaved.getCarteMere() + " a changé. La nouvelle est : " + PcWeb.getCarteMere() + "\r"
                                + "Cette Carte mère n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte mère", PcWeb.getCarteMere());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteMere());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte mère", PcWeb.getCarteMere());
                        }
                    }
                    if (PcGamerSaved.getCarteGraphique() != PcWeb.getCarteGraphique())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteGraphique());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("La Carte Graphique \"" + PcGamerSaved.getCarteGraphique() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". La nouvelle est : \"" + PcWeb.getCarteGraphique() + "\"\r"
                                + "Cette Carte Graphique existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte Graphique", PcWeb.getCarteGraphique());
                        }
                        else
                        {
                            Debug.WriteLine("La Carte Graphique " + PcGamerSaved.getCarteGraphique() + " a changé. La nouvelle est : " + PcWeb.getCarteGraphique() + "\r"
                                + "Cette Carte Graphique n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte Graphique", PcWeb.getCarteGraphique());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteGraphique());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte Graphique", PcWeb.getCarteGraphique());
                        }
                    }
                    if (PcGamerSaved.getAccessoireCarteGraphique() != PcWeb.getAccessoireCarteGraphique())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireCarteGraphique());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("L'accessoire de Carte Graphique \"" + PcGamerSaved.getAccessoireCarteGraphique() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getAccessoireCarteGraphique() + "\"\r"
                                + "Cet accessoire de Carte Graphique existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de Carte Graphique", PcWeb.getAccessoireCarteGraphique());
                        }
                        else
                        {
                            Debug.WriteLine("L'accessoire de Carte Graphique " + PcGamerSaved.getAccessoireCarteGraphique() + " a changé. Le nouveau est : " + PcWeb.getAccessoireCarteGraphique() + "\r"
                                + "Cet accessoire de Carte Graphique n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire de Carte Graphique", PcWeb.getAccessoireCarteGraphique());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireCarteGraphique());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de Carte Graphique", PcWeb.getAccessoireCarteGraphique());
                        }
                    }
                    if (PcGamerSaved.getRam() != PcWeb.getRam())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getRam());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("La mémoire vive \"" + PcGamerSaved.getRam() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". La nouvelle est : \"" + PcWeb.getRam() + "\"\r"
                                + "Cette mémoire vive existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Mémoire vive", PcWeb.getRam());
                        }
                        else
                        {
                            Debug.WriteLine("La mémoire vive " + PcGamerSaved.getRam() + " a changé. La nouvelle est : " + PcWeb.getRam() + "\r"
                                + "Cette mémoire vive n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Mémoire vive", PcWeb.getRam());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getRam());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Mémoire vive", PcWeb.getRam());
                        }
                    }
                    if (PcGamerSaved.getDisqueSsd() != PcWeb.getDisqueSsd())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSsd());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le SSD \"" + PcGamerSaved.getDisqueSsd() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getDisqueSsd() + "\"\r"
                                + "Ce SSD existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "SSD", PcWeb.getDisqueSsd());
                        }
                        else
                        {
                            Debug.WriteLine("Le SSD " + PcGamerSaved.getDisqueSsd() + " a changé. Le nouveau est : " + PcWeb.getDisqueSsd() + "\r"
                                + "Ce SSD n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("SSD", PcWeb.getDisqueSsd());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSsd());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getDisqueSsd(), composantId, "SSD", PcWeb.getDisqueSsd());
                        }
                    }
                    if (PcGamerSaved.getDisqueSupplementaire() != PcWeb.getDisqueSupplementaire())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSupplementaire());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le HDD \"" + PcGamerSaved.getDisqueSupplementaire() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getDisqueSupplementaire() + "\"\r"
                                + "Ce HDD existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "HDD", PcWeb.getDisqueSupplementaire());
                        }
                        else
                        {
                            Debug.WriteLine("Le HDD " + PcGamerSaved.getDisqueSupplementaire() + " a changé. Le nouveau est : " + PcWeb.getDisqueSupplementaire() + "\r"
                                + "Ce HDD n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("HDD", PcWeb.getDisqueSupplementaire());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSupplementaire());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getDisqueSupplementaire(), composantId, "HDD", PcWeb.getDisqueSupplementaire());
                        }
                    }
                    if (PcGamerSaved.getCarteReseau() != PcWeb.getCarteReseau())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteReseau());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("La carte réseau \"" + PcGamerSaved.getCarteReseau() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". La nouvelle est : \"" + PcWeb.getCarteReseau() + "\"\r"
                                + "Cette carte réseau existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte réseau", PcWeb.getCarteReseau());
                        }
                        else
                        {
                            Debug.WriteLine("La carte réseau " + PcGamerSaved.getCarteReseau() + " a changé. La nouvelle est : " + PcWeb.getCarteReseau() + "\r"
                                + "Cette carte réseau n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte réseau", PcWeb.getCarteReseau());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteReseau());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getCarteReseau(), composantId, "Carte réseau", PcWeb.getCarteReseau());
                        }
                    }
                    if (PcGamerSaved.getAlimentation() != PcWeb.getAlimentation())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getAlimentation());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("L'alimentation \"" + PcGamerSaved.getAlimentation() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". La nouvelle est : \"" + PcWeb.getAlimentation() + "\"\r"
                                + "Cette alimentation existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Alimentation", PcWeb.getAlimentation());
                        }
                        else
                        {
                            Debug.WriteLine("L'alimentation " + PcGamerSaved.getAlimentation() + " a changé. La nouvelle est : " + PcWeb.getAlimentation() + "\r"
                                + "Cette alimentation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Alimentation", PcWeb.getAlimentation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAlimentation());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getAlimentation(), composantId, "Alimentation", PcWeb.getAlimentation());
                        }
                    }
                    if (PcGamerSaved.getAccessoireAlimentation() != PcWeb.getAccessoireAlimentation())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireAlimentation());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("L'accessoire d'alimentation \"" + PcGamerSaved.getAccessoireAlimentation() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getAccessoireAlimentation() + "\"\r"
                                + "Cet accessoire d'alimentation existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire alimentation", PcWeb.getAccessoireAlimentation());
                        }
                        else
                        {
                            Debug.WriteLine("L'accessoire d'alimentation " + PcGamerSaved.getAccessoireAlimentation() + " a changé. Le nouveau est : " + PcWeb.getAccessoireAlimentation() + "\r"
                                + "Cet Accessoire d'alimentation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire alimentation", PcWeb.getAccessoireAlimentation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireAlimentation());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getAccessoireAlimentation(), composantId, "Accessoire alimentation", PcWeb.getAccessoireAlimentation());
                        }
                    }
                    if (PcGamerSaved.getSystemeExploitation() != PcWeb.getSystemeExploitation())
                    {
                        int composantId = databaseManager.FindComposantIdByName(PcWeb.getSystemeExploitation());
                        if (composantId != -1)
                        {
                            Debug.WriteLine("Le Système d'Exploitation \"" + PcGamerSaved.getSystemeExploitation() + "\" a changé dans la config \"" + PcGamerSaved.getName() + "\". Le nouveau est : \"" + PcWeb.getSystemeExploitation() + "\"\r"
                                + "Ce Système d'Exploitation existe déjà dans la base de données sous l'ID : " + composantId);
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Système Exploitation", PcWeb.getSystemeExploitation());
                        }
                        else
                        {
                            Debug.WriteLine("Le Système d'Exploitation " + PcGamerSaved.getSystemeExploitation() + " a changé. Le nouveau est : " + PcWeb.getSystemeExploitation() + "\r"
                                + "Ce Système d'Exploitation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Système Exploitation", PcWeb.getSystemeExploitation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getSystemeExploitation());
                            databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getSystemeExploitation(), composantId, "Système Exploitation", PcWeb.getSystemeExploitation());
                        }
                    }

                }
            }
        }

        public void CompareWebwithDbAndInsertNews(List<Models.PcGamer> productsList, List<int> IdsPCSaved)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                if (!IdsPCSaved.Contains(productList[i].getIdConfig()))
                {
                    Debug.WriteLine("Le PC \"" + productList[i].getName() + "\" n'existe pas dans la base de données. Ajout à la base de donnée... \r");
                    databaseManager.InsertPCGamer(
                            productList[i].getIdConfig(),
                            productList[i].getName(),
                            productList[i].getPrix().ToString(),
                            productList[i].getPrixBarre().ToString(),
                            productList[i].getWebLink(),
                            DateTime.Now.Date);
                    if (productList[i].getBoitier() != "")
                    {
                        databaseManager.InsertComposants("Boîtier", productList[i].getBoitier());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Boîtier", productList[i].getBoitier()),
                                "Boîtier",
                                productList[i].getBoitier()
                            );
                    }

                    if (productList[i].getAccessoireBoitier() != "")
                    {
                        databaseManager.InsertComposants("Accessoire de boîtier", productList[i].getAccessoireBoitier());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire de boîtier", productList[i].getAccessoireBoitier()),
                                "Accessoire de boîtier",
                                productList[i].getAccessoireBoitier()
                            );

                    }
                    if (productList[i].getVentilateurBoitier() != "")
                    {
                        databaseManager.InsertComposants("Ventilateurs", productList[i].getVentilateurBoitier());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Ventilateurs", productList[i].getVentilateurBoitier()),
                                "Ventilateurs",
                                productList[i].getVentilateurBoitier()
                            );

                    }
                    if (productList[i].getProcesseur() != "")
                    {
                        databaseManager.InsertComposants("Processeur", productList[i].getProcesseur());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Processeur", productList[i].getProcesseur()),
                                "Processeur",
                                productList[i].getProcesseur()
                            );

                    }
                    if (productList[i].getVentirad() != "")
                    {
                        databaseManager.InsertComposants("Ventirad", productList[i].getVentirad());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Ventirad", productList[i].getVentirad()),
                                "Ventirad",
                                productList[i].getVentirad()
                            );

                    }
                    if (productList[i].getWaterCooling() != "")
                    {
                        databaseManager.InsertComposants("Watercooling", productList[i].getWaterCooling());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Watercooling", productList[i].getWaterCooling()),
                                "Watercooling",
                                productList[i].getWaterCooling()
                            );

                    }
                    if (productList[i].getCarteMere() != "")
                    {
                        databaseManager.InsertComposants("Carte mère", productList[i].getCarteMere());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte mère", productList[i].getCarteMere()),
                                "Carte mère",
                                productList[i].getCarteMere()
                            );

                    }
                    if (productList[i].getCarteGraphique() != "")
                    {
                        databaseManager.InsertComposants("Carte Graphique", productList[i].getCarteGraphique());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte Graphique", productList[i].getCarteGraphique()),
                                "Carte Graphique",
                                productList[i].getCarteGraphique()
                            );

                    }
                    if (productList[i].getAccessoireCarteGraphique() != "")
                    {
                        databaseManager.InsertComposants("Accessoire de Carte Graphique", productList[i].getAccessoireCarteGraphique());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire de Carte Graphique", productList[i].getAccessoireCarteGraphique()),
                                "Accessoire de Carte Graphique",
                                productList[i].getAccessoireCarteGraphique()
                            );

                    }
                    if (productList[i].getRam() != "")
                    {
                        databaseManager.InsertComposants("Mémoire vive", productList[i].getRam());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Mémoire vive", productList[i].getRam()),
                                "Mémoire vive",
                                productList[i].getRam()
                            );

                    }
                    if (productList[i].getDisqueSsd() != "")
                    {
                        databaseManager.InsertComposants("SSD", productList[i].getDisqueSsd());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("SSD", productList[i].getDisqueSsd()),
                                "SSD",
                                productList[i].getDisqueSsd()
                            );

                    }
                    if (productList[i].getDisqueSupplementaire() != "")
                    {
                        databaseManager.InsertComposants("HDD", productList[i].getDisqueSupplementaire());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("HDD", productList[i].getDisqueSupplementaire()),
                                "HDD",
                                productList[i].getDisqueSupplementaire()
                            );

                    }
                    if (productList[i].getCarteReseau() != "")
                    {
                        databaseManager.InsertComposants("Carte réseau", productList[i].getCarteReseau());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte réseau", productList[i].getCarteReseau()),
                                "Carte réseau",
                                productList[i].getCarteReseau()
                            );

                    }
                    if (productList[i].getAlimentation() != "")
                    {
                        databaseManager.InsertComposants("Alimentation", productList[i].getAlimentation());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Alimentation", productList[i].getAlimentation()),
                                "Alimentation",
                                productList[i].getAlimentation()
                            );

                    }
                    if (productList[i].getAccessoireAlimentation() != "")
                    {
                        databaseManager.InsertComposants("Accessoire alimentation", productList[i].getAccessoireAlimentation());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire alimentation", productList[i].getAccessoireAlimentation()),
                                "Accessoire alimentation",
                                productList[i].getAccessoireAlimentation()
                            );

                    }
                    if (productList[i].getSystemeExploitation() != "")
                    {
                        databaseManager.InsertComposants("Système exploitation", productList[i].getSystemeExploitation());
                        databaseManager.InsertPcGamerComposants(productList[i].getIdConfig(),
                                productList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Système exploitation", productList[i].getSystemeExploitation()),
                                "Système exploitation",
                                productList[i].getSystemeExploitation()
                            );

                    }
                }
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

        private void OpenURLinNav(object sender, RoutedEventArgs e)
        {
            string IdJajaSelected = ConfigGroupDataGrid.SelectedItem.ToString();
            List<long> IdJajaSelectedList = new List<long>();
            IdJajaSelectedList = FindIdNumbers(IdJajaSelected);
            IdJajaSelected = IdJajaSelectedList.First().ToString();
            Models.PcGamer product = new();
            product = databaseManager.SelectPcGamerByID(int.Parse(IdJajaSelected));
            
            string url = product.getWebLink();
            var sInfo = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }

        private void CraftPoster(string Libelle, string Boitier, string CarteMere, string Processeur, string RAM, string CG, string Stockage, string Alim, string OS, string PrixBarre, string Prix)
        {
            Debug.WriteLine("Création de l'affiche...");
            string PatronAffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            string AffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp";
            string LogoPath = AppDomain.CurrentDomain.BaseDirectory + "Img\\LogoCybertek.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(PatronAffichePath);
            Bitmap bitmapLogo = (Bitmap)System.Drawing.Image.FromFile(LogoPath);

            StringFormat IntroFormat = new();
            IntroFormat.Alignment = StringAlignment.Center;
            IntroFormat.LineAlignment = StringAlignment.Near;
            Rectangle rectIntro = new(0, 55, bitmap.Width, 40);

            StringFormat LibelleFormat = new();
            LibelleFormat.Alignment = StringAlignment.Center;
            LibelleFormat.LineAlignment = StringAlignment.Near;
            Rectangle rectLibelle = new(0, 94, bitmap.Width, 100);

            StringFormat CategorieBoitierFormat = new();
            CategorieBoitierFormat.Alignment = StringAlignment.Far;
            CategorieBoitierFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieBoitier = new(-110, 225, bitmap.Width/2, 40);

            StringFormat CategorieCMFormat = new();
            CategorieCMFormat.Alignment = StringAlignment.Far;
            CategorieCMFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieCM = new(-110, 295, bitmap.Width / 2, 40);

            StringFormat CategorieProcFormat = new();
            CategorieProcFormat.Alignment = StringAlignment.Far;
            CategorieProcFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieProc = new(-110, 365, bitmap.Width / 2, 40);

            StringFormat CategorieRamFormat = new();
            CategorieRamFormat.Alignment = StringAlignment.Far;
            CategorieRamFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieRam = new(-110, 435, bitmap.Width / 2, 40);

            StringFormat CategorieCGFormat = new();
            CategorieCGFormat.Alignment = StringAlignment.Far;
            CategorieCGFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieCG = new(-110, 505, bitmap.Width / 2, 40);

            StringFormat CategorieStockageFormat = new();
            CategorieStockageFormat.Alignment = StringAlignment.Far;
            CategorieStockageFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieStockage = new(-110, 575, bitmap.Width / 2, 40);

            StringFormat CategorieAlimFormat = new();
            CategorieAlimFormat.Alignment = StringAlignment.Far;
            CategorieAlimFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieAlim = new(-110, 645, bitmap.Width / 2, 40);

            StringFormat CategorieOSFormat = new();
            CategorieOSFormat.Alignment = StringAlignment.Far;
            CategorieOSFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieOS = new(-110, 715, bitmap.Width / 2, 40);

            StringFormat CaracBoitierFormat = new();
            CaracBoitierFormat.Alignment = StringAlignment.Near;
            CaracBoitierFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracBoitier = new(320, 225, bitmap.Width / 2, 40);

            StringFormat CaracCMFormat = new();
            CaracCMFormat.Alignment = StringAlignment.Near;
            CaracCMFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracCM = new(320, 295, bitmap.Width / 2, 40);

            StringFormat CaracProcFormat = new();
            CaracProcFormat.Alignment = StringAlignment.Near;
            CaracProcFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracProc = new(320, 365, bitmap.Width / 2, 40);

            StringFormat CaracRamFormat = new();
            CaracRamFormat.Alignment = StringAlignment.Near;
            CaracRamFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracRam = new(320, 435, bitmap.Width / 2, 40);

            StringFormat CaracCGFormat = new();
            CaracCGFormat.Alignment = StringAlignment.Near;
            CaracCGFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracCG = new(320, 505, bitmap.Width / 2, 40);

            StringFormat CaracStockageFormat = new();
            CaracStockageFormat.Alignment = StringAlignment.Near;
            CaracStockageFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracStockage = new(320, 575, bitmap.Width / 2, 40);

            StringFormat CaracAlimFormat = new();
            CaracAlimFormat.Alignment = StringAlignment.Near;
            CaracAlimFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracAlim = new(320, 645, bitmap.Width / 2, 40);

            StringFormat CaracOSFormat = new();
            CaracOSFormat.Alignment = StringAlignment.Near;
            CaracOSFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracOS = new(320, 715, bitmap.Width / 2, 40);

            StringFormat PrixBarreFormat = new();
            PrixBarreFormat.Alignment = StringAlignment.Center;
            PrixBarreFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrixBarre = new(0, 785, bitmap.Width, 60);

            StringFormat PrixFormat = new();
            PrixFormat.Alignment = StringAlignment.Center;
            PrixFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrix = new(0, 870, bitmap.Width, 100);

            Rectangle RectLogo = new(bitmap.Width / 2 - (bitmapLogo.Width*9/10) / 2, 1020, bitmapLogo.Width*9/10, bitmapLogo.Height*9/10);

            if(OS == "")
            {
                OS = "149 € l'installation";
            }
            if (CG == "")
            {
                CG = "Chipset Graphique";
            }

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                PrivateFontCollection privateFonts = new PrivateFontCollection(); 
                privateFonts.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\CenturyGothic.ttf");
                privateFonts.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\Mass_Effect.ttf");
                privateFonts.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\Boombox.ttf");

                Style test = new();
                test.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.UltraBold));

                Font IntroFont = new(privateFonts.Families[1], 25);
                Font NameFont = new(privateFonts.Families[0], 47);
                Font CategorieFont = new(privateFonts.Families[1], 14, System.Drawing.FontStyle.Regular);
                Font CaracFont = new(privateFonts.Families[1], 20, System.Drawing.FontStyle.Bold);
                Font PrixBarreFont = new(privateFonts.Families[1], 30, System.Drawing.FontStyle.Strikeout);
                Font PrixFont = new(privateFonts.Families[1], 65, System.Drawing.FontStyle.Bold);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                
                graphics.DrawString("PC monté", IntroFont, Brushes.Black, rectIntro, IntroFormat);
                graphics.DrawString(Libelle, NameFont, Brushes.Black, rectLibelle, LibelleFormat);

                graphics.DrawString("Boîtier", CategorieFont, Brushes.Gray, rectCategorieBoitier, CategorieBoitierFormat);
                graphics.DrawString("Carte mère", CategorieFont, Brushes.Gray, rectCategorieCM, CategorieCMFormat);
                graphics.DrawString("Processeur", CategorieFont, Brushes.Gray, rectCategorieProc, CategorieProcFormat);
                graphics.DrawString("Mémoire vive", CategorieFont, Brushes.Gray, rectCategorieRam, CategorieRamFormat);
                graphics.DrawString("Carte graphique", CategorieFont, Brushes.Gray, rectCategorieCG, CategorieCGFormat);
                graphics.DrawString("Stockage", CategorieFont, Brushes.Gray, rectCategorieStockage, CategorieStockageFormat);
                graphics.DrawString("Alimentation", CategorieFont, Brushes.Gray, rectCategorieAlim, CategorieAlimFormat);
                graphics.DrawString("Système d'exploitation", CategorieFont, Brushes.Gray, rectCategorieOS, CategorieOSFormat);

                graphics.DrawString(Boitier, CaracFont, Brushes.Black, rectCaracBoitier, CaracBoitierFormat);
                graphics.DrawString(CarteMere, CaracFont, Brushes.Black, rectCaracCM, CaracCMFormat);
                graphics.DrawString(Processeur, CaracFont, Brushes.Black, rectCaracProc, CaracProcFormat);
                graphics.DrawString(RAM, CaracFont, Brushes.Black, rectCaracRam, CaracRamFormat);
                graphics.DrawString(CG, CaracFont, Brushes.Black, rectCaracCG, CaracCGFormat);
                graphics.DrawString(Stockage, CaracFont, Brushes.Black, rectCaracStockage, CaracStockageFormat);
                graphics.DrawString(Alim, CaracFont, Brushes.Black, rectCaracAlim, CaracAlimFormat);
                graphics.DrawString(OS, CaracFont, Brushes.Black, rectCaracOS, CaracOSFormat);

                if(PrixBarre != "0")
                {
                    graphics.DrawString(PrixBarre + " €", PrixBarreFont, Brushes.Gray, rectPrixBarre, PrixBarreFormat);
                }else
                {
                    PrixFormat.Alignment = StringAlignment.Center;
                    PrixFormat.LineAlignment = StringAlignment.Center;
                    rectPrix = new(0, 840, bitmap.Width, 100);
                }
                
                graphics.DrawString(Prix + " €", PrixFont, Brushes.Red, rectPrix, PrixFormat);

                graphics.DrawImage(bitmapLogo, RectLogo);

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
            
        }

        private void LineClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg == null)
                return;
            if (dg.RowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.VisibleWhenSelected)
            {
                dg.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

            }
            else
            {
                dg.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }

        private void UpdateDataGrid(object sender, RoutedEventArgs e)
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

    }

        
    }
