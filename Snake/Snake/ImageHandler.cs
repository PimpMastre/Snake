using System;
using System.Collections.Generic;

namespace Snake
{
    public class ImageHandler
    {
        private static readonly Dictionary<int, string> LockedImages = new Dictionary<int, string>
        {
            { 1, "img/one.png" },
            { 2, "img/twoLocked.png" },
            { 3, "img/threeLocked.png" },
            { 4, "img/fourLocked.png" },
            { 5, "img/fiveLocked.png" },
            { 6, "img/sixLocked.png" },
            { 7, "img/sevenLocked.png" }
        };

        private static readonly Dictionary<int, string> UnlockedImages = new Dictionary<int, string>
        {
            { 1, "img/one.png" },
            { 2, "img/two.png" },
            { 3, "img/three.png" },
            { 4, "img/four.png" },
            { 5, "img/five.png" },
            { 6, "img/six.png" },
            { 7, "img/seven.png" }
        };

        private static readonly int[] LevelUnlockThresholds = {
            0,    // level 1 - no threshold (always available)
            2,    // level 2 unlocks at player level 3
            6,    // level 3 unlocks at player level 7
            10,   // level 4 unlocks at player level 11
            13,   // level 5 unlocks at player level 14
            17,   // level 6 unlocks at player level 18
            20    // level 7 unlocks at player level 21
        };

        public bool IsLocked(int levelNo)
        {
            if (levelNo < 1 || levelNo > 7)
                return true;
            return SnakeClass.GetPlayerLevel() < LevelUnlockThresholds[levelNo - 1];
        }

        public string GetImage(int levelNo)
        {
            if (levelNo < 1 || levelNo > 7)
                return string.Empty;

            if (IsLocked(levelNo))
                return LockedImages[levelNo];
            return UnlockedImages[levelNo];
        }
    }
}
