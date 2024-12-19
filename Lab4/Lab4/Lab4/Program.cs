using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.CommandLine;

class Program
{
    static void Main(string[] args)
    {
        // Коренева команда
        var rootCommand = new RootCommand("Інструмент для запуску лабораторних робіт")
        {
            new Command("version", "Показує інформацію про програму")
            {
                Handler = System.CommandLine.Invocation.CommandHandler.Create(() =>
                {
                    Console.WriteLine("Автор: Ім'я автора");
                    Console.WriteLine("Версія: 1.0.0");
                })
            },
            new Command("run", "Запускає вказану лабораторну роботу")
            {
                new Argument<string>("lab", "Назва лабораторної роботи (lab1, lab2, lab3)"),
                new Option<string>(new[] { "-I", "--input" }, "Шлях до вхідного файлу"),
                new Option<string>(new[] { "-o", "--output" }, "Шлях до вихідного файлу")
            }
            .WithHandler((string lab, string input, string output) =>
            {
                RunLab(lab, input, output);
            }),
            new Command("set-path", "Задає шлях до папки")
            {
                new Option<string>(new[] { "-p", "--path" }, "Шлях до папки") { IsRequired = true }
            }
            .WithHandler((string path) =>
            {
                Environment.SetEnvironmentVariable("LAB_PATH", path, EnvironmentVariableTarget.User);
                Console.WriteLine($"Шлях встановлено: {path}");
            })
        };

        rootCommand.Invoke(args);
    }

    static void RunLab(string lab, string inputFile, string outputFile)
    {
        string inputPath = inputFile ?? GetDefaultPath("INPUT.txt");
        string outputPath = outputFile ?? GetDefaultPath("OUTPUT.txt");

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Помилка: вхідний файл не знайдено.");
            return;
        }

        switch (lab.ToLower())
        {
            case "lab1":
                ExecuteLab1(inputPath, outputPath);
                break;
            case "lab2":
            case "lab3":
                Console.WriteLine($"Лабораторна {lab} ще не реалізована.");
                break;
            default:
                Console.WriteLine("Невідома лабораторна робота.");
                break;
        }
    }

    static void ExecuteLab1(string input, string output)
    {
        var lines = File.ReadAllLines(input);
        int N = int.Parse(lines[0]);
        var sheets = new List<Sheet>();

        for (int i = 0; i < N; i++)
        {
            var values = lines[i + 1].Split(' ');
            double ai = double.Parse(values[0], CultureInfo.InvariantCulture);
            double bi = double.Parse(values[1], CultureInfo.InvariantCulture);
            sheets.Add(new Sheet(i + 1, ai, bi));
        }

        sheets.Sort((x, y) => Math.Min(y.A, y.B).CompareTo(Math.Min(x.A, x.B)));

        double totalTime = 0, dissolutionTimeA = 0;
        foreach (var sheet in sheets)
        {
            dissolutionTimeA += sheet.A;
            totalTime = Math.Max(totalTime, dissolutionTimeA) + sheet.B;
        }

        using (var writer = new StreamWriter(output))
        {
            writer.WriteLine(totalTime.ToString("F3"));
            writer.WriteLine(string.Join(" ", sheets.Select(s => s.Index)));
        }
        Console.WriteLine($"Результат записано у файл {output}");
    }

    static string GetDefaultPath(string fileName)
    {
        string labPath = Environment.GetEnvironmentVariable("LAB_PATH");
        if (!string.IsNullOrEmpty(labPath) && File.Exists(Path.Combine(labPath, fileName)))
        {
            return Path.Combine(labPath, fileName);
        }

        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (File.Exists(Path.Combine(homePath, fileName)))
        {
            return Path.Combine(homePath, fileName);
        }

        Console.WriteLine($"Помилка: файл {fileName} не знайдено.");
        return null;
    }

    class Sheet
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