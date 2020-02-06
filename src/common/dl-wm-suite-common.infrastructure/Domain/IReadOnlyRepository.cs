using System.Collections.Generic;

namespace dl.wm.suite.common.infrastructure.Domain
{
    public interface IReadOnlyRepository<T, in TId>
        where T : IAggregateRoot
    {
        T FindBy(TId id);
        IList<T> FindAll();
        IList<T> FindAll(int index, int count);
    }
}
