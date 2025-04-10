using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NaviriaApiTest.DB  // Namespace according to your project structure
{
    //public class MongoDbDuplicate
    //{
    //    private static readonly string sourceConnectionString = "mongodb://localhost:27017"; // Source DB connection string
    //    private static readonly string targetConnectionString = "mongodb://localhost:27017"; // Target DB connection string

    //    public static void Main(string[] args)
    //    {
    //        string sourceDatabaseName = "NaviriaDB"; // Replace with your source DB name
    //        string targetDatabaseName = "NaviriaDBTest"; // Replace with your target DB name

    //        var client = new MongoClient(sourceConnectionString);
    //        var sourceDatabase = client.GetDatabase(sourceDatabaseName);
    //        var targetDatabase = new MongoClient(targetConnectionString).GetDatabase(targetDatabaseName);

    //        DuplicateDatabase(sourceDatabase, targetDatabase);
    //    }

    //    public static void DuplicateDatabase(IMongoDatabase sourceDatabase, IMongoDatabase targetDatabase)
    //    {
    //        // Step 1: List all collections in the source database
    //        var collections = sourceDatabase.ListCollections().ToList();

    //        // Step 2: For each collection, copy data and indexes to the target database
    //        foreach (var collection in collections)
    //        {
    //            var collectionName = collection["name"].AsString;

    //            Console.WriteLine($"Duplicating collection: {collectionName}");

    //            // Step 2.1: Create the collection in the target database
    //            var sourceCollection = sourceDatabase.GetCollection<BsonDocument>(collectionName);
    //            var targetCollection = targetDatabase.GetCollection<BsonDocument>(collectionName);

    //            // Step 2.2: Copy all documents from the source to the target collection
    //            var documents = sourceCollection.Find(new BsonDocument()).ToList();
    //            if (documents.Any())
    //            {
    //                targetCollection.InsertMany(documents);
    //            }

    //            // Step 2.3: Copy indexes (if any) from the source to the target collection
    //            var indexes = sourceCollection.Indexes.List().ToList();
    //            foreach (var index in indexes)
    //            {
    //                var indexKeys = index["key"].AsBsonDocument;  // Get the index keys as a BsonDocument
    //                var indexOptions = index["options"].AsBsonDocument;

    //                // Use Builders<BsonDocument>.IndexKeys to convert the BsonDocument into IndexKeysDefinition
    //                var indexDefinition = Builders<BsonDocument>.IndexKeys.Combine(
    //                    indexKeys.Elements.Select(element => Builders<BsonDocument>.IndexKeys.Ascending(element.Name)) // Handling ascending indexes as an example
    //                );

    //                // Create the index using the IndexKeysDefinition and the index options
    //                targetCollection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(indexDefinition, new CreateIndexOptions
    //                {
    //                    Name = indexOptions["name"].ToString()
    //                }));
    //            }


    //            Console.WriteLine($"Collection {collectionName} duplicated successfully.");
    //        }

    //        Console.WriteLine("Database duplication completed.");
    //    }
    //}
}
