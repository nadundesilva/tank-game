using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;

public class WaterGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;

    Quaternion defaultRotation;

    float transformY;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("WaterGroup/Water");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        List<Water> water = GameManager.Instance.GameEngine.Water;

        int i = 0;
        while (i < water.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(water[i].PositionX * 80, transformY, water[i].PositionY * 80);
            i++;
        }
        while (i < water.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(water[i].PositionX * 80, transformY, water[i].PositionY * 80), defaultRotation);
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
