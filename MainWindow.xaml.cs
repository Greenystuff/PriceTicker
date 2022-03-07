using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace PriceTicker
{


    public partial class MainWindow : Window
    {
        public static MainWindow? gui;
        String? IdJaja = null;
        String? Libelle = null;
        String? Prix = null;
        int NbEtiquettes = 0;
        int OnPageNumber = 0;
        int nbColonnes = 0;
        int nbLignes = 0;
        int nbPages = 1;
        int pageSelected = 1;

        public MainWindow()
        {
            InitializeComponent();
            gui = this;
            ServerSocket socketServer = new();
            socketServer.SetupServer();
            InitUi();
        }

        private void InitUi()
        {
            NbEtiquettes = Properties.Settings.Default.nbEtiquettes;
            nbLignes = Properties.Settings.Default.nbLignes;
            nbColonnes = Properties.Settings.Default.nbColonnes;
            OnPageNumber = Properties.Settings.Default.OnPageNumber;
            nbPages = Properties.Settings.Default.Pagenumber;
            ValiderRecherche.IsEnabled = false;


            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + 1 + ".bmp"))
            {
                btnPrecedent.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Hidden;
                pageNumber.Visibility = Visibility.Hidden;
                nbPages = Properties.Settings.Default.Pagenumber;
                pageSelected = nbPages;
                pageNumber.Text = pageSelected.ToString();
                string LastPagePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
                Uri lastPageUri = new(LastPagePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                imgEtiquette.Source = _image;

            }
            else
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale2.bmp"))
                {
                    btnPrecedent.Visibility = Visibility.Hidden;
                    btnSuivant.Visibility = Visibility.Hidden;
                    pageNumber.Visibility = Visibility.Hidden;

                }
                else
                {
                    btnPrecedent.Visibility = Visibility.Visible;
                    btnSuivant.Visibility = Visibility.Hidden;
                    pageNumber.Visibility = Visibility.Visible;
                }

                nbPages = Properties.Settings.Default.Pagenumber;
                pageSelected = nbPages;
                pageNumber.Text = pageSelected.ToString();
                string LastPagePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + nbPages + ".bmp";
                Uri lastPageUri = new(LastPagePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                imgEtiquette.Source = _image;
            }
        }

        public void UpdateLogText(String Data, TextBox Target)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                if (Target.Text == "")
                {
                    Target.Text += Data ;
                }
                else
                {
                    Target.Text += "\n" + Data;
                }
                    
                
            }), DispatcherPriority.SystemIdle);
        }

        public void FindPriceById(String Id)
        {
            Dispatcher.BeginInvoke(new Action(() => {

                if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD"))
                {
                    Debug.WriteLine("Dossier JajaCD non trouvé");
                    MessageBox.Show("Veuillez Installer JajaCD ! (Ou alors le développer a enfin décidé de sécuriser son système et d'arrêter avec les XML...)");
                    return;
                }
                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD\\Temp_xml\\Produit.xml"))
                {
                    Debug.WriteLine("XML non trouvés");
                    MessageBox.Show("Veuillez lancer JajaCD pour mettre à jour les informations produit");
                    return;
                }

                Debug.WriteLine("Recherche de l'ID demandé dans les XML (ID => " + Id + " )");
                XmlTextReader ProduitsXml = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD\\Temp_xml\\Produit.xml");
                XmlTextReader PrixXml = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD\\Temp_xml\\Tarif_Produit.xml");


                String? Product_ID = null;

                while (ProduitsXml.Read())
                {

                    if (ProduitsXml.NodeType == XmlNodeType.Element && ProduitsXml.Name == "produit_id")
                    {
                        Product_ID = ProduitsXml.ReadElementContentAsString();
                    }

                    if (ProduitsXml.NodeType == XmlNodeType.Element && ProduitsXml.Name == "num_produit")
                    {

                        if (ProduitsXml.ReadElementContentAsString() == Id)
                        {
                            IdJaja = Id;
                            ProduitsXml.ReadToNextSibling("libelle");
                            Libelle = ProduitsXml.ReadElementContentAsString();
                            Debug.WriteLine("Libellé : " + Libelle);
                            break;
                        }
                        else
                        {
                            Product_ID = "";
                            IdJaja = null;
                            Prix = null;
                            Libelle = null;
                        }
                    }
                }
                if (IdJaja != null)
                {
                    while (PrixXml.Read())
                    {

                        if (PrixXml.NodeType == XmlNodeType.Element && PrixXml.Name == "tarif_id")
                        {
                            if (PrixXml.ReadElementContentAsString() == "7")
                            {
                                PrixXml.ReadToNextSibling("produit_id");

                                if (PrixXml.ReadElementContentAsString() == Product_ID)
                                {
                                    PrixXml.ReadToNextSibling("prix_ttc");

                                    Prix = PrixXml.ReadElementContentAsString() + " €";
                                    Debug.WriteLine("Prix : " + Prix);
                                    TicketCrafter(IdJaja, Libelle, Prix);
                                    break;
                                }
                            }
                        }
                    }
                }else
                {
                    Debug.WriteLine("Aucun produit trouvé pour l'ID Jaja demandé (ID demandé => " + Id + " )");
                }

            }), DispatcherPriority.SystemIdle);
           
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ValiderRecherche_Click(object sender, RoutedEventArgs e)
        {
            if(!IdRecherche.Text.ToString().Equals(""))
            {
                FindPriceById(IdRecherche.Text);
            }else
            {
                Debug.WriteLine("Champ ID vide");
            }
            
        }

        private void TicketCrafter(String IdJaja, String Libelle, String Prix)
        {
            Debug.WriteLine("Création de l'étiquette...");
            string PatronEtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_etiquette_murale.bmp";
            string EtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Etiquette.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(PatronEtiquettePath);

            StringFormat LibelleFormat = new();
            LibelleFormat.Alignment = StringAlignment.Center;
            LibelleFormat.LineAlignment = StringAlignment.Near;
            Rectangle rectLibelle = new(0, 5, bitmap.Width, 30);

            StringFormat PrixFormat = new();
            PrixFormat.Alignment = StringAlignment.Center;
            PrixFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrix = new(0, 5, bitmap.Width, 115);

            StringFormat IdFormat = new();
            IdFormat.Alignment = StringAlignment.Near;
            IdFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectId = new(0, 90, bitmap.Width, 30);

            StringFormat GarantieFormat = new();
            GarantieFormat.Alignment = StringAlignment.Far;
            GarantieFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectGarantie = new(0, 90, bitmap.Width, 30);


            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                String garantie = "Garantie 2 ans";
                using Font LibelleFont = new("Arial", 11);
                using Font PrixFont = new("Arial", 24);
                using Font IdFont = new("Arial", 12);
                using Font GarantieFont = new("Arial", 10);

                graphics.DrawString(Libelle, LibelleFont, Brushes.Black, rectLibelle, LibelleFormat);
                graphics.DrawString(Prix, PrixFont, Brushes.Red, rectPrix, PrixFormat);
                graphics.DrawString(IdJaja, IdFont, Brushes.Black, rectId, IdFormat);
                graphics.DrawString(garantie, GarantieFont, Brushes.Black, rectGarantie, GarantieFormat);
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(0, bitmap.Height));
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(bitmap.Width, 0));
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(214, 0), new System.Drawing.Point(214, 214));
                graphics.Dispose();
            }

            using (MemoryStream memory = new())
                {
                Bitmap bm = new(bitmap);
                bitmap.Dispose();
                using (FileStream fs = new(EtiquettePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                Debug.WriteLine("Étiquette crée avec succès !");


            }
            PagesCrafter();

        }

        private void PagesCrafter()
        {
            
            string FeuilleFilePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Etiquette.bmp";
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + nbPages + ".bmp";
            Bitmap bitmapFeuille = (Bitmap)System.Drawing.Image.FromFile(FeuilleFilePath);
            Bitmap bitmapEtiquette = (Bitmap)System.Drawing.Image.FromFile(imageFilePath);
            Rectangle Etiquette = new(nbColonnes * 215, nbLignes * 155, bitmapEtiquette.Width, bitmapEtiquette.Height);

            if(File.Exists(FeuilleFinalePath))
            {
                Debug.WriteLine("Mise à jour de la page " + nbPages);
                Bitmap bitmapFeuilleFinale = (Bitmap)System.Drawing.Image.FromFile(FeuilleFinalePath);
                using (Graphics graphics = Graphics.FromImage(bitmapFeuilleFinale))
                {
                    graphics.DrawImage(bitmapEtiquette, Etiquette);
                    graphics.Dispose();

                }
                
                using (MemoryStream memory = new())
                {
                    Bitmap bm = new(bitmapFeuilleFinale);
                    bitmapFeuilleFinale.Dispose();
                    using (FileStream fs = new(FeuilleFinalePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        bm.Save(memory, ImageFormat.Bmp);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                        fs.Dispose();
                        
                    }
                    memory.Close();
                    memory.Dispose();
                    bm.Dispose();
                }

                bitmapFeuilleFinale.Dispose();
                bitmapEtiquette.Dispose();
            }
            else
            {
                Debug.WriteLine("Création de la première page d'étiquettes");
                using (Graphics graphics = Graphics.FromImage(bitmapFeuille))
                {
                    graphics.DrawImage(bitmapEtiquette, Etiquette);
                    graphics.Dispose();

                }
                using Bitmap tempImage = new(bitmapFeuille);
                tempImage.Save(FeuilleFinalePath, ImageFormat.Bmp);
                tempImage.Dispose();
                bitmapFeuille.Dispose();
                bitmapEtiquette.Dispose();
            }

            NbEtiquettes++;
            OnPageNumber++;
            nbColonnes++;
            Properties.Settings.Default.nbColonnes = nbColonnes;
            Properties.Settings.Default.nbEtiquettes = NbEtiquettes;
            Properties.Settings.Default.OnPageNumber = OnPageNumber;
            Properties.Settings.Default.Save();
            if(nbColonnes == 3)
            {
                nbColonnes = 0;
                nbLignes++;
                Properties.Settings.Default.nbColonnes = nbColonnes;
                Properties.Settings.Default.nbLignes = nbLignes;
                Properties.Settings.Default.Save();
            }
            
            if(OnPageNumber == 21)
            {
                OnPageNumber = 0;
                nbPages++;
                pageSelected++;
                nbColonnes = 0;
                nbLignes = 0;
                Properties.Settings.Default.Pagenumber = nbPages;
                Properties.Settings.Default.nbColonnes = nbColonnes;
                Properties.Settings.Default.nbLignes = nbLignes;
                Properties.Settings.Default.Save();
            }
            if(NbEtiquettes > 21)
            {
                btnPrecedent.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Visible;
                pageNumber.Visibility = Visibility.Visible;
            }


            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            imgEtiquette.Source = _image;
            btnSuivant.Visibility=Visibility.Hidden;
            pageNumber.Text = pageSelected.ToString();

        }

        private void ShowPreviousPage(object sender, RoutedEventArgs e)
        {
            pageSelected--;
            pageNumber.Text = pageSelected.ToString();
            btnSuivant.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + pageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            imgEtiquette.Source = _image;
            if (pageSelected == 1)
            {
                btnPrecedent.Visibility = Visibility.Hidden;
            }
            

        }

        private void ShowNextPage(object sender, RoutedEventArgs e)
        {

            pageSelected++;
            pageNumber.Text = pageSelected.ToString();
            btnPrecedent.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + pageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            imgEtiquette.Source = _image;
            if (pageSelected == nbPages)
            {
                btnSuivant.Visibility = Visibility.Hidden;
            }

        }

        private void ResetDatas(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= nbPages; i++)
            {
                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + i + ".bmp";
                Debug.WriteLine("Page " + i + " effacée");
                File.Delete(FeuilleFinalePath);
            }
            Properties.Settings.Default.Pagenumber = 1;
            Properties.Settings.Default.nbColonnes = 0;
            Properties.Settings.Default.nbLignes = 0;
            Properties.Settings.Default.nbEtiquettes = 0;
            Properties.Settings.Default.Pagenumber = 1;
            Properties.Settings.Default.OnPageNumber = 0;
            Properties.Settings.Default.Save();
            nbPages = 1;
            nbColonnes = 0;
            nbLignes = 0;
            pageSelected = 1;
            NbEtiquettes = 0;
            OnPageNumber = 0;
            btnPrecedent.Visibility = Visibility.Hidden;
            btnSuivant.Visibility = Visibility.Hidden;
            pageNumber.Visibility = Visibility.Hidden;
            

            string FeuilleFilePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            Uri lastPageUri = new(FeuilleFilePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            imgEtiquette.Source = _image;

        }

        private void PrintPages(object sender, RoutedEventArgs e)
        {

            // Selection de l'imprimante et des paramètres
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            // Creation du Document
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

            // Création des pages
            for (int i = 1; i <= nbPages; i++)
            {
                FixedPage page = new FixedPage();
                page.Width = document.DocumentPaginator.PageSize.Width;
                page.Height = document.DocumentPaginator.PageSize.Height;

                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + i + ".bmp";
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();

                Uri PageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

                MemoryStream ms = new MemoryStream();
                BitmapImage bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.CacheOption = BitmapCacheOption.None;
                bImg.StreamSource = new MemoryStream(ms.ToArray());
                bImg.CacheOption = BitmapCacheOption.OnLoad;
                bImg.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bImg.UriSource = PageUri;
                bImg.EndInit();
                ms.Dispose();
                

                //img is an Image control.
                img.Source = bImg;
                page.Children.Add(img);

                // Ajout de la page au document
                PageContent pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(page);
                document.Pages.Add(pageContent);

            }

            // Lancer l'impression
            pd.PrintDocument(document.DocumentPaginator, "Étiquettes");



        }

        private void InputIdJaja_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IdRecherche.Text.ToString().Equals(""))
            {
                ValiderRecherche.IsEnabled = false;
            }else
            {
                ValiderRecherche.IsEnabled = true;
            }
        }
    }

}

