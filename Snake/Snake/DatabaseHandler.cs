using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace Snake
{
    public class DatabaseHandler
    {
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = UserDatabase.accdb;Jet OLEDB:Database Password=youshallnotpass";
        private OleDbConnection connection;

        private readonly Dictionary<int, string> difficultyToTableMap = new Dictionary<int, string>
        {
            { 1, "HSEasy" },
            { 2, "HSMedium" },
            { 3, "HSHard" },
            { 4, "HSExtreme" }
        };

        private readonly string[] allScoreColumns = {
            "HSEasyLevel1", "HSEasyLevel2", "HSEasyLevel3", "HSEasyLevel4", "HSEasyLevel5", "HSEasyLevel6", "HSEasyLevel7",
            "HSMediumLevel1", "HSMediumLevel2", "HSMediumLevel3", "HSMediumLevel4", "HSMediumLevel5", "HSMediumLevel6", "HSMediumLevel7",
            "HSHardLevel1", "HSHardLevel2", "HSHardLevel3", "HSHardLevel4", "HSHardLevel5", "HSHardLevel6", "HSHardLevel7",
            "HSExtremeLevel1", "HSExtremeLevel2", "HSExtremeLevel3", "HSExtremeLevel4", "HSExtremeLevel5", "HSExtremeLevel6", "HSExtremeLevel7"
        };

        public void Initialise()
        {
            Connect();
        }

        private void Connect()
        {
            connection?.Close();
            connection = new OleDbConnection(connectionString);
            connection.Open();
        }

        private void EnsureOpen()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
                Connect();
        }

        public void SaveHighScore(string user, int difficulty, int score, int level)
        {
            EnsureOpen();
            if (!difficultyToTableMap.TryGetValue(difficulty, out var tableName))
                return;

            string columnName = tableName + "Level" + level;

            // Read existing score
            int existingScore = 0;
            string selectQuery = $"SELECT [{columnName}] FROM [Users] WHERE [Username] = @user";
            using (var command = new OleDbCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@user", user);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        existingScore = Convert.ToInt32(reader[0]);
                }
            }

            if (score > existingScore)
            {
                string updateQuery = $"UPDATE [Users] SET [{columnName}] = @score WHERE [Username] = @user";
                using (var command = new OleDbCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@score", score);
                    command.Parameters.AddWithValue("@user", user);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SaveProgress(string user)
        {
            EnsureOpen();
            using (var command = new OleDbCommand(
                "UPDATE [Users] SET [Level] = @level, [LevelXP] = @levelXP, [UsedPalette] = @palette WHERE [Username] = @user", connection))
            {
                command.Parameters.AddWithValue("@level", SnakeClass.GetPlayerLevel());
                command.Parameters.AddWithValue("@levelXP", SnakeClass.GetLevelXP());
                command.Parameters.AddWithValue("@palette", SnakeClass.GetSelectedPalette());
                command.Parameters.AddWithValue("@user", user);
                command.ExecuteNonQuery();
            }
        }

        public void ResetProgress(string user)
        {
            EnsureOpen();
            var setClause = "Level = @level, LevelXP = @levelXP, UsedPalette = @palette";
            foreach (var col in allScoreColumns)
            {
                setClause += $", [{col}] = 0";
            }

            using (var command = new OleDbCommand(
                $"UPDATE [Users] SET {setClause} WHERE [Username] = @user", connection))
            {
                command.Parameters.AddWithValue("@level", 1);
                command.Parameters.AddWithValue("@levelXP", 0);
                command.Parameters.AddWithValue("@palette", 1);
                command.Parameters.AddWithValue("@user", user);
                command.ExecuteNonQuery();
            }
        }

        public int DeleteAccount(string user, string password)
        {
            EnsureOpen();
            using (var command = new OleDbCommand("SELECT [Password] FROM [Users] WHERE [Username] = @user", connection))
            {
                command.Parameters.AddWithValue("@user", user);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return 1; // user not found

                    if (password != Convert.ToString(reader[0]))
                        return 1; // wrong password

                    using (var deleteCmd = new OleDbCommand("DELETE FROM [Users] WHERE [Username] = @user", connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@user", user);
                        deleteCmd.ExecuteNonQuery();
                    }
                }
            }
            return 2; // success
        }

        public void AddNewAccount(string user, string password)
        {
            EnsureOpen();
            // OleDb uses positional parameters — all 34 columns must use ? placeholders
            var values = new object[]
            {
                user, password, 1, 0, 1,  // Username, Password, Level, LevelXP, UsedPalette
                0, 0, 0, 0, 0, 0, 0,    // HSEasy L1-L7
                0, 0, 0, 0, 0, 0, 0,    // HSMedium L1-L7
                0, 0, 0, 0, 0, 0, 0,    // HSHard L1-L7
                0, 0, 0, 0, 0, 0, 0     // HSExtreme L1-L7
            };

            var placeholders = string.Join(", ", values.Select(_ => "?"));
            string query = "INSERT INTO [Users] ([Username], [Password], [Level], [LevelXP], [UsedPalette], " +
                           "[HSEasyLevel1], [HSEasyLevel2], [HSEasyLevel3], [HSEasyLevel4], [HSEasyLevel5], " +
                           "[HSEasyLevel6], [HSEasyLevel7], [HSMediumLevel1], [HSMediumLevel2], [HSMediumLevel3], " +
                           "[HSMediumLevel4], [HSMediumLevel5], [HSMediumLevel6], [HSMediumLevel7], [HSHardLevel1], " +
                           "[HSHardLevel2], [HSHardLevel3], [HSHardLevel4], [HSHardLevel5], [HSHardLevel6], " +
                           "[HSHardLevel7], [HSExtremeLevel1], [HSExtremeLevel2], [HSExtremeLevel3], " +
                           "[HSExtremeLevel4], [HSExtremeLevel5], [HSExtremeLevel6], [HSExtremeLevel7]) " +
                           $"VALUES ({placeholders})";

            using (var command = new OleDbCommand(query, connection))
            {
                foreach (var v in values)
                    command.Parameters.AddWithValue(null, v);
                command.ExecuteNonQuery();
            }
        }

        public void ReadHighScores(int difficulty, int level, HighScore[] scores)
        {
            EnsureOpen();
            if (!difficultyToTableMap.TryGetValue(difficulty, out var tableName))
                return;

            string columnName = tableName + "Level" + level;
            string query = $"SELECT [Username], [{columnName}] FROM [Users] ORDER BY [{columnName}] DESC, [Username]";

            using (var command = new OleDbCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                for (int i = 1; i <= 10 && reader.Read(); i++)
                {
                    scores[i].Name = Convert.ToString(reader[0]);
                    scores[i].Score = Convert.ToInt32(reader[1]);
                }
            }
        }

        public int CheckLoginCredentials(string user, string password, out int playerLevel, out int levelXP, out int userPalette)
        {
            playerLevel = 0;
            levelXP = 0;
            userPalette = 0;

            EnsureOpen();

            using (var command = new OleDbCommand(
                "SELECT [Password], [Level], [LevelXP], [UsedPalette] FROM [Users] WHERE [Username] = @user", connection))
            {
                command.Parameters.AddWithValue("@user", user);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return 1; // user not found

                    var dbPassword = Convert.ToString(reader[0]);
                    if (password != dbPassword)
                        return 2; // wrong password

                    playerLevel = Convert.ToInt32(reader[1]);
                    levelXP = Convert.ToInt32(reader[2]);
                    userPalette = Convert.ToInt32(reader[3]);
                    return 3; // success
                }
            }
        }
    }
}
