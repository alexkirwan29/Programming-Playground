using System.Collections.Generic;
using UnityEngine;

public struct Brush {
    public Color brushColour;
    public int brushSize;
}

public struct PixelData {
    public int x;
    public int y;
    public Color color;
}

public class paint : MonoBehaviour {
    public Renderer PaintingCanvas;
    public string ImageDirectory;
    public int autosaveDuration;

    private Brush brush;
    private GameObject vector3Pointer;
    private double autosaveTimer;

    void Start() {
        //Load the default image to start with and start the autosave timer
        LoadImage();
        autosaveTimer = Time.time;

        vector3Pointer = new GameObject();
        vector3Pointer.transform.parent = PaintingCanvas.transform;
        vector3Pointer.transform.localRotation = Quaternion.Euler(0, 0, 0);

        brush = new Brush { brushColour = Color.black, brushSize = 8 };
        //PaintCircleAtPosition(64,64);

        //brush = new Brush { brushColour = Color.green, brushSize = 4 };
        //PaintCircleAtPosition(32,32);

        //brush = new Brush { brushColour = Color.red, brushSize = 2 };
        //PaintSquareAtPosition(32,32);
    }

    void Update() {
        //Display the FPS
        print(1/Time.deltaTime);

        //If we are clicking our mouse, check if we are hitting the canvas
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.name == "Painting Canvas")) {
                //We have hit the canvas. So, we want to get the co-ordinates of the pixel we hit, then paint there
                Vector2 hitPosition = Vector3ToPixel(hit.point);
                PaintCircleAtPosition((int)hitPosition.x, (int)hitPosition.y);
            }
        }

        //Every x seconds (autosaveDuration), reset the autosave timer and save the image
        if (Time.time - autosaveTimer > autosaveDuration) {
            autosaveTimer = Time.time;
            SaveImage();
        }
    }

    void LoadImage() {
        //If the file doesn't exist, we cannot load it!
        if (!System.IO.File.Exists(ImageDirectory)) {
            throw new System.ArgumentException("Image Directory Does Not Exist!!!");
        }

        //Make a texture ready to put onto the canvas plane, then load it with the image data
        Texture2D tex2D = new Texture2D(2, 2);
        tex2D.LoadImage(System.IO.File.ReadAllBytes(ImageDirectory));

        //tex2D now contains a completely valid texture form of the file
        //Simply apply the texture to the plane and you are done
        PaintingCanvas.material.mainTexture = tex2D;
        PaintingCanvas.transform.localScale = new Vector3(tex2D.width / 128, tex2D.height / 128, 1);
    }

    void SaveImage() {
        //Take the canvas plane texture and save it to a file
        Texture2D tex2D = (Texture2D)PaintingCanvas.material.mainTexture;
        System.IO.File.WriteAllBytes("Assets/graphics/Save Location.jpg", tex2D.EncodeToJPG());
        print("Autosaved!");
    }

    //Just multiple variations of how you can set a pixel. All are equally valid and each can be handy at certain times
    void SetPixel(int x, int y, Color col) { SetPixel(new PixelData { x = x, y = y, color = col }); }
    void SetPixel(Vector2 input, Color col) { SetPixel(new PixelData { x = (int)input.x, y = (int)input.y, color = col }); }
    void SetPixel(PixelData data) { SetPixels(new List<PixelData> { data }); }
    void SetPixels(List<PixelData> data) {
        //Get a copy of the texture on the canvas
        Texture2D tex2D = (Texture2D)PaintingCanvas.material.mainTexture;

        //For each pixel data variable, set the pixel on the texture
        for (int i = 0; i < data.Count; i++) {
            if (!((data[i].x < 0) || (data[i].y < 0) || (data[i].x > tex2D.width - 1) || (data[i].y > tex2D.height - 1)))
                tex2D.SetPixel(data[i].x, data[i].y, data[i].color);
        }

        //Save the texture data and apply it back to the canvas
        tex2D.Apply();
        PaintingCanvas.material.mainTexture = tex2D;
    }

    void EfficiencyTest() {
        List<PixelData> temp = new List<PixelData>();
        for (int i = 0; i < 100000; i++) {
            PixelData data = new PixelData {
                x = Random.Range(0, 127),
                y = Random.Range(0, 127),
                color = Random.ColorHSV()
            };
            temp.Add(data);
        }

        SetPixels(temp);
    }

    void PaintSquareAtPosition(int x_in, int y_in) {
        List<PixelData> temp = new List<PixelData>();
        for (int x = x_in - brush.brushSize; x < x_in + brush.brushSize; x++) {
            for (int y = y_in - brush.brushSize; y < y_in + brush.brushSize; y++) {
                //For each x and y position, add a pixelData object to the "temp" list
                PixelData data = new PixelData {
                    x = x,
                    y = y,
                    color = brush.brushColour
                };

                temp.Add(data);
            }
        }

        SetPixels(temp);
    }

    void PaintCircleAtPosition(int x, int y) {
        List<PixelData> temp = new List<PixelData>();
        for (float i = 0; i < 2 * Mathf.PI; i += 0.01f) {
            for (float length = 0; length < brush.brushSize; length += 1f) {
                PixelData data = new PixelData {
                    x = x + (int)(length * Mathf.Sin(i)),
                    y = y + (int)(length * Mathf.Cos(i)),
                    color = brush.brushColour
                };
                temp.Add(data);
            }
        }

        SetPixels(temp);
    }

    Vector2 Vector3ToPixel(Vector3 input) {
        vector3Pointer.transform.position = input;

        Vector3 localPos = vector3Pointer.transform.localPosition;
        Texture canvasTexture = PaintingCanvas.material.mainTexture;

        float width = Mathf.Round(localPos.x * canvasTexture.width / 5 / 2 - 0.5f) + canvasTexture.width / 2;
        float height = Mathf.Round(localPos.z * canvasTexture.height / 5 / 2 - 0.5f) + canvasTexture.height / 2;
        
        return new Vector2(128 - width, 128 - height);
    }
}
