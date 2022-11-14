namespace Practice_Artificial_intelligence.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cluster")]
    public partial class ClusterTB
    {
        [Key]
        public int Cluster_Id { get; set; }

        [StringLength(150)]
        public string ClusterName { get; set; }

        public int? Point_Id { get; set; }

        public double? Grad { get; set; }

        public virtual Iris Iris { get; set; }
    }
}
