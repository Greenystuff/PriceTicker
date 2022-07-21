using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace PriceTicker
{
    internal class DatabaseManager 
    {

        SQLiteConnection dbConnection;
        SQLiteCommand command;
        private string sqlCommand;
        string dbPath = Environment.CurrentDirectory + "\\DB";
        string dbFilePath;

        public void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            dbFilePath = dbPath + "\\PC_Gamer.db";

            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }

        public string CreateDbConnection()
        {
            dbFilePath = dbPath + "\\PC_Gamer.db";
            string strCon = string.Format("Data Source={0}", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return strCon;
        }

        public SQLiteConnection Connection()
        {
            dbFilePath = dbPath + "\\PC_Gamer.db";
            string strCon = string.Format("Data Source={0}", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return dbConnection;
        }

        public void CloseDbConnection()
        {
            dbConnection.Close();
        }

        public void InsertPCGamer(int idPcGamer ,string name, string prix, string prixBarre, string webLink)
        {
            string insertQuery = "INSERT INTO PcGamer(IdPcGamer,Name,Prix,PrixBarre,WebLink,DateEntree)"
                                + "VALUES('"
                                + idPcGamer + "','"
                                + name + "','"
                                + prix + "','"
                                + prixBarre + "','"
                                + webLink + "',"
                                + "datetime('now'));";
            CreateDbConnection();
            ExecuteQuery(insertQuery);
            CloseDbConnection();
        }

        public void InsertComposants(string typeComposant, string nomComposant)
        {
           
            string checkQuery = "SELECT * FROM Composants WHERE TypeComposant ='" + typeComposant + "'"
                            + "AND NomComposant='" + nomComposant + "';";
            
                CreateDbConnection();

                SQLiteDataReader sqldr = ExecuteQueryWithReturn(checkQuery);

                bool hasRows = sqldr.HasRows;
                sqldr.Close();
                CloseDbConnection();
            
            if (!hasRows)
            {;

                string insertQuery = "INSERT INTO Composants(TypeComposant,NomComposant)"
                                    + "VALUES('"
                                    + typeComposant + "','"
                                    + nomComposant + "');";
                CreateDbConnection();
                ExecuteQueryWithIntReturn(insertQuery);
                CloseDbConnection();
            }

            
        }

        public void InsertPcGamerComposants(int idPcGamer, string nomPcGamer, int idComposants, string typeComposant, string nomComposant)
        {
            string insertQuery = "INSERT INTO ComposantsPcGamer(IdPcGamer,NomPcGamer,IdComposant,TypeComposant,NomComposant)"
                                + "VALUES('"
                                + idPcGamer + "','"
                                + nomPcGamer + "','"
                                + idComposants + "','"
                                + typeComposant + "','"
                                + nomComposant 
                                + "');";
            CreateDbConnection();
            ExecuteQuery(insertQuery);
            CloseDbConnection();
        }

        public void CreateTables()
        {
            
            if (!CheckIfExist("PcGamer"))
            {
                sqlCommand = "CREATE TABLE PcGamer("
                             + "IdPcGamer INTEGER PRIMARY KEY,"
                             + "Name NVARCHAR(100) NOT NULL,"
                             + "Prix NVARCHAR(100) NOT NULL,"
                             + "PrixBarre NVARCHAR(100) NOT NULL,"
                             + "WebLink NVARCHAR(100) NOT NULL,"
                             + "DateEntree DATETIME"
                             + ");";
                ExecuteQuery(sqlCommand);
            }
            if (!CheckIfExist("Composants"))
            {
                sqlCommand = "CREATE TABLE Composants("
                             + "IdComposant INTEGER PRIMARY KEY AUTOINCREMENT,"
                             + "TypeComposant TEXT CHECK(TypeComposant IN ( 'Boîtier',"
                                                                            + "'Accessoire de boîtier',"
                                                                            + "'Ventilateurs',"
                                                                            + "'Processeur',"
                                                                            + "'Ventirad',"
                                                                            + "'Watercooling',"
                                                                            + "'Carte mère',"
                                                                            + "'Carte Graphique',"
                                                                            + "'Accessoire de Carte Graphique',"
                                                                            + "'Mémoire vive',"
                                                                            + "'SSD',"
                                                                            + "'HDD',"
                                                                            + "'Carte réseau',"
                                                                            + "'Alimentation',"
                                                                            + "'Accessoire alimentation',"
                                                                            + "'Système exploitation'"
                                                                            + ") ) NOT NULL,"
                             + "NomComposant NVARCHAR(100) NOT NULL"
                             +");";
                ExecuteQuery(sqlCommand);
            }
            if (!CheckIfExist("ComposantsPcGamer"))
            {
                sqlCommand = "CREATE TABLE ComposantsPcGamer("
                             + "Id INTEGER PRIMARY KEY AUTOINCREMENT,"
                             + "IdPcGamer INTEGER,"
                             + "NomPcGamer NVARCHAR(100) NOT NULL,"
                             + "IdComposant INTEGER,"
                             + "TypeComposant NVARCHAR(100) NOT NULL,"
                             + "NomComposant NVARCHAR(100) NOT NULL"
                             + ");";
                ExecuteQuery(sqlCommand);
            }

            if (!CheckIfExist("ArchivesPcGamer"))
            {
                sqlCommand = "CREATE TABLE ArchivesPcGamer("
                             + "IdPcGamer INTEGER,"
                             + "Name NVARCHAR(100) NOT NULL,"
                             + "Prix NVARCHAR(100) NOT NULL,"
                             + "PrixBarre NVARCHAR(100) NOT NULL,"
                             + "WebLink NVARCHAR(100) NOT NULL,"
                             + "DateEntree DATETIME,"
                             + "DateSortie DATETIME,"
                             + "PRIMARY KEY ('IdPcGamer','DateEntree','DateSortie')"
                             + ");";
                ExecuteQuery(sqlCommand);
            }
            
            if (!CheckIfExist("ArchivesComposantsPcGamer"))
            {
                sqlCommand = "CREATE TABLE ArchivesComposantsPcGamer("
                             + "IdPcGamer INTEGER,"
                             + "IdComposant INTEGER,"
                             + "DateEntree DATETIME,"
                             + "DateSortie DATETIME,"
                             + "PRIMARY KEY ('IdPcGamer','IdComposant','DateEntree','DateSortie')"
                             + ");";
                ExecuteQuery(sqlCommand);
            }
        }

        public bool CheckIfExist(string tableName)
        {
            
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            object result = command.ExecuteScalar();

            return result != null && result.ToString() == tableName;
        }

        public void ExecuteQuery(string sqlCommand)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = sqlCommand;
            triggerCommand.ExecuteNonQuery();
        }

        public bool CheckIfTableContainsData(string tableName)
        {
            CreateDbConnection();
            command.CommandText = "SELECT count(*) FROM " + tableName;
            var result = command.ExecuteScalar();
            CloseDbConnection();

            return Convert.ToInt32(result) > 0;
        }

        public int ExecuteQueryWithIntReturn(string query)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = query;
            var result = triggerCommand.ExecuteScalar();

            return Convert.ToInt32(result);
        }

        public SQLiteDataReader ExecuteQueryWithReturn(string select)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = select;
            SQLiteDataReader reader = triggerCommand.ExecuteReader();
            return reader;
        }

        public void DeletePcGamer()
        {
            string cmd = "DELETE FROM PcGamer";
            CreateDbConnection();
            ExecuteQuery(cmd);
            CloseDbConnection();
        }

        public int GetLastIDPcGamerComposants(string TypeComposant, string NomComposant)
        {
            string insertQuery = "SELECT IdComposant FROM Composants WHERE TypeComposant='" + TypeComposant + "'"
                + " AND NomComposant='" + NomComposant + "'" + ";";
            CreateDbConnection();
            int lastID = ExecuteQueryWithIntReturn(insertQuery);
            CloseDbConnection();
            return lastID;
        }

        public List<int> SelectAllIdPcGamer()
        {
            string select = "SELECT IdPcGamer FROM PcGamer";
            CreateDbConnection();
            SQLiteDataReader reader = ExecuteQueryWithReturn(select);
            List<int> IdPcGamer = new();
            while (reader.Read())
            {
                IdPcGamer.Add(int.Parse(reader["IdPcGamer"].ToString()));
            }
            reader.Close();

            CloseDbConnection();
            return IdPcGamer;
        }

        public List<int> SelectAllIdPcGamerArchived()
        {
            string select = "SELECT IdPcGamer FROM ArchivesPcGamer";
            CreateDbConnection();
            SQLiteDataReader reader = ExecuteQueryWithReturn(select);
            List<int> IdPcGamer = new();
            while (reader.Read())
            {
                IdPcGamer.Add(int.Parse(reader["IdPcGamer"].ToString()));
            }
            reader.Close();

            CloseDbConnection();
            return IdPcGamer;
        }

        public Models.PcGamer SelectPcGamerByID(int IdConfig)
        {
            Models.PcGamer PcGamer = new();
            PcGamer.setIdConfig(IdConfig);
            string selectPcGamer = "SELECT Name,Prix,PrixBarre,WebLink,DateEntree FROM PcGamer WHERE IdPcGamer=" + IdConfig;
            CreateDbConnection();
            SQLiteDataReader PcGamerReader = ExecuteQueryWithReturn(selectPcGamer);

            while (PcGamerReader.Read())
            {
                PcGamer.setName(PcGamerReader["Name"].ToString());
                PcGamer.setPrix(decimal.Parse(PcGamerReader["Prix"].ToString()));
                PcGamer.setPrixBarre(decimal.Parse(PcGamerReader["PrixBarre"].ToString()));
                PcGamer.setWebLink(PcGamerReader["WebLink"].ToString());
                PcGamer.setDateEntree(DateTime.Parse(PcGamerReader["DateEntree"].ToString()));

            }
            PcGamerReader.Close();
            CloseDbConnection();



            string selectIdComposant = "SELECT IdComposant FROM ComposantsPcGamer WHERE IdPcGamer=" + IdConfig;
            CreateDbConnection();
            SQLiteDataReader ComposantPcGamerReader = ExecuteQueryWithReturn(selectIdComposant);
            List<int> idComposants = new();
            
            while (ComposantPcGamerReader.Read())
            {
                idComposants.Add(int.Parse(ComposantPcGamerReader["IdComposant"].ToString()));
            }
            ComposantPcGamerReader.Close();
            CloseDbConnection();
            
            
            for (int i = 0; i < idComposants.Count; i++)
            {                
                string selectComposants = "SELECT TypeComposant,NomComposant FROM Composants WHERE IdComposant=" + idComposants[i];
                CreateDbConnection();
                SQLiteDataReader ComposantReader = ExecuteQueryWithReturn(selectComposants);
                while (ComposantReader.Read())
                {
                    if(ComposantReader["TypeComposant"].ToString() == "Boîtier")
                    {
                        PcGamer.setBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire de boîtier")
                    {
                        PcGamer.setAccessoireBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Ventilateurs")
                    {
                        PcGamer.setVentilateurBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Processeur")
                    {
                        PcGamer.setProcesseur(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Ventirad")
                    {
                        PcGamer.setVentirad(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Watercooling")
                    {
                        PcGamer.setWaterCooling(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte mère")
                    {
                        PcGamer.setCarteMere(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte Graphique")
                    {
                        PcGamer.setCarteGraphique(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire de Carte Graphique")
                    {
                        PcGamer.setAccessoireCarteGraphique(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Mémoire vive")
                    {
                        PcGamer.setRam(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "SSD")
                    {
                        PcGamer.setDisqueSsd(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "HDD")
                    {
                        PcGamer.setDisqueSupplementaire(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte réseau")
                    {
                        PcGamer.setCarteReseau(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Alimentation")
                    {
                        PcGamer.setAlimentation(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire alimentation")
                    {
                        PcGamer.setAccessoireAlimentation(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Système exploitation")
                    {
                        PcGamer.setSystemeExploitation(ComposantReader["NomComposant"].ToString());
                    }
                }
                ComposantReader.Close();
                CloseDbConnection();
            }
            return PcGamer;
        }

        public Models.PcGamer SelectArchivedPcGamerByID(int IdConfig)
        {
            Models.PcGamer PcGamer = new();
            PcGamer.setIdConfig(IdConfig);
            string selectPcGamer = "SELECT Name,Prix,PrixBarre,WebLink,DateEntree,DateSortie FROM ArchivesPcGamer WHERE IdPcGamer=" + IdConfig;
            CreateDbConnection();
            SQLiteDataReader PcGamerReader = ExecuteQueryWithReturn(selectPcGamer);

            while (PcGamerReader.Read())
            {
                PcGamer.setName(PcGamerReader["Name"].ToString());
                PcGamer.setPrix(decimal.Parse(PcGamerReader["Prix"].ToString()));
                PcGamer.setPrixBarre(decimal.Parse(PcGamerReader["PrixBarre"].ToString()));
                PcGamer.setWebLink(PcGamerReader["WebLink"].ToString());
                PcGamer.setDateEntree(DateTime.ParseExact(PcGamerReader["DateEntree"].ToString().Trim(), @"dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
                PcGamer.setDateSortie(DateTime.ParseExact(PcGamerReader["DateSortie"].ToString().Trim(), @"dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

            }
            PcGamerReader.Close();
            CloseDbConnection();



            string selectIdComposant = "SELECT IdComposant FROM ArchivesComposantsPcGamer WHERE IdPcGamer=" + IdConfig;
            CreateDbConnection();
            SQLiteDataReader ComposantPcGamerReader = ExecuteQueryWithReturn(selectIdComposant);
            List<int> idComposants = new();

            while (ComposantPcGamerReader.Read())
            {
                idComposants.Add(int.Parse(ComposantPcGamerReader["IdComposant"].ToString()));
            }
            ComposantPcGamerReader.Close();
            CloseDbConnection();


            for (int i = 0; i < idComposants.Count; i++)
            {
                string selectComposants = "SELECT TypeComposant,NomComposant FROM Composants WHERE IdComposant=" + idComposants[i];
                CreateDbConnection();
                SQLiteDataReader ComposantReader = ExecuteQueryWithReturn(selectComposants);
                while (ComposantReader.Read())
                {
                    if (ComposantReader["TypeComposant"].ToString() == "Boîtier")
                    {
                        PcGamer.setBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire de boîtier")
                    {
                        PcGamer.setAccessoireBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Ventilateurs")
                    {
                        PcGamer.setVentilateurBoitier(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Processeur")
                    {
                        PcGamer.setProcesseur(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Ventirad")
                    {
                        PcGamer.setVentirad(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Watercooling")
                    {
                        PcGamer.setWaterCooling(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte mère")
                    {
                        PcGamer.setCarteMere(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte Graphique")
                    {
                        PcGamer.setCarteGraphique(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire de Carte Graphique")
                    {
                        PcGamer.setAccessoireCarteGraphique(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Mémoire vive")
                    {
                        PcGamer.setRam(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "SSD")
                    {
                        PcGamer.setDisqueSsd(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "HDD")
                    {
                        PcGamer.setDisqueSupplementaire(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Carte réseau")
                    {
                        PcGamer.setCarteReseau(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Alimentation")
                    {
                        PcGamer.setAlimentation(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Accessoire alimentation")
                    {
                        PcGamer.setAccessoireAlimentation(ComposantReader["NomComposant"].ToString());
                    }
                    if (ComposantReader["TypeComposant"].ToString() == "Système exploitation")
                    {
                        PcGamer.setSystemeExploitation(ComposantReader["NomComposant"].ToString());
                    }
                }
                ComposantReader.Close();
                CloseDbConnection();
            }
            return PcGamer;
        }

        public List<Models.PcGamer> SelectArchivedPcGamerByDates(DateTime DateEntree, DateTime DateSortie)
        {
            List<Models.PcGamer> ArchivedProductsFound = new();

            
            string selectPcGamer = "SELECT IdPcGamer,Name,Prix,PrixBarre,WebLink,DateEntree,DateSortie FROM ArchivesPcGamer";
            CreateDbConnection();
            SQLiteDataReader PcGamerReader = ExecuteQueryWithReturn(selectPcGamer);

            while (PcGamerReader.Read())
            {
                DateTime foundStartDate = DateTime.Parse(PcGamerReader["DateEntree"].ToString());
                DateTime foundEndDate = DateTime.Parse(PcGamerReader["DateSortie"].ToString());

                if (foundStartDate >= DateEntree && foundEndDate <= DateSortie)
                {
                    Models.PcGamer PcGamer = new();

                    PcGamer.setIdConfig(int.Parse(PcGamerReader["IdPcGamer"].ToString()));
                    PcGamer.setName(PcGamerReader["Name"].ToString());
                    PcGamer.setPrix(decimal.Parse(PcGamerReader["Prix"].ToString()));
                    PcGamer.setPrixBarre(decimal.Parse(PcGamerReader["PrixBarre"].ToString()));
                    PcGamer.setWebLink(PcGamerReader["WebLink"].ToString());
                    PcGamer.setDateEntree(DateTime.Parse(PcGamerReader["DateEntree"].ToString()));
                    PcGamer.setDateSortie(DateTime.Parse(PcGamerReader["DateSortie"].ToString()));

                    ArchivedProductsFound.Add(PcGamer);
                }

            }
            PcGamerReader.Close();
            CloseDbConnection();

            for(int j = 0; j < ArchivedProductsFound.Count; j++)
            {
                string selectIdComposant = "SELECT IdComposant FROM ArchivesComposantsPcGamer WHERE IdPcGamer=" + ArchivedProductsFound[j].getIdConfig();
                CreateDbConnection();
                SQLiteDataReader ComposantPcGamerReader = ExecuteQueryWithReturn(selectIdComposant);
                List<int> idComposants = new();

                while (ComposantPcGamerReader.Read())
                {
                    idComposants.Add(int.Parse(ComposantPcGamerReader["IdComposant"].ToString()));
                }
                ComposantPcGamerReader.Close();
                CloseDbConnection();

                for (int i = 0; i < idComposants.Count; i++)
                {
                    string selectComposants = "SELECT TypeComposant,NomComposant FROM Composants WHERE IdComposant=" + idComposants[i];
                    CreateDbConnection();
                    SQLiteDataReader ComposantReader = ExecuteQueryWithReturn(selectComposants);
                    while (ComposantReader.Read())
                    {
                        if (ComposantReader["TypeComposant"].ToString() == "Boîtier")
                        {
                            ArchivedProductsFound[j].setBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire de boîtier")
                        {
                            ArchivedProductsFound[j].setAccessoireBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Ventilateurs")
                        {
                            ArchivedProductsFound[j].setVentilateurBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Processeur")
                        {
                            ArchivedProductsFound[j].setProcesseur(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Ventirad")
                        {
                            ArchivedProductsFound[j].setVentirad(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Watercooling")
                        {
                            ArchivedProductsFound[j].setWaterCooling(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte mère")
                        {
                            ArchivedProductsFound[j].setCarteMere(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte Graphique")
                        {
                            ArchivedProductsFound[j].setCarteGraphique(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire de Carte Graphique")
                        {
                            ArchivedProductsFound[j].setAccessoireCarteGraphique(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Mémoire vive")
                        {
                            ArchivedProductsFound[j].setRam(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "SSD")
                        {
                            ArchivedProductsFound[j].setDisqueSsd(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "HDD")
                        {
                            ArchivedProductsFound[j].setDisqueSupplementaire(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte réseau")
                        {
                            ArchivedProductsFound[j].setCarteReseau(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Alimentation")
                        {
                            ArchivedProductsFound[j].setAlimentation(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire alimentation")
                        {
                            ArchivedProductsFound[j].setAccessoireAlimentation(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Système exploitation")
                        {
                            ArchivedProductsFound[j].setSystemeExploitation(ComposantReader["NomComposant"].ToString());
                        }
                    }
                    ComposantReader.Close();
                    CloseDbConnection();
                }

            }

            


            
            return ArchivedProductsFound;
        }

        public List<Models.PcGamer> SelectArchivedPcGamerByName(string ConfigName)
        {
            List<Models.PcGamer> ArchivedProductsFound = new();

            string selectPcGamer = "SELECT IdPcGamer,Name,Prix,PrixBarre,WebLink,DateEntree,DateSortie FROM ArchivesPcGamer";
            CreateDbConnection();
            SQLiteDataReader PcGamerReader = ExecuteQueryWithReturn(selectPcGamer);

            while (PcGamerReader.Read())
            {
                string configName = PcGamerReader["Name"].ToString();

                if (configName.Contains(ConfigName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Models.PcGamer PcGamer = new();

                    PcGamer.setIdConfig(int.Parse(PcGamerReader["IdPcGamer"].ToString()));
                    PcGamer.setName(PcGamerReader["Name"].ToString());
                    PcGamer.setPrix(decimal.Parse(PcGamerReader["Prix"].ToString()));
                    PcGamer.setPrixBarre(decimal.Parse(PcGamerReader["PrixBarre"].ToString()));
                    PcGamer.setWebLink(PcGamerReader["WebLink"].ToString());
                    PcGamer.setDateEntree(DateTime.Parse(PcGamerReader["DateEntree"].ToString()));
                    PcGamer.setDateSortie(DateTime.Parse(PcGamerReader["DateSortie"].ToString()));

                    ArchivedProductsFound.Add(PcGamer);
                }

            }
            PcGamerReader.Close();
            CloseDbConnection();

            for (int j = 0; j < ArchivedProductsFound.Count; j++)
            {
                string selectIdComposant = "SELECT IdComposant FROM ArchivesComposantsPcGamer WHERE IdPcGamer=" + ArchivedProductsFound[j].getIdConfig();
                CreateDbConnection();
                SQLiteDataReader ComposantPcGamerReader = ExecuteQueryWithReturn(selectIdComposant);
                List<int> idComposants = new();

                while (ComposantPcGamerReader.Read())
                {
                    idComposants.Add(int.Parse(ComposantPcGamerReader["IdComposant"].ToString()));
                }
                ComposantPcGamerReader.Close();
                CloseDbConnection();

                for (int i = 0; i < idComposants.Count; i++)
                {
                    string selectComposants = "SELECT TypeComposant,NomComposant FROM Composants WHERE IdComposant=" + idComposants[i];
                    CreateDbConnection();
                    SQLiteDataReader ComposantReader = ExecuteQueryWithReturn(selectComposants);
                    while (ComposantReader.Read())
                    {
                        if (ComposantReader["TypeComposant"].ToString() == "Boîtier")
                        {
                            ArchivedProductsFound[j].setBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire de boîtier")
                        {
                            ArchivedProductsFound[j].setAccessoireBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Ventilateurs")
                        {
                            ArchivedProductsFound[j].setVentilateurBoitier(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Processeur")
                        {
                            ArchivedProductsFound[j].setProcesseur(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Ventirad")
                        {
                            ArchivedProductsFound[j].setVentirad(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Watercooling")
                        {
                            ArchivedProductsFound[j].setWaterCooling(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte mère")
                        {
                            ArchivedProductsFound[j].setCarteMere(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte Graphique")
                        {
                            ArchivedProductsFound[j].setCarteGraphique(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire de Carte Graphique")
                        {
                            ArchivedProductsFound[j].setAccessoireCarteGraphique(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Mémoire vive")
                        {
                            ArchivedProductsFound[j].setRam(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "SSD")
                        {
                            ArchivedProductsFound[j].setDisqueSsd(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "HDD")
                        {
                            ArchivedProductsFound[j].setDisqueSupplementaire(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Carte réseau")
                        {
                            ArchivedProductsFound[j].setCarteReseau(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Alimentation")
                        {
                            ArchivedProductsFound[j].setAlimentation(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Accessoire alimentation")
                        {
                            ArchivedProductsFound[j].setAccessoireAlimentation(ComposantReader["NomComposant"].ToString());
                        }
                        if (ComposantReader["TypeComposant"].ToString() == "Système exploitation")
                        {
                            ArchivedProductsFound[j].setSystemeExploitation(ComposantReader["NomComposant"].ToString());
                        }
                    }
                    ComposantReader.Close();
                    CloseDbConnection();
                }

            }





            return ArchivedProductsFound;
        }

        public void UpdatePcGamerByID(int idConfig, string type, string value)
        {
            string insertQuery = "UPDATE PcGamer SET '" + type + "' = '" + value + "' WHERE IdPcGamer = " + idConfig + ";";
            CreateDbConnection();
            ExecuteQuery(insertQuery);
            CloseDbConnection();
        }

        public void UpdatePcGamerComposantByID(int idPcGamer, string nomPcGamer, int idComposant, string typeComposant, string nomComposant)
        {
            
                string insertQuery = "UPDATE ComposantsPcGamer SET NomPcGamer ='" + nomPcGamer + "',IdComposant=" + idComposant + ",NomComposant='" + nomComposant + "' WHERE IdPcGamer = " + idPcGamer + " AND TypeComposant ='" + typeComposant + "';"; 
                CreateDbConnection();
                ExecuteQuery(insertQuery);
                CloseDbConnection();
            
        }

        public int FindComposantIdByName(string name)
        {
            string selectIdComposant = "SELECT IdComposant FROM Composants WHERE NomComposant='" + name + "';";
            CreateDbConnection();
            SQLiteDataReader ComposantReader = ExecuteQueryWithReturn(selectIdComposant);
            int idComposant = -1;

            while (ComposantReader.Read())
            {
                idComposant = int.Parse(ComposantReader["IdComposant"].ToString());
            }
            ComposantReader.Close();
            CloseDbConnection();
            return idComposant;
        }

        public void DeleteConfigByID(int rowID)
        {
            string cmd = "DELETE FROM PcGamer WHERE IdPcGamer=" + rowID;
            CreateDbConnection();
            ExecuteQuery(cmd);
            CloseDbConnection();

            cmd = "DELETE FROM ComposantsPcGamer WHERE IdPcGamer=" + rowID;
            CreateDbConnection();
            ExecuteQuery(cmd);
            CloseDbConnection();
        }

        public bool ArchiveConfigByID(int rowID)
        {
            Models.PcGamer pcGamer = SelectPcGamerByID(rowID);
            bool archivePcGamerSucced = false;

            string dateEntree = pcGamer.getDateEntree().ToString("yyyy-MM-dd HH:mm:ss");
            string dateSortie = "datetime('now')";
            string insertQuery = "INSERT INTO ArchivesPcGamer(IdPcGamer,Name,Prix,PrixBarre,WebLink,DateEntree,DateSortie)"
                                + "VALUES('"
                                + pcGamer.getIdConfig() + "','"
                                + pcGamer.getName() + "','"
                                + pcGamer.getPrix().ToString() + "','"
                                + pcGamer.getPrixBarre().ToString() + "','"
                                + pcGamer.getWebLink() + "','"
                                + dateEntree + "',"
                                + dateSortie + ");";
            CreateDbConnection();
            try
            {
                ExecuteQuery(insertQuery);
                archivePcGamerSucced = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lors de l'insertion de l'archive du PC Gamer : " + ex.Message);
                archivePcGamerSucced = false;
            }
            
            CloseDbConnection();

            if(archivePcGamerSucced)
            {
                string selectQuery = "SELECT * FROM ComposantsPcGamer WHERE IdPcGamer = " + rowID;
                CreateDbConnection();
                SQLiteDataReader dataReader = ExecuteQueryWithReturn(selectQuery);


                List<int> composantsPcGamer = new List<int>();
                int index = 0;

                while (dataReader.Read())
                {
                    composantsPcGamer.Add(int.Parse(dataReader["IdComposant"].ToString()));
                    Debug.WriteLine("Id composant trouvé : " + composantsPcGamer[index]);
                    index++;
                }

                dataReader.Close();
                CloseDbConnection();

                /* Insertion dans la table d'archives */
                for (int i = 0; i < composantsPcGamer.Count(); i++)
                {
                    string insertComposantQuery = "INSERT INTO ArchivesComposantsPcGamer(IdPcGamer,IdComposant,DateEntree,DateSortie)"
                                    + "VALUES('"
                                    + pcGamer.getIdConfig() + "','"
                                    + composantsPcGamer[i] + "','"
                                    + dateEntree + "',"
                                    + dateSortie + ");";
                    CreateDbConnection();
                    ExecuteQuery(insertComposantQuery);
                    CloseDbConnection();
                }
            }

            

            //nbComposants

            return archivePcGamerSucced;
        }

    }
}
