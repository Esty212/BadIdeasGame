using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public Transform lightOrb;
    public float speed = 2f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            lightOrb.position,
            speed * Time.deltaTime
        );
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bug"))
        {
            Instantiate(gameObject, transform.position, Quaternion.identity);
        }
    }

}
