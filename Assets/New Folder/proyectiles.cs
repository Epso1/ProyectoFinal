using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectiles : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private Rigidbody2D rb;
    private float destroyProject = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        LaunchProjectiles();
    }
    private void LaunchProjectiles()
    {
        Vector2 directiomToPlayer = (player.position - transform.position).normalized;
        rb.velocity = directiomToPlayer * speed;
        StartCoroutine(DestroyProjectiles());
    }

    IEnumerator DestroyProjectiles()
    {
        float destroyTime = destroyProject;
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }
}
