using PriceTicker.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

            string AffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp";
            if (File.Exists(AffichePath))
            {
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
                ValiderAfficheBtn.IsEnabled = true;
            }else
            {
                ValiderAfficheBtn.IsEnabled = false;
            }
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

            if(LastUpdate == DateTime.Parse("01/01/1970"))
            {
                LastUpdateDate.Text = "Dernière mise à jour : Jamais";
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
            int nbrAffiches = Settings.Default.nbrAffiches+1;
            string AffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Nouvelle_Affiche.bmp";
            string FinaleAffichePath = AppDomain.CurrentDomain.BaseDirectory + "Img\\Affiche_" + nbrAffiches + ".bmp";

            if (File.Exists(AffichePath))
            {
                File.Copy(AffichePath, FinaleAffichePath);
                Settings.Default.nbrAffiches++;
                Settings.Default.affichePageSelected++;
                Settings.Default.Save();
                Uri lastPageUri = new(FinaleAffichePath, UriKind.RelativeOrAbsolute);

                BitmapImage _image = new();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = lastPageUri;
                _image.EndInit();
                MainWindow.gui.A4imgEtiquette.Source = _image;
                MainWindow.gui.A4btnSuivant.Visibility = Visibility.Hidden;
                MainWindow.gui.A4pageNumber.Text = Properties.Settings.Default.affichePageSelected.ToString();

            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une affiche !");
            }
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
            UpdateDataGridFromWeb.IsEnabled = false;
            dateEntreeField.SelectedDate = null;
            dateSortieField.SelectedDate = null;
            SearchedLibelle.Text = "";
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

        private void ArchiveLineSelected(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            dynamic selected_row = grid.SelectedItem;
            if (selected_row != null)
            {
                string LineSelected = ArchiveDataGrid.SelectedItem.ToString();
                List<long> IdConfigSelectedList = FindIdNumbers(LineSelected);
                Models.PcGamer product = new();
                product = databaseManager.SelectArchivedPcGamerByID(int.Parse(IdConfigSelectedList.First().ToString()));

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

        private void SelectedStartDate(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            List<Models.PcGamer> ArchiveProductsFoundByDates = new();

            if (datePicker.SelectedDate != null)
            {
                dateSortieField.BlackoutDates.Clear();
                dateSortieField.BlackoutDates.Add(new CalendarDateRange(new DateTime(1500, 1, 1), (DateTime)datePicker.SelectedDate));

                
                if (dateSortieField.SelectedDate == null)
                {
                    dateSortieField.SelectedDate = DateTime.Now;
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates((DateTime)datePicker.SelectedDate, DateTime.Now);
                }else
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates((DateTime)datePicker.SelectedDate, (DateTime)dateSortieField.SelectedDate);
                }

                
                List<Models.PcGamer> ArchiveProducts = new();

                if (SearchedLibelle.Text == "")
                {
                    ArchiveProducts = ArchiveProductsFoundByDates;
                }
                else
                {
                    
                    for (int i = 0; i < ArchiveProductsFoundByDates.Count() ; i++)
                    {
                        if (ArchiveProductsFoundByDates[i].getName().Contains(SearchedLibelle.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByDates[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {

                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;
                }), DispatcherPriority.Background);

            }else
            {
                dateSortieField.BlackoutDates.Clear();

                
                if (dateSortieField.SelectedDate == null)
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectAllArchivedPcGamer();
                }
                else
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates(new DateTime(1500, 1, 1), (DateTime)dateSortieField.SelectedDate);
                }

                List<Models.PcGamer> ArchiveProducts = new();

                if (SearchedLibelle.Text == "")
                {
                    ArchiveProducts = ArchiveProductsFoundByDates;
                }
                else
                {

                    for (int i = 0; i < ArchiveProductsFoundByDates.Count(); i++)
                    {
                        if (ArchiveProductsFoundByDates[i].getName().Contains(SearchedLibelle.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByDates[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {

                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;
                }), DispatcherPriority.Background);
            }
            

        }

        private void SelectedEndDate(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            List<Models.PcGamer> ArchiveProductsFoundByDates = new();

            if (datePicker.SelectedDate != null)
            {
                dateEntreeField.BlackoutDates.Clear();
                dateEntreeField.BlackoutDates.Add(new CalendarDateRange((DateTime)datePicker.SelectedDate, new DateTime(2500, 1, 1)));

                
                if (dateEntreeField.SelectedDate == null)
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates(new DateTime(1500, 1, 1), (DateTime)datePicker.SelectedDate);
                }else
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates((DateTime)dateEntreeField.SelectedDate, (DateTime)datePicker.SelectedDate);
                }

                List<Models.PcGamer> ArchiveProducts = new();

                if (SearchedLibelle.Text == "")
                {
                    ArchiveProducts = ArchiveProductsFoundByDates;
                }
                else
                {

                    for (int i = 0; i < ArchiveProductsFoundByDates.Count(); i++)
                    {
                        if (ArchiveProductsFoundByDates[i].getName().Contains(SearchedLibelle.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByDates[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {

                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;
                }), DispatcherPriority.Background);

            }else
            {
                dateEntreeField.BlackoutDates.Clear();

                
                if (dateEntreeField.SelectedDate == null)
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectAllArchivedPcGamer();
                }
                else
                {
                    ArchiveProductsFoundByDates = databaseManager.SelectArchivedPcGamerByDates((DateTime)dateEntreeField.SelectedDate, DateTime.Now);
                }

                List<Models.PcGamer> ArchiveProducts = new();

                if (SearchedLibelle.Text == "")
                {
                    ArchiveProducts = ArchiveProductsFoundByDates;
                }
                else
                {

                    for (int i = 0; i < ArchiveProductsFoundByDates.Count(); i++)
                    {
                        if (ArchiveProductsFoundByDates[i].getName().Contains(SearchedLibelle.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByDates[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {

                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;
                }), DispatcherPriority.Background);
            }

        }

        private void searchedLibelleChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            List<Models.PcGamer> ArchiveProductsFoundByName = new();

            if (textBox.Text != "")
            {
                
                ArchiveProductsFoundByName = databaseManager.SelectArchivedPcGamerByName(textBox.Text);

                List<Models.PcGamer> ArchiveProducts = new();

                if (dateEntreeField.SelectedDate == null && dateSortieField.SelectedDate == null)
                {
                    ArchiveProducts = ArchiveProductsFoundByName;
                }
                else
                {
                    if(dateEntreeField.SelectedDate != null && dateSortieField.SelectedDate == null)
                    {
                        for(int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                        {
                            if (ArchiveProductsFoundByName[i].getDateEntree() >= dateEntreeField.SelectedDate)
                            {
                                ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                            }
                        }
                    }

                    if (dateEntreeField.SelectedDate == null && dateSortieField.SelectedDate != null)
                    {
                        for (int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                        {
                            if (ArchiveProductsFoundByName[i].getDateSortie().AddDays(-1) <= dateSortieField.SelectedDate)
                            {
                                ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                            }
                        }
                    }
                }

                if (dateEntreeField.SelectedDate != null && dateSortieField.SelectedDate != null)
                {
                    for (int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                    {
                        if (ArchiveProductsFoundByName[i].getDateEntree() >= dateEntreeField.SelectedDate && ArchiveProductsFoundByName[i].getDateSortie().AddDays(-1) <= dateSortieField.SelectedDate)
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {

                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;
                }), DispatcherPriority.Background);

            }else
            {

                ArchiveProductsFoundByName = databaseManager.SelectAllArchivedPcGamer();

                List<Models.PcGamer> ArchiveProducts = new();

                if (dateEntreeField.SelectedDate == null && dateSortieField.SelectedDate == null)
                {
                    ArchiveProducts = ArchiveProductsFoundByName;
                }
                else
                {
                    if (dateEntreeField.SelectedDate != null && dateSortieField.SelectedDate == null)
                    {
                        for (int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                        {
                            if (ArchiveProductsFoundByName[i].getDateEntree() >= dateEntreeField.SelectedDate)
                            {
                                ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                            }
                        }
                    }

                    if (dateEntreeField.SelectedDate == null && dateSortieField.SelectedDate != null)
                    {
                        for (int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                        {
                            if (ArchiveProductsFoundByName[i].getDateSortie().AddDays(-1) <= dateSortieField.SelectedDate)
                            {
                                ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                            }
                        }
                    }
                }

                if (dateEntreeField.SelectedDate != null && dateSortieField.SelectedDate != null)
                {
                    for (int i = 0; i < ArchiveProductsFoundByName.Count; i++)
                    {
                        if (ArchiveProductsFoundByName[i].getDateEntree() >= dateEntreeField.SelectedDate && ArchiveProductsFoundByName[i].getDateSortie().AddDays(-1) <= dateSortieField.SelectedDate)
                        {
                            ArchiveProducts.Add(ArchiveProductsFoundByName[i]);
                        }
                    }
                }

                var sortedArchivedProducts = ArchiveProducts.OrderBy(c => c.getDateSortie());

                Dispatcher.Invoke(new Action(() =>
                {
                    ArchiveDataGrid.AutoGenerateColumns = false;

                    IEnumerable _bindArchives = sortedArchivedProducts.Select(product => new
                    {
                        id = product.getIdConfig(),
                        dateEntree = product.getDateEntree(),
                        dateSortie = product.getDateSortie(),
                        name = product.getName(),
                        prix = product.getPrix(),
                        boitier = product.getBoitier(),
                        carteMere = product.getCarteMere(),
                        processeur = product.getProcesseur(),
                        carteGraphique = product.getCarteGraphique(),
                        ram = product.getRam(),

                    });

                    ArchiveDataGrid.ItemsSource = _bindArchives;

                }), DispatcherPriority.Background);
            }
        }
    }
}
