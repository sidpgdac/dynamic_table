using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmbcApp.Data;
using SmbcApp.Model;
using smbcbackend.Model;
using smbcbackend.ValidationService;

namespace smbcbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataValuesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidationService _validationService;

        public DataValuesController(AppDbContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentDetail>>> GetDatasources()
        {
            return await _context.ParentDetails.ToListAsync();
        }


        // GET: api/Datasource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetDatasourceDetails(int id)
        {
            var parentDetail = await _context.ParentDetails
                .Include(p => p.ColumnDetails)
                .ThenInclude(c => c.ValueDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (parentDetail == null)
            {
                return NotFound();
            }

            var columns = parentDetail.ColumnDetails.Select(c => new
            {
                c.ColumnName,
                c.DataType,
                c.IsRequired,
                c.IsNullable,
                c.ScreenSequence,
                c.UserFriendlyName,
                c.DisplayFormat,
                c.IsEditable,
                c.ConstraintExpression,
                c.StartDate,
                c.EndDate,
                c.ErrorMessage

            });

            var values = parentDetail.ColumnDetails
                .SelectMany(c => c.ValueDetails)
                .GroupBy(v => v.RowId)
                .Select(g => g.ToDictionary(v => v.ColumnDetail.ColumnName, v => v.Value));

            return new
            {
                Datasource = parentDetail.DatasourceName,
                Description = parentDetail.Description,
                IsActive = parentDetail.IsActive,
                Columns = columns,
                Values = values
            };
        }

        //[HttpPost("{parentId}/values")]
        //public async Task<ActionResult> AddValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        //{
        //    var parentDetails = await _context.ParentDetails
        //        .Include(p => p.ColumnDetails)
        //        .FirstOrDefaultAsync(p => p.Id == parentId);

        //    if (parentDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    var valueDetails = await _context.ValueDetails
        //        .Where(v => v.ColumnDetail.ParentId == parentId)
        //        .ToListAsync();

        //    // Deactivate all current active rows


        //    var maxRowId = valueDetails.Select(v => v.RowId).DefaultIfEmpty(0).Max();
        //    int newRowId = maxRowId + 1;

        //    //updating isActive of prev active row



        //    foreach (var rowData in values)
        //    {
        //        foreach (var columnDetail in parentDetails.ColumnDetails)
        //        {
        //            if (columnDetail.IsEditable)
        //            {
        //                // Process new value for editable columns
        //                if (rowData.TryGetValue(columnDetail.ColumnName, out var newValue))
        //                {
        //                    var valueDetail = new ValueDetail
        //                    {
        //                        ColumnId = columnDetail.Id,
        //                        RowId = newRowId,
        //                        Value = newValue,
        //                    };
        //                    _context.ValueDetails.Add(valueDetail);
        //                }
        //                else
        //                {
        //                    // Handle the case where the column name in the payload doesn't match any column in the database
        //                    // You can log a warning or handle it based on your application's requirements
        //                }
        //            }
        //            else
        //            {
        //                // Fetch and copy the latest value for non-editable columns
        //                var latestValue = valueDetails
        //                    .Where(v => v.ColumnId == columnDetail.Id)
        //                    .OrderByDescending(v => v.RowId)
        //                    .FirstOrDefault()?.Value;

        //                var valueDetail = new ValueDetail
        //                {
        //                    ColumnId = columnDetail.Id,
        //                    RowId = newRowId,
        //                    Value = latestValue,

        //                };
        //                _context.ValueDetails.Add(valueDetail);
        //            }
        //        }
        //        newRowId++; // Increment rowId for the next row of data
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}


        [HttpPost("{parentId}/values")]
        public async Task<ActionResult> AddValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        {
            var parentDetails = await _context.ParentDetails
                    .Include(p => p.ColumnDetails)
                    .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parentDetails == null) { return NotFound(); }

            var valueDetails = await _context.ValueDetails
        .Where(v => v.ColumnDetail.ParentId == parentId)
        .ToListAsync();

            var maxRowId = valueDetails.Select(v => v.RowId).DefaultIfEmpty(0).Max();
            int newRowId = maxRowId + 1;

            foreach (var rowData in values)
            {
                foreach (var kvp in rowData)
                {
                    var columnDetail = parentDetails.ColumnDetails.FirstOrDefault(c => c.ColumnName == kvp.Key);
                    if (columnDetail != null)
                    {
                        var valueDetail = new ValueDetail
                        {
                            ColumnId = columnDetail.Id,
                            RowId = newRowId,
                            Value = kvp.Value
                        };
                        _context.ValueDetails.Add(valueDetail);
                    }
                    else
                    {
                        // Handle the case where the column name in the payload doesn't match any column in the database
                        // You can log a warning or handle it based on your application's requirements
                    }
                }
                newRowId++; // Increment rowId for the next row of data
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPut("{parentId}/values")]
        //public async Task<ActionResult> UpdateValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        //{
        //    var parentDetails = await _context.ParentDetails
        //        .Include(p => p.ColumnDetails)
        //        .FirstOrDefaultAsync(p => p.Id == parentId);

        //    if (parentDetails == null) { return NotFound(); }

        //    var valueDetails = await _context.ValueDetails
        //        .Where(v => v.ColumnDetail.ParentId == parentId)
        //        .ToListAsync();

        //    foreach (var rowData in values)
        //    {
        //        if (!rowData.ContainsKey("__rowId"))
        //        {
        //            return BadRequest("Missing __rowId in row data.");
        //        }

        //        if (!int.TryParse(rowData["__rowId"], out int rowId))
        //        {
        //            return BadRequest("Invalid __rowId in row data.");
        //        }

        //        foreach (var kvp in rowData)
        //        {
        //            if (kvp.Key == "__rowId") continue;

        //            var columnDetail = parentDetails.ColumnDetails.FirstOrDefault(c => c.ColumnName == kvp.Key);
        //            if (columnDetail != null)
        //            {
        //                var valueDetail = valueDetails.FirstOrDefault(v => v.ColumnId == columnDetail.Id && v.RowId == rowId);
        //                if (valueDetail != null)
        //                {
        //                    valueDetail.Value = kvp.Value;
        //                }
        //                else
        //                {
        //                    var newValueDetail = new ValueDetail
        //                    {
        //                        ColumnId = columnDetail.Id,
        //                        RowId = rowId,
        //                        Value = kvp.Value
        //                    };
        //                    _context.ValueDetails.Add(newValueDetail);
        //                }
        //            }
        //            else
        //            {
        //                // Handle the case where the column name in the payload doesn't match any column in the database
        //                // You can log a warning or handle it based on your application's requirements
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}


        //imp
        //[HttpPut("{parentId}/values")]
        //public async Task<ActionResult> UpdateValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        //{
        //    var parentDetails = await _context.ParentDetails
        //        .Include(p => p.ColumnDetails)
        //        .FirstOrDefaultAsync(p => p.Id == parentId);

        //    if (parentDetails == null) { return NotFound(); }

        //    var valueDetails = await _context.ValueDetails
        //        .Where(v => v.ColumnDetail.ParentId == parentId)
        //        .ToListAsync();

        //    foreach (var rowData in values)
        //    {
        //        if (!rowData.ContainsKey("__rowId"))
        //        {
        //            return BadRequest("Missing __rowId in row data.");
        //        }

        //        if (!int.TryParse(rowData["__rowId"], out int rowId))
        //        {
        //            return BadRequest("Invalid __rowId in row data.");
        //        }

        //        foreach (var kvp in rowData)
        //        {
        //            if (kvp.Key == "__rowId") continue;

        //            var columnDetail = parentDetails.ColumnDetails.FirstOrDefault(c => c.ColumnName == kvp.Key);
        //            if (columnDetail != null)
        //            {
        //                var valueDetail = valueDetails.FirstOrDefault(v => v.ColumnId == columnDetail.Id && v.RowId == rowId);
        //                if (valueDetail != null)
        //                {
        //                    valueDetail.Value = kvp.Value;
        //                }
        //                else
        //                {
        //                    var newValueDetail = new ValueDetail
        //                    {
        //                        ColumnId = columnDetail.Id,
        //                        RowId = rowId,
        //                        Value = kvp.Value
        //                    };
        //                    _context.ValueDetails.Add(newValueDetail);
        //                }
        //            }
        //            else
        //            {
        //                // Log warning for unmatched column name
        //                Console.WriteLine($"Warning: Column name '{kvp.Key}' not found in database.");
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpPut("{parentId}/values")]
        public async Task<ActionResult> UpdateValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        {
            var parentDetails = await _context.ParentDetails
                .Include(p => p.ColumnDetails)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parentDetails == null) { return NotFound(); }

            var valueDetails = await _context.ValueDetails
                .Where(v => v.ColumnDetail.ParentId == parentId)
                .ToListAsync();

            var currentUser = HttpContext.User.Identity.Name ?? "System"; // Get current user from HttpContext

            foreach (var rowData in values)
            {
                if (!rowData.ContainsKey("__rowId"))
                {
                    return BadRequest("Missing __rowId in row data.");
                }

                if (!int.TryParse(rowData["__rowId"], out int rowId))
                {
                    return BadRequest("Invalid __rowId in row data.");
                }

                foreach (var kvp in rowData)
                {
                    if (kvp.Key == "__rowId") continue;

                    var columnDetail = parentDetails.ColumnDetails.FirstOrDefault(c => c.ColumnName == kvp.Key);
                    if (columnDetail != null)
                    {
                        var valueDetail = valueDetails.FirstOrDefault(v => v.ColumnId == columnDetail.Id && v.RowId == rowId);
                        if (valueDetail != null)
                        {
                            var oldValue = valueDetail.Value;
                            var newValue = kvp.Value;

                            if (oldValue != newValue)
                            {
                                // Add audit entry before updating the value
                                var auditEntry = new ValueDetailsAudit
                                {
                                    ColumnId = valueDetail.ColumnId,
                                    RowId = valueDetail.RowId,
                                    OldValue = oldValue,
                                    NewValue = newValue,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedBy = currentUser,
                                    ColumnName = valueDetail.ColumnDetail.UserFriendlyName,
                                    DatasourceName = valueDetail.ColumnDetail.ParentDetail.DatasourceName
                                };

                                _context.ValueDetailsAudits.Add(auditEntry);
                            }
                            // Update the value
                            valueDetail.Value = newValue;
                        }
                        else
                        {
                            var newValueDetail = new ValueDetail
                            {
                                ColumnId = columnDetail.Id,
                                RowId = rowId,
                                Value = kvp.Value
                            };
                            _context.ValueDetails.Add(newValueDetail);
                        }
                    }
                    else
                    {
                        // Log warning for unmatched column name
                        Console.WriteLine($"Warning: Column name '{kvp.Key}' not found in database.");
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        //[HttpGet("{parentId}/audit")]
        //public async Task<ActionResult<IEnumerable<ValueDetailsAudit>>> GetAuditEntries(int parentId)
        //{
        //    var auditEntries = await _context.ValueDetailsAudits
        //        .Where(a => a.ColumnDetail.ParentId == parentId)
        //        .ToListAsync();

        //    if (auditEntries == null || !auditEntries.Any())
        //    {
        //        return NotFound("No audit entries found.");
        //    }

        //    return auditEntries;
        //}


        //[HttpPut("{parentId}/values")]
        //public async Task<ActionResult> UpdateValues(int parentId, [FromBody] Dictionary<string, string>[] values)
        //{
        //    var parentDetails = await _context.ParentDetails
        //        .Include(p => p.ColumnDetails)
        //        .FirstOrDefaultAsync(p => p.Id == parentId);

        //    if (parentDetails == null) { return NotFound(); }

        //    var valueDetails = await _context.ValueDetails
        //        .Where(v => v.ColumnDetail.ParentId == parentId)
        //        .ToListAsync();

        //    var currentUser = HttpContext.User.Identity.Name ?? "System"; // Get current user from HttpContext

        //    foreach (var rowData in values)
        //    {
        //        if (!rowData.ContainsKey("__rowId"))
        //        {
        //            return BadRequest("Missing __rowId in row data.");
        //        }

        //        if (!int.TryParse(rowData["__rowId"], out int rowId))
        //        {
        //            return BadRequest("Invalid __rowId in row data.");
        //        }

        //        foreach (var kvp in rowData)
        //        {
        //            if (kvp.Key == "__rowId") continue;

        //            var columnDetail = parentDetails.ColumnDetails.FirstOrDefault(c => c.ColumnName == kvp.Key);
        //            if (columnDetail != null)
        //            {
        //                // Perform validation
        //                if (!_validationService.Validate(columnDetail.DataType, kvp.Value, columnDetail.ConstraintExpression, out var validationMessage))
        //                {
        //                    return BadRequest($"Validation failed for column {columnDetail.ColumnName}: {validationMessage}");
        //                }

        //                var valueDetail = valueDetails.FirstOrDefault(v => v.ColumnId == columnDetail.Id && v.RowId == rowId);
        //                if (valueDetail != null)
        //                {
        //                    var oldValue = valueDetail.Value;
        //                    var newValue = kvp.Value;

        //                    if (oldValue != newValue)
        //                    {
        //                        // Add audit entry before updating the value
        //                        var auditEntry = new ValueDetailsAudit
        //                        {
        //                            ColumnId = valueDetail.ColumnId,
        //                            RowId = valueDetail.RowId,
        //                            OldValue = oldValue,
        //                            NewValue = newValue,
        //                            ModifiedDate = DateTime.Now,
        //                            ModifiedBy = currentUser,
        //                            ColumnName = valueDetail.ColumnDetail.UserFriendlyName,
        //                            DatasourceName = valueDetail.ColumnDetail.ParentDetail.DatasourceName
        //                        };

        //                        _context.ValueDetailsAudits.Add(auditEntry);
        //                    }
        //                    // Update the value
        //                    valueDetail.Value = newValue;
        //                }
        //                else
        //                {
        //                    var newValueDetail = new ValueDetail
        //                    {
        //                        ColumnId = columnDetail.Id,
        //                        RowId = rowId,
        //                        Value = kvp.Value
        //                    };
        //                    _context.ValueDetails.Add(newValueDetail);
        //                }
        //            }
        //            else
        //            {
        //                // Log warning for unmatched column name
        //                Console.WriteLine($"Warning: Column name '{kvp.Key}' not found in database.");
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpGet("Audit")]
        public ActionResult<IEnumerable<ValueDetailsAudit>> GetAuditEntries()
        {
            return _context.ValueDetailsAudits.ToList();
        }

    }
}