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

    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
        ResetPosition();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y < -6.5f)
        {
            gameObject.SetActive(false);
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(Random.Range(-2.5f, 2.5f), 6.2f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }
}
