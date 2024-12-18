using FirmeRemoteLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmeRemoteLib.Interfaces
{
    public interface IRemoteSign
    {
        Task<bool> SendOTP(string alias);

        void EnqueueDocument(String filename, byte[] fileToSign);

        void ClearDocumentQueue();

        Task<byte[]> SignCades(string alias, string pin, string otp, String filename, byte[] filebyteToSign);

        //Task<byte[]> MultiSignCades(string alias, string pin, string otp);
        Task<(byte[] Result, string FileName)> MultiSignCades(string alias, string pin, string otp);

        Task<byte[]> SignPades(string alias, string pin, string otp, String filename, byte[] filebyteToSign, FirmaPades firma);

        Task<(byte[] Result, string FileName)> MultiSignPades(string alias, string pin, string otp, FirmaPades firma);

    }
}
