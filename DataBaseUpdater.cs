using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTicker
{
    internal class DataBaseUpdater
    {
        
        DatabaseManager databaseManager = new DatabaseManager();

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
                    //PcWeb.setName("Caca");
                    //Si une caractéristique a changé, il faut archiver la config et mettre à jour la config actuelle.
                    if (PcGamerSaved.getName() != PcWeb.getName())
                    {
                        bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                        if (isArchived)
                        databaseManager.UpdatePcGamerByID(IdsPCWeb[i], "Name", PcWeb.getName());
                    }
                    if (PcGamerSaved.getPrix() != PcWeb.getPrix())
                    {
                        bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                        if (isArchived)
                            databaseManager.UpdatePcGamerByID(IdsPCWeb[i], "Prix", PcWeb.getPrix().ToString());
                    }
                    if (PcGamerSaved.getPrixBarre() != PcWeb.getPrixBarre())
                    {
                        bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                        if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Boîtier", PcWeb.getBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("Le boîtier " + PcGamerSaved.getBoitier() + " a changé. Le nouveau est : " + PcWeb.getBoitier() + "\r"
                                + "Ce boîtier n'existe pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Boîtier", PcWeb.getBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getBoitier());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de boîtier", PcWeb.getAccessoireBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("L'Accessoire de boîtier " + PcGamerSaved.getAccessoireBoitier() + " a changé. Le nouveau est : " + PcWeb.getAccessoireBoitier() + "\r"
                                + "Cet accessoire de boîtier n'existe pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire de boîtier", PcWeb.getAccessoireBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireBoitier());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventilateurs", PcWeb.getVentilateurBoitier());
                        }
                        else
                        {
                            Debug.WriteLine("Les ventilateurs de boîtier " + PcGamerSaved.getVentilateurBoitier() + " ont changé. Les nouveaux sont : " + PcWeb.getVentilateurBoitier() + "\r"
                                + "Ces ventilateurs de boîtier n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Ventilateurs", PcWeb.getVentilateurBoitier());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getVentilateurBoitier());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Processeur", PcWeb.getProcesseur());
                        }
                        else
                        {
                            Debug.WriteLine("Le Processeur " + PcGamerSaved.getProcesseur() + " a changé. Le nouveau est : " + PcWeb.getProcesseur() + "\r"
                                + "Ce Processeur n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Processeur", PcWeb.getProcesseur());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getProcesseur());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Ventirad", PcWeb.getVentirad());
                        }
                        else
                        {
                            Debug.WriteLine("Le Ventirad " + PcGamerSaved.getVentirad() + " a changé. Le nouveau est : " + PcWeb.getVentirad() + "\r"
                                + "Ce Ventirad n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Ventirad", PcWeb.getVentirad());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getVentirad());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Watercooling", PcWeb.getWaterCooling());
                        }
                        else
                        {
                            Debug.WriteLine("Le Watercooling " + PcGamerSaved.getWaterCooling() + " a changé. Le nouveau est : " + PcWeb.getWaterCooling() + "\r"
                                + "Ce Watercooling n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Watercooling", PcWeb.getWaterCooling());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getWaterCooling());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte mère", PcWeb.getCarteMere());
                        }
                        else
                        {
                            Debug.WriteLine("La Carte mère " + PcGamerSaved.getCarteMere() + " a changé. La nouvelle est : " + PcWeb.getCarteMere() + "\r"
                                + "Cette Carte mère n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte mère", PcWeb.getCarteMere());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteMere());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte Graphique", PcWeb.getCarteGraphique());
                        }
                        else
                        {
                            Debug.WriteLine("La Carte Graphique " + PcGamerSaved.getCarteGraphique() + " a changé. La nouvelle est : " + PcWeb.getCarteGraphique() + "\r"
                                + "Cette Carte Graphique n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte Graphique", PcWeb.getCarteGraphique());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteGraphique());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire de Carte Graphique", PcWeb.getAccessoireCarteGraphique());
                        }
                        else
                        {
                            Debug.WriteLine("L'accessoire de Carte Graphique " + PcGamerSaved.getAccessoireCarteGraphique() + " a changé. Le nouveau est : " + PcWeb.getAccessoireCarteGraphique() + "\r"
                                + "Cet accessoire de Carte Graphique n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire de Carte Graphique", PcWeb.getAccessoireCarteGraphique());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireCarteGraphique());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Mémoire vive", PcWeb.getRam());
                        }
                        else
                        {
                            Debug.WriteLine("La mémoire vive " + PcGamerSaved.getRam() + " a changé. La nouvelle est : " + PcWeb.getRam() + "\r"
                                + "Cette mémoire vive n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Mémoire vive", PcWeb.getRam());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getRam());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "SSD", PcWeb.getDisqueSsd());
                        }
                        else
                        {
                            Debug.WriteLine("Le SSD " + PcGamerSaved.getDisqueSsd() + " a changé. Le nouveau est : " + PcWeb.getDisqueSsd() + "\r"
                                + "Ce SSD n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("SSD", PcWeb.getDisqueSsd());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSsd());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "HDD", PcWeb.getDisqueSupplementaire());
                        }
                        else
                        {
                            Debug.WriteLine("Le HDD " + PcGamerSaved.getDisqueSupplementaire() + " a changé. Le nouveau est : " + PcWeb.getDisqueSupplementaire() + "\r"
                                + "Ce HDD n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("HDD", PcWeb.getDisqueSupplementaire());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getDisqueSupplementaire());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Carte réseau", PcWeb.getCarteReseau());
                        }
                        else
                        {
                            Debug.WriteLine("La carte réseau " + PcGamerSaved.getCarteReseau() + " a changé. La nouvelle est : " + PcWeb.getCarteReseau() + "\r"
                                + "Cette carte réseau n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Carte réseau", PcWeb.getCarteReseau());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getCarteReseau());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Alimentation", PcWeb.getAlimentation());
                        }
                        else
                        {
                            Debug.WriteLine("L'alimentation " + PcGamerSaved.getAlimentation() + " a changé. La nouvelle est : " + PcWeb.getAlimentation() + "\r"
                                + "Cette alimentation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Alimentation", PcWeb.getAlimentation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAlimentation());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Accessoire alimentation", PcWeb.getAccessoireAlimentation());
                        }
                        else
                        {
                            Debug.WriteLine("L'accessoire d'alimentation " + PcGamerSaved.getAccessoireAlimentation() + " a changé. Le nouveau est : " + PcWeb.getAccessoireAlimentation() + "\r"
                                + "Cet Accessoire d'alimentation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Accessoire alimentation", PcWeb.getAccessoireAlimentation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getAccessoireAlimentation());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
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
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getName(), composantId, "Système Exploitation", PcWeb.getSystemeExploitation());
                        }
                        else
                        {
                            Debug.WriteLine("Le Système d'Exploitation " + PcGamerSaved.getSystemeExploitation() + " a changé. Le nouveau est : " + PcWeb.getSystemeExploitation() + "\r"
                                + "Ce Système d'Exploitation n'existent pas dans la base de données (ID : " + composantId + ").");
                            databaseManager.InsertComposants("Système Exploitation", PcWeb.getSystemeExploitation());
                            composantId = databaseManager.FindComposantIdByName(PcWeb.getSystemeExploitation());
                            bool isArchived = databaseManager.ArchiveConfigByID(PcGamerSaved.getIdConfig());
                            if (isArchived)
                                databaseManager.UpdatePcGamerComposantByID(IdsPCWeb[i], PcWeb.getSystemeExploitation(), composantId, "Système Exploitation", PcWeb.getSystemeExploitation());
                        }
                    }

                }
            }
        }

        public void CompareWebwithDbAndInsertNews(List<Models.PcGamer> productsList, List<int> IdsPCSaved)
        {
            for (int i = 0; i < productsList.Count; i++)
            {
                if (!IdsPCSaved.Contains(productsList[i].getIdConfig()))
                {
                    Debug.WriteLine("Le PC \"" + productsList[i].getName() + "\" n'existe pas dans la base de données. Ajout à la base de donnée... \r");
                    databaseManager.InsertPCGamer(
                            productsList[i].getIdConfig(),
                            productsList[i].getName(),
                            productsList[i].getPrix().ToString(),
                            productsList[i].getPrixBarre().ToString(),
                            productsList[i].getWebLink(),
                            DateTime.Now.Date);
                    if (productsList[i].getBoitier() != "")
                    {
                        databaseManager.InsertComposants("Boîtier", productsList[i].getBoitier());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Boîtier", productsList[i].getBoitier()),
                                "Boîtier",
                                productsList[i].getBoitier()
                            );
                    }

                    if (productsList[i].getAccessoireBoitier() != "")
                    {
                        databaseManager.InsertComposants("Accessoire de boîtier", productsList[i].getAccessoireBoitier());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire de boîtier", productsList[i].getAccessoireBoitier()),
                                "Accessoire de boîtier",
                                productsList[i].getAccessoireBoitier()
                            );

                    }
                    if (productsList[i].getVentilateurBoitier() != "")
                    {
                        databaseManager.InsertComposants("Ventilateurs", productsList[i].getVentilateurBoitier());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Ventilateurs", productsList[i].getVentilateurBoitier()),
                                "Ventilateurs",
                                productsList[i].getVentilateurBoitier()
                            );

                    }
                    if (productsList[i].getProcesseur() != "")
                    {
                        databaseManager.InsertComposants("Processeur", productsList[i].getProcesseur());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Processeur", productsList[i].getProcesseur()),
                                "Processeur",
                                productsList[i].getProcesseur()
                            );

                    }
                    if (productsList[i].getVentirad() != "")
                    {
                        databaseManager.InsertComposants("Ventirad", productsList[i].getVentirad());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Ventirad", productsList[i].getVentirad()),
                                "Ventirad",
                                productsList[i].getVentirad()
                            );

                    }
                    if (productsList[i].getWaterCooling() != "")
                    {
                        databaseManager.InsertComposants("Watercooling", productsList[i].getWaterCooling());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Watercooling", productsList[i].getWaterCooling()),
                                "Watercooling",
                                productsList[i].getWaterCooling()
                            );

                    }
                    if (productsList[i].getCarteMere() != "")
                    {
                        databaseManager.InsertComposants("Carte mère", productsList[i].getCarteMere());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte mère", productsList[i].getCarteMere()),
                                "Carte mère",
                                productsList[i].getCarteMere()
                            );

                    }
                    if (productsList[i].getCarteGraphique() != "")
                    {
                        databaseManager.InsertComposants("Carte Graphique", productsList[i].getCarteGraphique());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte Graphique", productsList[i].getCarteGraphique()),
                                "Carte Graphique",
                                productsList[i].getCarteGraphique()
                            );

                    }
                    if (productsList[i].getAccessoireCarteGraphique() != "")
                    {
                        databaseManager.InsertComposants("Accessoire de Carte Graphique", productsList[i].getAccessoireCarteGraphique());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire de Carte Graphique", productsList[i].getAccessoireCarteGraphique()),
                                "Accessoire de Carte Graphique",
                                productsList[i].getAccessoireCarteGraphique()
                            );

                    }
                    if (productsList[i].getRam() != "")
                    {
                        databaseManager.InsertComposants("Mémoire vive", productsList[i].getRam());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Mémoire vive", productsList[i].getRam()),
                                "Mémoire vive",
                                productsList[i].getRam()
                            );

                    }
                    if (productsList[i].getDisqueSsd() != "")
                    {
                        databaseManager.InsertComposants("SSD", productsList[i].getDisqueSsd());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("SSD", productsList[i].getDisqueSsd()),
                                "SSD",
                                productsList[i].getDisqueSsd()
                            );

                    }
                    if (productsList[i].getDisqueSupplementaire() != "")
                    {
                        databaseManager.InsertComposants("HDD", productsList[i].getDisqueSupplementaire());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("HDD", productsList[i].getDisqueSupplementaire()),
                                "HDD",
                                productsList[i].getDisqueSupplementaire()
                            );

                    }
                    if (productsList[i].getCarteReseau() != "")
                    {
                        databaseManager.InsertComposants("Carte réseau", productsList[i].getCarteReseau());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Carte réseau", productsList[i].getCarteReseau()),
                                "Carte réseau",
                                productsList[i].getCarteReseau()
                            );

                    }
                    if (productsList[i].getAlimentation() != "")
                    {
                        databaseManager.InsertComposants("Alimentation", productsList[i].getAlimentation());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Alimentation", productsList[i].getAlimentation()),
                                "Alimentation",
                                productsList[i].getAlimentation()
                            );

                    }
                    if (productsList[i].getAccessoireAlimentation() != "")
                    {
                        databaseManager.InsertComposants("Accessoire alimentation", productsList[i].getAccessoireAlimentation());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Accessoire alimentation", productsList[i].getAccessoireAlimentation()),
                                "Accessoire alimentation",
                                productsList[i].getAccessoireAlimentation()
                            );

                    }
                    if (productsList[i].getSystemeExploitation() != "")
                    {
                        databaseManager.InsertComposants("Système exploitation", productsList[i].getSystemeExploitation());
                        databaseManager.InsertPcGamerComposants(productsList[i].getIdConfig(),
                                productsList[i].getName(),
                                databaseManager.GetLastIDPcGamerComposants("Système exploitation", productsList[i].getSystemeExploitation()),
                                "Système exploitation",
                                productsList[i].getSystemeExploitation()
                            );

                    }
                }
            }
        }



    }
}
