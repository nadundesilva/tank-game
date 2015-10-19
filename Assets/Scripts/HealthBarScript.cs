using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Game;

public class HealthBarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        int playerNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        int value = GameManager.Instance.GameEngine.Tanks[playerNumber - 1].Health;

	    GameObject healthValue = GameObject.Find(gameObject.name + "/HealthValue");
        healthValue.GetComponent<Text>().text = value.ToString();
        GameObject healthSlider = GameObject.Find(gameObject.name + "/HealthSlider");
        healthSlider.GetComponent<Slider>().value = Mathf.Clamp(value, 0, 100);
	}
}
