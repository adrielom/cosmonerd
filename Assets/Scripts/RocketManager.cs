﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RocketManager : MonoBehaviour
{
    //Wrapper class to hold down the list of items in the json
    [Serializable]
    public class RocketList
    {
        //This object's got to be named exactly like the json key - In the Json I have { "Rockets" : [{"smthg"}, {"smthgElse"}] }
        public List<Rocket> Rockets;
    }

    //Individual Objects in the Json
    [Serializable]
    public class Rocket
    {
        //If the value inherits from monobehaviour it's going to print out null
        public string name;
        public SpriteRenderer image;
        public ParticleSystem particle;
        public float speed;
        public Rocket(string name, SpriteRenderer image, ParticleSystem particle, float speed)
        {
            this.name = name;
            this.image = image;
            this.particle = particle;
            this.speed = speed;
        }

        public Rocket(float speed)
        {
            this.speed = speed;
        }

    }

    //string PATH = Application.dataPath;
    public TextAsset json;
    float speed;
    public List<Rocket> rock;

    void Start()
    {
        //To deserialize a list of objects from the json you've to wrapp the object class on another one that has a list sitting on it
        var rocketlist = JsonUtility.FromJson<RocketList>(json.text);
        print(rocketlist);
        print(rocketlist.ToString());

    }

    void Update()
    {
        //Moves rotation back slowly
        if (transform.rotation.z != 0)
        {
            ResetRotation();
        }
        // Makes the y position constant throughout the game
        transform.position = new Vector3(transform.position.x, -2.35f, transform.position.z);
    }

    void OnMouseDrag()
    {
        MoveRocket();
    }

    public void MoveRocket()
    {
        //Drag Rocket
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, transform.position.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Resets position
        objPosition.y = transform.position.y;
        objPosition.z = 0;
        //Clamps player's movement to left and right edges
        objPosition.x = Mathf.Clamp(objPosition.x, -2.5f, 2.5f);
        //makes player move with a slightly offset delay
        transform.position = Vector3.MoveTowards(transform.position, objPosition, speed * Time.deltaTime);

        //Checks if the touch was on left or right part of the screen compared to this object
        if (transform.position.x - objPosition.x > 0)
        {
            //Rotates right on movement
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 6);
        }
        else if (transform.position.x - objPosition.x == 0)
        {
            ResetRotation();
        }
        else
        {
            //Rotates left on movement
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -6);
        }
    }

    public void ResetRotation()
    {
        //Sets moves rotation to target's one
        Quaternion q = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * speed * 10);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            CoinManager c = other.gameObject.GetComponent<CoinManager>();
            c.gameObject.SetActive(false);
            c.ResetPosition();
        }
    }

}
