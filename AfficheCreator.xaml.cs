using PriceTicker.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PriceTicker
{
   
    public partial class AfficheCreator : Window
    {
        public static AfficheCreator? gui;
        DatabaseManager databaseManager = new DatabaseManager();
        Scraping scraping = new();
        TicketCrafter ticketCrafter = new TicketCrafter();
        DispatcherTimer timer = new DispatcherTimer();
        private BackgroundWorker? worker = null;

        public AfficheCreator()
        {
            InitializeComponent();
            gui = this;
            databaseManager.CreateDbFile();
            databaseManager.CreateDbConnection();
            databaseManager.CreateTables();
            databaseManager.CloseDbConnection();
            scraping.ScrapWebsite();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Stop();
            if (scraping.worker != null)
            {
                scraping.worker.CancelAsync();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime LastUpdate = Settings.Default.LastUpdateDate;

            TimeSpan Interval = DateTime.Now.Subtract(LastUpdate);
            if (Interval.Days == 0 && Interval.Hours == 0 && Interval.Minutes == 0)
            {
                LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Seconds + " secondes)";
            }
            if (Interval.Days == 0 && Interval.Hours == 0 && Interval.Minutes != 0)
            {
                LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Minutes + " minutes)";
            }
            if (Interval.Days == 0 && Interval.Hours != 0)
            {
                LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Hours + " heures et " + Interval.Minutes + " minutes)";
            }
            if (Interval.Days != 0)
            {
                LastUpdateDate.Text = "Dernière mise à jour : " + LastUpdate.ToShortDateString() + " à " + LastUpdate.ToShortTimeString() + " (Il y a " + Interval.Days + " jours, " + Interval.Hours + " heures et " + Interval.Minutes + " minutes)";
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
            string LineSelected = ConfigGroupDataGrid.SelectedItem.ToString();
            List<long> IdConfigSelectedList = FindIdNumbers(LineSelected);
            LineSelected = IdConfigSelectedList.First().ToString();
            Models.PcGamer product = new();
            product = databaseManager.SelectPcGamerByID(int.Parse(LineSelected));
            
            string url = product.getWebLink();
            ProcessStartInfo sInfo = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }

        private void ValiderAffiche(object sender, RoutedEventArgs e)
        {
            
        }

        private void LineSelected(object sender, SelectionChangedEventArgs e)
        {

            DataGrid grid = (DataGrid)sender;
            dynamic selected_row = grid.SelectedItem;
            if (selected_row != null)
            {
                string LineSelected = ConfigGroupDataGrid.SelectedItem.ToString();
                List<long> IdConfigSelectedList = FindIdNumbers(LineSelected);
                Models.PcGamer product = new();
                product = databaseManager.SelectPcGamerByID(int.Parse(IdConfigSelectedList.First().ToString()));

                ticketCrafter.CraftPoster(product.getName(),
                    product.getBoitier(),
                    product.getCarteMere(),
                    product.getProcesseur(),
                    product.getRam(),
                    product.getCarteGraphique(),
                    product.getDisqueSsd(),
                    product.getAlimentation(),
                    product.getSystemeExploitation(),
                    product.getPrixBarre().ToString(),
                    product.getPrix().ToString());

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
            }


            
        }

        private void UpdateDataGrid(object sender, RoutedEventArgs e)
        {

            ProgressBar.Visibility = Visibility.Visible;
            ProgressTextBlock.Visibility = Visibility.Visible;
            scraping.worker = new();
            scraping.worker.RunWorkerCompleted += scraping.worker_RunWorkerCompleted;
            scraping.worker.WorkerReportsProgress = true;
            scraping.worker.WorkerSupportsCancellation = true;
            scraping.worker.DoWork += scraping.worker_DoWork;
            scraping.worker.ProgressChanged += scraping.worker_ProgressChanged;
            scraping.worker.RunWorkerAsync();
        }

        public void DoSelectedRow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null && !cell.IsEditing)
            {
                DataGridRow row = FindVisualParent<DataGridRow>(cell);
                if (row != null)
                {
                    row.IsSelected = !row.IsSelected;
                    e.Handled = true;
                }
            }
            
        }

        public static Parent FindVisualParent<Parent>(DependencyObject child) where Parent : DependencyObject
        {
            DependencyObject parentObject = child;

            while (!((parentObject is System.Windows.Media.Visual)
                    || (parentObject is System.Windows.Media.Media3D.Visual3D)))
            {
                if (parentObject is Parent || parentObject == null)
                {
                    return parentObject as Parent;
                }
                else
                {
                    parentObject = (parentObject as FrameworkContentElement).Parent;
                }
            }
            parentObject = VisualTreeHelper.GetParent(parentObject);
            if (parentObject is Parent || parentObject == null)
            {
                return parentObject as Parent;
            }
            else
            {
                return FindVisualParent<Parent>(parentObject);
            }
        }


    }
}
