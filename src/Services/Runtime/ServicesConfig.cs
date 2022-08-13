// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime
{
    public interface IServicesConfig
    {
        string MongoDBConnectionString { get; set; }
        string MongoDBDatabaseName { get; set; }
    }

    public class ServicesConfig : IServicesConfig
    {
        public string MongoDBConnectionString { get; set; }
        public string MongoDBDatabaseName { get; set; }
    }
}
