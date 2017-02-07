// UTF-8 Enforcer: 足の不自由なハッキング
using System;
using System.ComponentModel;

namespace Kemori.Utils
{
    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T> ( this T @this, Action<T> action ) where T : ISynchronizeInvoke
        {
            if ( @this.InvokeRequired )
            {
                @this.Invoke ( action, new Object[] { @this } );
            }
            else
            {
                action?.Invoke ( @this );
            }
        }
    }
}
