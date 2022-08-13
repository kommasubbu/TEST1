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

    public interface IUserInteractionsRepository
    {
        Task<bool> DeleteAsync(string id);

        Task<UserInteractionsServiceModel> GetAsync(string id);

        Task<List<UserInteractionsServiceModel>> GetListAsync(
            DateTime from,
            DateTime to,
            string order,
            int skip,
            int limit,
             int tenantid
           );
        Task<int> GetUserViolationCount(
         DateTimeOffset from,
         DateTimeOffset to,
          int userid,
           int tenantid
         );
        Task<List<UserInteractionsServiceModel>> GetAllInteractionByUser(
       DateTimeOffset from,
       DateTimeOffset to,
        int userid,
         int tenantid
       );
        Task<List<UserInteractionsServiceModel>> GetAllAsync();

        Task<UserInteractionsServiceModel> CreateAsync(UserInteractionsServiceModel document);

        Task<UserInteractionsServiceModel> UpsertIfNotDeletedAsync(string id, UserInteractionsServiceModel document);

    }
    public class UserInteractionsRepository : IUserInteractionsRepository
    {
        private readonly IServicesConfig config;
        private readonly ILogger log;
        private readonly SystemConfigContext context = null;

        public UserInteractionsRepository(IServicesConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.context = new SystemConfigContext(config, log);
        }
        public async Task<UserInteractionsServiceModel> CreateAsync(UserInteractionsServiceModel document)
        {
            document.DetectedTime = DateTime.Now;
            await this.context.UserInteractions.InsertOneAsync(document);

            return document;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            DeleteResult actionResult = await this.context.UserInteractions.DeleteOneAsync(
                     Builders<UserInteractionsServiceModel>.Filter.Eq("Id", internalId));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<List<UserInteractionsServiceModel>> GetAllAsync()
        {
            var response = await this.context.UserInteractions.Find(_ => true).ToListAsync();

            return new List<UserInteractionsServiceModel>(response);
        }

        public async Task<List<UserInteractionsServiceModel>> GetAllInteractionByUser(DateTimeOffset from, DateTimeOffset to, int userid, int tenantid)
        {
            DateTime fromdate = from.UtcDateTime;
            DateTime todate = to.UtcDateTime;
                    var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day);
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);
            var response =await  this.context.UserInteractions.Find(x => x.UserId == userid && x.TenantId == tenantid && x.DetectedTime <= FilterEnddate && x.DetectedTime >= FilterStartdate)
                .SortByDescending(x => x.DateCreated)
                .ToListAsync();
            return response;
        }

        public async Task<UserInteractionsServiceModel> GetAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await this.context.UserInteractions
                            .Find(d => d.Id == internalId)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<UserInteractionsServiceModel>> GetListAsync(DateTime fromdate, DateTime todate, string order, int skip, int limit, int tenantid)
        {
            var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day);
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);

            var filterBuilder = Builders<UserInteractionsServiceModel>.Filter;
            var filter = filterBuilder.Gte(x =>x.DetectedTime, FilterStartdate) & filterBuilder.Lt(x => x.DetectedTime, FilterEnddate) & filterBuilder.Eq(x => x.TenantId, tenantid);
            var response = await this.context.UserInteractions.Find(filter)
               .SortByDescending(x=>x.DateCreated)
                .ToListAsync();
            return new List<UserInteractionsServiceModel>(response);
        }

        public async Task<int> GetUserViolationCount(DateTimeOffset from, DateTimeOffset to, int userid, int tenantid)
        {
            DateTime fromadate = from.UtcDateTime;
            DateTime todate = to.UtcDateTime;
            var response =  this.context.UserInteractions.Find(x => x.UserId== userid && x.TenantId == tenantid && x.DetectedTime <= todate && x.DetectedTime >= fromadate)             
                .ToList();
            return response.Count;
        }

        public async Task<UserInteractionsServiceModel> UpsertIfNotDeletedAsync(string id, UserInteractionsServiceModel document)
        {
            ObjectId internalId = GetInternalId(id);

            var filter = Builders<UserInteractionsServiceModel>.Filter.Eq(s => s.Id, internalId);
            var update = Builders<UserInteractionsServiceModel>.Update
                            .Set(s => s.TenantId, document.TenantId)
                            .Set(s => s.UserId, document.UserId)
                            .Set(s => s.DetectedDistance, document.DetectedDistance)
                            .Set(s => s.DetectedTime, document.DetectedTime)
                            .Set(s => s.DetectedUserId, document.DetectedUserId)
                            .Set(s => s.UserDetectionState, document.UserDetectionState)
                            .Set(s => s.UserDetectionState, document.UserDetectionState)
                            .Set(s => s.DetectedDistance, document.DetectedDistance);

            UpdateResult actionResult = await this.context.UserInteractions.UpdateOneAsync(filter, update);
            document.Id = internalId;
            return document;
        }
        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}
