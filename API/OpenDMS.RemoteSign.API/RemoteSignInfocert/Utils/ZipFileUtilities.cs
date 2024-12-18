using System;

namespace RemoteSignInfocert.Utils
{
    public class ZipFileUtilities
    {
        private static readonly byte[] ZipBytes1 = { 0x50, 0x4b, 0x03, 0x04 };
        private static readonly byte[] GzipBytes = { 0x1f, 0x8b };
        private static readonly byte[] TarBytes = { 0x1f, 0x9d };
        private static readonly byte[] LzhBytes = { 0x1f, 0xa0 };
        private static readonly byte[] Bzip2Bytes = { 0x42, 0x5a, 0x68 };
        private static readonly byte[] LzipBytes = { 0x4c, 0x5a, 0x49, 0x50 };
        private static readonly byte[] ZipBytes2 = { 0x50, 0x4b, 0x05, 0x06 };
        private static readonly byte[] ZipBytes3 = { 0x50, 0x4b, 0x07, 0x08 };

        private static byte[] GetFirstBytes(string filepath, int length)
        {
            using (var sr = new StreamReader(filepath))
            {
                sr.BaseStream.Seek(0, 0);
                var bytes = new byte[length];
                sr.BaseStream.Read(bytes, 0, length);

                return bytes;
            }
        }

        private static byte[] GetFirstBytes(byte[] content, int length)
        {
            byte[] result = new byte[4];
            Array.Copy(content, result, result.Length);
            return result;
        }

        public static bool IsZipFile(string filepath)
        {
            return IsCompressedData(GetFirstBytes(filepath, 5));
        }

        public static bool IsZipFile(byte[] content)
        {
            return IsCompressedData(GetFirstBytes(content, 5));
        }

        public static bool IsCompressedData(byte[] data)
        {
            foreach (var headerBytes in new[] { ZipBytes1, ZipBytes2, ZipBytes3, GzipBytes, TarBytes, LzhBytes, Bzip2Bytes, LzipBytes })
            {
                if (HeaderBytesMatch(headerBytes, data))
                    return true;
            }

            return false;
        }

        private static bool HeaderBytesMatch(byte[] headerBytes, byte[] dataBytes)
        {
            if (dataBytes.Length < headerBytes.Length)
                throw new ArgumentOutOfRangeException(nameof(dataBytes),
                    $"Passed databytes length ({dataBytes.Length}) is shorter than the headerbytes ({headerBytes.Length})");

            for (var i = 0; i < headerBytes.Length; i++)
            {
                if (headerBytes[i] == dataBytes[i]) continue;

                return false;
            }

            return true;
        }

    }

}
