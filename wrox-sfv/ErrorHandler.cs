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
    Filename: ErrorHandler.cs
    Function: handles errors
*/

using System;
using System.IO;
using System.Windows.Forms;

namespace MTK
{
	/// <summary>
	/// This class is in charge of all error handling for this application.
	/// </summary>
	public sealed class ErrorHandler
	{
		private static string strMainDirectory = "WRoX-SFV";
		private string strErrorLoggingPath;
		private DirectoryInfo diLogDirectory;
		private static string strLogFileName = "Log File.txt";

		/// <summary>
		/// Creates a new error handling object.
		/// </summary>
		/// <param name="Exception">The system exception that just occured.</param>
		public unsafe ErrorHandler( ref System.Exception Exception )
		{
			CreateLogDirectory();
			LogError( ref Exception );
		}


		public static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			System.Exception error = e.Exception;
			
			ErrorHandler eh = new ErrorHandler( ref error );
		}

		public static void OnThreadException(object sender, UnhandledExceptionEventArgs e)
		{
			System.Exception error = (System.Exception)e.ExceptionObject;

			unsafe { ErrorHandler eh = new ErrorHandler( ref error ); }
		}


		private void CreateLogDirectory()
		{
			strErrorLoggingPath = Utilities.ApplicationDataPath + "\\" + strMainDirectory
				+ "\\" + Application.ProductName;

			// Attempt to create Bloodhound directory for data
			if ( !Directory.Exists( strErrorLoggingPath ) )
				diLogDirectory = Directory.CreateDirectory( strErrorLoggingPath );
			else
				diLogDirectory = new DirectoryInfo( strErrorLoggingPath );
		}

		private unsafe void LogError(ref System.Exception seInfo)
		{
			if ( seInfo is System.Threading.ThreadAbortException )
				return;

			if ( diLogDirectory.Exists )
			{
				string strLogFileFullPath = strErrorLoggingPath + "\\" + Application.ProductName + " " + strLogFileName;

				// Using ASCII encoding since Win98 doesn't natively support Unicode
				System.IO.StreamWriter swLogFile = new StreamWriter( strLogFileFullPath, true, System.Text.Encoding.ASCII, 1024 );

				for ( int index = 0; index < 70; index++ )
					swLogFile.Write('=');

				swLogFile.WriteLine("");
				swLogFile.WriteLine( "Logging Error at " + System.DateTime.Today );
				swLogFile.WriteLine( "Application version " + Application.ProductVersion );

				for ( int index = 0; index < 70; index++ )
					swLogFile.Write('=');

				swLogFile.WriteLine("");
				swLogFile.WriteLine( seInfo.Message );
				swLogFile.WriteLine("");
				swLogFile.WriteLine( "Source of error is: " + seInfo.Source );
				swLogFile.WriteLine("");
				swLogFile.WriteLine( "Stack trace shows:" );
				swLogFile.WriteLine("");
				swLogFile.WriteLine( seInfo.StackTrace );
				swLogFile.WriteLine("");
				swLogFile.WriteLine();
				swLogFile.WriteLine("");
				swLogFile.WriteLine("");
				swLogFile.WriteLine("");

				swLogFile.Close();
			}
		}
	}
}
