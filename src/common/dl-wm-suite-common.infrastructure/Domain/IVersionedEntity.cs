namespace dl.wm.suite.common.infrastructure.Domain
{
    public interface IVersionedEntity
    {
        int Revision { get; set; }
    }
}