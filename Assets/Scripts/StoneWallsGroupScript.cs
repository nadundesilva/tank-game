using UnityEngine;

using System.Collections.Generic;

using Assets.Game.GameEntities;
using Assets.Game;

public class StoneWallsGroupScript : MonoBehaviour {
    private List<UnityEngine.GameObject> gameObjects;

    private float transformY;
    private Quaternion defaultRotation;

    private int gridSquareSize;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("StoneWallsGroup/StoneWall");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;

        //setting animation parameters
        Constants constants = new Constants();
        gridSquareSize = constants.GridSquareSize;
    }
	
	// Update is called once per frame
	void Update () {
        List<StoneWall> stoneWalls = GameManager.Instance.GameEngine.StoneWalls;

        int i = 0;
        while (i < stoneWalls.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(stoneWalls[i].PositionX * gridSquareSize, transformY, stoneWalls[i].PositionY * gridSquareSize);
            i++;
        }
        while (i < stoneWalls.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(stoneWalls[i].PositionX * gridSquareSize, transformY, stoneWalls[i].PositionY * gridSquareSize), defaultRotation);
            gameObjects.Add(go);
            i++;
        }
        while (i < gameObjects.Count)
        {
            gameObjects[i].SetActive(false);
            i++;
        }
    }
}
