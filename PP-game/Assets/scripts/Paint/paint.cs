//base code by technokid

using System.Collections.Generic;
using System;
using UnityEngine;

namespace PP.Draw
{

  public class PixelData {
      
      [Obsolete("Use Vector2Int Position instead.")]
      public int x => Position.x;

      [Obsolete("Use Vector2Int Position instead.")]
      public int y => Position.y;

      public Vector2Int Position;

      public PixelData(Vector2Int position, Color colour)
      {
        this.Position = position;
        this.color = colour;
      }

      public Color color;
  }

  public class paint : MonoBehaviour, ICanvas{
      public Renderer PaintingCanvas;
      public string ImageDirectory;
      public int autosaveDuration;

      private Brush brush;
      private GameObject vector3Pointer;
      private double autosaveTimer;

      private Vector2Int lastPosition;

      void Start() {
          //Load the default image to start with and start the autosave timer
          LoadImage();
          autosaveTimer = Time.time;

          vector3Pointer = new GameObject();
          vector3Pointer.transform.parent = PaintingCanvas.transform;
          vector3Pointer.transform.localRotation = Quaternion.Euler(0, 0, 0);

          brush = new Brushes.Circle(Color.red, 10f,0.2f);
          // brush = new Brushes.Square(Color.red, 5f);

      }

      void Update()
      {
        //If we are clicking our mouse, check if we are hitting the canvas
        if (Input.GetMouseButton(0))
        {
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit = new RaycastHit();

          if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.name == "Painting Canvas"))
          {
            //We have hit the canvas. So, we want to get the co-ordinates of the pixel we hit, then paint there
            var hitPos = Vector3ToPixel(hit.point);
            Vector2Int hitPosition = new Vector2Int((int)hitPos.x, (int)hitPos.y);

            if(lastPosition == hitPosition)
            {
              lastPosition = hitPosition;
              return;
            }

            if(Input.GetMouseButtonDown(0))
              lastPosition = hitPosition;

            // brush.Plot(hitPosition, this);
            PlotLineSegment(lastPosition, hitPosition, brush);

            lastPosition = hitPosition;
          }

        }

        //Every x seconds (autosaveDuration), reset the autosave timer and save the image
        if (Time.time - autosaveTimer > autosaveDuration) {
          autosaveTimer = Time.time;
          SaveImage();
        }
      }

      void PlotLineSegment(Vector2Int start, Vector2Int end, Brush brush)
      {
        // If the line started and stopped at the same place, draw once.
        if(start == end)
        {
          brush.Plot(start,this);
          return;
        }
        // Get the diagonal distance between the two points.
        float diag;
        {
          var dx = end.x - start.x;
          var dy = end.y - start.y;

          // Find the largest of the two diagonals.
          diag = Mathf.Max(Math.Abs(dx), Math.Abs(dy));
        }

        for (int i = 0; i <= diag; i++)
        {
            var t = i / diag;

            Vector2 lerpPos = Vector2.Lerp(start, end, t);
            Vector2Int pixelPos = Vector2Int.FloorToInt(lerpPos);

            brush.Plot(pixelPos, this);
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

          //print("Autosaved!");
      }

      void SetPixel(PixelData data) { SetPixels(new List<PixelData> { data }); }
      public void SetPixels(IEnumerable<PixelData> data, bool alpha = false) {
          //Get a copy of the texture on the canvas
          Texture2D tex2D = (Texture2D)PaintingCanvas.material.mainTexture;
          tex2D.filterMode = FilterMode.Point;

          var boundary = new RectInt(0,0, tex2D.width, tex2D.height);

          //For each pixel data variable, set the pixel on the texture
          foreach (var pixel in data)
          {
            if(pixel != null && boundary.Contains(pixel.Position))
              if(!alpha)
                tex2D.SetPixel(pixel.Position.x, pixel.Position.y, pixel.color);
              else
              {
                var baseColour = tex2D.GetPixel(pixel.Position.x, pixel.Position.y);
                baseColour = Color.Lerp(baseColour, pixel.color, pixel.color.a);
                tex2D.SetPixel(pixel.Position.x, pixel.Position.y, baseColour);
              }
          }

          //Save the texture data and apply it back to the canvas
          tex2D.Apply();
          PaintingCanvas.material.mainTexture = tex2D;
      }
      Vector2 Vector3ToPixel(Vector3 input) {
          //Could not quite figure out the maths required for localised vector3 rotation
          //The alternative solution was to use a pointer object (vector3Pointer) and parent it to the canvas (Check start method)
          //Step 1, set the vector3Pointer position to the input Vector3
          vector3Pointer.transform.position = input;

          //Step 2, now that the position is correct, we pump out the local position and can ignore the Vector3.y variable
          Vector3 localPos = vector3Pointer.transform.localPosition;
          Texture canvasTexture = PaintingCanvas.material.mainTexture;

          //Now we simply have to take the Vector2 (Vector3 simply ignoring the y axis) and calculate where on the canvas we actually clicked
          //Then, it's a simple matter using rounding and dividing by the canvas width to get the pixel that you actually clicked on
          float width = Mathf.Round(localPos.x * canvasTexture.width / 5 / 2 - 0.5f) + canvasTexture.width / 2;
          float height = Mathf.Round(localPos.z * canvasTexture.height / 5 / 2 - 0.5f) + canvasTexture.height / 2;
          
          return new Vector2(128 - width, 128 - height);
      }
  }
}