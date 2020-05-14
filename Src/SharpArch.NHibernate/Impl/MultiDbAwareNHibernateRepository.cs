namespace SharpArch.NHibernate.Impl
{
    using System;
    using Domain.DomainModel;
    using Domain.PersistenceSupport;
    using global::NHibernate;
    using JetBrains.Annotations;
    using MultiDb;


    /// <summary>
    ///     Base NHibernate repository with multi-database support.
    /// </summary>
    /// <typeparam name="TEntity">Entity type./</typeparam>
    /// <typeparam name="TId">Entity identifier type.</typeparam>
    /// <remarks>
    ///     <para>
    ///         Repository resolves correct <see cref="ISession" /> based on the entity type
    ///         using <see cref="IDatabaseIdentifierProvider" /> and <see cref="ISessionRegistry" />.
    ///     </para>
    /// </remarks>
    [PublicAPI]
    public class MultiDbAwareNHibernateRepository<TEntity, TId> : NHibernateRepositoryBase<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        ///     Creates instance of the repository.
        /// </summary>
        /// <param name="sessionRegistry"></param>
        /// <param name="databaseIdentifierProvider"></param>
        public MultiDbAwareNHibernateRepository(ISessionRegistry sessionRegistry, IDatabaseIdentifierProvider databaseIdentifierProvider)
            : base(GetTransactionManager(sessionRegistry, databaseIdentifierProvider))
        {
        }

        static INHibernateTransactionManager GetTransactionManager(
            [NotNull] ISessionRegistry sessionRegistry, [NotNull] IDatabaseIdentifierProvider databaseIdentifierProvider)
        {
            if (sessionRegistry == null) throw new ArgumentNullException(nameof(sessionRegistry));
            if (databaseIdentifierProvider == null) throw new ArgumentNullException(nameof(databaseIdentifierProvider));

            var databaseIdentifier = databaseIdentifierProvider.GetFromType(typeof(TEntity));
            return sessionRegistry.GetTransactionManager(databaseIdentifier);
        }
    }
}
