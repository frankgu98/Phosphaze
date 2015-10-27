using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze.Core.Dml.Parse
{
    public class Tokenizer
    {

        /// <summary>
        /// Decompose the input string into it's individual tokens, which can
        /// then be more easily parsed by higher level Dml processors.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Tokenize(string input)
        {
            input = CompressWhitespace(input);
            var tokens = new LinkedList<StringBuilder>();
            tokens.AddLast(new StringBuilder());
            LinkedListNode<StringBuilder> currentToken = tokens.Last;
            string currentChar;

            bool inString = false;

            for (int i = 0; i < input.Length; i++)
            {
                currentChar = input[i].ToString();
                // Always check if we are in a string first. Being inside a string overrules
                // every other tokenizer rule.
                if (inString)
                {
                    currentToken.Value.Append(currentChar);
                    // If we've reached the end of the string, set inString to false and start
                    // a new token.
                    if (currentChar == DmlTokens.QUOTE)
                    {
                        inString = false;
                        tokens.AddLast(new StringBuilder());
                        currentToken = tokens.Last;
                    }
                }
                // Parse single char and double char tokens.
                // This looks deceiving because the if statement just checks if the current char
                // is a single char token, but inside we check if the previously parsed token allows
                // the current token to be turned into a double char token. 
                else if (DmlTokens.SINGLE_CHAR_TOKENS.Contains(currentChar))
                {
                    // Current char and previous char make either `==`, `>=`, `<=`, or `!=`.
                    if (currentChar == DmlTokens.EQUAL && currentToken.Previous.Value.Length == 1)
                    {
                        string prev = currentToken.Previous.Value.ToString();
                        if (prev == DmlTokens.LANGLE ||
                            prev == DmlTokens.RANGLE ||
                            prev == DmlTokens.EQUAL ||
                            prev == DmlTokens.BANG)
                            currentToken.Previous.Value.Append(currentChar);
                    }
                    // Current char and previous char make `->` or `\>`.
                    else if (
                        currentChar == DmlTokens.RANGLE &&
                        currentToken.Previous.Value.Length == 1 &&
                        (currentToken.Previous.Value.ToString() == DmlTokens.MINUS ||
                        currentToken.Previous.Value.ToString() == DmlTokens.BSLASH))
                        currentToken.Previous.Value.Append(currentChar);
                    // Current char and previous char make either `||` or `&&`.
                    else if (
                        (currentChar == DmlTokens.AMPERSAND ||
                         currentChar == DmlTokens.VBAR) &&
                        (currentToken.Previous.Value.Length == 1 &&
                         currentToken.Previous.Value.ToString() == currentChar))
                        currentToken.Previous.Value.Append(currentChar);
                    // Current char and previous two chars make `...`.
                    else if (
                        currentChar == DmlTokens.DOT &&
                        currentToken.Previous.Value.ToString() == DmlTokens.DOT &&
                        currentToken.Previous.Previous.Value.ToString() == DmlTokens.DOT)
                    {
                        tokens.RemoveLast();
                        tokens.RemoveLast();
                        currentToken = tokens.Last;
                        currentToken.Value.Append(currentChar + currentChar);

                        tokens.AddLast(new StringBuilder());
                        currentToken = tokens.Last;
                    }
                    // Current char doesn't pair with any previous chars.
                    else
                    {
                        // Make a new token if we are continuing from a previous token/word.
                        if (currentToken.Value.Length != 0)
                        {
                            tokens.AddLast(new StringBuilder());
                            currentToken = tokens.Last;
                        }
                        currentToken.Value.Append(currentChar);
                        // Check if we are inside a string. We don't want to make a new token
                        // because we want the characters associated with the string to be
                        // attached to the string.
                        if (currentChar == DmlTokens.QUOTE)
                            inString = true;
                        else
                        {
                            tokens.AddLast(new StringBuilder());
                            currentToken = tokens.Last;
                        }
                    }
                }
                // Add a newline token. This allows higher level parsers to determine what
                // line in the original code an error occurs on.
                else if (currentChar == DmlTokens.NEWLINE)
                {
                    tokens.AddLast(new StringBuilder());
                    currentToken = tokens.Last;
                    currentToken.Value.Append(currentChar);

                    tokens.AddLast(new StringBuilder());
                    currentToken = tokens.Last;
                }
                // Ignore whitespace (so long as we are not in a string).
                else if (currentChar == " ")
                {
                    tokens.AddLast(new StringBuilder());
                    currentToken = tokens.Last;
                }
                // Just an ordinary word-like token (name, number, etc.).
                else
                    currentToken.Value.Append(currentChar);
            }

            // Filter empty tokens.
            var tokenList = from token in tokens
                            where token.Length != 0
                            select token.ToString();

            string[] output = tokenList.ToArray();

            // Run the second pass.
            output = SecondPass(output);

            return output;
        }

        private static string CompressWhitespace(string input)
        {
            StringBuilder builder = new StringBuilder();
            char currentChar;
            bool seenWhitespace = false, inString = false;
            for (int i = 0; i < input.Length; i++)
            {
                currentChar = input[i];
                if (inString)
                {
                    if (currentChar == '"')
                        inString = false;
                    builder.Append(currentChar);
                }
                else if (currentChar == '\r')
                    continue;
                else if (currentChar == ' ' || currentChar == '\t')
                {
                    if (!seenWhitespace)
                    {
                        builder.Append(' ');
                        seenWhitespace = true;
                    }
                }
                else
                {
                    if (currentChar == '"')
                        inString = true;
                    seenWhitespace = false;
                    builder.Append(currentChar);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Run a second pass through the tokens to clean them up.
        /// </summary>
        private static string[] SecondPass(string[] tokens)
        {
            List<string> output = new List<string>();
            int length = tokens.Length;
            string currentToken;
            bool seenOperator = false;

            for (int i = 0; i < length; i++)
            {
                currentToken = tokens[i];
                // Check for comments and ignore them.
                if (DmlTokens.IsMatch(currentToken, DmlTokens.HASH)) 
                {
                    while (i < length && !DmlTokens.IsMatch(currentToken, DmlTokens.NEWLINE)) 
                        currentToken = tokens[++i];
                    output.Add(currentToken);
                }
                // Check for unary operators.
                else if (DmlTokens.IsOperator(currentToken) && !DmlTokens.IsMatch(currentToken, DmlTokens.RPAR)) 
                {
                    if (!seenOperator) 
                    {
                        seenOperator = true;
                        output.Add(currentToken);
                    }
                    else 
                    {
                        switch (currentToken) {
                            case DmlTokens.MINUS:
                                output.Add(DmlTokens.UNARY_NEG);
                                break;
                            case DmlTokens.TILDE:
                                output.Add(DmlTokens.UNARY_ABS);
                                break;
                            default:
                                output.Add(currentToken);
                                break;
                        }
                    }
                }
                // Check for decimals. This folds "123", ".", "567", into "123.567"
                else if (i < length - 2 && // At least 2 more tokens to parse.
                    DmlTokens.IsNumber(currentToken) && 
                    DmlTokens.IsMatch(tokens[i + 1], DmlTokens.DOT) &&
                    DmlTokens.IsNumber(tokens[i + 2]))
                {
                    output.Add(currentToken + DmlTokens.DOT + tokens[i + 2]);
                    i += 2;
                }
                else {
                    seenOperator = false;
                    output.Add(currentToken);
                }
            }

            return output.ToArray();
        }
    }
}
