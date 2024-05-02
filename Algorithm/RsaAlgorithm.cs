using System.Numerics;
using System.Text;

namespace Learning_RSA_Algorithm.Algorithm;

public sealed class RsaAlgorithm : IRsaAlgorithm
{
    private readonly BigInteger _publicExponent;

    private readonly BigInteger _privateExponent;

    public string PublicKey => $"{_publicExponent}:{Modulos}";
    
    public string PrivateKey => $"{_privateExponent}:{Modulos}";

    public BigInteger P { get; private set; }

    public BigInteger Q { get; private set; }

    public BigInteger Phi => (P - 1) * (Q - 1);

    public BigInteger Modulos => P * Q;

    public RsaAlgorithm(BigInteger p, BigInteger q)
    {
        if (!p.IsPrime() || !q.IsPrime() || p == q)
            throw new ArgumentException("p and q must be prime numbers and different from each other.");

        P = p;
        Q = q;
        _publicExponent = Phi.GetCoprime();
        _privateExponent = _publicExponent.GetModularInverseOf(Phi);
    }

    public string Encrypt(string plainText) =>
        Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                string.Join(' ', plainText.Select(ApplyEncryptAlgorithm))));

    private BigInteger ApplyEncryptAlgorithm(char utf16Char) =>
        BigInteger.ModPow(utf16Char, _publicExponent, Modulos);

    public string Decrypt(string base64EncryptedText) =>
        string.Join(
            string.Empty,
            Encoding
                .UTF8
                .GetString(Convert.FromBase64String(base64EncryptedText))
                .Split(' ')
                .Select(ApplyDecryptAlgorithm));

    private char ApplyDecryptAlgorithm(string bigIntergerChunck) =>
        (char)BigInteger.ModPow(BigInteger.Parse(bigIntergerChunck), _privateExponent, Modulos);
}