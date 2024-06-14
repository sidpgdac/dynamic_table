using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmbcApp.Model
{
    public class ParentDetail
    {
        public int Id { get; set; }

        [Column("datasource_name")]
        public string DatasourceName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public ICollection<ColumnDetail> ColumnDetails { get; set; }
    }
}
