using System;
using System.Collections;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.fleet.model.Assets;

namespace dl.wm.suite.fleet.repository.ContractRepositories
{
    public interface IAssetRepository : IRepository<Asset, int>
    {
        Asset FindByNumPlate(string numPlate);

        IList<Asset> FindAllActiveAssets();

        QueryResult<Asset> FindAllAssetsPagedOf(int? pageNum = -1, int? pageSize = -1);
        QueryResult<Asset> FindAllActiveAssetsPagedOf(int? pageNum = -1, int? pageSize = -1);
        IList<Asset> FindAtLeastOneByNameOrNumPlate(string name, string numPlate);
        Asset FindOneByNameOrNumPlate(string name, string numPlate);
        Asset FindOneByNumPlate(string numPlate);
    }
}
