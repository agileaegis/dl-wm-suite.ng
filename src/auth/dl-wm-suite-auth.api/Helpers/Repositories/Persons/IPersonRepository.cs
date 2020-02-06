using System;
using System.Collections.Generic;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Persons
{
    public interface IPersonRepository : IRepository<Person, Guid>
    {
        Person FindPersonByEmail(string email);
        IList<Person> FindPersonsByEmailOrLogin(string login);
    }
}
