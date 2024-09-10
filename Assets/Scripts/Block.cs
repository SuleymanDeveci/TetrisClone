using System;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float stepDelay = 1f;
    [SerializeField] private float lockDelay = 0.5f;

    public GameBoard Board {  get; private set; }
    public Vector3Int Position { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public TetrominoData TData { get; private set; }
    public int RotationIndex {  get; private set; }

    private float stepTime;
    private float lockTime;

    public void Initialize(GameBoard board, Vector3Int position, TetrominoData tData)
    {
        Board = board;
        Position = position;
        TData = tData;
        RotationIndex = 0;

        stepTime = Time.time + stepDelay;
        lockTime = 0f;

       /*  --------------------Bir alt satýrdaki kodun uzun hali
        if(Cells == null)
        {
            Cells = new Vector3Int[tData.Cells.Length];
        }
       */
        Cells ??= new Vector3Int[tData.Cells.Length];

        for (int i = 0; i < tData.Cells.Length; ++i)
        {
            Cells[i] = (Vector3Int)tData.Cells[i]; //Type Casting (Vector2Int türünü Vector3Int'e çevirdik)
        }
    }

    private void Update()
    {
        if(Board.isGameOver == true)
        {
            return;
        }
        Board.Clear(this);

        lockTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            HandleRotation(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleRotation(+1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            HandleMovement(Vector2Int.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        { 
            HandleMovement(Vector2Int.right);
        }

        if(Input.GetKeyDown(KeyCode.S)) 
        { 
            HandleMovement(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HandleHardDrop();
        }

        if(Time.time >= stepTime)
        {
            Step();
        }

        Board.Set(this);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        HandleMovement(Vector2Int.down);

        if(lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        score.score += 10;
        scoreText.text = score.score.ToString();

        soundManager.PlaySound("lockSound");

        Board.Set(this);
        Board.ClearLines();
        Board.SpawnPiece();
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
            lockTime = 0f;
        }

        return isValid;
    }

    public void HandleRotation(int direction)
    {
        int originalRotation = RotationIndex;
        RotationIndex = ExtensionMethods.Wrap(RotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
        soundManager.PlaySound("rotateSound");

        if(!TestWallkicks(RotationIndex,direction))
        {
            RotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private bool TestWallkicks(int rotationIndex, int rotationDirection)
    {
        int wallkickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < TData.WallKicks.GetLength(1); ++i)
        {
            Vector2Int translation = TData.WallKicks[wallkickIndex, i];

            if (HandleMovement(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallkickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallkickIndex--;
        }
        return ExtensionMethods.Wrap(wallkickIndex, 0, TData.WallKicks.GetLength(0));
    }

    private void ApplyRotationMatrix(int direction)
    {
        for(int i = 0; i< Cells.Length; ++i) 
        {
            Vector3 cell = Cells[i];

            int x, y;

            switch (TData.tetromino)
            {
                case Tetromino.Letter_I:
                case Tetromino.Letter_O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private void HandleHardDrop()
    {
        while(HandleMovement(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

}
