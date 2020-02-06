using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class DepartmentRepository : RepositoryBase<Department, Guid>, IDepartmentRepository
    {
        public DepartmentRepository(ISession session)
            : base(session)
        {
        }

        public QueryResult<Department> FindAllDepartmentsPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Department>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Department>(query?
                    .Where(r => r.IsActive == true)
                    .List().AsQueryable());
            }

            return new QueryResult<Department>(query
                        .Where(r => r.IsActive == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                        .Take((int) pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int) pageSize)
                ;
        }

        public Department FindOneByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Employee> FindAllActiveDepartments(bool active)
        {
            throw new NotImplementedException();
        }
    }
}
