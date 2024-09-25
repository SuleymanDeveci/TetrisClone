using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lineText;
    [SerializeField] private GameObject gameoOverPanel;
    [SerializeField] private TetrominoData[] tetrominos;
    [SerializeField] private Vector3Int spawnPosition;
    

    public Block ActiveBlock {  get; private set; }
    public Tilemap Tilemap { get; private set; }

    public Vector2Int boardSize = new Vector2Int(10, 20);

    public bool isGameOver {  get; private set; }

    public RectInt Bounds // Unity'nin bize saðlamýþ olduðu sýnýr oluþturma kodu diyebiliriz
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

        isGameOver = false;
    }

    private void Start()
    {
        SpawnPiece();
    }
    public void SpawnPiece()
    {
         int random = UnityEngine.Random.Range(0, tetrominos.Length); // baþta unity engine yazmasýnýn sebebi
        // eðer ilerde System kütüphanesini dahil edersek, Random fonksiyonu hem System hemde UnityEngine de
        // olduðu için hata verir ve hangisindeki Random fonksiyonunu kullanmak istiyorsun onu belir diye kýzar
        // o yüzden þimdiden belirtiyoruz ki ilerde baþýmýz aðrýmasýn

        TetrominoData data = tetrominos[random];

        ActiveBlock.Initialize(this, spawnPosition, data);

        if(IsValidPosition(ActiveBlock, spawnPosition))
        {
            Set(ActiveBlock);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        soundManager.PlaySound("gameOverSound");
        isGameOver = true;
        gameoOverPanel.SetActive(true);

        Time.timeScale = 0;
        
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

            if (!bounds.Contains((Vector2Int)tilePosition)) // burda yukarda tanýmladýðýmýz sýnýr dýþýnýn geçersiz olduðunu söylüyor
            {
                return false;
            }
            if (Tilemap.HasTile(tilePosition)) // burada tilemapte dolu olan hücrelerin geçersiz olduðunu söylüyor
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines() // Line doldumu kontol edip, dolduysa alttaki clearLine fonksiyonunu kullanarak temizleyen fonksiyon
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if (IslineFull(row))
            {
                ClearLine(row);
                score.lines += 1;
                score.score += 100;
                scoreText.text = score.score.ToString();
                lineText.text = score.lines.ToString();

                soundManager.PlaySound("lineClearSound");
            }
            else
            {
                row++;
            }
            
        }
    }

    private bool IslineFull(int row)
    {
        RectInt bound = Bounds;

        for(int col = Bounds.xMin; col < bound.xMax; ++col)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!Tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    private void ClearLine(int row) // Lineyi temizleyen fonksiyon
    {
        RectInt bound = Bounds;

        for (int col = Bounds.xMin; col < bound.xMax; ++col)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            Tilemap.SetTile(position, null);
        }

        while(row < Bounds.yMax)
        {
            for(int col = Bounds.xMin; col < Bounds.xMax; ++col)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase aboveTile = Tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                Tilemap.SetTile(position,aboveTile);
            }

            row++;
        }
    }
}
