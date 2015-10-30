using UnityEngine;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;
using UnityEngine.UI;

public class CoinPilesGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;

    Quaternion defaultRotation;

    float transformY;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("CoinPilesGroup/CoinPile");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        List<CoinPile> coinPiles = GameManager.Instance.GameEngine.CoinPiles;

        int i = 0;
        while (i < coinPiles.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(coinPiles[i].PositionX * 80 + 20, transformY, coinPiles[i].PositionY * 80 + 20);
             i++;
        }
        while (i < coinPiles.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(coinPiles[i].PositionX * 80 + 20, transformY, coinPiles[i].PositionY * 80 + 20), defaultRotation);
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
