using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface ITourRepository : IRepository<Tour, Guid>
    {
        Tour FindByName(string name);
        QueryResult<Tour> FindAllToursPagedOf(int? pageNum, int? pageSize);
        QueryResult<Tour> FindAllToursPagedOfByScheduledDate(DateTime scheduledDateTour, int? pageNum, int? pageSize);
        Tour FindByNameSpecifiedDate(string nameTour, DateTime scheduledDateTour);
        IList<Tour> FindAllByScheduledDate(DateTime scheduledDate);
        IList<Tour> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate);
        IList<Tour> FindTodayTourByEmployee(Guid employeeId);
    }
}