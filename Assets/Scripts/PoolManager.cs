using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    [System.Serializable]
    public class Coin
    {
        public float speed;
        public int value;
        public Color color;
    }


    public GameObject prefab;
    public Coin[] coins;
    public int maxCoins;
    public List<GameObject> allCoins;
    public List<GameObject> toUseCoins;

    // Use this for initialization
    void Awake()
    {
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
    }

    GameObject PopulateCoinsPrefabs(Coin c)
    {
        GameObject i = prefab;
        CoinManager cm = i.GetComponent<CoinManager>();
        cm.speed = c.speed;
        cm.color = c.color;
        cm.value = c.value;
        return i;
    }

    public GameObject GetsRandomCoin()
    {
        if (allCoins.Count <= 0)
        {
            allCoins = new List<GameObject>(toUseCoins);
            toUseCoins.RemoveRange(0, toUseCoins.Count);
        }

        int value = Random.Range(0, allCoins.Count);
        GameObject g = allCoins[value];
        allCoins.RemoveAt(value);
        toUseCoins.Add(g);

        return g;
    }
}
