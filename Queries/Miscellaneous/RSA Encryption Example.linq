<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Numerics.dll</Reference>
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	string message = "WOW";
	byte[] messageBytes = Encoding.UTF8.GetBytes(message);
	
	BigInteger messageAsNumber = new BigInteger(messageBytes);
	
	message.Dump("Original message");
	messageAsNumber.Dump("Message as a number");

	long p = 9739;	// random large prime
	long q = 10007;	// random large prime
	
	long n = p * q;	// modulus for the public key and private key
	
	long totient = (p - 1) * (q - 1);	// Euler's totient
	
	long e = 17;	//	random prime where 1 < e < n

	long d = 0;		// finding d so that d * e = 1 + (some number k) * totient
	while (true)
	{
		if (e * d % totient == 1)
		{
			break;
		}
		else
		{
			d++;
		}
	}
	
	EncryptionKey publicKey = new EncryptionKey() { Key = e, Modulus = n };
	EncryptionKey privateKey = new EncryptionKey() { Key = d, Modulus = n };
	
	var ciphertext = Encrypt(messageAsNumber, publicKey.Key, publicKey.Modulus);
	
	ciphertext.Dump("Encrypted number");
	
	Encoding.UTF8.GetString(ciphertext.ToByteArray()).Dump("Ciphertext");
	BitConverter.ToString(ciphertext.ToByteArray()).Replace("-", string.Empty).Dump("Ciphertext as hexadecimal");
	
	var decryptedMessage = Decrypt(ciphertext, privateKey.Key, privateKey.Modulus);
	decryptedMessage.Dump("Decrypted number");
	
	// Convert BigInteger to string
	// Now we can see the original message
	Encoding.UTF8.GetString(decryptedMessage.ToByteArray()).Dump("Decrypted message");
	
	publicKey.Dump();
	privateKey.Dump();
}

class EncryptionKey
{
	public long Key { get; set; }
	public long Modulus { get; set;}
}

BigInteger Encrypt(BigInteger messageNumber, long publicKey, long modulusValue)
{
	// ciphertext c = (message ^ e) % n
	// This is a large number representing the message once encrypted
	return BigInteger.ModPow(messageNumber, publicKey, modulusValue);
}

BigInteger Decrypt(BigInteger ciphertext, long privateKey, long modulusValue)
{
	// message m = (c ^ d) % n
	// This is a large number representing the message once decrypted
	return BigInteger.ModPow(ciphertext, privateKey, modulusValue);
}

// Define other methods and classes here