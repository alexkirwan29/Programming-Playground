// using Unity;
using UnityEngine;
using PP.Draw;
using System.Collections.Generic;

namespace PP.Draw.Brushes
{
  public class Circle : Brush
  {
    private int bounds;

    public float Hardness;
    public Circle(Color colour, float size = 1, float hardness = 1) : base(colour, size)
    {
      bounds = Mathf.FloorToInt(size);
      this.Hardness = hardness;
    }

    public override void Plot(Vector2Int position, ICanvas canvas)
    {
      var data = new PixelData[bounds * bounds];

      var halfSize = new Vector2Int(bounds / 2, bounds / 2);
      position -= halfSize;

      Vector2Int centred, pos = new Vector2Int();

      for(pos.x = 0; pos.x < bounds; pos.x ++)
      {
        for(pos.y = 0; pos.y < bounds; pos.y ++)
        {
          centred = pos - halfSize;
          if(centred.magnitude < Size * .5f)
          {
            var newColour = Colour;
            if(Hardness < 1f)
              newColour.a = Mathf.SmoothStep(1f,0f, centred.magnitude / (Size * 0.5f)) * Hardness;
              
            data[pos.x + bounds * pos.y] = new PixelData(pos + position, newColour);
          }
        }
      }
      canvas.SetPixels(data, true);
    }
  }
}