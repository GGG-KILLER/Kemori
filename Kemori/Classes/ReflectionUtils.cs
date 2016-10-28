using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kemori.Classes
{
    internal class ReflectionUtils
    {
        /// <summary>
        /// Returns all types inside a namespace in an assembly
        /// </summary>
        /// <param name="assm">Assembly to search in</param>
        /// <param name="Ns">Namespace to search in</param>
        /// <param name="Condition">Condition to check for the types</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllTypesInNamespace ( Assembly assm, String Ns, Func<Type, Boolean> Condition = null )
        {
            Condition = Condition ?? ( _ => true );
            return assm.GetTypes ( )
                .Where ( t => t.Namespace.Equals ( Ns, StringComparison.Ordinal ) )
                .Where ( Condition );
        }

        /// <summary>
        /// Returns all types in the namspace in the assembly
        /// with the same type as the typeparam passed to the function
        /// </summary>
        /// <typeparam name="T">Type to search for (interface, parent class or same class)</typeparam>
        /// <param name="assembly">Assembly to search in</param>
        /// <param name="Ns">Namespace to search in</param>
        /// <returns></returns>
        public static IEnumerable<T> GetTypesInNamespace<T> ( Assembly assembly, String Ns )
        {
            // Gets the type of T
            var type = typeof ( T );

            // Makes sure that the type is an interface or a class
            Assert.Test ( type.IsInterface || type.IsClass, "T must be a class or interface." );

            // Finds out if it's an interface or class
            var isInterface = type.IsInterface;

            // Gets all types that implement the interface if T is an interface
            // or the types that are the same or children of T otherwise.
            return GetAllTypesInNamespace ( assembly, Ns, assemb =>
            {
                return isInterface ? assemb.GetInterface ( type.FullName ) != null : TypeIsSameOrChild ( assemb, type );
            } )
                // Creates an instance of each one and returns them
                .Select ( typ => ( T ) Activator.CreateInstance ( typ ) );
        }

        /// <summary>
        /// Returns wether a type is the child of another
        /// </summary>
        /// <param name="child">Class to check if its a child</param>
        /// <param name="parent">Class to check if its a parent</param>
        /// <returns></returns>
        public static Boolean TypeIsChildOf ( Type child, Type parent )
        {
            return child.IsSubclassOf ( parent );
        }

        /// <summary>
        /// Checks if the type is the same or a child of the other
        /// </summary>
        /// <param name="child">Possible child type</param>
        /// <param name="parent">Possible parent type</param>
        /// <returns></returns>
        public static Boolean TypeIsSameOrChild ( Type child, Type parent )
        {
            return TypeIsChildOf ( child, parent ) || child == parent;
        }
    }
}