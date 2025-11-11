using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int spawnCount = 3;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Transform target = GameObject.FindWithTag("Player").transform;

        for (int i = 0; i < spawnCount; i++)
        {

            Vector3 randomOffset = new Vector3(
                Random.Range(-3f, 3f),
                0,
                Random.Range(-3f, 3f)
            );

            Vector3 pos = transform.position + randomOffset * i;

            GameObject obj = Instantiate(monsterPrefab, pos, Quaternion.identity);
            obj.GetComponent<Monster>().target = target;
        }
    }
}