using System;
using System.Collections.Generic;

namespace DeviceDetectorNET.Parser.Client
{
    public class FactoryClient<T>
    {
        private FactoryClient() { }

        private static readonly Dictionary<string, Func<T>> Clients
            = new Dictionary<string, Func<T>>();

        public static T Create(string name)
        {
            if (Clients.TryGetValue(name, out var constructor))
                return constructor();

            throw new ArgumentException("No type registered for this id");
        }

        public static void Register(string name, Func<T> ctor)
        {
            Clients.Add(name, ctor);
        }
    }
}