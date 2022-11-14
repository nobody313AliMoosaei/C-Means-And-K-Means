using System;
using System.Collections.Generic;
using System.Linq;
using Practice_Artificial_intelligence.Data;
using Practice_Artificial_intelligence.Entities;
using System.Data.Entity;
using System.Diagnostics;

namespace Practice_Artificial_intelligence
{
    internal class Program
    {
        public static Db_Context Context = new Db_Context();
        public static List<Point> Centers;
        public static List<Dictionary<Point, int>> Cluster;

        /*
         در ابتدا کا تا نقطه رندم به عنوان مرکز های خوشه ها انتخاب می کنیم
        سپس یکی یکی فاصله نقطه های داده شده را با مراکز حساب کرده و کمترین فاصله را بدست می اوریم
        هر نقطه در خوشه ای قرار می گیرد که ک کمترین فاصله با آن خوشه داشته
        سپس مرکز همان خوشه دوباره آپدیت میشود
        و از نو دوباره نقطه بعدی را محاسبه می کنیم 
         */
        static void Main(string[] args)
        {
            #region Get K From Input
            int K = 0;
            bool Flag1 = true;
            while (Flag1)
            {
                Console.Write(@" K : ");
                string KInput = Console.ReadLine();

                int.TryParse(KInput, out K);

                if (K == 0)
                {
                    Console.WriteLine("Input Not Valid");
                }
                else
                {
                    Flag1 = false;
                }
            }
            #endregion
            #region Random K Point as Centers
            Centers = GetRandom_K_Points(K);
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();
            #region Main
            // Get All Point From Iris Table
            var Data = Context.IrisTb.ToList();

            // Defnition Cluster
            Cluster = new List<Dictionary<Point, int>>();

            // Counter For End Loop
            int Counter_End_Loop = 0;

            // Loop => True
            int Count_End_Program = 0;

            for (int i = 0; true; i++)
            {
                if (i > Data.Count - 1)
                {
                    i = 0;
                    Count_End_Program++;
                }
                if (Count_End_Program > 10)
                    break;


                var OnePoint = new Point()
                {
                    Petal_Lenght = float.Parse(Data[i].Petal_Length),
                    Petal_Width = float.Parse(Data[i].Petal_Width),
                    Sepal_Lenght = float.Parse(Data[i].Sepal_Length),
                    Sepal_Width = float.Parse(Data[i].Sepal_Width)
                };
                //6037991199510854
                // Distance From All K points so Select Min Distance
                int Index = MinDistance(Centers, OnePoint);
                Dictionary<Point, int> ValuePairs = new Dictionary<Point, int>();

                ValuePairs.Add(OnePoint, Index);

                // if Point in Cluster => Remove this Point From Cluster
                var Exist_Point = Find_Point(OnePoint);
                if (Exist_Point != -1)
                {
                   Cluster.RemoveAt(Exist_Point);
                }

                Cluster.Add(ValuePairs);
                // Old Center Cluster[Index]
                Point OldPointOfCluster = new
                    Point(Centers[Index].Sepal_Width, Centers[Index].Sepal_Lenght,
                    Centers[Index].Petal_Width, Centers[Index].Petal_Lenght);

                //Update center Cluster == Index
                UpdateCLuster(GetClusterByIndex(Index).ToList(), Index);

                // new Center Cluster[Index]
                Point NewPointofCluster =
                    new Point(Centers[Index].Sepal_Width, Centers[Index].Sepal_Lenght,
                    Centers[Index].Petal_Width, Centers[Index].Petal_Lenght);

                if (OldPointOfCluster.Petal_Lenght.ToString("0.###") == NewPointofCluster.Petal_Lenght.ToString("0.###") &&
                    OldPointOfCluster.Petal_Width.ToString("0.###") == NewPointofCluster.Petal_Width.ToString("0.###") &&
                    OldPointOfCluster.Sepal_Width.ToString("0.###") == NewPointofCluster.Sepal_Width.ToString("0.###") &&
                    OldPointOfCluster.Sepal_Lenght.ToString("0.###") == NewPointofCluster.Sepal_Lenght.ToString("0.###"))
                {
                    Counter_End_Loop++;
                }

                // if COunter>1 => Break
                if (Counter_End_Loop > 2)
                {
                    break;
                }

            }
            ShowExistCluster(K);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Target Function : " + Target_Function(K));
            #endregion
            sw.Stop();
            Console.WriteLine("Total Time : " + sw.Elapsed);

            Console.ReadKey();
        }

        // Methods
        #region Show All Data From Iris Table
        public static void ShowAllDataIris()
        {
            var Resualt = Context.IrisTb.ToList();
            foreach (var item in Resualt)
            {
                Console.WriteLine($"{item.Petal_Width} \t {item.Petal_Length} \t {item.Sepal_Width} \t {item.Sepal_Length} \t" +
                    $"{item.Class}");
            }
        }
        #endregion

        #region Get K-Point by Random 
        public static List<Point> GetRandom_K_Points(int K)
        {
            var ListPoints = new List<Point>();
            var Random = new Random();
            for (int i = 0; i < K; i++)
            {
                float x1 = (float)(Random.NextDouble() * 10);
                float y1 = (float)(Random.NextDouble() * 10);
                float x2 = (float)(Random.NextDouble() * 10);
                float y2 = (float)(Random.NextDouble() * 10);
                var NewPoint = new Point(x1, y1, x2, y2);
                ListPoints.Add(NewPoint);
            }
            return ListPoints;
        }
        #endregion

