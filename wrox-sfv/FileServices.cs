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
    Filename: FileServices.cs
    Function: main gui to wrox-sfv
*/

using System;
using System.IO;
using System.Collections;
using System.Security.Permissions;

namespace MTK.WRoX_SFV
{
	/// <summary>
	/// This class traverses all directories and files
	/// in a given location which will then allow access
	/// to the file names and the number of files.
	/// </summary>
	public sealed class FileServices
	{	
		/// <summary>
		/// This event handler is used to begin any processing
		/// to signify to the user that the computer is being
		/// scanned for files.
		/// </summary>
		public delegate void ScanEventHandler( int Files );
		
		/// <summary>
		/// This event is fired when the
		/// scanning process begins.
		/// </summary>
		public event ScanEventHandler BeginScan;
		
		/// <summary>
		/// This event is fired when a
		/// file has been scanned.
		/// </summary>
		public event ScanEventHandler FileScanned;
		
		/// <summary>
		/// This event is fired when a directory
		/// has been scanned.
		/// </summary>
		public event ScanEventHandler DirectoryScanned;
		
		/// <summary>
		/// This event is fired when the scanning
		/// process ends.
		/// </summary>
		public event ScanEventHandler EndScan;

		/// <summary>
		/// The location that will be scanned.
		/// </summary>
		//private string strScanLocation;
		
		/// <summary>
		/// Keeps track of all file names. Also used
		/// to determine number of files. The queue
		/// maintains string objects.
		/// </summary>
		private Queue queFiles = new Queue();
		
		/// <summary>
		/// Keeps track of all folder names in a
		/// given location. Used to find all files
		/// in each folder. The queue mantains
		/// DirectoryInfo objects.
		/// </summary>
		private Queue queFolders = new Queue();

		/// <summary>
		/// Use this method to instantiate a FileServices object
		/// if you want to use events. You must add your event
		/// handler before using the SetLocationPath property.
		/// </summary>
		public FileServices()
		{
		}
		
		/// <summary>
		/// Use this method to instantiate a FileServices object
		/// if you don't wish to use event handling. Using this
		/// method traverses the folders and files automatically.
		/// </summary>
		/// <param name="FolderName">The location you wish to obtain
		/// file and folder information on.</param>
	//	public FileServices( string FolderName )
//		{	
//			this.LocationPath = FolderName;
//		}

		/// <summary>
		/// Assigns or retrieves the location that you wish to obtain
		/// file and folder information on. If you call this property
		/// after you have already specified a scan location the new
		/// location you set will be used after that point and previous
		/// file and folder information will be discarded.
		/// </summary>
		/// 
/*

		public string LocationPath
		{
			get
			{
				return strScanLocation;
			}

			set
			{	
				strScanLocation = value;


				 //Clear folders queue

			//	queFolders.Clear();
			//	queFolders.TrimToSize();


				//Clear files queue

				queFiles.Clear();
				queFiles.TrimToSize();
				
				if ( BeginScan != null )
					BeginScan( 0, 0);

				
				 //Hack to get first folder

				queFolders.Enqueue( new DirectoryInfo( strScanLocation ) );

				
				 //Now we can get the rest of the folders

				GetAllFolders( strScanLocation );

				foreach ( DirectoryInfo di in queFolders )
				{
					if ( DirectoryScanned != null )
						DirectoryScanned( queFolders.Count, queFiles.Count );

					foreach ( FileInfo fi in di.GetFiles() ) 
					{
						queFiles.Enqueue( fi.FullName );

				if ( FileScanned != null )
					FileScanned( queFiles.Count );

				if ( EndScan != null )
					EndScan( queFiles.Count );
			}
		}*/

		public void filesCount(int maxFiles)
		{
			//get
			//{
			//	return queFiles.Count;
			//}

			//	set
			//	{
			//queFiles.Count = value;
				
			//Clear files queue

			queFiles.Clear();
			queFiles.TrimToSize();

			if ( BeginScan != null )
				BeginScan(0);

			//if ( DirectoryScanned != null )
			//DirectoryScanned( queFiles.Count );
				
			//queFiles.Enqueue( "anything" );
			//queFiles.Enqueue( "anything" );

			if ( FileScanned != null )
				FileScanned( maxFiles );//queFiles.Count );

			if ( EndScan != null )
				EndScan( maxFiles );//queFiles.Count );
			//	}

		}


		/// <summary>
		/// Gets the number of files found in the location
		/// passed to the class.
		/// </summary>
		public int GetFileCount
		{	
			get
			{
				return queFiles.Count;
			}
		}

		/// <summary>
		/// Obtains the file size of a file that has already
		/// been found to exist in the location passed to
		/// the class.
		/// </summary>
		/// <param name="FileName">The name of the file you wish
		/// to obtain size information on.</param>
		/// <returns>The file size in bytes.</returns>
		public string GetFileSize( string FileName )
		{
			FileInfo fiFile = new FileInfo( FileName );
			return fiFile.Length.ToString();
		}

		/// <summary>
		/// Retrieves a file stream for a file.
		/// </summary>
		/// <param name="FileName">
		/// This is the name of the file.
		/// </param>
		/// <returns>A FileStream object in read only mode.</returns>
		public FileStream GetFileStream( string FileName )
		{	
			return new FileStream( FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 1024 );
		}

		/// <summary>
		/// Returns a list of the full paths to all files
		/// found in the location passed to the class.
		/// </summary>
		public string[] GetFileNames
		{
			get
			{
				string []temp = new string[queFiles.Count];
				queFiles.CopyTo( temp, 0 );
				return temp;
			}
		}

		/// <summary>
		/// Returns a list of the full paths to all the folders
		/// found in the location passed to the class.
		/// </summary>
		public string[] GetFolderNames
		{
			get
			{
				string []temp = new string[queFolders.Count];

				int index = 0;

				foreach ( DirectoryInfo diItem in queFolders ) 
				{
					temp[0] = diItem.FullName;
					++index;
				}

				return temp;	
			}
		}

		/// <summary>
		/// Finds all subfolders given a folder name.
		/// </summary>
		/// <param name="FolderName">The name of the folder
		/// that you want to check.</param>
		private void GetAllFolders( string FolderName )
		{	
			//throw new NullReferenceException( "dummy error null referece" );
			
			DirectoryInfo diFolder = new DirectoryInfo( FolderName );
			DirectoryInfo[] diFolders = diFolder.GetDirectories();

			if ( DirectoryScanned != null )
				DirectoryScanned( queFiles.Count );

			for (int i = 0, max = diFolders.Length; i < max; i++)
			{
				queFolders.Enqueue( diFolders[i] );
				GetAllFolders( diFolders[i].FullName );
			}
		}
	}
}