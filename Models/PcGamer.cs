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
            if (boitier.Contains("Boîtier PC"))
            {
                string[] boitierSplit = boitier.Split("Boîtier PC ");
                boitier = boitierSplit[1];
            }
            return boitier.Trim();
        }

        public void setBoitier(string Boitier)
        {
            this.boitier = Boitier;
        }

        public string getAccessoireBoitier()
        {
            return accessoireBoitier.Trim();
        }

        public void setAccessoireBoitier(string AccessoireBoitier)
        {
            this.accessoireBoitier = AccessoireBoitier;
        }

        public string getVentilateurBoitier()
        {
            return ventilateurBoitier.Trim();
        }

        public void setVentilateurBoitier(string VentilateurBoitier)
        {
            this.ventilateurBoitier = VentilateurBoitier;
        }

        public string getProcesseur()
        {
            if (processeur.Contains("Processeur"))
            {
                string[] processeurSplit = processeur.Split("Processeur ");
                processeur = processeurSplit.Last();
            }

            return processeur.Trim();
        }

        public void setProcesseur(string Processeur)
        {
            this.processeur = Processeur;
        }

        public string getVentirad()
        {
            return ventirad.Trim();
        }

        public void setVentirad(string Ventirad)
        {
            this.ventirad = Ventirad;
        }

        public string getWaterCooling()
        {
            return waterCooling.Trim();
        }

        public void setWaterCooling(string WaterCooling)
        {
            this.waterCooling = WaterCooling;
        }

        public string getCarteMere()
        {
            if (carteMere.Contains("Carte mère"))
            {
                string[] cmSplit = carteMere.Split("Carte mère ");
                carteMere = cmSplit.Last();
            }
            return carteMere.Trim();
        }

        public void setCarteMere(string CarteMere)
        {
            this.carteMere = CarteMere;
        }

        public string getCarteGraphique()
        {
            if (carteGraphique.Contains("Carte graphique"))
            {
                string[] cgSplit = carteGraphique.Split("Carte graphique ");
                carteGraphique = cgSplit.Last();
            }
            return carteGraphique.Trim();
        }

        public void setCarteGraphique(string CarteGraphique)
        {
            this.carteGraphique = CarteGraphique;
        }

        public string getAccessoireCarteGraphique()
        {
            return accessoireCarteGraphique.Trim();
        }

        public void setAccessoireCarteGraphique(string AccessoireCarteGraphique)
        {
            this.accessoireCarteGraphique = AccessoireCarteGraphique;
        }

        public string getRam()
        {
            if (ram.Contains("Mémoire PC"))
            {
                string[] ramSplit = ram.Split("Mémoire PC ");
                ram = ramSplit.Last();
            }
            return ram.Trim();
        }

        public void setRam(string Ram)
        {
            this.ram = Ram;
        }

        public string getDisqueSsd()
        {
            return disqueSsd.Trim();
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
            return disqueSupplementaire.Trim();
        }

        public void setDisqueSupplementaire(string DisqueSupplementaire)
        {
            this.disqueSupplementaire = DisqueSupplementaire;
        }

        public string getCarteReseau()
        {
            return carteReseau.Trim();
        }

        public void setCarteReseau(string CarteReseau)
        {
            this.carteReseau = CarteReseau;
        }

        public string getAlimentation()
        {
            if (alimentation.Contains("Alimentation"))
            {
                string[] alimSplit = alimentation.Split("Alimentation ");
                alimentation = alimSplit.Last();
            }
            return alimentation.Trim();
        }

        public void setAlimentation(string Alimentation)
        {
            this.alimentation = Alimentation;
        }

        public string getAccessoireAlimentation()
        {
            return accessoireAlimentation.Trim();
        }

        public void setAccessoireAlimentation(string AccessoireAlimentation)
        {
            this.accessoireAlimentation = AccessoireAlimentation;
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
