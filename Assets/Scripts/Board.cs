using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject tilePrefab;
    private TileBackground[,] allTiles;
    public GameObject[] fruits;
    public GameObject[,] allFruits;

    void Start()
    {
        allTiles = new TileBackground[width, height];
        allFruits = new GameObject[width, height];
        MakeBoard();
    }

    private void MakeBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempos = new Vector2(i, j);
                GameObject backgroundTemp = Instantiate(tilePrefab, tempos, Quaternion.identity);
                backgroundTemp.name = "(" + i + ", " + j + ")";
                backgroundTemp.transform.parent = transform;


                int randomInt = Random.Range(0, fruits.Length);
                GameObject fruit = Instantiate(fruits[randomInt], transform.position, Quaternion.identity);
                fruit.transform.position = new Vector3(i, j, -1);
                fruit.transform.parent = backgroundTemp.transform;

                allFruits[i, j] = fruit;

            } 
        }
    }
}
