using PP.API;
using Newtonsoft.Json;

namespace PP.API.Objects
{
  public class Error : APIObject
  {
    public short code { get; set; }    
    public string message { get; set; }

    public long HttpCode;
    public string url;

    public Error(string url, long httpCode, string message, short apiError = -1)
    {
      this.code = apiError;
      this.HttpCode = httpCode;
      this.message = message;
    }

    public override string ToString()
    {
      return $"[ {HttpCode} URL:{url} ] {message} [ {code:X} ]";
    }
  }
}