using System;
using System.Text;
using System.IO;

/// <summary>
/// C# implementation of ASCII85 encoding. 
/// Based on C code from http://www.stillhq.com/cgi-bin/cvsweb/ascii85/
/// and
/// Jeff Atwood from http://www.codinghorror.com/blog/archives/000410.html
/// </summary>
/// <remarks>
/// Dustin J. Sparks
/// 2011
/// This extends the ASCII85 standard to recognize the "z" and "y" shorthand, and to be a static
/// class with static functions.
/// </remarks>
public static class Ascii85yz
{
	/// <summary>
	/// Prefix mark that identifies an encoded ASCII85 string, traditionally '<~'
	/// </summary>
	public const string PrefixMark = "<~";
	/// <summary>
	/// Suffix mark that identifies an encoded ASCII85 string, traditionally '~>'
	/// </summary>
	public const string SuffixMark = "~>";
	/// <summary>
	/// Maximum line length for encoded ASCII85 string; 
	/// set to zero for one unbroken line.
	/// </summary>
	public const int LineLength = 72;
	/// <summary>
	/// Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding
	/// </summary>
	public const bool EnforceMarks = true;
    /// <summary>
    /// This switches on the ability to use the Y character exception in encoding to represent 4 spaces.
    /// Adobe does not recognize or use this character, and will error out of it exists, so set this to
    /// FALSE whenever the data has to interact with Adobe products.
    /// </summary>
    public const bool AllowYBlock = true;
    /// <summary>
    /// If true, then remove invalid characters and do not throw exceptions.
    /// If false, throw an exception on first invalid character.
    /// </summary>
    private const bool ErrorOnInvalidChars = true;

	private const int ASCIIOffset = 33;

    //private UInt32[] pow85 = {1, 85, 85 * 85, 85 * 85 * 85, 85 * 85 * 85 * 85};//{ 85*85*85*85, 85*85*85, 85*85, 85, 1 };


	/// <summary>
	/// Decodes an ASCII85 encoded string into the original binary data
	/// </summary>
	/// <param name="s">ASCII85 encoded string</param>
	/// <returns>byte array of decoded binary data</returns>
	public static byte[] Decode(string inString)
	{
        inString = inString.Replace("\r", "").Replace("\n", "");
		if (EnforceMarks)
		{
			if (!inString.StartsWith(PrefixMark) | !inString.EndsWith(SuffixMark)) 
			{
				throw new ArgumentException("ASCII85 encoded data should begin with '" + PrefixMark + 
					"' and end with '" + SuffixMark + "'", "inString");
			}  
		}

		// strip prefix and suffix if present
		if (inString.StartsWith(PrefixMark))
		{
			inString = inString.Substring(PrefixMark.Length);
		}
		if (inString.EndsWith(SuffixMark))
		{
			inString = inString.Substring(0, inString.Length - SuffixMark.Length);
		}

        StringBuilder sterile = new StringBuilder(inString.Length);
        foreach (char C in inString)
        {
            switch (C)
            {
                case 'z':  //special character for a byte array of 4 nulls
                    sterile.Append(C);
                    break;
                case 'y':  //special character for a byte array of 4 spaces ({0x20,0x20,0x20,0x20})
                    if (AllowYBlock)
                        sterile.Append(C);
                    else if (ErrorOnInvalidChars)
                    {    
                        sterile = null;
                        throw new ArgumentException("Invalid character '" + C + "' found in source string!", "inString");
                    }
                    break;
                //case '\n': //do nothing, formatting character  CR/LF removed further up, uncomment if that is removed up there
                //case '\r': //do nothing, formatting character
                case '\t': //do nothing, formatting character
                case '\0': //do nothing, null character
                case '\f': //do nothing, formatting character
                case '\b': //do nothing, formatting character  
                    break;
                default:
                    if (C >= '!' && C <= 'u')   // normal encoded characters in the stream
                        sterile.Append(C);
                    else if (ErrorOnInvalidChars)  // invalid character, do we throw an exception?
                    {   
                        sterile = null;
                        throw new ArgumentException("Invalid character '" + C + "' found in source string!", "inString");
                    }
                    break;
            }
        }
        string sterilized = sterile.ToString();
        sterile = null;

		MemoryStream ms = new MemoryStream();
        int padding = 0;
        for (int crawl = 0; crawl < sterilized.Length; ) // advancement of the crawl variable is handeled in the block!!
        {
            if (sterilized[crawl] == 'z')
            {    
                ms.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
                crawl++;
                continue;
            }
            else if (sterilized[crawl] == 'y')
            {
                ms.Write(new byte[] { 0x20, 0x20, 0x20, 0x20 }, 0, 4);
                crawl++;
                continue;
            }
            else
            {   
                string block;
                if (crawl + 5 > sterilized.Length)
                {
                    block = sterilized.Substring(crawl);
                    switch (block.Length)
                    {
                        case 1:
                            throw new ArgumentException("Invalid length!  The string is improperly truncated.", "inString");
                        case 2: block += "uuu";
                            padding = 3;
                            break;
                        case 3: block += "uu";
                            padding = 2;
                            break;
                        case 4: block += "u";
                            padding = 1;
                            break;
                    }
                }
                else
                    block = sterilized.Substring(crawl, 5);
                
                if (block.Contains("z") || block.Contains("y"))
                    throw new ArgumentException("Invalid 'z' or 'y' character found in a block of the source string! " +
                        "They must not be in the middle of a block!", "inString");
                else
                {
                    byte[] ASCIIValues = Encoding.ASCII.GetBytes(block);
                    UInt32 result = 0;
                    UInt32[] pow85 = { 85 * 85 * 85 * 85, 85 * 85 * 85, 85 * 85, 85, 1 };
                    for (int i = 0; i < 5; i++)
                        result += (UInt32)((ASCIIValues[i] - ASCIIOffset) * pow85[i]);
                    byte[] interim = BitConverter.GetBytes(result);
                    Array.Reverse(interim);
                    ms.Write(interim, 0, 4);
                    crawl += 5;
                }
            }
        }

        if (padding == 0)
            return ms.ToArray();
        else
        {
            byte[] ret = new byte[ms.Length - padding];
            Buffer.BlockCopy(ms.ToArray(), 0, ret, 0, Buffer.ByteLength(ret));
            return ret;
        }
	}

