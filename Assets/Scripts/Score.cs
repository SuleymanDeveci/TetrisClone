using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score { get; set; }
    public int lines { get; set; }

    private void Start()
    {
        score = 0;
        lines = 0;
    }

    private void FixedUpdate()
    {
        Debug.Log(score);
    }
}
