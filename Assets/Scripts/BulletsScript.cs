using UnityEngine;
using System.Collections;
using Assets.Game.GameEntities;
using Assets.Game;

public class BulletsScript : MonoBehaviour {
    Vector3 originPosition;
    float tPosition;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        int bulletNumber = int.Parse(gameObject.name.Substring(6));
        Bullet bullet = GameManager.Instance.GameEngine.Bullets[bulletNumber - 1];
    }
}
