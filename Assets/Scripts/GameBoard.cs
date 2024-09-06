using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private TetrominoData[] tetrominos;
    [SerializeField] private Vector3Int spawnPosition;

    public Block ActiveBlock {  get; private set; }
    public Tilemap Tilemap { get; private set; }

    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds // Unity'nin bize sa�lam�� oldu�u s�n�r olu�turma kodu diyebiliriz
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x /2, -boardSize.y /2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        ActiveBlock = GetComponentInChildren<Block>();

        for (int i = 0; i < tetrominos.Length; ++i)
        {
            tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }
    public void SpawnPiece()
    {
         int random = UnityEngine.Random.Range(0, tetrominos.Length); // ba�ta unity engine yazmas�n�n sebebi
        // e�er ilerde System k�t�phanesini dahil edersek, Random fonksiyonu hem System hemde UnityEngine de
        // oldu�u i�in hata verir ve hangisindeki Random fonksiyonunu kullanmak istiyorsun onu belir diye k�zar
        // o y�zden �imdiden belirtiyoruz ki ilerde ba��m�z a�r�mas�n

        TetrominoData data = tetrominos[random];

        ActiveBlock.Initialize(this, spawnPosition, data);
        Set(ActiveBlock);
    }

    public void Set(Block block)
    {
        for(int i = 0; i < block.Cells.Length; ++i)
        {
            Vector3Int tilePosition = block.Cells[i] + block.Position;
            Tilemap.SetTile(tilePosition, block.TData.tile);
        }
    }

    public void Clear(Block block)
    {
        for (int i = 0; i < block.Cells.Length; ++i)
        {
            Vector3Int tilePosition = block.Cells[i] + block.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Block block, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for(int i = 0; i < block.Cells.Length; ++i)
        {
            Vector3Int tilePosition = block.Cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) // burda yukarda tan�mlad���m�z s�n�r d���n�n ge�ersiz oldu�unu s�yl�yor
            {
                return false;
            }
            if (Tilemap.HasTile(tilePosition)) // burada tilemapte dolu olan h�crelerin ge�ersiz oldu�unu s�yl�yor
            {
                return false;
            }
        }
        return true;
    }
}
