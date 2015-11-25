using System.Configuration;
using System.Collections.Specialized;

namespace Assets.Game
{
    // For storing all the constants for the game
    public class Constants
    {
        // Game Parameters
        public static int MapSize = 10;
        public static int GameTimeMinutes = 15;
        public static int GameTimeSeconds = 0;
        public static int BulletSpeed = 4;
        public static int TankSpeed = 1;

        // AI Parameters
        public static int AITreeDepth = 10;

        /*
         * Animation Parameters
         * Specific for the Unity GUI designed for this game
         * Should  be replaced if another GUI is plugged in
        */
        public static float GridSquareScale = 80;
        public static float DeltaTime = 0.1f;
        /*
        private Constants()
        {
            MapSize = 10;
            GameTimeMinutes = 15;
            GameTimeSeconds = 0;
            BulletSpeed = 4;
            TankSpeed = 1;

            AITreeDepth = 10;

            GridSquareScale = 80;
            DeltaTime = 0.1f;
        }*/
    }
}
