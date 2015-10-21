using UnityEngine;
using System.Collections;
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
        cameraDistanceMax = 830;
        cameraDistanceMin = 130;
        cameraDistance = transform.position.y;

        //Setting constraints for panning
        minSpanX = -120;
        maxSpanX = 780;
        minSpanY = -10;
        maxSpanY = 780;

        //Storing the default position
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        //Setting references to gameobjects
        hudCanvas = GameObject.Find("HUDCanvas");
        gameLauncherCanvas = GameObject.Find("GameLauncherCanvas");
        escapeCanvas = GameObject.Find("EscapeCanvas");

        //Deactivating unused canvases
        hudCanvas.SetActive(false);
        escapeCanvas.SetActive(false);
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
        
        #region Checking for pressed buttons for enabling canvases
        if (Input.GetKeyDown(KeyCode.Escape) && !gameLauncherCanvas.activeSelf)
        {
            hudCanvas.SetActive(!hudCanvas.activeSelf);
            escapeCanvas.SetActive(!escapeCanvas.activeSelf);
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

            if (GUI.Button(new Rect(Screen.width / 2 - 100, 500, 200, 100), new GUIContent(playButtonImage, "Click to play the game"))
                && regexIP.Match(ip).Success && regexPort.Match(port).Success)
            {
                hudCanvas.SetActive(true);
                GameManager.Instance.StartGame();
                GameManager.Instance.JoinServer(ip, int.Parse(port));
                gameLauncherCanvas.SetActive(false);
            }
        }

        //Drawing the button for restoring the default position of the camera if HUDCanvas is active
        if (hudCanvas.activeSelf)
        {
            if (GUI.Button(new Rect(10, 425, 50, 50), new GUIContent(defaultCameraPositionButtonImage, "Click to restore default position")))
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
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 300, 200, 100), new GUIContent(resumeButtonImage, "Click to resume the game")))
            {
                hudCanvas.SetActive(true);
                escapeCanvas.SetActive(false);
            }
            //Exit button
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 450, 200, 100), new GUIContent(exitButtonImage, "Click to resume the game")))
            {
                Application.Quit();
            }
        }

        GUI.backgroundColor = c;
    }
}