﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    move,
    stop
}

public class Board : MonoBehaviour
{
    public GameState currentstate = GameState.move;
    public int width = 8;
    public int height;

    public GameObject tilePrefab;
    private TileBackground[,] allTiles;
    public GameObject[] fruits;
    public GameObject[,] allFruits;

    private bool ismatch = false;

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
                if (j < 8)
                {
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

                GameObject fruit = Instantiate(fruits[randomInt], tempos, Quaternion.identity);
                if (j >= 8)
                {
                    fruit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                    if(fruit.transform.GetComponent<BoxCollider2D>() != null)
                        fruit.transform.GetComponent<BoxCollider2D>().enabled = false;
                }
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

    
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)//only destroy on 8x8 board
        {
            for (int j = 0; j < 8; j++)
            {
                if (allFruits[i,j] != null)
                {
                    StartCoroutine(DestroyMatchesOnPos(i, j));
                }
            }
        }
        if (ismatch == true)
            StartCoroutine(DownRowCo());//if Destroy happened bring the upside fruits
        else
            currentstate = GameState.move;

    }

    private IEnumerator DestroyMatchesOnPos(int coloum, int row)
    {
        allFruits[coloum, row].transform.localScale = new Vector3(1, 1, 1);

        if (allFruits[coloum, row].GetComponent<Fruit>().isMatched)
        {
            //currentstate = GameState.stop;
            ismatch = true;
            allFruits[coloum, row].transform.localScale = Vector3.Lerp(allFruits[coloum, row].transform.localScale
            , new Vector3(1.3f, 1.3f, 1.3f),
            .4f);

            yield return new WaitForSeconds(0.1f);

            Destroy(allFruits[coloum, row]);
            allFruits[coloum, row] = null;
        }
    }

    private IEnumerator DownRowCo()
    {
        ismatch = false;
        yield return new WaitForSeconds(.7f);

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
                    {
                        allFruits[i, j].transform.GetComponent<BoxCollider2D>().enabled = true;
                        allFruits[i, j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }

                    allFruits[i, j].GetComponent<Fruit>().prevColoumn = allFruits[i, j].GetComponent<Fruit>().coloumn;
                    allFruits[i, j].GetComponent<Fruit>().prevRow = allFruits[i, j].GetComponent<Fruit>().row;
                    allFruits[i, j] = null;
                }
            }
            nullCounter = 0;
        }
        yield return new WaitForSeconds(0.5f);

        DestroyMatches();
        
    }
}
