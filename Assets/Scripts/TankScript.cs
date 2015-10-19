using UnityEngine;
using System;
using Assets.Game.GameEntities;
using Assets.Game;
using System.Collections.Generic;

public class TankScript : MonoBehaviour {

    float deltaMovementX;
    float deltaMovementZ;
    float deltaRotation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        int playerNumber = int.Parse(gameObject.name.Substring(4));
        Tank tank = GameManager.Instance.GameEngine.Tanks[playerNumber - 1];
        if (tank.Health > 0)
        {
            if (GameManager.Instance.GameStarted || GameManager.Instance.GameEnded)
            {
                animateMove(tank.PositionX, tank.PositionY);
                animateRotation(tank.Direction);
            }
            else
            {
                transform.position = new Vector3(tank.PositionX * 80 + 20, transform.position.y, tank.PositionY * 80 + 20);

                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.y = getAngle(tank.Direction);
                transform.rotation = Quaternion.Euler(rotationVector);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    void animateMove (int destinationX, int destinationZ)
    {
        if (deltaMovementX == 0 && deltaMovementZ == 0)
        {
            deltaMovementX = (destinationX * 80 + 20 - transform.position.x) * Time.deltaTime * 10;
            deltaMovementZ = (destinationZ * 80 + 20 - transform.position.z) * Time.deltaTime * 10;
        }

        transform.position = new Vector3(transform.position.x + deltaMovementX, transform.position.y, transform.position.z + deltaMovementZ);

        if (Math.Abs(destinationX * 80 + 20 - transform.position.x) * Time.deltaTime * 10 <= Math.Abs(deltaMovementX) &&
            Math.Abs(destinationZ * 80 + 20 - transform.position.z) * Time.deltaTime * 10 <= Math.Abs(deltaMovementZ))
        {
            deltaMovementX = 0;
            deltaMovementZ = 0;
        }
    }

    void animateRotation (Direction direction)
    {
        int angle = getAngle(direction);
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

    int getAngle(Direction direction)
    {
        if (direction == Direction.NORTH)
        {
            return 90;
        }
        else if (direction == Direction.EAST)
        {
            return 180;
        }
        else if (direction == Direction.SOUTH)
        {
            return 270;
        }
        else
        {
            return 0;
        }
    }
}