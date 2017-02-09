using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;
using System.Data.OleDb;

namespace Snake
{
    class DatabaseHandler
    {
        OleDbConnection conn = new OleDbConnection();

        public void DBInitialisation()
        {
            connect();
        }

        private void connect()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = UserDatabase.accdb;Jet OLEDB:Database Password=youshallnotpass";
            conn = new OleDbConnection(connectionString);
            conn.Open();
        }

        public void saveHighScore(string user, int DIFF, int score, int level)
        {
            string high = "";

            switch(DIFF)
            {
                case 1:
                    high = "HSEasy";
                    break;
                case 2:
                    high = "HSMedium";
                    break;
                case 3:
                    high = "HSHard";
                    break;
                case 4:
                    high = "HSExtreme";
                    break;
            }

            high += "Level" + level;

            OleDbCommand command = new OleDbCommand("SELECT [Username], [" + high + "] FROM [Users] WHERE [Username] = '" + user + "'", conn);
            OleDbDataReader dataReader = command.ExecuteReader();
            dataReader.Read();

            if (score > Convert.ToInt32(dataReader[1]))
            {
                OleDbCommand command2 = new OleDbCommand("UPDATE [Users] SET [" + high + "] = '" + score + "' WHERE [Username] = '" + user + "'", conn);
                command2.ExecuteNonQuery();
            }
        }

        public void saveProgress(string user)
        {
            OleDbCommand command = new OleDbCommand("UPDATE [Users] SET [Level] = '" + Snake.level + "', [LevelXP] = '" + Snake.levelXP + "', [UsedPalette] = '" + Snake.selectedPalette + "' WHERE [Username] = '" + user + "'", conn);
            command.ExecuteNonQuery();
        }

        public void resetProgress(string user)
        {
            OleDbCommand command = new OleDbCommand("UPDATE [Users] SET [Level] = '1', [LevelXP] = '0', [UsedPalette] = '1', [HSEasyLevel1] = '0', [HSEasyLevel2] = '0', [HSEasyLevel3] = '0', [HSEasyLevel4] = '0', [HSEasyLevel5] = '0', [HSEasyLevel6] = '0', [HSEasyLevel7] = '0', [HSMediumLevel1] = '0', [HSMediumLevel2] = '0', [HSMediumLevel3] = '0', [HSMediumLevel4] = '0', [HSMediumLevel5] = '0', [HSMediumLevel6] = '0', [HSMediumLevel7] = '0', [HSHardLevel1] = '0', [HSHardLevel2] = '0', [HSHardLevel3] = '0', [HSHardLevel4] = '0', [HSHardLevel5] = '0', [HSHardLevel6] = '0', [HSHardLevel7] = '0', [HSExtremeLevel1] = '0', [HSExtremeLevel2] = '0', [HSExtremeLevel3] = '0', [HSExtremeLevel4] = '0', [HSExtremeLevel5] = '0', [HSExtremeLevel6] = '0', [HSExtremeLevel7] = '0' WHERE [Username] = '" + user + "'", conn);
            command.ExecuteNonQuery();
        }

        public int deleteAccount(string user, string pass)
        {
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Users] WHERE [Username] = '" + user + "'", conn);
            OleDbDataReader dataReader = command.ExecuteReader();
            dataReader.Read();

            if (pass != dataReader[2].ToString()) // nu coincid parolele
                return 1;
            else
            {
                OleDbCommand command2 = new OleDbCommand("DELETE FROM [Users] WHERE [Username] = '" + user + "'", conn);
                command2.ExecuteNonQuery();
                conn.Close();
                return 2;
            }
        }

        public void addNewAccount(string user, string pass)
        {
            OleDbCommand command = new OleDbCommand("INSERT INTO [Users] ([Username], [Password], [Level], [LevelXP], [UsedPalette], [HSEasyLevel1], [HSEasyLevel2], [HSEasyLevel3], [HSEasyLevel4], [HSEasyLevel5], [HSEasyLevel6], [HSEasyLevel7], [HSMediumLevel1], [HSMediumLevel2], [HSMediumLevel3], [HSMediumLevel4], [HSMediumLevel5], [HSMediumLevel6], [HSMediumLevel7], [HSHardLevel1], [HSHardLevel2], [HSHardLevel3], [HSHardLevel4], [HSHardLevel5], [HSHardLevel6], [HSHardLevel7], [HSExtremeLevel1], [HSExtremeLevel2], [HSExtremeLevel3], [HSExtremeLevel4], [HSExtremeLevel5], [HSExtremeLevel6], [HSExtremeLevel7]) VALUES ('" + user + "', '" + pass + "', '1', '0', '1', '0', '0' ,'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0')", conn);
            command.ExecuteNonQuery();
        }

        public void readHighScores(int category, int level)
        {
            string search = "";
            
            switch(category)
            {
                case 1:
                    search = "HSEasy";
                    break;
                case 2:
                    search = "HSMedium";
                    break;
                case 3:
                    search = "HSHard";
                    break;
                case 4:
                    search = "HSExtreme";
                    break;
            }

            search += "Level" + level.ToString();

            OleDbCommand command = new OleDbCommand("SELECT [Username], [" + search + "] FROM [Users] ORDER BY [" + search + "] DESC, [Username]", conn);

            OleDbDataReader dataReader = command.ExecuteReader();
            for(int i = 1; i <= 10; ++i)
            {
                dataReader.Read();
                Snake.hsTable[i].name = Convert.ToString(dataReader[0]);
                Snake.hsTable[i].score = Convert.ToInt32(dataReader[1]);
            }
        }

        public int checkLoginCredentials(string user, string pass)
        {
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Users] WHERE [Username] = '" + user + "'", conn);
            if (command.ExecuteScalar() == null) // daca nu exista un user cu acel username
                return 1;
            else
            {
                OleDbDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                if (pass != dataReader[2].ToString()) // nu coincid parolele
                    return 2;
                else
                {
                    Snake.level = Convert.ToInt32(dataReader[3]);
                    Snake.levelXP = Convert.ToInt32(dataReader[4]);
                    Snake.userPalette = Convert.ToInt32(dataReader[5]);
                    return 3;
                }
            }     
        }
    }
}
