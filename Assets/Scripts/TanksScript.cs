using UnityEngine;

using Assets.Game.GameEntities;
using Assets.Game;

public class TanksScript : MonoBehaviour
{
    private Vector3 originPosition;
    private float tPosition;

    private Vector3 originRotation;
    private float tRotation;

    private float deltaTime;
    private float gridSquareSize;

    private float positionY;

    // Use this for initialization
    void Start()
    {
        positionY = gameObject.transform.position.y;

        //setting animation parameters
        Constants constants = new Constants();
        deltaTime = constants.DeltaTime;
        gridSquareSize = constants.GridSquareSize;
    }

    // Update is called once per frame
    void Update()
    {
        int playerNumber = int.Parse(gameObject.name.Substring(4));
        if (playerNumber - 1 < GameManager.Instance.GameEngine.Tanks.Count)
        {
            Tank tank = GameManager.Instance.GameEngine.Tanks[playerNumber - 1];
            if (tank.Health > 0)
            {
                animateMove(tank.PositionX, tank.PositionY);
                animateRotation(tank.Direction);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void animateMove(int destinationX, int destinationZ)
    {
        if (destinationX * gridSquareSize == transform.position.x &&
            destinationZ * gridSquareSize == transform.position.z)
        {
            originPosition = transform.position;
            tPosition = 0;
        }

        transform.position = Vector3.Lerp(originPosition, new Vector3(destinationX * gridSquareSize, positionY, destinationZ * gridSquareSize), tPosition);

        tPosition += deltaTime;
    }

    void animateRotation(Direction direction)
    {
        int angle = getAngle(direction);
        Vector3 rotationVector = transform.rotation.eulerAngles;

        if (angle == rotationVector.y)
        {
            originRotation = transform.rotation.eulerAngles;
            tRotation = 0;
        }

        transform.rotation = Quaternion.Euler(Vector3.Lerp(originRotation, new Vector3(originRotation.x, angle, originRotation.z), tRotation));

        tRotation += deltaTime;
    }

    int getAngle(Direction direction)
    {
        if (direction == Direction.NORTH)
        {
            return 270;
        }
        else if (direction == Direction.EAST)
        {
            return 180;
        }
        else if (direction == Direction.SOUTH)
        {
            return 90;
        }
        else
        {
            return 0;
        }
    }
}
