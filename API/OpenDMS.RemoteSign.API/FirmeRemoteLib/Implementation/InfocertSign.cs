using FirmeRemoteLib.Interfaces;
using FirmeRemoteLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmeRemoteLib.Implementation
{
    public class InfocertSign : IRemoteSign
    {
        public void ClearDocumentQueue()
        {
            throw new NotImplementedException();
        }

        public void EnqueueDocument(string filename, byte[] fileToSign)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> MultiSignCades(string alias, string pin, string otp)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> MultiSignPades(string alias, string pin, string otp, FirmaPades firma)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendOTP(string alias)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> SignCades(string alias, string pin, string otp, string filename, byte[] filebyteToSign)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> SignPades(string alias, string pin, string otp, string filename, byte[] filebyteToSign, FirmaPades firma)
        {
            throw new NotImplementedException();
        }

        Task<(byte[] Result, string FileName)> IRemoteSign.MultiSignCades(string alias, string pin, string otp)
        {
            throw new NotImplementedException();
        }

		Task<(byte[] Result, string FileName)> IRemoteSign.MultiSignPades(string alias, string pin, string otp, FirmaPades firma)
		{
			throw new NotImplementedException();
		}
	}
}
