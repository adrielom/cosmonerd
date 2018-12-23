using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{

    public float speed;
    public int value, spawnRate = 1;
    public bool canMove;

    void Start()
    {
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Move();
        }
    }

    IEnumerator WaitFor(float f)
    {
        yield return new WaitForSeconds(f * spawnRate);
        canMove = true;
    }

    void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y <= -6.5f)
        {
            canMove = false;
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(6.5f, 8), 0);
        StartCoroutine(WaitFor(Random.Range(3, 10)));
    }
}
