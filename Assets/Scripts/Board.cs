using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 8;
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
                if (j < 8)
                {
                    Vector2 tempos = new Vector2(i, j);
                    GameObject backgroundTemp = Instantiate(tilePrefab, tempos, Quaternion.identity);
                    backgroundTemp.name = "(" + i + ", " + j + ")";
                    backgroundTemp.transform.parent = transform;
                }

                int randomInt = Random.Range(0, fruits.Length);

                int maxIteration = 0;
                while (MathesPoint(i, j, fruits[randomInt]) && maxIteration < 100)
                {
                    randomInt = Random.Range(0, fruits.Length);
                    maxIteration++;
                }
                maxIteration = 0;

                GameObject fruit = Instantiate(fruits[randomInt], transform.position, Quaternion.identity);
                fruit.transform.position = new Vector3(i, j, -1);
                if (j >= 8)
                    fruit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                fruit.transform.parent = this.transform;
                allFruits[i, j] = fruit;
            } 
        }
    }

    private bool MathesPoint(int coloum, int row, GameObject fruit)//make board without matching
    {
        if (coloum > 1 && row  > 1)
        {
            if (allFruits[coloum-1,row].tag == fruit.tag && allFruits[coloum-2,row].tag == fruit.tag)
            {
                return true;
            }
            if (allFruits[coloum , row-1].tag == fruit.tag && allFruits[coloum , row-2].tag == fruit.tag)
            {
                return true;
            }
        }
        else if (coloum <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allFruits[coloum,row -1].tag == fruit.tag && allFruits[coloum,row-2].tag == fruit.tag)
                {
                    return true;
                }
            }
            if (coloum > 1)
            {
                if (allFruits[coloum - 1, row].tag == fruit.tag && allFruits[coloum - 2, row].tag == fruit.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyMatchesOnPos(int coloum, int row)
    {
        if (allFruits[coloum,row].GetComponent<Fruit>().isMatched)
        {
            allFruits[coloum, row].transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            Destroy(allFruits[coloum, row]);
            allFruits[coloum, row] = null;
        }
    }
    
    public void DestroyMatches()
    {
        StopCoroutine(DownRowCo());

        for (int i = 0; i < width; i++)//only destroy on 8x8 board
        {
            for (int j = 0; j < 8; j++)
            {
                if (allFruits[i,j] != null)
                {
                    DestroyMatchesOnPos(i, j);
                }
            }
        }
            StartCoroutine(DownRowCo());//if Destroy happened bring the upside fruits
    }


    private IEnumerator DownRowCo()
    {
        int nullCounter = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allFruits[i,j] == null)
                {
                    nullCounter++;
                }
                else if(nullCounter > 0)
                {
                    allFruits[i, j].GetComponent<Fruit>().row -= nullCounter;
                    if (j < 8 + nullCounter)
                        allFruits[i, j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                    allFruits[i, j].GetComponent<Fruit>().prevColoumn = allFruits[i, j].GetComponent<Fruit>().coloumn;
                    allFruits[i, j].GetComponent<Fruit>().prevRow = allFruits[i, j].GetComponent<Fruit>().row;
                    allFruits[i, j] = null;
                }
            }
            nullCounter = 0;
        }
        yield return new WaitForSeconds(0.5f);
        // there is should be a destroy check
        DestroyMatches();
    }
}
