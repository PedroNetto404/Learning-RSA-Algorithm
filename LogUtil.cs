using System.Text.Encodings.Web;
using System.Text.Json;

namespace Learning_RSA_Algorithm;

public static class LogUtil
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task<string> LogOperation
    (
        IRsaAlgorithm rsa,
        string plainText,
        string encryptedText
    )
    {
        var log = new
        {
            P = rsa.P.ToString(),
            Q = rsa.Q.ToString(),
            rsa.PublicKey,
            rsa.PrivateKey,
            PlainText = plainText,
            EncryptedText = encryptedText,
            DecryptedText = rsa.Decrypt(encryptedText),
            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
        };

        var currentPath = Directory.GetCurrentDirectory();
        var logPath = Path.Combine(currentPath, "logs");

        if (!Directory.Exists(logPath))
            Directory.CreateDirectory(logPath);

        var filePath = Path.Combine(logPath, $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}.json");

        await File.WriteAllTextAsync(
            filePath,
            JsonSerializer.Serialize(
                log,
                _jsonSerializerOptions
            )
        );

        return filePath;
    }
}
