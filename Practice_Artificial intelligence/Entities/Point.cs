using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Artificial_intelligence.Entities
{
    public class Point
    {
        public float Sepal_Width { get; set; }
        public float Sepal_Lenght { get; set; }
        public float Petal_Width { get; set; }
        public float Petal_Lenght { get; set; }
        public Point()
        {
            Sepal_Width = 0;
            Sepal_Lenght = 0;
            Petal_Width = 0;
            Petal_Lenght = 0;
        }

        public Point(float Sepal_width, float Sepal_lenght,float Petal_width,float Petal_lenght)
        {
            Sepal_Width = Sepal_width;
            Sepal_Lenght = Sepal_lenght;
            Petal_Width = Petal_width;
            Petal_Lenght = Petal_lenght;
        }
    }
}
