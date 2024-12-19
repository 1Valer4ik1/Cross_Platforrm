using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

public class DissolutionTests
{
    [Fact]
    public void TestExample1()
    {
        // Arrange
        string[] input = {
            "4",
            "1 2",
            "1 2",
            "0.5 1.5",
            "7 3.5"
        };

        // Act
        var (totalTime, sheetOrder) = CalculateDissolutionTime(input);

        // Assert
        Assert.Equal(6.000, totalTime, 0.001);
        Assert.Equal(new[] { 4, 2, 1, 3 }, sheetOrder);
    }

    [Fact]
    public void TestSingleSheet()
    {
        // Arrange
        string[] input = {
            "1",
            "2 3"
        };

        // Act
        var (totalTime, sheetOrder) = CalculateDissolutionTime(input);

        // Assert
        Assert.Equal(5.000, totalTime, 0.001);
        Assert.Equal(new[] { 1 }, sheetOrder);
    }

    private (double TotalTime, int[] SheetOrder) CalculateDissolutionTime(string[] input)
    {
        int N = int.Parse(input[0]);
        var sheets = new List<Sheet>();

        for (int i = 0; i < N; i++)
        {
            var values = input[i + 1].Split(' ');
            double ai = double.Parse(values[0], CultureInfo.InvariantCulture);
            double bi = double.Parse(values[1], CultureInfo.InvariantCulture);

            sheets.Add(new Sheet(i + 1, ai, bi));
        }

        // Sorting for optimal arrangement
        sheets.Sort((x, y) => Math.Min(y.A, y.B).CompareTo(Math.Min(x.A, x.B)));

        // Calculating dissolution time
        double totalTime = 0;
        double dissolutionTimeA = 0;
        var sheetOrder = new List<int>();

        foreach (var sheet in sheets)
        {
            dissolutionTimeA += sheet.A;
            totalTime = Math.Max(totalTime, dissolutionTimeA) + sheet.B;
            sheetOrder.Add(sheet.Index);
        }

        return (totalTime, sheetOrder.ToArray());
    }

    private class Sheet
    {
        public int Index { get; }
        public double A { get; }
        public double B { get; }

        public Sheet(int index, double a, double b)
        {
            Index = index;
            A = a;
            B = b;
        }
    }
}
