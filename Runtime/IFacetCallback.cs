using System;

namespace FacetAPI;

/// <summary>
/// Interface for all facet callbacks, providing basic functionality.
/// </summary>
public interface IFacetCallback
{
    /// <summary>
    /// Gets whether this callback updates in realtime.
    /// </summary>
    bool IsReactive { get; }
    bool IsPaused { get; } // New property
    
    /// <summary>
    /// Invokes the callback with the specified parameters.
    /// </summary>
    /// <param name="parameters">Parameters to pass to the callback.</param>
    void Invoke(params object[] parameters);
    
    /// <summary>
    /// Unsubscribes all handlers from this callback.
    /// </summary>
    void UnsubscribeAll();
    
    void Pause(); // New method
    void Resume(); // New method
}

/// <summary>
/// Generic interface for facet callbacks with specific delegate types.
/// </summary>
/// <typeparam name="TDelegate">The delegate type for this callback.</typeparam>
public interface IFacetCallback<TDelegate> : IFacetCallback where TDelegate : Delegate
{
    /// <summary>
    /// Subscribes a handler to this callback.
    /// </summary>
    /// <param name="handler">The handler to subscribe.</param>
    void Subscribe(TDelegate handler);
    
    /// <summary>
    /// Unsubscribes a specific handler from this callback.
    /// </summary>
    /// <param name="handler">The handler to unsubscribe.</param>
    void Unsubscribe(TDelegate handler);
    
    /// <summary>
    /// Invokes the callback and maintains the value as the persistent state.
    /// New subscribers will immediately receive this value when subscribing.
    /// </summary>
    /// <param name="parameters">Parameters to pass to the callback and store as state.</param>
    void StasisInvoke(params object[] parameters); // New method
}
