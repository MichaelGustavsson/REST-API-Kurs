namespace tuples;

public class MethodsDemo
{
  public void MethodOne(string name, int age)
  {
    // Console.WriteLine($"Mitt namn är {name}, är {age} gammal");
  }

  public string MethodTwo(string name, int age)
  {
    return $"Mitt namn är {name}, är {age} gammal";
  }

  // Returnera en typ som består av namn och ålder...
  public record ReturnData
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }
  public ReturnData MethodThree(string name, int age)
  {
    var result = new ReturnData { Name = name, Age = age };
    return result;
  }

  public List<ReturnData> MethodFour()
  {
    var data = new List<ReturnData>
    {
        new() { Name = "Kalle", Age = 20 },
        new() { Name = "Eva", Age = 25 },
        new() { Name = "Oscar", Age = 22 },
        new() { Name = "Bertil", Age = 65 }
    };

    return data;
  }

  public string MethodFive(string name, ref int age)
  {
    age = 20;
    // Console.WriteLine($"Mitt namn är {name}, är {age++} gammal");
    return name;
  }
  public string MethodSix(string name, out int age)
  {
    age = 30;
    // Console.WriteLine($"Mitt namn är {name}, är {age++} gammal");
    return name;
  }

  // TUPLES...
  public (string, int) MethodSeven()
  {
    return ("Nisse", 35);
  }

  public (string Name, int Age) MethodEight()
  {
    return (Name: "Nisse", Age: 35);
  }
}