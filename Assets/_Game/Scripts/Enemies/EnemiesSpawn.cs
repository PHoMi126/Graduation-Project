using System.Collections;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{
    [Header("Enemy Spawn Type")]
    public GameObject enemy;
    public int enemyCount;

    [Header("Random Spawn Positions")]
    public int xPos; //Ignore in Inspector
    public int zPos; //Ignore in Inspector
    public float yPos;
    public int x1, x2, z1, z2;

    [Header("Random Spawn Numbers")]
    public int minSpawn;
    public int maxSpawn;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (enemyCount <= Random.Range(minSpawn, maxSpawn))
        {
            enemy.SetActive(true);
            xPos = Random.Range(x1, x2);
            zPos = Random.Range(z1, z2);
            Instantiate(enemy, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
