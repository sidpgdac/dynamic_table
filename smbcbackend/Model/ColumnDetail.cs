using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmbcApp.Model
{
    public class ColumnDetail
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [Column("column_name")]
        public string ColumnName { get; set; }

        [Column("data_type")]
        public string DataType { get; set; }

        [Column("is_required")]
        public bool IsRequired { get; set; }

        [Column("is_nullable")]
        public bool IsNullable { get; set; }

        [Column("screen_sequence")]
        public int? ScreenSequence { get; set; }

        [Column("user_friendly_name")]
        public string UserFriendlyName { get; set; }

        [Column("display_format")]
        public string DisplayFormat { get; set; }

        [Column("is_editable")]
        public bool IsEditable { get; set; }

        [Column("constraint_expression")]
        public string ConstraintExpression { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("error_message")]
        public string ErrorMessage { get; set; }

        public ParentDetail ParentDetail { get; set; }
        public ICollection<ValueDetail> ValueDetails { get; set; }
    }
}
