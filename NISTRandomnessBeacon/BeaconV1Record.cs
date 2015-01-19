using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Threading;
namespace NISTRNGBeaconThingy.BeaconV1
{
    public class Record
    {
        static internal Int64 StartOfCurrentChain = 0;
        static public Int64 StartOfCurrentChainTimeStamp { get { return StartOfCurrentChain; } }

        public const string CACHE_DIR = ".\\Cache\\";
        internal const string XML_ROOTNODENAME = "record";
        internal const string XML_NODENAME_VER = "version";
        internal const string XML_NODENAME_FREQ = "frequency";
        internal const string XML_NODENAME_TIME = "timeStamp";
        internal const string XML_NODENAME_SEED = "seedValue";
        internal const string XML_NODENAME_PREV = "previousOutputValue";
        internal const string XML_NODENAME_SIG = "signatureValue";
        internal const string XML_NODENAME_OUT = "outputValue";
        internal const string XML_NODENAME_STATUS = "statusCode";

        public readonly string RawXml = null;

        // for Version 1.0, schema 0.1.0
        public readonly string Version;
        public readonly Int32 Frequency;
        public readonly Int64 TimeStamp;
        public readonly string SeedValue;
        public readonly string PreviousOutputValue;
        public readonly string SignatureValue;
        public readonly string OutputValue;
        public readonly Int32 StatusCode;

        internal byte[] _seedBytes = null;
        internal byte[] _prevOutBytes = null;
        internal byte[] _sigBytes = null;
        internal byte[] _outBytes = null;
        public byte[] SeedValueBytes { get { return (byte[])_seedBytes.Clone(); } }
        public byte[] PreviousOutputValueBytes { get { return (byte[])_prevOutBytes.Clone(); } }
        public byte[] SignatureValueBytes { get { return (byte[])_sigBytes.Clone() ; } }
        public byte[] OutputValueBytes { get { return (byte[])_outBytes.Clone(); } }

        internal Record(XElement rawXml, bool IsStartOfCurrentChain = false)
        {
            if (rawXml.Name.LocalName != XML_ROOTNODENAME ||
                rawXml.Element(XML_NODENAME_VER) == null ||
                rawXml.Element(XML_NODENAME_FREQ) == null ||
                rawXml.Element(XML_NODENAME_TIME) == null ||
                rawXml.Element(XML_NODENAME_SEED) == null ||
                rawXml.Element(XML_NODENAME_PREV) == null ||
                rawXml.Element(XML_NODENAME_SIG) == null ||
                rawXml.Element(XML_NODENAME_OUT) == null ||
                rawXml.Element(XML_NODENAME_STATUS) == null)
                throw new InvalidDataException("Not a valid record (incomplete or malformated xml node information).");
            this.Version = rawXml.Element(XML_NODENAME_VER).Value as string;
            if (string.IsNullOrWhiteSpace(this.Version) || this.Version != "Version 1.0")
                throw new InvalidDataException("Unexpected version (must be 'Version 1.0', was '" + (this.Version ?? "null") + "').");
            this.Frequency = Int32.Parse(rawXml.Element(XML_NODENAME_FREQ).Value);
            this.TimeStamp = Int64.Parse(rawXml.Element(XML_NODENAME_TIME).Value);
            this.SeedValue = rawXml.Element(XML_NODENAME_SEED).Value as string;
            this._seedBytes = Utilities.HexStringToBytes(this.SeedValue);
            if (this._seedBytes == null || this._seedBytes.Length != 64)
                throw new InvalidDataException("Unexpected number of seed bytes (must be 64 bytes).");
            this.PreviousOutputValue = rawXml.Element(XML_NODENAME_PREV).Value as string;
            this._prevOutBytes = Utilities.HexStringToBytes(this.PreviousOutputValue);
            if (this._prevOutBytes == null || this._prevOutBytes.Length != 64)
                throw new InvalidDataException("Unexpected number of previous output bytes (must be 64 bytes).");
            this.SignatureValue = rawXml.Element(XML_NODENAME_SIG).Value as string;
            this._sigBytes = Utilities.HexStringToBytes(this.SignatureValue);
            if (this._sigBytes == null || this._sigBytes.Length != 256)
                throw new InvalidDataException("Unexpected number of signature bytes (must be 256 bytes).");
            this.OutputValue = rawXml.Element(XML_NODENAME_OUT).Value as string;
            this._outBytes = Utilities.HexStringToBytes(this.OutputValue);
            if (this._outBytes == null || this._outBytes.Length != 64)
                throw new InvalidDataException("Unexpected number of output bytes (must be 64 bytes).");

            Int32 tempStatusCode = int.MinValue;
            if (!Int32.TryParse(rawXml.Element(XML_NODENAME_STATUS).Value, out tempStatusCode))
                throw new InvalidDataException(
                    "Unexpected Status Code value (must be 0,1,2, was '" + rawXml.Element(XML_NODENAME_STATUS).Value + "')");
            if (tempStatusCode < 0 || tempStatusCode > 2)
                throw new InvalidDataException(
                    "Unexpected Status Code value (must be 0,1,2, was '" + tempStatusCode + "')");
            this.StatusCode = tempStatusCode;
            this.RawXml = rawXml.ToString(SaveOptions.None);
            System.Diagnostics.Debug.Print("Record Constructor, timeStamp: " + this.TimeStamp.ToString());
            try
            {
                if (StartOfCurrentChain == 0 && IsStartOfCurrentChain)
                    StartOfCurrentChain = this.TimeStamp;
                else if (StartOfCurrentChain == 0)
                    StartOfCurrentChain = RetrieveStartOfCurrentChain().TimeStamp;
                Thread.MemoryBarrier();
            }
            catch { } // don't let the caching of this internal contstant fail a new record, just move along
        }

