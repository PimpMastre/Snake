using System;

namespace Snake
{
    public class XPSystem
    {
        private static readonly int[] RequiredXPTable = {
            100,   // level 1
            150,   // level 2
            200,   // level 3
            300,   // level 4
            350,   // level 5
            350,   // level 6
            370,   // level 7
            390,   // level 8
            400,   // level 9
            410,   // level 10
            420,   // level 11
            450,   // level 12
            460,   // level 13
            470,   // level 14
            490,   // level 15
            500,   // level 16
            500,   // level 17
            500,   // level 18
            510,   // level 19
            520,   // level 20
            550,   // level 21
            600,   // level 22
            9001   // level 23+ (capped)
        };

        public int GetRequiredXP(int userLevel)
        {
            if (userLevel < 1 || userLevel > RequiredXPTable.Length)
                return 9001;
            return RequiredXPTable[userLevel - 1];
        }
    }
}
