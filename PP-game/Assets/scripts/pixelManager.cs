using System.Collections.Generic;
using UnityEngine;

public class pixelManager : MonoBehaviour {
    [HideInInspector]
    public List<List<Renderer>> pixels = new List<List<Renderer>>();

    void Start() {
        spawnAllPixels(128, 128);
        loadExternalImage("Assets/testImage.png");
        saveExternalImage("Assets/testImageSaved.png");
    }

    void spawnAllPixels(int canvasWidth, int canvasHeight) {
        GameObject parent = new GameObject();
        parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        parent.transform.position = new Vector3(0, 0, 0);

        for (int y = 0; y < canvasWidth; y++) {
            List<Renderer> tempList = new List<Renderer>();
            for (int x = 0; x < canvasHeight; x++) {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Plane);

                temp.transform.SetParent(parent.transform);
                //temp.transform.localRotation = Quaternion.Euler(0, 0, 0);
                temp.transform.localPosition = new Vector3(x, y, 0);

                Renderer ren = temp.GetComponent<Renderer>();
                ren.material = new Material(Shader.Find("Standard"));

                tempList.Add(ren);
            }
            pixels.Add(tempList);
        }
    }

    void loadExternalImage(string dir) {
        if (!System.IO.File.Exists(dir)) { throw new System.ArgumentException("Image Does Not Exist!!!"); }

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(System.IO.File.ReadAllBytes(dir));
        for (int y = 0; y < tex.height; y++) {
            for (int x = 0; x < tex.width; x++) {
                pixels[y][x].material.color = tex.GetPixel(x, y);
            }
        }
    }

    void saveExternalImage(string dir) {
        Texture2D tex = new Texture2D(pixels[0].Count, pixels.Count);
        for (int y = 0; y < pixels.Count; y++) {
            for (int x = 0; x < pixels[0].Count; x++) {
                tex.SetPixel(x,y, pixels[y][x].material.color);
            }
        }

        tex.Apply();
        System.IO.File.WriteAllBytes(dir,tex.EncodeToPNG());
    }
}
