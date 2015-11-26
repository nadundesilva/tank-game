using UnityEngine;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;

public class BulletsGroupScript : MonoBehaviour {
    private List<UnityEngine.GameObject> gameObjects;
    private List<float> tPositions;
    private List<Vector3> originPositions;

    private Quaternion defaultRotation;
    private float transformY;

    private float coordinateMultiplierX;
    private float coordinateMultiplierY;
    private float deltaTime;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();
        tPositions = new List<float>();
        originPositions = new List<Vector3>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("BulletsGroup/Bullet");
        gameObjects.Add(go);
        tPositions.Add(0);
        originPositions.Add(go.transform.position);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;

        // Setting animation parameters
        Constants constants = Constants.Instance;
        coordinateMultiplierX = constants.GridSquareScale * 10 / constants.MapSize;
        coordinateMultiplierY = (-1) * constants.GridSquareScale * 10 / constants.MapSize;
        deltaTime = constants.DeltaTime;

        // resizing bullet to fit the map
        float scale = go.transform.localScale.x * 10 / constants.MapSize;
        go.transform.localScale = new Vector3(scale, scale, scale);
        Light halo = go.GetComponent<Light>();
        halo.range = Mathf.Ceil(halo.range * 10.0f / constants.MapSize);
    }
	
	// Update is called once per frame
	void Update () {
        List<Bullet> bullets = GameManager.Instance.GameEngine.Bullets;

        int i = 0;
        while (i < bullets.Count && i < gameObjects.Count)
        {
            if (!gameObjects[i].activeSelf)
            {
                gameObjects[i].SetActive(true);

                if (bullets[i].Direction == Direction.NORTH)
                    originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
                else if (bullets[i].Direction == Direction.EAST)
                    originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
                else if (bullets[i].Direction == Direction.SOUTH)
                    originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
                else
                    originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);

                gameObjects[i].transform.position = originPositions[i];
                tPositions[i] = 0;
            }
            else
            {

                if (bullets[i].PositionX * coordinateMultiplierX == gameObjects[i].transform.position.x &&
                    bullets[i].PositionY * coordinateMultiplierY == gameObjects[i].transform.position.z)
                {
                    originPositions[i] = gameObjects[i].transform.position;
                    tPositions[i] = 0;
                }

                if (bullets[i].Direction == Direction.NORTH)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY), tPositions[i]);
                else if (bullets[i].Direction == Direction.EAST)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY), tPositions[i]);
                else if (bullets[i].Direction == Direction.SOUTH)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY), tPositions[i]);
                else
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY), tPositions[i]);
                tPositions[i] += deltaTime;
            }
            i++;
        }
        while (i < bullets.Count)
        {
            originPositions.Add(transform.position);

            if (bullets[i].Direction == Direction.NORTH)
                originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
            else if (bullets[i].Direction == Direction.EAST)
                originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
            else if (bullets[i].Direction == Direction.SOUTH)
                originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);
            else
                originPositions[i] = new Vector3(bullets[i].PositionX * coordinateMultiplierX, transformY, bullets[i].PositionY * coordinateMultiplierY);

            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], originPositions[i], defaultRotation);
            gameObjects.Add(go);
            tPositions.Add(0);
            i++;
        }
        while (i < gameObjects.Count)
        {
            gameObjects[i].SetActive(false);
            i++;
        }
    }
}
