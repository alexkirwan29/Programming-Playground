// using Unity;
using UnityEngine;
using PP.Draw;
using System.Collections.Generic;

namespace PP.Draw.Brushes
{
  public class Square : Brush
  {
    private int bounds;
    public Square(Color colour, float size = 1) : base(colour, size)
    {
      bounds = Mathf.FloorToInt(size);
    }

    public override void Plot(Vector2Int position, ICanvas canvas)
    {
      var data = new PixelData[bounds * bounds];

      position.x -= (bounds / 2);
      position.y -= (bounds / 2);

      for(Vector2Int pos = new Vector2Int(); pos.x < bounds; pos.x ++)
      {
        for(pos.y = 0; pos.y < bounds; pos.y ++)
        {
          data[pos.x + bounds * pos.y] = new PixelData(pos + position, Colour);
        }
      }
      canvas.SetPixels(data);
    }
  }
}