using System;
using System.IO;
using Xunit;

public class ProgramTests
{
    [Fact]
    public void Test_MinimumRadiationPath_SimpleCase()
    {
        // Arrange
        File.WriteAllText("INPUT.TXT", "3 3\n1 2 3\n4 1 5\n6 7 1");

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.Equal("8", output);  // Очікуємо мінімальну суму радіації
    }

    [Fact]
    public void Test_MinimumRadiationPath_SingleRow()
    {
        // Arrange
        File.WriteAllText("INPUT.TXT", "1 4\n2 3 1 5");

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.Equal("11", output);  // Мінімальний шлях тут: 2 -> 3 -> 1 -> 5
    }

    [Fact]
    public void Test_MinimumRadiationPath_SingleColumn()
    {
        // Arrange
        File.WriteAllText("INPUT.TXT", "4 1\n1\n2\n3\n4");

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.Equal("10", output);  // Мінімальний шлях: 1 -> 2 -> 3 -> 4
    }

    [Fact]
    public void Test_SmallGrid()
    {
        // Arrange
        File.WriteAllText("INPUT.TXT", "2 2\n10 1\n1 5");

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.Equal("6", output);  // Мінімальний шлях: 10 -> 1 -> 5
    }

    [Fact]
    public void Test_LargeGrid()
    {
        // Arrange
        string input = "30 30\n";
        var rand = new Random();
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                input += rand.Next(1, 101) + " ";
            }
            input = input.Trim() + "\n";
        }
        File.WriteAllText("INPUT.TXT", input);

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.NotEmpty(output);  // Перевіряємо, що файл OUTPUT.TXT не порожній
    }

    [Fact]
    public void Test_InvalidDimensions()
    {
        // Arrange
        File.WriteAllText("INPUT.TXT", "31 30\n10 20 30\n...");

        // Act
        Program.Main(Array.Empty<string>());

        // Assert
        string output = File.ReadAllText("OUTPUT.TXT");
        Assert.Contains("Помилка: розміри карти повинні бути між 1 та 30.", output); // Очікуємо помилку
    }
}
