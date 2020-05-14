namespace SharpArch.NHibernate.MultiDb
{
    using System;
    using Domain.DomainModel;
    using Domain.PersistenceSupport;
    using Impl;
    using JetBrains.Annotations;


    /// <summary>
    ///     Prototype.
    /// </summary>
    [PublicAPI]
    public class TaggedNHibernateRepository<TEntity, TId> : NHibernateRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
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
