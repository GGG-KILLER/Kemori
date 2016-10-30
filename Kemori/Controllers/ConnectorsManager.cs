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
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Extensions;
using Kemori.Utils;

namespace Kemori.Controllers
{
    internal class ConnectorsManager
    {
        /// <summary>
        /// Retrieves all <see cref="MangaConnector"/>s from all assemblies in the
        /// "Connectors" folder.
        /// </summary>
        /// <returns></returns>
        public static IList<MangaConnector> GetAll ( )
        {
            var di = new DirectoryInfo ( "Connectors" );
            if ( !di.Exists )
            {
                di.Create ( );
                throw new Exception ( "No connectors in the \"Connectors\" folder" );
            }

            Console.WriteLine ( "BGF:" + String.Join ( ",", di.GetFiles ( "*.dll" ).Select ( d => d.FullName ) ) );

            var assemblies = di.GetFiles ( "*.dll", SearchOption.TopDirectoryOnly )
                .Where ( fi =>
                {
                    Console.WriteLine ( fi.FullName.LastIndexOf ( Path.DirectorySeparatorChar, 0 ) );
                    var b = fi.FullName.AfterLast ( Path.DirectorySeparatorChar ).Before ( '.' );
                    Console.WriteLine ( $"T: {b} = {b == "Kemori"}" );
                    return b == "Kemori";
                } )
                .Select ( fi =>
                {
                    Console.WriteLine ( "FN:" + fi.FullName );
                    try
                    {
                        return Assembly.LoadFile ( fi.FullName );
                    }
                    catch ( Exception e )
                    {
#if DEBUG
                        var l = new Logger ( );
                        l.LogAsync ( "Couldn't load assembly: " )
                        .ContinueWith ( t => l.LogAsync ( e ) );
#endif
                        return null;
                    }
                } )
                .Where ( @as => @as != null )
                .Where ( @as =>
                {
                    return ReflectionUtils.GetTypesInNamespace<MangaConnector> ( @as, "Kemori.Connectors" ).Length > 1;
                } );

            System.Windows.Forms.MessageBox.Show ( "ASS: " + assemblies.Count ( ) );

            var connectors = new List<MangaConnector> ( );
            foreach ( var assembly in assemblies )
            {
                connectors.AddRange
                (
                    ReflectionUtils.GetTypesInNamespace<MangaConnector> ( assembly, "Kemori.Connectors" )
                );
            }

            return connectors;
        }

        /// <summary>
        /// Retrieves all <see cref="MangaConnector"/>s from all assemblies in the
        /// "Connectors" folder asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<MangaConnector>> GetAllAsync ( )
        {
            return await Task.Run ( ( ) => GetAll ( ) );
        }

        public static MangaConnector GetByID ( String ID )
        {
            return GetAll ( )
                .FirstOrDefault ( conn => conn.ID == ID );
        }

        public static IEnumerable<MangaConnector> GetByWebsite ( String Website )
        {
            return GetAll ( )
                .Where ( conn => conn.Website == Website );
        }
    }
}