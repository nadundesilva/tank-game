using UnityEngine;
using UnityEngine.UI;

using Assets.Game;

public class PointCoinCountScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        int playerNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));

        GameObject player = GameObject.Find(gameObject.name + "/PlayerNumber");
        int playerNumberValue = GameManager.Instance.GameEngine.Tanks[playerNumber - 1].PlayerNumber + 1;
        player.GetComponent<Text>().text = "0" + playerNumberValue.ToString();

        GameObject point = GameObject.Find(gameObject.name + "/Point");
        int pointsValue = GameManager.Instance.GameEngine.Tanks[playerNumber - 1].Points;
        point.GetComponent<Text>().text = pointsValue.ToString();

        GameObject coin = GameObject.Find(gameObject.name + "/Coin");
        int coinsValue = GameManager.Instance.GameEngine.Tanks[playerNumber - 1].Coins;
        coin.GetComponent<Text>().text = coinsValue.ToString();
    }
}
