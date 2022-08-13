using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Exceptions;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Repository
{
    public interface IKeyValueContainer
    {

        //Task<ValueServiceModel> CheckCreateDatabaseIfNotExistsAsync();
        /// <summary>
        /// Get single key-value pair
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="key">Key</param>
        /// <returns>Key-value pair including key, data, etag and timestamp</returns>
        Task<ValueServiceModel> GetAsync(string collectionId, string key);

        /// <summary>
        /// Get all key-value pairs in given collection
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <returns>List of key-value pairs</returns>
        Task<IEnumerable<ValueServiceModel>> GetAllAsync(string collectionId,int tenantId);

        /// <summary>
        /// Create key-value pair
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="key">Key</param>
        /// <param name="input">Data</param>
        /// <returns>Created key-value pair</returns>
        Task<ValueServiceModel> CreateAsync(string collectionId, ValueServiceModel input);

        /// <summary>
        /// Update key-value pair (create if pair does not exist)
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="key">Key</param>
        /// <param name="input">Data plus etag</param>
        /// <returns>Updated key-value pair</returns>
        Task<ValueServiceModel> UpsertAsync(string collectionId, string key, ValueServiceModel input);

        /// <summary>
        /// Delete key-value pair
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        Task DeleteAsync(string collectionId, string key);

        /// <summary>
        /// Delete mutiple 
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="query">query</param>
        /// <returns></returns>
        Task<bool> DeleteAsyncWithQuery(string collectionId, string query);

    }

   public class ValuesRepository : IKeyValueContainer
    {
        private readonly IServicesConfig config;
        private readonly ILogger log;
        private readonly SystemConfigContext context = null;      
        private readonly IMongoDatabase database;

        public ValuesRepository(         
            IServicesConfig config,
            ILogger logger)
        {
            this.log = logger;
            this.context = new SystemConfigContext(config, log);
            this.database = this.context.GetDatabase();
        }

        public async Task<ValueServiceModel> GetAsync(string collectionId, string key)
        {

            try
            {
                var collection = database.GetCollection<ValueServiceModel>(collectionId);
                var response = collection.Find(a => a.key == key).FirstOrDefault();
                return response;

            }
            catch (Exception ex)
            {
                throw new ResourceNotFoundException(ex.Message);
            }
        }

        public async Task<IEnumerable<ValueServiceModel>> GetAllAsync(string CollectionName,int tenantId)
        {
            var collection = database.GetCollection<ValueServiceModel>(CollectionName);

            var result = await collection.Find(a=>a.tenantId== tenantId).ToListAsync();
            return result;

        }

        public async Task<ValueServiceModel> CreateAsync(string collectionId, ValueServiceModel model)
        {

            try
            {
                var collection = database.GetCollection<ValueServiceModel>(collectionId);
                await collection.InsertOneAsync(model);
                return model;

            }
            catch (Exception ex)
            {
                throw new ConflictingResourceException(ex.Message);
            }
        }

        public async Task<ValueServiceModel> UpsertAsync(string collectionId, string key, ValueServiceModel input)
        {

            try
            {
                var collection = database.GetCollection<ValueServiceModel>(collectionId);
                var _object = await collection.Find(a => a.key == key).FirstOrDefaultAsync();
                input.objectid = _object.objectid;
                var response = await collection.ReplaceOneAsync(a => a.key == key, input);
                return input;
            }
            catch (Exception ex)
            {
              
                throw new ConflictingResourceException(ex.Message);
            }
        }

        public async Task DeleteAsync(string collectionId, string key)
        {

            try
            {
                var collection = database.GetCollection<ValueServiceModel>(collectionId);
                var response = collection.DeleteMany(a => a.key == key);
            }
            catch (Exception ex)
            {
               
               Console.WriteLine("Key does not exist, nothing to do");
            }
        }



        public async Task<bool> DeleteAsyncWithQuery(string collectionId, string query)
        {

            try
            {
                var collection = database.GetCollection<ValueServiceModel>(collectionId);
                FilterDefinition<ValueServiceModel> filter = query;
                var actionResult = await collection.DeleteManyAsync(filter);

                return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                return false;
                Console.WriteLine("Key does not exist, nothing to do");
            }
        }

    }
}
