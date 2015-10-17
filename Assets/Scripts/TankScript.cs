using UnityEngine;
using System.Collections;
using System;

public class TankScript : MonoBehaviour {

    float deltaMovementX;
    float deltaMovementZ;
    float deltaRotation;

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        int playerNumber = int.Parse(gameObject.name.Substring(4));
        animateMove(playerNumber, playerNumber);
        animateRotation(90);
	}

    void animateMove (int destinationX, int destinationZ)
    {
        if (deltaMovementX == 0 && deltaMovementZ == 0)
        {
            deltaMovementX = (destinationX * 40 - transform.position.x) * Time.deltaTime * 5;
            deltaMovementZ = (destinationZ * 40 - transform.position.z) * Time.deltaTime * 5;
        }

        transform.position = new Vector3(transform.position.x + deltaMovementX, transform.position.y, transform.position.z + deltaMovementZ);

        if (Math.Abs(destinationX * 40 - transform.position.x) * Time.deltaTime * 5 <= Math.Abs(deltaMovementX) &&
            Math.Abs(destinationZ * 40 - transform.position.z) * Time.deltaTime * 5 <= Math.Abs(deltaMovementZ))
        {
            deltaMovementX = 0;
            deltaMovementZ = 0;
        }
    }

    void animateRotation (int angle)
    {
        var rotationVector = transform.rotation.eulerAngles;
        if (deltaRotation == 0)
        {
            deltaRotation = (angle - rotationVector.y) * Time.deltaTime * 10;
        }

        rotationVector.y += deltaRotation;
        transform.rotation = Quaternion.Euler(rotationVector);

        if (Math.Abs(angle - rotationVector.y) * Time.deltaTime * 10 <= Math.Abs(deltaRotation))
        {
            deltaRotation = 0;
        }
    }
}