using System;

public class UnitTest2
{
    // Функція перевірки, чи є в візерунку неприпустимий квадрат 2x2
    public static bool IsSymmetric(int M, int N, int[,] pattern)
    {
        for (int i = 0; i < M - 1; i++)
        {
            for (int j = 0; j < N - 1; j++)
            {
                // Перевіряємо квадрат 2x2
                if (pattern[i, j] == pattern[i, j + 1] &&
                    pattern[i, j] == pattern[i + 1, j] &&
                    pattern[i, j] == pattern[i + 1, j + 1])
                {
                    return false; // Якщо квадрат 2x2 однотонний, візерунок не симпатичний
                }
            }
        }
        return true; // Якщо неприпустимих квадратів немає
    }

    // Функція для обчислення всіх візерунків
    public static int CountSymmetricPatterns(int M, int N)
    {
        int totalPatterns = 0;
        int maxPattern = (int)Math.Pow(2, M * N); // Кількість всіх можливих варіантів плиток

        // Перебираємо всі можливі варіанти плиток (0 - чорна плитка, 1 - біла плитка)
        for (int i = 0; i < maxPattern; i++)
        {
            int[,] pattern = new int[M, N];

            // Заповнюємо плитки для поточного варіанту
            for (int j = 0; j < M * N; j++)
            {
                pattern[j / N, j % N] = (i >> j) & 1; // Генеруємо плитки за допомогою біта
            }

            // Перевіряємо, чи є неприпустимий квадрат 2x2
            if (IsSymmetric(M, N, pattern))
            {
                totalPatterns++; // Якщо візерунок симпатичний, збільшуємо лічильник
            }
        }

        return totalPatterns; // Повертаємо кількість симпатичних візерунків
    }

    // Метод для тестування
    public static void RunTests()
    {
        // Тести
        string[] testCases = {
            "2 2",
            "3 3",
            "1 5",
            "3 2"
        };

        string[] expectedResults = {
            "14",
            "322",
            "32",
            "56"
        };

        for (int i = 0; i < testCases.Length; i++)
        {
            string[] dimensions = testCases[i].Split(' ');
            int M = int.Parse(dimensions[0]);
            int N = int.Parse(dimensions[1]);

            int result = CountSymmetricPatterns(M, N);
            Console.WriteLine($"Test {i + 1}: Expected = {expectedResults[i]}, Actual = {result}");

            // Порівняння результату
            if (result.ToString() == expectedResults[i])
            {
                Console.WriteLine($"Test {i + 1} passed!");
            }
            else
            {
                Console.WriteLine($"Test {i + 1} failed.");
            }
        }
    }
}
