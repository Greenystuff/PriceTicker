using System;
using System.Linq;

namespace PriceTicker.Models
{
    //[Serializable()]
    public class PcGamer
    {
        private int idConfig = 0;

        private DateTime dateEntree;

        private DateTime dateSortie;

        private string name = "";

        private decimal prix = 0;

        private decimal prixBarre = 0;

        private string idJaja = "";

        private string webLink = "";

        private string boitier = "";

        private string accessoireBoitier = "";

        private string ventilateurBoitier = "";

        private string processeur = "";

        private string ventirad = "";

        private string waterCooling = "";

        private string carteMere = "";

        private string carteGraphique = "";

        private string accessoireCarteGraphique = "";

        private string ram = "";

        private string disqueSsd = "";

        private string disqueSupplementaire = "";

        private string carteReseau = "";

        private string alimentation = "";

        private string accessoireAlimentation = "";

        private string systemeExploitation = "";

        public int getIdConfig()
        { 
            return idConfig; 
        }

        public void setIdConfig(int IdConfig)
        {
            this.idConfig = IdConfig;
        }

        public DateTime getDateEntree()
        {
            return dateEntree;
        }

        public void setDateEntree(DateTime DateEntree)
        {
            this.dateEntree = DateEntree;
        }

        public DateTime getDateSortie()
        {
            return dateSortie;
        }

        public void setDateSortie(DateTime DateSortie)
        {
            this.dateSortie = DateSortie;
        }


        public string getName()
        {
            return name;
        }

        public void setName(string Name)
        {
            this.name = Name;
        }

        public decimal getPrix()
        {
            return prix;
        }

        public void setPrix(decimal Prix)
        {
            this.prix = Prix;
        }

        public decimal getPrixBarre()
        {
            return prixBarre;
        }

        public void setPrixBarre(decimal PrixBarre)
        {
            this.prixBarre = PrixBarre;
        }

        public string getIdJaja()
        {
            return idJaja;
        }

        public void setIdJaja(string IdJaja)
        {
            this.idJaja = IdJaja;
        }

        public string getWebLink()
        {
            return webLink;
        }

        public void setWebLink(string WebLink)
        {
            this.webLink = WebLink;
        }

        public string getBoitier()
        {
            return boitier;
        }

        public void setBoitier(string Boitier)
        {
            if (Boitier.Contains("Boîtier PC"))
            {
                string[] boitierSplit = Boitier.Split("Boîtier PC");
                Boitier = boitierSplit.Last();
            }
            this.boitier = Boitier.Trim();
        }

        public string getAccessoireBoitier()
        {
            return accessoireBoitier;
        }

        public void setAccessoireBoitier(string AccessoireBoitier)
        {
            string[] boitierSplit = AccessoireBoitier.Split("Accessoire boîtier");
            AccessoireBoitier = boitierSplit.Last();
            this.accessoireBoitier = AccessoireBoitier.Trim();
        }

        public string getVentilateurBoitier()
        {
            return ventilateurBoitier;
        }

        public void setVentilateurBoitier(string VentilateurBoitier)
        {
            string[] ventilateurBoitier = VentilateurBoitier.Split("Ventilateur boîtier");
            VentilateurBoitier = ventilateurBoitier.Last();
            this.ventilateurBoitier = VentilateurBoitier.Trim();
        }

        public string getProcesseur()
        {
            return processeur;
        }

        public void setProcesseur(string Processeur)
        {
            string[] processeurSplit = Processeur.Split("Processeur");
            Processeur = processeurSplit.Last();
            this.processeur = Processeur.Trim();
        }

        public string getVentirad()
        {
            return ventirad;
        }

        public void setVentirad(string Ventirad)
        {
            string[] ventiradSplit = Ventirad.Split("Ventilateur CPU");
            Ventirad = ventiradSplit.Last();
            this.ventirad = Ventirad.Trim();
        }

        public string getWaterCooling()
        {
            return waterCooling.Trim();
        }

        public void setWaterCooling(string WaterCooling)
        {
            string[] watercoolingSplit = WaterCooling.Split("Watercooling");
            WaterCooling = watercoolingSplit.Last();
            this.waterCooling = WaterCooling;
        }

        public string getCarteMere()
        {
            return carteMere;
        }

        public void setCarteMere(string CarteMere)
        {
            if (CarteMere.Contains("Carte mère"))
            {
                string[] cmSplit = CarteMere.Split("Carte mère");
                CarteMere = cmSplit.Last();
            }
            this.carteMere = CarteMere.Trim();
        }

        public string getCarteGraphique()
        {
            return carteGraphique;
        }