	/// <summary>
	/// Encodes binary data into a plaintext ASCII85 format string
	/// </summary>
	/// <param name="inBytes">binary data to encode</param>
	/// <returns>ASCII85 encoded string</returns>
    public static string Encode(byte[] inBytes)
    {
        int padding = (4 - (inBytes.Length % 4)) % 4;

        StringBuilder result = new StringBuilder();
        if (EnforceMarks)
            result.Append(PrefixMark);
        byte[] inGoodBytes = new byte[inBytes.Length + padding];
        Buffer.BlockCopy(inBytes, 0, inGoodBytes, 0, inBytes.Length);
        for (int crawl = 0; crawl < inGoodBytes.Length; crawl += 4)
        {
            long interim = (UInt32)(inGoodBytes[3 + crawl]); // encode in MSB (big-endien) fashion regardless of system orientation.
            interim += (UInt32)(inGoodBytes[2 + crawl] << 8);
            interim += (UInt32)(inGoodBytes[1 + crawl] << 16);
            interim += (UInt32)(inGoodBytes[0 + crawl] << 24);
            if (interim == 0)
            {
                result.Append('z');
                continue;
            }
            else if (interim == 0x20202020) // 0x20 0x20 0x20 0x20, or 0x20202020
            {
                result.Append('y');
                continue;
            }
            long throwAway = 0;
            byte[] encodedBytes = 
                {(byte)(Math.DivRem(interim, 85*85*85*85, out throwAway) % 85 + ASCIIOffset), 
                (byte)(Math.DivRem(interim, 85*85*85, out throwAway) % 85 + ASCIIOffset),
                (byte)(Math.DivRem(interim, 85*85, out throwAway) % 85 + ASCIIOffset),
                (byte)(Math.DivRem(interim, 85, out throwAway) % 85 + ASCIIOffset),
                (byte)(interim % 85 + ASCIIOffset)};

            if (crawl == inGoodBytes.Length - 4)
                result.Append(ASCIIEncoding.ASCII.GetChars(encodedBytes, 0, 5 - padding));
            else
                result.Append(ASCIIEncoding.ASCII.GetChars(encodedBytes));
        }
        if (EnforceMarks)
            result.Append(SuffixMark);

        for (int i = LineLength; i < result.Length; i += LineLength + 2)
            result.Insert(i, "\r\n");

        return result.ToString();
    }
}
