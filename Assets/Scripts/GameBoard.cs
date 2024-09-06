using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private TetrominoData[] tetrominos;

    public Block ActiveBlock {  get; private set; }
    public Tilemap Tilemap { get; private set; }

    private void Awake()
    {
        ActiveBlock = GetComponentInChildren<Block>();
        Tilemap = GetComponentInChildren<Tilemap>();

        for(int i = 0; i < tetrominos.Length; ++i)
        {
            tetrominos[i].Initialize();
        }
    }
}
