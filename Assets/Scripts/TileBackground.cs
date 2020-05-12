using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBackground : MonoBehaviour
{
    public GameObject[] fruits;

    void Start()
    {
        //SpawnFruit();
    }
    private void SpawnFruit()
    {
        int randomInt = Random.Range(0, fruits.Length);
        GameObject fruit = Instantiate(fruits[randomInt], transform.position, Quaternion.identity);
        fruit.transform.position += new Vector3(0, 0, -1);
        fruit.transform.parent = transform;
    }
}
