using System;
using System.Reflection;
using System.Runtime.Loader;

namespace SquawkBus.Distributor.Plugins
{
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            var expandedPluginPath = Environment.ExpandEnvironmentVariables(pluginPath);
            _resolver = new AssemblyDependencyResolver(expandedPluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath == null)
                return null;

            var assembly = LoadFromAssemblyPath(assemblyPath);
            return assembly;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath == null)
                return IntPtr.Zero;

            var unmanagedDll = LoadUnmanagedDllFromPath(libraryPath);
            return unmanagedDll;
        }
    }
}
