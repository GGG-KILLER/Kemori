/*
 * Kemori - An open and community friendly manga downloader
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

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Naming", "CC0029:Disposables Should Call Suppress Finalize", Justification = "Does not actually uses any resources that need to be disposed", Scope = "member", Target = "~M:Kemori.Classes.IO.TempAppend.Dispose" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Usage", "CC0067:Virtual Method Called On Constructor", Justification = "The method needs to be called for initalization but the user might modify it.", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.#ctor" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.LoadChapterInfo(Kemori.Base.MangaChapter)~System.Threading.Tasks.Task" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.GetPageLinks(Kemori.Base.MangaChapter)~System.Threading.Tasks.Task{System.String[]}" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.DownloadChapter(Kemori.Base.MangaChapter)~System.Threading.Tasks.Task" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.GetChapters(Kemori.Base.Manga)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{Kemori.Base.MangaChapter}}" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.GetMangaList~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{Kemori.Base.Manga}}" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "The interface implements teh name without async", Scope = "member", Target = "~M:Kemori.Base.MangaChapter.Load" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Design", "CC0021:Use nameof", Justification = "The name is related to the filename prefix rather than the assembly base namespace", Scope = "member", Target = "~M:Kemori.Controllers.ConnectorsManager.GetAll~System.Collections.Generic.IList{Kemori.Base.MangaConnector}" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Design", "CC0091:Use static method", Justification = "To underlying connectors, we only give an instance of IO, not the class", Scope = "member", Target = "~M:Kemori.Classes.IO.SafeFolderName(System.String)~System.String" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "Interface was not made to have the name containing Async", Scope = "member", Target = "~M:Kemori.Base.Manga.Load" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Style", "CC0061:Async method can be terminating with 'Async' name.", Justification = "Interface was not made to have the name containing Async", Scope = "member", Target = "~M:Kemori.Base.MangaConnector.UpdateMangaList~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{Kemori.Base.Manga}}" )]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage ( "Design", "CC0091:Use static method", Justification = "This is a form event listener and cannot be marked as static", Scope = "member", Target = "~M:Kemori.Forms.MainForm.dlPathButton_Click(System.Object,System.EventArgs)" )]