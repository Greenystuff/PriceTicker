using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PriceTicker
{
    internal class TicketCrafter
    {
        public void InitUi()
        {
            MainWindow.gui.ValiderRecherche.IsEnabled = false;
            MainWindow.gui.RailValiderRecherche.IsEnabled = false;
            
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + 1 + ".bmp"))
            {
                MainWindow.gui.btnImprimer.IsEnabled = false;
                MainWindow.gui.btnPrecedent.Visibility = Visibility.Hidden;
                MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
                MainWindow.gui.pageNumber.Visibility = Visibility.Hidden;
                MainWindow.gui.pageNumber.Text = Properties.Settings.Default.Pagenumber.ToString();
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
                MainWindow.gui.imgEtiquette.Source = _image;

            }
            else
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale2.bmp"))
                {
                    MainWindow.gui.btnPrecedent.Visibility = Visibility.Hidden;
                    MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.pageNumber.Visibility = Visibility.Hidden;

                }
                else
                {
                    MainWindow.gui.btnPrecedent.Visibility = Visibility.Visible;
                    MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.pageNumber.Visibility = Visibility.Visible;
                }

                MainWindow.gui.btnImprimer.IsEnabled = true;
                MainWindow.gui.pageNumber.Text = Properties.Settings.Default.Pagenumber.ToString();
                string LastPagePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + Properties.Settings.Default.Pagenumber + ".bmp";
                Uri lastPageUri = new(LastPagePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                MainWindow.gui.imgEtiquette.Source = _image;
            }

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + 1 + ".bmp"))
            {
                MainWindow.gui.RailbtnImprimer.IsEnabled = false;
                MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Hidden;
                MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
                MainWindow.gui.RailpageNumber.Visibility = Visibility.Hidden;
                MainWindow.gui.RailpageNumber.Text = Properties.Settings.Default.PagenumberRail.ToString();
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
                MainWindow.gui.RailimgEtiquette.Source = _image;

            }
            else
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail2.bmp"))
                {
                    MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Hidden;
                    MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.RailpageNumber.Visibility = Visibility.Hidden;

                }
                else
                {
                    MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Visible;
                    MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.RailpageNumber.Visibility = Visibility.Visible;
                }

                MainWindow.gui.RailbtnImprimer.IsEnabled = true;
                MainWindow.gui.RailpageNumber.Text = Properties.Settings.Default.RailPageSelected.ToString();
                string LastPagePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + Properties.Settings.Default.PagenumberRail + ".bmp";
                Uri lastPageUri = new(LastPagePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                MainWindow.gui.RailimgEtiquette.Source = _image;
            }

            Properties.Settings.Default.affichePageSelected = Properties.Settings.Default.nbrAffiches;
            Properties.Settings.Default.Save();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_1.bmp"))
            {
                MainWindow.gui.A4btnImprimer.IsEnabled = false;
                MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Hidden;
                MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
                MainWindow.gui.A4pageNumber.Visibility = Visibility.Hidden;
                MainWindow.gui.A4pageNumber.Text = Properties.Settings.Default.nbrAffiches.ToString();
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
                MainWindow.gui.A4imgEtiquette.Source = _image;
            }
            else
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_2.bmp"))
                {
                    MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Hidden;
                    MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.A4pageNumber.Visibility = Visibility.Hidden;
                }
                else
                {
                    MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Visible;
                    MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
                    MainWindow.gui.A4pageNumber.Visibility = Visibility.Visible;
                }

                MainWindow.gui.A4btnImprimer.IsEnabled = true;
                MainWindow.gui.A4pageNumber.Text = Properties.Settings.Default.nbrAffiches.ToString();
                string LastPagePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_" + Properties.Settings.Default.nbrAffiches + ".bmp";
                Uri lastPageUri = new(LastPagePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                MainWindow.gui.A4imgEtiquette.Source = _image;
            }
        }

        public void WallTicketCrafter(String IdJaja, String Libelle, String Prix)
        {
            Debug.WriteLine("Création de l'étiquette Murale...");
            string PatronEtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_etiquette_murale.bmp";
            string EtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Etiquette_Murale.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(PatronEtiquettePath);

            StringFormat LibelleFormat = new();
            LibelleFormat.Alignment = StringAlignment.Center;
            LibelleFormat.LineAlignment = StringAlignment.Near;
            Rectangle rectLibelle = new(0, 5, bitmap.Width, 35);

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

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

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
                Debug.WriteLine("Étiquette Murale crée avec succès !");


            }
            WallPagesCrafter(EtiquettePath);

        }

        public void RailTicketCrafter(String IdJaja, String Libelle, String Prix)
        {
            Debug.WriteLine("Création de l'étiquette pour Rail...");
            string PatronEtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_etiquette_rail.bmp";
            string EtiquettePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Etiquette_Rail.bmp";
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(PatronEtiquettePath);

            StringFormat LibelleFormat = new();
            LibelleFormat.Alignment = StringAlignment.Center;
            LibelleFormat.LineAlignment = StringAlignment.Near;
            Rectangle rectLibelle = new(0, 5, bitmap.Width, 30);

            StringFormat PrixFormat = new();
            PrixFormat.Alignment = StringAlignment.Center;
            PrixFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrix = new(0, 5, bitmap.Width, 75);

            StringFormat IdFormat = new();
            IdFormat.Alignment = StringAlignment.Near;
            IdFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectId = new(0, 48, bitmap.Width, 30);

            StringFormat GarantieFormat = new();
            GarantieFormat.Alignment = StringAlignment.Far;
            GarantieFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectGarantie = new(0, 48, bitmap.Width, 30);


            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                String garantie = "Garantie 2 ans";
                using Font LibelleFont = new("Arial", 8);
                using Font PrixFont = new("Arial", 20);
                using Font IdFont = new("Arial", 12);
                using Font GarantieFont = new("Arial", 10);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                graphics.DrawString(Libelle, LibelleFont, Brushes.Black, rectLibelle, LibelleFormat);
                graphics.DrawString(Prix, PrixFont, Brushes.Red, rectPrix, PrixFormat);
                graphics.DrawString(IdJaja, IdFont, Brushes.Black, rectId, IdFormat);
                graphics.DrawString(garantie, GarantieFont, Brushes.Black, rectGarantie, GarantieFormat);
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(0, bitmap.Height));
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(0, 0), new System.Drawing.Point(bitmap.Width, 00));
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(247, 0), new System.Drawing.Point(247, 247));
                graphics.DrawLine(new Pen(Brushes.Black, 1), new System.Drawing.Point(247, 71), new System.Drawing.Point(0, 71));
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
                Debug.WriteLine("Étiquette pour Rail crée avec succès !");


            }
            RailPagesCrafter(EtiquettePath);

        }

        public void CraftPoster(string Libelle, string Boitier, string CarteMere, string Processeur, string RAM, string CG, string Stockage, string Alim, string OS, string PrixBarre, string Prix)
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
            Rectangle rectCategorieBoitier = new(-170, 225, bitmap.Width / 2, 40);

            StringFormat CategorieCMFormat = new();
            CategorieCMFormat.Alignment = StringAlignment.Far;
            CategorieCMFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieCM = new(-170, 295, bitmap.Width / 2, 40);

            StringFormat CategorieProcFormat = new();
            CategorieProcFormat.Alignment = StringAlignment.Far;
            CategorieProcFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieProc = new(-170, 365, bitmap.Width / 2, 40);

            StringFormat CategorieRamFormat = new();
            CategorieRamFormat.Alignment = StringAlignment.Far;
            CategorieRamFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieRam = new(-170, 435, bitmap.Width / 2, 40);

            StringFormat CategorieCGFormat = new();
            CategorieCGFormat.Alignment = StringAlignment.Far;
            CategorieCGFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieCG = new(-170, 505, bitmap.Width / 2, 40);

            StringFormat CategorieStockageFormat = new();
            CategorieStockageFormat.Alignment = StringAlignment.Far;
            CategorieStockageFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieStockage = new(-170, 575, bitmap.Width / 2, 40);

            StringFormat CategorieAlimFormat = new();
            CategorieAlimFormat.Alignment = StringAlignment.Far;
            CategorieAlimFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieAlim = new(-170, 645, bitmap.Width / 2, 40);

            StringFormat CategorieOSFormat = new();
            CategorieOSFormat.Alignment = StringAlignment.Far;
            CategorieOSFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCategorieOS = new(-170, 715, bitmap.Width / 2, 40);

            StringFormat CaracBoitierFormat = new();
            CaracBoitierFormat.Alignment = StringAlignment.Near;
            CaracBoitierFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracBoitier = new(250, 225, 550, 40);

            StringFormat CaracCMFormat = new();
            CaracCMFormat.Alignment = StringAlignment.Near;
            CaracCMFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracCM = new(250, 295, 550, 40);

            StringFormat CaracProcFormat = new();
            CaracProcFormat.Alignment = StringAlignment.Near;
            CaracProcFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracProc = new(250, 365, 550, 40);

            StringFormat CaracRamFormat = new();
            CaracRamFormat.Alignment = StringAlignment.Near;
            CaracRamFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracRam = new(250, 435, 550, 40);

            StringFormat CaracCGFormat = new();
            CaracCGFormat.Alignment = StringAlignment.Near;
            CaracCGFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracCG = new(250, 505, 550, 40);

            StringFormat CaracStockageFormat = new();
            CaracStockageFormat.Alignment = StringAlignment.Near;
            CaracStockageFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracStockage = new(250, 575, 550, 40);

            StringFormat CaracAlimFormat = new();
            CaracAlimFormat.Alignment = StringAlignment.Near;
            CaracAlimFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracAlim = new(250, 645, 550, 40);

            StringFormat CaracOSFormat = new();
            CaracOSFormat.Alignment = StringAlignment.Near;
            CaracOSFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectCaracOS = new(250, 715, 550, 40);

            StringFormat PrixBarreFormat = new();
            PrixBarreFormat.Alignment = StringAlignment.Center;
            PrixBarreFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrixBarre = new(0, 785, bitmap.Width, 60);

            StringFormat PrixFormat = new();
            PrixFormat.Alignment = StringAlignment.Center;
            PrixFormat.LineAlignment = StringAlignment.Center;
            Rectangle rectPrix = new(0, 870, bitmap.Width, 100);

            Rectangle RectLogo = new(bitmap.Width / 2 - (bitmapLogo.Width * 9 / 10) / 2, 1020, bitmapLogo.Width * 9 / 10, bitmapLogo.Height * 9 / 10);

            if (OS == "")
            {
                OS = "149 € l'installation";
            }
            if (CG == "")
            {
                CG = "Chipset Graphique";
            }
            Boitier = Boitier.Replace("Sans Alim/", "", StringComparison.CurrentCultureIgnoreCase);
            if (Boitier.Length >= 37) 
            {
                int start = Boitier.IndexOf(" ", 28, StringComparison.CurrentCultureIgnoreCase);
                if (start != -1)
                {
                    Boitier = Boitier.Substring(0, start);
                    if (Boitier[Boitier.Length - 1] == char.Parse("-"))
                    {
                        Boitier = Boitier.Substring(0, Boitier.Length - 1);
                    }
                }
            }

            if (CarteMere.Length >= 34)
            {
                int Index = CarteMere.IndexOf("/LGA", StringComparison.CurrentCultureIgnoreCase);
                if (Index != -1)
                {
                    string first = CarteMere.Substring(0, Index);
                    string second = CarteMere.Substring(Index+8);
                    CarteMere = first + second;
                }
                Index = CarteMere.IndexOf("/DDR", StringComparison.CurrentCultureIgnoreCase);
                if (Index != -1)
                {
                    string first = CarteMere.Substring(0, Index);
                    string second = CarteMere.Substring(Index+5);
                    CarteMere = first + second;
                }
                Index = CarteMere.IndexOf("/AM", StringComparison.CurrentCultureIgnoreCase);
                if (Index != -1)
                {
                    string first = CarteMere.Substring(0, Index);
                    string second = CarteMere.Substring(Index + 4);
                    CarteMere = first + second;
                }
                if (CarteMere.Length >= 35)
                {
                    Index = CarteMere.IndexOf("/", StringComparison.CurrentCultureIgnoreCase);
                    if (Index != -1)
                    {
                        string first = CarteMere.Substring(0, Index - 4);
                        string second = CarteMere.Substring(Index + 1);
                        CarteMere = first + second;
                    }
                }
                if (CarteMere.Length >= 35)
                {
                    Index = CarteMere.IndexOf(" -", StringComparison.CurrentCultureIgnoreCase);
                    if (Index != -1)
                    {
                        CarteMere = CarteMere.Substring(0, Index);
                    }
                }

            }

            if(Processeur.Length > 35)
            {
                int Index = Processeur.IndexOf("/", StringComparison.CurrentCultureIgnoreCase);
                if (Index != -1)
                {
                    Processeur = Processeur.Substring(0, Index);
                }
            }

            if(RAM.Length > 34)
            {
                string[] Ram = RAM.Split();
                RAM = "";
                for(int i = 0; i < Ram.Length; i++)
                {
                    if(i != 1)
                    {
                        RAM += Ram[i] + " ";
                    }
                }
                List<char> charsToRemove = new List<char>() {'(', ')'};
                foreach (char c in charsToRemove)
                {
                    RAM = RAM.Replace(c.ToString(), String.Empty);
                }

                RAM = RAM.Trim();
                if(RAM.Length >= 34)
                {
                    int Index = RAM.LastIndexOf(" ");
                    RAM = RAM.Substring(0, Index);
                }
            }
            
            while (CG.Length >= 34)
            {
                CG = CG.Trim();
                int Index = CG.LastIndexOf(" ");
                CG = CG.Substring(0, Index);
            }
            char CGcharAt = CG[CG.Length - 1];
            if (CGcharAt == '-')
            {
                CG = CG.Substring(0, CG.Length - 1);
            }


            if (Stockage.Length >= 34)
            {
                int Index = Stockage.IndexOf(" -", StringComparison.CurrentCultureIgnoreCase);
                if (Index != -1)
                {
                    Stockage = Stockage.Substring(0, Index);
                }
                while (Stockage.Length >= 34)
                {
                    Stockage = Stockage.Trim();
                    Index = Stockage.LastIndexOf(" ");
                    Stockage = Stockage.Substring(0, Index);
                }
            }

            while (Alim.Length >= 34)
            {
                Alim = Alim.Trim();
                int Index = Alim.LastIndexOf(" ");
                Alim = Alim.Substring(0, Index);
            }

            char AlimcharAt = Alim[Alim.Length - 1];
            if(AlimcharAt == '-')
            {
                Alim = Alim.Substring(0, Alim.Length - 1);
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

                if (Libelle.Length > 14)
                {
                    NameFont = new(privateFonts.Families[0], 42);
                }

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

                if (PrixBarre != "0")
                {
                    graphics.DrawString(PrixBarre + " €", PrixBarreFont, Brushes.Gray, rectPrixBarre, PrixBarreFormat);
                }
                else
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

        private void WallPagesCrafter(string EtiquettePath)
        {

            string FeuilleFilePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + Properties.Settings.Default.Pagenumber + ".bmp";
            Bitmap bitmapFeuille = (Bitmap)System.Drawing.Image.FromFile(FeuilleFilePath);
            Bitmap bitmapEtiquette = (Bitmap)System.Drawing.Image.FromFile(EtiquettePath);
            Rectangle Etiquette = new(Properties.Settings.Default.nbColonnes * 215, Properties.Settings.Default.nbLignes * 155, bitmapEtiquette.Width, bitmapEtiquette.Height);

            if (File.Exists(FeuilleFinalePath))
            {
                Debug.WriteLine("Mise à jour de la page " + Properties.Settings.Default.Pagenumber);
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

            
            Properties.Settings.Default.nbColonnes++;
            Properties.Settings.Default.nbEtiquettes++;
            Properties.Settings.Default.OnPageNumber++;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.nbColonnes == 3)
            {
                Properties.Settings.Default.nbColonnes = 0;
                Properties.Settings.Default.nbLignes++;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.OnPageNumber == 21)
            {
                Properties.Settings.Default.OnPageNumber = 0;
                Properties.Settings.Default.PageSelected++;
                
                Properties.Settings.Default.Pagenumber++;
                Properties.Settings.Default.nbColonnes = 0;
                Properties.Settings.Default.nbLignes = 0;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.nbEtiquettes > 21)
            {
                MainWindow.gui.btnPrecedent.Visibility = Visibility.Visible;
                MainWindow.gui.btnSuivant.Visibility = Visibility.Visible;
                MainWindow.gui.pageNumber.Visibility = Visibility.Visible;
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
            MainWindow.gui.imgEtiquette.Source = _image;
            MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
            MainWindow.gui.pageNumber.Text = Properties.Settings.Default.PageSelected.ToString();
            MainWindow.gui.btnImprimer.IsEnabled = true;

        }

        private void RailPagesCrafter(string EtiquettePath)
        {

            string FeuilleFilePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Patron_feuille_finale.bmp";
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + Properties.Settings.Default.PagenumberRail + ".bmp";
            Bitmap bitmapFeuille = (Bitmap)System.Drawing.Image.FromFile(FeuilleFilePath);
            Bitmap bitmapEtiquette = (Bitmap)System.Drawing.Image.FromFile(EtiquettePath);
            Rectangle Etiquette = new(Properties.Settings.Default.nbColonnesRail * 249, Properties.Settings.Default.nbLignesRail * 73, bitmapEtiquette.Width, bitmapEtiquette.Height);

            if (File.Exists(FeuilleFinalePath))
            {
                Debug.WriteLine("Mise à jour de la page pour Rail " + Properties.Settings.Default.PagenumberRail);
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
                Debug.WriteLine("Création de la première page d'étiquettes pour rail");
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

            Properties.Settings.Default.nbColonnesRail++;
            Properties.Settings.Default.nbEtiquettesRail++;
            Properties.Settings.Default.OnPageNumberRail++;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.nbColonnesRail == 3)
            {
                Properties.Settings.Default.nbColonnesRail = 0;
                Properties.Settings.Default.nbLignesRail++;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.OnPageNumberRail == 42)
            {
                Properties.Settings.Default.OnPageNumberRail = 0;
                Properties.Settings.Default.PagenumberRail++;
                Properties.Settings.Default.RailPageSelected++;
                Properties.Settings.Default.nbColonnesRail = 0;
                Properties.Settings.Default.nbLignesRail = 0;
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.nbEtiquettesRail > 42)
            {
                MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Visible;
                MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Visible;
                MainWindow.gui.RailpageNumber.Visibility = Visibility.Visible;
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
            MainWindow.gui.RailimgEtiquette.Source = _image;
            MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
            MainWindow.gui.RailpageNumber.Text = Properties.Settings.Default.RailPageSelected.ToString();
            MainWindow.gui.RailbtnImprimer.IsEnabled = true;
        }

        public void setPreviousWallTicketPage()
        {
            Properties.Settings.Default.PageSelected--;
            MainWindow.gui.pageNumber.Text = Properties.Settings.Default.PageSelected.ToString();
            MainWindow.gui.btnSuivant.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + Properties.Settings.Default.PageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.imgEtiquette.Source = _image;
            if (Properties.Settings.Default.PageSelected == 1)
            {
                MainWindow.gui.btnPrecedent.Visibility = Visibility.Hidden;
            }
        }

        public void setNextWallTicketPage()
        {
            Properties.Settings.Default.PageSelected++;
            MainWindow.gui.pageNumber.Text = Properties.Settings.Default.PageSelected.ToString();
            MainWindow.gui.btnPrecedent.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + Properties.Settings.Default.PageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.imgEtiquette.Source = _image;
            if (Properties.Settings.Default.PageSelected == Properties.Settings.Default.Pagenumber)
            {
                MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
            }
        }

        public void setPreviousRailTicketPage()
        {
            Properties.Settings.Default.RailPageSelected--;
            MainWindow.gui.RailpageNumber.Text = Properties.Settings.Default.RailPageSelected.ToString();
            MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + Properties.Settings.Default.RailPageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.RailimgEtiquette.Source = _image;
            if (Properties.Settings.Default.RailPageSelected == 1)
            {
                MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Hidden;
            }
        }
                
        public void setNextRailTicketPage()
        {
            Properties.Settings.Default.RailPageSelected++;
            MainWindow.gui.RailpageNumber.Text = Properties.Settings.Default.RailPageSelected.ToString();
            MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + Properties.Settings.Default.RailPageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.RailimgEtiquette.Source = _image;
            if (Properties.Settings.Default.RailPageSelected == Properties.Settings.Default.PagenumberRail)
            {
                MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
            }
        }

        public void setPreviousAffiche()
        {
            Properties.Settings.Default.affichePageSelected--;
            MainWindow.gui.A4pageNumber.Text = Properties.Settings.Default.affichePageSelected.ToString();
            MainWindow.gui.A4btnSuivant.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_" + Properties.Settings.Default.affichePageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.A4imgEtiquette.Source = _image;
            if (Properties.Settings.Default.affichePageSelected == 1)
            {
                MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Hidden;
            }
        }

        public void setNextAffiche()
        {
            Properties.Settings.Default.affichePageSelected++;
            MainWindow.gui.A4pageNumber.Text = Properties.Settings.Default.affichePageSelected.ToString();
            MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Visible;
            string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_" + Properties.Settings.Default.affichePageSelected + ".bmp";
            Uri lastPageUri = new(FeuilleFinalePath, UriKind.RelativeOrAbsolute);

            BitmapImage _image = new();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = lastPageUri;
            _image.EndInit();
            MainWindow.gui.A4imgEtiquette.Source = _image;
            if (Properties.Settings.Default.affichePageSelected == Properties.Settings.Default.nbrAffiches)
            {
                MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
            }
        }

        public void ResetWallDatas()
        {
            for (int i = 1; i <= Properties.Settings.Default.Pagenumber; i++)
            {
                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale" + i + ".bmp";
                Debug.WriteLine("Page Murale " + i + " effacée");
                File.Delete(FeuilleFinalePath);
            }
            Properties.Settings.Default.Pagenumber = 1;
            Properties.Settings.Default.nbColonnes = 0;
            Properties.Settings.Default.nbLignes = 0;
            Properties.Settings.Default.nbEtiquettes = 0;
            Properties.Settings.Default.PageSelected = 1;
            Properties.Settings.Default.OnPageNumber = 0;
            Properties.Settings.Default.Save();
            MainWindow.gui.btnPrecedent.Visibility = Visibility.Hidden;
            MainWindow.gui.btnSuivant.Visibility = Visibility.Hidden;
            MainWindow.gui.pageNumber.Visibility = Visibility.Hidden;


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
            MainWindow.gui.imgEtiquette.Source = _image;

            MainWindow.gui.btnImprimer.IsEnabled = false;

        }

        public void ResetRailDatas()
        {
            for (int i = 1; i <= Properties.Settings.Default.PagenumberRail; i++)
            {
                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Feuille_Finale_Rail" + i + ".bmp";
                Debug.WriteLine("Page Rail " + i + " effacée");
                File.Delete(FeuilleFinalePath);
            }
            Properties.Settings.Default.PagenumberRail = 1;
            Properties.Settings.Default.nbColonnesRail = 0;
            Properties.Settings.Default.nbLignesRail = 0;
            Properties.Settings.Default.nbEtiquettesRail = 0;
            Properties.Settings.Default.PageSelected = 1;
            Properties.Settings.Default.OnPageNumberRail = 0;
            Properties.Settings.Default.Save();
            MainWindow.gui.RailbtnPrecedent.Visibility = Visibility.Hidden;
            MainWindow.gui.RailbtnSuivant.Visibility = Visibility.Hidden;
            MainWindow.gui.RailpageNumber.Visibility = Visibility.Hidden;


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
            MainWindow.gui.RailimgEtiquette.Source = _image;
            MainWindow.gui.RailbtnImprimer.IsEnabled = false;
        }

        public void ResetAffichesDatas()
        {
            for (int i = 1; i <= Properties.Settings.Default.nbrAffiches; i++)
            {
                string FeuilleFinalePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_" + i + ".bmp";
                Debug.WriteLine("Affiche " + i + " effacée");
                File.Delete(FeuilleFinalePath);
            }
            Properties.Settings.Default.nbrAffiches = 0;
            Properties.Settings.Default.affichePageSelected = 1;
            Properties.Settings.Default.Save();
            MainWindow.gui.A4btnPrecedent.Visibility = Visibility.Hidden;
            MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
            MainWindow.gui.A4pageNumber.Visibility = Visibility.Hidden;


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
            MainWindow.gui.A4imgEtiquette.Source = _image;
            MainWindow.gui.A4btnImprimer.IsEnabled = false;

        }

    }
}
