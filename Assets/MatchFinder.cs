using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.3f);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject currenFruit = board.allFruits[i, j];
                if (currenFruit != null)
                {
                    if (i>0 && i<board.width-1)
                    {
                        GameObject leftfruit = board.allFruits[i - 1, j];
                        GameObject rightfruit = board.allFruits[i + 1, j];
                        if (leftfruit != null && rightfruit != null)
                        {
                            if (leftfruit.tag == currenFruit.tag && rightfruit.tag == currenFruit.tag)
                            {
                                leftfruit.GetComponent<Fruit>().isMatched = true;
                                rightfruit.GetComponent<Fruit>().isMatched = true;
                                currenFruit.GetComponent<Fruit>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < 7)
                    {
                        GameObject upfruit = board.allFruits[i, j+1];
                        GameObject downfruit = board.allFruits[i, j-1];
                        if (upfruit != null && downfruit != null)
                        {
                            if (upfruit.tag == currenFruit.tag && downfruit.tag == currenFruit.tag)
                            {
                                upfruit.GetComponent<Fruit>().isMatched = true;
                                downfruit.GetComponent<Fruit>().isMatched = true;
                                currenFruit.GetComponent<Fruit>().isMatched = true;
                            }
                        }
                    }

                }
            }
        }
    }

}
