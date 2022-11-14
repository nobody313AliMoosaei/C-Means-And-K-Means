using C_Means.Data;
using Practice_Artificial_intelligence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C_Means
{
    internal class Program
    {
        public static DB_Context Context = new DB_Context();
        public static List<C_Means.Entities.Clusters> _Cluster = new List<Entities.Clusters>();
        public static List<Point> _Center = new List<Point>();

        static void Main(string[] args)
        {
            #region Input - C
            int C = -1;
            while (true)
            {
                Console.Write(" Enter 'C' : ");
                int.TryParse(Console.ReadLine(), out C);
                if (C != -1)
                    break;
                else
                {
                    Console.WriteLine("Please Enter C Again !!");
                }
            }
            #endregion
            for (int i = 0; i < C; i++)
            {
                _Center.Add(new Point(0, 0, 0, 0));
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool Resualt = Initial_Data_First(C).Result;
            if (!Resualt)
            {
                throw new Exception("Method => Initial_Data_First Has a Error");
            }

            int m = 2;
            int Counter = 0;
            while (true)
            {
                List<Point> OldCenter = new List<Point>();
                OldCenter = _Center.ToList();

                Calculate_Center_Of_Cluster(m, C);

                List<Point> NewCenter = new List<Point>();
                NewCenter = _Center.ToList();
                Update_Grad(C, m);
                Counter++;
                if (Counter > 10 && OldCenter.Count == NewCenter.Count)
                {
                    int count = 0;
                    for (int i = 0; i < OldCenter.Count; i++)
                    {
                        if (EqualPTP(OldCenter[i], NewCenter[i]))
                            count++;
                    }
                    if (count == (OldCenter.Count - 1))
                        break;
                }
            }

            Show_Data_Cluster(C);
            sw.Stop();
            Console.WriteLine($"Time = {sw.Elapsed}");
            Console.ReadKey();
        }

        #region Initial Data
        public static async Task<bool> Initial_Data_First(int C)
        {
            try
            {
                var Data = Context.Irides.ToList();
                for (int i = 0; i < Data.Count; i++)
                {
                    Point One_Point = new Point(
                        float.Parse(Data[i].Sepal_Width),
                        float.Parse(Data[i].Sepal_Length),
                        float.Parse(Data[i].Petal_Width),
                        float.Parse(Data[i].Petal_Length));

                    //یه نقطه با لیستی از درجه عضویت ها باید بسازیم
                    // نقطه که داریم باید برای هر خوشه یه درجه عضویت رندوم قرار دهیم
                    // مجموع درجه عضویت ها برابر یک شود
                    // ایندکس هر درجه عضویت به ایندکس هر خوشه اشاره می کند
                    Entities.Clusters clu = new Entities.Clusters();
                    clu.Point = One_Point;
                    clu.Grad.Add(await Random_Float(1, 0));
                    for (int j = 1; j < C; j++)
                    {
                        if (j == C - 1)
                            clu.Grad.Add((float)1 - clu.Grad.Sum());
                        else
                            clu.Grad.Add(await Random_Float(1 - clu.Grad.Sum(), 0));
                    }
                    _Cluster.Add(clu);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        private static async Task<float> Random_Float(float Max, float Min)
        {
            Random random = new Random();
            await System.Threading.Tasks.Task.Delay(100);
            var float_Number = (random.NextDouble() * (Max - Min) + Min);
            return (float)float_Number;
        }
        #endregion
        #region Center
        public static void Calculate_Center_Of_Cluster(int m, int C)
        {

            for (int k = 0; k < C; k++)
            {
                Point center = new Point();
                for (int i = 0; i < 4; i++)
                {
                    // صورت کسر
                    float sum_numerator = 0;
                    //0: Sepal_Width
                    //1: Sepal_Lenght
                    //2: Petal_Width
                    //3: Petal_Lenght

                    for (int j = 0; j < _Cluster.Count; j++)
                    {
                        if (i == 0)
                        {
                            sum_numerator += _Cluster[j].Point.Sepal_Width *
                                (float)Math.Pow(_Cluster[j].Grad[k], m);
                        }
                        else if (i == 1)
                        {
                            sum_numerator += _Cluster[j].Point.Sepal_Lenght *
                                (float)Math.Pow(_Cluster[j].Grad[k], m);
                        }
                        else if (i == 2)
                        {
                            sum_numerator += _Cluster[j].Point.Petal_Width *
                                (float)Math.Pow(_Cluster[j].Grad[k], m);
                        }
                        else if (i == 3)
                        {
                            sum_numerator += _Cluster[j].Point.Petal_Lenght *
                                (float)Math.Pow(_Cluster[j].Grad[k], m);
                        }
                    }
                    //مخرج کسر
                    float Sum_Denominator = 0;

                    for (int t = 0; t < _Cluster.Count; t++)
                    {
                        Sum_Denominator += (float)Math.Pow(_Cluster[t].Grad[k], m);
                    }

                    if (i == 0)
                    {
                        center.Sepal_Width = sum_numerator / Sum_Denominator;
                    }
                    else if (i == 1)
                    {
                        center.Sepal_Lenght = sum_numerator / Sum_Denominator;
                    }
                    else if (i == 2)
                    {
                        center.Petal_Width = sum_numerator / Sum_Denominator;
                    }
                    else if (i == 3)
                    {
                        center.Petal_Lenght = sum_numerator / Sum_Denominator;
                    }
                }
                _Center[k] = center;
            }
        }
        #endregion
        #region Distance Point To Point
        public static float Distance_PTP(Point Point1, Point Point2)
        {
            float Distance = 0;
            Distance = (float)Math.Sqrt(
                (Math.Pow(Point1.Sepal_Lenght - Point2.Sepal_Lenght, 2)) +
                (Math.Pow(Point1.Sepal_Width - Point2.Sepal_Width, 2)) +
                (Math.Pow(Point1.Petal_Width - Point2.Petal_Width, 2)) +
                (Math.Pow(Point1.Petal_Lenght - Point2.Petal_Lenght, 2)));

            return Math.Abs((float)Distance);
        }
        #endregion
        #region Update Grad
        public static void Update_Grad(int C, int m)
        {
            foreach (var point in _Cluster)
            {
                // مخرج
                float Total_Distance_Point = 0;
                foreach (var item in _Center)
                {
                    Total_Distance_Point += (float)Math.Pow(((float)1 / Distance_PTP(item, point.Point)), 1 / (m - 1));
                }
                for (int i = 0; i < C; i++)
                {
                    var Numerator = (float)Math.Pow((float)1 / Distance_PTP(point.Point, _Center[i]), 1 / (m - 1));
                    point.Grad[i] = (float)
                        Numerator / Total_Distance_Point;
                }
            }
        }
        #endregion
        #region Eaual Point To Point
        public static bool EqualPTP(Point point1, Point point2)
        {
            if ((point1.Sepal_Lenght.ToString("0.###") == point2.Sepal_Lenght.ToString("0.###")) &&
                (point1.Sepal_Width.ToString("0.###") == point2.Sepal_Width.ToString("0.###")) &&
                (point1.Petal_Width.ToString("0.###") == point2.Petal_Width.ToString("0.###")) &&
                (point1.Petal_Lenght.ToString("0.###") == point2.Petal_Lenght.ToString("0.###")))
                return true;
            return false;
        }
        #endregion
        #region Show Data
        public static void Show_Data_Cluster(int C)
        {
            Console.Write("Sepal_Width \t Sepal_Lenght \t Petal_Width \t Petal_Lenght \t");
            for (int i = 0; i < C; i++)
            {
                Console.Write($"C{i} \t");
            }
            Console.WriteLine();
            foreach (var item in _Cluster)
            {
                Console.Write($"{item.Point.Sepal_Width} \t {item.Point.Sepal_Lenght} \t {item.Point.Petal_Width} \t {item.Point.Petal_Lenght} \t");
                for(int i=0;i<C;i++)
                {
                    Console.Write($"{item.Grad[i]} \t");
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
