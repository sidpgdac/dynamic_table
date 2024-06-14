using SmbcApp.Model;

namespace smbcbackend.Model
{
    public class ValueDetailsAudit
    {
        public int AuditId { get; set; }
        public int ColumnId { get; set; }
        public int RowId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        
        public string ColumnName { get; set; }

        public string DatasourceName { get; set; }
        public ColumnDetail ColumnDetail { get; set; }
    }
}
