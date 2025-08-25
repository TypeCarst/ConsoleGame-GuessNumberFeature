using System.Security.Cryptography;
using System.Text;

List<(int, int)> numberWithClasses = [(0, 6), (1, 2), (2, 5), (3, 5), (4, 4), (5, 5), (6, 6), (7, 3), (8, 7), (9, 6)];
List<int> solutionHashes = [686097902, 1548081509, -295317885, -170342170, -617196139];
List<string> hints =
[
    "The numbers are grouped into classes based on their visual appearance.",
    "Each class represents a specific feature of the numbers.",
    "Think straight, not curved.",
    "Think digitally."
];
int givenHintIndex = 0;

Console.OutputEncoding = Encoding.UTF8;


Console.WriteLine("┅ What is it? ┅\n");
Console.WriteLine("Goal: You have to name, what the hints are implying. As inputs the numbers 0-9 are expected.");
Console.WriteLine("You can use the commands \'classOf <number>\', \'numbersOf <class number>\', " +
                  "\'solve <string>\' \'help\', \'hint\' and \'quit\'/\'exit\'.");

while (true)
{
    Console.WriteLine("\n> Please enter a valid command.\n");
    
    string? input = Console.ReadLine();
    Console.WriteLine();

    string[] commandParts = input?.Split(' ', 2) ?? [];

    if (commandParts.Length == 0)
    {
        Console.Write("Command not valid.");
        continue;
    }

    string command = commandParts[0].Trim();
    string argument = commandParts.Length > 1 ? commandParts[1].Trim() : string.Empty;

    switch (command)
    {
        case "classOf":
            CheckClassOfNumber(argument);
            break;
        case "numbersOf":
            CheckNumbersOfClass(argument);
            break;
        case "solve":
            TrySolve(argument);
            break;
        case "help":
            ShowHelp();
            break;
        case "hint":
            ShowHint();
            break;
        case "quit":
        case "exit":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Unknown command.\n");
            ShowHelp();
            break;
    }
}

return;

void CheckClassOfNumber(string argument)
{
    if (!int.TryParse(argument, out int num))
    {
        Console.WriteLine("Invalid input. Please use a number between 0 and 9 as argument.");
        return;
    }

    int classOfNumber = GetClassOfNumber(num);
    if (classOfNumber >= 0)
    {
        Console.WriteLine("The class of the number " + argument + " is: \'" + classOfNumber + "\'.");
    }
}

int GetClassOfNumber(int number)
{
    if (number < 0 || number > 9)
    {
        Console.WriteLine("Invalid number. Please enter a number between 0 and 9.");
        return -1;
    }

    return numberWithClasses.Find(tuple => tuple.Item1 == number).Item2;
}

void CheckNumbersOfClass(string argument)
{
    if (!int.TryParse(argument, out int classNumber))
    {
        Console.WriteLine("Invalid input. Please use a number between 0 and 9 as argument.");
        return;
    }

    int[] count = GetNumbersOfClass(classNumber);
    Console.WriteLine("There are " + count.Length + " numbers in the class \'" + argument + "\'.");
    if (count.Length > 0)
    {
        Console.WriteLine("These numbers are: " + string.Join(", ", count) + ".");
    }
}

int[] GetNumbersOfClass(int numberClass)
{
    if (numberClass < 0 || numberClass > 9)
    {
        Console.WriteLine("Invalid class number. Please enter a number between 0 and 9.");
        return [];
    }

    return numberWithClasses
        .Where(tuple => tuple.Item2 == numberClass)
        .Select(tuple => tuple.Item1)
        .ToArray();
}

void TrySolve(string solution)
{
    string lowerSolution = solution.ToLowerInvariant();
    
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(lowerSolution));
        int consistentHash = BitConverter.ToInt32(hashBytes, 0);
        
        bool isCorrect = solutionHashes.Any(hash => consistentHash == hash);
        Console.WriteLine(isCorrect ? "The solution is correct!" : "The solution is incorrect. Please try again.");
    }
}

void ShowHelp()
{
    Console.WriteLine("Goal: You have to name, what the hints are implying.\n");
    Console.WriteLine("Commands:");
    Console.WriteLine("┣ classOf <number> - Get the class of a given number (0-9).");
    Console.WriteLine("┣ numbersOf <class> - Get all numbers that belong to a given class (0-9).");
    Console.WriteLine("┣ solve <solution> - Two english words.");
    Console.WriteLine("┣ help - Show this help message.");
    Console.WriteLine("┣ hint - Show a hint related to the task.");
    Console.WriteLine("┗ quit - Exit the program.");
}

void ShowHint()
{
    if (givenHintIndex < hints.Count)
    {
        Console.WriteLine(hints[givenHintIndex]);
        givenHintIndex++;
    }
    else
    {
        Console.WriteLine("No more hints available. Here are all hints:");
        for (int i = 0; i < hints.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + hints[i]);
        }
    }
}