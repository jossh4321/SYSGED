using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class DigitalSignatureService
    {

        public RSACryptoServiceProvider generatePPKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            return rsa;
        }

        public byte[] signData(byte[] data, RSACryptoServiceProvider PPKeyPair)
        {
            byte[] signedHashValue;
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(PPKeyPair);
            rsaFormatter.SetHashAlgorithm("SHA256");
            signedHashValue = rsaFormatter.CreateSignature(data);
            return signedHashValue;
        }

        public bool verifySign(RSAParameters rsaKeyInfo /*exponente y modulo*/
            ,byte[] data,byte[] dataHahSigned)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaKeyInfo);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            if (rsaDeformatter.VerifySignature(data, dataHahSigned))
            {
                Console.WriteLine("The signature is valid.");
                return true;
            }
            else
            {
                Console.WriteLine("The signature is not valid.");
                return false;
            }
        }
    }
}
