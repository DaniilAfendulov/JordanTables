using System;
using static System.Math;
namespace JordanTables
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] jor1 = new double[][]
            {
                new double[]{ 9, 1,   0,   0,   6},      // x4
                new double[]{2,  3,   1,   -4,  2},      // x7
                new double[]{6,  1,   2,   0,   2},      // x5       
                new double[]{ 15,    1,   3, - 1,  9 },  //F=
                new double[]{ 2, 3,   1, - 4,  2 }       //F=M
            };

            // Шаги жорданова исключения
            Console.WriteLine("Изначальная таблица");
            Print(jor1);
            Print(jor1 = JordanStep(jor1, 1, 1, new int[] { }, new int[] { 1 })); // с удалением 1 строки(отсчет с 0) // x1,x7
            Print(jor1 = JordanStep(jor1, 1, 3, new int[] { 4 }, new int[] { })); // с удалением 4 столбца(отсчет с 0) //x1,x6
            Print(jor1 = JordanStep(jor1, 0, 2)); //x1,x6
            Print(jor1 = JordanStep(jor1, 2, 1)); //x4,x3
            Console.WriteLine("введенная таблица:");
            double[][] jor2 = new double[][]
            {
                new double[]{ 5.0/8,   1.0/8, 1.0/24,    -7.0/12},//x3   
                new double[]{ 3.0/2,   0,   1.0/6, 1.0/6},//x6
                new double[]{ 3.0/2,   1.0/2, -1.0/6,    2.0/6},//x2
                new double[]{ -19.0/8, - 11.0 / 8, - 23.0 / 24, - 25.0 / 12 }//F
            };
            Print(jor2);

            Cmpr(jor1, jor2, 3);
            Console.ReadLine();
        }

        /// <summary>
        /// Make Jordan step
        /// </summary>
        /// <param name="coef">Модифицированная Жорданова таблица</param>
        /// <param name="ind1">Строка разрешеющего элемента(отсчет с 0)</param>
        /// <param name="ind2">Столбец разрешеющего элемента(отсчет с 0)</param>
        /// <returns></returns>
        static double[][] JordanStep(double[][] coef, int ind1, int ind2)
        {
            double[][] ans = new double[coef.Length][];
            for (int i = 0; i < coef.Length; i++)
            {
                ans[i] = new double[coef[i].Length];
                for (int j = 0; j < coef[i].Length; j++)
                {
                    if (i == ind1 && j == ind2)
                    {
                        ans[i][j] = 1.0 / coef[ind1][ind2];
                        continue;
                    }
                    if (i == ind1)
                    {
                        ans[i][j] = coef[i][j] / coef[ind1][ind2];
                        continue;
                    }
                    if (j == ind2)
                    {
                        ans[i][j] = -coef[i][j] / coef[ind1][ind2];
                        continue;
                    }
                    ans[i][j] = coef[i][j] - (coef[i][ind2] * coef[ind1][j]) / coef[ind1][ind2];
                }
            }
            return ans;
        }
        /// <summary>
        /// Make Jordan step with remove
        /// </summary>
        /// <param name="coef">Модифицированная Жорданова таблица</param>
        /// <param name="ind1">Строка разрешеющего элемента(отсчет с 0)</param>
        /// <param name="ind2">Столбец разрешеющего элемента(отсчет с 0)</param>
        /// <param name="indi">Массив индексов строк для удаления</param>
        /// <param name="indj">Массив индексов столбцов для удаления</param>
        /// <returns></returns>
        static double[][] JordanStep(double[][] coef, int ind1, int ind2, int[] indi, int[] indj)
        {
            double[][] jor = JordanStep(coef, ind1, ind2);
            double[][] ans = new double[jor.Length - indi.Length][];
            int ansi = 0;
            int ansj = 0;
            for (int i = 0; i < jor.Length; i++)
            {
                if (IsInArr(i, indi)) continue;
                ans[ansi] = new double[jor[i].Length - indj.Length];
                for (int j = 0; j < jor[i].Length; j++)
                {
                    if (IsInArr(j, indj)) continue;
                    ans[ansi][ansj] = jor[i][j];
                    ansj++;
                }
                ansi++;
                ansj = 0;
            }
            return ans;
        }
        /// <summary>
        /// Выводит в консоль значения массивов(разделяя табом и строками),
        /// исключая элементы удовлетв условию для индексов
        /// </summary>
        /// <param name="arr">массив, который нужно вывести</param>
        /// <param name="exep">условие для индексов</param>
        static void Print(double[][] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write(Round(arr[i][j], 3) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Сравнивает таблицы, выводит на консоль и подсвечивает несоответствия
        /// </summary>
        /// <param name="jor1">первая таблица</param>
        /// <param name="jor2">вторая таблица</param>
        /// <param name="digits">точность сравнения и вывода(кол-во знаков после запятой)</param>
        static void Cmpr(double[][] jor1, double[][] jor2, int digits)
        {
            for (int i = 0; i < jor2.Length; i++)
            {
                for (int j = 0; j < jor2[i].Length; j++)
                {
                    if (Abs(Round(jor1[i][j], digits) - Round(jor2[i][j], digits)) < 1 / Pow(10, digits - 1))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Round(jor2[i][j], digits) + "\t");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static bool IsInArr(int a, int[] arr)
        {
            bool isEx = false;
            foreach (int iitem in arr)
            {
                if (iitem == a)
                {
                    isEx = true;
                    break;
                }
            }
            return isEx;
        }
    }
}
