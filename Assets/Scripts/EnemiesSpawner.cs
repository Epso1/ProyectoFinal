using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPosition = (Vector2)player.transform.position + new Vector2(Random.Range(0.2f, 2f), Random.Range(0.2f,2f));
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }       
        
    }
}
