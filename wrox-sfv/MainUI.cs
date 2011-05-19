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
    Filename: MainUI.cs
    Function: main gui to wrox-sfv
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;					// for reg key
using System.Runtime.InteropServices;	// used for WIN32 API calls
using System.IO;						// used for file system services
using System.Diagnostics;				// used to determine application instance
using System.Threading;					// used for separating UI and CRC threads
using System.Drawing.Drawing2D;			// for custom progress bar


namespace MTK.WRoX_SFV
{
	/// <summary>
	/// Summary description for WRoXMain.
	/// </summary>
	public class WRoXMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabMain;
				
		/// <summary>
		/// Vars used in app
		/// </summary>
		/// 
		private System.ComponentModel.IContainer components;
		private static DateTime dtScanStart;
		private static DateTime dtFileScanStart;
		private static DateTime dtVerifyScanStart;
		private static DateTime dtVerifyFileScanStart;
		private static bool bScanRequested = false;
		private static bool bVerifyScanRequested = false;
		private static bool bRecursiveScanRequested = false;
		public static bool bVerifyScanGood = true;
		private const string regSFVAssoc = ".sfv";
		private const string regMainMenu = "WRoX.SFV";
		private const string regShellMenu = "WRoX.SFV\\shell";
		private const string regOpenMenu = "WRoX.SFV\\shell\\open";
		private const string regCmdMenu = "WRoX.SFV\\shell\\open\\command";
		public const string regUserMenu = "Software\\WRoX.SFV";
		public const string regUserConfMenu = "Software\\WRoX.SFV\\conf";
        public bool boolargz = false;
        public string strargz;

		public delegate void DelegateRecSFV(object sender, System.EventArgs e);
		public DelegateRecSFV m_DelegateRecSFV;
		public Thread m_WorkerThread;
		public ManualResetEvent m_EventStopThread;
		public ManualResetEvent m_EventThreadStopped;

		// Thread that will be in charge of scanning files
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Label lblAppInfo;
		private System.Windows.Forms.Button btnFileHeader;
		private System.Windows.Forms.Button btnFileSFV;
		private System.Windows.Forms.TextBox edtFileSFV;
		private System.Windows.Forms.TabPage tabCreate;
		private System.Windows.Forms.TabPage tabValidate;
		private System.Windows.Forms.TabPage tabHome;
		private System.Windows.Forms.SaveFileDialog saveSFVDialog;
		private System.Windows.Forms.ColumnHeader colFilename;
		private System.Windows.Forms.ColumnHeader colResult;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.GroupBox grpStatus;
		private System.Windows.Forms.Button btnFilesLst;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Label lblFileSFV;
		private System.Windows.Forms.ListView lstCreateResults;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private SmoothProgressBar.SmoothProgressBar pbarSystemScan;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox lstBoxSelectedFiles;
		private System.Windows.Forms.ColumnHeader colFileSize;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkBG;
		private System.Windows.Forms.CheckBox chkEdit;
		private System.Windows.Forms.CheckBox chkWinSFV;
		private System.Windows.Forms.CheckBox chkDatesSizes;
		private System.Windows.Forms.NotifyIcon m_notifyicon;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox edtFileHeader;
		private System.Windows.Forms.OpenFileDialog openHeaderDialog;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.CheckBox chkVerifyDel;
		private System.Windows.Forms.CheckBox chkVerifyUnrar;
		private System.Windows.Forms.Button btnVerifySingleFile;
		private System.Windows.Forms.Label lblVerifyCheckFile;
		private System.Windows.Forms.TextBox edtVerifySingleFile;
		private System.Windows.Forms.ListView lstVerifySFV;
		private System.Windows.Forms.CheckBox chkVerifySingleFile;
		private System.Windows.Forms.Button btnVerifyFileSFV;
		private System.Windows.Forms.Label lblVerifyFileSFV;
		private System.Windows.Forms.TextBox edtVerifyFileSFV;
		private SmoothProgressBar.SmoothProgressBar pbarVerifySystemScan;
		private System.Windows.Forms.CheckBox chkVerifyBG;
		// Thread that will be in charge of CRC generation
		private System.Threading.Thread GenerateCRCThread;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.OpenFileDialog openSFVDialog;
		private System.Windows.Forms.CheckBox chkVerifyShowFailed;
		private System.Windows.Forms.CheckBox chkVerifyDelSFV;
		private System.Windows.Forms.PictureBox pBoxMain;
		private System.Windows.Forms.TabPage tabOptions;
		private System.Windows.Forms.TabPage tabNull;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Button btnOptionsApply;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.CheckBox chkOptionsConext;
		private System.Windows.Forms.CheckBox chkRememWinSize;
		private System.Windows.Forms.CheckBox chkRememResSettings;
		private System.Windows.Forms.CheckBox chkAutoRun;
		private System.Windows.Forms.CheckBox chkAutoQuit;
		private System.Windows.Forms.CheckBox chkDelSamples;
		private System.Windows.Forms.CheckBox chkDelNFO;
		private System.Windows.Forms.CheckBox chkDelDIZ;
		private System.Windows.Forms.Button btnOptionsReset;
		private System.Windows.Forms.TabPage tabRecursive;
		private System.Windows.Forms.CheckedListBox chklistboxRecursive;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.Button btnRecursiveScan;
		private System.Windows.Forms.TextBox edtRecursiveTopDir;
		private System.Windows.Forms.Label lblRecursiveNB;
		private System.Windows.Forms.Label lblRecursiveDir;
		private System.Windows.Forms.Button btnRecursiveFileTopDir;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Button btnRecursiveVerify;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ListView lstRecursiveSFVs;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnRecursiveScanClear;
		private System.Windows.Forms.CheckBox chkDelSubs;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.PictureBox pBoxMMenu5;
		private System.Windows.Forms.PictureBox pBoxMMenu4;
		private System.Windows.Forms.PictureBox pBoxMMenu3;
		private System.Windows.Forms.PictureBox pBoxMMenu2;
        private System.Windows.Forms.PictureBox pBoxMMenu1;
		private System.Threading.Thread VerifyCRCThread;

		public WRoXMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        //Main Class holding data for ALL crc procedures
		private class CRCState : ICloneable
		{
			public FileStream fsFileStream;
			public FileServices fsFileServices;
			public crc32 myCRC;
			public int iFileCount = 0;
			public Hashtable FileCRC = new Hashtable();
			public Hashtable FileSizes = new Hashtable();
			public Hashtable FileVerifyCRC = new Hashtable();
			public Queue queFiles = new Queue();
			public Queue queSFVList = new Queue();
            public String strSingleFile;
            public bool boolSingleFile = false;
            public bool boolShowFailed = false;
            public bool boolEditSFV = false;
            public bool boolWinSFVCompat = false;
            public bool boolIncDateSize = false;
            public bool boolUnrar = false;
            public bool boolDelRars = false;
            public bool boolDelSFV = false;
            public bool boolVerBG = false;

			public CRCState()
			{
				fsFileServices = new FileServices();
				myCRC = new crc32();
				FileCRC = new Hashtable();
				FileSizes = new Hashtable();
				FileVerifyCRC = new Hashtable();
				queFiles = new Queue();
				queSFVList = new Queue();
                strSingleFile = "";
                boolSingleFile = new bool();
                boolShowFailed = new bool();
                boolEditSFV = new bool();
                boolWinSFVCompat = new bool();
                boolIncDateSize = new bool();
                boolUnrar = new bool();
                boolDelRars = new bool();
                boolDelSFV = new bool();
                boolVerBG = new bool();
			}

			public string[] GetSFVFileNames
			{
				get
				{
					string []temp = new string[queSFVList.Count];
					queSFVList.CopyTo( temp, 0 );
					return temp;
				}
			}

			public void setSFVFileNames( string fileToAdd )
			{
				queSFVList.Enqueue( fileToAdd );
			}

			public string[] GetFileNames
			{
				get
				{
					string []temp = new string[queFiles.Count];
					queFiles.CopyTo( temp, 0 );
					return temp;
				}
			}

			public void setFileNames( string fileToAdd )
			{
				queFiles.Enqueue( fileToAdd );
			}

			~CRCState()
			{
				if ( fsFileStream != null )
				{
					fsFileStream.Close();
					fsFileStream = null;
				}
				fsFileServices = null;
				myCRC = null;
				FileCRC = null;
				FileSizes = null;
				queFiles = null;
				queSFVList = null;
                strSingleFile = null;
                boolSingleFile = false;
                boolShowFailed = false;
                boolEditSFV = false;
                boolWinSFVCompat = false;
                boolIncDateSize = false;
                boolUnrar = false;
                boolDelRars = false;
                boolDelSFV = false;
                boolVerBG = false;
			}

			#region ICloneable Members

			/// <summary>
			/// Performs a deep copy of the current object and returns the new instance.
			/// </summary>
			/// <returns></returns>
			public object Clone()
			{
				CRCState newState = new CRCState();

				newState.fsFileStream = this.fsFileStream;
				newState.fsFileServices = this.fsFileServices;
				newState.myCRC = this.myCRC;
				newState.iFileCount = this.iFileCount;
				newState.FileCRC = (Hashtable)this.FileCRC.Clone();
				newState.FileSizes = (Hashtable)this.FileSizes.Clone();
				newState.FileVerifyCRC = (Hashtable)this.FileVerifyCRC.Clone();
				newState.queFiles = (Queue)this.queFiles.Clone();
				newState.queSFVList = (Queue)this.queSFVList.Clone();
                newState.strSingleFile = this.strSingleFile;
                newState.boolSingleFile = this.boolSingleFile;
                newState.boolShowFailed = this.boolShowFailed;
                newState.boolEditSFV = this.boolEditSFV;
                newState.boolWinSFVCompat = this.boolWinSFVCompat;
                newState.boolIncDateSize = this.boolIncDateSize;
                newState.boolUnrar = this.boolUnrar;
                newState.boolDelRars = this.boolDelRars;
                newState.boolDelSFV = this.boolDelSFV;
                newState.boolVerBG = this.boolVerBG;

				return (object)newState;
			}
			#endregion
		}

		private class ReportDetails
		{
			private WRoXMain.CRCState crcState;
			private TimeSpan tsTimeSpan;

			public ReportDetails( TimeSpan TimeSpan, CRCState CRCDetails )
			{
				crcState = CRCDetails;
				tsTimeSpan = TimeSpan;
			}

			public CRCState GetCRCDetails
			{
				get
				{
					return crcState;
				}
			}

			public TimeSpan GetTimeSpan
			{
				get
				{
					return tsTimeSpan;
				}
			}
		}

		public class RecSFV
		{
			#region Members

			ManualResetEvent m_EventStop;
			ManualResetEvent m_EventStopped;

			WRoXMain m_form;  //ref main form for syncronous user interface calls

			#endregion

			#region Functions

			public RecSFV(ManualResetEvent eventStop, 
				ManualResetEvent eventStopped,
				WRoXMain form)
			{
				m_EventStop = eventStop;
				m_EventStopped = eventStopped;
				m_form = form;
			}

			// Function runs in worker thread and emulates long process.
			public void Run()
			{
				for (int i = 1; i <= 3; i++)
				{

					Thread.Sleep(2000);

					//Thread.CurrentThread.Resume();

					// Make synchronous call to main form.
					// MainForm.AddString function runs in main thread.
					// To make asynchronous call use BeginInvoke
					m_form.Invoke(m_form.m_DelegateRecSFV, new Object[] {this, System.EventArgs.Empty});

					// check if thread is cancelled
					if ( m_EventStop.WaitOne(0, true) )
					{
						// clean-up operations may be placed here
						// ...

						// inform main thread that this thread stopped
						m_EventStopped.Set();

						return;
					}
				}

				// Make asynchronous call to main form
				// to inform it that thread finished
				//				m_form.Invoke(m_form.m_DelegateThreadFinished, null);
			}

			#endregion
		}


		private CRCState myState;

