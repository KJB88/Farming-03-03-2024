using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator // Decouple systems that have a large scope from individual sources, minimising use of the Singleton pattern but maintaining large-scope access by low-scope systems
{
    static Dictionary<string, IService> services = new Dictionary<string, IService>();

    /// <summary>
    /// Add a Service to the Service Locator's collection.
    /// </summary>
    /// <param name="serviceKey">string: The ID used by requests to find this service.</param>
    /// <param name="globalService">GlobalService: The Service itself.</param>
    public static void AddService(string serviceKey, IService globalService)
    {
        serviceKey = serviceKey.ToLower();
        if (services.ContainsKey(serviceKey))
        {
            Debug.Log("Service already registered under this service key.");
            return;
        }
        else
            services.Add(serviceKey, globalService);

        Debug.Log("SERVICE LOCATOR: Service added under type of " + serviceKey);
    }

    /// <summary>
    /// Remove a service from the ServiceLocator.
    /// </summary>
    /// <param name="serviceKey">string: The ID of the Service to remove.</param>
    public static void RemoveService(string serviceKey)
    {
        serviceKey = serviceKey.ToLower();
        if (!services.ContainsKey(serviceKey))
            return;

        services.Remove(serviceKey);
    }

    /// <summary>
    /// Request access to a specific service via its service key.
    /// </summary>
    /// <param name="serviceKey">string: The ID of the Service to be accessed.</param>
    /// <param name="outService">GlobalService: The service to out.</param>
    /// <returns>Bool: Return true if the Service exists with the ServiceLocator. Returns false if the Service does not exist within the ServiceLocator.</returns>
    public static bool RequestService(string serviceKey, out IService? outService)
    {
        outService = null;
        serviceKey = serviceKey.ToLower();

        if (!services.ContainsKey(serviceKey))
            return false;

        outService = services[serviceKey];
        return true;
    }
}