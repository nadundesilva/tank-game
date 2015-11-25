using UnityEngine;

using System.Collections.Generic;

using Assets.Game.GameEntities;
using Assets.Game;

public class StoneWallsGroupScript : MonoBehaviour {
    private List<UnityEngine.GameObject> gameObjects;

    private float transformY;
    private Quaternion defaultRotation;

    private float coordinateMultiplierX;
    private float coordinateMultiplierY;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("StoneWallsGroup/StoneWall");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;

        // Setting animation parameters
        coordinateMultiplierX = Constants.GridSquareScale * 10 / Constants.MapSize;
        coordinateMultiplierY = (-1) * Constants.GridSquareScale * 10 / Constants.MapSize;

        // resizing stone wall to fit the map
        float scale = go.transform.localScale.x * 10 / Constants.MapSize;
        go.transform.localScale = new Vector3(scale, scale, scale);
    }
	
	// Update is called once per frame
	void Update () {
        List<StoneWall> stoneWalls = GameManager.Instance.GameEngine.StoneWalls;

        int i = 0;
        while (i < stoneWalls.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(stoneWalls[i].PositionX * coordinateMultiplierX, transformY, stoneWalls[i].PositionY * coordinateMultiplierY);
            i++;
        }
        while (i < stoneWalls.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(stoneWalls[i].PositionX * coordinateMultiplierX, transformY, stoneWalls[i].PositionY * coordinateMultiplierY), defaultRotation);
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
