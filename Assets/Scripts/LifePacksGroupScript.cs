using UnityEngine;
using System.Collections.Generic;
using Assets.Game;
using Assets.Game.GameEntities;

public class LifePacksGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;

    Quaternion defaultRotation;

    float transformY;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("LifePacksGroup/LifePack");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        List<LifePack> lifePacks = GameManager.Instance.GameEngine.LifePacks;

        int i = 0;
        while (i < lifePacks.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(lifePacks[i].PositionX * 2 + 0.5f, transformY, lifePacks[i].PositionY * 2 + 0.5f);
            i++;
        }
        while (i < lifePacks.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(lifePacks[i].PositionX * 80 + 20, transformY, lifePacks[i].PositionY * 80 + 20), defaultRotation);
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
