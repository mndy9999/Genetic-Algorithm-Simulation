using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChange : MonoBehaviour {

    public Texture2D tex;
    Color[] colors;

    // Update is called once per frame
    void Update () {

        colors = new Color[50*50];

        for(int i = 0; i < 50*50; i++)
        {
            colors[i] = Color.green;
        }

        tex.SetPixels(1, 1, 50, 50, colors);

        tex.Apply();
        
        //mat.color = Color.black;
	}
}
