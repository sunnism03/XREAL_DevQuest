using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterVariants;   // Variant 배열
    public int spawnCount = 3;
    public Transform environment;          // Environment 프리팹 Transform
                                           // (씬에 배치된 Environment 오브젝트 드래그)

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Transform target = GameObject.FindWithTag("Player").transform;

        float sizeX = environment.localScale.x * 200f;
        float sizeZ = environment.localScale.z * 200f;

        for (int i = 0; i < spawnCount; i++)
        {
            // ✅ Environment 범위 안에서 랜덤 위치 생성
            float randX = Random.Range(-sizeX/2f, sizeX/2f);
            float randZ = Random.Range(-sizeZ/2f, sizeZ/2f);

            Vector3 randomOffset = new Vector3(randX, 0, randZ);
            Vector3 pos = transform.position + randomOffset;

            // ✅ Variant 랜덤 선택
            int r = Random.Range(0, monsterVariants.Length);
            GameObject obj = Instantiate(monsterVariants[r], pos, Quaternion.identity);

            // ✅ 타겟(Player) 설정
            obj.GetComponent<Monster>().target = target;
        }
    }
}