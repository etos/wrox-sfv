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
    Filename: UnrarDlg.cs
    Function: main gui to wrox-sfv
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;					// for reg key

namespace MTK.WRoX_SFV
{
	/// <summary>
	/// Summary description for UnrarDlg.
	/// </summary>
	public class UnrarDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblUnrarStatus;
		private System.Windows.Forms.Button btnUnrarAbort;
		private SmoothProgressBar.SmoothProgressBar pbarUnrar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Panel panel1;
		private Schematrix.Unrar unrar;
		private Point mouseOffset;
        private bool UnrarAutoClose = true;
//		private bool deleteRar = false;
//		private bool deleteSFV = false;
//		private string sfvFullPath = "";
		private string rarFullPath = "";
		string[] allRars;
		private System.Windows.Forms.CheckBox chkAutoClose;

		private System.Threading.Thread UnRarThread;  //pub so unrar.cs can stop mid unrar

		public UnrarDlg()
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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		//[STAThread]
		//static void Main()
//		{
	//		Application.Run(new UnrarDlg());
		//}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UnrarDlg));
			this.pbarUnrar = new SmoothProgressBar.SmoothProgressBar();
			this.lblUnrarStatus = new System.Windows.Forms.Label();
			this.btnUnrarAbort = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkAutoClose = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pbarUnrar
			// 
			this.pbarUnrar.Location = new System.Drawing.Point(8, 24);
			this.pbarUnrar.Maximum = 100;
			this.pbarUnrar.Minimum = 0;
			this.pbarUnrar.Name = "pbarUnrar";
			this.pbarUnrar.ProgressBarColor = System.Drawing.Color.Black;
			this.pbarUnrar.Size = new System.Drawing.Size(336, 16);
			this.pbarUnrar.TabIndex = 0;
			this.pbarUnrar.Value = 0;
			// 
			// lblUnrarStatus
			// 
			this.lblUnrarStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUnrarStatus.Location = new System.Drawing.Point(8, 8);
			this.lblUnrarStatus.Name = "lblUnrarStatus";
			this.lblUnrarStatus.Size = new System.Drawing.Size(336, 16);
			this.lblUnrarStatus.TabIndex = 1;
			this.lblUnrarStatus.Text = "Ready";
			// 
			// btnUnrarAbort
			// 
			this.btnUnrarAbort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUnrarAbort.Location = new System.Drawing.Point(224, 46);
			this.btnUnrarAbort.Name = "btnUnrarAbort";
			this.btnUnrarAbort.Size = new System.Drawing.Size(120, 20);
			this.btnUnrarAbort.TabIndex = 2;
			this.btnUnrarAbort.Text = "Abort";
			this.btnUnrarAbort.Click += new System.EventHandler(this.btnUnrarAbort_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.chkAutoClose);
			this.panel1.Controls.Add(this.btnUnrarAbort);
			this.panel1.Controls.Add(this.pbarUnrar);
			this.panel1.Controls.Add(this.lblUnrarStatus);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(352, 72);
			this.panel1.TabIndex = 4;
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseMove);
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
			// 
			// chkAutoClose
			// 
			this.chkAutoClose.Checked = true;
			this.chkAutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoClose.Location = new System.Drawing.Point(8, 44);
			this.chkAutoClose.Name = "chkAutoClose";
			this.chkAutoClose.Size = new System.Drawing.Size(160, 24);
			this.chkAutoClose.TabIndex = 3;
			this.chkAutoClose.Text = "Close Dialog on Complete";
			this.chkAutoClose.CheckedChanged += new System.EventHandler(this.chkAutoClose_CheckedChanged);
			// 
			// UnrarDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.ControlText;
			this.ClientSize = new System.Drawing.Size(358, 78);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UnrarDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "UnRAR";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
			this.Load += new System.EventHandler(this.Form_Load);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseMove);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnUnrarAbort_Click(object sender, System.EventArgs e)
		{
			if ( UnRarThread != null  &&  UnRarThread.IsAlive )  // thread is active
			{
				unrar.Close();
				unrar.Dispose();
				Application.DoEvents();
			}

			Dispose(true);
			this.Close();
		}


		private void Start()
		{
			try
			{
				// Create new unrar class and attach event handlers for
				// progress, missing volumes, and password
				unrar=new Schematrix.Unrar();
				AttachHandlers(unrar);

				// Set destination path for all files
				unrar.DestinationPath = System.IO.Path.GetDirectoryName(rarFullPath);

				// Open archive for extraction
				unrar.Open(rarFullPath, Schematrix.Unrar.OpenMode.Extract);

				// Extract each file found in hashtable
				while( unrar.ReadHeader() )
				{
					//this.pbarUnrar.Value=0;
                    SafeInvoke.Invoke(this, "pbarIncUnrar", 0);
					unrar.Extract();
				}

				unrar.Close();
			}
			catch(Exception ex)
			{
                SafeInvoke.Invoke(this, "MsgBox", ex.Message, "Unrar Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                MTK.WRoX_SFV.WRoXMain.bVerifyScanGood = false;
			}
			finally
			{
                if (MTK.WRoX_SFV.WRoXMain.bVerifyScanGood == true)
                {
                    SafeInvoke.Invoke(this, "UnrarStatus", "UnRar Completed!");
                }
                else if (MTK.WRoX_SFV.WRoXMain.bVerifyScanGood == false)
                {
                    SafeInvoke.Invoke(this, "UnrarStatus", "UnRar Failed!");
                    UnrarAutoClose = false;
                }
                
                if (this.unrar != null)
                {
                    unrar.Close();
                }

                //CHECK DLG AUTO-CLOSE
                if (UnrarAutoClose)
                {
                    SafeInvoke.Invoke(this, "CloseUnrar");
                }
			}
		}

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void pbarIncUnrar(int value)
        {
            this.pbarUnrar.Value = value;
            //this.pbarUnrar.Refresh();
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void UnrarStatus(string newlabel)
        {
            this.lblUnrarStatus.Text = newlabel;
            this.btnUnrarAbort.Text = "Close";
            this.Cursor = Cursors.Default;
        }

        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void CloseUnrar()
        {
            this.Close();
        }

        
        //run via SafeInvoke for safe Cross-Thread calls to UI
        public void MsgBox(string mainmsg, string titlemsg, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show(this, mainmsg, titlemsg, buttons, icon);
        }


		private void AttachHandlers(Schematrix.Unrar unrar)
		{
			unrar.ExtractionProgress+=new Schematrix.ExtractionProgressHandler(unrar_ExtractionProgress);
			unrar.MissingVolume+=new Schematrix.MissingVolumeHandler(unrar_MissingVolume);
			unrar.PasswordRequired+=new Schematrix.PasswordRequiredHandler(unrar_PasswordRequired);
		}

		private void unrar_ExtractionProgress(object sender, Schematrix.ExtractionProgressEventArgs e)
		{
			//lblUnrarStatus.Text = e.FileName;
			//pbarUnrar.Value=(int)e.PercentComplete;
            SafeInvoke.Invoke(this, "UnrarStatus", e.FileName);
            SafeInvoke.Invoke(this, "pbarIncUnrar", (int)e.PercentComplete);
		}

		private void unrar_MissingVolume(object sender, Schematrix.MissingVolumeEventArgs e)
		{
			Schematrix.TextInputDialog dialog=new Schematrix.TextInputDialog();
			dialog.Value=e.VolumeName;
			dialog.Prompt=string.Format("Volume is missing.  Correct or cancel.");
			if(dialog.ShowDialog()==DialogResult.OK)
			{
				e.VolumeName=dialog.Value;
				e.ContinueOperation=true;
			}
			else
				e.ContinueOperation=false;
		}

		private void unrar_PasswordRequired(object sender, Schematrix.PasswordRequiredEventArgs e)
		{
			Schematrix.TextInputDialog dialog=new Schematrix.TextInputDialog();
			dialog.Prompt=string.Format("Password is required for extraction.");
			dialog.PasswordChar='*';
			if(dialog.ShowDialog()==DialogResult.OK)
			{
				e.Password=dialog.Value;
				e.ContinueOperation=true;
			}
			else
				e.ContinueOperation=false;
		}


		private string StripFileName(string filename)
		{
			string p = filename;
			string []tempstr;
			p.Trim();
			tempstr = p.Split('\\',(char)10);
			string retstr = "";

			for (int i=0; i<(tempstr.Length-1); i++)
			{
				retstr += tempstr[i] + "\\";
			}

			return retstr;
		}

		public void UnrarStart()
		{
            this.Cursor = Cursors.WaitCursor;

			UnRarThread = new System.Threading.Thread( new System.Threading.ThreadStart( Start ));
			UnRarThread.IsBackground = true;
			UnRarThread.Name = "UnRar Files";
			UnRarThread.Priority = System.Threading.ThreadPriority.Normal;
			UnRarThread.Start();
		}


		//mouse events to drag entire window when clicking canvas

		private void titleBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouseOffset = new Point(-e.X, -e.Y);
		}

		private void titleBar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) 
			{
				Point mousePos = Control.MousePosition;
				mousePos.Offset(mouseOffset.X, mouseOffset.Y);
				Location = mousePos;
			}
		}

		public void PassUnrarDeets(string pathName)
		{
			rarFullPath = pathName;
		}

		private void chkAutoClose_CheckedChanged(object sender, System.EventArgs e)
		{
            //tell form if its checked
            if (chkAutoClose.Checked)
            {
                UnrarAutoClose = true;
            }
            else
            {
                UnrarAutoClose = false;
            }

            //save checked settings in registry for next time
			RegistryKey regKeyUserMenu = null;
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserMenu = Registry.CurrentUser.CreateSubKey(MTK.WRoX_SFV.WRoXMain.regUserMenu);
				if(regKeyUserMenu != null)
				{
					regKeyUserMenu.SetValue("","WRoX SFV USER DATA");
				}

				regKeyUserConfMenu = Registry.CurrentUser.CreateSubKey(MTK.WRoX_SFV.WRoXMain.regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					regKeyUserConfMenu.SetValue("","");
					regKeyUserConfMenu.SetValue("UnrarAutoClose",chkAutoClose.Checked.ToString());
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


		private void Form_Load(object sender, System.EventArgs e)
		{
			//check and restore saved program settings
			RegistryKey regKeyUserConfMenu = null;

			try
			{
				regKeyUserConfMenu = Registry.CurrentUser.OpenSubKey(MTK.WRoX_SFV.WRoXMain.regUserConfMenu);
				if(regKeyUserConfMenu != null)
				{
					if ( Convert.ToBoolean(regKeyUserConfMenu.GetValue("RememberSettings")) == true )
					{
						chkAutoClose.Checked = Convert.ToBoolean(regKeyUserConfMenu.GetValue("UnrarAutoClose"));
                        UnrarAutoClose = Convert.ToBoolean(regKeyUserConfMenu.GetValue("UnrarAutoClose"));
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

            //start the unrar process!!
            UnrarStart();
		}

	}
}
