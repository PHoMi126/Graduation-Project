using System.Collections;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (enemyCount < 20)
        {
            xPos = Random.Range(-24, 15);
            zPos = Random.Range(35, 75);
            Instantiate(enemy, new Vector3(xPos, 0.05f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
