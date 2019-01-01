using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* -----------------------------------------READ-ME------------------------------------------------

                    A FEW NOTES WORTH READING - So far locally stored

    1 *** AssetBundles is great in memory to load up data. ***
    
    Creates asset bundle through editor
    Build the data via the Editor/CreateAssetBundle.cs file
    Creates an instance of the AssetBundle from path - StreamingAssets Folder
    Loads up the data through streaming assets folder (certain path for multiple devices)
    Unload AssetBundle to clear memory

    2 *** An Alternative for this - that's not so good due performance issues *cause it's baked* - is Resorces.Load<type>("path"); -- Good for projects that performance is not an issue ***

    3 *** To read JSON files you deserialize it from json using jsonutility into an object ***

    if your json is an standart one that holds down a list of objects you must create a wrapper class that shall hold down a list of your objects. THIS LIST MUST BE WRITTEN EXACTLY LIKE THE JSON FILE'S KEY!!!!!!!!!! Otherwise you'll get null all over the places

    4 *** To write to the json you add elements to the wrapper list and then serialize IT;
    if isn't a list of objects you serialize the objet straight out. jsonutility to json

    5 *** To read from server you must make a web request or a www - pass it through an IEnumerator that yields return of type www or web request, hence it must download the info before proceding to another step

 ----------------------------------------------------------------------------------------------- */
public class RocketManager : MonoBehaviour
{

    //Wrapper class to hold down the list of items in the json
    #region  WRAPPER CLASS

    [Serializable] // All classes to deserialize from external files MUSSSSSSSSSSSSST BE 'SERIALIZABLE'
    public class RocketList
    {
        //This object's got to be named exactly like the json key - In the Json I have { "Rockets" : [{"smthg"}, {"smthgElse"}] }
        public List<Rocket> Rockets;
    }

    #endregion

    #region PARTICLE CLASS
    [Serializable]

    //Class the holds down info from the json object to populate particles after instantiating them from Resources.Load - Issue with pink materials with streaming assets lead into this decision
    public class Particle
    {
        //Json doesnt store complex data, vector3 becomes an array of floats
        public float[] position;
        public float[] scale;
    }
    #endregion

    //Individual Objects in the Json
    [Serializable]
    #region  ROCKET CLASS
    public class Rocket
    {
        //If the value inherits from monobehaviour it's going to print out null
        public string name;
        public string image;
        public string particle;
        public float speed;
        public float angle;
        public Particle particleInfo;

        public Rocket(string name, string image, string particle, float speed)
        {
            this.name = name;
            this.image = image;
            this.particle = particle;
            this.speed = speed;
            this.angle = speed;
        }

        public Rocket(float speed)
        {
            this.speed = speed;
        }

        public Rocket()
        {
        }

    }
    #endregion

    #region  VARIABLES DEFINITION
    string PATH, defaultRocket, currentRocket;
    TextAsset json;
    float speed, angle;
    public List<Rocket> rock;

    #endregion
    void Start()
    {
        InitializeData();
    }

    #region READS DATA FROM JSON AND ASSETBUNDLE
    void InitializeData()
    {
        defaultRocket = "Awesome Galaxy Rocket";
        //constant path to the asset bundles folder
        PATH = Application.streamingAssetsPath + "/AssetBundles/";
        //To load up an asset bundle you need to access the path to the bundle through the streaming assets folder
        AssetBundle aBJson = AssetBundle.LoadFromFile(PATH + "rocketsjson");
        //and load the asset sitting there "as type" OR <Type> - Second one is better
        json = aBJson.LoadAsset("rockets") as TextAsset;

        PlayerPrefs.SetString("currentRocket", defaultRocket);
        //Checkign for current rockets preferences 
        if (PlayerPrefs.GetString("currentRocket") == null || PlayerPrefs.GetString("currentRocket") == "")
        {
            PlayerPrefs.SetString("currentRocket", defaultRocket);
        }
        else
        {
            currentRocket = PlayerPrefs.GetString("currentRocket");
        }

        //Creates an instance of Rocket to deserialize the Json content into
        Rocket myRocket = new Rocket();

        //To deserialize a list of objects from the json you've to wrapp the object class on another one that has a list sitting on it
        var rocketlist = JsonUtility.FromJson<RocketList>(json.text);

        //Checks for the json data that corresponds to the rockets preferences
        foreach (var r in rocketlist.Rockets)
        {
            if (r.name == currentRocket)
            {
                myRocket = r;
            }
        }

        //Gets the sprites assetbundle from folder
        AssetBundle aBSprite = AssetBundle.LoadFromFile(PATH + "sprites");
        //Loads it through
        Sprite s = aBSprite.LoadAsset<Sprite>(myRocket.image);
        GetComponent<SpriteRenderer>().sprite = s;

        //Sets the parsed info into the actual gameobject
        speed = myRocket.speed;
        angle = myRocket.angle;

        //Loading from resources bakes the info whereas the assetbundle doesn't
        //Resources.Load holds the info, but doesnt instantiate - it also accepts 'as type' or 'typeof(type)' - but <type> it's best for C#
        var part = Instantiate(Resources.Load<GameObject>("Particles/" + myRocket.particle));
        //makes the instantiated particle a child of the rocket object
        part.transform.parent = transform;
        //Resets its local position and scale based on the info saved on json
        part.transform.localScale = new Vector3(myRocket.particleInfo.position[0], myRocket.particleInfo.position[1], myRocket.particleInfo.position[2]);
        part.transform.localPosition = new Vector3(myRocket.particleInfo.scale[0], myRocket.particleInfo.scale[1], myRocket.particleInfo.scale[2]);

        //clear memory of the loaded files in the assetbundle
        aBJson.Unload(false);
        aBSprite.Unload(false);

        gameObject.AddComponent<PolygonCollider2D>();
    }

    #endregion

    #region ACTUAL GAME LOGIC

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
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angle);
        }
        else if (transform.position.x - objPosition.x == 0)
        {
            ResetRotation();
        }
        else
        {
            ;
            //Rotates left on movement
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -angle);
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

    #endregion
}
