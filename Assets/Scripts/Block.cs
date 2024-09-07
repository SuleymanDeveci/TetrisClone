using UnityEngine;

public class Block : MonoBehaviour
{
    public GameBoard Board {  get; private set; }
    public Vector3Int Position { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public TetrominoData TData { get; private set; }
    public int RotationIndex {  get; private set; }

    public void Initialize(GameBoard board, Vector3Int position, TetrominoData tData)
    {
        Board = board;
        Position = position;
        TData = tData;
        RotationIndex = 0;

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
        Board.Clear(this);

        if(Input.GetKeyDown(KeyCode.Q))
        {
            HandleRotation(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleRotation(1);
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

    public void HandleRotation(int direction)
    {
        int originalRotation = RotationIndex;
        RotationIndex = ExtensionMethods.Wrap(RotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
    }

    private void ApplyRotationMatrix(int direction)
    {
        for(int i = 0; i< Cells.Length; ++i) 
        {
            Vector3Int cell = Cells[i];

            int x, y;

            switch (TData.tetromino)
            {
                case Tetromino.Letter_I:
                case Tetromino.Letter_O:
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
    }

}
