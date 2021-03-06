﻿// UTF-8 Enforcer: 足の不自由なハッキング
/*
 * Kemori - An open source and community friendly manga downloader
 * Copyright (C) 2016  GGG KILLER
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kemori.Base;

namespace Kemori.Controllers
{
    /// <summary>
    /// Class that manages the loading and searching of <see
    /// cref="MangaConnector" /> s
    /// </summary>
    internal class ConnectorsManager
    {
        /// <summary>
        /// Retrieves all <see cref="MangaConnector" /> s from all assemblies in
        /// the "Connectors" folder asynchronously
        /// </summary>
        /// <returns></returns>
        public static MangaConnector[] GetAll ( )
        {
            return GetPaths ( )
                .Select ( GetConnectorsTypes )
                .Where ( types => types.Count > 0 )
                .Select ( types => ( IEnumerable<Type> ) types )
                .Aggregate ( ( typesA, typesB ) => typesA.Concat ( typesB ) )
                .Select ( connectorType =>
                {
                    try
                    {
                        var instance = ( MangaConnector ) Activator.CreateInstance ( connectorType );
                        ValidateConnector ( connectorType, instance );
                        return instance;
                    }
                    catch ( Exception ex )
                    {
                        // Logs validation errors
                        Logger.Log ( $"Error loading connector {connectorType.FullName} at \"{connectorType.Assembly.Location}\":" );
                        Logger.Log ( ex );
                        return null;
                    }
                } )
                .Where ( connector => connector != null )
                .ToArray ( );
        }

        #region Reflection

        /// <summary>
        /// The type of the <see cref="MangaConnector" /> class
        /// </summary>
        private static Type MangaConnectorType = typeof ( MangaConnector );

        /// <summary>
        /// Returns the paths of all Kemori.Connectors.*.dll in the Connectors directory
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<String> GetPaths ( )
        {
            var di = new DirectoryInfo ( "Connectors" );
            if ( !di.Exists )
            {
                di.Create ( );
                return new String[0];
            }

            return di.EnumerateFiles ( "Kemori.Connectors.*.dll" )
                .Select ( fi => fi.FullName );
        }

        /// <summary>
        /// Gets all public classes that are of <see cref="MangaConnector" />
        /// type and return their <see cref="Type" />
        /// </summary>
        /// <param name="AssemblyFile">Path of the assembly to load</param>
        /// <returns></returns>
        private static IList<Type> GetConnectorsTypes ( String AssemblyFile )
        {
            var assm = Assembly.LoadFrom ( AssemblyFile );
            var types = assm.ExportedTypes;
            var conns = new List<Type> ( );

            // Goes through each public type
            foreach ( var type in types )
            {
                // Checks if they inherit from MangaConnector
                if ( type.IsSubclassOf ( MangaConnectorType ) )
                {
                    // Adds it to list if it does
                    conns.Add ( type );
                }
            }

            return conns;
        }

        /// <summary>
        /// Checks a connector for exposed properties and methods, and throws an
        /// exception when one of them is missing
        /// </summary>
        /// <param name="connType">Connector's <see cref="Type" /></param>
        private static void ValidateConnector ( Type connType, MangaConnector inst )
        {
            // Properties being checked
            var properties = new[]
            {
                "ID",
                "Website"
            };

            // Checks each of them
            foreach ( var property in properties )
            {
                var propVal = GetPropertyValueOrDefault<String> ( connType, property, inst );
                if ( propVal == null )
                    throw new Exception ( $"Connector is missing the \"{property}\" property or value is null." );
            }

            // Methods being checked
            var methods = new[]
            {
                "InitHTTP",
                //"DownloadChapterAsync", // -> moved to base class
                "UpdateMangaListAsync",
                "GetChaptersAsync",
                "GetPageLinksAsync",
                "GetImageLinkAsync"
            };

            // Checks each of them
            foreach ( var method in methods )
            {
                if ( !MethodExists ( connType, method ) )
                {
                    throw new Exception ( $"Connector is missing the \"{method}\" method." );
                }
            }
        }

        /// <summary>
        /// Returns the property value in a fresh instance of the class or null
        /// if it doesn't exists
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="type">Type to get the property from</param>
        /// <param name="propName">Property name</param>
        private static T GetPropertyValueOrDefault<T> ( Type type, String propName, MangaConnector inst ) where T : class
        {
            // Attempts to get the property descriptor
            var prop = type.GetProperty ( propName );

            if ( prop == null )
                return null; // Null for classes and idk what for other types

            // Gets the value and attempts to convert it to T (as returns null
            // upon failure)
            var value = prop.GetValue ( inst );
            return value as T;
        }

        /// <summary>
        /// Returns wether an method exists in the class (doesn't includes
        /// inherited methods)
        /// </summary>
        /// <param name="type">Type to find the method in</param>
        /// <param name="methodName">Method name to search</param>
        private static Boolean MethodExists ( Type type, String methodName )
        {
            var method = type.GetMethod ( methodName );
            return method != null && method.DeclaringType == type;
        }

        #endregion Reflection
    }
}
