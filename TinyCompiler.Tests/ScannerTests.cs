using NUnit.Framework;
using Tiny_Compiler; // Replace with your namespace


namespace TinyCompiler.Tests
{
    public class ScannerTests
    {
        private Scanner scanner;

        [SetUp]
        public void Setup()
        {
            scanner = new Scanner();
        }

        [Test]
        public void TokenizeReservedWord_Main_ReturnsCorrectToken()
        {
            // Arrange
            string input = "main";

            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.AreEqual(1, scanner.Tokens.Count);
            Assert.AreEqual(Token_Class.Main, scanner.Tokens[0].token_type);
        }

        [Test]
        public void TokenizeInvalidToken_ReturnsError()
        {
            // Arrange
            string input = "@";

            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.Contains("Unrecognized token: @", Errors.Error_List);
        }
        [Test]
        public void UnclosedString_ReturnsError()
        {
            string input = "\"I am an unclosed string.";

            // Act
            scanner.StartScanning(input);

            Assert.Contains("Unclosed string literal: " + input, Errors.Error_List);
        }

    }
}