        public XElement ToXElement()
        {
            return XElement.Parse(RawXml);
        }

        public static Record FromXMLFile(string theXMLFilename)
        {
            return FromXML(theXMLFilename);
        }

        public static Record FromXML(string theXML)
        {
            XElement thing = XElement.Parse(theXML);
            return FromXML(thing);
        }

        public static Record FromXML(XElement element)
        {
            return new Record(element);
        }

        private static bool CacheRecordToDisk(Record theRecord)
        {
            try
            {
                if (!Directory.Exists(CACHE_DIR))
                    Directory.CreateDirectory(CACHE_DIR);
                System.IO.File.WriteAllText(CACHE_DIR + theRecord.TimeStamp.ToString().Trim() + ".xml", theRecord.RawXml);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("*Write to cache failed: " + ex.Message);
                return false; 
            }
        }

        public bool CacheToDisk()
        {
            return CacheRecordToDisk(this);
        }

        public static Record RetrieveRecordFromDiskCache(Int64 timeStamp)
        {
            if (!Directory.Exists(CACHE_DIR))
                return null;
            string stuff = null;
            try
            {
                stuff = System.IO.File.ReadAllText(CACHE_DIR + timeStamp.ToString().Trim() + ".xml");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("*Read from cache failed: " + ex.Message);
                return null;
            };
            return FromXML(stuff);
        }

        public static Record RetrieveLastFromDiskCache()
        {
            if(!System.IO.Directory.Exists(CACHE_DIR))
                return null;
            IEnumerable<string> rawfiles = System.IO.Directory.EnumerateFiles(CACHE_DIR, "*.xml", SearchOption.TopDirectoryOnly);
            Int64 max = 0;
            foreach(string f in rawfiles)
            {
                Int64 fnum = Int64.Parse(f.Replace(CACHE_DIR, "").Replace(".xml",""));
                if (fnum > max) max = fnum;
            }
            if(max == 0)
                return null;
            else
                return RetrieveRecordFromDiskCache(max);
        }

        public static Record RetrieveLastModifiedFromDiskCache()
        {
            if (!System.IO.Directory.Exists(CACHE_DIR))
                return null;
            IEnumerable<string> rawfiles = System.IO.Directory.EnumerateFiles(CACHE_DIR, "*.xml", SearchOption.TopDirectoryOnly);
            Int64 mostRecentTimeStamp = 0;
            DateTime mostRecentTime = DateTime.MinValue;
            foreach (string f in rawfiles)
            {
                DateTime fTime = new System.IO.FileInfo(f).LastWriteTime;
                if (fTime > mostRecentTime)
                {
                    mostRecentTimeStamp = Int64.Parse(f.Replace(CACHE_DIR, "").Replace(".xml", ""));
                    mostRecentTime = fTime;
                }
            }
            if (mostRecentTimeStamp == 0)
                return null;
            else
                return RetrieveRecordFromDiskCache(mostRecentTimeStamp);
        }


        public static Record RetrieveLast()
        {
            string response = null;
            try
            {
                response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/last");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("*RetrieveLast failed: " + ex.Message);
                throw ex;
            }
            Record result = new Record(XElement.Parse(response));
            if (result != null)
                CacheRecordToDisk(result);
            return result;
        }

