using UnityEngine;

using System.Collections.Generic;

using Assets.Game.GameEntities;
using Assets.Game;

public class CoinPilesGroupScript : MonoBehaviour {
    private List<UnityEngine.GameObject> gameObjects;

    private Quaternion defaultRotation;
    private float transformY;

    private float coordinateMultiplierX;
    private float coordinateMultiplierY;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("CoinPilesGroup/CoinPile");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;

        // Setting animation parameters
        coordinateMultiplierX = Constants.GridSquareScale * 10 / Constants.MapSize;
        coordinateMultiplierY = (-1) * Constants.GridSquareScale * 10 / Constants.MapSize;

        // resizing coin pile to fit the map
        float scale = go.transform.localScale.x * 10 / Constants.MapSize;
        go.transform.localScale = new Vector3(scale, scale, scale);
        for (int i = 1; i < 10; i++) {
            Light halo = UnityEngine.GameObject.Find("CoinPilesGroup/CoinPile/Coin" + i).GetComponent<Light>();
            halo.range = Mathf.Ceil(halo.range * 10.0f / Constants.MapSize);
        }
    }
	
	// Update is called once per frame
	void Update () {
        List<CoinPile> coinPiles = GameManager.Instance.GameEngine.CoinPiles;

        int i = 0;
        while (i < coinPiles.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(coinPiles[i].PositionX * coordinateMultiplierX, transformY, coinPiles[i].PositionY * coordinateMultiplierY);
             i++;
        }
        while (i < coinPiles.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(coinPiles[i].PositionX * coordinateMultiplierX, transformY, coinPiles[i].PositionY * coordinateMultiplierY), defaultRotation);
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
