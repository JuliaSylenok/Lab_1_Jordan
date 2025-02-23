using System;

class MatrixSolver
{
    public static double[,] JordanEliminationMethod(double[,] A, double[] B)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);
        double[,] resultMatrix = (double[,])A.Clone();
        int swapCount = 0;

        string[] rowNames = B.Select((value, index) => $"{value}").ToArray();
        string[] colNames = Enumerable.Range(1, cols).Select(i => $"x{i}").ToArray();

        Console.WriteLine("\n----Знаходження розв'язків СЛАР 1-м методом----");

        Console.WriteLine("\nВхідна матриця B:");
        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"{B[i]}");
        }

        Console.WriteLine("\nВхідна матриця A:");
        PrintLabeledMatrix(resultMatrix, rowNames, colNames);

        for (int s = 0; s < Math.Min(rows, cols); s++)
        {
            resultMatrix = PerformJordanStep(resultMatrix, s, ref swapCount, rowNames, colNames);
        }

        Console.WriteLine("\nОбернена матриця:");
        PrintMatrix(resultMatrix);
        Console.WriteLine($"\nРанг матриці: {swapCount}");
        CalculateSolutionsAfterJordan(resultMatrix, B);

        return resultMatrix;
    }

    public static void CalculateSolutionsAfterJordan(double[,] A, double[] B)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);

        Console.WriteLine("\nОбчислення розв'язків:");

        for (int i = 0; i < rows; i++)
        {
            double sum = 0;
            string calculation = "";
            for (int j = 0; j < cols; j++)
            {
                double term = B[j] * A[i, j];
                calculation += $" {B[j]:F2} * {(A[i, j] < 0 ? ($"({A[i, j]:F2})") : A[i, j].ToString("F2"))} +";
                sum += term;
            }
            calculation = calculation.TrimEnd('+', ' ');
            Console.WriteLine($"X[{i + 1}] = {calculation} = {sum:F2}");
        }
    }
    public static double[,] PerformJordanStep(double[,] matrix, int step, ref int swapCount, string[] rowNames, string[] colNames)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        double pivot = matrix[step, step];
        if (Math.Abs(pivot) < 1e-9) return matrix;

        double[,] BMatrix = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == step)
                {
                    BMatrix[i, j] = (j == step) ? 1 : -matrix[i, j];
                }
                else if (j == step)
                {
                    BMatrix[i, j] = matrix[i, j];
                }
                else
                {
                    BMatrix[i, j] = matrix[i, j] * pivot - matrix[i, step] * matrix[step, j];
                }
            }
        }

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                BMatrix[i, j] /= pivot;

        Console.WriteLine($"\nКрок #{step + 1}");
        Console.WriteLine($"\nРозв’язувальний елемент: A[{step + 1}, {step + 1}] = {pivot:F2}");

        (rowNames[step], colNames[step]) = (colNames[step], rowNames[step]);
        swapCount++;
        Console.WriteLine($"Кількість перестановок = {swapCount}");


        Console.WriteLine("\nПоточна матриця Жорданових виключень:");
        PrintLabeledMatrix(BMatrix, rowNames, colNames);

        return BMatrix;
    }


    public static double[,] JordanMethod(double[,] A, string[] B)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);
        double[,] resultMatrix = (double[,])A.Clone();
        int swapCount = 0;

        string[] rowNames = B.Select((value, index) => $"{value}").ToArray();
        string[] colNames = Enumerable.Range(1, cols).Select(i => $"x{i}").ToArray();

        Console.WriteLine("\n----Протокол пошуку оберненої матриці та рангу матриці:----");
        Console.WriteLine("\nВхідна матриця А:");
        PrintLabeledMatrix(resultMatrix, rowNames, colNames);

        for (int s = 0; s < Math.Min(rows, cols); s++)
        {
            resultMatrix = PerformJordanStep(resultMatrix, s, ref swapCount, rowNames, colNames);
        }

        Console.WriteLine("\nОбернена матриця:");
        PrintMatrix(resultMatrix);
        Console.WriteLine($"\nРанг матриці: {swapCount}");
        return resultMatrix;
    }



    public static void PrintLabeledMatrix(double[,] A, string[] rowNames, string[] colNames)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);

        Console.Write("\t");
        for (int j = 0; j < cols; j++)
        {
            Console.Write($"{colNames[j],8}");
        }
        Console.WriteLine();

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"{rowNames[i],8}");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{A[i, j],8:F2} ");
            }
            Console.WriteLine();
        }
    }

    public static void PrintMatrix(double[,] A)
    {
        int rows = A.GetLength(0);
        int cols = A.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{A[i, j],8:F2} ");
            }
            Console.WriteLine();
        }
    }

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        double[,] matrix = ReadMatrixFromUser();

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Виконати пошук оберненої матриці та її рангу");
            Console.WriteLine("2 - СЛАР 1-м методом");
            Console.WriteLine("3 - Перезаписати матрицю A");
            Console.WriteLine("4 - Вихід");
            Console.Write("Оберіть опцію: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                int rows = matrix.GetLength(0);
                string[] B = Enumerable.Range(1, rows).Select(i => $"b{i}").ToArray();
                JordanMethod(matrix, B);
            }
            else if (choice == 2)
            {
                Console.WriteLine("Введіть вектор B (через пробіл):");
                double[] B = Console.ReadLine().Split().Select(double.Parse).ToArray();
                JordanEliminationMethod(matrix, B);
            }
            else if (choice == 3)
            {
                Console.WriteLine("Перезапис матриці A:");
                matrix = ReadMatrixFromUser();
                Console.WriteLine("Матриця A успішно перезаписана.");
            }
            else if (choice == 4)
            {
                Console.WriteLine("Вихід з програми.");
                break;
            }
            else
            {
                Console.WriteLine("Некоректний вибір, спробуйте ще раз.");
            }
        }
    }

    public static double[,] ReadMatrixFromUser()
    {
        Console.WriteLine("Введіть елементи матриці A по рядках (через пробіл):");

        List<double[]> matrixList = new List<double[]>();
        string input;

        while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
        {
            double[] rowValues = input.Split().Select(double.Parse).ToArray();
            matrixList.Add(rowValues);
        }

        int rows = matrixList.Count;
        int cols = rows > 0 ? matrixList[0].Length : 0;

        double[,] matrix = new double[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = matrixList[i][j];
            }
        }
        return matrix;
    }


}