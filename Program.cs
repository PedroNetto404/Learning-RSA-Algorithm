using Learning_RSA_Algorithm;
using Learning_RSA_Algorithm.Algorithm;

RsaUi.ClearConsole();
RsaUi.DisplayWelcomeMessage();

await Task.Delay(3000);
RsaUi.ClearConsole();

while (true)
{
    try
    {
        var primeNumbersTask = RsaUi.GetPrimeNumbers();

        RsaUi.ClearConsole();
        var loadingTask = RsaUi.ShowLoadingAsync("Generating RSA Keys...", 4000);

        var (firstPrime, secondPrime) = await primeNumbersTask;
        var rsa = new RsaAlgorithm(firstPrime, secondPrime);

        await loadingTask;

        while (true)
        {
            RsaUi.DisplayKeys(rsa);

            var encryptOption = RsaUi.GetEncryptOption();
            var textToProcess = RsaUi.GetText(encryptOption);

            var processTextLoadingTask = RsaUi.ShowLoadingAsync($"{(encryptOption == "Encrypt" ? "encrypting" : "decrypting")}...", 3000);

            var processedText = encryptOption == "Encrypt" ? rsa.Encrypt(textToProcess) : rsa.Decrypt(textToProcess);

            await processTextLoadingTask;
            RsaUi.ClearConsole();

            RsaUi.DisplayProcessedText(encryptOption, processedText);

            try
            {

                var logPath = await (
                    encryptOption == "Encrypt" ?
                        LogUtil.LogOperation(rsa, textToProcess, processedText) :
                        LogUtil.LogOperation(rsa, processedText, textToProcess)
                );
                
                RsaUi.DisplayLogFilePath(logPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while trying to log the operation.");
                Console.WriteLine(e.Message);
            }


            if (RsaUi.ContinueWithSameKeys())
            {
                RsaUi.ClearConsole();
                continue;
            }

            break;
        }

        if (RsaUi.ExitConfirmation())
        {
            RsaUi.DisplayGoodbyeMessage();
            return;
        }

        RsaUi.LetsStartAgain();
        await Task.Delay(3000);

        RsaUi.ClearConsole();
    }
    catch (Exception ex)
    {
        var canContinue = RsaUi.PrintExceptionAndAskForContinuation(ex);
        if (!canContinue) return;
    }
}