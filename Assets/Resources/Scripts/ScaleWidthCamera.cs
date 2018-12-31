using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour {

    public int targetWidth = 640;
    public float pixelsToUnits = 100;

    new Camera camera;

    void Awake () {
        camera = GetComponent<Camera> ();
    }

    void Update () {

        int height = Mathf.RoundToInt (targetWidth / (float)Screen.width * Screen.height);

        camera.orthographicSize = height / pixelsToUnits / 2;
    }
}