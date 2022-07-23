using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace PriceTicker
{
    internal class XmlReader
    {
        TicketCrafter ticketCrafter = new();

        public List<String> FindPriceById(string Id, String typeEtiquette)
        {
            String? IdJaja = "N/A";
            String? Libelle = "Aucun Produit ne correspond à l'Id demandé";
            String? Prix = "N/A";

            MainWindow.gui.Dispatcher.Invoke(new Action(() =>
            {

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD"))
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

                DateTime lastModified = File.GetLastWriteTime(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAJACD\\Temp_xml\\Produit.xml");
                TimeSpan updateDelay = DateTime.Now.Subtract(lastModified);

                if(updateDelay.Days == 1)
                {
                    MessageBox.Show("Attention ! Les données provenant de JAJACD n'ont pas été mises à jour depuis " + updateDelay.Days + " jour ! Veuillez Lancer JAJACD.");
                }
                if (updateDelay.Days > 1)
                {
                    MessageBox.Show("Attention ! Les données provenant de JAJACD n'ont pas été mises à jour depuis " + updateDelay.Days + " jours ! Veuillez Lancer JAJACD.");
                }

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
                            IdJaja = "N/A";
                            Prix = "N/A";
                            Libelle = "Aucun Produit ne correspond à l'Id demandé";
                        }
                    }
                }
                ProduitsXml.Close();
                if (IdJaja != "N/A")
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


                                    Prix = PrixXml.ReadElementContentAsString().Replace(".", ",");
                                    Debug.WriteLine("Prix non arrondi : " + Prix);
                                    decimal RoundedPrice = Math.Round(decimal.Parse(Prix), 2, MidpointRounding.AwayFromZero);
                                    Prix = RoundedPrice.ToString() + " €";
                                    Debug.WriteLine("Prix : " + RoundedPrice);

                                    switch (typeEtiquette)
                                    {
                                        case "Murale":
                                            ticketCrafter.WallTicketCrafter(IdJaja, Libelle, Prix);
                                            break;

                                        case "Rail":
                                            ticketCrafter.RailTicketCrafter(IdJaja, Libelle, Prix);
                                            break;

                                        default:
                                            Debug.WriteLine("Type d'étiquette \"" + typeEtiquette + "\" inconnu !!");
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    PrixXml.Close();
                }
                else
                {
                    Debug.WriteLine("Aucun produit trouvé pour l'ID Jaja demandé (ID demandé => " + Id + " )");
                }


            }), DispatcherPriority.Background);

            List<string> ProductSpecList = new List<string>();
            ProductSpecList.Add(Id);
            ProductSpecList.Add(Libelle);
            ProductSpecList.Add(Prix);

            return ProductSpecList;


        }

    }
}
