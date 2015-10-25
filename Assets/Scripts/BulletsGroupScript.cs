using UnityEngine;
using System.Collections.Generic;
using Assets.Game.GameEntities;
using Assets.Game;

public class BulletsGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> gameObjects;

    Quaternion defaultRotation;

    float transformY;

    Vector3 originPosition;
    float tPosition;

    // Use this for initialization
    void Start () {
        gameObjects = new List<UnityEngine.GameObject>();

        UnityEngine.GameObject go = UnityEngine.GameObject.Find("BulletsGroup/Bullet1");
        gameObjects.Add(go);

        defaultRotation = go.transform.rotation;
        transformY = go.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        List<Bullet> bullets = GameManager.Instance.GameEngine.Bullets;

        int i = 0;
        while (i < bullets.Count && i < gameObjects.Count)
        {
            gameObjects[i].SetActive(true);
            gameObjects[i].transform.position = new Vector3(bullets[i].PositionX * 80 + 20, transformY, bullets[i].PositionY * 80 + 20);
            i++;
        }
        while (i < bullets.Count)
        {
            UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObjects[0], new Vector3(bullets[i].PositionX * 80 + 20, transformY, bullets[i].PositionY * 80 + 20), defaultRotation);
            go.name = "Bullet" + i;
            gameObjects.Add(go);
            i++;
        }
        while (i < gameObjects.Count)
        {
            gameObjects[i].SetActive(false);
            i++;
        }
    }

    void animateMove(int destinationX, int destinationZ)
    {
        if (destinationX * 80 + 20 == transform.position.x &&
            destinationZ * 80 + 20 == transform.position.z)
        {
            originPosition = transform.position;
            tPosition = 0;
        }

        transform.position = Vector3.Lerp(originPosition, new Vector3(destinationX * 80 + 20, transform.position.y, destinationZ * 80 + 20), tPosition);

        tPosition += (1 / 30.0f);
    }
}
