namespace smbcbackend
{
    public class sqlscript
    {

        /*
       -- Drop valuedetails table if it exists
IF OBJECT_ID('valuedetails', 'U') IS NOT NULL
    DROP TABLE valuedetails;

-- Drop columndetail table if it exists
IF OBJECT_ID('columndetail', 'U') IS NOT NULL
    DROP TABLE columndetail;

-- Drop parentdetail table if it exists
IF OBJECT_ID('parentdetail', 'U') IS NOT NULL
    DROP TABLE parentdetail;

-- Drop ValueDetailsAudit table if it exists
IF OBJECT_ID('ValueDetailsAudit', 'U') IS NOT NULL
    DROP TABLE ValueDetailsAudit;

-- Create parentdetail table to store datasource details
CREATE TABLE parentdetail (
    id INT PRIMARY KEY IDENTITY(1,1),
    datasource_name NVARCHAR(255) NOT NULL,
    description NVARCHAR(500),
    is_active BIT NOT NULL DEFAULT 1
);

-- Create columndetail table to store column details of each datasource
CREATE TABLE columndetail (
    id INT PRIMARY KEY IDENTITY(1,1),
    parent_id INT NOT NULL FOREIGN KEY REFERENCES parentdetail(id),
    column_name NVARCHAR(255) NOT NULL,
    data_type NVARCHAR(50) NOT NULL,
    is_required BIT NOT NULL,
    is_nullable BIT NOT NULL,
    screen_sequence INT,
    user_friendly_name NVARCHAR(255),
    display_format NVARCHAR(100),
    is_editable BIT NOT NULL DEFAULT 1,
    constraint_expression NVARCHAR(255),
    start_date DATETIME,
    end_date DATETIME,
    error_message NVARCHAR(500)
);

-- Create valuedetails table to store values for each column of each datasource
CREATE TABLE valuedetails (
    id INT PRIMARY KEY IDENTITY(1,1),
    column_id INT NOT NULL FOREIGN KEY REFERENCES columndetail(id),
    row_id INT NOT NULL,
    value NVARCHAR(MAX) NULL
);

-- Create ValueDetailsAudit table to store audit information
CREATE TABLE ValueDetailsAudit (
AuditId INT PRIMARY KEY IDENTITY(1,1),
    ColumnId INT NOT NULL FOREIGN KEY REFERENCES columndetail(id), -- Add this column
    RowId INT NOT NULL,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
	ColumnName  NVARCHAR(MAX),
	DatasourceName NVARCHAR(MAX),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(255)
);


-- Insert data into parentdetail table
INSERT INTO parentdetail (datasource_name, description, is_active) VALUES 
('Datasource A', 'Description for Datasource A', 1),
('Datasource B', 'Description for Datasource B', 1),
('Datasource C', 'Description for Datasource C', 1);
GO

-- Insert data into columndetail table
-- For Datasource A
INSERT INTO columndetail (parent_id, column_name, data_type, is_required, is_nullable, screen_sequence, user_friendly_name, display_format, is_editable, constraint_expression, start_date, end_date, error_message) VALUES 
(1, 'Column1_A', 'INT', 1, 0, 1, 'Client ID', NULL, 1, 'value > 100', NULL, NULL, 'Client ID must be greater than 100'),
(1, 'Column2_A', 'NVARCHAR(255)', 1, 0, 2, 'Address', NULL, 1, NULL, NULL, NULL, 'Address must not be empty'),
(1, 'Column3_A', 'DATETIME', 0, 1, 3, 'On Boarding Date', NULL, 1, 'value <= GETDATE()', NULL, NULL, 'On Boarding Date must be in the past'),
(1, 'Column4_A', 'DATETIME', 0, 1, 4, 'On Boarding Time', NULL, 0, NULL, NULL, NULL, 'On Boarding Time must be within working hours'),
(1, 'Column5_A', 'BIT', 1, 0, 5, 'Checked ?', NULL, 1, 'value IN (0, 1)', NULL, NULL, 'Checked value must be 0 or 1'),
(1, 'Column6_A', 'BIT', 1, 0, 6, 'IsActive', NULL, 1, 'value IN (0, 1)', NULL, NULL, 'IsActive value must be 0 or 1');

-- For Datasource B
INSERT INTO columndetail (parent_id, column_name, data_type, is_required, is_nullable, screen_sequence, user_friendly_name, display_format, is_editable, constraint_expression, start_date, end_date, error_message) VALUES 
(2, 'Column1_B', 'NVARCHAR(255)', 1, 0, 1, 'Column 1 B', NULL, 0, NULL, NULL, NULL, NULL),
(2, 'Column2_B', 'INT', 0, 1, 2, 'Column 2 B', NULL, 1, NULL, NULL, NULL, NULL),
(2, 'Column3_B', 'BIT', 1, 0, 3, 'Column 3 B', NULL, 1, NULL, NULL, NULL, NULL),
(2, 'Column4_B', 'FLOAT', 0, 1, 4, 'Column 4 B', NULL, 0, NULL, NULL, NULL, NULL),
(2, 'Column5_B', 'DATETIME', 1, 0, 5, 'Column 5 B', NULL, 1, NULL, NULL, NULL, NULL),
(2, 'Column6_B', 'BIT', 1, 0, 6, 'IsActive', NULL, 0, NULL, NULL, NULL, NULL);

-- For Datasource C
INSERT INTO columndetail (parent_id, column_name, data_type, is_required, is_nullable, screen_sequence, user_friendly_name, display_format, is_editable, constraint_expression, start_date, end_date, error_message) VALUES 
(3, 'Column1_C', 'DATETIME', 1, 0, 1, 'Column 1 C', NULL, 0, NULL, NULL, NULL, NULL),
(3, 'Column2_C', 'NVARCHAR(50)', 1, 0, 2, 'Column 2 C', NULL, 1, NULL, NULL, NULL, NULL),
(3, 'Column3_C', 'INT', 0, 1, 3, 'Column 3 C', NULL, 0, NULL, NULL, NULL, NULL),
(3, 'Column4_C', 'BIT', 1, 0, 6, 'IsActive', NULL, 1, NULL, NULL, NULL, NULL);
GO

-- Insert data into valuedetails table
-- For Datasource A
INSERT INTO valuedetails (column_id, row_id, value) VALUES 
(1, 1, '123'),
(2, 1, 'Sample Text A1'),
(3, 1, '2023-01-01'),
(4, 1, '2023-01-01'),
(5, 1, '1'),
(6, 1, '1'); -- Ensuring IsActive is set

-- For Datasource B
INSERT INTO valuedetails (column_id, row_id, value) VALUES 
(7, 1, 'Sample Text B1'),
(8, 1, '789'),
(9, 1, '1'),
(10, 1, '34.56'),
(11, 1, '2023-03-01'),
(12, 1, '1'); -- Ensuring IsActive is set

-- For Datasource C
INSERT INTO valuedetails (column_id, row_id, value) VALUES 
(13, 1, '2023-05-01'),
(14, 1, 'Sample Text C1'),
(15, 1, '345'),
(16, 1, '1'); -- Ensuring IsActive is set
GO

         */
    }
}
