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
    public interface IUserLocationsRepository
    {
        Task<bool> DeleteAsync(string id);

        Task<UserLocationServiceModel> GetAsync(string id);

        Task<List<UserLocationServiceModel>> GetListAsync(
            DateTime from,
            DateTime To,
            string order,
            int skip,
            int limit,
            int tenantid
           );
        Task<List<UserLocationServiceModel>> GetAllLocationsByUser(
            DateTimeOffset from,
            DateTimeOffset to,
            int userid,
            int tenantid
            );
        Task<List<UserLocationServiceModel>> GetAllAsync();

        Task<UserLocationServiceModel> CreateAsync(UserLocationServiceModel document);

        Task<UserLocationServiceModel> UpsertIfNotDeletedAsync(string id, UserLocationServiceModel document);
    }

    public class UserLocationsRepository : IUserLocationsRepository
    {
        private readonly IServicesConfig config;
        private readonly ILogger log;
        private readonly SystemConfigContext context = null;

        public UserLocationsRepository(IServicesConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.context = new SystemConfigContext(config, log);
        }
        public async Task<UserLocationServiceModel> CreateAsync(UserLocationServiceModel document)
        {
            document.DateCreated = DateTime.UtcNow;
            document.DateModified = DateTime.UtcNow;
            await this.context.UserLocations.InsertOneAsync(document);

            return document;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            DeleteResult actionResult = await this.context.UserLocations.DeleteOneAsync(
                     Builders<UserLocationServiceModel>.Filter.Eq("Id", internalId));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<List<UserLocationServiceModel>> GetAllAsync()
        {
            var response = await this.context.UserLocations.Find(_ => true).ToListAsync();

            return new List<UserLocationServiceModel>(response);
        }

        public async Task<List<UserLocationServiceModel>> GetAllLocationsByUser(DateTimeOffset from, DateTimeOffset to, int userid, int tenantid)
        {
            DateTime fromdate = from.UtcDateTime;
            DateTime todate = to.UtcDateTime;
            var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day);
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);

            var response = await this.context.UserLocations.Find(x => x.UserId == userid && x.TenantId == tenantid && x.DateCreated <= FilterEnddate && x.DateCreated >= FilterStartdate)
                 .SortByDescending(x => x.DateCreated)
                .ToListAsync();
            return response;
        }

        public async Task<UserLocationServiceModel> GetAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await this.context.UserLocations
                            .Find(d => d.Id == internalId)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<UserLocationServiceModel>> GetListAsync(DateTime fromdate, DateTime todate, string order, int skip, int limit, int tenantid)
        {
            var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day );
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);
            var filterBuilder = Builders<UserLocationServiceModel>.Filter;
            var filter = filterBuilder.Gte(x => x.DateCreated, FilterStartdate) & filterBuilder.Lt(x => x.DateCreated, FilterEnddate) &
          filterBuilder.Eq(x => x.TenantId, tenantid);
            var response = await this.context.UserLocations.Find(filter)
                .SortByDescending(x=>x.DateCreated)
                 .ToListAsync();
            return new List<UserLocationServiceModel>(response);


        }

        public async Task<UserLocationServiceModel> UpsertIfNotDeletedAsync(string id, UserLocationServiceModel document)
        {
            ObjectId internalId = GetInternalId(id);

            var filter = Builders<UserLocationServiceModel>.Filter.Eq(s => s.Id, internalId);
            var update = Builders<UserLocationServiceModel>.Update
                            .Set(s => s.TenantId, document.TenantId)
                            .Set(s => s.UserId, document.UserId)
                            .Set(s => s.Latitude, document.Latitude)
                            .Set(s => s.Longitude, document.Longitude)
                            .Set(s => s.Location, document.Location);

            UpdateResult actionResult = await this.context.UserLocations.UpdateOneAsync(filter, update);
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
