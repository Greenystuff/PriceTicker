using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTicker.Models
{
    internal class PcGamer
    {

        private string name = "";

        private decimal prix = 0;

        private decimal prixBarre = 0;

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



        public string getName()
        {
            return name;
        }

        public void setname(string Name)
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
            this.boitier = Boitier;
        }

        public string getAccessoireBoitier()
        {
            return accessoireBoitier;
        }

        public void setAccessoireBoitier(string AccessoireBoitier)
        {
            this.accessoireBoitier = AccessoireBoitier;
        }

        public string getVentilateurBoitier()
        {
            return ventilateurBoitier;
        }

        public void setVentilateurBoitier(string VentilateurBoitier)
        {
            this.ventilateurBoitier = VentilateurBoitier;
        }

        public string getProcesseur()
        {
            return processeur;
        }

        public void setProcesseur(string Processeur)
        {
            this.processeur = Processeur;
        }

        public string getVentirad()
        {
            return ventirad;
        }

        public void setVentirad(string Ventirad)
        {
            this.ventirad = Ventirad;
        }

        public string getWaterCooling()
        {
            return waterCooling;
        }

        public void setWaterCooling(string WaterCooling)
        {
            this.waterCooling = WaterCooling;
        }

        public string getCarteMere()
        {
            return carteMere;
        }

        public void setCarteMere(string CarteMere)
        {
            this.carteMere = CarteMere;
        }

        public string getCarteGraphique()
        {
            return carteGraphique;
        }

        public void setCarteGraphique(string CarteGraphique)
        {
            this.carteGraphique = CarteGraphique;
        }

        public string getAccessoireCarteGraphique()
        {
            return accessoireCarteGraphique;
        }

        public void setAccessoireCarteGraphique(string AccessoireCarteGraphique)
        {
            this.accessoireCarteGraphique = AccessoireCarteGraphique;
        }

        public string getRam()
        {
            return ram;
        }

        public void setRam(string Ram)
        {
            this.ram = Ram;
        }

        public string getDisqueSsd()
        {
            return disqueSsd;
        }

        public void setDisqueSsd(string DisqueSsd)
        {
            this.disqueSsd = DisqueSsd;
        }

        public string getDisqueSupplementaire()
        {
            return disqueSupplementaire;
        }

        public void setDisqueSupplementaire(string DisqueSupplementaire)
        {
            this.disqueSupplementaire = DisqueSupplementaire;
        }

        public string getCarteReseau()
        {
            return carteReseau;
        }

        public void setCarteReseau(string CarteReseau)
        {
            this.carteReseau = CarteReseau;
        }

        public string getAlimentation()
        {
            return alimentation;
        }

        public void setAlimentation(string Alimentation)
        {
            this.alimentation = Alimentation;
        }

        public string getAccessoireAlimentation()
        {
            return accessoireAlimentation;
        }

        public void setAccessoireAlimentation(string AccessoireAlimentation)
        {
            this.accessoireAlimentation = AccessoireAlimentation;
        }

        public string getSystemeExploitation()
        {
            return systemeExploitation;
        }

        public void setSystemeExploitation(string SystemeExploitation)
        {
            this.systemeExploitation = SystemeExploitation;
        }

        public string getAllCaracteristiques()
        {
            string allCaracteristiques =
                "Nom du PC : " + name + "\r" +
                "Prix : " + prix + "\r" +
                "Prix Barré : " + prixBarre + "\r" +
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
