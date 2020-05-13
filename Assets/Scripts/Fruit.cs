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
    }

    // Update is called once per frame
    void Update()
    {
        targetX = coloumn;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {//swipe fruits with lerping
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        }
        else
        {//set positon directly
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            board.allFruits[coloumn, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {//swipe fruits with lerping
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        }
        else
        {//set positon directly
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            board.allFruits[coloumn, row] = this.gameObject;
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
        angle = Mathf.Atan2(lastTouchPos.y - firstTouchPos.y, lastTouchPos.x - firstTouchPos.x)*180/Mathf.PI;
        MovePieces();
    }
    
    void MovePieces()
    {
        if (angle > -45 && 45 >= angle && coloumn < board.width)//Right Swipe
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
        
        else if (angle <= 135 && angle > 45 && row < board.height)//Up Swipe
        {
            swipefruit = board.allFruits[coloumn, row + 1];
            swipefruit.GetComponent<Fruit>().row -= 1;
            row += 1;
        }
    }

    private void CheckMatches()
    {

    }

}
