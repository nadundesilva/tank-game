using UnityEngine;

using System.Collections.Generic;

using Assets.Game.GameEntities;
using Assets.Game;

public class TanksGroupScript : MonoBehaviour {
    private List<UnityEngine.GameObject> tankGameObjects;
    private List<UnityEngine.GameObject> healthGameObjects;
    private List<UnityEngine.GameObject> pointsGameObjects;

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
        while (i < tanks.Count)
        {
            if (tanks[i].Health > 0)
                tankGameObjects[i].SetActive(true);
            else
                tankGameObjects[i].SetActive(false);
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

        // For identifying key presses for moving the tank if the game in in manual mode
        if (GameManager.Instance.Mode == GameMode.MANUAL && GameManager.Instance.State == GameState.PROGRESSING)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                GameManager.Instance.CurrentTank.MoveUp();
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                GameManager.Instance.CurrentTank.MoveRight();
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                GameManager.Instance.CurrentTank.MoveDown();
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                GameManager.Instance.CurrentTank.MoveLeft();
            else if (Input.GetKeyDown(KeyCode.Space))
                GameManager.Instance.CurrentTank.Shoot();
        }
    }
}
