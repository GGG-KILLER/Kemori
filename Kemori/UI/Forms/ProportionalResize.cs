// UTF-8 Enforcer: 足の不自由なハッキング
/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
 * documentation files (the “Software”), to deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of
 * the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
 * THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 *
 * ----------------------------------------------------------------------------------
 * This was imported from GUtils.NET which is under an MIT license.
 */

using System;
using System.Windows.Forms;

namespace GUtils.Forms
{
    public class ProportionalResize
    {
        public static EventHandler GetResizeEventHandler ( Form Container, Control Control1, Control Control2, Control Control2Docked = null )
        {
            var initialContainerWidth = Container.Width;
            var initialControl1Width = Control1.Width;
            var initialControl2Width = Control2.Width;
            var initialControl2X = Control2.Location.X;

            return new EventHandler ( ( sender, e ) =>
             {
                 var proportion = DDiv ( Container.Width, initialContainerWidth );

                 Control1.Width = ( Int32 ) Math.Floor ( initialControl1Width * proportion );
                 Control2.Width = ( Int32 ) Math.Ceiling ( initialControl2Width * proportion );

                 var newX = ( Int32 ) Math.Floor ( initialControl2X * proportion );
                 Control2.Location = new System.Drawing.Point ( newX, Control2.Location.Y );

                 if ( Control2Docked != null )
                     Control2Docked.Location = new System.Drawing.Point ( newX, Control2Docked.Location.Y );
             } );
        }

        private static Double DDiv ( Double a, Double b ) => a / b;
    }
}
