using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PriceTicker
{
    internal class TicketCrafter
    {

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
            Rectangle rectCategorieBoitier = new(-110, 225, bitmap.Width / 2, 40);

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

            Rectangle RectLogo = new(bitmap.Width / 2 - (bitmapLogo.Width * 9 / 10) / 2, 1020, bitmapLogo.Width * 9 / 10, bitmapLogo.Height * 9 / 10);

            if (OS == "")
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

                graphics.DrawString(Boitier, CaracFont, System.Drawing.Brushes.Black, rectCaracBoitier, CaracBoitierFormat);
                graphics.DrawString(CarteMere, CaracFont, System.Drawing.Brushes.Black, rectCaracCM, CaracCMFormat);
                graphics.DrawString(Processeur, CaracFont, System.Drawing.Brushes.Black, rectCaracProc, CaracProcFormat);
                graphics.DrawString(RAM, CaracFont, System.Drawing.Brushes.Black, rectCaracRam, CaracRamFormat);
                graphics.DrawString(CG, CaracFont, System.Drawing.Brushes.Black, rectCaracCG, CaracCGFormat);
                graphics.DrawString(Stockage, CaracFont, System.Drawing.Brushes.Black, rectCaracStockage, CaracStockageFormat);
                graphics.DrawString(Alim, CaracFont, System.Drawing.Brushes.Black, rectCaracAlim, CaracAlimFormat);
                graphics.DrawString(OS, CaracFont, System.Drawing.Brushes.Black, rectCaracOS, CaracOSFormat);

                if (PrixBarre != "0")
                {
                    graphics.DrawString(PrixBarre + " €", PrixBarreFont, System.Drawing.Brushes.Gray, rectPrixBarre, PrixBarreFormat);
                }
                else
                {
                    PrixFormat.Alignment = StringAlignment.Center;
                    PrixFormat.LineAlignment = StringAlignment.Center;
                    rectPrix = new(0, 840, bitmap.Width, 100);
                }

                graphics.DrawString(Prix + " €", PrixFont, System.Drawing.Brushes.Red, rectPrix, PrixFormat);

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

    }
}
