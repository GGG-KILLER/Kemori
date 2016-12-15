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

namespace Kemori.Classes
{
    internal class LoadProgress : Progress<(Int32, String)>
    {
        public String Message { get; private set; }
        public Int32 Progress { get; private set; }

        public void Report ( (Int32 perc, String msg) value )
        {
            Progress = value.perc;
            Message = value.msg;
            OnReport ( value );
        }
    }
}
