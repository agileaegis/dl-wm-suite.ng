﻿namespace dl.wm.suite.common.infrastructure.Domain.Events
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}