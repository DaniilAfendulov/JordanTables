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
                DblArrConvert("104	8	13	0"),
                DblArrConvert("208	26	16	0"),
                DblArrConvert("6	1	0	-1"),
                DblArrConvert("7	0	1	0"),
                DblArrConvert("0	-6	-2	0")
            };

            // Шаги жорданова исключения
            Console.WriteLine("Изначальная таблица");
            Print(jor1);
            Print(jor1=JordanStep(jor1,2,1, new int[] { }, new int[] { 1 }));
            //Print(jor1 = JordanStep(jor1, 1, 3, new int[] { }, new int[] { 3 }));
            //Print(jor1 = JordanStep(jor1, 0, 1, new int[] { 4 }, new int[] { 1 }));
            //Print(jor1 = JordanStep(jor1, 0, 2));
            //Print(jor1 = JordanStep(jor1, 0, 3));
            Print(jor1 = SimplexMethod(jor1, true));
            Console.WriteLine("введенная таблица:");
            double[][] jor2 = new double[][]
            {
                DblArrConvert("40	105/13	-4/13"),
                DblArrConvert("2	8/13	1/26"),
                DblArrConvert("8	8/13	1/26"),
                DblArrConvert("7	1	0"),
                DblArrConvert("48	22/13	3/13")
            };
            //Print(jor2);
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

        static double[][] SimplexMethod(double[][] coef, bool Max, bool[] isFree)
        {
            int k = 5;
            while(Max && k>0)
            {
                double max = -1;
                int columnind = -1;
                for (int j = 1; j < coef[coef.Length-1].Length; j++)
                {
                    double t = coef[coef.Length-1][j];
                    if (t < 0 && Abs(t) > max)
                    {
                        max = Abs(t);
                        columnind = j;
                    }
                }
                if (columnind == -1) return coef;
                int rowind = -1;
                double min=0;
                for (int i = 0; i < coef
                    .Length; i++)
                {
                    double t = coef[i][columnind];
                    if (t > 0)
                    {
                        if (rowind == -1)
                        {
                            rowind = i;
                            min = coef[i][0] / t;
                            continue;
                        }
                        if (coef[i][0] / t < min)
                        {
                            rowind = i;
                            min = coef[i][0] / t;
                        }
                    }
                }
                if (rowind == -1) return coef;

                for (int i = 0; i < coef.Length; i++)
                {
                    for (int j = 0; j < coef[i].Length; j++)
                    {
                        bool highlight = i == rowind && j == columnind;
                        if (highlight)
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(Round(coef[i][j], 2) + "\t");
                            Console.ResetColor();
                            continue;
                        }
                        Console.Write(Round(coef[i][j], 2) + "\t");
                       
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                if (isFree[rowind])
                {
                    isFree[rowind] = false;
                    coef = JordanStep(coef, rowind, columnind, new int[] { }, new int[] { columnind });
                }
                else coef = JordanStep(coef, rowind,columnind);
                k--;
            }
            return coef;
        }
        static double[][] SimplexMethod(double[][] coef, bool Max)
        {
            bool[] isFree = new bool[coef.Length];
            for (int i = 0; i < isFree.Length; i++)
            {
                isFree[i] = false;
            }
            return SimplexMethod(coef,Max,isFree);
        }

        static bool[] FreeRows(int[] fr, int n)
        {
            bool[] ans = new bool[n];
            for (int i = 0; i < ans.Length; i++)
            {
                for (int j = 0; j < fr.Length; j++)
                {
                    ans[i] = fr[j] == i;
                    if (ans[i]) break;
                }
            }
            return ans;
        }

        /*static double[][] StrConvert(string[] str)
        {
            string[] rows = str.Split(new char[] {'\n'});
            double[][] ans = new double[rows.Length][];
            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(new char[] { '\t' });
                ans[i] = new double[columns.Length];
                for (int j = 0; j < columns.Length; j++)
                {
                    if (columns[j].Contains("/"))
                    {
                        string[] fraction = columns[j].Split(new char[] { '/' });
                        ans[i][j] = double.Parse(fraction[0]) / double.Parse(fraction[1]);
                        continue;
                    }
                    ans[i][j] = double.Parse(columns[j]);
                }
            }
            return ans;
        }*/
        static double[] DblArrConvert(string str)
        {
            string[] columns = str.Split(new char[] { '\t' });
            double[] ans = new double[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Contains("/"))
                {
                    string[] fraction = columns[i].Split(new char[] { '/' });
                    ans[i] = double.Parse(fraction[0]) / double.Parse(fraction[1]);
                    continue;
                }
                ans[i] = double.Parse(columns[i]);
            }
            return ans;
        }
    }
}