        public static Record RetrieveByDate(DateTime when)
        {
            return RetrieveByTimeStamp(Utilities.GetNISTTimeCode(when));
        }

        /// <summary>
        /// Returns the specific record as indicated by the provided Time Stamp (which is in Unix Epoch Time, rounded to the minute).
        /// </summary>
        /// <remarks>CAUTION NOTE: Will return the NEAREST result (rounded up) if the exact timestamp isn't available.  This will be
        /// the start of the chain (if the number is too low), or a System.Net.WebException (404) if the number is too high.</remarks>
        /// <param name="timeStamp">The Unix Epoch timestamp.</param>
        /// <param name="CacheToDisk">Write the XML to a local \Cache\ sub-folder.</param>
        /// <returns>Record Object</returns>
        public static Record RetrieveByTimeStamp(Int64 timeStamp, bool ForceUpdateFromNIST = false)
        {
            Record result = null;
            if (!ForceUpdateFromNIST)
            {
                result = RetrieveRecordFromDiskCache(timeStamp);
                if (result == null)
                    result = RetrieveByTimeStamp(timeStamp, true);
            }
            if (ForceUpdateFromNIST || result == null)
            {
                string response = null;
                try
                {
                    response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/" + timeStamp.ToString().Trim());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("*RetrieveByTimeStamp failed: " + ex.Message);
                    throw ex;
                }
                result = FromXML(response);
                if (result == null)
                    throw new WebException(response);
                CacheRecordToDisk(result);
            }
            return result;
        }

        public static Record RetrieveNext(Int64 timeStamp)
        {
            string response = null;
            try
            {
                response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/next/" + timeStamp.ToString().Trim());
            }
            catch (WebException webEx)
            {
                if (webEx.Message.Contains("(404)"))
                {
                    System.Diagnostics.Debug.Print("*RetrieveNext failed, not found, so return null: " + webEx.Message);
                    return null;
                }
                else
                {
                    System.Diagnostics.Debug.Print("*RetrieveNext failed: " + webEx.Message);
                    throw webEx;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("*RetrieveNext failed, not WebException: " + ex.Message);
                throw ex;
            }
            Record result = new Record(XElement.Parse(response));
            if (result != null)
                CacheRecordToDisk(result);
            return result;
        }

        public static Record RetrievePrevious(Int64 timeStamp)
        {
            string response = null;
            try
            {
                response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/previous/" + timeStamp.ToString().Trim());
            }
            catch (WebException webEx)
            {
                if (webEx.Message.Contains("(404)"))
                {
                    System.Diagnostics.Debug.Print("*RetrievePrevious failed, not found, so return null: " + webEx.Message);
                    return null;
                }
                else
                {
                    System.Diagnostics.Debug.Print("*RetrievePrevious failed: " + webEx.Message);
                    throw webEx;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("*RetrievePrevious failed, not WebException: " + ex.Message);
                throw ex;
            }
            Record result = new Record(XElement.Parse(response));
            if (result != null)
                CacheRecordToDisk(result);
            return result;
        }

        public static Record RetrieveStartOfCurrentChain(bool ForceUpdateFromNist = false)
        {
            Record result = null;
            if (ForceUpdateFromNist || StartOfCurrentChain == 0)
            {
                Int64 timeStamp = Utilities.GetNISTTimeCode(DateTime.Now);
                string response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/start-chain/" + timeStamp.ToString().Trim());
                result = new Record(XElement.Parse(response), true);
                if (result == null)
                    throw new WebException(response);
                if (result.StatusCode != 1)
                    throw new InvalidDataException("Unexpected status code for start of chain.");
                CacheRecordToDisk(result);
            }
            else
            {
                if (StartOfCurrentChain != 0)
                    result = RetrieveRecordFromDiskCache(StartOfCurrentChain);
                if (result == null)
                    return RetrieveStartOfCurrentChain(true);
            }
            StartOfCurrentChain = result.TimeStamp;
            return result;
        }

        public static Record RetrieveStartOfChain(Int64 timeStamp)
        {
            string response = new WebClient().DownloadString("https://beacon.nist.gov/rest/record/start-chain/" + timeStamp.ToString().Trim());
            Record result = new Record(XElement.Parse(response));
            if (result == null)
                throw new WebException(response);
            if (result.StatusCode != 1)
                throw new InvalidDataException("Unexpected status code for start of chain.");
            CacheRecordToDisk(result);
            return result;
        }
    }
}