        #region Find Point Form Cluster
        public static int Find_Point(Point point)
        {
            if (point == null)
            {
                throw new Exception("Point In Func 'Find_Point' Is null");
            }
            for (int i = 0; i < Cluster.Count; i++)
            {
                var tr = Cluster[i].ToList();
                foreach (var item in tr)
                {
                    if (item.Key.Petal_Width == point.Petal_Width &&
                        item.Key.Petal_Lenght == point.Petal_Lenght &&
                        item.Key.Sepal_Width == point.Sepal_Width &&
                        item.Key.Sepal_Lenght == point.Sepal_Lenght)
                    {
                        return i;
                    }

                }
            }
            return -1;
        }
        #endregion

        #region Distance Point To Point
        public static float DistancePTP(Point Point1, Point Point2)
        {
            var b = Math.Pow(Point1.Sepal_Width - Point2.Sepal_Width, 2) +
                Math.Pow(Point1.Sepal_Lenght - Point2.Sepal_Lenght, 2) +
                Math.Pow(Point1.Petal_Width - Point2.Petal_Width, 2) +
                Math.Pow(Point1.Petal_Lenght - Point2.Petal_Lenght, 2);
            var D = Math.Sqrt(b);
            return Math.Abs((float)D);
        }
        #endregion

        #region Find Min Distance 
        // Return Indext Center
        public static int MinDistance(List<Point> _center, Point point)
        {
            // float = Distance
            // int = index Center
            Dictionary<int, float> Overlab = new Dictionary<int, float>();
            for (int i = 0; i < _center.Count; i++)
            {
                var Dis_Point = DistancePTP(_center[i], point);
                Overlab.Add(i, Dis_Point);
            }
            var resualt = Overlab.OrderBy(t => t.Value).ToList();
            return resualt[0].Key;
        }
        #endregion

        #region Get Cluster == Index
        public static List<Dictionary<Point, int>> GetClusterByIndex(int Index)
        {
            var resualt = new List<Dictionary<Point, int>>();
            for (int i = 0; i < Cluster.Count; i++)
            {
                var r = Cluster[i].ToList();
                for (int j = 0; j < r.Count; j++)
                {
                    if (r[j].Value == Index)
                    {
                        var index = r[j].Value;
                        var point = r[j].Key;
                        Dictionary<Point, int> NewRecord = new Dictionary<Point, int>();
                        NewRecord.Add(point, index);
                        resualt.Add(NewRecord);
                    }
                }
            }
            return resualt;
        }

        #endregion

        #region Update Cluster
        public static void UpdateCLuster(List<Dictionary<Point, int>> clusters, int Indext)
        {
            // updete Cluster by Sum(X)/Count(Cluster+1) , Sum(Y)/Count(Cluster)+1 , ... 
            // Add To Sum Center of Cluster

            float Sum_Sepal_lenght = Centers[Indext].Sepal_Lenght;
            float Sum_Sepal_Width = Centers[Indext].Sepal_Width;
            float Sum_Petal_Lenght = Centers[Indext].Petal_Lenght;
            float Sum_Petal_width = Centers[Indext].Petal_Width;
            int Count = 1;
            for (int i = 0; i < clusters.Count; i++)
            {
                var Points = clusters[i].ToList();
                for (int j = 0; j < Points.Count; j++)
                {
                    Sum_Sepal_lenght += Points[j].Key.Sepal_Lenght;
                    Sum_Sepal_Width += Points[j].Key.Sepal_Width;
                    Sum_Petal_Lenght += Points[j].Key.Petal_Lenght;
                    Sum_Petal_width += Points[j].Key.Petal_Width;
                    Count++;
                }
            }
            // Update Center Cluster
            Centers[Indext].Sepal_Width = Sum_Sepal_Width / Count;
            Centers[Indext].Sepal_Lenght = Sum_Sepal_lenght / Count;
            Centers[Indext].Petal_Width = Sum_Petal_width / Count;
            Centers[Indext].Petal_Lenght = Sum_Petal_Lenght / Count;
        }
        #endregion

        #region Show Exist Cluster Still
        public static void ShowExistCluster(int K)
        {
            Console.WriteLine();
            Console.WriteLine("**************************************************************************");
            Console.WriteLine();

            for (int i = 0; i < K; i++)
            {
                var DataForOnCluster = GetClusterByIndex(i).ToList();
                // Petal_width   Petal_Lenght   Sepal_Lenght   Sepal_Width
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Center Cluster '{i}' = {Centers[i].Petal_Width} \t {Centers[i].Petal_Lenght} \t {Centers[i].Sepal_Lenght} \t {Centers[i].Sepal_Width}");
                Console.WriteLine("Petal_width \t Petal_Lenght \t Sepal_Lenght \t Sepal_Width");
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

                for (int j = 0; j < DataForOnCluster.Count; j++)
                {
                    var res = DataForOnCluster[j].ToList();
                    for (int x = 0; x < res.Count; x++)
                    {
                        Console.WriteLine($"{res[x].Key.Petal_Width}\t{res[x].Key.Petal_Lenght}\t{res[x].Key.Sepal_Lenght}\t{res[x].Key.Sepal_Width}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        #endregion

        #region Target Function
        public static float Target_Function(int K)
        {
            float Sum = 0;
            for (int i = 0; i < K; i++)
            {
                Point center = Centers[i];
                var _cluster = GetClusterByIndex(i).ToList();
                for (int j = 0; j < _cluster.Count; j++)
                {
                    var listOne = _cluster[j].ToList();
                    foreach (var item in listOne)
                    {
                        Sum +=(float)Math.Pow(DistancePTP(center, item.Key),2);
                    }
                }
            }
            return Sum;
        }
        #endregion

    }
}
