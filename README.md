# Learning the RSA Algorithm

## Overview
This repo contains .NET Console Application that demonstrates simple implementation of the RSA algorithm for encryption and decryption. The RSA algorithm is a widely used asymmetric encryption algorithm that relies on the use of public and private keys to secure data. This project provides a basic understanding of how RSA works and how to implement it in C#.

## Key Concepts
- **Public Key Cryptography:** RSA is an asymmetric encryption algorithm that uses two keys: a public key for encryption and a private key for decryption. The public key can be shared with anyone, while the private key is kept secret.
- **Key Generation:** The RSA algorithm involves generating two large prime numbers and calculating the public and private keys based on these numbers.
- **Encryption and Decryption:** RSA encryption involves raising a message to the power of the public key and taking the modulus of the result. Decryption is done by raising the ciphertext to the power of the private key and taking the modulus of the result.

## Getting Started
1. Clone the repository to your local machine.
2. Open the solution in preferred text editor or IDE.
3. Build the solution to restore dependencies.
```bash
    dotnet restore && dotnet build
```
4. Run the application to see the RSA encryption and decryption in action.
```bash
    dotnet run
```

You can specify the starting prime numbers (choose a large prime number for better security) or use randomly generated prime numbers. The application will generate the public and private keys, encrypt and decrypt a message, and display the results.


## The Algorithm Step by Step

1. **Choose Two Prime Numbers (p and q):** Select two large prime numbers for better security. The size of these primes is crucial as it determines the strength of the key.
2. **Calculate the Modulus (n):** Compute `n = p * q`. This modulus will be part of both the public and private keys.
3. **Calculate Phi (ϕ):** Compute `phi = (p - 1) * (q - 1)`. Phi represents the number of coprimes less than `n`.
4. **Choose an Exponent (e):** Select `e`, a coprime of `phi` where `1 < e < phi` and the greatest common divisor (gcd) of `e` and `phi` is 1. This `e` will be part of the public key.
5. **Calculate the Private Exponent (d):** Determine `d` such that `(e * d) % phi = 1`. This `d` is the private key.
6. **Encryption:** Encrypt a message `m` using the public key `(n, e)` by computing `c = m^e % n`, where `c` is the ciphertext.
7. **Decryption:** Decrypt the ciphertext `c` using the private key `(n, d)` by computing `m = c^d % n`, where `m` is the original message.

**Note:** Ensure that the numbers `p`, `q`, and `e` are chosen carefully to maintain the security and functionality of the RSA system.

## More About Auxiliary Algorithms

### Miller-Rabin Primality Test: A probabilistic algorithm to determine if a number is prime.

1. **Input:** An odd integer `n > 2` and a parameter `k` (number of iterations).
2. **Output:** Either "composite" (definitely composite) or "probably prime" (with high probability).

This algorithm is based on Fermat's Little Theorem, which states that if `n` is a prime number and `a` is an integer such that `1 < a < n`, then `a^(n-1) ≡ 1 (mod n)`. The Miller-Rabin test uses this theorem to check if a number is composite or probably prime.

Read more about the Miller-Rabin Primality Test [here](https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test).

### Modular Inverse: An algorithm to find the modular multiplicative inverse of a number.

1. **Input:** Two integers `a` and `m` where `m` is a prime number.
2. **Output:** An integer `x` such that `(a * x) % m = 1`.

The modular inverse is crucial in RSA encryption as it helps calculate the private key `d` from the public key `e`. This algorithm finds the multiplicative inverse of `a` modulo `m` using the Extended Euclidean Algorithm.

Read more about the Modular Inverse [here](https://en.wikipedia.org/wiki/Modular_multiplicative_inverse).