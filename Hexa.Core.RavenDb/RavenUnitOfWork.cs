#region Header

// ===================================================================================
// Copyright 2010 HexaSystems Corporation
// ===================================================================================
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// ===================================================================================
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// See the License for the specific language governing permissions and
// ===================================================================================

#endregion Header

namespace Hexa.Core.Domain
{
    using System;
    using System.ComponentModel.Composition;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    [Export(typeof(IUnitOfWork))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RavenUnitOfWork : IRavenUnitOfWork
    {
        #region Fields

        private bool disposed;
        DocumentStore documentStore;
        private IDocumentSession session;

        #endregion Fields

        #region Constructors

        public RavenUnitOfWork(DocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        #endregion Constructors

        #region Properties

        public IDocumentSession Session
        {
            get
            {
                return this.session;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            Guard.IsNotNull(entity, "entity");
            this.Session.Store(entity);
        }

        public void Attach<TEntity>(TEntity entity)
            where TEntity : class
        {
        }

        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        public void Commit()
        {
            this.session.SaveChanges();
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            Guard.IsNotNull(entity, "entity");
            this.Session.Delete(entity);
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Modifies the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Modify<TEntity>(TEntity entity)
            where TEntity : class
        {
        }

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public System.Linq.IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            return this.Session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow());
        }

        public void Start()
        {
            this.session = documentStore.OpenSession();
        }

        public void Start(System.Data.IsolationLevel isolationLevel)
        {
            this.session = documentStore.OpenSession();
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (this.session != null)
                {
                    this.session.Dispose();

                    this.session = null;
                }

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion Methods
    }
}