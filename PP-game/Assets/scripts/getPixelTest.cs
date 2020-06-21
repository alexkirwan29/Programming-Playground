using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getPixelTest : MonoBehaviour
{
    public Texture2D texture;


    // Start is called before the first frame update
    void Start()
    {

        //   GetComponent<Renderer>().material.mainTexture = texture;

        Instantiate(texture, new Vector3(0, 0, 0), Quaternion.identity);
        
        for (int y = 0; y < texture.height; y++)
        {

            for (int x = 0; x < texture.width; x++)
            {
                Color color = ((x & y) != 0 ? Color.white : Color.gray);
                texture.SetPixel(x, y, color);
            }
            texture.Apply();
        }
                      

    }

    // Update is called once per frame
    void Update()
    {
    
        


    }
}
