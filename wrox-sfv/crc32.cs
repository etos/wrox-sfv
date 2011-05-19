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
    Filename: crc32.cs
    Function: main CRC guts, adapted/edited from bloodhounds src
*/

using System;

namespace MTK.WRoX_SFV
{
	
	/// <summary>
	/// Generate a table for a byte-wise 32-bit CRC calculation on the polynomial:
	/// x^32+x^26+x^23+x^22+x^16+x^12+x^11+x^10+x^8+x^7+x^5+x^4+x^2+x+1.
	///
	/// Polynomials over GF(2) are represented in binary, one bit per coefficient,
	/// with the lowest powers in the most significant bit.  Then adding polynomials
	/// is just exclusive-or, and multiplying a polynomial by x is a right shift by
	/// one.  If we call the above polynomial p, and represent a byte as the
	/// polynomial q, also with the lowest power in the most significant bit (so the
	/// byte 0xb1 is the polynomial x^7+x^3+x+1), then the CRC is (q*x^32) mod p,
	/// where a mod b means the remainder after dividing a by b.
	///
	/// This calculation is done using the shift-register method of multiplying and
	/// taking the remainder.  The register is initialized to zero, and for each
	/// incoming bit, x^32 is added mod p to the register if the bit is a one (where
	/// x^32 mod p is p+x^32 = x^26+...+1), and the register is multiplied mod p by
	/// x (which is shifting right by one and adding x^32 mod p if the bit shifted
	/// out is a one).  We start with the highest power (least significant bit) of
	/// q and repeat for all eight bits of q.
	///
	/// The table is simply the CRC of all possible eight bit values.  This is all
	/// the information needed to generate CRC's on data a byte at a time for all
	/// combinations of CRC register values and incoming bytes.
	/// </summary>
	public sealed class crc32/* : ICloneable*/
	{
		/// <summary>
		/// Private static unsigned 32-bit number used to mask the 32-bit CRC.
		/// </summary>
		private static UInt32 HighBitMask = 0xFFFFFFFF;
		
		/// <summary>
		/// Private constant integer used to determine the number of bytes to buffer
		/// when reading files.
		/// </summary>
		private const int BUFFER_SIZE = 1024;
		
		/// <summary>
		/// Private unsigned 32-bit number used to track the CRC number for the whole process.
		/// </summary>
		private UInt32 LifetimeCRC = 0;

		/// <summary>
		/// Private static array containing unsigned 32-bit numbers of all the possible
		/// CRC values for a byte. This is subsequently used to generate the CRC values
		/// for files.
		/// </summary>
		private static UInt32[] CRCTable;

		/// <summary>
		/// If set to to true subsequent constructor calls will not reinitialize the table.
		/// </summary>
		private static bool isCRCTableInitialized = false;

		/// <summary>
		/// Public constructor where the <code>CRCTable</code> is initialized.
		/// </summary>
		public crc32()
		{
			if ( !isCRCTableInitialized ) 
			{
				unchecked
				{
					// This is the official polynomial used by CRC32 in PKZIP, WINZIP,
					// and TCP packets. Often the polynomial is shown reversed as 0x04C11DB7.
					UInt32 CRC32_POLYNOMIAL = 0xEDB88320;

					// Create table
					CRCTable = new UInt32[256];

					// Holds temporary crc
					UInt32 crc;

					for ( UInt32 i = 0; i < 256; i++ )
					{
						crc = i;

						for ( UInt32 j = 8; j > 0; j-- )
						{
							if ( (crc & 1) == 1 )
								crc = ( crc >> 1 ) ^ CRC32_POLYNOMIAL;
							else
								crc >>= 1;
						}
					
						CRCTable[i] = crc;
					}

					isCRCTableInitialized = true;
				}
			}
		}
		
		/// <summary>
		/// Returns the CRC32 data checksum computed so far for the media or directory
		/// (e.g. floppy disk, CDROM, folder).
		/// </summary>
		public UInt32 FinalValue 
		{
			get 
			{
				return LifetimeCRC;
			}
		}
		
		/// <summary>
		/// Resets the CRC32 data checksum as if no update was ever called. Use this
		/// method when you are re-using a reference to a <code>CRC32</code> object
		/// and you wish to run a new CRC check.
		/// </summary>
		public void Reset() 
		{ 
			LifetimeCRC = 0; 
		}

		/// <summary>
		/// Performs a CRC check on a file and updates the CRC32 checksum for the
		/// whole process.
		/// </summary>
		/// <param name="file">The System.IO.FileStream to scan.</param>
		/// <returns>Returns an unsigned 32-bit number representing the CRC value for the file.</returns>
		public string Update( System.IO.FileStream file )
		{
			unchecked
			{	
				byte[] buffer = new byte[BUFFER_SIZE];
				int readSize = BUFFER_SIZE;

				// Currently we do a "straight" CRC where only the data
				// is collected. Previous builds included the file name
				// as part of the CRC, to prevent getting CRCs with 0
				// if the file is empty. However, this was changed and is
				// no longer done.

				// Used when file names not included
				UInt32 crc = HighBitMask;

				// Include file names into CRC
				// UInt32 crc = Update( file.Name );

				try 
				{
					file.Lock( 0, file.Length );

					int count = file.Read(buffer, 0, readSize);
					while (count > 0)
					{
						
						for (int i = 0; i < count; i++)
							crc = ((crc) >> 8) ^ CRCTable[(buffer[i]) ^ ((crc) & 0x000000FF)];
						
						//ProcessBuffer( ref buffer, ref crc );

						count = file.Read(buffer, 0, readSize);
					}

					file.Unlock( 0, file.Length );
				}
				catch ( System.Exception e )
				{
					throw e;
				}
				
				crc = ~crc;
				LifetimeCRC ^= crc;

				//Prefix 0's to CRC's less than 8 chars
				string temp = crc.ToString("X");
				int intemp = ( 8 - temp.Length );
				do 
				{
					if ( temp.Length < 8 )
					{
						temp = "0" + temp;
					}
					intemp--;
				}
				while (intemp>0);

				return temp;

				//return crc;
			}
		}

		private UInt32 Update( string FullPathToFile )
		{
			unchecked
			{	
				// Creates an encoder that will map each Unicode character to 2 bytes
				System.Text.UnicodeEncoding myEncoder = new System.Text.UnicodeEncoding();
				
				// Include file name only, strip off path information
				// This is done so we get the same CRC no matter if the CD is in drive G
				// on one machine and drive F on another, kappish?

				int index = FullPathToFile.LastIndexOf( '\\' );
				string strFileName = FullPathToFile.Substring( index + 1 );

				// Convert file name to byte array
				
				int count = myEncoder.GetByteCount( strFileName );
				byte[] buffer = new byte[count];
				buffer = myEncoder.GetBytes(strFileName);

				UInt32 crc = HighBitMask;

				//ProcessBuffer( ref buffer, ref crc );
				for (int i = 0; i < count; i++)
					crc = ((crc) >> 8) ^ CRCTable[(buffer[i]) ^ ((crc) & 0x000000FF)];
				
				crc = ~crc;
				LifetimeCRC ^= crc;

				return crc;
			}
		}

		private unsafe UInt32 ProcessBuffer( ref byte[] buffer, ref UInt32 crc )
		{
			for (int i = 0; i < buffer.Length; i++)
				crc = ((crc) >> 8) ^ CRCTable[(buffer[i]) ^ ((crc) & 0x000000FF)];

			return crc;
		}


	}
}
