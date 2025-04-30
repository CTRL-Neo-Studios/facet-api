# FacetAPI Library Documentation (llm.md)

## Overview
FacetAPI is a flexible event/callback management library with support for reactive programming, pause/resume functionality, and state persistence. Key features include:
- Generic callback management with type safety
- Reactive callbacks that push latest state to new subscribers
- Pause/resume control for callbacks
- Async condition monitoring helpers
- Stasis invocation (persistent state)

## Core Components

### 1. FacetApi (`FacetAPI.cs`)
Main entry point for callback management.

**Methods**:
```csharp
public IFacetCallback<TDelegate> CreateCallback<TDelegate>(string name, bool reactive = false)
public IFacetCallback<TDelegate> Get<TDelegate>(string name)
public void PauseAll()
public void ResumeAll()
```

### 2. ActionFacet (`ActionFacet.cs`)
Pre-built implementations for common Action signatures.

**Variants**:
- `ActionFacet<T1>`
- `ActionFacet<T1, T2>`

**Usage**:
```csharp
var callback = new ActionFacet<string>(reactive: true);
callback.Invoke("test");
```

### 3. FacetCallback (`FacetCallback.cs`)
Core implementation of callback handling.

**Key Members**:
```csharp
void Subscribe(TDelegate handler)
void Unsubscribe(TDelegate handler)
void StasisInvoke(params object[] parameters)  // Persistent invocation
bool IsReactive { get; }  // New subscribers get last value
bool IsPaused { get; }
```

### 4. FacetExtensions (`FacetExtensions.cs`)
Extension methods for enhanced functionality.

**Methods**:
```csharp
void InvokeIf<T>(this IFacetCallback<Action<T>> callback, T arg, Func<T, bool> condition)
Task WatchAndInvokeAsync<T>(/*...*/)  // Async condition monitoring
```

### 5. Interfaces (`IFacetCallback.cs`)
```csharp
public interface IFacetCallback  // Base interface
public interface IFacetCallback<TDelegate>  // Generic version
```

## Key Concepts

### Reactive Callbacks
- When created with `reactive: true`, new subscribers immediately receive:
    - Last `StasisInvoke` parameters (if any)
    - Last regular invocation parameters (only if created after StasisInvoke)

### Stasis Invocation
- `StasisInvoke` performs two actions:
    1. Stores parameters as persistent state
    2. Triggers normal invocation
- Subsequent subscribers receive stasis parameters immediately if:
    - Callback is reactive
    - Stasis value exists

### Pause/Resume
- Control callback triggering at individual or global level:
```csharp
// Per callback
callback.Pause();
callback.Resume();

// Global
api.PauseAll();
api.ResumeAll();
```

## Usage Examples

### Basic Usage
```csharp
var api = new FacetApi();
var callback = api.CreateCallback<Action<string>>("message", reactive: true);

// Subscribe
callback.Subscribe(msg => Console.WriteLine($"Received: {msg}"));

// Invoke
callback.Invoke("Hello World");

// Stasis invocation
callback.StasisInvoke("Persistent message");
```

### Async Monitoring
```csharp
await callback.WatchAndInvokeAsync(
    arg: "server_up",
    condition: () => CheckServerStatus(),
    checkInterval: TimeSpan.FromSeconds(5)
);
```

### Conditional Invocation
```csharp
callback.InvokeIf(42, x => x > 0);  // Only invokes if value is positive
```

## Important Notes

1. **Parameter Matching**:
    - Ensure invocation parameters match delegate type signatures
    - Mismatches will throw `ArgumentException` during DynamicInvoke

2. **Thread Safety**:
    - Uses `ToArray()` in Invoke for thread-safe enumeration
    - Concurrent modifications during invocation are safe but might not be atomic

3. **Performance**:
    - Prefer strongly-typed ActionFacet over `FacetDelegate` for better performance
    - `DynamicInvoke` has overhead compared to direct invocation

4. **Lifetime Management**:
    - Always unsubscribe handlers when no longer needed
    - Use `UnsubscribeAll()` to clear all handlers

## Best Practices

- **Naming**: Use descriptive names when creating callbacks
- **Reactive Mode**: Enable for stateful components that need latest values
- **Stasis**: Use for initialization data or important persistent state
- **Extensions**: Leverage `WatchAndInvokeAsync` for polling scenarios
