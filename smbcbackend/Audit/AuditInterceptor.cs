using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmbcApp.Model;
using smbcbackend.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace smbcbackend.Audit
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly string _currentUser;

        public AuditInterceptor(string currentUser)
        {
            _currentUser = currentUser;
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            var context = eventData.Context;
            if (context != null)
            {
                AddAuditEntries(context);
            }
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                AddAuditEntries(context);
            }
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditEntries(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<ValueDetail>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var auditEntry = new ValueDetailsAudit
                {
                    ColumnId = entry.Entity.ColumnId,
                    RowId = entry.Entity.RowId,
                    OldValue = entry.OriginalValues["Value"]?.ToString(),
                    NewValue = entry.CurrentValues["Value"]?.ToString(),
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = _currentUser
                };

                context.Set<ValueDetailsAudit>().Add(auditEntry);
            }
        }
    }
}
