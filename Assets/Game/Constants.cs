using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Assets.Game
{
    // For storing all the constants for the game
    public class Constants
    {
        /*
         * Game Parameters
        */
        #region Game Parameters
        private int mapSize;
        public int MapSize
        {
            get
            {
                return mapSize;
            }
        }

        private int gameTimeMinutes;
        public int GameTimeMinutes
        {
            get
            {
                return gameTimeMinutes;
            }
        }

        private int gameTimeSeconds;
        public int GameTimeSeconds
        {
            get
            {
                return gameTimeSeconds;
            }
        }

        private int bulletSpeed;
        public int BulletSpeed
        {
            get
            {
                return bulletSpeed;
            }
        }

        private int tankSpeed;
        public int TankSpeed;
        #endregion

        /*
         * AI Parameters
        */
        private int aITreeDepth;
        public int AITreeDepth
        {
            get
            {
                return aITreeDepth;
            }
        }

        /*
         * Animation Parameters
         * Specific for the Unity GUI designed for this game
         * Should be changed or removed if a different GUI is used
        */
        #region Animation Parameters
        private float gridSquareScale;
        public float GridSquareScale
        {
            get
            {
                return gridSquareScale;
            }
        }

        private float deltaTime;
        public float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }
        #endregion

        #region Singleton
        public static Constants instance;

        public static Constants Instance
        {
            get
            {
                if (instance == null)
                    instance = new Constants();
                return instance;
            }
        }

        private Constants()
        {
            LoadDefaultData();
        }
        #endregion

        public void LoadData(string fileLocation)
        {
            try
            {
                LoadXMLData(fileLocation);
            }
            catch (XmlException e) { }
        }

        /*
         * Loads parameters from the xml file specified
        */
        private void LoadXMLData(string fileLocation)
        {
            XDocument doc = XDocument.Load(fileLocation);
            Dictionary<string, string> dict = doc.Root.DescendantNodes().OfType<XElement>()
                                                 .ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);

            // Loading game parameters from xml strings
            mapSize = int.Parse(dict["MapSize"]);
            gameTimeMinutes = int.Parse(dict["GameTimeMinutes"]);
            gameTimeSeconds = int.Parse(dict["GameTimeSeconds"]);
            bulletSpeed = int.Parse(dict["BulletSpeed"]);
            tankSpeed = int.Parse(dict["TankSpeed"]);

            // Loading AI parameters from xml strings
            aITreeDepth = int.Parse(dict["AITreeDepth"]);
        }

        /*
         * Loads the factory default parameters
        */
        private void LoadDefaultData()
        {
            // Loading default game parameters
            mapSize = 10;
            gameTimeMinutes = 15;
            gameTimeSeconds = 0;
            bulletSpeed = 4;
            tankSpeed = 1;

            // Loading default AI parameters
            aITreeDepth = 10;

            /*
             * Animation parameters
             * Do not change unless the GUI is changed to fit the parameters
             * Should be changed or removed if a different GUI is used
            */
            gridSquareScale = 80;
            deltaTime = 0.1f;
        }
    }
}
