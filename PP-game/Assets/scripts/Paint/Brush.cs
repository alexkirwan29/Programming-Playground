// using Unity;
using UnityEngine;

namespace PP.Draw
{
  public abstract class Brush
  {
    public Color Colour;
    public float Size;

    public Brush(Color colour, float size = 1)
    {
      this.Colour = colour;
      this.Size = size;
    }

    public abstract void Plot(Vector2Int position, ICanvas canvas);
  }
}