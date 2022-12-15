using UnityEngine;

public class Asteroids : MonoBehaviour 
{
    public GameObject AsteroidPrefab;
    private float SpawnerTimer;
    public float TimeToSpawnAsteroids = 15f;
    public int SpawnAmount = 3;

    private void Update()
    {
        SpawnerTimer -= Time.deltaTime;
        if (SpawnerTimer <= 0)
        {
            for (int i = 0; i < SpawnAmount; i++) SpawnAsteroids();
            SpawnerTimer = TimeToSpawnAsteroids;
        }
    }

    public void SpawnAsteroids()
    {
        var asteroid = Instantiate(AsteroidPrefab, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);

        if (asteroid.transform.position.x > 0)
        {
            asteroid.GetComponent<Asteroid>().Direction = new Vector3(-1, -1, 0);
        } else
        {
            asteroid.GetComponent<Asteroid>().Direction = new Vector3(1, -1, 0);
        }

        asteroid.GetComponent<Asteroid>().Speed = Random.Range(0.3f, 0.8f);
        asteroid.GetComponent<Rotate>().Rotation = new Vector3(0, 0, Random.Range(10, 20));
    }
}