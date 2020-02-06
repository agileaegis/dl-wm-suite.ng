using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IContainerRepository : IRepository<Container, Guid>
    {
        Container FindOneByName(string name);
        QueryResult<Container> FindAllContainersPagedOf(int? pageNum, int? pageSize);
        QueryResult<Container> FindAllContainersPagedOfByScheduledDate(DateTime scheduledDateContainer, int? pageNum, int? pageSize);
        Container FindByNameSpecifiedDate(string nameContainer, DateTime scheduledDateContainer);
        IList<Container> FindAllByScheduledDate(DateTime scheduledDate);
        IList<Container> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate);
    }
}