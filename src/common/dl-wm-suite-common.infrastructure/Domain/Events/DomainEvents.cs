namespace dl.wm.suite.common.infrastructure.Domain.Events
{
    public static class DomainEvents
    {
        public static IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public static void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            DomainEventHandlerFactory.GetDomainEventHandlersFor(domainEvent)
                .ForEach(h => h.Handle(domainEvent));
        }
    }
}