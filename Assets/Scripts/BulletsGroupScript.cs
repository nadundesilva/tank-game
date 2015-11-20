using UnityEngine;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;

public class BulletsGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;
    List<float> tPositions;
    List<Vector3> originPositions;

    Quaternion defaultRotation;

    float transformY;

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
                    originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
                else if (bullets[i].Direction == Direction.EAST)
                    originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
                else if (bullets[i].Direction == Direction.SOUTH)
                    originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
                else
                    originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);

                gameObjects[i].transform.position = originPositions[i];
                tPositions[i] = 0;
            }
            else
            {

                if (bullets[i].PositionX * 80 == gameObjects[i].transform.position.x &&
                    bullets[i].PositionY * 80 == gameObjects[i].transform.position.z)
                {
                    originPositions[i] = gameObjects[i].transform.position;
                    tPositions[i] = 0;
                }

                if (bullets[i].Direction == Direction.NORTH)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80), tPositions[i]);
                else if (bullets[i].Direction == Direction.EAST)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80), tPositions[i]);
                else if (bullets[i].Direction == Direction.SOUTH)
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80), tPositions[i]);
                else
                    gameObjects[i].transform.position = Vector3.Lerp(originPositions[i], new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80), tPositions[i]);
                tPositions[i] += (1 / 30.0f);
            }
            i++;
        }
        while (i < bullets.Count)
        {
            originPositions.Add(transform.position);

            if (bullets[i].Direction == Direction.NORTH)
                originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
            else if (bullets[i].Direction == Direction.EAST)
                originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
            else if (bullets[i].Direction == Direction.SOUTH)
                originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);
            else
                originPositions[i] = new Vector3(bullets[i].PositionX * 80, transformY, bullets[i].PositionY * 80);

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
