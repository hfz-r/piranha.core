/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Piranha.Runtime
{
    public sealed class HookManager
    {
        /// <summary>
        /// The standard repository hooks for a data model.
        /// </summary>
        public sealed class RepositoryHooks<T>
        {
            /// <summary>
            /// Registers a new hook to be executed after the model
            /// has been loaded but BEFORE it has been added into
            /// the cache.
            /// </summary>
            /// <param name="hook">The hook</param>
            public void RegisterOnLoad(ModelDelegate<T> hook)
            {
                App.Hooks.RegisterOnLoad<T>(hook);
            }

            /// <summary>
            /// Registers a new hook to be executed before the model
            /// is saved to the database.
            /// </summary>
            /// <param name="hook">The hook</param>
            public void RegisterOnBeforeSave(ModelDelegate<T> hook)
            {
                App.Hooks.RegisterOnBeforeSave<T>(hook);
            }

            /// <summary>
            /// Registers a new hook to be executed after the model
            /// is saved to the database.
            /// </summary>
            /// <param name="hook">The hook</param>
            public void RegisterOnAfterSave(ModelDelegate<T> hook)
            {
                App.Hooks.RegisterOnAfterSave<T>(hook);
            }

            /// <summary>
            /// Registers a new hook to be executed before the model
            /// is deleted from the database.
            /// </summary>
            /// <param name="hook">The hook</param>
            public void RegisterOnBeforeDelete(ModelDelegate<T> hook)
            {
                App.Hooks.RegisterOnBeforeDelete<T>(hook);
            }

            /// <summary>
            /// Registers a new hook to be executed after the model
            /// is deleted from the database.
            /// </summary>
            /// <param name="hook">The hook</param>
            public void RegisterOnAfterDelete(ModelDelegate<T> hook)
            {
                App.Hooks.RegisterOnAfterDelete<T>(hook);
            }

            /// <summary>
            /// Removes all registered hooks.
            /// </summary>
            public void Clear()
            {
                App.Hooks.Clear<T>();
            }
        }

        //
        // Private hook collections.
        //
        private readonly Dictionary<Type, object> _onLoad;
        private readonly Dictionary<Type, object> _onBeforeSave;
        private readonly Dictionary<Type, object> _onAfterSave;
        private readonly Dictionary<Type, object> _onBeforeDelete;
        private readonly Dictionary<Type, object> _onAfterDelete;

        /// <summary>
        /// Delegate for repository events.
        /// </summary>
        public delegate void ModelDelegate<T>(T model);

        /// <summary>
        /// Delegate for generating a slug.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public delegate string SlugDelegate(string str);

        /// <summary>
        /// Gets the hooks available for aliases.
        /// </summary>
        public RepositoryHooks<Data.Alias> Alias { get; private set; }

        /// <summary>
        /// Gets the hooks available for categories.
        /// </summary>
        public RepositoryHooks<Data.Category> Category { get; private set; }

        /// <summary>
        /// Gets the hooks available for media.
        /// </summary>
        public RepositoryHooks<Data.Media> Media { get; private set; }

        /// <summary>
        /// Gets the hooks available for media folders.
        /// </summary>
        public RepositoryHooks<Data.MediaFolder> MediaFolder { get; private set; }

        /// <summary>
        /// Gets the hooks available for params.
        /// </summary>
        public RepositoryHooks<Data.Param> Param { get; private set; }

        /// <summary>
        /// Gets the hooks available for sites.
        /// </summary>
        public RepositoryHooks<Data.Site> Site { get; private set; }

        /// <summary>
        /// Gets the hooks available for tags.
        /// </summary>
        public RepositoryHooks<Data.Tag> Tag { get; private set; }

        /// <summary>
        /// Gets the hook for slug generation.
        /// </summary>
        public SlugDelegate OnGenerateSlug;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HookManager()
        {
            _onLoad = new Dictionary<Type, object>();
            _onBeforeSave = new Dictionary<Type, object>();
            _onAfterSave = new Dictionary<Type, object>();
            _onBeforeDelete = new Dictionary<Type, object>();
            _onAfterDelete = new Dictionary<Type, object>();

            Alias = new RepositoryHooks<Data.Alias>();
            Category = new RepositoryHooks<Data.Category>();
            Media = new RepositoryHooks<Data.Media>();
            MediaFolder = new RepositoryHooks<Data.MediaFolder>();
            Param = new RepositoryHooks<Data.Param>();
            Site = new RepositoryHooks<Data.Site>();
            Tag = new RepositoryHooks<Data.Tag>();
        }

        /// <summary>
        /// Removes all hooks for the specified model type.
        /// </summary>
        internal void Clear<T>()
        {
            if (_onLoad.ContainsKey(typeof(T)))
                _onLoad.Remove(typeof(T));
            if (_onBeforeSave.ContainsKey(typeof(T)))
                _onBeforeSave.Remove(typeof(T));
            if (_onAfterSave.ContainsKey(typeof(T)))
                _onAfterSave.Remove(typeof(T));
            if (_onBeforeDelete.ContainsKey(typeof(T)))
                _onBeforeDelete.Remove(typeof(T));
            if (_onAfterDelete.ContainsKey(typeof(T)))
                _onAfterDelete.Remove(typeof(T));
        }

        /// <summary>
        /// Registers a new hook to be executed after the model
        /// has been loaded but BEFORE it has been added into
        /// the cache.
        /// </summary>
        /// <param name="hook">The hook</param>
        internal void RegisterOnLoad<T>(ModelDelegate<T> hook)
        {
            if (!_onLoad.ContainsKey(typeof(T)))
                _onLoad[typeof(T)] = new List<ModelDelegate<T>>();
            ((List<ModelDelegate<T>>)_onLoad[typeof(T)]).Add(hook);
        }

        /// <summary>
        /// Registers a new hook to be executed before the model
        /// is saved to the database.
        /// </summary>
        /// <param name="hook">The hook</param>
        internal void RegisterOnBeforeSave<T>(ModelDelegate<T> hook)
        {
            if (!_onBeforeSave.ContainsKey(typeof(T)))
                _onBeforeSave[typeof(T)] = new List<ModelDelegate<T>>();
            ((List<ModelDelegate<T>>)_onBeforeSave[typeof(T)]).Add(hook);
        }

        /// <summary>
        /// Registers a new hook to be executed after the model
        /// is saved to the database.
        /// </summary>
        /// <param name="hook">The hook</param>
        internal void RegisterOnAfterSave<T>(ModelDelegate<T> hook)
        {
            if (!_onAfterSave.ContainsKey(typeof(T)))
                _onAfterSave[typeof(T)] = new List<ModelDelegate<T>>();
            ((List<ModelDelegate<T>>)_onAfterSave[typeof(T)]).Add(hook);
        }

        /// <summary>
        /// Registers a new hook to be executed before the model
        /// is deleted from the database.
        /// </summary>
        /// <param name="hook">The hook</param>
        internal void RegisterOnBeforeDelete<T>(ModelDelegate<T> hook)
        {
            if (!_onBeforeDelete.ContainsKey(typeof(T)))
                _onBeforeDelete[typeof(T)] = new List<ModelDelegate<T>>();
            ((List<ModelDelegate<T>>)_onBeforeDelete[typeof(T)]).Add(hook);
        }

        /// <summary>
        /// Registers a new hook to be executed after the model
        /// is deleted from the database.
        /// </summary>
        /// <param name="hook">The hook</param>
        internal void RegisterOnAfterDelete<T>(ModelDelegate<T> hook)
        {
            if (!_onAfterDelete.ContainsKey(typeof(T)))
                _onAfterDelete[typeof(T)] = new List<ModelDelegate<T>>();
            ((List<ModelDelegate<T>>)_onAfterDelete[typeof(T)]).Add(hook);
        }

        /// <summary>
        /// Executes the registered hooks on the given model.
        /// </summary>
        /// <param name="model">The model</param>
        internal void OnLoad<T>(T model)
        {
            if (_onLoad.ContainsKey(typeof(T)))
            {
                var hooks = (List<ModelDelegate<T>>)_onLoad[typeof(T)];

                foreach (var hook in hooks)
                    hook.Invoke(model);
            }
        }

        /// <summary>
        /// Executes the registered hooks on the given model.
        /// </summary>
        /// <param name="model">The model</param>
        internal void OnBeforeSave<T>(T model)
        {
            if (_onBeforeSave.ContainsKey(typeof(T)))
            {
                var hooks = (List<ModelDelegate<T>>)_onBeforeSave[typeof(T)];

                foreach (var hook in hooks)
                    hook.Invoke(model);
            }
        }

        /// <summary>
        /// Executes the registered hooks on the given model.
        /// </summary>
        /// <param name="model">The model</param>
        internal void OnAfterSave<T>(T model)
        {
            if (_onAfterSave.ContainsKey(typeof(T)))
            {
                var hooks = (List<ModelDelegate<T>>)_onAfterSave[typeof(T)];

                foreach (var hook in hooks)
                    hook.Invoke(model);
            }
        }

        /// <summary>
        /// Executes the registered hooks on the given model.
        /// </summary>
        /// <param name="model">The model</param>
        internal void OnBeforeDelete<T>(T model)
        {
            if (_onBeforeDelete.ContainsKey(typeof(T)))
            {
                var hooks = (List<ModelDelegate<T>>)_onBeforeDelete[typeof(T)];

                foreach (var hook in hooks)
                    hook.Invoke(model);
            }
        }

        /// <summary>
        /// Executes the registered hooks on the given model.
        /// </summary>
        /// <param name="model">The model</param>
        internal void OnAfterDelete<T>(T model)
        {
            if (_onAfterDelete.ContainsKey(typeof(T)))
            {
                var hooks = (List<ModelDelegate<T>>)_onAfterDelete[typeof(T)];

                // Execute all of the available hooks.
                foreach (var hook in hooks)
                    hook.Invoke(model);
            }
        }
    }
}
