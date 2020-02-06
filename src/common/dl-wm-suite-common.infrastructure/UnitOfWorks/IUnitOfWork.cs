namespace dl.wm.suite.common.infrastructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        void Commit();
        void Close();
    }
}
