namespace C_Means.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Iris")]
    public partial class Iris
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Sepal_Length { get; set; }

        [StringLength(50)]
        public string Sepal_Width { get; set; }

        [StringLength(50)]
        public string Petal_Length { get; set; }

        [StringLength(50)]
        public string Petal_Width { get; set; }

        [StringLength(50)]
        public string Class { get; set; }
    }
}
