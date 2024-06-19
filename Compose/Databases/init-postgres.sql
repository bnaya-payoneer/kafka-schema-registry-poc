CREATE TABLE IF NOT EXISTS schema_registry (
    key VARCHAR(100) NOT NULL,
    version INT NOT NULL,
    definition JSONB NOT NULL,
    modifiedDate TIMESTAMPTZ NOT NULL,
    type VARCHAR(50) NOT NULL DEFAULT 'avro',
    PRIMARY KEY (key, version)
);

CREATE INDEX IF NOT EXISTS idx_schema_registry ON schema_registry (key, version);
