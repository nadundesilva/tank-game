using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;

public class StoneWallsGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;

    float transformY;

    Quaternion defaultRotation;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("StoneWallsGroup/StoneWall");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        List<StoneWall> stoneWalls = GameManager.Instance.GameEngine.StoneWalls;

        int i = 0;
        while (i < stoneWalls.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(stoneWalls[i].PositionX * 80, transformY, stoneWalls[i].PositionY * 80);
            i++;
        }
        while (i < stoneWalls.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(stoneWalls[i].PositionX * 80, transformY, stoneWalls[i].PositionY * 80), defaultRotation);
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
