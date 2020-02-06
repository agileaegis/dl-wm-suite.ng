using dl.wm.suite.common.infrastructure.Domain;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.auth.api.Helpers.Mappings.Base
{
    public abstract class VersionedClassMap<T> : ClassMap<T> where T : IVersionedEntity
    {
        protected VersionedClassMap()
        {
            Version(x => x.Revision)
                .Column("revision")
                .CustomSqlType("integer")
                .Generated.Always()
                .UnsavedValue("null");
        }
    }
}