using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
    public float speed;

    void OnMouseDrag()
    {
        MoveRocket();
    }

    public void MoveRocket()
    {

        //Drag Rocket
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.y = transform.position.y;
        objPosition.z = 0;
        objPosition.x = Mathf.Clamp(objPosition.x, -2.5f, 2.5f);

        transform.position = Vector3.MoveTowards(transform.position, objPosition, speed * Time.deltaTime);

    }
}
