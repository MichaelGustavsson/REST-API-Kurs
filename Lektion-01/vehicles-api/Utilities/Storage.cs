using System.Text.Json;
using vehicles_api.Models;

namespace vehicles_api.Utilities;

public class Storage<T>
{
  public static List<T> ReadJson(string path)
  {
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var json = File.ReadAllText(path);
    var result = JsonSerializer.Deserialize<List<T>>(json, options);
    return result;
  }
}
