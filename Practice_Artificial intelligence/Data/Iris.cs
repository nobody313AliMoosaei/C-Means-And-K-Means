namespace Practice_Artificial_intelligence.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Iris")]
    public partial class Iris
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Iris()
        {
            Clusters = new HashSet<ClusterTB>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClusterTB> Clusters { get; set; }
    }
}
