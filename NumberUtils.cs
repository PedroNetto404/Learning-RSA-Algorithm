using System.Numerics;
using System.Security.Cryptography;

namespace Learning_RSA_Algorithm;

public static class NumberUtils
{
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    private const int certainty = 10;

    /// <summary>
    /// Miller-Rabin Primality Test Algorithm for Quickly Check if a Number is Prime
    /// </summary>
    /// <param name="number"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static bool IsPrime(this BigInteger number)
    {
        if (number <= 1) return false;
        if (number == 2 || number == 3) return true;
        if (number % 2 == 0 || number % 3 == 0) return false;

        BigInteger d = number - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        for (int i = 0; i < certainty; i++)
        {
            BigInteger a = TakeRandom(2, number - 2);
            BigInteger x = BigInteger.ModPow(a, d, number);
            if (x == 1 || x == number - 1) continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, number);
                if (x == 1) return false;
                if (x == number - 1) break;
            }

            if (x != number - 1) return false;
        }

        return true;
    }

    /// <summary>
    /// Generate a prime number with a specific bit length randomly
    /// </summary>
    /// <param name="bits">Quantity of bits for the prime number</param>
    /// <returns></returns>
    public static BigInteger GeneratePrime(int bits)
    {
        BigInteger candidate;

        do
        {
            candidate = TakeRandom(bits);
        } while (!(candidate.IsPrime() && candidate.GetBitLength() == bits));

        return candidate;
    }

    public static BigInteger GetCoprime(this BigInteger phi)
    {
        BigInteger e;

        do
        {
            e = TakeRandom(2, phi);
        } while (BigInteger.GreatestCommonDivisor(e, phi) != 1);

        return e;
    }

    /// <summary>
    /// Algorithm to get the modular inverse of a number
    /// </summary>
    /// <returns></returns>
    public static BigInteger GetModularInverseOf(this BigInteger a, BigInteger m)
    {
        BigInteger m0 = m;
        (BigInteger x, BigInteger y) = (0, 1);

        if (m == 1) return 0;

        while (a > 1)
        {
            BigInteger q = a / m;
            (a, m) = (m, a % m);
            (x, y) = (y - q * x, x);
        }

        return y < 0 ? y + m0 : y;
    }

    /// <summary>
    /// Take a random big integer in interval
    /// </summary>
    public static BigInteger TakeRandom(BigInteger min, BigInteger max)
    {
        var range = max - min;
        var buffer = new byte[range.ToByteArray().Length + 1];
        Rng.GetBytes(buffer);
        buffer[^1] = 0;
        return new BigInteger(buffer) % range + min;
    }

    /// <summary>
    /// Take a random bit integer with a specific bit length
    /// </summary>
    /// <param name="bits"></param>
    /// <returns></returns>
    public static BigInteger TakeRandom(int bits)
    {
        var buffer = new byte[(bits >> 3) + 1];
        Rng.GetBytes(buffer);
        buffer[^1] = 0;
        return new BigInteger(buffer);
    }
}
