namespace Snake
{
    /// <summary>
    /// Static accessors for the game's persistent state.
    /// Decouples game logic classes from the Form class.
    /// </summary>
    public static class SnakeClass
    {
        public static int PlayerLevel { get; set; } = 1;
        public static int LevelXP { get; set; } = 0;
        public static int SelectedPalette { get; set; } = 1;
        public static string CurrentUser { get; set; } = string.Empty;
        public static bool IsLoggedIn { get; set; } = false;

        public static int GetPlayerLevel() => PlayerLevel;
        public static int GetLevelXP() => LevelXP;
        public static int GetSelectedPalette() => SelectedPalette;
        public static string GetCurrentUser() => CurrentUser;
        public static bool IsLoggedInUser() => IsLoggedIn;

        public static void SetLoggedIn(string user)
        {
            IsLoggedIn = true;
            CurrentUser = user;
        }

        public static void SetLoggedOut()
        {
            IsLoggedIn = false;
            CurrentUser = string.Empty;
        }
    }
}
