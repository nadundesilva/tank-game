using UnityEngine;
using Assets.Game;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MainCameraScript : MonoBehaviour {

    //Speed for panning. rotating and zooming
    private float speed = 20;

    //For panning to store the position where the mouse was clicked down
    private Vector3 dragOrigin;
    private Vector3 origin;

    //To store the Y value of location for zooming
    float cameraDistance;

    //Constraints in panning
    private float minSpanX;
    private float maxSpanX;
    private float minSpanY;
    private float maxSpanY;

    //Constraints for zooming
    private float cameraDistanceMax;
    private float cameraDistanceMin;

    //For storing the default position
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    //For button position
    private float centeredButtonPositionX;
    private float playButtonPositionY;
    private float defaultCameraLocationPositionY;

    //Public variables
    public Texture2D defaultCameraPositionButtonImage;
    public Texture2D playButtonImage;
    public Texture2D resumeButtonImage;
    public Texture2D exitButtonImage;

    //References to canvases
    private GameObject hudCanvas;
    private GameObject gameLauncherCanvas;
    private GameObject escapeCanvas;

    //Use this for initialization
    void Start ()
    {
        //Setting constraints and initial location for zooming
        cameraDistanceMax = 1055;
        cameraDistanceMin = 70;
        cameraDistance = transform.position.y;

        //Setting constraints for panning
        minSpanX = transform.position.x - (Screen.width / 2);
        maxSpanX = transform.position.x + (Screen.width / 2);
        minSpanY = transform.position.z - (Screen.height / 2);
        maxSpanY = transform.position.z + (Screen.height / 2);

        //Storing the default position
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        //Setting button positions
        centeredButtonPositionX = Screen.width / 2 - 100;
        playButtonPositionY = (Screen.height * 73) / 100;
        defaultCameraLocationPositionY = (Screen.height * 11) / 20;

        //Setting references to gameobjects
        hudCanvas = GameObject.Find("HUDCanvas");
        gameLauncherCanvas = GameObject.Find("GameLauncherCanvas");
        escapeCanvas = GameObject.Find("EscapeCanvas");

        //Deactivating unused canvases
        hudCanvas.SetActive(false);
        escapeCanvas.SetActive(false);

        //Setting default port and ip on gameLauncher canvas
        GameObject.Find("GameLauncherCanvas/ServerIPInputField").GetComponent<InputField>().text = GameManager.Instance.ServerIP;
        GameObject.Find("GameLauncherCanvas/ServerPortInputField").GetComponent<InputField>().text = GameManager.Instance.ServerPort.ToString();
        GameObject.Find("GameLauncherCanvas/AutoModeToggle").GetComponent<Toggle>().isOn = (GameManager.Instance.Mode == GameMode.AUTO);
    }

    // Update is called once per frame
    void Update()
    {
        #region Moving the main camera using mouse
        if (Input.GetMouseButtonDown(2))    //Storing the origin of the click for panning
        {
            dragOrigin = Input.mousePosition;
            origin = transform.position;
            return;
        }

        if (Input.GetMouseButton(2)) //When middle mouse button is clicked
        {
            //Panning
            Vector3 pos = dragOrigin - Input.mousePosition;
            transform.position = new Vector3(Mathf.Clamp(origin.x + pos.x, minSpanX, maxSpanX), 0, Mathf.Clamp(origin.z + pos.y, minSpanY, maxSpanY));
        }
        else if (Input.GetMouseButton(1))   //When right mouse button is clicked
        {
            //Rotating
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), (-1) * Input.GetAxis("Mouse X"), 0) * Time.deltaTime, 1);
        }
        
        //Zooming
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * speed * 50;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
        Vector3 zoomVector = new Vector3(transform.position.x, cameraDistance, transform.position.z);
        transform.position = zoomVector;
        #endregion

        //For updating the player number and server message
        if (hudCanvas.activeSelf)
        {
            //setting the current player number
            GameObject.Find("HUDCanvas/HUDTop/CurrentPlayer").GetComponent<Text>().text = "Player Number : 0" + (GameManager.Instance.GameEngine.PlayerNumber + 1);

            //setting specific server messages
            string message = "";
            ServerMessage gameManagerMessage = GameManager.Instance.Message;
            if (gameManagerMessage == ServerMessage.ALREADY_ADDED)
                message = "You have been already added to the server";
            else if (gameManagerMessage == ServerMessage.CELL_OCCUPIED)
                message = "The cell had been already occupied";
            else if (gameManagerMessage == ServerMessage.DEAD)
                message = "You are dead";
            else if (gameManagerMessage == ServerMessage.GAME_ALREADY_STARTED)
                message = "Game had already started";
            else if (gameManagerMessage == ServerMessage.GAME_FINISHED)
                message = "Game had finished";
            else if (gameManagerMessage == ServerMessage.GAME_HAS_FINISHED)
                message = "Game had finished";
            else if (gameManagerMessage == ServerMessage.GAME_NOT_STARTED_YET)
                message = "Game had not yet started";
            else if (gameManagerMessage == ServerMessage.INVALID_CELL)
                message = "Invalid Cell";
            else if (gameManagerMessage == ServerMessage.NOT_A_VALID_CONTESTANT)
                message = "You are not a valid contestant";
            else if (gameManagerMessage == ServerMessage.OBSTACLE)
                message = "Obstacle in your way";
            else if (gameManagerMessage == ServerMessage.PLAYERS_FULL)
                message = "No vacancies for you to join";
            else if (gameManagerMessage == ServerMessage.TOO_QUICK)
                message = "You have to wait for one second before moving again";
            else if (gameManagerMessage == ServerMessage.PITFALL)
                message = "You fell into a pit of water and died";
            GameObject.Find("HUDCanvas/HUDTop/ServerMessage").GetComponent<Text>().text = message;
        }

        #region Checking for pressed buttons
        //For opening the escape canvas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameLauncherCanvas.activeSelf) {
                Application.Quit();
            } else {
                hudCanvas.SetActive(!hudCanvas.activeSelf);
                escapeCanvas.SetActive(!escapeCanvas.activeSelf);
            }
        }

        //For moving the tank if the game in in manual mode
        if (GameManager.Instance.Mode == GameMode.MANUAL)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                GameManager.Instance.CurrentTank.MoveDown();
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                GameManager.Instance.CurrentTank.MoveRight();
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                GameManager.Instance.CurrentTank.MoveUp();
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                GameManager.Instance.CurrentTank.MoveLeft();
            else if (Input.GetKeyDown(KeyCode.Space))
                GameManager.Instance.CurrentTank.Shoot();
        }
        #endregion
    }

    //For drawing GUI components
    void OnGUI()
    {
        Color c = GUI.backgroundColor;
        GUI.backgroundColor = Color.clear;

        //Drawing the button for starting the game if the GameLauncherCanvas is active
        if (gameLauncherCanvas.activeSelf)
        {
            Regex regexIP = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            Regex regexPort = new Regex(@"^[0-9]+$");

            string ip = GameObject.Find("GameLauncherCanvas/ServerIPInputField").GetComponent<InputField>().text;
            string port = GameObject.Find("GameLauncherCanvas/ServerPortInputField").GetComponent<InputField>().text;

            if (GUI.Button(new Rect(centeredButtonPositionX, playButtonPositionY, 150, 75), new GUIContent(playButtonImage, "Click to play the game"))
                && regexIP.Match(ip).Success && regexPort.Match(port).Success)
            {
                GameManager.Instance.JoinServer(ip, int.Parse(port));
                if (GameManager.Instance.Error == GameError.NO_ERROR)
                {
                    if (GameObject.Find("GameLauncherCanvas/AutoModeToggle").GetComponent<Toggle>().isOn)
                    {
                        GameManager.Instance.Mode = GameMode.AUTO;
                    } else
                    {
                        GameManager.Instance.Mode = GameMode.MANUAL;
                    }
                    hudCanvas.SetActive(true);
                    gameLauncherCanvas.SetActive(false);
                }
            }
        }

        //Drawing the button for restoring the default position of the camera if HUDCanvas is active
        if (hudCanvas.activeSelf)
        {
            if (GUI.Button(new Rect(10, defaultCameraLocationPositionY, 50, 50), new GUIContent(defaultCameraPositionButtonImage, "Click to restore default position")))
            {
                transform.position = defaultPosition;
                transform.rotation = defaultRotation;
                transform.position = new Vector3(transform.position.x, cameraDistanceMax, transform.position.z);
            }

        }

        //Drawing the buttons on the escape canvas
        if (escapeCanvas.activeSelf)
        {
            //Resume button
            if (GUI.Button(new Rect(centeredButtonPositionX, 300, 200, 100), new GUIContent(resumeButtonImage, "Click to resume the game")))
            {
                hudCanvas.SetActive(true);
                escapeCanvas.SetActive(false);
            }
            //Exit button
            if (GUI.Button(new Rect(centeredButtonPositionX, 450, 200, 100), new GUIContent(exitButtonImage, "Click to resume the game")))
            {
                Application.Quit();
            }
        }

        GUI.backgroundColor = c;
    }
}