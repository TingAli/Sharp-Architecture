namespace Tests.SharpArch.NHibernate.MultiDb
{
    using System;
    using global::SharpArch.Domain.DomainModel;
    using global::SharpArch.Domain.PersistenceSupport;
    using global::SharpArch.NHibernate;
    using global::SharpArch.NHibernate.Impl;
    using global::SharpArch.NHibernate.MultiDb;
    using JetBrains.Annotations;


    /// <summary>
    ///     Prototype.
    /// </summary>
    public class TaggedNHibernateRepository<TEntity, TId> : NHibernateRepository<TEntity, TId>
        where TEntity : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>
    {
        public TaggedNHibernateRepository([NotNull] ISessionRegistry sessionRegistry, [NotNull] IDatabaseIdentifierProvider keyProvider)
            : base(GetTransactionManager(sessionRegistry, keyProvider))
        {
        }

        static INHibernateTransactionManager GetTransactionManager(
            [NotNull] ISessionRegistry sessionRegistry, [NotNull] IDatabaseIdentifierProvider keyProvider)
        {
            if (sessionRegistry == null) throw new ArgumentNullException(nameof(sessionRegistry));
            if (keyProvider == null) throw new ArgumentNullException(nameof(keyProvider));

            return sessionRegistry.GetTransactionManager(keyProvider.GetFromType(typeof(TEntity)));
        }
    }
}
