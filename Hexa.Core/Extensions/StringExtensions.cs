﻿#region License

//===================================================================================
//Copyright 2010 HexaSystems Corporation
//===================================================================================
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//===================================================================================
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//===================================================================================

#endregion

using System.Text;
using System.Security.Cryptography;

namespace System
{
	public static class MD5Helper
	{
		public static string CalculateMD5Hash(this string input)
		{
            byte[] inputBytes;
            byte[] hash;

			// step 1, calculate MD5 hash from input
            using (MD5 md5 = MD5.Create())
            {
                inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                hash = md5.ComputeHash(inputBytes);
            }

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2", System.Globalization.CultureInfo.InvariantCulture));
			}
			return sb.ToString();
		}
	}
}