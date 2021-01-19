using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridScript : MonoBehaviour

    

{
    public Sprite sprite;
    public float[,] Grid;
    int Vertical, Horizontal, Columns, Rows;
    // Start is called before the first frame update

    void makeGrid(float[,] Grid)
    {

        int i;
        int j;

        for (i = 0; i < Columns; i++)
        {
            for (j = 0; j < Rows; j++)
            {
                Grid[i, j] = Random.Range(0.0f, 1.0f);
                createTiles(i, j, Grid[i, j]);
            }


        }

    }
    void Start()
    {
        Vertical = (int)Camera.main.orthographicSize;
        Horizontal = Vertical * (Screen.width / Screen.height);
        Columns = Horizontal * 2;
        Rows = Vertical * 2;
        Grid = new float[Columns, Rows];
        makeGrid(Grid);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createTiles(int x, int y, float value)
    {
        GameObject g = new GameObject("X: " + x + "Y: " + y);
        g.transform.position = new Vector3(x - (Horizontal - 0.5f), y - (Vertical - 0.5f));
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = new Color(value, value, value);

    }
}
