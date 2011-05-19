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
    Filename: AddFilesDlg.cs
    Function: easier dialog for adding sfv files
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MTK.WRoX_SFV
{
	/// <summary>
	/// Summary description for AddFilesDlg.
	/// </summary>
	public class AddFilesDlg : System.Windows.Forms.Form
	{
		private Microsoft.VisualBasic.Compatibility.VB6.DriveListBox driveLstBox;
		private Microsoft.VisualBasic.Compatibility.VB6.DirListBox dirLstBox;
		private Microsoft.VisualBasic.Compatibility.VB6.FileListBox fileLstBox;
		private System.Windows.Forms.ListBox lstBoxSelectedFiles;
		private System.Windows.Forms.Button btnAddFile;
		private System.Windows.Forms.Button btnMinusFile;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox grpOptions;
		private System.Windows.Forms.ComboBox cmbBoxOptions;
		private System.Windows.Forms.Button btnAddAllFiles;
		/// <summary>
		/// Vars used in app
		/// </summary>
		/// 
		// Defines a delegate. Sender is the object that is being returned to the other form.
		public delegate void PassControl(object sender);

		// Declare a new instance of the delegate (null)
		public PassControl passControl;
		private Point mouseOffset;
		private int filesSelected = 0;
		private string fileSelected = "";
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddFilesDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddFilesDlg));
			this.driveLstBox = new Microsoft.VisualBasic.Compatibility.VB6.DriveListBox();
			this.dirLstBox = new Microsoft.VisualBasic.Compatibility.VB6.DirListBox();
			this.fileLstBox = new Microsoft.VisualBasic.Compatibility.VB6.FileListBox();
			this.lstBoxSelectedFiles = new System.Windows.Forms.ListBox();
			this.btnAddFile = new System.Windows.Forms.Button();
			this.btnMinusFile = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.grpOptions = new System.Windows.Forms.GroupBox();
			this.cmbBoxOptions = new System.Windows.Forms.ComboBox();
			this.btnAddAllFiles = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grpOptions.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// driveLstBox
			// 
			this.driveLstBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.driveLstBox.Location = new System.Drawing.Point(6, 6);
			this.driveLstBox.Name = "driveLstBox";
			this.driveLstBox.Size = new System.Drawing.Size(228, 22);
			this.driveLstBox.TabIndex = 0;
			this.driveLstBox.SelectedIndexChanged += new System.EventHandler(this.driveLstBox_SelectedIndexChanged);
			// 
			// dirLstBox
			// 
			this.dirLstBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dirLstBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dirLstBox.HorizontalScrollbar = true;
			this.dirLstBox.IntegralHeight = false;
			this.dirLstBox.Location = new System.Drawing.Point(6, 33);
			this.dirLstBox.Name = "dirLstBox";
			this.dirLstBox.Size = new System.Drawing.Size(228, 132);
			this.dirLstBox.TabIndex = 1;
			this.dirLstBox.Change += new System.EventHandler(this.dirLstBox_SelectedIndexChanged);
			this.dirLstBox.SelectedIndexChanged += new System.EventHandler(this.dirLstBox_SelectedIndexChanged);
			// 
			// fileLstBox
			// 
			this.fileLstBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.fileLstBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fileLstBox.Location = new System.Drawing.Point(240, 6);
			this.fileLstBox.Name = "fileLstBox";
			this.fileLstBox.Pattern = "*.*";
			this.fileLstBox.Size = new System.Drawing.Size(237, 158);
			this.fileLstBox.TabIndex = 4;
			this.fileLstBox.DoubleClick += new System.EventHandler(this.btnAddFile_Click);
			// 
			// lstBoxSelectedFiles
			// 
			this.lstBoxSelectedFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstBoxSelectedFiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lstBoxSelectedFiles.HorizontalScrollbar = true;
			this.lstBoxSelectedFiles.Location = new System.Drawing.Point(6, 171);
			this.lstBoxSelectedFiles.Name = "lstBoxSelectedFiles";
			this.lstBoxSelectedFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstBoxSelectedFiles.Size = new System.Drawing.Size(315, 132);
			this.lstBoxSelectedFiles.TabIndex = 5;
			this.lstBoxSelectedFiles.DoubleClick += new System.EventHandler(this.btnMinusFile_Click);
			// 
			// btnAddFile
			// 
			this.btnAddFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddFile.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnAddFile.Location = new System.Drawing.Point(327, 171);
			this.btnAddFile.Name = "btnAddFile";
			this.btnAddFile.Size = new System.Drawing.Size(72, 21);
			this.btnAddFile.TabIndex = 6;
			this.btnAddFile.Text = "+";
			this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
			// 
			// btnMinusFile
			// 
			this.btnMinusFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMinusFile.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnMinusFile.Location = new System.Drawing.Point(405, 171);
			this.btnMinusFile.Name = "btnMinusFile";
			this.btnMinusFile.Size = new System.Drawing.Size(72, 21);
			this.btnMinusFile.TabIndex = 7;
			this.btnMinusFile.Text = "--";
			this.btnMinusFile.Click += new System.EventHandler(this.btnMinusFile_Click);
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(405, 276);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(72, 27);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// grpOptions
			// 
			this.grpOptions.Controls.Add(this.cmbBoxOptions);
			this.grpOptions.Controls.Add(this.btnAddAllFiles);
			this.grpOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpOptions.Location = new System.Drawing.Point(327, 195);
			this.grpOptions.Name = "grpOptions";
			this.grpOptions.Size = new System.Drawing.Size(150, 75);
			this.grpOptions.TabIndex = 9;
			this.grpOptions.TabStop = false;
			this.grpOptions.Text = "Add All Options:";
			// 
			// cmbBoxOptions
			// 
			this.cmbBoxOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbBoxOptions.Items.AddRange(new object[] {
															   "ALL FILES (*.*)",
															   "RAR ARCHIVE",
															   ".rar",
															   ".zip",
															   ".ace",
															   ".iso",
															   ".bin",
															   ".tar",
															   ".gz",
															   ".tgz",
															   ".exe",
															   ".avi",
															   ".mp3",
															   ".rpm",
															   ".nrg"});
			this.cmbBoxOptions.Location = new System.Drawing.Point(6, 18);
			this.cmbBoxOptions.Name = "cmbBoxOptions";
			this.cmbBoxOptions.Size = new System.Drawing.Size(138, 21);
			this.cmbBoxOptions.TabIndex = 0;
			this.cmbBoxOptions.Text = "RAR ARCHIVE";
			// 
			// btnAddAllFiles
			// 
			this.btnAddAllFiles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddAllFiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnAddAllFiles.Location = new System.Drawing.Point(6, 45);
			this.btnAddAllFiles.Name = "btnAddAllFiles";
			this.btnAddAllFiles.Size = new System.Drawing.Size(138, 21);
			this.btnAddAllFiles.TabIndex = 10;
			this.btnAddAllFiles.Text = "Add";
			this.btnAddAllFiles.Click += new System.EventHandler(this.btnAddAllFiles_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(327, 276);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 27);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.lstBoxSelectedFiles);
			this.panel1.Controls.Add(this.driveLstBox);
			this.panel1.Controls.Add(this.btnAddFile);
			this.panel1.Controls.Add(this.dirLstBox);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnMinusFile);
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.fileLstBox);
			this.panel1.Controls.Add(this.grpOptions);
			this.panel1.Location = new System.Drawing.Point(5, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(483, 309);
			this.panel1.TabIndex = 11;
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseMove);
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
			// 
			// AddFilesDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.DarkGray;
			this.ClientSize = new System.Drawing.Size(493, 319);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AddFilesDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddFilesDlg";
			this.TopMost = true;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleBar_MouseMove);
			this.grpOptions.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			// If the delegate was instantiated, then call it
			if (passControl != null)
			{
				passControl(lstBoxSelectedFiles);
			}

			// Close form
			//this.Hide();
			Dispose(true);
		}
					

		private void driveLstBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.dirLstBox.Path = this.driveLstBox.Drive;
				this.fileLstBox.Path = this.dirLstBox.Path;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

		private void dirLstBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.fileLstBox.Path = this.dirLstBox.Path;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error); 
			}
		}

		private void btnAddFile_Click(object sender, System.EventArgs e)
		{
			try
			{
				if ((this.fileLstBox.SelectedItem.ToString() == "") == false)
				{
					if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
					{
						this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.SelectedItem));
						filesSelected++;
					}
					else
					{
						this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.SelectedItem));
						filesSelected++;
					}
				}
			}
			catch(Exception ex)
			{
			//	MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error); 
			}
		}

		private void btnMinusFile_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.lstBoxSelectedFiles.Items.Remove(this.lstBoxSelectedFiles.SelectedItem);
				filesSelected--;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error); 
			}
		}

		private void btnAddAllFiles_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (cmbBoxOptions.Text == "ALL FILES (*.*)")              //ALL FILES
				{
					for (int i=0; i<=1000; i++) 
					{
						if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
						{
							this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.get_Items(i)));
						}
						else
						{
							this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.get_Items(i)));
						}
					}
				}
				else if (cmbBoxOptions.Text == "RAR ARCHIVE")             //FULL & COMPLETE RAR ARCHIVES < .r100
				{
					for (int i=0; i<=500; i++) 
					{
						fileSelected = this.fileLstBox.get_Items(i);

						if (fileSelected.EndsWith((".rar")))
						{
							if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
							{
								this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.get_Items(i)));
							}
							else
							{
								this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.get_Items(i)));
							}
						}

						for (int c=0; c<=100; c++) 
						{
							if (fileSelected.EndsWith((".r0"+c)))
							{
								if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
								{
									this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.get_Items(i)));
								}
								else
								{
									this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.get_Items(i)));
								}
							}
							else if (fileSelected.EndsWith((".r"+c)))
							{
								if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
								{
									this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.get_Items(i)));
								}
								else
								{
									this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.get_Items(i)));
								}
							}
						}
					}
				}
				else             //Uses just the extension specified in combobox's text
				{
					for (int i=0; i<=1000; i++) 
					{
						fileSelected = this.fileLstBox.get_Items(i);
						if (fileSelected.EndsWith(this.cmbBoxOptions.Text))
						{
							if (this.fileLstBox.Path.EndsWith( "\\" ))     //corrects issues with backslashes added to fileLstBox
							{
								this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+this.fileLstBox.get_Items(i)));
							}
							else
							{
								this.lstBoxSelectedFiles.Items.Add((this.fileLstBox.Path+"\\"+this.fileLstBox.get_Items(i)));
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				//do nothing
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			// Close form
			Dispose(true);
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
	}
}
