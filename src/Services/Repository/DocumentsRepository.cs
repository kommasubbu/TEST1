
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Repository
{
    public interface IDocumentsRepository
    {
        Task<bool> DeleteAsync(string id);

        Task<DocumentsServiceModel> GetAsync(string id);

        Task<List<DocumentsServiceModel>> GetListAsync(
           DateTime from,
            DateTime To,
            string order,
            int skip,
            int limit,
             int tenantid
           );
        Task<List<DocumentsServiceModel>> GetAllDocumnetsByUser(
     DateTimeOffset from,
     DateTimeOffset to,
      int userid,
       int tenantid
     );
        Task<List<DocumentsServiceModel>> GetAllAsync();

        Task<DocumentsServiceModel> CreateAsync(DocumentsServiceModel document);

        Task<DocumentsServiceModel> UpsertIfNotDeletedAsync(string id, DocumentsServiceModel document);

    }

    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IServicesConfig config;
        private readonly ILogger log;
        private readonly SystemConfigContext context = null;

        public DocumentsRepository(IServicesConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.context = new SystemConfigContext(config, log);
        }
        public async Task<DocumentsServiceModel> CreateAsync(DocumentsServiceModel document)
        {
            document.DateCreated = DateTime.UtcNow;
            document.DateModified = DateTime.UtcNow;
            await this.context.Documents.InsertOneAsync(document);

            return document;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            DeleteResult actionResult = await this.context.Documents.DeleteOneAsync(
                     Builders<DocumentsServiceModel>.Filter.Eq("Id", internalId));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<List<DocumentsServiceModel>> GetAllAsync()
        {
            var response = await this.context.Documents.Find(_ => true).ToListAsync();

            return new List<DocumentsServiceModel>(response);
        }

        public async Task<List<DocumentsServiceModel>> GetAllDocumnetsByUser(DateTimeOffset from, DateTimeOffset to, int userid, int tenantid)
        {
            DateTime fromdate = from.UtcDateTime;
            DateTime todate = to.UtcDateTime;
            var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day);
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);
            var response = await this.context.Documents.Find(x => x.UserId == userid && x.TenantId == tenantid && x.DateCreated <= FilterEnddate && x.DateCreated >= FilterStartdate)
                .SortByDescending(x => x.DateCreated)
                .ToListAsync();
            return response;

        }

        public async Task<DocumentsServiceModel> GetAsync(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await this.context.Documents
                            .Find(d => d.Id == internalId)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<DocumentsServiceModel>> GetListAsync(DateTime fromdate, DateTime todate, string order, int skip, int limit, int tenantid)
        {
            var FilterStartdate = new DateTime(fromdate.Year, fromdate.Month, fromdate.Day);
            var FilterEnddate = new DateTime(todate.Year, todate.Month, todate.Day);
            var filterBuilder = Builders<DocumentsServiceModel>.Filter;
            var filter = filterBuilder.Gte(x => x.DateCreated, FilterStartdate) & filterBuilder.Lt(x => x.DateCreated, FilterEnddate) &
          filterBuilder.Eq(x => x.TenantId, tenantid);
            var response = await this.context.Documents.Find(filter)
                 // .Skip(skip)
                 //.Limit(limit)
                 .SortByDescending(x=>x.DateCreated)
                 .ToListAsync();
            return new List<DocumentsServiceModel>(response);

        }

        public async Task<DocumentsServiceModel> UpsertIfNotDeletedAsync(string id, DocumentsServiceModel document)
        {
            ObjectId internalId = GetInternalId(id);

            var filter = Builders<DocumentsServiceModel>.Filter.Eq(s => s.Id, internalId);
            var update = Builders<DocumentsServiceModel>.Update
                            .Set(s => s.TenantId, document.TenantId)
                            .Set(s => s.UserId, document.UserId)
                            .Set(s => s.Fields, document.Fields);

            UpdateResult actionResult = await this.context.Documents.UpdateOneAsync(filter, update);
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
