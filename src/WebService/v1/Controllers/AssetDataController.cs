using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Exceptions;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Repository;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Filters;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Controllers
{
    [Route(Version.PATH + "/[controller]"), TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class AssetDataController : Controller
    {
     
        private readonly IAssetDataRepository _assetDataService;

        public AssetDataController(IAssetDataRepository assetDataService)
        {
            _assetDataService = assetDataService;
        }

        [HttpPost()]
        [Route("GetDataByQueryString")]
        public async Task<AssetDataListApiModel> GetDataByQueryString([FromBody] queryApiModel json)
        {


            return new AssetDataListApiModel( await this._assetDataService.GetDataByQueryString(json.query));
        }

        [HttpPost]
        public async Task<bool> PostAsync(  [FromBody] List<AssetDataApiModel> assetsData)
        {

            if (assetsData == null)
            {
                throw new InvalidInputException("data not provided in request body.");
            }
            foreach (var data in assetsData)
            {
                var Isucess = await this._assetDataService.InsertAsync(data.ToServiceModel());
            }

            return true;
        }

        [HttpPost()]
        [Route("DeleteBulkBatches")]
        public async Task DeleteAsync([FromBody] List<Guid> ids)
        {
            foreach (var id in ids)
            {
                await this._assetDataService.DeleteAsync(id);
            }
        }
    }
}
