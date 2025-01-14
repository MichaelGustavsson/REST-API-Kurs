namespace tuples;

class Program
{
    static void Main(string[] args)
    {
        var methodsDemo = new MethodsDemo();

        // Metod 1:
        methodsDemo.MethodOne("Kalle", 20);

        // Metod 2:
        // Console.WriteLine(methodsDemo.MethodTwo("Kalle", 20));

        // Metod 3:
        var result = methodsDemo.MethodThree("Kalle", 20);
        // Console.WriteLine($"Namn: {result.Name} Ålder: {result.Age}");

        // Metod 4:
        var list = methodsDemo.MethodFour();
        // Console.WriteLine(list);

        // Metod 5:
        int age = 30;
        var returnedName = methodsDemo.MethodFive("Kalle", ref age);
        // Console.WriteLine("Ny ålder " + age + " Name " + returnedName);

        // Metod 6:
        var outName = methodsDemo.MethodSix("Kalle", out int inAge);
        // Console.WriteLine("Ny ålder " + inAge + " Name " + outName);

        // Här kommer tuples exempel...
        // Metod 7:
        (string, int) response = methodsDemo.MethodSeven();
        Console.WriteLine(response);
        Console.WriteLine(response.Item1);
        Console.WriteLine(response.Item2);

        // Metod 8:
        var (Name, Age) = methodsDemo.MethodEight();
        Console.WriteLine(Name);
        Console.WriteLine(Age);

        (string givenName, int personAge) = methodsDemo.MethodEight();
        Console.WriteLine(givenName);
        Console.WriteLine(personAge);




    }


}