		private void VerifyScan(object sender, System.EventArgs e)
		{
			bVerifyScanRequested = true;
			pbarVerifySystemScan.Enabled = true;
			pbarVerifySystemScan.ProgressBarColor = System.Drawing.Color.Black;
			this.lstVerifySFV.Items.Clear();
			this.btnVerify.Text = "Abort Scan";
			edtVerifyFileSFV.Text = edtVerifyFileSFV.Text.Trim();
			edtVerifyFileSFV.Enabled = false;
			btnVerifyFileSFV.Enabled = false;
			edtVerifySingleFile.Enabled = false;
			btnVerifySingleFile.Enabled = false;
            pbarVerifySystemScan.Value = 0;
            pbarVerifySystemScan.Minimum = 0;

			btnVerify.Click -= new EventHandler(this.VerifyScan);
			btnVerify.Click += new EventHandler(this.StopScan);

            myState = new CRCState();

			try
			{
				//WHOLE RELEASE (NO SINGLE FILE) VALIDATION
				if ( (edtVerifyFileSFV.Text == "")  || (edtVerifyFileSFV.TextLength == 0) ) 
				{	
					MessageBox.Show( this, "You have not entered a valid location path for your SFV File.",
						"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					RestartScanCleanUp();
					return;
				}
				else if ( (edtVerifyFileSFV.Text.Length < 2) || (edtVerifyFileSFV.Text.Substring( 0, 1 ) != "\\") 
					&& (edtVerifyFileSFV.Text.Substring( 1, 1 ) != "\\") )
				{
					if ( (edtVerifyFileSFV.Text.Length < 2) || (edtVerifyFileSFV.Text.Substring( 1, 1 ) != ":" ) )
					{
						MessageBox.Show( this, "You entered an invalid location path for your SFV. The location" +
							"\nyou wish to use must begin with a drive letter or network path. Some" +
							"\nexamples are: D:\\, or \\\\computername\\sharename, or C:\\dir",
							"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						RestartScanCleanUp();
						return;
					}
				}
				else if ( !(new FileInfo( edtVerifyFileSFV.Text ).Exists ) )
				{
					MessageBox.Show( this, "The SFV File you chose to verify does not exist!\n" +
						"Please re-check the location path and retry the scan."
						, "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Information );
					RestartScanCleanUp();
					return;
				}

				//SINGLE FILE VALIDATION
				if ( chkVerifySingleFile.Checked )
				{
					if ( (edtVerifySingleFile.Text == "")  || (edtVerifySingleFile.Text.Length == 0) ) 
					{	
						MessageBox.Show( this, "You have not entered a valid location path for your Single File.",
							"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						RestartScanCleanUp();
						return;
					}
					else if ( (edtVerifySingleFile.Text.Length < 2) || (edtVerifySingleFile.Text.Substring( 0, 1 ) != "\\") 
						&& (edtVerifySingleFile.Text.Substring( 1, 1 ) != "\\") )
					{
						if ( (edtVerifySingleFile.Text.Length < 2) || (edtVerifySingleFile.Text.Substring( 1, 1 ) != ":" ) )
						{
							MessageBox.Show( this, "You entered an invalid location path for your Single File. The location" +
								"\nyou wish to use must begin with a drive letter or network path. Some" +
								"\nexamples are: D:\\, or \\\\computername\\sharename, or C:\\dir",
								"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							RestartScanCleanUp();
							return;
						}
					}
					else if ( !(new FileInfo( this.edtVerifySingleFile.Text ).Exists ) )
					{
						MessageBox.Show( this, "The Single File you chose to verify does not exist!\n" +
							"Please re-check the location path and retry the scan."
							, "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Information );
						RestartScanCleanUp();
						return;
					}
    
                    //if singlefile + all is valid then add to mystate
                    myState.boolSingleFile = true;
                    myState.strSingleFile = StripPath(edtVerifySingleFile.Text);
				}

                //PUT SETTINGS INTO myState (required as the new thread cant cross thread call to UI)
                if ( chkVerifyShowFailed.Checked ) { myState.boolShowFailed = true; }
                if ( chkVerifyUnrar.Checked ) { myState.boolUnrar = true; }
                if ( chkVerifyDel.Checked ) { myState.boolDelRars = true; }
                if ( chkVerifyDelSFV.Checked ) { myState.boolDelSFV = true; }
                if ( chkVerifyBG.Checked ) { myState.boolVerBG = true; }

				//RUN PROGRAM IN BACKGROUND MINIMISED TO TRAY
                if ( chkVerifyBG.Checked ) { this.Hide(); }

				//RUN CHECK AND VERIFY
				myState.setSFVFileNames(edtVerifyFileSFV.Text);

				dtVerifyScanStart = DateTime.Now;	

				VerifyCRCThread = new System.Threading.Thread( new System.Threading.ThreadStart( VerifyCRC ));
				VerifyCRCThread.IsBackground = true;
				VerifyCRCThread.Name = "Verify File CRC";
                VerifyCRCThread.Priority = System.Threading.ThreadPriority.Normal;
				VerifyCRCThread.Start();
			}
			catch ( System.Threading.ThreadAbortException se )
			{
				RestartScanCleanUp();
				return;
			}
			catch ( System.ArgumentException se )
			{
				RestartScanCleanUp();
				return;
			}
		}


		private void VerifyCRC()
        {
			foreach ( string sfvFile in myState.GetSFVFileNames )  //foreach sfv file 
			{
				bVerifyScanGood = true;
                SafeInvoke.Invoke(this, "UpdateVerSFVDlg", sfvFile); //add sfvFile to Dlg + resets pbar
                SafeInvoke.Invoke(this, "UpdateVerPbarColour", System.Drawing.Color.Black);

                myState.myCRC.Reset();
                myState.FileCRC.Clear();
                myState.FileVerifyCRC.Clear();
                myState.FileSizes.Clear();
                myState.queFiles.Clear();
                myState.iFileCount = 0;

				//listview items
				ListViewItem lvMain;
				ListViewItem.ListViewSubItem lvSub1;
				ListViewItem.ListViewSubItem lvSub2;
				ListViewItem.ListViewSubItem lvSub3;
				ListViewItem.ListViewSubItem lvSub4;

				//################################ READ SFV FILE ################################
				//GET LIST OF FILENAMES FROM SFV
				try
				{
					if (  (new FileInfo( sfvFile ).Exists ) )
					{
						bool commentFound = false;
						FileInfo fiVerify = new FileInfo( sfvFile.Trim() );
						StreamReader srVerify = fiVerify.OpenText();

						string FileBuffer = srVerify.ReadToEnd();
						srVerify.Close();

						//convert unicode to stuff windows likes
						FileBuffer = System.Text.RegularExpressions.Regex.Replace(FileBuffer,"\u000D","");
						FileBuffer = System.Text.RegularExpressions.Regex.Replace(FileBuffer,"\u000A","\n");
						FileBuffer = System.Text.RegularExpressions.Regex.Replace(FileBuffer,"\u0020"," ");
						FileBuffer = System.Text.RegularExpressions.Regex.Replace(FileBuffer,"\u000C","");

						// Split buffer into lines
						string[] Lines = FileBuffer.Split('\n');

                        for (int i = 0; i < Lines.Length; i++)
						{
							string lineData = Lines[i].Trim();

							if ( lineData != null )
							{
								commentFound = false;            //reset comment boolean notifier
									
								foreach (char c in lineData)	 //check line for comment chars
								{
									if ( c.ToString() == ";" )
									{
										commentFound = true;
									}
								}
								if ( commentFound == false )     //if no comment chars read line into class
								{
									//split file+crc from line
									string l = lineData;
									string []tempstr;
									char[] delimeters = new char[] {' ','\r','\n','\r','\u000D','\u000A'}; //
									l.Trim();
									tempstr = l.Split(delimeters);

                                    if (tempstr.Length >= 2)
                                    {
                                        // if only checking single file, then just add that 1 files details to myState
                                        if (myState.boolSingleFile)
                                        {
                                            if (myState.strSingleFile.ToLower() == tempstr[0].ToLower())
                                            {
                                                myState.setFileNames(tempstr[0].Trim());
                                                myState.FileVerifyCRC.Add(tempstr[0].Trim().ToLower(), tempstr[1].ToLower());
                                                myState.iFileCount++;
                                            }
                                        }
                                        else
                                        {
                                            myState.setFileNames(tempstr[0].Trim());
                                            myState.FileVerifyCRC.Add(tempstr[0].Trim().ToLower(), tempstr[1].ToLower());
                                            myState.iFileCount++;
                                        }
                                    }
								}
							}
						}
					}
					else
					{
						//sfv File doesnt exist error prompt

                        string mbmsg = "ERROR: The SFV file you selected does not exist.";
                        string mbtitle = Application.ProductName;
                        SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}


                    //################################ CRC CHECK ################################

					string[] rarNames = new string[myState.GetFileNames.Length];
					int rarNamesCount = 0;

                    foreach (string strFile in myState.GetFileNames)
                    {
                        //add list of sfv files to rarNames[] to be used for deletion later
                        rarNames[rarNamesCount] = System.IO.Path.GetDirectoryName(sfvFile) + "\\" + strFile;
                        rarNamesCount++;

                        // ListView stuff
                        lvMain = new ListViewItem();
                        lvSub1 = new ListViewItem.ListViewSubItem();
                        lvSub2 = new ListViewItem.ListViewSubItem();
                        lvSub3 = new ListViewItem.ListViewSubItem();
                        lvSub4 = new ListViewItem.ListViewSubItem();

                        IDictionaryEnumerator verifyEnumerator = myState.FileVerifyCRC.GetEnumerator();
                        verifyEnumerator.MoveNext();

                        //SafeInvoke.Invoke(this, "MsgBox", verifyEnumerator.Key.ToString(), "title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        while (verifyEnumerator.Key.ToString().ToLower() != strFile.ToLower())
                            verifyEnumerator.MoveNext();

                        dtVerifyFileScanStart = DateTime.Now;  //start timer for individual file

                        //make current directory same as sfv's location
                        System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(sfvFile));

                        // thread got killed but we haven't been notified yet so proactively
                        // try to prevent running if thread will be aborted
                        if (VerifyCRCThread.ThreadState == System.Threading.ThreadState.AbortRequested)
                        {
                            return;
                        }


                        if ((new FileInfo(strFile).Exists))
                        {
                            SafeInvoke.Invoke(this, "UpdateVerMaxPbar", myState.iFileCount);

                            string strSize = myState.fsFileServices.GetFileSize(strFile); //get file size info
                            myState.fsFileStream = myState.fsFileServices.GetFileStream(strFile);
                            string crc = myState.myCRC.Update(myState.fsFileStream);
                            myState.fsFileStream.Close();
                            myState.fsFileStream = null;

                            myState.FileCRC.Add(strFile, crc.ToString());
                            myState.FileSizes.Add(strFile, strSize);
                            myState.setFileNames(strFile);
                            SafeInvoke.Invoke(this, "UpdateVerIncrPbar");


                            TimeSpan tsVerifyFileScanTime = new TimeSpan();
                            DateTime dtVerifyFileScanEnd = DateTime.Now;
                            tsVerifyFileScanTime = dtVerifyFileScanEnd - dtVerifyFileScanStart;


                            if (StripPath(strFile.ToLower()) == verifyEnumerator.Key.ToString().ToLower())
                            {
                                lvMain.Text = StripPath(strFile);
                            }
                            else
                            {
                                lvMain.Text = "Name Error";
                            }

                            lvSub1.Text = strSize;
                            lvSub2.Text = "" + tsVerifyFileScanTime.Seconds + "." + tsVerifyFileScanTime.Milliseconds + "secs";
                            lvSub3.Text = verifyEnumerator.Value.ToString();

                            lvMain.SubItems.Add(lvSub1); //Add sub item to main
                            lvMain.SubItems.Add(lvSub2); //Add sub item to main
                            lvMain.SubItems.Add(lvSub3); //Add sub item to main

                            if (verifyEnumerator.Value.ToString().ToLower() == crc.ToLower())
                            {
                                lvSub4.Text = "OK";

                                lvMain.UseItemStyleForSubItems = false;
                                lvSub4.ForeColor = System.Drawing.Color.Black;
                                lvSub4.Font = new System.Drawing.Font(
                                    "Tahoma", 8, System.Drawing.FontStyle.Bold);

                                lvMain.SubItems.Add(lvSub4);
                            }
                            else
                            {
                                bVerifyScanGood = false;    //scan not good

                                lvSub4.Text = "BAD";

                                lvMain.UseItemStyleForSubItems = true;
                                lvMain.ForeColor = System.Drawing.Color.Firebrick;
                                lvMain.Font = new System.Drawing.Font(
                                    "Tahoma", 8, System.Drawing.FontStyle.Bold);

                                lvMain.SubItems.Add(lvSub4);
                                SafeInvoke.Invoke(this, "UpdateVerPbarColour", System.Drawing.Color.Firebrick);
                            }

                            //Add All/Failed only depending on option checked
                            if (!myState.boolShowFailed)
                            {
                                SafeInvoke.Invoke(this, "UpdateVerView", lvMain);
                            }
                            else
                            {
                                if (lvSub4.Text != "OK")
                                {
                                    SafeInvoke.Invoke(this, "UpdateVerView", lvMain);
                                }
                            }
                        }
                        else
                        {
                            TimeSpan tsVerifyFileScanTime = new TimeSpan();
                            DateTime dtVerifyFileScanEnd = DateTime.Now;
                            tsVerifyFileScanTime = dtVerifyFileScanEnd - dtVerifyFileScanStart;

                            bVerifyScanGood = false;    //scan not good

                            lvSub1.Text = "0";
                            lvSub2.Text = "" + tsVerifyFileScanTime.Seconds + "." + tsVerifyFileScanTime.Milliseconds + "secs";
                            lvSub3.Text = verifyEnumerator.Value.ToString();
                            lvSub4.Text = "NOT FOUND";

                            if (StripPath(strFile) == verifyEnumerator.Key.ToString())
                            {
                                lvMain.Text = StripPath(strFile);
                            }
                            else
                            {
                                lvMain.Text = "Name Error";
                            }

                            lvMain.SubItems.Add(lvSub1);
                            lvMain.SubItems.Add(lvSub2);
                            lvMain.SubItems.Add(lvSub3);

                            lvMain.UseItemStyleForSubItems = true;
                            lvMain.ForeColor = System.Drawing.Color.Firebrick;
                            lvMain.Font = new System.Drawing.Font(
                                "Tahoma", 8, System.Drawing.FontStyle.Bold);

                            lvMain.SubItems.Add(lvSub4);

                            SafeInvoke.Invoke(this, "UpdateVerView", lvMain);
                            SafeInvoke.Invoke(this, "UpdateVerIncrPbar");
                            SafeInvoke.Invoke(this, "UpdateVerPbarColour", System.Drawing.Color.Firebrick);
                        }
                    }


					//################################ UNRAR CODE ################################

					if ( ( myState.boolUnrar ) && ( bVerifyScanGood == true ) )
					{
						string[] allNames = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(sfvFile));
						//						string[] rarNames = {""};
						int rarCT = 0;
						string leadingRar = "";

						//get all .rars from dir
						foreach (string rarsFiles in allNames)
						{
							System.IO.FileInfo fi_rar = new System.IO.FileInfo(rarsFiles);

							if (fi_rar.Extension.ToLower() == ".rar")  //checks for .rars
							{
								if ( rarCT >= 1 )
								{
									if ( System.Text.RegularExpressions.Regex.IsMatch(rarsFiles,".part01.",
										System.Text.RegularExpressions.RegexOptions.IgnoreCase) )
									{
										leadingRar = rarsFiles;
									}
								}
								else
								{
									leadingRar = rarsFiles;
									rarCT++;
								}
							}
						}







                        //  THE UNRAR CODE BELOW.. SOMETIMES THE INVOKE CODE IS THERE THE CATCHED ERRORS
                        //  END AT..  MAYBE I NEED TO REMOVE THIS INVOKE AND CARRY ON WITH THIS THREAD




						//send info to unrarDlg
						if ( leadingRar != "" )
						{
                            SafeInvoke.Invoke(this, "startUnrar", leadingRar);
                        }
						else // no rars found
						{
                            string mbmsg = "\n No .rar file located for extraction." ;
                            string mbtitle = "Rar Location Error";
                            SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}

					}


					//########################## AUTO-DELETE FILES CODE ##########################

					if ( bVerifyScanGood == true )
					{
						//Auto-Delete Rar Files (ALL)

						if ( myState.boolDelRars )
						{
							foreach ( string delRar in rarNames )
							{
								System.IO.File.Delete(delRar);
							}
						}

						//Auto-Delete SFV File

						if ( myState.boolDelSFV )
						{
							System.IO.File.Delete(sfvFile);
						}

						RegistryKey regKeyUserConfMenu = null;
						try
						{
							regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(regUserConfMenu);
							if(regKeyUserConfMenu != null)
							{
								//Auto-Delete DIZ
								if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelDIZ")) == true )
								{
                                    delete_ext(System.IO.Path.GetDirectoryName(sfvFile), ".diz");
								}
								//Auto-Delete NFO
								if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelNFO")) == true )
								{
                                    delete_ext(System.IO.Path.GetDirectoryName(sfvFile), ".nfo");
								}
								//Auto-Delete Sample Folder
								if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelSample")) == true )
								{
                                    delete_folder(System.IO.Path.GetDirectoryName(sfvFile), "sample");
								}
								//Auto-Delete Subs Folder
								if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelSubs")) == true )
								{
                                    delete_folder(System.IO.Path.GetDirectoryName(sfvFile), "subs");
								}
							}
						}
						catch(Exception ex)
						{
							//do nothing (no settings set)
						}
						finally       
						{
							if(regKeyUserConfMenu != null)
								regKeyUserConfMenu.Close();
						}
					}
				}

				catch ( System.Threading.ThreadAbortException se )
				{
                    SafeInvoke.Invoke(this, "RestartScanCleanUp");
					return;
				}
				catch ( System.ArgumentException se )
				{
					//do nothing
				}
				catch ( System.IO.DirectoryNotFoundException se )
				{
					System.Exception ex = (System.Exception)se;
					unsafe { ErrorHandler eh = new ErrorHandler( ref ex ); }

                    string mbmsg = se.Message + "\n\nAs a result of this " + Application.ProductName + " will stop the current scan.";
                    string mbtitle = "Directory or File Not Found";
                    SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    SafeInvoke.Invoke(this, "RestartScanCleanUp");
                    return;
				}
				catch ( System.IO.FileNotFoundException se )
				{
					System.Exception ex = (System.Exception)se;
					unsafe { ErrorHandler eh = new ErrorHandler( ref ex ); }

                    string mbmsg = se.Message + "\n\nAs a result of this " + Application.ProductName + " will stop the current scan.";
                    string mbtitle = "File Does Not Exist";
                    SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    SafeInvoke.Invoke(this, "RestartScanCleanUp");
					return;
				}
				catch
				{
                    SafeInvoke.Invoke(this, "RestartScanCleanUp");
                    return;
				}

                
				//################################ Recursive Scans ################################

				//if recursive, log the sfv file result to listview
				if ( bRecursiveScanRequested == true )
				{
					//time calcs
					TimeSpan tsScanTime = new TimeSpan();
					DateTime dtScanEnd = DateTime.Now;
					tsScanTime = dtScanEnd - dtVerifyScanStart;
					// ListView stuff
					lvMain = new ListViewItem();
					lvSub1 = new ListViewItem.ListViewSubItem();
					lvSub2 = new ListViewItem.ListViewSubItem();
					
					lvMain.Text = StripPath(sfvFile);
					lvSub1.Text = tsScanTime.Hours+"hrs "+tsScanTime.Minutes+"mins "+tsScanTime.Seconds+"secs";
					
					lvMain.UseItemStyleForSubItems = false;
					lvSub2.Font  = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);

					if ( bVerifyScanGood == true)
					{
						lvSub2.ForeColor = System.Drawing.Color.Black;
						lvSub2.Text = "OK";
					}
					else
					{
						lvSub2.ForeColor = System.Drawing.Color.Firebrick;
						lvSub2.Text = "FAILED";
					}

					lvMain.SubItems.Add(lvSub1); //Add sub item to main
					lvMain.SubItems.Add(lvSub2); //Add sub item to main
                    SafeInvoke.Invoke(this, "UpdateRecView", lvMain);
				}

			}

			//Clear Up
            SafeInvoke.Invoke(this, "CleanUpAfterScan");
		}


        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateVerView(ListViewItem lvGen)
        {
            lstVerifySFV.Items.Add(lvGen);	 // display them
            lstVerifySFV.Focus();
            lstVerifySFV.Refresh();

            //auto-scroll listview for crc results
            if (lvGen.Index > 0)
            {
                lstVerifySFV.EnsureVisible(lvGen.Index);
            }
        }

        public void startUnrar(string leadingRar)
        {
            UnrarDlg rarDlg = new UnrarDlg();

            rarDlg.PassUnrarDeets(leadingRar);
            //rarDlg.UnrarStart();   //moved to unrardlg form_load
            rarDlg.ShowDialog(this);

            if (myState.boolVerBG)  //hide rardlg too if runbg settings selected
            {
                rarDlg.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            }
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateVerSFVDlg(string sfvFile)
        {
            edtVerifyFileSFV.Text = sfvFile;
            pbarVerifySystemScan.Value = 0;
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateVerIncrPbar()
        {
            ++pbarVerifySystemScan.Value;
            pbarVerifySystemScan.Refresh();
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateVerMaxPbar(int value)
        {
            pbarVerifySystemScan.Maximum = value;
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateVerPbarColour(System.Drawing.Color colour)
        {
            pbarVerifySystemScan.ProgressBarColor = colour;
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateRecView(ListViewItem lvGen)
        {
            lstRecursiveSFVs.Items.Add(lvGen);	 // display them
            lstRecursiveSFVs.Focus();
            lstRecursiveSFVs.Refresh();

            //auto-scroll listview for crc results
            if (lvGen.Index > 0)
            {
                lstRecursiveSFVs.EnsureVisible(lvGen.Index);
            }
        }


        private void CreateScan(object sender, System.EventArgs e)
        {
            bScanRequested = true;
            pbarSystemScan.Enabled = true;
            lblAppInfo.Text = "";
            this.lstCreateResults.Items.Clear();
            this.btnCreate.Text = "Abort Scan";
            edtFileSFV.Text = edtFileSFV.Text.Trim();
            edtFileSFV.Enabled = false;
            btnFileSFV.Enabled = false;
            edtFileHeader.Enabled = false;
            btnFileHeader.Enabled = false;
            btnFilesLst.Enabled = false;

            btnCreate.Click -= new EventHandler(this.CreateScan);
            btnCreate.Click += new EventHandler(this.StopScan);

            try
            {
                if ((edtFileSFV.Text == "") || (edtFileSFV.TextLength == 0))
                {
                    MessageBox.Show(this, "You have not entered a valid location to create your SFV File.",
                        "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    RestartScanCleanUp();
                    return;
                }
                else if ((edtFileSFV.Text.Length < 2) || (edtFileSFV.Text.Substring(0, 1) != "\\")
                    && (edtFileSFV.Text.Substring(1, 1) != "\\"))
                {
                    if ((edtFileSFV.Text.Length < 2) || (edtFileSFV.Text.Substring(1, 1) != ":"))
                    {
                        MessageBox.Show(this, "You entered an invalid location to create your SFV. The location" +
                            "\nyou wish to use must begin with a drive letter or network path. Some" +
                            "\nexamples are: D:\\, or \\\\computername\\sharename, or C:\\dir",
                            "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        RestartScanCleanUp();
                        return;
                    }
                }

                //CHECK IF USERS HAVE SELECTED FILES TO SCAN
                if (this.lstBoxSelectedFiles.Items.Count == 0)
                {
                    MessageBox.Show(this, "Hmm, its seems no files were selected for scan!!",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    RestartScanCleanUp();
                    return;
                }

                myState = new CRCState();

                //PUT SETTINGS INTO myState (required as the new thread cant cross thread call to UI)
                if ( chkEdit.Checked ) { myState.boolEditSFV = true; }
                if ( chkWinSFV.Checked ) { myState.boolWinSFVCompat = true; }
                if ( chkDatesSizes.Checked ) { myState.boolIncDateSize = true; }

                //RUN PROGRAM IN BACKGROUND MINIMISED TO TRAY
                if ( chkBG.Checked ) { this.Hide(); }

                dtScanStart = DateTime.Now;
                myState.iFileCount = this.lstBoxSelectedFiles.Items.Count;
                pbarSystemScan.Value = 0;
                pbarSystemScan.Minimum = 0;
                pbarSystemScan.Maximum = myState.iFileCount;

                //getting filenames from stBoxSelectedFiles + add data to myState class
                string[] sFiles = new string[this.lstBoxSelectedFiles.Items.Count];
                this.lstBoxSelectedFiles.Items.CopyTo(sFiles, 0);
                foreach (string strFile in sFiles)
                {
                    myState.setFileNames(strFile);
                }

                GenerateCRCThread = new System.Threading.Thread(new System.Threading.ThreadStart(GenerateCRC));
                GenerateCRCThread.IsBackground = true;
                GenerateCRCThread.Name = "Generate File CRC";
                GenerateCRCThread.Priority = System.Threading.ThreadPriority.Normal;
                GenerateCRCThread.Start();
            }
            catch (System.Threading.ThreadAbortException se)
            {
                RestartScanCleanUp();
                return;
            }
            catch (System.ArgumentException se)
            {
                RestartScanCleanUp();
                return;
            }

        }



		private void GenerateCRC()
		{
			//listview items
			ListViewItem lvMain;
			ListViewItem.ListViewSubItem lvSub1;
			ListViewItem.ListViewSubItem lvSub2;
			ListViewItem.ListViewSubItem lvSub3;

			try
			{
				foreach ( string strFile in myState.GetFileNames )
				{
					dtFileScanStart = DateTime.Now;  //start timer for individual file
					string strSize = myState.fsFileServices.GetFileSize(strFile); //get file size info

                    SafeInvoke.Invoke(this, "UpdateGenInfo", StripPath(strFile));

					// thread got killed but we haven't been notified yet so proactively
					// try to prevent running if thread will be aborted
					if ( GenerateCRCThread.ThreadState == System.Threading.ThreadState.AbortRequested  )
					{
						return;
					}

					myState.fsFileStream = myState.fsFileServices.GetFileStream( strFile );
					string crc = myState.myCRC.Update( myState.fsFileStream  );
					myState.fsFileStream.Close();
					myState.fsFileStream = null;

					myState.FileCRC.Add( strFile, crc.ToString() );
					myState.FileSizes.Add( strFile, strSize );
					//myState.setFileNames( strFile );

					TimeSpan tsFileScanTime = new TimeSpan();
					DateTime dtFileScanEnd = DateTime.Now;
					tsFileScanTime = dtFileScanEnd - dtFileScanStart;

					// ListView stuff
					lvMain = new ListViewItem();
					lvSub1 = new ListViewItem.ListViewSubItem();
					lvSub2 = new ListViewItem.ListViewSubItem();
					lvSub3 = new ListViewItem.ListViewSubItem();

					lvMain.Text = StripPath(strFile);                                               // assign an item - FILE NAME
					lvSub1.Text = strSize;                                                          // assign sub item - FILE SIZE
					lvSub2.Text = tsFileScanTime.Seconds+"."+tsFileScanTime.Milliseconds+"secs";    // assign sub item - TIME
					lvSub3.Text = crc.Trim().ToLower();                                             // assign sub item - CRC VAL

					lvMain.SubItems.Add(lvSub1); //Add sub item to main
					lvMain.SubItems.Add(lvSub2); //Add sub item to main
					lvMain.SubItems.Add(lvSub3); //Add sub item to main

                    SafeInvoke.Invoke(this, "UpdateGenView", lvMain);
                    SafeInvoke.Invoke(this, "UpdateGenPbar");
                    Thread.Sleep(300);
				}

				TimeSpan tsScanTime = new TimeSpan();
				DateTime dtScanEnd = DateTime.Now;
				tsScanTime = dtScanEnd - dtScanStart;

				string strFileCount = "WRoX-SFV scanned " + myState.iFileCount.ToString() + " files.";

                string strResults = strFileCount + "  [ Time: " + tsScanTime.Hours + "hrs " +
                tsScanTime.Minutes + "mins " + tsScanTime.Seconds + "secs ]";  //strCRCnumber
                SafeInvoke.Invoke(this, "UpdateGenInfo", strResults);

				CRCState CRCStateCopy = (CRCState)myState.Clone();
				//ReportDetails state = new ReportDetails( tsScanTime, CRCStateCopy );
				//ThreadPool.QueueUserWorkItem( new System.Threading.WaitCallback(MakeReport), state );
                CreateReport();

                SafeInvoke.Invoke(this, "CleanUpAfterScan");
			}
			catch ( System.Threading.ThreadAbortException se )
			{
                SafeInvoke.Invoke(this, "RestartScanCleanUp");
				return;
			}
			catch ( System.IO.DirectoryNotFoundException se )
			{
				System.Exception ex = (System.Exception)se;
				unsafe { ErrorHandler eh = new ErrorHandler( ref ex ); }

                string mbmsg = se.Message + "\n\nAs a result of this " + Application.ProductName + " will stop the current scan.";
                string mbtitle = "Directory or File Not Found";
                SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                SafeInvoke.Invoke(this, "RestartScanCleanUp");
				return;
			}
			catch ( System.IO.FileNotFoundException se )
			{
				System.Exception ex = (System.Exception)se;
				unsafe { ErrorHandler eh = new ErrorHandler( ref ex ); }

                string mbmsg = se.Message + "\n\nAs a result of this " + Application.ProductName + " will stop the current scan.";
                string mbtitle = "Directory or File Not Found";
                SafeInvoke.Invoke(this, "MsgBox", mbmsg, mbtitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                SafeInvoke.Invoke(this, "RestartScanCleanUp");
				return;
			}
			catch
			{
                //SafeInvoke.Invoke(this, "RestartScanCleanUp");
				//return;
			}
		}

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateGenView(ListViewItem lvGen)
        {
            this.lstCreateResults.Items.Add(lvGen);	 // display them
            this.lstCreateResults.Focus();
            this.lstCreateResults.Refresh();

            //auto-scroll listview for crc results
            if (lvGen.Index > 0)
            {
                this.lstCreateResults.EnsureVisible(lvGen.Index);
            }
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateGenInfo(string lblnew)
        {
            lblAppInfo.Text = lblnew ;
            lblAppInfo.Refresh();
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UpdateGenPbar()
        {
            ++pbarSystemScan.Value;
            pbarSystemScan.Refresh();
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void MsgBox(string mainmsg, string titlemsg, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show(this, mainmsg, titlemsg, buttons, icon);
        }

/*        private void MakeReport(object state)
        {
            TimeSpan TimeSpan = ((ReportDetails)state).GetTimeSpan;
            CRCState CRCDetails = ((ReportDetails)state).GetCRCDetails;
            unsafe { CreateReport(ref TimeSpan, ref CRCDetails); }
        }*/

        private unsafe void CreateReport()  //ref TimeSpan TimeSpan, ref CRCState CRCState
		{	
			try 
			{
				FileInfo fiReport = new FileInfo( this.edtFileSFV.Text.Trim() );
				StreamWriter swReport = fiReport.CreateText();

				// BEGIN SFV COMMENT HEADER
                if ( myState.boolWinSFVCompat )
				{
					swReport.WriteLine("; Generated by WIN-SFV32 v1 (uhh NOT!!)");
                    swReport.WriteLine("; file(s) probed by the mighty WRoX-SFV ( http://www.mtksoft.com/wroxsfv )");
				}
				else
				{
                    swReport.WriteLine("; file(s) probed by the mighty WRoX-SFV ( http://www.mtksoft.com/wroxsfv )");
				}
				swReport.WriteLine(";");


				// FILE INPUT HEADER
				if ( (edtFileHeader.Text.Length < 2) || (edtFileHeader.Text.Substring( 0, 1 ) != "\\") 
					&& (edtFileHeader.Text.Substring( 1, 1 ) != "\\") )
				{
					if ( (edtFileHeader.Text.Length < 2) || (edtFileHeader.Text.Substring( 1, 1 ) != ":" ) )
					{
						if ( edtFileHeader.Text != "Enter text (or text based file) to be added to SFV's comment (optional)" )
						{
							//add manual header typed in edtFileHeader (non file input)
							swReport.WriteLine("; " + this.edtFileHeader.Text.Trim());
						}
					}
					else
					{
						if (  (new FileInfo( this.edtFileHeader.Text ).Exists ) )
						{
							FileInfo fiHeader = new FileInfo( this.edtFileHeader.Text.Trim() );
							StreamReader srHeader = fiHeader.OpenText();
							string lineData;

							using ( srHeader )
							{
								while ( ( lineData = srHeader.ReadLine() ) != null )
								{
									swReport.WriteLine("; " + lineData);
								}
							}

							srHeader.Close();
						}
						else
						{
							//Header File doesnt exist error prompt
							MessageBox.Show( this, "ERROR: Hmm, the header file you selected does not exist\n" 
								+ "WRoX-SFV will complete scan without adding header to the SFV.",
								Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
						}
					}
				}

				swReport.WriteLine(";");

				//ADD DATES AND FILE SIZES (COMMENTED) TO SFV
                if ( myState.boolIncDateSize )
				{
                    foreach ( string file in myState.GetFileNames )// & ( string size in CRCState.GetFileSizes )
					{
                        IDictionaryEnumerator sizesEnumerator = myState.FileSizes.GetEnumerator();
						sizesEnumerator.MoveNext();

						while ( sizesEnumerator.Key.ToString() != file )
							sizesEnumerator.MoveNext();

                        IDictionaryEnumerator crcEnumerator = myState.FileCRC.GetEnumerator();
						crcEnumerator.MoveNext();

						while ( crcEnumerator.Key.ToString() != file )
							crcEnumerator.MoveNext();

						swReport.WriteLine(";     " + sizesEnumerator.Value + " " + DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + 
							"." + DateTime.Now.Second.ToString("D2") + " " + DateTime.Now.Day + "-" + DateTime.Now.Month + 
							"-" + DateTime.Now.Year + " " + StripPath(crcEnumerator.Key.ToString()));
					}
				}
				swReport.WriteLine(";");

				//ADD SFV GUTS
                foreach ( string file in myState.GetFileNames )
				{
                    IDictionaryEnumerator crcEnumerator = myState.FileCRC.GetEnumerator();
					crcEnumerator.MoveNext();
					while ( crcEnumerator.Key.ToString() != file )
						crcEnumerator.MoveNext();

					string crcHash = crcEnumerator.Value.ToString();

					swReport.WriteLine(StripPath(crcEnumerator.Key.ToString()) + " " + crcHash);
				}

				swReport.Flush();
				swReport.Close();


                if ( myState.boolEditSFV )
				{

					// OPEN NOTEPAD RESULTS
					string ScanReport = fiReport.FullName;
				
					System.Diagnostics.Process note = new System.Diagnostics.Process();
				
					note.StartInfo.FileName = "notepad.exe";
					note.StartInfo.UseShellExecute = true;
					note.StartInfo.Arguments = ScanReport;
					note.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
					note.StartInfo.WorkingDirectory = fiReport.DirectoryName;
			
					// If notepad didnt launch
					if ( !note.Start() )
					{
						note.StartInfo.FileName = ScanReport;

						// ...then attempt to launch default notepad
						// (whichever app registered the html extension)
						if ( !note.Start() )
						{
							MessageBox.Show( this, "An ERROR has occured: Could not launch notepad to view SFV! \n"
								+ " The scan report was saved to " + fiReport.DirectoryName + " and the name"
								+ " of the file is " + fiReport.Name, Application.ProductName, MessageBoxButtons.OK,
								MessageBoxIcon.Information );
						}
					}
				}
			}
			catch ( System.ArgumentException se )
			{
				MessageBox.Show( this, "An ERROR has occured: \n"
					+ "WRoX-SFV has raised an Argument Exception.. \n\n" + " Solutions: \n"
					+ "             - Make sure the Header file you selected is text based \n"
					+ "             - Make sure the SFV File path is correct.)"
					, Application.ProductName, MessageBoxButtons.OK,
					MessageBoxIcon.Information );
			}
			catch ( System.IO.DirectoryNotFoundException se )
			{
				MessageBox.Show( this, "An ERROR has occured: \n"
					+ "A Directory used to by WRoX-SFV could not be accessed.\n\n"
					+ " Solution: Re-check your SFV and Header file paths and retry. )"
					, Application.ProductName, MessageBoxButtons.OK,
					MessageBoxIcon.Information );
			}
			catch ( System.IO.IOException se )
			{
				MessageBox.Show( this, "An ERROR has occured: \n"
					+ "A Directory used to by WRoX-SFV could not be found/accessed.\n\n" 
					+ " Solution: \n" 
					+ "             - Re-check your SFV and Header file paths and retry. )"
					, Application.ProductName, MessageBoxButtons.OK,
					MessageBoxIcon.Information );
			}
		}

		public void RestartScanCleanUp()
		{	
			try
			{
				if ( bScanRequested )
				{
					StopThreads();
					bScanRequested = false;

					btnCreate.Click -= new EventHandler(this.StopScan);
					//remember to remove stopscan &&&&& any existing registrations
					btnCreate.Click -= new EventHandler(this.CreateScan);
					btnCreate.Click += new EventHandler(this.CreateScan);

					edtFileSFV.Enabled = true;
					btnFileSFV.Enabled = true;
					edtFileHeader.Enabled = true;
					btnFileHeader.Enabled = true;
					btnFilesLst.Enabled = true;
					//edtFileSFV.Text = "";
					btnCreate.Text = "Create";

					edtFileHeader.Text = "Enter text (or text based file) to be added to SFV's comment (optional)";

					pbarSystemScan.Value = 0;
					pbarSystemScan.Refresh();

					lblAppInfo.Text = "";

					myState = null;
				}
				else if ( bVerifyScanRequested )
				{
					StopThreads();
					bVerifyScanRequested = false;
					bRecursiveScanRequested = false;
				
					btnVerify.Click -= new EventHandler(this.StopScan);
					btnRecursiveVerify.Click -= new EventHandler(this.StopScan);
					//remember to remove stopscan &&&&& any existing registrations
					btnVerify.Click -= new EventHandler(this.VerifyScan);
					btnVerify.Click += new EventHandler(this.VerifyScan);
					btnRecursiveVerify.Click -= new EventHandler(this.btnRecursiveVerify_Click);
					btnRecursiveVerify.Click += new EventHandler(this.btnRecursiveVerify_Click);

					this.btnVerify.Text = "Verify";
					this.btnRecursiveVerify.Text = "Verify All";

					edtVerifyFileSFV.Enabled = true;
					btnVerifyFileSFV.Enabled = true;

					if ( this.chkVerifySingleFile.Checked )
					{
						edtVerifySingleFile.Enabled = true;
						btnVerifySingleFile.Enabled = true;
					}
					else
					{
						edtVerifySingleFile.Enabled = false;
						btnVerifySingleFile.Enabled = false;
					}
				
					//edtVerifyFileSFV.Text = "";
					//edtVerifySingleFile.Text = "";
					pbarVerifySystemScan.ProgressBarColor = System.Drawing.Color.Black;

					pbarVerifySystemScan.Value = 0;
					pbarVerifySystemScan.Refresh();

					myState = null;
				}
				else
				{
					StopThreads();
				}
			}
			catch
			{
				StopThreads();
			}
		}

		public void CleanUpAfterScan()
		{	
			try
			{
                //if recursive then change tab back
                if (bRecursiveScanRequested == true)
                {
                    tabMain.SelectedTab = tabRecursive;

                    MessageBox.Show(this, "Recursive Validation Completed!","Complete!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

				//Reset Create SFV Components
				btnCreate.Click -= new EventHandler(this.StopScan);
				btnCreate.Click += new EventHandler(this.CreateScan);
			
				bScanRequested = false;

				btnCreate.Text = "Create";
				edtFileSFV.Enabled = true;
				btnFileSFV.Enabled = true;
				edtFileHeader.Enabled = true;
				btnFileHeader.Enabled = true;
				btnFilesLst.Enabled = true;
				pbarSystemScan.Enabled = false;

				//Reset Valifate SFV Components
				btnVerify.Click -= new EventHandler(this.StopScan);
				btnRecursiveVerify.Click -= new EventHandler(this.StopScan);
				//remember to remove stopscan &&&&& any existing registrations
				btnVerify.Click -= new EventHandler(this.VerifyScan);
				btnVerify.Click += new EventHandler(this.VerifyScan);
				btnRecursiveVerify.Click -= new EventHandler(this.btnRecursiveVerify_Click);
				btnRecursiveVerify.Click += new EventHandler(this.btnRecursiveVerify_Click);

				bVerifyScanRequested = false;
				bRecursiveScanRequested = false;

				this.btnVerify.Text = "Verify";
				this.btnRecursiveVerify.Text = "Verify All";
				edtVerifyFileSFV.Enabled = true;
				btnVerifyFileSFV.Enabled = true;

				if ( this.chkVerifySingleFile.Checked )
				{
					edtVerifySingleFile.Enabled = true;
					btnVerifySingleFile.Enabled = true;
				}
				else
				{
					edtVerifySingleFile.Enabled = false;
					btnVerifySingleFile.Enabled = false;
				}

				pbarVerifySystemScan.Enabled = false;

				//both
				myState = null;
				StopThreads();


				//Check for Auto-Quit
				RegistryKey regKeyUserConfMenu = null;
				try
				{
                    if (bVerifyScanGood == true)
                    {
                        regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(regUserConfMenu);
                        if (regKeyUserConfMenu != null)
                        {
                            //Auto-Quit
                            if (Convert.ToBoolean(regKeyUserConfMenu.GetValue("AutoQuit")) == true)
                            {
                                this.Close();
                            }
                        }
                    }
				}
				catch(Exception ex)
				{
					//do nothing (no settings set)
				}
				finally       
				{
					if(regKeyUserConfMenu != null)
						regKeyUserConfMenu.Close();
				}

			}
			catch
			{
				StopThreads();
			}
		}

		private void QuitApp( object o, System.ComponentModel.CancelEventArgs e )
		{
			StopThreads();

			// save program settings (if wanted, settings will be restored at restart)
			RegistryKey regKeyUserMenu = null;
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserMenu = Registry.CurrentUser.CreateSubKey(regUserMenu);
				if(regKeyUserMenu != null)
				{
					regKeyUserMenu.SetValue("","WRoX SFV USER DATA");
				}

				regKeyUserConfMenu = Registry.CurrentUser.CreateSubKey(regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					regKeyUserConfMenu.SetValue("","");
					//CreateSFV Tab
					regKeyUserConfMenu.SetValue("DatesSizes",chkDatesSizes.Checked.ToString());
					regKeyUserConfMenu.SetValue("UseWinSFV",chkWinSFV.Checked.ToString());
					regKeyUserConfMenu.SetValue("CreateSFVRunBG",chkBG.Checked.ToString());
					regKeyUserConfMenu.SetValue("CreateSFVEdit",chkEdit.Checked.ToString());
					//VerifySFV Tab
					regKeyUserConfMenu.SetValue("SingleFile",chkVerifySingleFile.Checked.ToString());
					regKeyUserConfMenu.SetValue("FailedOnly",chkVerifyShowFailed.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelSFV",chkVerifyDelSFV.Checked.ToString());
					regKeyUserConfMenu.SetValue("VerifySFVRunBG",chkVerifyBG.Checked.ToString());
					regKeyUserConfMenu.SetValue("UnrarArchive",chkVerifyUnrar.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelAfterUnrar",chkVerifyDel.Checked.ToString());
					//Save Window Size
					regKeyUserConfMenu.SetValue("WindowSizeWidth",this.Width);//.ToString());
					regKeyUserConfMenu.SetValue("WindowSizeHeight",this.Height);//.ToString());
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,ex.ToString());
			}
			finally       
			{
				if(regKeyUserConfMenu != null)
					regKeyUserConfMenu.Close();
				if(regKeyUserMenu != null)
					regKeyUserMenu.Close();
			}
		}

		private void StopScan(object sender, System.EventArgs e)
		{
            //SafeInvoke.Invoke(this, "RestartScanCleanUp");
			RestartScanCleanUp();
			//StopThreads();
		}

		private void StopThreads()
		{
			//			ThreadPool.QueueUserWorkItem( new System.Threading.WaitCallback( KillThreads ) );

			try
			{
				if ( GenerateCRCThread != null  &&  GenerateCRCThread.IsAlive )  // thread is active
				{
					Application.DoEvents();
				}
				else if ( VerifyCRCThread != null  &&  VerifyCRCThread.IsAlive )  // thread is active
				{
					Application.DoEvents();
				}
			}
			catch
			{
				//do nothing
			}
		}

		private void KillThreads( object state )  //not used anymore--------------------------------
		{
			try
			{
				if ( VerifyCRCThread != null )  //kill verify SFV threads
				{
					if ( (VerifyCRCThread.ThreadState == System.Threading.ThreadState.Running) ||
						(VerifyCRCThread.ThreadState == System.Threading.ThreadState.Background ) )
					{
						VerifyCRCThread.Abort();
						VerifyCRCThread.Join();
						VerifyCRCThread = null;
					}
				}
				if ( GenerateCRCThread != null )  //kill create SFV threads
				{
					if ( (GenerateCRCThread.ThreadState == System.Threading.ThreadState.Running) ||
						(GenerateCRCThread.ThreadState == System.Threading.ThreadState.Background ) )
					{
						GenerateCRCThread.Abort();
						GenerateCRCThread.Join();
						GenerateCRCThread = null;
					}
				}

				//Application.DoEvents();
			}
			catch (System.NullReferenceException se)
			{
				//do nothing
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WRoXMain));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.pBoxMMenu4 = new System.Windows.Forms.PictureBox();
            this.pBoxMMenu3 = new System.Windows.Forms.PictureBox();
            this.pBoxMMenu2 = new System.Windows.Forms.PictureBox();
            this.pBoxMMenu1 = new System.Windows.Forms.PictureBox();
            this.pBoxMMenu5 = new System.Windows.Forms.PictureBox();
            this.pBoxMain = new System.Windows.Forms.PictureBox();
            this.tabCreate = new System.Windows.Forms.TabPage();
            this.chkEdit = new System.Windows.Forms.CheckBox();
            this.chkBG = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstBoxSelectedFiles = new System.Windows.Forms.ListBox();
            this.pbarSystemScan = new SmoothProgressBar.SmoothProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkWinSFV = new System.Windows.Forms.CheckBox();
            this.chkDatesSizes = new System.Windows.Forms.CheckBox();
            this.btnFilesLst = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblAppInfo = new System.Windows.Forms.Label();
            this.lstCreateResults = new System.Windows.Forms.ListView();
            this.colFilename = new System.Windows.Forms.ColumnHeader();
            this.colFileSize = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colResult = new System.Windows.Forms.ColumnHeader();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnFileHeader = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.edtFileHeader = new System.Windows.Forms.TextBox();
            this.btnFileSFV = new System.Windows.Forms.Button();
            this.lblFileSFV = new System.Windows.Forms.Label();
            this.edtFileSFV = new System.Windows.Forms.TextBox();
            this.tabValidate = new System.Windows.Forms.TabPage();
            this.chkVerifyDelSFV = new System.Windows.Forms.CheckBox();
            this.chkVerifyBG = new System.Windows.Forms.CheckBox();
            this.pbarVerifySystemScan = new SmoothProgressBar.SmoothProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkVerifyDel = new System.Windows.Forms.CheckBox();
            this.chkVerifyUnrar = new System.Windows.Forms.CheckBox();
            this.btnVerifySingleFile = new System.Windows.Forms.Button();
            this.lblVerifyCheckFile = new System.Windows.Forms.Label();
            this.edtVerifySingleFile = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.lstVerifySFV = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.chkVerifySingleFile = new System.Windows.Forms.CheckBox();
            this.btnVerifyFileSFV = new System.Windows.Forms.Button();
            this.lblVerifyFileSFV = new System.Windows.Forms.Label();
            this.edtVerifyFileSFV = new System.Windows.Forms.TextBox();
            this.chkVerifyShowFailed = new System.Windows.Forms.CheckBox();
            this.tabRecursive = new System.Windows.Forms.TabPage();
            this.btnRecursiveScanClear = new System.Windows.Forms.Button();
            this.btnRecursiveVerify = new System.Windows.Forms.Button();
            this.btnRecursiveScan = new System.Windows.Forms.Button();
            this.lstRecursiveSFVs = new System.Windows.Forms.ListView();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.btnRecursiveFileTopDir = new System.Windows.Forms.Button();
            this.lblRecursiveDir = new System.Windows.Forms.Label();
            this.edtRecursiveTopDir = new System.Windows.Forms.TextBox();
            this.lblRecursiveNB = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chklistboxRecursive = new System.Windows.Forms.CheckedListBox();
            this.tabNull = new System.Windows.Forms.TabPage();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.chkDelSubs = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnOptionsReset = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkOptionsConext = new System.Windows.Forms.CheckBox();
            this.chkDelDIZ = new System.Windows.Forms.CheckBox();
            this.chkDelNFO = new System.Windows.Forms.CheckBox();
            this.chkDelSamples = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.chkAutoQuit = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.btnOptionsApply = new System.Windows.Forms.Button();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.chkRememResSettings = new System.Windows.Forms.CheckBox();
            this.chkRememWinSize = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveSFVDialog = new System.Windows.Forms.SaveFileDialog();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.m_notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.openHeaderDialog = new System.Windows.Forms.OpenFileDialog();
            this.openSFVDialog = new System.Windows.Forms.OpenFileDialog();
            this.listView2 = new System.Windows.Forms.ListView();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabMain.SuspendLayout();
            this.tabHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMain)).BeginInit();
            this.tabCreate.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.tabValidate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabRecursive.SuspendLayout();
            this.tabOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMain.Controls.Add(this.tabHome);
            this.tabMain.Controls.Add(this.tabCreate);
            this.tabMain.Controls.Add(this.tabValidate);
            this.tabMain.Controls.Add(this.tabRecursive);
            this.tabMain.Controls.Add(this.tabNull);
            this.tabMain.Controls.Add(this.tabOptions);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(652, 438);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.Tab_switches);
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.pBoxMMenu4);
            this.tabHome.Controls.Add(this.pBoxMMenu3);
            this.tabHome.Controls.Add(this.pBoxMMenu2);
            this.tabHome.Controls.Add(this.pBoxMMenu1);
            this.tabHome.Controls.Add(this.pBoxMMenu5);
            this.tabHome.Controls.Add(this.pBoxMain);
            this.tabHome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabHome.Location = new System.Drawing.Point(4, 25);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(644, 409);
            this.tabHome.TabIndex = 2;
            this.tabHome.Text = "    Main    ";
            // 
            // pBoxMMenu4
            // 
            this.pBoxMMenu4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMMenu4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pBoxMMenu4.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMMenu4.Image")));
            this.pBoxMMenu4.Location = new System.Drawing.Point(68, 270);
            this.pBoxMMenu4.Name = "pBoxMMenu4";
            this.pBoxMMenu4.Size = new System.Drawing.Size(385, 39);
            this.pBoxMMenu4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxMMenu4.TabIndex = 5;
            this.pBoxMMenu4.TabStop = false;
            this.pBoxMMenu4.Click += new System.EventHandler(this.pBoxMMenu4_Click);
            // 
            // pBoxMMenu3
            // 
            this.pBoxMMenu3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMMenu3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pBoxMMenu3.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMMenu3.Image")));
            this.pBoxMMenu3.Location = new System.Drawing.Point(66, 234);
            this.pBoxMMenu3.Name = "pBoxMMenu3";
            this.pBoxMMenu3.Size = new System.Drawing.Size(385, 43);
            this.pBoxMMenu3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxMMenu3.TabIndex = 4;
            this.pBoxMMenu3.TabStop = false;
            this.pBoxMMenu3.Click += new System.EventHandler(this.pBoxMMenu3_Click);
            // 
            // pBoxMMenu2
            // 
            this.pBoxMMenu2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMMenu2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pBoxMMenu2.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMMenu2.Image")));
            this.pBoxMMenu2.Location = new System.Drawing.Point(66, 194);
            this.pBoxMMenu2.Name = "pBoxMMenu2";
            this.pBoxMMenu2.Size = new System.Drawing.Size(385, 41);
            this.pBoxMMenu2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxMMenu2.TabIndex = 3;
            this.pBoxMMenu2.TabStop = false;
            this.pBoxMMenu2.Click += new System.EventHandler(this.pBoxMMenu2_Click);
            // 
            // pBoxMMenu1
            // 
            this.pBoxMMenu1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMMenu1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pBoxMMenu1.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMMenu1.Image")));
            this.pBoxMMenu1.Location = new System.Drawing.Point(68, 140);
            this.pBoxMMenu1.Name = "pBoxMMenu1";
            this.pBoxMMenu1.Size = new System.Drawing.Size(382, 54);
            this.pBoxMMenu1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxMMenu1.TabIndex = 2;
            this.pBoxMMenu1.TabStop = false;
            this.pBoxMMenu1.Click += new System.EventHandler(this.pBoxMMenu1_Click);
            // 
            // pBoxMMenu5
            // 
            this.pBoxMMenu5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMMenu5.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMMenu5.Image")));
            this.pBoxMMenu5.Location = new System.Drawing.Point(420, 370);
            this.pBoxMMenu5.Name = "pBoxMMenu5";
            this.pBoxMMenu5.Size = new System.Drawing.Size(192, 26);
            this.pBoxMMenu5.TabIndex = 1;
            this.pBoxMMenu5.TabStop = false;
            this.pBoxMMenu5.Click += new System.EventHandler(this.pBoxMMenu5_Click);
            // 
            // pBoxMain
            // 
            this.pBoxMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pBoxMain.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pBoxMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pBoxMain.BackgroundImage")));
            this.pBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBoxMain.Image = ((System.Drawing.Image)(resources.GetObject("pBoxMain.Image")));
            this.pBoxMain.Location = new System.Drawing.Point(0, 0);
            this.pBoxMain.Name = "pBoxMain";
            this.pBoxMain.Size = new System.Drawing.Size(644, 409);
            this.pBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBoxMain.TabIndex = 0;
            this.pBoxMain.TabStop = false;
            // 
            // tabCreate
            // 
            this.tabCreate.BackColor = System.Drawing.SystemColors.Control;
            this.tabCreate.Controls.Add(this.chkEdit);
            this.tabCreate.Controls.Add(this.chkBG);
            this.tabCreate.Controls.Add(this.label5);
            this.tabCreate.Controls.Add(this.label4);
            this.tabCreate.Controls.Add(this.label1);
            this.tabCreate.Controls.Add(this.lstBoxSelectedFiles);
            this.tabCreate.Controls.Add(this.pbarSystemScan);
            this.tabCreate.Controls.Add(this.groupBox1);
            this.tabCreate.Controls.Add(this.chkWinSFV);
            this.tabCreate.Controls.Add(this.chkDatesSizes);
            this.tabCreate.Controls.Add(this.btnFilesLst);
            this.tabCreate.Controls.Add(this.grpStatus);
            this.tabCreate.Controls.Add(this.lstCreateResults);
            this.tabCreate.Controls.Add(this.btnCreate);
            this.tabCreate.Controls.Add(this.btnFileHeader);
            this.tabCreate.Controls.Add(this.lblHeader);
            this.tabCreate.Controls.Add(this.edtFileHeader);
            this.tabCreate.Controls.Add(this.btnFileSFV);
            this.tabCreate.Controls.Add(this.lblFileSFV);
            this.tabCreate.Controls.Add(this.edtFileSFV);
            this.tabCreate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCreate.Location = new System.Drawing.Point(4, 25);
            this.tabCreate.Name = "tabCreate";
            this.tabCreate.Size = new System.Drawing.Size(644, 409);
            this.tabCreate.TabIndex = 0;
            this.tabCreate.Text = " Create SFV ";
            // 
            // chkEdit
            // 
            this.chkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEdit.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkEdit.Checked = true;
            this.chkEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEdit.Location = new System.Drawing.Point(506, 74);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(132, 15);
            this.chkEdit.TabIndex = 10;
            this.chkEdit.Text = "Edit SFV after scan";
            // 
            // chkBG
            // 
            this.chkBG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBG.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkBG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBG.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBG.Location = new System.Drawing.Point(506, 55);
            this.chkBG.Name = "chkBG";
            this.chkBG.Size = new System.Drawing.Size(132, 15);
            this.chkBG.TabIndex = 9;
            this.chkBG.Text = "Run in Background";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(293, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "3) Click \"Create\"";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(293, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "2) Use File List to Add files to SFV File.";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(293, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "1) Select a location to save your SFV File.";
            // 
            // lstBoxSelectedFiles
            // 
            this.lstBoxSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBoxSelectedFiles.BackColor = System.Drawing.SystemColors.Window;
            this.lstBoxSelectedFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBoxSelectedFiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxSelectedFiles.HorizontalScrollbar = true;
            this.lstBoxSelectedFiles.Location = new System.Drawing.Point(0, 57);
            this.lstBoxSelectedFiles.Name = "lstBoxSelectedFiles";
            this.lstBoxSelectedFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstBoxSelectedFiles.Size = new System.Drawing.Size(287, 132);
            this.lstBoxSelectedFiles.TabIndex = 11;
            // 
            // pbarSystemScan
            // 
            this.pbarSystemScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbarSystemScan.Location = new System.Drawing.Point(0, 192);
            this.pbarSystemScan.Maximum = 100;
            this.pbarSystemScan.Minimum = 0;
            this.pbarSystemScan.Name = "pbarSystemScan";
            this.pbarSystemScan.ProgressBarColor = System.Drawing.Color.Black;
            this.pbarSystemScan.Size = new System.Drawing.Size(644, 23);
            this.pbarSystemScan.TabIndex = 19;
            this.pbarSystemScan.Value = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(503, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 3);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // chkWinSFV
            // 
            this.chkWinSFV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWinSFV.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkWinSFV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWinSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWinSFV.Location = new System.Drawing.Point(506, 36);
            this.chkWinSFV.Name = "chkWinSFV";
            this.chkWinSFV.Size = new System.Drawing.Size(132, 15);
            this.chkWinSFV.TabIndex = 8;
            this.chkWinSFV.Text = "WIN-SFV Compatible";
            // 
            // chkDatesSizes
            // 
            this.chkDatesSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDatesSizes.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDatesSizes.Checked = true;
            this.chkDatesSizes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDatesSizes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDatesSizes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDatesSizes.Location = new System.Drawing.Point(506, 5);
            this.chkDatesSizes.Name = "chkDatesSizes";
            this.chkDatesSizes.Size = new System.Drawing.Size(132, 27);
            this.chkDatesSizes.TabIndex = 7;
            this.chkDatesSizes.Text = "Include Dates and File Sizes in SFV";
            // 
            // btnFilesLst
            // 
            this.btnFilesLst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilesLst.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilesLst.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilesLst.Location = new System.Drawing.Point(293, 108);
            this.btnFilesLst.Name = "btnFilesLst";
            this.btnFilesLst.Size = new System.Drawing.Size(105, 23);
            this.btnFilesLst.TabIndex = 15;
            this.btnFilesLst.Tag = "";
            this.btnFilesLst.Text = "<   File List";
            this.btnFilesLst.Click += new System.EventHandler(this.btnFilesLst_Click);
            // 
            // grpStatus
            // 
            this.grpStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStatus.Controls.Add(this.lblAppInfo);
            this.grpStatus.Location = new System.Drawing.Point(293, 132);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(351, 57);
            this.grpStatus.TabIndex = 17;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "Status";
            // 
            // lblAppInfo
            // 
            this.lblAppInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAppInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppInfo.Location = new System.Drawing.Point(9, 18);
            this.lblAppInfo.Name = "lblAppInfo";
            this.lblAppInfo.Size = new System.Drawing.Size(339, 36);
            this.lblAppInfo.TabIndex = 18;
            // 
            // lstCreateResults
            // 
            this.lstCreateResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCreateResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCreateResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilename,
            this.colFileSize,
            this.colTime,
            this.colResult});
            this.lstCreateResults.FullRowSelect = true;
            this.lstCreateResults.GridLines = true;
            this.lstCreateResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstCreateResults.Location = new System.Drawing.Point(0, 217);
            this.lstCreateResults.Name = "lstCreateResults";
            this.lstCreateResults.Size = new System.Drawing.Size(644, 192);
            this.lstCreateResults.TabIndex = 20;
            this.lstCreateResults.UseCompatibleStateImageBehavior = false;
            this.lstCreateResults.View = System.Windows.Forms.View.Details;
            // 
            // colFilename
            // 
            this.colFilename.Text = "Filename:";
            this.colFilename.Width = 325;
            // 
            // colFileSize
            // 
            this.colFileSize.Text = "File Size (Bytes):";
            this.colFileSize.Width = 100;
            // 
            // colTime
            // 
            this.colTime.Text = "Scan Time:";
            this.colTime.Width = 100;
            // 
            // colResult
            // 
            this.colResult.Text = "Results:";
            this.colResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colResult.Width = 100;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(500, 108);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(144, 23);
            this.btnCreate.TabIndex = 16;
            this.btnCreate.Text = "Create";
            this.btnCreate.Click += new System.EventHandler(this.CreateScan);
            // 
            // btnFileHeader
            // 
            this.btnFileHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileHeader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFileHeader.Location = new System.Drawing.Point(461, 30);
            this.btnFileHeader.Name = "btnFileHeader";
            this.btnFileHeader.Size = new System.Drawing.Size(32, 21);
            this.btnFileHeader.TabIndex = 6;
            this.btnFileHeader.Text = "...";
            this.btnFileHeader.Click += new System.EventHandler(this.btnFileHeader_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(5, 32);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(56, 16);
            this.lblHeader.TabIndex = 4;
            this.lblHeader.Text = "Header:";
            // 
            // edtFileHeader
            // 
            this.edtFileHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFileHeader.BackColor = System.Drawing.SystemColors.Window;
            this.edtFileHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtFileHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtFileHeader.ForeColor = System.Drawing.Color.Gray;
            this.edtFileHeader.Location = new System.Drawing.Point(65, 30);
            this.edtFileHeader.Name = "edtFileHeader";
            this.edtFileHeader.Size = new System.Drawing.Size(393, 21);
            this.edtFileHeader.TabIndex = 5;
            this.edtFileHeader.Text = "Enter text (or text based file) to be added to SFV\'s comment (optional)";
            this.edtFileHeader.Leave += new System.EventHandler(this.edtFileHeader_LeaveFocus);
            this.edtFileHeader.Enter += new System.EventHandler(this.edtFileHeader_SetFocus);
            // 
            // btnFileSFV
            // 
            this.btnFileSFV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileSFV.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFileSFV.Location = new System.Drawing.Point(461, 5);
            this.btnFileSFV.Name = "btnFileSFV";
            this.btnFileSFV.Size = new System.Drawing.Size(32, 21);
            this.btnFileSFV.TabIndex = 3;
            this.btnFileSFV.Text = "...";
            this.btnFileSFV.Click += new System.EventHandler(this.btnFileSFV_Click);
            // 
            // lblFileSFV
            // 
            this.lblFileSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileSFV.Location = new System.Drawing.Point(5, 7);
            this.lblFileSFV.Name = "lblFileSFV";
            this.lblFileSFV.Size = new System.Drawing.Size(56, 16);
            this.lblFileSFV.TabIndex = 1;
            this.lblFileSFV.Text = "SFV File:";
            // 
            // edtFileSFV
            // 
            this.edtFileSFV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFileSFV.BackColor = System.Drawing.SystemColors.Window;
            this.edtFileSFV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtFileSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtFileSFV.Location = new System.Drawing.Point(65, 5);
            this.edtFileSFV.Name = "edtFileSFV";
            this.edtFileSFV.Size = new System.Drawing.Size(393, 21);
            this.edtFileSFV.TabIndex = 2;
            // 
            // tabValidate
            // 
            this.tabValidate.Controls.Add(this.chkVerifyDelSFV);
            this.tabValidate.Controls.Add(this.chkVerifyBG);
            this.tabValidate.Controls.Add(this.pbarVerifySystemScan);
            this.tabValidate.Controls.Add(this.groupBox2);
            this.tabValidate.Controls.Add(this.btnVerifySingleFile);
            this.tabValidate.Controls.Add(this.lblVerifyCheckFile);
            this.tabValidate.Controls.Add(this.edtVerifySingleFile);
            this.tabValidate.Controls.Add(this.btnVerify);
            this.tabValidate.Controls.Add(this.lstVerifySFV);
            this.tabValidate.Controls.Add(this.chkVerifySingleFile);
            this.tabValidate.Controls.Add(this.btnVerifyFileSFV);
            this.tabValidate.Controls.Add(this.lblVerifyFileSFV);
            this.tabValidate.Controls.Add(this.edtVerifyFileSFV);
            this.tabValidate.Controls.Add(this.chkVerifyShowFailed);
            this.tabValidate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabValidate.Location = new System.Drawing.Point(4, 25);
            this.tabValidate.Name = "tabValidate";
            this.tabValidate.Size = new System.Drawing.Size(644, 409);
            this.tabValidate.TabIndex = 1;
            this.tabValidate.Text = "Validate SFV";
            // 
            // chkVerifyDelSFV
            // 
            this.chkVerifyDelSFV.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifyDelSFV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifyDelSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifyDelSFV.Location = new System.Drawing.Point(306, 30);
            this.chkVerifyDelSFV.Name = "chkVerifyDelSFV";
            this.chkVerifyDelSFV.Size = new System.Drawing.Size(99, 15);
            this.chkVerifyDelSFV.TabIndex = 4;
            this.chkVerifyDelSFV.Text = "Delete SFV File";
            // 
            // chkVerifyBG
            // 
            this.chkVerifyBG.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifyBG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifyBG.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifyBG.Location = new System.Drawing.Point(306, 45);
            this.chkVerifyBG.Name = "chkVerifyBG";
            this.chkVerifyBG.Size = new System.Drawing.Size(120, 15);
            this.chkVerifyBG.TabIndex = 6;
            this.chkVerifyBG.Text = "Run in Background";
            // 
            // pbarVerifySystemScan
            // 
            this.pbarVerifySystemScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbarVerifySystemScan.Location = new System.Drawing.Point(0, 92);
            this.pbarVerifySystemScan.Maximum = 100;
            this.pbarVerifySystemScan.Minimum = 0;
            this.pbarVerifySystemScan.Name = "pbarVerifySystemScan";
            this.pbarVerifySystemScan.ProgressBarColor = System.Drawing.Color.Black;
            this.pbarVerifySystemScan.Size = new System.Drawing.Size(644, 23);
            this.pbarVerifySystemScan.TabIndex = 12;
            this.pbarVerifySystemScan.Value = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkVerifyDel);
            this.groupBox2.Controls.Add(this.chkVerifyUnrar);
            this.groupBox2.Location = new System.Drawing.Point(488, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(156, 63);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archive options:";
            // 
            // chkVerifyDel
            // 
            this.chkVerifyDel.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifyDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifyDel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifyDel.Location = new System.Drawing.Point(15, 42);
            this.chkVerifyDel.Name = "chkVerifyDel";
            this.chkVerifyDel.Size = new System.Drawing.Size(123, 15);
            this.chkVerifyDel.TabIndex = 1;
            this.chkVerifyDel.Text = "Delete Archive After";
            // 
            // chkVerifyUnrar
            // 
            this.chkVerifyUnrar.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifyUnrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifyUnrar.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifyUnrar.Location = new System.Drawing.Point(15, 21);
            this.chkVerifyUnrar.Name = "chkVerifyUnrar";
            this.chkVerifyUnrar.Size = new System.Drawing.Size(120, 15);
            this.chkVerifyUnrar.TabIndex = 0;
            this.chkVerifyUnrar.Text = "UnRAR SFV Archive";
            // 
            // btnVerifySingleFile
            // 
            this.btnVerifySingleFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerifySingleFile.Enabled = false;
            this.btnVerifySingleFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerifySingleFile.Location = new System.Drawing.Point(446, 66);
            this.btnVerifySingleFile.Name = "btnVerifySingleFile";
            this.btnVerifySingleFile.Size = new System.Drawing.Size(32, 21);
            this.btnVerifySingleFile.TabIndex = 11;
            this.btnVerifySingleFile.Text = "...";
            this.btnVerifySingleFile.Click += new System.EventHandler(this.btnVerifySingleFile_Click);
            // 
            // lblVerifyCheckFile
            // 
            this.lblVerifyCheckFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyCheckFile.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblVerifyCheckFile.Location = new System.Drawing.Point(6, 45);
            this.lblVerifyCheckFile.Name = "lblVerifyCheckFile";
            this.lblVerifyCheckFile.Size = new System.Drawing.Size(87, 16);
            this.lblVerifyCheckFile.TabIndex = 9;
            this.lblVerifyCheckFile.Text = "File to Check:";
            // 
            // edtVerifySingleFile
            // 
            this.edtVerifySingleFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtVerifySingleFile.BackColor = System.Drawing.Color.LightGray;
            this.edtVerifySingleFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtVerifySingleFile.Enabled = false;
            this.edtVerifySingleFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtVerifySingleFile.ForeColor = System.Drawing.SystemColors.WindowText;
            this.edtVerifySingleFile.Location = new System.Drawing.Point(6, 66);
            this.edtVerifySingleFile.Name = "edtVerifySingleFile";
            this.edtVerifySingleFile.Size = new System.Drawing.Size(436, 21);
            this.edtVerifySingleFile.TabIndex = 10;
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerify.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerify.Location = new System.Drawing.Point(488, 65);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(156, 23);
            this.btnVerify.TabIndex = 8;
            this.btnVerify.Text = "Verify";
            this.btnVerify.Click += new System.EventHandler(this.VerifyScan);
            // 
            // lstVerifySFV
            // 
            this.lstVerifySFV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVerifySFV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstVerifySFV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.lstVerifySFV.FullRowSelect = true;
            this.lstVerifySFV.GridLines = true;
            this.lstVerifySFV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstVerifySFV.Location = new System.Drawing.Point(0, 118);
            this.lstVerifySFV.Name = "lstVerifySFV";
            this.lstVerifySFV.Size = new System.Drawing.Size(644, 291);
            this.lstVerifySFV.TabIndex = 13;
            this.lstVerifySFV.UseCompatibleStateImageBehavior = false;
            this.lstVerifySFV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Filename:";
            this.columnHeader5.Width = 245;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "File Size (Bytes):";
            this.columnHeader6.Width = 95;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Scan Time:";
            this.columnHeader7.Width = 95;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Results:";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 95;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Validation:";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader9.Width = 95;
            // 
            // chkVerifySingleFile
            // 
            this.chkVerifySingleFile.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifySingleFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifySingleFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifySingleFile.Location = new System.Drawing.Point(144, 30);
            this.chkVerifySingleFile.Name = "chkVerifySingleFile";
            this.chkVerifySingleFile.Size = new System.Drawing.Size(135, 15);
            this.chkVerifySingleFile.TabIndex = 3;
            this.chkVerifySingleFile.Text = "Check Single File Only";
            this.chkVerifySingleFile.CheckedChanged += new System.EventHandler(this.chkVerifySingleFile_CheckedChanged);
            // 
            // btnVerifyFileSFV
            // 
            this.btnVerifyFileSFV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerifyFileSFV.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerifyFileSFV.Location = new System.Drawing.Point(446, 5);
            this.btnVerifyFileSFV.Name = "btnVerifyFileSFV";
            this.btnVerifyFileSFV.Size = new System.Drawing.Size(32, 21);
            this.btnVerifyFileSFV.TabIndex = 2;
            this.btnVerifyFileSFV.Text = "...";
            this.btnVerifyFileSFV.Click += new System.EventHandler(this.btnVerifyFileSFV_Click);
            // 
            // lblVerifyFileSFV
            // 
            this.lblVerifyFileSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyFileSFV.Location = new System.Drawing.Point(5, 7);
            this.lblVerifyFileSFV.Name = "lblVerifyFileSFV";
            this.lblVerifyFileSFV.Size = new System.Drawing.Size(56, 16);
            this.lblVerifyFileSFV.TabIndex = 0;
            this.lblVerifyFileSFV.Text = "SFV File:";
            // 
            // edtVerifyFileSFV
            // 
            this.edtVerifyFileSFV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtVerifyFileSFV.BackColor = System.Drawing.SystemColors.Window;
            this.edtVerifyFileSFV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtVerifyFileSFV.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtVerifyFileSFV.Location = new System.Drawing.Point(65, 5);
            this.edtVerifyFileSFV.Name = "edtVerifyFileSFV";
            this.edtVerifyFileSFV.Size = new System.Drawing.Size(377, 21);
            this.edtVerifyFileSFV.TabIndex = 1;
            // 
            // chkVerifyShowFailed
            // 
            this.chkVerifyShowFailed.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkVerifyShowFailed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVerifyShowFailed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVerifyShowFailed.Location = new System.Drawing.Point(144, 45);
            this.chkVerifyShowFailed.Name = "chkVerifyShowFailed";
            this.chkVerifyShowFailed.Size = new System.Drawing.Size(141, 15);
            this.chkVerifyShowFailed.TabIndex = 5;
            this.chkVerifyShowFailed.Text = "Display Failed Files Only";
            // 
            // tabRecursive
            // 
            this.tabRecursive.Controls.Add(this.btnRecursiveScanClear);
            this.tabRecursive.Controls.Add(this.btnRecursiveVerify);
            this.tabRecursive.Controls.Add(this.btnRecursiveScan);
            this.tabRecursive.Controls.Add(this.lstRecursiveSFVs);
            this.tabRecursive.Controls.Add(this.btnRecursiveFileTopDir);
            this.tabRecursive.Controls.Add(this.lblRecursiveDir);
            this.tabRecursive.Controls.Add(this.edtRecursiveTopDir);
            this.tabRecursive.Controls.Add(this.lblRecursiveNB);
            this.tabRecursive.Controls.Add(this.label23);
            this.tabRecursive.Controls.Add(this.label11);
            this.tabRecursive.Controls.Add(this.label9);
            this.tabRecursive.Controls.Add(this.chklistboxRecursive);
            this.tabRecursive.Location = new System.Drawing.Point(4, 25);
            this.tabRecursive.Name = "tabRecursive";
            this.tabRecursive.Size = new System.Drawing.Size(644, 409);
            this.tabRecursive.TabIndex = 5;
            this.tabRecursive.Text = "Recursive SFV";
            // 
            // btnRecursiveScanClear
            // 
            this.btnRecursiveScanClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecursiveScanClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecursiveScanClear.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecursiveScanClear.Location = new System.Drawing.Point(342, 27);
            this.btnRecursiveScanClear.Name = "btnRecursiveScanClear";
            this.btnRecursiveScanClear.Size = new System.Drawing.Size(78, 23);
            this.btnRecursiveScanClear.TabIndex = 6;
            this.btnRecursiveScanClear.Text = "Clear";
            this.btnRecursiveScanClear.Click += new System.EventHandler(this.btnRecursiveScanClear_Click);
            // 
            // btnRecursiveVerify
            // 
            this.btnRecursiveVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecursiveVerify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecursiveVerify.Location = new System.Drawing.Point(506, 27);
            this.btnRecursiveVerify.Name = "btnRecursiveVerify";
            this.btnRecursiveVerify.Size = new System.Drawing.Size(120, 23);
            this.btnRecursiveVerify.TabIndex = 8;
            this.btnRecursiveVerify.Text = "Verify All";
            this.btnRecursiveVerify.Click += new System.EventHandler(this.btnRecursiveVerify_Click);
            // 
            // btnRecursiveScan
            // 
            this.btnRecursiveScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecursiveScan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecursiveScan.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecursiveScan.Location = new System.Drawing.Point(424, 27);
            this.btnRecursiveScan.Name = "btnRecursiveScan";
            this.btnRecursiveScan.Size = new System.Drawing.Size(78, 23);
            this.btnRecursiveScan.TabIndex = 7;
            this.btnRecursiveScan.Text = "Scan";
            this.btnRecursiveScan.Click += new System.EventHandler(this.btnRecursiveScan_Click);
            // 
            // lstRecursiveSFVs
            // 
            this.lstRecursiveSFVs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRecursiveSFVs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstRecursiveSFVs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader12,
            this.columnHeader13});
            this.lstRecursiveSFVs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstRecursiveSFVs.FullRowSelect = true;
            this.lstRecursiveSFVs.GridLines = true;
            this.lstRecursiveSFVs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstRecursiveSFVs.Location = new System.Drawing.Point(0, 226);
            this.lstRecursiveSFVs.Name = "lstRecursiveSFVs";
            this.lstRecursiveSFVs.Size = new System.Drawing.Size(644, 183);
            this.lstRecursiveSFVs.TabIndex = 11;
            this.lstRecursiveSFVs.UseCompatibleStateImageBehavior = false;
            this.lstRecursiveSFVs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "SFV Filename:";
            this.columnHeader10.Width = 371;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Total Scan Time:";
            this.columnHeader12.Width = 147;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Results:";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader13.Width = 104;
            // 
            // btnRecursiveFileTopDir
            // 
            this.btnRecursiveFileTopDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecursiveFileTopDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecursiveFileTopDir.Location = new System.Drawing.Point(594, 3);
            this.btnRecursiveFileTopDir.Name = "btnRecursiveFileTopDir";
            this.btnRecursiveFileTopDir.Size = new System.Drawing.Size(32, 21);
            this.btnRecursiveFileTopDir.TabIndex = 2;
            this.btnRecursiveFileTopDir.Text = "...";
            this.btnRecursiveFileTopDir.Click += new System.EventHandler(this.btnRecursiveFileTopDir_Click);
            // 
            // lblRecursiveDir
            // 
            this.lblRecursiveDir.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecursiveDir.Location = new System.Drawing.Point(3, 6);
            this.lblRecursiveDir.Name = "lblRecursiveDir";
            this.lblRecursiveDir.Size = new System.Drawing.Size(126, 16);
            this.lblRecursiveDir.TabIndex = 0;
            this.lblRecursiveDir.Text = "Top Level Directory:";
            // 
            // edtRecursiveTopDir
            // 
            this.edtRecursiveTopDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtRecursiveTopDir.BackColor = System.Drawing.SystemColors.Window;
            this.edtRecursiveTopDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtRecursiveTopDir.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtRecursiveTopDir.Location = new System.Drawing.Point(132, 3);
            this.edtRecursiveTopDir.Name = "edtRecursiveTopDir";
            this.edtRecursiveTopDir.ReadOnly = true;
            this.edtRecursiveTopDir.Size = new System.Drawing.Size(456, 21);
            this.edtRecursiveTopDir.TabIndex = 1;
            // 
            // lblRecursiveNB
            // 
            this.lblRecursiveNB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecursiveNB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecursiveNB.Location = new System.Drawing.Point(393, 57);
            this.lblRecursiveNB.Name = "lblRecursiveNB";
            this.lblRecursiveNB.Size = new System.Drawing.Size(252, 30);
            this.lblRecursiveNB.TabIndex = 9;
            this.lblRecursiveNB.Text = "NB: All SFV Files checked within this tab use same Settings as set in \"Validate S" +
                "FV\".";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(3, 72);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(312, 18);
            this.label23.TabIndex = 5;
            this.label23.Text = "3) Press \"Verify All\" button to valdiate all checked SFV Files.";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(396, 18);
            this.label11.TabIndex = 4;
            this.label11.Text = "2) Press \"Scan\" button, on complete check/uncheck SFV Files to be validated.";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(336, 18);
            this.label9.TabIndex = 3;
            this.label9.Text = "1) Enter Directory which will be recursively scanned for SFV Files.";
            // 
            // chklistboxRecursive
            // 
            this.chklistboxRecursive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chklistboxRecursive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chklistboxRecursive.HorizontalScrollbar = true;
            this.chklistboxRecursive.Location = new System.Drawing.Point(0, 93);
            this.chklistboxRecursive.Name = "chklistboxRecursive";
            this.chklistboxRecursive.Size = new System.Drawing.Size(644, 130);
            this.chklistboxRecursive.TabIndex = 10;
            // 
            // tabNull
            // 
            this.tabNull.Location = new System.Drawing.Point(4, 25);
            this.tabNull.Name = "tabNull";
            this.tabNull.Size = new System.Drawing.Size(644, 409);
            this.tabNull.TabIndex = 4;
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.chkDelSubs);
            this.tabOptions.Controls.Add(this.label25);
            this.tabOptions.Controls.Add(this.label27);
            this.tabOptions.Controls.Add(this.label28);
            this.tabOptions.Controls.Add(this.label24);
            this.tabOptions.Controls.Add(this.btnOptionsReset);
            this.tabOptions.Controls.Add(this.pictureBox1);
            this.tabOptions.Controls.Add(this.chkOptionsConext);
            this.tabOptions.Controls.Add(this.chkDelDIZ);
            this.tabOptions.Controls.Add(this.chkDelNFO);
            this.tabOptions.Controls.Add(this.chkDelSamples);
            this.tabOptions.Controls.Add(this.label10);
            this.tabOptions.Controls.Add(this.label35);
            this.tabOptions.Controls.Add(this.label38);
            this.tabOptions.Controls.Add(this.label37);
            this.tabOptions.Controls.Add(this.label36);
            this.tabOptions.Controls.Add(this.chkAutoQuit);
            this.tabOptions.Controls.Add(this.label33);
            this.tabOptions.Controls.Add(this.label34);
            this.tabOptions.Controls.Add(this.label8);
            this.tabOptions.Controls.Add(this.label31);
            this.tabOptions.Controls.Add(this.label32);
            this.tabOptions.Controls.Add(this.label30);
            this.tabOptions.Controls.Add(this.label29);
            this.tabOptions.Controls.Add(this.label26);
            this.tabOptions.Controls.Add(this.btnOptionsApply);
            this.tabOptions.Controls.Add(this.chkAutoRun);
            this.tabOptions.Controls.Add(this.chkRememResSettings);
            this.tabOptions.Controls.Add(this.chkRememWinSize);
            this.tabOptions.Controls.Add(this.label21);
            this.tabOptions.Controls.Add(this.label22);
            this.tabOptions.Controls.Add(this.label20);
            this.tabOptions.Controls.Add(this.label18);
            this.tabOptions.Controls.Add(this.label19);
            this.tabOptions.Controls.Add(this.label17);
            this.tabOptions.Controls.Add(this.label16);
            this.tabOptions.Controls.Add(this.label15);
            this.tabOptions.Controls.Add(this.label14);
            this.tabOptions.Controls.Add(this.label13);
            this.tabOptions.Controls.Add(this.label12);
            this.tabOptions.Controls.Add(this.label7);
            this.tabOptions.Controls.Add(this.label6);
            this.tabOptions.Controls.Add(this.label3);
            this.tabOptions.Controls.Add(this.label2);
            this.tabOptions.Location = new System.Drawing.Point(4, 25);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(644, 409);
            this.tabOptions.TabIndex = 3;
            this.tabOptions.Text = "   Options   ";
            // 
            // chkDelSubs
            // 
            this.chkDelSubs.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDelSubs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDelSubs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDelSubs.Location = new System.Drawing.Point(612, 288);
            this.chkDelSubs.Name = "chkDelSubs";
            this.chkDelSubs.Size = new System.Drawing.Size(12, 15);
            this.chkDelSubs.TabIndex = 30;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(366, 288);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(177, 17);
            this.label25.TabIndex = 28;
            this.label25.Text = ". . . . . . . . . . . . . . . . . . . . . . . . .";
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(522, 288);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(78, 17);
            this.label27.TabIndex = 29;
            this.label27.Text = ". . . . . . . . . . . . . . .";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(135, 288);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(253, 17);
            this.label28.TabIndex = 27;
            this.label28.Text = "Delete \"Subs\" folders located in scan directory:";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(126, 384);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(315, 15);
            this.label24.TabIndex = 40;
            this.label24.Text = "Your settings will be LOST if you do not select \"Apply\":";
            // 
            // btnOptionsReset
            // 
            this.btnOptionsReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOptionsReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOptionsReset.Location = new System.Drawing.Point(444, 375);
            this.btnOptionsReset.Name = "btnOptionsReset";
            this.btnOptionsReset.Size = new System.Drawing.Size(90, 23);
            this.btnOptionsReset.TabIndex = 41;
            this.btnOptionsReset.Text = "Reset";
            this.btnOptionsReset.Click += new System.EventHandler(this.options_reset);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(123, 396);
            this.pictureBox1.TabIndex = 70;
            this.pictureBox1.TabStop = false;
            // 
            // chkOptionsConext
            // 
            this.chkOptionsConext.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOptionsConext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOptionsConext.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOptionsConext.Location = new System.Drawing.Point(612, 42);
            this.chkOptionsConext.Name = "chkOptionsConext";
            this.chkOptionsConext.Size = new System.Drawing.Size(12, 15);
            this.chkOptionsConext.TabIndex = 4;
            // 
            // chkDelDIZ
            // 
            this.chkDelDIZ.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDelDIZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDelDIZ.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDelDIZ.Location = new System.Drawing.Point(612, 336);
            this.chkDelDIZ.Name = "chkDelDIZ";
            this.chkDelDIZ.Size = new System.Drawing.Size(12, 15);
            this.chkDelDIZ.TabIndex = 38;
            // 
            // chkDelNFO
            // 
            this.chkDelNFO.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDelNFO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDelNFO.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDelNFO.Location = new System.Drawing.Point(612, 312);
            this.chkDelNFO.Name = "chkDelNFO";
            this.chkDelNFO.Size = new System.Drawing.Size(12, 15);
            this.chkDelNFO.TabIndex = 34;
            // 
            // chkDelSamples
            // 
            this.chkDelSamples.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDelSamples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDelSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDelSamples.Location = new System.Drawing.Point(612, 264);
            this.chkDelSamples.Name = "chkDelSamples";
            this.chkDelSamples.Size = new System.Drawing.Size(12, 15);
            this.chkDelSamples.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(357, 336);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(189, 17);
            this.label10.TabIndex = 36;
            this.label10.Text = ". . . . . . . . . . . . . . . . . . . . . . . . . . . .";
            // 
            // label35
            // 
            this.label35.Location = new System.Drawing.Point(540, 336);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(60, 17);
            this.label35.TabIndex = 37;
            this.label35.Text = ". . . . . . . . . . . . . . .";
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(135, 336);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(253, 17);
            this.label38.TabIndex = 35;
            this.label38.Text = "Delete \".DIZ\" files located in scan directory:";
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(456, 162);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(84, 17);
            this.label37.TabIndex = 18;
            this.label37.Text = ". . . . . . . . . . . . . . . .";
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(519, 138);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(24, 17);
            this.label36.TabIndex = 14;
            this.label36.Text = ". . . . . . . . . . . . . . . .";
            // 
            // chkAutoQuit
            // 
            this.chkAutoQuit.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAutoQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAutoQuit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoQuit.Location = new System.Drawing.Point(612, 162);
            this.chkAutoQuit.Name = "chkAutoQuit";
            this.chkAutoQuit.Size = new System.Drawing.Size(12, 15);
            this.chkAutoQuit.TabIndex = 20;
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(540, 162);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(60, 17);
            this.label33.TabIndex = 19;
            this.label33.Text = ". . . . . . . . . . . . . . .";
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(135, 162);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(333, 17);
            this.label34.TabIndex = 17;
            this.label34.Text = "Quit WRoX-SFV on completed scan (program halts if scan fails):";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(357, 312);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(189, 17);
            this.label8.TabIndex = 32;
            this.label8.Text = ". . . . . . . . . . . . . . . . . . . . . . . . . . . .";
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(540, 312);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(60, 17);
            this.label31.TabIndex = 33;
            this.label31.Text = ". . . . . . . . . . . . . . .";
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(135, 312);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(253, 17);
            this.label32.TabIndex = 31;
            this.label32.Text = "Delete \".NFO\" files located in scan directory:";
            // 
            // label30
            // 
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label30.Location = new System.Drawing.Point(129, 234);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(129, 17);
            this.label30.TabIndex = 22;
            this.label30.Text = "On Validate SFV:";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(378, 264);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(168, 17);
            this.label29.TabIndex = 24;
            this.label29.Text = ". . . . . . . . . . . . . . . . . . . . . . . . .";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label26.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(126, 366);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(27, 15);
            this.label26.TabIndex = 39;
            this.label26.Text = "NB:";
            // 
            // btnOptionsApply
            // 
            this.btnOptionsApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOptionsApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOptionsApply.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptionsApply.Location = new System.Drawing.Point(540, 375);
            this.btnOptionsApply.Name = "btnOptionsApply";
            this.btnOptionsApply.Size = new System.Drawing.Size(90, 23);
            this.btnOptionsApply.TabIndex = 42;
            this.btnOptionsApply.Text = "Apply";
            this.btnOptionsApply.Click += new System.EventHandler(this.btnOptionsApply_Click);
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAutoRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAutoRun.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoRun.Location = new System.Drawing.Point(612, 138);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(12, 15);
            this.chkAutoRun.TabIndex = 16;
            // 
            // chkRememResSettings
            // 
            this.chkRememResSettings.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkRememResSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRememResSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRememResSettings.Location = new System.Drawing.Point(612, 90);
            this.chkRememResSettings.Name = "chkRememResSettings";
            this.chkRememResSettings.Size = new System.Drawing.Size(12, 15);
            this.chkRememResSettings.TabIndex = 12;
            // 
            // chkRememWinSize
            // 
            this.chkRememWinSize.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkRememWinSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRememWinSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRememWinSize.Location = new System.Drawing.Point(612, 66);
            this.chkRememWinSize.Name = "chkRememWinSize";
            this.chkRememWinSize.Size = new System.Drawing.Size(12, 15);
            this.chkRememWinSize.TabIndex = 8;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(540, 264);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(60, 17);
            this.label21.TabIndex = 25;
            this.label21.Text = ". . . . . . . . . . . . . . .";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(135, 264);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(253, 17);
            this.label22.TabIndex = 23;
            this.label22.Text = "Delete \"Sample\" folders located in scan directory:";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(540, 138);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(60, 17);
            this.label20.TabIndex = 15;
            this.label20.Text = ". . . . . . . . . . . . . . .";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(492, 90);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(108, 17);
            this.label18.TabIndex = 11;
            this.label18.Text = ". . . . . . . . . . . . . . .";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(372, 90);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(120, 17);
            this.label19.TabIndex = 10;
            this.label19.Text = ". . . . . . . . . . . . . . . . .";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(483, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 17);
            this.label17.TabIndex = 6;
            this.label17.Text = ". . . . . . . . . . . . . . .";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(504, 66);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(96, 17);
            this.label16.TabIndex = 7;
            this.label16.Text = ". . . . . . . . . . . . . . .";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(492, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 17);
            this.label15.TabIndex = 3;
            this.label15.Text = ". . . . . . . . . . . . . . .";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(471, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 17);
            this.label14.TabIndex = 2;
            this.label14.Text = ". . . . . . . . . . . . . . .";
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Location = new System.Drawing.Point(129, 207);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(504, 17);
            this.label13.TabIndex = 21;
            this.label13.Text = "Extended program functions:";
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.Location = new System.Drawing.Point(129, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(504, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "WRoX-SFV Settings:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(135, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(397, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Automatically begin SFV check when WRoX-SFV is launched from an SFV file:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(135, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(244, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Remember programs settings between restarts:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(135, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(361, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Remember programs Window Size between restarts (else use default):";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(135, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(343, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Add WRoX-SFV options to windows context (right clickable) menus:";
            // 
            // saveSFVDialog
            // 
            this.saveSFVDialog.DefaultExt = "sfv";
            this.saveSFVDialog.Filter = "Simple File Validator (*.sfv) | *.sfv";
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(0, 233);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(618, 144);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename:";
            this.columnHeader1.Width = 258;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status:";
            this.columnHeader2.Width = 118;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Time:";
            this.columnHeader3.Width = 95;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Results:";
            this.columnHeader4.Width = 91;
            // 
            // m_notifyicon
            // 
            this.m_notifyicon.Text = "WRoX-SFV";
            this.m_notifyicon.Visible = true;
            this.m_notifyicon.Click += new System.EventHandler(this.Form_show);
            // 
            // openHeaderDialog
            // 
            this.openHeaderDialog.DefaultExt = "*.*";
            this.openHeaderDialog.Filter = "All Files | *.*";
            // 
            // openSFVDialog
            // 
            this.openSFVDialog.DefaultExt = "sfv";
            this.openSFVDialog.Filter = "Simple File Validator (*.sfv) | *.sfv";
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView2.Location = new System.Drawing.Point(0, 118);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(644, 291);
            this.listView2.TabIndex = 35;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Pick Directory to Scan";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(644, 409);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "     Logs    ";
            // 
            // WRoXMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(652, 438);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WRoXMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WRoX-SFV v1.0.7";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.QuitApp);
            this.Resize += new System.EventHandler(this.Form_hide);
            this.tabMain.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabHome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMMenu5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxMain)).EndInit();
            this.tabCreate.ResumeLayout(false);
            this.tabCreate.PerformLayout();
            this.grpStatus.ResumeLayout(false);
            this.tabValidate.ResumeLayout(false);
            this.tabValidate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabRecursive.ResumeLayout(false);
            this.tabRecursive.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        /// 
        [STAThread]
		static void Main( string[] args ) 
		{
            WRoXMain appMain = new WRoXMain();

            if (args.Length > 0)
            {
                appMain.boolargz = true;
                appMain.strargz = args[0];
            }

            Application.Run(appMain);
		}

		private void btnFileSFV_Click(object sender, System.EventArgs e)
		{
			if((saveSFVDialog.ShowDialog() == DialogResult.OK))
			{
				edtFileSFV.Text = saveSFVDialog.FileName;
			}
		}

		private void chkVerifySingleFile_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!chkVerifySingleFile.Checked)
			{
				edtVerifySingleFile.BackColor = System.Drawing.Color.LightGray;
				lblVerifyCheckFile.ForeColor = System.Drawing.SystemColors.ControlDark;
				edtVerifySingleFile.Enabled = false;
				btnVerifySingleFile.Enabled = false;
				//edtSingleFile.Text = "";

				this.chkVerifyDelSFV.Checked = false;
				this.chkVerifyUnrar.Checked = false;
				this.chkVerifyDel.Checked = false;
				this.chkVerifyDelSFV.Enabled = true;
				this.chkVerifyUnrar.Enabled = true;
				this.chkVerifyDel.Enabled = true;
			}
			else
			{
				edtVerifySingleFile.BackColor = System.Drawing.SystemColors.Window;
				lblVerifyCheckFile.ForeColor = System.Drawing.SystemColors.ControlText;
				edtVerifySingleFile.Enabled = true;
				btnVerifySingleFile.Enabled = true;

				edtVerifySingleFile.Text = "NB: SFV File Deletion & UnRar options wont work with single file check";
				this.chkVerifyDelSFV.Enabled = false;
				this.chkVerifyUnrar.Enabled = false;
				this.chkVerifyDel.Enabled = false;
				this.chkVerifyDelSFV.Checked = false;
				this.chkVerifyUnrar.Checked = false;
				this.chkVerifyDel.Checked = false;
			}
		}

		private void btnFilesLst_Click(object sender, System.EventArgs e)
		{
			AddFilesDlg addDlg = new AddFilesDlg();
			addDlg.passControl = new AddFilesDlg.PassControl(PassData);
			addDlg.ShowDialog( this );
		}

		//delegate data between MainUI and AddFilesDlg
		private void PassData(object sender)
		{
			// copies ListBox values from AddFileDlg to MainUI

			ListBox listbox = (ListBox)sender;
			object[] items = new object[listbox.Items.Count];
			listbox.Items.CopyTo(items,0);
			this.lstBoxSelectedFiles.BeginUpdate();
			this.lstBoxSelectedFiles.Items.Clear();
			this.lstBoxSelectedFiles.Items.AddRange(items);
			this.lstBoxSelectedFiles.EndUpdate();
		}

		private void edtFileHeader_SetFocus(object sender, System.EventArgs e)
		{
			edtFileHeader.ForeColor = System.Drawing.SystemColors.WindowText;

			if (edtFileHeader.Text == "Enter text (or text based file) to be added to SFV's comment (optional)")
			{
				edtFileHeader.Text = "";
			}			
		}

		private void edtFileHeader_LeaveFocus(object sender, System.EventArgs e)
		{
			if (( edtFileHeader.Text == "") || (edtFileHeader.Text.Length == 0))
			{
                edtFileHeader.ForeColor = System.Drawing.Color.Gray;
				edtFileHeader.Text = "Enter text (or text based file) to be added to SFV's comment (optional)";
			}
			else
			{
				edtFileHeader.ForeColor = System.Drawing.SystemColors.WindowText;
			}
		}

		private string StripPath(string path)
		{
			string p = path;
			string []tempstr;
			p.Trim();
			tempstr = p.Split('\\',(char)10);
			return tempstr[tempstr.Length - 1];
		}

		private void Form_Load(object sender, System.EventArgs e)
		{
			//Code for Tray Icon
			m_notifyicon.Text = "WRoX-SFV"; 
			m_notifyicon.Visible = true; 
			m_notifyicon.Icon = this.Icon;  //= new Icon(GetType(),"wrox.ico");

			// initialize delegates / events for recursive sfv
			m_DelegateRecSFV = new DelegateRecSFV(VerifyScan);
			m_EventStopThread = new ManualResetEvent(false);
			m_EventThreadStopped = new ManualResetEvent(false);

			//check and restore saved program settings
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("RememberSettings")) == true )
					{
						//CreateSFV Tab
						chkDatesSizes.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DatesSizes"));
						chkWinSFV.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("UseWinSFV"));
						chkBG.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("CreateSFVRunBG"));
						chkEdit.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("CreateSFVEdit"));
						//VerifySFV Tab
						chkVerifySingleFile.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("SingleFile"));
						chkVerifyShowFailed.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("FailedOnly"));
						chkVerifyDelSFV.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelSFV"));
						chkVerifyBG.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("VerifySFVRunBG"));
						chkVerifyUnrar.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("UnrarArchive"));
						chkVerifyDel.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelAfterUnrar"));
					}
					//Restore Window Size
					if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("WindowSize")) == true )
					{
						this.Width = Convert.ToInt16(regKeyUserConfMenu.GetValue("WindowSizeWidth"));
						this.Height = Convert.ToInt16(regKeyUserConfMenu.GetValue("WindowSizeHeight"));
					}
					
				}
			}
			catch(Exception ex)
			{
				//do nothing as no settings exist yet (ie first program start)
			}
			finally       
			{
				if(regKeyUserConfMenu != null)
					regKeyUserConfMenu.Close();
			}



            //IF ARGS THEN ENTER DETAILS INTO FORM AND START VERIFY SCAN
            if (boolargz == true)
            {
                tabMain.SelectedTab = tabValidate;
                edtVerifyFileSFV.Text = strargz;

                //Check registry for Auto-Run settings
                regKeyUserConfMenu = null;
                try
                {
                    regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(regUserConfMenu);
                    if (regKeyUserConfMenu != null)
                    {
                        //Restore Window Size
                        if (Convert.ToBoolean(regKeyUserConfMenu.GetValue("AutoRun")) == true)
                        {
                            VerifyScan(this, System.EventArgs.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //do nothing as no settings exist yet (ie first program start)
                }
                finally
                {
                    if (regKeyUserConfMenu != null)
                        regKeyUserConfMenu.Close();
                }
            }
		}

		private void Form_hide(object sender, System.EventArgs e)
		{
			//hide main window
			if (FormWindowState.Minimized == this.WindowState)
			{
				this.Hide();
			}
			else
			{
				//resize columns of lstviews when resizing window

				this.colFilename.Width = (299 + (this.Width - 634));
				this.columnHeader5.Width = (217 + (this.Width - 634));
				this.columnHeader10.Width = (343 + (this.Width - 634));
			}
		}

		private void Form_show(object sender, System.EventArgs e)
		{
			//return the main window
			Show();
			WindowState = FormWindowState.Normal;
		}

		private void btnFileHeader_Click(object sender, System.EventArgs e)
		{
			if((openHeaderDialog.ShowDialog() == DialogResult.OK))
			{
				edtFileHeader.Text = openHeaderDialog.FileName;
				edtFileHeader_SetFocus(sender, e);
			}
		}

		private void btnVerifyFileSFV_Click(object sender, System.EventArgs e)
		{
			if((openSFVDialog.ShowDialog() == DialogResult.OK))
			{
				edtVerifyFileSFV.Text = openSFVDialog.FileName;
			}
		}

		private void btnVerifySingleFile_Click(object sender, System.EventArgs e)
		{
			if((openHeaderDialog.ShowDialog() == DialogResult.OK))
			{
				edtVerifySingleFile.Text = openHeaderDialog.FileName;
			}
		}

		private void Tab_switches(object sender, System.EventArgs e)
		{
			if ( this.tabMain.SelectedTab == this.tabHome )
			{
				this.pBoxMain.Focus();
			}
			else if ( this.tabMain.SelectedTab == this.tabNull )
			{
				this.tabMain.SelectedTab = this.tabOptions;
			}
			else if ( this.tabMain.SelectedTab == this.tabCreate )
			{
				this.edtFileSFV.Focus();
			}
			else if ( this.tabMain.SelectedTab == this.tabValidate )
			{
				this.edtVerifyFileSFV.Focus();
			}
			else if ( this.tabMain.SelectedTab == this.tabRecursive )
			{
				this.edtRecursiveTopDir.Focus();
			}
			else if ( this.tabMain.SelectedTab == this.tabOptions )
			{
				options_reset(sender,e);
			}
		}

		//not used atm
		private bool VerifyAscii(string Buffer)
		{
			// Create Regex for matching only the Ascii Table
			System.Text.RegularExpressions.Regex R = 
				new System.Text.RegularExpressions.Regex("[\x00-\xFF]");
			// The Size of the block that we want to analyze
			// Done this way for performance
			// Much overhead (depending on size of file) to Regex the whole thing
			int BlockSize = 10;
			// Our Iteration variables
			int Start;
			int Len;
			string Block;
			System.Text.RegularExpressions.MatchCollection matchColl;
			// Iterate through our buffer
			for (int i=0;i<(Buffer.Length/BlockSize);i++)
			{
				// Starting Point for this iteration
				Start = (i*5);
				// Ternerary operator used to assign length of this block
				// we don't want to overshoot the end of the string buffer
				Len = (Start+BlockSize>Buffer.Length) ? (Buffer.Length-Start) : BlockSize;
				// Get our block from the buffer
				Block  = Buffer.Substring(Start,Len);
				// Run our Regex, and get our match collection
				matchColl = R.Matches(Block);
				// If our match count is less that the length of the string,
				// we know that we have characters outside of the ascii table
				if (matchColl.Count<Len)
				{
					// Return false, this buffer could not be
					// evaluated as Ascii Only
					return false;
				}
			}
			// No bad charaters were found, 
			// so all characters are within the ascii table
			// Return true
			return true;
		}

		private void btnOptionsApply_Click(object sender, System.EventArgs e)
		{

			//  ALWAYS SAVED: ---------------SAVE OPTIONS SETTINGS!-------------------------------

			RegistryKey regKeyUserMenu = null;
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserMenu = Registry.CurrentUser.CreateSubKey(regUserMenu);
				if(regKeyUserMenu != null)
				{
					regKeyUserMenu.SetValue("","WRoX SFV USER DATA");
				}

				regKeyUserConfMenu = Registry.CurrentUser.CreateSubKey(regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					regKeyUserConfMenu.SetValue("","");
					regKeyUserConfMenu.SetValue("ContextMenus",chkOptionsConext.Checked.ToString());
					regKeyUserConfMenu.SetValue("WindowSize",chkRememWinSize.Checked.ToString());
					regKeyUserConfMenu.SetValue("RememberSettings",chkRememResSettings.Checked.ToString());
					regKeyUserConfMenu.SetValue("AutoRun",chkAutoRun.Checked.ToString());
					regKeyUserConfMenu.SetValue("AutoQuit",chkAutoQuit.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelSample",chkDelSamples.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelSubs",chkDelSubs.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelNFO",chkDelNFO.Checked.ToString());
					regKeyUserConfMenu.SetValue("DelDIZ",chkDelDIZ.Checked.ToString());
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,ex.ToString());
			}
			finally       
			{
				if(regKeyUserConfMenu != null)
					regKeyUserConfMenu.Close();
				if(regKeyUserMenu != null)
					regKeyUserMenu.Close();
			}



			//  ACTION:  CONTEXT MENUs------------------------------------------------------------

			if ( chkOptionsConext.Checked )   //ADD SETTINGS TO REGISTRY
			{
				RegistryKey regKeySFVAssoc = null;
				RegistryKey regKeyMainMenu = null;
				RegistryKey regKeyShellMenu = null;
				RegistryKey regKeyOpenMenu = null;
				RegistryKey regKeyCmdMenu = null;

				try
				{
					regKeySFVAssoc = Registry.ClassesRoot.CreateSubKey(regSFVAssoc);
					if(regKeySFVAssoc != null)
					{
						regKeySFVAssoc.SetValue("","WRoX.SFV");
					}

					regKeyMainMenu = Registry.ClassesRoot.CreateSubKey(regMainMenu);
					if(regKeyMainMenu != null)
					{
						regKeyMainMenu.SetValue("","WRoX SFV Creator");
					}

					regKeyShellMenu = Registry.ClassesRoot.CreateSubKey(regShellMenu);
					if(regKeyShellMenu != null)
					{
						regKeyShellMenu.SetValue("","open");
					}

					regKeyOpenMenu = Registry.ClassesRoot.CreateSubKey(regOpenMenu);
					if(regKeyOpenMenu != null)
					{
						regKeyOpenMenu.SetValue("","Open with &WRoX-SFV");
					}

					regKeyCmdMenu = Registry.ClassesRoot.CreateSubKey(regCmdMenu);
					if(regKeyCmdMenu != null)
					{
						regKeyCmdMenu.SetValue("",("\"" + Application.ExecutablePath + "\"" + " " + "\"%1\""));//("\"D:\\PROGRA~1\\DAMNNF~1\\DAMNNF~1.EXE\"" + " " + "\"%1\""));
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(this,ex.ToString());
				}
				finally       
				{
					if (regSFVAssoc != null)
						regKeySFVAssoc.Close();
					if(regMainMenu != null)
						regKeyMainMenu.Close();
					if(regShellMenu != null)
						regKeyShellMenu.Close();
					if(regOpenMenu != null)
						regKeyOpenMenu.Close();
					if(regCmdMenu != null)
						regKeyCmdMenu.Close();
				}
			}
			else      //REMOVE SETTINGS FROM REGISTRY
			{
				try
				{

					RegistryKey reg = Registry.ClassesRoot.OpenSubKey(regSFVAssoc);
					if(reg != null)
					{
						reg.Close();
						Registry.ClassesRoot.DeleteSubKey(regSFVAssoc);
					}

					reg = Registry.ClassesRoot.OpenSubKey(regCmdMenu);
					if(reg != null)
					{
						reg.Close();
						Registry.ClassesRoot.DeleteSubKey(regCmdMenu);
					}

					reg = Registry.ClassesRoot.OpenSubKey(regOpenMenu);
					if(reg != null)
					{
						reg.Close();
						Registry.ClassesRoot.DeleteSubKey(regOpenMenu);
					}

					reg = Registry.ClassesRoot.OpenSubKey(regShellMenu);
					if(reg != null)
					{
						reg.Close();
						Registry.ClassesRoot.DeleteSubKey(regShellMenu);
					}

					reg = Registry.ClassesRoot.OpenSubKey(regMainMenu);
					if(reg != null)
					{
						reg.Close();
						Registry.ClassesRoot.DeleteSubKey(regMainMenu);
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(this,ex.ToString());
				}
			}

			//  ACTION:  remember WINDOW SIZE  +  PROGRAM SETTINGs ---------------
			//TO  Be SAVE IN REG ON PROGRAM EXIT!!!


		}

		private void options_reset(object sender, System.EventArgs e)
		{
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					chkOptionsConext.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("ContextMenus"));
					chkRememWinSize.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("WindowSize"));
					chkRememResSettings.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("RememberSettings"));
					chkAutoRun.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("AutoRun"));
					chkAutoQuit.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("AutoQuit"));
					chkDelSamples.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelSample"));
					chkDelSubs.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelSubs"));
					chkDelNFO.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelNFO"));
					chkDelDIZ.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("DelDIZ"));
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,ex.ToString());
			}
			finally       
			{
				if(regKeyUserConfMenu != null)
					regKeyUserConfMenu.Close();
			}
		}

		//Deletes files from specified path that have the specified extensions

		private void delete_ext(string path, string ext)
		{
			string[] fileNames = System.IO.Directory.GetFiles(path);

			foreach ( string strFiles in fileNames )
			{
				System.IO.FileInfo info = new System.IO.FileInfo(strFiles);

				if (info.Extension.ToLower() == ext.ToLower())
				{
					System.IO.File.Delete(info.ToString());
				}
			}
		}

		//Deletes folders from specified path that match the specified name

		private void delete_folder(string path, string name)
		{
			string[] folderNames = System.IO.Directory.GetDirectories(path);

			foreach ( string strFolders in folderNames )
			{
				System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(strFolders);

				if (info.Name.ToLower() == name.ToLower())
				{
					System.IO.Directory.Delete(info.ToString(),true);
				}
			}
		}

		private void btnRecursiveScan_Click(object sender, System.EventArgs e)
		{
			//DIR VALIDATION
			if ( (edtRecursiveTopDir.Text == "")  || (edtRecursiveTopDir.TextLength == 0) ) 
			{	
				MessageBox.Show( this, "You have not entered a valid location path.",
					"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartScanCleanUp();
				return;
			}
			else if ( (edtRecursiveTopDir.Text.Length < 2) || (edtRecursiveTopDir.Text.Substring( 0, 1 ) != "\\") 
				&& (edtRecursiveTopDir.Text.Substring( 1, 1 ) != "\\") )
			{
				if ( (edtRecursiveTopDir.Text.Length < 2) || (edtRecursiveTopDir.Text.Substring( 1, 1 ) != ":" ) )
				{
					MessageBox.Show( this, "You entered an invalid location path. The location" +
						"\nyou wish to use must begin with a drive letter or network path. Some" +
						"\nexamples are: D:\\, or \\\\computername\\sharename, or C:\\dir",
						"Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					RestartScanCleanUp();
					return;
				}
			}
			else if ( !(new DirectoryInfo( edtRecursiveTopDir.Text.Trim() ).Exists ) )
			{
				MessageBox.Show( this, "The Top-Level Directory chosen does not Exist! \n"
					, "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				RestartScanCleanUp();
				return;
			}

			try
			{
				getDirsFiles(edtRecursiveTopDir.Text.Trim());

				MessageBox.Show(this,"Scan Completed!", "Completed", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
			}
			catch
			{
				MessageBox.Show(this,"An Directory Error Occured somewhere, \n" +
					"and not all dirs were added, try moving down a Directory Level to resolve."
					, "Invalid Scan Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			}
		}

		//this is the recursive dir finding function
		public void getDirsFiles(string d)
		{
			string[] fileNames = System.IO.Directory.GetFiles(d);

			foreach (string strFiles in fileNames)
			{
				System.IO.FileInfo info = new System.IO.FileInfo(strFiles);

				if (info.Extension.ToLower() == ".sfv")
				{
					chklistboxRecursive.Items.Add(info.ToString(),true);
					Application.DoEvents();
				}
			}
            
			//Iterate getDirsFiles untill all folders found

			string[] folderNames = System.IO.Directory.GetDirectories(d);

			foreach ( string strFolders in folderNames )
			{
				getDirsFiles(strFolders);
			}
		}

		private void btnRecursiveFileTopDir_Click(object sender, System.EventArgs e)
		{
			if((folderBrowserDialog.ShowDialog() == DialogResult.OK))
			{
				edtRecursiveTopDir.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void btnRecursiveVerify_Click(object sender, System.EventArgs e)
		{
			//CHECK IF USERS HAVE SELECTED FILES TO SCAN
			if ( this.chklistboxRecursive.Items.Count == 0 )
			{
				MessageBox.Show( this, "Please Scan files first!",
					Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				RestartScanCleanUp();
				return;
			}

			bVerifyScanRequested = true;
			bRecursiveScanRequested = true;
			pbarVerifySystemScan.Enabled = true;
			pbarVerifySystemScan.ProgressBarColor = System.Drawing.Color.Black;
			this.lstVerifySFV.Items.Clear();
			this.lstRecursiveSFVs.Items.Clear();
			edtVerifyFileSFV.Text = edtVerifyFileSFV.Text.Trim();
			edtVerifyFileSFV.Enabled = false;
			btnVerifyFileSFV.Enabled = false;
			edtVerifySingleFile.Enabled = false;
			btnVerifySingleFile.Enabled = false;

			this.btnVerify.Text = "Abort Scan";
			btnVerify.Click -= new EventHandler(this.VerifyScan);
			btnVerify.Click += new EventHandler(this.StopScan);

			this.btnRecursiveVerify.Text = "Abort Scan";
			btnRecursiveVerify.Click -= new EventHandler(this.btnRecursiveVerify_Click);
			btnRecursiveVerify.Click += new EventHandler(this.StopScan);

			//RUN CHECK AND VERIFY
			myState = new CRCState();
			
			string []sfvFiles = new string[this.chklistboxRecursive.CheckedItems.Count];
			this.chklistboxRecursive.CheckedItems.CopyTo( sfvFiles, 0 );

			foreach ( string sfvName in sfvFiles )
			{
				if ( ( sfvName != null ) && ( sfvName != "" ) )
				{
					myState.setSFVFileNames(sfvName);
				}
			}

			tabMain.SelectedTab = tabValidate;

            //PUT SETTINGS INTO myState (required as the new thread cant cross thread call to UI)
            if (chkVerifyShowFailed.Checked) { myState.boolShowFailed = true; }
            if (chkVerifyUnrar.Checked) { myState.boolUnrar = true; }
            if (chkVerifyDel.Checked) { myState.boolDelRars = true; }
            if (chkVerifyDelSFV.Checked) { myState.boolDelSFV = true; }
            if (chkVerifyBG.Checked) { myState.boolVerBG = true; }

            //RUN PROGRAM IN BACKGROUND MINIMISED TO TRAY
            if (chkVerifyBG.Checked) { this.Hide(); }

			dtVerifyScanStart = DateTime.Now;

			pbarVerifySystemScan.Value = 0;
			pbarVerifySystemScan.Minimum = 0;
			VerifyCRCThread = new System.Threading.Thread( new System.Threading.ThreadStart( VerifyCRC ));
			VerifyCRCThread.IsBackground = true;
			VerifyCRCThread.Name = "Verify File CRC";
			VerifyCRCThread.Priority = System.Threading.ThreadPriority.Normal;
			VerifyCRCThread.Start();
		}

		private void btnRecursiveScanClear_Click(object sender, System.EventArgs e)
		{
			chklistboxRecursive.Items.Clear();
		}

		private void pBoxMMenu5_Click(object sender, System.EventArgs e)
		{
			AboutDlg aDlg = new AboutDlg();
			aDlg.ShowDialog( this );
		}

		private void pBoxMMenu1_Click(object sender, System.EventArgs e)
		{
			tabMain.SelectedTab = tabCreate;
		}

		private void pBoxMMenu2_Click(object sender, System.EventArgs e)
		{
			tabMain.SelectedTab = tabValidate;
			chkVerifySingleFile.Checked = false;
			chkVerifySingleFile_CheckedChanged(sender, e);
		}

		private void pBoxMMenu3_Click(object sender, System.EventArgs e)
		{
			tabMain.SelectedTab = tabValidate;
			chkVerifySingleFile.Checked = true;
			chkVerifySingleFile_CheckedChanged(sender, e);
		}

		private void pBoxMMenu4_Click(object sender, System.EventArgs e)
		{
			tabMain.SelectedTab = tabRecursive;
		}

	}
}