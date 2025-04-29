using System;
using System.Collections.Generic;
using System.Reflection;

namespace FacetAPI;

public class FacetCallback<TDelegate> : IFacetCallback<TDelegate> where TDelegate : Delegate
{
    // Store handlers in a list instead of using an event
    private readonly List<TDelegate> _handlers = new();
    private object[] _lastStasisValue;
    private bool _hasStasisValue;
    
    public bool IsReactive { get; }
    public bool IsPaused { get; private set; }

    public FacetCallback(bool reactive = false)
    {
        IsReactive = reactive;
    }

    public void Subscribe(TDelegate handler)
    {
        if (handler == null) return;
        _handlers.Add(handler);
        
        // Immediately invoke with last stasis value if available
        if (_hasStasisValue && IsReactive)
        {
            handler.DynamicInvoke(_lastStasisValue);
        }
    }

    public void Unsubscribe(TDelegate handler)
    {
        _handlers.Remove(handler);
    }

    public void UnsubscribeAll()
    {
        _handlers.Clear();
    }
    
    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void Invoke(params object[] parameters)
    {
        if (IsPaused) return;
        
        foreach (var handler in _handlers.ToArray()) // ToArray for thread-safety
        {
            handler.DynamicInvoke(parameters);
        }
    }

    public void StasisInvoke(params object[] parameters)
    {
        _lastStasisValue = parameters;
        _hasStasisValue = true;
        Invoke(parameters);
    }
}
