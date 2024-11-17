using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
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

            // Assert
            Assert.Contains("Unclosed string literal: " + input, Errors.Error_List);
        }

        [Test]
        public void MultipleUnknownChars_ReturnErrors()
        {
            // Arrange
            string input = "$# @! ^";

            // Act
            scanner.StartScanning(input);

            // Assert
            foreach (char c in input)
            {
                if (c.Equals(' ')) 
                        continue;
                Assert.Contains($"Unrecognized token: {c}", Errors.Error_List);
            }
        }

        [Test]
        public void NumberWithMultidot_ReturnsErrors()
        {
            string[] inputs = { "2.1.1.1", "3..1", ".555" };

            foreach (string input in inputs)
            {
                // Act
                scanner.StartScanning(input);

                // Assert
                if (input.Count(c => c == '.') > 1)
                {
                    Assert.Contains($"More than one dot in number: {input}", Errors.Error_List);
                    continue;
                }
                else if (input[0] == '.')
                {
                    Assert.Contains($"Invalid Constant: {input}", Errors.Error_List);
                }
                else
                {
                    Assert.Contains($"Invalid number format: {input}", Errors.Error_List);
                }
            }
        }

        [Test]
        public void UnclosedComment_ReturnsError()
        {
            string input = "/* This is an unclosed comment";

            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.Contains("Unclosed comment: /* This is an unclosed comment", Errors.Error_List);
        }

        [TestCase("x := x+1")]
        [TestCase("x:=x-1")]
        [TestCase("x := x - 1")]
        [TestCase("x := 1 + x")]
        [TestCase("x := 1 - x")]
        [TestCase("x := 1-x")]
        public void ValidArithmeticExpressions_ReturnsCorrectTokens(string input)
        {
            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.AreEqual(5, scanner.Tokens.Count);
            Assert.AreEqual(Token_Class.Identifier, scanner.Tokens[0].token_type);
            Assert.AreEqual(Token_Class.AssignOp, scanner.Tokens[1].token_type);
        }

        [Test]
        public void RelationalOperators_ReturnsCorrectTokens()
        {
            string input = "<= >=";

            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.AreEqual(2, scanner.Tokens.Count);
            Assert.AreEqual(Token_Class.LessThanOrEqualOp, scanner.Tokens[0].token_type);
            Assert.AreEqual(Token_Class.GreaterThanOrEqualOp, scanner.Tokens[1].token_type);
        }

        [Test]
        public void InvalidNumberWithAlphabet_ReturnsError()
        {
            string input = "123abc";

            // Act
            scanner.StartScanning(input);

            // Assert
            Assert.Contains("Mix of number and identifier: 123abc", Errors.Error_List);
        }
    }
}
