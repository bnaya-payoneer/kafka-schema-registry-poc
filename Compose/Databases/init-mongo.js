db.createCollection("schema", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["key", "version", "definition", "modifiedDate"],
            properties: {
                key: {
                    bsonType: "string"
                },
                version: {
                    bsonType: "int"
                },
                type: {
                    bsonType: "string",
                    description: "The schema type [Avro,etc.]"
                },
                definition: {
                    bsonType: "string",
                    description: "The schema structure"
                },
                "modifiedDate": {
                    bsonType: "date"
                }
            }
        }
    }
});

db.products.createIndex({
    key: 1,
    version: 2
});
