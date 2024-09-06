using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameBoard Board {  get; private set; }
    public Vector3Int Position { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public TetrominoData TData { get; private set; }

    public void Initialize(GameBoard board, Vector3Int position, TetrominoData tData)
    {
        Board = board;
        Position = position;
        TData = tData;

       /*  --------------------Bir alt sat�rdaki kodun uzun hali
        if(Cells == null)
        {
            Cells = new Vector3Int[tData.Cells.Length];
        }
       */
        Cells ??= new Vector3Int[tData.Cells.Length];

        for (int i = 0; i < tData.Cells.Length; ++i)
        {
            Cells[i] = (Vector3Int)tData.Cells[i]; //Type Casting (Vector2Int t�r�n� Vector3Int'e �evirdik)
        }
    }

    private void Update()
    {
        Board.Clear(this);

        if(Input.GetKeyDown(KeyCode.A))
        {
            HandleMovement(Vector2Int.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        { 
            HandleMovement(Vector2Int.right);
        }

        Board.Set(this);
    }

    private bool HandleMovement(Vector2Int translation)
    {
        Vector3Int newPosition = Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool isValid = Board.IsValidPosition(this, newPosition);

        if (isValid)
        {
            Position = newPosition;
        }

        return isValid;
    }

}
