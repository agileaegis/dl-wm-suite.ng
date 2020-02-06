using System.Collections.Generic;

namespace dl.wm.suite.common.infrastructure.Domain.Events
{
    public interface IDomainEventHandlerFactory
    {
        IEnumerable<IDomainEventHandler<T>> GetDomainEventHandlersFor<T>(T domainEvent)
            where T : IDomainEvent;
    }
}