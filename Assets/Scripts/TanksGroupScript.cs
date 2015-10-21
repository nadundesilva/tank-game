using UnityEngine;
using System.Collections;
using Assets.Game.GameEntities;
using System.Collections.Generic;
using Assets.Game;

public class TanksGroupScript : MonoBehaviour {
    List<UnityEngine.GameObject> tankGameObjects;
    List<UnityEngine.GameObject> healthGameObjects;
    List<UnityEngine.GameObject> pointsGameObjects;

	// Use this for initialization
	void Start () {
        tankGameObjects = new List<UnityEngine.GameObject>();
        healthGameObjects = new List<UnityEngine.GameObject>();
        pointsGameObjects = new List<UnityEngine.GameObject>();

        int i = 1;
        while (i <= 5)
        {
            tankGameObjects.Add(UnityEngine.GameObject.Find("TanksGroup/Tank" + i));
            healthGameObjects.Add(UnityEngine.GameObject.Find("HUDCanvas/HealthUIGroup/HealthUIPlayer" + i));
            pointsGameObjects.Add(UnityEngine.GameObject.Find("HUDCanvas/PointCoinCountUIGroup/PointCoinCountPlayer" + i));
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
        List<Tank> tanks = GameManager.Instance.GameEngine.Tanks;

        int i = 0;
        while (i < tanks.Count - 1)
        {
            tankGameObjects[i].SetActive(true);
            healthGameObjects[i].SetActive(true);
            pointsGameObjects[i].SetActive(true);
            i++;
        }
        while (i < 5)
        {
            tankGameObjects[i].SetActive(false);
            healthGameObjects[i].SetActive(false);
            pointsGameObjects[i].SetActive(false);
            i++;
        }
	}
}
