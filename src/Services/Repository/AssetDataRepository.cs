using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Diagnostics;
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
    public interface IAssetDataRepository
    {
        Task<bool> DeleteAsync(Guid id);

        Task<bool> InsertAsync(AssetDataServiceModel assetData);

        Task<List<AssetDataServiceModel>> GetDataByQueryString(string query);
    }
    public class AssetDataRepository: IAssetDataRepository
    {

        private readonly IServicesConfig config;
        private readonly ILogger log;
        private readonly SystemConfigContext context = null;


        public AssetDataRepository(IServicesConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.context = new SystemConfigContext(config, log);
        }
      

        public async Task<bool> InsertAsync(AssetDataServiceModel assetData)
        {

          await context.AssetData.InsertOneAsync(assetData);

          return true;
          
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var actionResult = await this.context.AssetData.DeleteManyAsync(
                     Builders<AssetDataServiceModel>.Filter.Eq("batchId", id));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<List<AssetDataServiceModel>> GetDataByQueryString(string query)
        {
            var result = await  this.context.AssetData.Aggregate()
                .Match(BsonDocument.Parse(query))
             .ToListAsync();

            return  result;
        }
    }
}
