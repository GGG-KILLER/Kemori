// UTF-8 Enforcer: 足の不自由なハッキング
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kemori.Classes.Tests
{
    [TestClass ( )]
    public class LoadProgressTests
    {
        [TestMethod ( )]
        public void ReportTest ( )
        {
            var p = new LoadProgress ( );
            p.ProgressChanged += ( Object sender, (Int32, String) e ) =>
            {
                Assert.AreSame ( p, sender );
                Assert.AreEqual ( e.Item1, 100 );
                Assert.AreEqual ( e.Item2, "Success" );
            };
            p.Report ( (100, "Success") );
        }
    }
}
