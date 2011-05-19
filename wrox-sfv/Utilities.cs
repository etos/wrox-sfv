/*
    MTK Software - WRoX-SFV
    Copyright (C) 2008  Daniel Stephenson

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


/*  Author: Daniel Stephenson
    Date: 27/03/05
    Filename: Utilities.cs
    Function: 
*/

using System;

namespace MTK
{
	public sealed class Utilities
	{
		/*/// <summary>
		/// Returns the path of the current running application.
		/// </summary>
		public static string ApplicationPath( string ApplicationName )
		{
			int iAppNameIndex = System.Application.ExecutablePath.LastIndexOf( ApplicationName );
			return Application.ExecutablePath.Substring( 0, iAppNameIndex );
		}*/

		/// <summary>
		/// Gets the path of where the current user stores their data.
		/// </summary>
		public static string DocumentsPath
		{
			get 
			{
				return Environment.GetFolderPath( Environment.SpecialFolder.Personal );
			}
		}

		/// <summary>
		/// Gets the path where applications can store data common to all users on the system.
		/// </summary>
		public static string ApplicationDataPath
		{
			get
			{
				return Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );
			}
		}
	}

}