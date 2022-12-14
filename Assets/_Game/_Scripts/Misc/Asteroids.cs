using UnityEngine;

public class Asteroids : MonoBehaviour 
{
    public GameObject AsteroidPrefab;

    private void Awake()
    {
        FunctionTimer.Create(() => { 
            var asteroid = Instantiate(AsteroidPrefab, new Vector3(10, 10, 0), Quaternion.identity);
            asteroid.GetComponent<Asteroid>().Direction = new Vector3(-1, -1, 0);
            asteroid.GetComponent<Asteroid>().Speed = 1f;
            asteroid.GetComponent<Rotate>().Rotation = new Vector3(0, 0, 20);
        }, 2f, "Create Asteroid");
    }
}