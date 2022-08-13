using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Storage
{
    public class SystemConfigContext
    {
        private readonly IMongoDatabase _database = null;

        public SystemConfigContext(IServicesConfig config, ILogger logger)
        {
            var client = new MongoClient(config.MongoDBConnectionString);
            if (client != null)
                _database = client.GetDatabase(config.MongoDBDatabaseName);
        }


       
        public  IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public IMongoCollection<DocumentsServiceModel> Documents
        {
            get
            {
                return _database.GetCollection<DocumentsServiceModel>("Documents");
            }
        }

        public IMongoCollection<UserInteractionsServiceModel> UserInteractions
        {
            get
            {
                return _database.GetCollection<UserInteractionsServiceModel>("UserInteractions");
            }
        }

        public IMongoCollection<UserLocationServiceModel> UserLocations
        {
            get
            {
                return _database.GetCollection<UserLocationServiceModel>("UserLocation");
            }
        }


        public IMongoCollection<AssetDataServiceModel> AssetData
        {
            get
            {
                return _database.GetCollection<AssetDataServiceModel>("AssetData");
            }
        }
    }
}
