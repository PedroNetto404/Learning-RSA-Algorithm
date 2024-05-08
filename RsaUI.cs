using System.Numerics;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Learning_RSA_Algorithm;

public static class RsaUi
{
    public static void DisplayKeys(IRsaAlgorithm rsaAlgorithm)
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(2)) 
            .AddColumn(new GridColumn().PadLeft(2))
            .AddRow("[u]Key[/]", "[u]Value[/]");

        grid.AddRow("Public Key", $"[{Color.Yellow.ToMarkup()}]{rsaAlgorithm.PublicKey}[/]");
        grid.AddRow("Private Key", $"[{Color.Yellow.ToMarkup()}]{rsaAlgorithm.PrivateKey}[/]");

        AnsiConsole.Write(
            new Panel(grid)
                .Header("[cyan]RSA Keys[/]", Justify.Center)
                .Border(BoxBorder.Rounded));
    }

    public static void DisplayProcessedText(string options, string processedText)
    {
        var label = options == "Encrypt" ? "Encrypted" : "Decrypted";

        Console.Write($"{label} text: ");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(processedText);
        Console.ResetColor();
    }

    public static bool ContinueWithSameKeys() => AnsiConsole.Confirm(" Do you want to continue using the same keys?");

    public static void DisplayWelcomeMessage() => AnsiConsole.Write(new Panel(new Rows([
            new Markup("[cyan]RSA Encryption and Decryption[/]"),
            new Markup(
                $"[grey](note: Choose large prime numbers for better security!!!)[/]")
        ]))
        .Header("[yellow]Welcome to[/]"));

    public static string GetEncryptOption() => AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select an option:")
            .PageSize(10)
            .AddChoices(["Encrypt", "Decrypt"]));

    public static string GetText(string option) => AnsiConsole.Ask<string>($"Enter the text to {option.ToLower()}:");

    public static bool PrintExceptionAndAskForContinuation(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
        AnsiConsole.WriteLine();

        var continueOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Ops... Something went wrong. Do you want to try again?[/]")
                .PageSize(10)
                .AddChoices(["Yes", "No"]));

        if (continueOption == "No")
        {
            DisplayGoodbyeMessage();
            return false;
        }

        ClearConsole();
        return true;
    }

    public static void DisplayGoodbyeMessage() => AnsiConsole.MarkupLine("[green]\nGoodbye![/]");

    public static void ClearConsole() => AnsiConsole.Clear();

    public static async Task<(BigInteger, BigInteger)> GetPrimeNumbers()
    {
        var modeChoise = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Select a mode of choosing starting prime numbers (p and q):[/]")
                .PageSize(10)
                .AddChoices(["Manual", "Automatic"]));

        return modeChoise == "Manual"
            ? GetManualPrimeNumbers()
            : await GetAutomaticPrimeNumbersAsync();
    }

    private static async Task<(BigInteger, BigInteger)> GetAutomaticPrimeNumbersAsync()
    {
        var primeNumbers = await Task.WhenAll(
            Task.Run(() => NumberUtils.GeneratePrime(IRsaAlgorithm.MinBitLenght)),
            Task.Run(() => NumberUtils.GeneratePrime(IRsaAlgorithm.MinBitLenght))
        );

        return (primeNumbers[0], primeNumbers[1]);
    }

    private static (BigInteger, BigInteger) GetManualPrimeNumbers()
    {
        BigInteger firstPrime;
        BigInteger secondPrime;

        while (true)
        {
            firstPrime = AskAPrime("Enter the first prime number:", NumberUtils.GeneratePrime(IRsaAlgorithm.MinBitLenght));

            if (!ValidatePrimeCandidate(firstPrime)) continue;

            secondPrime = AskAPrime("Enter the second prime number:", NumberUtils.GeneratePrime(IRsaAlgorithm.MinBitLenght));
            if (!ValidatePrimeCandidate(secondPrime)) continue;

            if (firstPrime == secondPrime)
            {
                AnsiConsole.MarkupLine("[red]Please select different primes...[/]");
                continue;
            }

            break;
        }

        return (firstPrime, secondPrime);

        static bool ValidatePrimeCandidate(BigInteger prime)
        {
            if (prime.GetBitLength() < IRsaAlgorithm.MinBitLenght)
            {
                AnsiConsole.MarkupLine("[red]Please select prime with 1024 bits or more...[/]");
                return false;
            }

            return true;
        }
    }

    private static BigInteger AskAPrime(string message, BigInteger defaultValue)
    {
        BigInteger prime;

        bool isPrime;
        do
        {
            prime = BigInteger.Parse(AnsiConsole.Ask(message, defaultValue.ToString()));
            isPrime = NumberUtils.IsPrime(prime);
            if (!isPrime) AnsiConsole.MarkupLine("[red]The number you entered is not a prime number. Please try again.[/]");

        } while (!isPrime);

        return prime;
    }

    public static void DisplayLogFilePath(string logPath) => AnsiConsole.MarkupLine($"[green] You can see the log file at: {logPath}[/]");

    public static async Task ShowLoadingAsync(string message, int miliseconds) =>
        await AnsiConsole.Status()
            .StartAsync(message, async ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);

                await Task.Delay(miliseconds);

                ctx.Status("Done!");
                ctx.Spinner(Spinner.Known.Star);

                await Task.Delay(1000);

                ClearConsole();
            });

    public static bool ExitConfirmation() => AnsiConsole.Confirm("Do you want to exit the application?");

    public static void LetsStartAgain() => AnsiConsole.MarkupLine("[cyan]Let's start again![/]");
}