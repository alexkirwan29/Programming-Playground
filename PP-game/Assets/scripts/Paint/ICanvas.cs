using System.Collections.Generic;

namespace PP.Draw
{
  public interface ICanvas
  {
      void SetPixels(IEnumerable<PixelData> data, bool alphaBlend = false);
  }
}