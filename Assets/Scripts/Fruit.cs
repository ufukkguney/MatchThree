using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;

    public float angle;

    public int coloumn;
    public int row;
    public int targetX, targetY;
    public int prevColoumn;
    public int prevRow;

    public bool isMatched = false;



    private Board board;
    private GameObject swipefruit;
    private Vector2 tempPos;
    
    void Start()
    {
        board = FindObjectOfType<Board>();

        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;

        row = targetY;
        coloumn = targetX;

        prevColoumn = coloumn;
        prevRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMatches();

        targetX = coloumn;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {//swipe fruits with lerping
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .5f);
            if (board.allFruits[coloumn,row] != this.gameObject)
            {
                board.allFruits[coloumn, row] = this.gameObject;
            }
        }
        else
        {//set positon directly
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            //board.allFruits[coloumn, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {//swipe fruits with lerping
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .5f);
            if (board.allFruits[coloumn, row] != this.gameObject)
            {
                board.allFruits[coloumn, row] = this.gameObject;
            }
        }
        else
        {//set positon directly
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            //board.allFruits[coloumn, row] = this.gameObject;
        }
        
       
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FingerMovesAngle();
    }

    void FingerMovesAngle()
    {
        if (Mathf.Abs(lastTouchPos.y - firstTouchPos.y ) > 1 ||Mathf.Abs(lastTouchPos.x  - firstTouchPos.x) >1 )//check if its just a click
        {
            angle = Mathf.Atan2(lastTouchPos.y - firstTouchPos.y, lastTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            MovePieces();
        }
    }
    
    void MovePieces()
    {
        if (angle > -45 && 45 >= angle && coloumn < board.width-1)//Right Swipe
        {
            swipefruit = board.allFruits[coloumn + 1, row];
            swipefruit.GetComponent<Fruit>().coloumn -= 1;
            coloumn += 1;
        }
        else if (angle <= -45 && angle > -135 && row > 0)//Down Swipe
        {
            swipefruit = board.allFruits[coloumn, row-1];
            swipefruit.GetComponent<Fruit>().row += 1;
            row -= 1;
        }
        else if (angle <= -135 || angle > 135 && coloumn > 0)//Left Swipe
        {
            swipefruit = board.allFruits[coloumn - 1, row];
            swipefruit.GetComponent<Fruit>().coloumn += 1;
            coloumn -= 1;
        }
        
        else if (angle <= 135 && angle > 45 && row < board.height-1)//Up Swipe
        {
            swipefruit = board.allFruits[coloumn, row + 1];
            swipefruit.GetComponent<Fruit>().row -= 1;
            row += 1;
        }
        

        StartCoroutine(ControlMoveCo());

    }
    

    public IEnumerator ControlMoveCo()
    {
        yield return new WaitForSeconds(0.5f);
        if (swipefruit != null)
        {
            if (!isMatched && !swipefruit.GetComponent<Fruit>().isMatched)
            {
                swipefruit.GetComponent<Fruit>().coloumn = coloumn;
                swipefruit.GetComponent<Fruit>().row = row;
                row = prevRow;
                coloumn = prevColoumn;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }

    public void CheckMatches()
    {
        if (coloumn > 0 && coloumn < board.width -1 && row >= 0 && row <= 7)
        {
            GameObject leftfruit = board.allFruits[coloumn - 1, row];
            GameObject rightfruit = board.allFruits[coloumn + 1, row];
            if (leftfruit != null && rightfruit != null)
            {
                if (leftfruit.gameObject.tag == this.transform.gameObject.tag && rightfruit.gameObject.tag == this.transform.gameObject.tag)
                {
                    leftfruit.GetComponent<Fruit>().isMatched = true;
                    rightfruit.GetComponent<Fruit>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < 7)
        {
            GameObject upfruit = board.allFruits[coloumn, row + 1];
            GameObject downfruit = board.allFruits[coloumn, row - 1];
            if (upfruit != null && downfruit != null)
            {
                if (upfruit.gameObject.tag == this.transform.gameObject.tag && downfruit.gameObject.tag == this.transform.gameObject.tag)
                {
                    upfruit.GetComponent<Fruit>().isMatched = true;
                    downfruit.GetComponent<Fruit>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

}
