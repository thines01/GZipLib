using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipLib
{
   /// <summary>
   /// Holds static methods for compressing and decompressing files
   /// using the GZipStream.
   /// </summary>
   public static class CGZipLib
   {
      public static int m_intBUFF_SIZE = 500000; // default buffer size.

      /// <summary>
      /// Compresses a file into a .gz file
      /// Manish.txt becomes Manish.txt.gz
      /// </summary>
      /// <param name="strUncompressedFileName">Manish.txt</param>
      /// <param name="strError">output from any Exception</param>
      /// <returns>bool(true) if succeeded</returns>
      public static bool Compress(string strUncompressedFileName, ref string strError)
      {
         return Compress(strUncompressedFileName, (strUncompressedFileName + ".gz"), ref strError);
      }
      /// <summary>
      /// Compresses a file into a .gz file
      /// </summary>
      /// <param name="strUncompressedFileName">Manish.txt</param>
      /// <param name="strGzFileName">Manish.txt.gz</param>
      /// <param name="strError">output from any Exception</param>
      /// <returns></returns>
      public static bool Compress(string strUncompressedFileName, string strGzFileName, ref string strError)
      {
         bool blnRetVal = true;
         try
         {
            FileStream fileIn = File.OpenRead(strUncompressedFileName);
            GZipStream gzFile = new GZipStream(
               new StreamWriter(strGzFileName).BaseStream,
               CompressionMode.Compress, false);

            byte[] arr_bytes = new byte[m_intBUFF_SIZE];
            int intNumBytesRead = -1;

            while (!intNumBytesRead.Equals(0))
            {
               if (!(intNumBytesRead = fileIn.Read(arr_bytes, 0, arr_bytes.Length)).Equals(0))
               {
                  gzFile.Write(arr_bytes, 0, intNumBytesRead);
               }
            }

            gzFile.Close();
            fileIn.Close();
         }
         catch (Exception exc)
         {
            blnRetVal = false;
            strError = exc.Message;
         }

         return blnRetVal;
      }
      /// <summary>
      /// Decompresses a .gz file
      /// Manish.txt.gz becomes Manish.txt
      /// </summary>
      /// <param name="strGzFileName">Manish.txt.gz</param>
      /// <param name="strError">output from any Exception</param>
      /// <returns>bool(true) if succeeded</returns>
      public static bool Decompress(string strGzFileName, ref string strError)
      {
         string[] arr_strFileName = strGzFileName.Split('.');
         StringBuilder sbFileName = new StringBuilder();
         for (int intLoop = 0; intLoop < (arr_strFileName.Length - 1); intLoop++)
         {
            sbFileName.Append(arr_strFileName[intLoop] + '.');
         }

         return Decompress(strGzFileName, sbFileName.ToString().TrimEnd('.'), ref strError);
      }
      /// <summary>
      /// Decompresses a .gz file into the named uncompressed file
      /// </summary>
      /// <param name="strGzFileName">Manish.txt.gz</param>
      /// <param name="strUncompressedFileName">Manish.txt</param>
      /// <param name="strError">output from any Exception</param>
      /// <returns>bool(true) if succeeded</returns>
      public static bool Decompress(string strGzFileName, string strUncompressedFileName, ref string strError)
      {
         bool blnRetVal = true;

         try
         {
            File.Delete(strUncompressedFileName); //precaution
            GZipStream fileIn = 
               new GZipStream(
                  new FileStream(
                     strGzFileName, FileMode.Open, FileAccess.Read, FileShare.Read), 
                     CompressionMode.Decompress);

            byte[] arr_bytes = new byte[m_intBUFF_SIZE];
            FileStream fileOut = new FileStream(strUncompressedFileName, FileMode.CreateNew, FileAccess.Write);
            //
            int intNumBytesRead = -1;
            //
            while (!(intNumBytesRead = fileIn.Read(arr_bytes, 0, arr_bytes.Length)).Equals(0))
            {
               fileOut.Write(arr_bytes, 0, intNumBytesRead);
            }

            fileOut.Close();
            fileIn.Close();
         }
         catch (Exception exc)
         {
            blnRetVal = false;
            strError = exc.Message;
         }

         return blnRetVal;
      }
   }
}
