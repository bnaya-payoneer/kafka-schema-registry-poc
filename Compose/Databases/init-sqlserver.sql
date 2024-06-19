IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='schema_registry' and xtype='U')
CREATE TABLE schema_registry (
    [key] NVARCHAR(100) NOT NULL,
    version INT NOT NULL,
    definition varbinary(max) NOT NULL,
    modifiedDate DATETIMEOFFSET NOT NULL,
    type NVARCHAR(50) NOT NULL DEFAULT 'avro',
    PRIMARY KEY ([key], version)
);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_schema_registry' AND object_id = OBJECT_ID('schema_registry'))
CREATE INDEX idx_schema_registry ON schema_registry ([key], version);
