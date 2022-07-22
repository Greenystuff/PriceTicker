using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Cache;
using System.Text.RegularExpressions;
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
        ServerSocket socketServer;
        XmlReader xmlReader = new();
        TicketCrafter ticketCrafter = new();

        public MainWindow()
        {
            InitializeComponent();
            gui = this;
            socketServer = new();
            socketServer.SetupServer();
            ticketCrafter.InitUi();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ValiderRechercheMurale_Click(object sender, RoutedEventArgs e)
        {
            if(!IdRecherche.Text.ToString().Equals(""))
            {
                xmlReader.FindPriceById(IdRecherche.Text, "Murale");
            }else
            {
                Debug.WriteLine("Champ ID mural vide");
            }
        }

        private void ValiderRechercheRail_Click(object sender, RoutedEventArgs e)
        {
            if (!RailIdRecherche.Text.ToString().Equals(""))
            {
                xmlReader.FindPriceById(RailIdRecherche.Text, "Rail");
            }
            else
            {
                Debug.WriteLine("Champ ID rail vide");
            }

        }

        private void ShowPreviousPage(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setPreviousWallTicketPage();
        }

        private void ShowNextPage(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setNextWallTicketPage();
        }

        private void ShowPreviousRailPage(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setPreviousRailTicketPage();
        }

        private void ShowRailNextPage(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setNextRailTicketPage();
        }

        private void ResetWallDatas(object sender, RoutedEventArgs e)
        {
            ticketCrafter.ResetWallDatas();
        }

        private void ResetRailDatas(object sender, RoutedEventArgs e)
        {
            ticketCrafter.ResetRailDatas();
        }

        private void PrintWallPages(object sender, RoutedEventArgs e)
        {

            // Selection de l'imprimante et des paramètres
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            // Creation du Document
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

            // Création des pages
            for (int i = 1; i <= Properties.Settings.Default.Pagenumber; i++)
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
            pd.PrintDocument(document.DocumentPaginator, "Étiquettes Murales");



        }

        private void PrintRailPages(object sender, RoutedEventArgs e)
        {

            // Selection de l'imprimante et des paramètres
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            // Creation du Document
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

            // Création des pages
            for (int i = 1; i <= Properties.Settings.Default.PagenumberRail; i++)
            {
                FixedPage page = new FixedPage();
                page.Width = document.DocumentPaginator.PageSize.Width;
                page.Height = document.DocumentPaginator.PageSize.Height;

                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + i + ".bmp";
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
            pd.PrintDocument(document.DocumentPaginator, "Étiquettes Rail");



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

            if (RailIdRecherche.Text.ToString().Equals(""))
            {
                RailValiderRecherche.IsEnabled = false;
            }
            else
            {
                RailValiderRecherche.IsEnabled = true;
            }
        }

        private void Params_Click(object sender, RoutedEventArgs e)
        {
            ParamsWindows paramsWindows = new ParamsWindows();
            paramsWindows.Owner = this;
            paramsWindows.Show();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            LogsWindow logsWindow = new LogsWindow();
            logsWindow.Owner = this;
            logsWindow.Show();
        }

        private void NouvelleAffiche(object sender, RoutedEventArgs e)
        {
            AfficheCreator creator = new AfficheCreator();
            creator.Owner = this;
            creator.Show();
        }

        private void ShowPreviousAffiche(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setPreviousAffiche();
        }

        private void ShowNextAffiche(object sender, RoutedEventArgs e)
        {
            ticketCrafter.setNextAffiche();
        }

        private void ResetAffichesDatas(object sender, RoutedEventArgs e)
        {
            ticketCrafter.ResetAffichesDatas();
        }
    }

}

