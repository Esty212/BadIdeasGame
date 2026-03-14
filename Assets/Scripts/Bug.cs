using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public Transform lightOrb;
    public float speed = 2f;
    public float avoidDistance = 0.5f;
    public float avoidStrength = 2f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            lightOrb.position,
            speed * Time.deltaTime
        );

        Vector2 moveToLight = (lightOrb.position - transform.position).normalized;

        Vector2 avoid = Vector2.zero;

        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, avoidDistance);

        foreach (Collider2D col in nearby)
        {
            if (col.gameObject != gameObject && col.CompareTag("Bug"))
            {
                Vector2 diff = transform.position - col.transform.position;
                avoid += diff.normalized;
            }
        }

        Vector2 finalDirection = (moveToLight + avoid * avoidStrength).normalized;

        transform.position += (Vector3)(finalDirection * speed * Time.deltaTime);

        Vector2 noise = Random.insideUnitCircle * 0.1f;
        finalDirection += noise;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bug"))
        {
            Instantiate(gameObject, transform.position, Quaternion.identity);
        }
    }

}
