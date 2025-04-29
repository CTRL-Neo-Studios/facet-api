using System;
using System.Collections.Generic;

namespace FacetAPI.Core
{
    public class FacetApi
    {
        private readonly Dictionary<string, IFacetCallback> callbacks = new();

        public void PauseAll()
        {
            foreach (var callback in callbacks.Values)
                callback.Pause();
        }

        public void ResumeAll()
        {
            foreach (var callback in callbacks.Values)
                callback.Resume();
        }

        public IFacetCallback<TDelegate> CreateCallback<TDelegate>(string name, bool reactive = false)
            where TDelegate : Delegate
        {
            var callback = new FacetCallback<TDelegate>(reactive);
            callbacks[name] = callback;
            return callback;
        }

        public IFacetCallback<TDelegate> Get<TDelegate>(string name) where TDelegate : Delegate
            => (IFacetCallback<TDelegate>)callbacks[name];
    }
}