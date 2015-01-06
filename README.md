Random
======

Things that don't fit under other headings/repositories, but that might be useful to others...

Such as:

-NIST Randomness Beacon 
 This is a small program I wrote in c# using inspiration from an article on HackADay.  
 NIST Maintains a list of publicly available and VERIFIABLE random strings.  These strings are produced
  every 60 seconds, are signed by a public key, and are CHAINED together, such that from the first block
  in the chain until the most recently produced output, values are dependent on the ones that came
  before, and are not predictable (by the public) until they are published.  They can be used for
  lottery drawings or simple game outcomes, where the "random" element can be replayed from an
  independently verifiable source, but whose values shouldn't be known ahead of time.
  Uses Legion of the Bouncy Castle for the crypto stuff for certificate handling and SHA256withRSA
  signature verification.
  http://hackaday.com/2014/12/19/nist-randomness-beacon/
  https://beacon.nist.gov/home
  http://www.bouncycastle.org/csharp/
  DO NOT USE FOR *CRYPTOGRAPHIC* KEYS OR SOURCES!
