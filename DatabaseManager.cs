using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PriceTicker
{
    internal class DatabaseManager
    {

        SQLiteConnection dbConnection;
        SQLiteCommand command;
        private string sqlCommand;
        string dbPath = Environment.CurrentDirectory + "\\DB";
        private string dbFilePath;

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
            string strCon = string.Format("Data Source={0}", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return strCon;
        }

        public SQLiteConnection Connection()
        {
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

        public void InsertPCGamer(int idPcGamer ,string name, string prix, string prixBarre, string webLink, DateTime dateEntree)
        {
            string insertQuery = "INSERT INTO PcGamer(IdPcGamer,Name,Prix,PrixBarre,WebLink,DateEntree)"
                                + "VALUES('"
                                + idPcGamer + "','"
                                + name + "','"
                                + prix + "','"
                                + prixBarre + "','"
                                + webLink + "','"
                                + dateEntree + "');";
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

        public int GetLastIDPcGamerComposants(string TypeComposant, string NomComposant)
        {
            string insertQuery = "SELECT IdComposant FROM Composants WHERE TypeComposant='"+ TypeComposant + "'" 
                + " AND NomComposant='"+ NomComposant + "'"+";";
            CreateDbConnection();
            int lastID = ExecuteQueryWithIntReturn(insertQuery);
            CloseDbConnection();
            return lastID;
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

            /*if (!CheckIfExist("ArchivesPcGamer"))
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
            if (!CheckIfExist("ArchivesComposants"))
            {
                sqlCommand = "CREATE TABLE ArchivesComposants("
                             + "IdComposant INTEGER PRIMARY KEY AUTOINCREMENT,"
                             + "TypeComposant TEXT CHECK(TypeComposant IN ( 'Boîtier',"
                                                                            +"'Accessoire de boîtier',"
                                                                            +"'Ventilateurs',"
                                                                            +"'Processeur',"
                                                                            +"'Ventirad',"
                                                                            +"'Watercooling',"
                                                                            +"'Carte mère',"
                                                                            +"'Carte Graphique',"
                                                                            +"'Accessoire de Carte Graphique',"
                                                                            +"'Mémoire vive',"
                                                                            +"'SSD',"
                                                                            +"'HDD',"
                                                                            +"'Carte réseau',"
                                                                            +"'Alimentation',"
                                                                            +"'Accessoire alimentation',"
                                                                            +"'Système Exploitation',"
                                                                            +") ) NOT NULL,"
                             + "NomComposant NVARCHAR(100) NOT NULL"
                             + ");";
                ExecuteQuery(sqlCommand);
            }
            if (!CheckIfExist("ArchivesComposantsPcGamer"))
            {
                sqlCommand = "CREATE TABLE ArchivesComposantsPcGamer("
                             + "Id INTEGER PRIMARY KEY AUTOINCREMENT,"
                             + "IdPcGamer INTEGER,"
                             + "IdComposant INTEGER"
                             + ");";
                ExecuteQuery(sqlCommand);
            }*/
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
            CreateDbFile();
            CreateDbConnection();
            ExecuteQuery(cmd);
            CloseDbConnection();
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

        public Models.PcGamer SelectPcGamerByID(int IdConfig)
        {
            Models.PcGamer PcGamer = new();
            PcGamer.setIdConfig(IdConfig);
            string selectPcGamer = "SELECT Name,Prix,PrixBarre,WebLink FROM PcGamer WHERE IdPcGamer=" + IdConfig;
            CreateDbConnection();
            SQLiteDataReader PcGamerReader = ExecuteQueryWithReturn(selectPcGamer);

            while (PcGamerReader.Read())
            {
                PcGamer.setName(PcGamerReader["Name"].ToString());
                PcGamer.setPrix(decimal.Parse(PcGamerReader["Prix"].ToString()));
                PcGamer.setPrixBarre(decimal.Parse(PcGamerReader["PrixBarre"].ToString()));
                PcGamer.setWebLink(PcGamerReader["WebLink"].ToString());
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

        public void UpdateByID()
        {

        }

        public void DeleteByID(int rowID)
        {
            string cmd = "DELETE FROM PcGamer WHERE IdPcGamer=" + rowID;
            CreateDbFile();
            CreateDbConnection();
            ExecuteQuery(cmd);
            CloseDbConnection();
        }

    }
}
