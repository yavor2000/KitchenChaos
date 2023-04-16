using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

/// <summary>
/// Simple service locator for <see cref="IGameService"/> instances.
/// </summary>
public class ServiceLocator
{
    private ServiceLocator() { }

    /// <summary>
    /// currently registered services.
    /// </summary>
    private readonly Dictionary<string, IGameService> _services = new Dictionary<string, IGameService>();

    private static ServiceLocator _current;

    /// <summary>
    /// Gets the currently active service locator instance.
    /// </summary>
    public static ServiceLocator Current {
        get
        {
            // Debug.Log("Current");
            if (_current == null)
            {
                Initiailze();
            }

            return _current;
        }
        private set => _current = value;
    }

    /// <summary>
    /// Initalizes the service locator with a new instance.
    /// </summary>
    public static void Initiailze()
    {
        Debug.Log("SL Init");
        Current = new ServiceLocator();
    }

    /// <summary>
    /// Gets the service instance of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the service to lookup.</typeparam>
    /// <returns>The service instance.</returns>
    public T Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        Debug.Log($"Get<{key}>");
        Debug.Log(Util.GetStackTrace(2, -1));
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)_services[key];
    }

    /// <summary>
    /// Registers the service with the current service locator.
    /// </summary>
    /// <typeparam name="T">Service type.</typeparam>
    /// <param name="service">Service instance.</param>
    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        Debug.Log($"Register<{key}>");
        if (_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }

    /// <summary>
    /// Unregisters the service from the current service locator.
    /// </summary>
    /// <typeparam name="T">Service type.</typeparam>
    public void Unregister<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
