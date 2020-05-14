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

                int maxIteration = 0;
                while (MathesPoint(i, j, fruits[randomInt]) && maxIteration < 100)
                {
                    randomInt = Random.Range(0, fruits.Length);
                    maxIteration++;
                    Debug.Log(maxIteration);

                }
                maxIteration = 0;

                GameObject fruit = Instantiate(fruits[randomInt], transform.position, Quaternion.identity);
                fruit.transform.position = new Vector3(i, j, -1);
                fruit.transform.parent = backgroundTemp.transform;
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
            Destroy(allFruits[coloum, row]);
            allFruits[coloum, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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
                    allFruits[i, j] = null;//cotrol it!

                }

            }
            nullCounter = 0;
        }
        yield return new WaitForSeconds(.4f);

        StartCoroutine(FillBoardCo());

    }

    private void RefillTheBlanks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allFruits[i,j] == null)
                {
                    int tempInt = Random.Range(0, fruits.Length);
                    allFruits[i, j] = Instantiate(fruits[tempInt], new Vector2(i, j), Quaternion.identity);
                }
            }
        }
    }

    private bool GetMatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allFruits[i,j] != null)
                {
                    if (allFruits[i,j].GetComponent<Fruit>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillTheBlanks();
        yield return new WaitForSeconds(.5f);

        while (GetMatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

    }


}
