using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    [Serializable]
    public class CoinList
    {
        public List<Coin> Coins;
    }

    [Serializable]
    public class Coin
    {
        public string name;
        public float speed;
        public int value;
        //from 0 to 255
        public byte[] color;
    }

    public GameObject prefab;
    Coin[] coins;
    public int maxCoins;
    public List<GameObject> allCoins;
    public List<GameObject> toUseCoins;
    public float rate;
    bool canLoad = false;

    // Use this for initialization
    void Start()
    {
        AssetBundle aBCoins = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + "coinsjson");
        TextAsset textJson = aBCoins.LoadAsset<TextAsset>("coins");
        var coinlist = JsonUtility.FromJson<CoinList>(textJson.text);
        coins = coinlist.Coins.ToArray();
        int group = (int)Mathf.Floor(maxCoins / coins.Length);
        foreach (Coin p in coins)
        {
            for (int i = 0; i < group; i++)
            {
                GameObject g = Instantiate(prefab, transform.position, Quaternion.identity);
                PopulateCoinsPrefabs(p);
                allCoins.Add(g);
                g.transform.parent = transform;
                g.SetActive(false);
            }
        }
        InvokeRepeating("GetsRandomCoin", 2f, rate);
    }

    GameObject PopulateCoinsPrefabs(Coin c)
    {
        GameObject i = prefab;
        CoinManager cm = i.GetComponent<CoinManager>();
        cm.ResetPosition();
        cm.speed = c.speed;
        cm.color = new Color32(c.color[0], c.color[1], c.color[2], c.color[3]);
        cm.value = c.value;
        return i;
    }

    public void GetsRandomCoin()
    {
        StartCoroutine(loadCoin(UnityEngine.Random.Range(0, 10)));
        if (canLoad)
        {
            if (allCoins.Count <= 0)
            {
                allCoins = new List<GameObject>(toUseCoins);
                toUseCoins.RemoveRange(0, toUseCoins.Count);
            }

            int value = UnityEngine.Random.Range(0, allCoins.Count);
            GameObject g = allCoins[value];
            allCoins.RemoveAt(value);
            toUseCoins.Add(g);
            g.SetActive(true);
            canLoad = false;
        }
    }

    IEnumerator loadCoin(int f)
    {
        yield return new WaitForSeconds(f);
        canLoad = true;
    }
}