        public void setCarteGraphique(string CarteGraphique)
        {
            if (CarteGraphique.Contains("Carte graphique"))
            {
                string[] cgSplit = CarteGraphique.Split("Carte graphique");
                CarteGraphique = cgSplit.Last();
            }
            this.carteGraphique = CarteGraphique.Trim();
        }

        public string getAccessoireCarteGraphique()
        {
            return accessoireCarteGraphique;
        }

        public void setAccessoireCarteGraphique(string AccessoireCarteGraphique)
        {
            string[] accessoireCarteGraphiqueSplit = AccessoireCarteGraphique.Split("Accessoire carte graphique");
            AccessoireCarteGraphique = accessoireCarteGraphiqueSplit.Last();
            this.accessoireCarteGraphique = AccessoireCarteGraphique.Trim();
        }

        public string getRam()
        {
            
            return ram;
        }

        public void setRam(string Ram)
        {
            if (Ram.Contains("Mémoire PC"))
            {
                string[] ramSplit = Ram.Split("Mémoire PC");
                Ram = ramSplit.Last();
            }
            this.ram = Ram.Trim();
        }

        public string getDisqueSsd()
        {
            return disqueSsd;
        }

        public void setDisqueSsd(string DisqueSsd)
        {
            string[] SsdSplit = DisqueSsd.Split("SSD :");
            DisqueSsd = SsdSplit.Last();

            SsdSplit = DisqueSsd.Split("Disque SSD");
            DisqueSsd = SsdSplit.Last();

            this.disqueSsd = DisqueSsd.Trim();
        }

        public string getDisqueSupplementaire()
        {
            return disqueSupplementaire;
        }

        public void setDisqueSupplementaire(string DisqueSupplementaire)
        {
            string[] HDDSplit = DisqueSupplementaire.Split("Disque dur interne 3.5\"");
            DisqueSupplementaire = HDDSplit.Last();
            this.disqueSupplementaire = DisqueSupplementaire.Trim();
        }

        public string getCarteReseau()
        {
            return carteReseau.Trim();
        }

        public void setCarteReseau(string CarteReseau)
        {
            string[] cartereseauSplit = CarteReseau.Split("Carte réseau");
            CarteReseau = cartereseauSplit.Last();
            this.carteReseau = CarteReseau.Trim();
        }

        public string getAlimentation()
        {
            return alimentation;
        }

        public void setAlimentation(string Alimentation)
        {
            if (Alimentation.Contains("Alimentation"))
            {
                string[] alimSplit = Alimentation.Split("Alimentation");
                Alimentation = alimSplit.Last();
            }
            this.alimentation = Alimentation.Trim();
        }

        public string getAccessoireAlimentation()
        {
            return accessoireAlimentation;
        }

        public void setAccessoireAlimentation(string AccessoireAlimentation)
        {
            string[] accessoireAlimSplit = AccessoireAlimentation.Split("Accessoire alimentation");
            AccessoireAlimentation = accessoireAlimSplit.Last();
            this.accessoireAlimentation = AccessoireAlimentation.Trim();
        }

        public string getSystemeExploitation()
        {
            return systemeExploitation.Trim();
        }

        public void setSystemeExploitation(string SystemeExploitation)
        {
            this.systemeExploitation = SystemeExploitation;
        }

        //Debug
        public string getAllCaracteristiques()
        {
            string allCaracteristiques =
                "Id unique de la config : " + idConfig + "\r" +
                "Nom du PC : " + name + "\r" +
                "Prix : " + prix + "\r" +
                "Prix Barré : " + prixBarre + "\r" +
                "ID Jaja : " + idJaja + "\r" +
                "Adresse Web : " + webLink + "\r" +
                "Boîtier : " + boitier + "\r" +
                "Accessoire de boîtier : " + accessoireBoitier + "\r" +
                "Ventilateur de boîtier : " + ventilateurBoitier + "\r" +
                "Processeur : " + processeur + "\r" +
                "Ventirad : " + ventirad + "\r" +
                "Watercooling : " + waterCooling + "\r" +
                "Carte mère : " + carteMere + "\r" +
                "Carte Graphique : " + carteGraphique + "\r" +
                "Accessoire de carte graphique : " + accessoireCarteGraphique + "\r" +
                "Mémoire RAM : " + ram + "\r" +
                "Disque SSD : " + disqueSsd + "\r" +
                "Disque supplémentaire : " + disqueSupplementaire + "\r" +
                "Carte réseau : " + carteReseau + "\r" +
                "Alimentation : " + alimentation + "\r" +
                "Accessoire d'alimentation : " + accessoireAlimentation + "\r" +
                "Système d'exploitation : " + systemeExploitation + "\r";

            return allCaracteristiques;
        }

    }
}
