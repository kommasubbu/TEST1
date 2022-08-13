// Copyright (c) Microsoft. All rights reserved.

using System;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.Runtime
{
    public interface IConfig
    {
        /// <summary>Web service listening port</summary>
        int Port { get; }

        /// <summary>Service layer configuration</summary>
        IServicesConfig ServicesConfig { get; }
    }

    /// <summary>Web service configuration</summary>
    public class Config : IConfig
    {
        private const string APPLICATION_KEY = "StorageAdapter:";
        private const string PORT_KEY = APPLICATION_KEY + "webservicePort";

        private const string MONGODB_CONNECTION_KEY = "MongoDBConnection:";
        private const string MONGODB_CONNECTION_STRING_KEY = MONGODB_CONNECTION_KEY + "connection_string";
        private const string MONGODB_CONFIG_DATABASE_KEY = MONGODB_CONNECTION_KEY + "database";
        /// <summary>Web service listening port</summary>
        public int Port { get; }

        /// <summary>Service layer configuration</summary>
        public IServicesConfig ServicesConfig { get; }

        public Config(IConfigData configData)
        {
            this.Port = configData.GetInt(PORT_KEY);          

            this.ServicesConfig = new ServicesConfig
            {
                MongoDBConnectionString = configData.GetString(MONGODB_CONNECTION_STRING_KEY),
                MongoDBDatabaseName = configData.GetString(MONGODB_CONFIG_DATABASE_KEY),
            };
        }
    }
}
