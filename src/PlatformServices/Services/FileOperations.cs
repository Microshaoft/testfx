// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices
{
    using System;
    using System.IO;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.Interface;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    /// <summary>
    /// The file operations.
    /// </summary>
    public class FileOperations : IFileOperations
    {
        private readonly bool _isPackaged;

        public FileOperations()
        {
            this._isPackaged = AppContainer.AppModel.IsPackagedProcess();
        }

        /// <summary>
        /// Loads an assembly.
        /// </summary>
        /// <param name="assemblyFileName"> The assembly name. </param>
        /// <param name="isReflectionOnly">Indicates whether this should be a reflection only load.</param>
        /// <returns> The <see cref="Assembly"/>. </returns>
        public Assembly LoadAssembly(string assemblyFileName, bool isReflectionOnly)
        {
#if NET6_0_OR_GREATER
            return Assembly.LoadFrom(assemblyFileName);
#else
    #if !NETSTANDARD1_4
            if (!_isPackaged && Path.IsPathRooted(assemblyFileName))
            {
                return isReflectionOnly
                    ? Assembly.ReflectionOnlyLoadFrom(assemblyFileName)
                    : Assembly.LoadFrom(assemblyFileName);
            }
    #endif

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assemblyFileName);
            return Assembly.Load(new AssemblyName(fileNameWithoutExtension));
#endif
        }

        /// <summary>
        /// Gets the path to the .DLL of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Path to the .DLL of the assembly.</returns>
        public string GetAssemblyPath(Assembly assembly)
        {
#if !NETSTANDARD1_4
            return assembly.Location;
#else
            return null;
#endif
        }

        /// <summary>
        /// Verifies if file exists in context.
        /// </summary>
        /// <param name="assemblyFileName"> The assembly file name. </param>
        /// <returns> true if file exists. </returns>
        /// <exception cref="NotImplementedException"> This is currently not implemented. </exception>
        public bool DoesFileExist(string assemblyFileName)
        {
#if NETSTANDARD1_4
            // For projectK these assemblies can be created on the fly which means the file might not exist on disk.
            // Depend on Assembly Load failures instead of this validation.
            return true;
#else
            try
            {
                var path = this.GetFullFilePath(assemblyFileName);
                return File.Exists(path);
            }
            catch
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Creates a Navigation session for the source file.
        /// This is used to get file path and line number information for its components.
        /// </summary>
        /// <param name="source"> The source file. </param>
        /// <returns> A Navigation session instance for the current platform. </returns>
        public object CreateNavigationSession(string source)
        {
            return DiaSessionOperations.CreateNavigationSession(source);
        }

        /// <summary>
        /// Gets the navigation data for a navigation session.
        /// </summary>
        /// <param name="navigationSession"> The navigation session. </param>
        /// <param name="className"> The class name. </param>
        /// <param name="methodName"> The method name. </param>
        /// <param name="minLineNumber"> The min line number. </param>
        /// <param name="fileName"> The file name. </param>
        public void GetNavigationData(object navigationSession, string className, string methodName, out int minLineNumber, out string fileName)
        {
            DiaSessionOperations.GetNavigationData(navigationSession, className, methodName, out minLineNumber, out fileName);
        }

        /// <summary>
        /// Disposes the navigation session instance.
        /// </summary>
        /// <param name="navigationSession"> The navigation session. </param>
        public void DisposeNavigationSession(object navigationSession)
        {
            DiaSessionOperations.DisposeNavigationSession(navigationSession);
        }

        /// <summary>
        /// Gets the full file path of an assembly file.
        /// </summary>
        /// <param name="assemblyFileName"> The file name. </param>
        /// <returns> The full file path. </returns>
        public string GetFullFilePath(string assemblyFileName)
        {
            var packagePath = AppContainer.AppModel.GetCurrentPackagePath();

            return packagePath == null
                ? assemblyFileName
                : Path.Combine(packagePath, assemblyFileName);
        }
    }
}