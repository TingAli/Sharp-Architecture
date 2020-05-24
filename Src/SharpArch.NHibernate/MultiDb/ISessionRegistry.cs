namespace SharpArch.NHibernate.MultiDb
{
    using System.Collections.Generic;
    using global::NHibernate;
    using JetBrains.Annotations;


    public interface ISessionRegistry
    {
        INHibernateTransactionManager GetTransactionManager([NotNull] string databaseIdentifier);

        /// <summary>
        ///     Creates new <see cref="IStatelessSession" />.
        /// </summary>
        /// <param name="databaseIdentifier">Database identifier.</param>
        /// <returns>New instance of <see cref="IStatelessSession" /></returns>
        /// <remarks>Stateless sessions are not tracked by SessionRegistry and it is client's responsibility to dispose them.</remarks>
        IStatelessSession CreateStatelessSession([NotNull] string databaseIdentifier);

        /// <summary>
        ///     Returns snapshot of all open transactions.
        /// </summary>
        /// <returns>
        ///     Array of <see cref="KeyValuePair{TKey,TValue}" /> of database identifier and
        ///     <see cref="INHibernateTransactionManager" />.
        /// </returns>
        KeyValuePair<string, INHibernateTransactionManager>[] GetExistingTransactionsSnapshot();
    }
}
