using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    public Color color;
    [SerializeField]
    public int value;
    [SerializeField]
    public float speed;

    void Update()
    {
        Move();
        //GetComponent<SpriteRenderer>().color = color;
    }

    public void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y < -6.5f)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(6.5f, 8), 0);
    }
}
