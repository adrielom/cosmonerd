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
    public float rate;
    bool canLoad = false;

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
        InvokeRepeating ("GetsRandomCoin", 2f, rate);
    }

    GameObject PopulateCoinsPrefabs(Coin c)
    {
        GameObject i = prefab;
        CoinManager cm = i.GetComponent<CoinManager>();
        cm.ResetPosition();
        cm.speed = c.speed;
        cm.color = c.color;
        cm.value = c.value;
        return i;
    }

    public void GetsRandomCoin()
    {   
        StartCoroutine (loadCoin (Random.Range(0, 10)));
        if (canLoad) {
            if (allCoins.Count <= 0)
            {
                allCoins = new List<GameObject>(toUseCoins);
                toUseCoins.RemoveRange(0, toUseCoins.Count);
            }

            int value = Random.Range(0, allCoins.Count);
            GameObject g = allCoins[value];
            allCoins.RemoveAt(value);
            toUseCoins.Add(g);
            g.SetActive(true);
            canLoad = false;
        }
    }

    IEnumerator loadCoin(int f)
    {
        yield return new WaitForSeconds (f);
        canLoad = true;
    }
}
