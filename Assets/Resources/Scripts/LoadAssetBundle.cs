using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadAssetBundle : MonoBehaviour
{
    public string assetBundle;
    string[] objects;
    void Awake()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles";
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory, assetBundle));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        objects = myLoadedAssetBundle.GetAllAssetNames();
        for (int i = 0; i < objects.Length; i++)
        {
            var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(objects[i]);
            Instantiate(prefab);
        }

        myLoadedAssetBundle.Unload(false);
    }
}