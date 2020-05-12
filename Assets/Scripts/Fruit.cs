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
    private Board board;
    private GameObject nearfruit;

    void Start()
    {
        board = FindObjectOfType<Board>();
        coloumn = (int)transform.position.x;
        row = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        coloumn = (int)transform.position.x;
        row = (int)transform.position.y;
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
    private void MakeMove(int x, int y)
    {
        nearfruit = board.allFruits[coloumn + x, row + y];
        transform.position = new Vector2(coloumn + x, row + y);
        nearfruit.transform.position = new Vector2(coloumn, row);

        GameObject tempfruit;
        tempfruit = board.allFruits[coloumn, row];
        board.allFruits[coloumn, row] = board.allFruits[coloumn + x, row + y];
        board.allFruits[coloumn + x, row + y] = tempfruit;
    }

    void MovePieces()
    {
        if (angle > -45 &&  45 >= angle)//Right Swipe
            MakeMove(1, 0);

        else if (angle <= -45 && angle > -135 )//Down Swipe
            MakeMove(0, -1);

        else if (angle <= -135 || angle > 135)//Left Swipe
            MakeMove(-1, 0);
        
        else if(angle <= 135 && angle > 45 )//Up Swipe
            MakeMove(0, 1);
    }

    //private void UpdateFruits()
    //{
    //    GameObject tempfruit;
    //    tempfruit = board.allFruits[coloumn, row];
    //    board.allFruits[coloumn, row] = board.allFruits[coloumn + 1, row];
    //    board.allFruits[coloumn + 1, row] = tempfruit;
    //}
}
