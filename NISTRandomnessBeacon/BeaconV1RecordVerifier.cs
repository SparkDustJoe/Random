using System;
using System.Text;
using System.IO;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;
using System.Net;

namespace NISTRNGBeaconThingy.BeaconV1
{
    public class RecordVerifier
    {
        private RsaDigestSigner MyRSA = null;

        public RecordVerifier()
        {
            Exception ex = null;
            if (!TryImportCertificate(".\\Cache\\beacon.cer", out ex))
            {
                //fail safe, try getting it direct and caching it
                byte[] response = new WebClient().DownloadData("https://beacon.nist.gov/certificate/beacon.cer");
                if (response != null)
                {
                    System.IO.File.WriteAllBytes(".\\Cache\\beacon.cer", response);
                    if (!TryImportCertificate(".\\Cache\\beacon.cer", out ex))
                        throw ex;
                }
                else
                    throw new WebException("Unable to retrieve cert from local cache or https://beacon.nist.gov/certificate/beacon.cer");
            }
                
        }

        public RecordVerifier(bool GetCertFromNIST)
        {
            Exception ex = null;
            if (!GetCertFromNIST)
            {
                if (!TryImportCertificate(".\\Cache\\beacon.cer", out ex))
                    throw ex;
            }
            else
            {
                byte[] response = new WebClient().DownloadData("https://beacon.nist.gov/certificate/beacon.cer");
                if (response != null)
                {
                    System.IO.File.WriteAllBytes(".\\Cache\\beacon.cer", response);
                    if (!TryImportCertificate(".\\Cache\\beacon.cer", out ex))
                        throw ex;
                }
                else
                    throw new WebException("Unable to retrieve cert from https://beacon.nist.gov/certificate/beacon.cer");
            }
        }

        public RecordVerifier(string CertificateFilename)
        {
            Exception ex = null;
            if (!TryImportCertificate(CertificateFilename, out ex))
                throw ex;

        }

        public bool TryImportCertificate(string CertificateFilename, out Exception Exception)
        {
            Exception = null;
            try
            {
                using (var certFileReader = System.IO.File.OpenText(CertificateFilename))
                {
                    var certReader = new PemReader(certFileReader);
                    X509Certificate cert = (X509Certificate)certReader.ReadObject();
                    certFileReader.Close();
                    RsaKeyParameters keyParamz = cert.GetPublicKey() as RsaKeyParameters;
                    MyRSA = new RsaDigestSigner(new Sha512Digest());
                    MyRSA.Init(false, keyParamz);
                }
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }
            return true;
        }

        public bool VerifySignature(Record theRecord)
        {
            if (theRecord == null)
                throw new ArgumentNullException("theRecord");
            if (MyRSA == null)
                throw new InvalidOperationException("Cannot verify a record without a certificate.");
            Sha512Digest sha512 = new Sha512Digest();
            sha512.Reset();
            byte[] compareOutputToSig = new byte[sha512.GetDigestSize()];
            sha512.BlockUpdate(theRecord.SignatureValueBytes, 0, theRecord.SignatureValueBytes.Length);
            sha512.DoFinal(compareOutputToSig, 0);
            string compareOutputToSigHex = Utilities.BytesToHexString(compareOutputToSig);
            if (compareOutputToSigHex.CompareTo(theRecord.OutputValue.ToLowerInvariant()) != 0)
            {
                // the SHA512 output of hashing the SignatureValue (bytes) does not produce the provided OutputValue
                // NOT VALID / TAMPERED WITH? / TRUNCATED?
                return false; 
            }
            // VERSION (UTF8 string) + FREQUENCY (4bytes) + TIME STAMP (8bytes) + SEED (64bytes) + 
            //    PREVIOUS OUTPUT (64bytes) + STATUS CODE (4bytes)
            // All values should be BIG endian, including the signature
            byte[] versionBytes = new UTF8Encoding().GetBytes(theRecord.Version);
            byte[] freqBytes = BitConverter.GetBytes(theRecord.Frequency);
            byte[] timeBytes = BitConverter.GetBytes(theRecord.TimeStamp);
            byte[] statusBytes = BitConverter.GetBytes(theRecord.StatusCode);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(freqBytes);
                Array.Reverse(timeBytes);
                Array.Reverse(statusBytes);
            }
            StringBuilder stuff = new StringBuilder();
            stuff.Append(Utilities.BytesToHexString(versionBytes));
            stuff.Append(Utilities.BytesToHexString(freqBytes));
            stuff.Append(Utilities.BytesToHexString(timeBytes));
            stuff.Append(Utilities.BytesToHexString(theRecord.SeedValueBytes));
            stuff.Append(Utilities.BytesToHexString(theRecord.PreviousOutputValueBytes));
            stuff.Append(Utilities.BytesToHexString(statusBytes));
            byte[] dataToVerify = Utilities.HexStringToBytes(stuff.ToString());
            MyRSA.Reset(); // always clean out any previous attempts
            MyRSA.BlockUpdate(dataToVerify, 0, dataToVerify.Length); // input the data to digest and verify
            byte[] sig = theRecord.SignatureValueBytes;
            // as of this version, the signature from NIST is output little endian and needs to be reversed
            Array.Reverse(sig);
            return MyRSA.VerifySignature(sig); // verify it against the provided signature
        }

    }
}
