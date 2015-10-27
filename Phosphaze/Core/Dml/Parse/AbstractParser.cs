using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml.Parse 
{

    /// <summary>
    /// A parser that handles a lot of abstract parser related operations common
    /// to all Dml parsers.
    /// </summary>
	public abstract class AbstractParser 
	{

		// Internal reference to the tokens.
		private LinkedList<string> _tokens;

		/// <summary>
		/// The tokens this parser is reading.
		/// </summary>
		protected LinkedList<string> Tokens { get { return _tokens; } }

        // Internal reference to the current node.
        private LinkedListNode<string> current;

		/// <summary>
		/// The value of the current token (or null).
		/// </summary>
		public string CurrentToken { get { return current == null ? null : current.Value; } }

        /// <summary>
        /// The value of the next token (or null).
        /// </summary>
        public string NextToken { get { return (current == null || current.Next == null) ? null : current.Next.Value; } }

		/// <summary>
		/// The tokens to expect.
		/// </summary>
		private Stack<string> expecting = new Stack<string>();

		/// <summary>
		/// The amount of tokens to skip.
		/// </summary>
		private int skipNext = 0;

		/// <summary>
		/// The current line the parser is on. This value automatically increments every time the
		/// parser encounters a newline token.
		/// </summary>
		protected int currentLine = 0;

		/// <summary>
		/// Are we done parsing?
		/// </summary>
		public bool Done 
		{
			get	{
			     // Only ever occurs as a result of advancing past the last token.
			     // current == null will NEVER imply we are before the first token.
			     return current == null; 
			}
		}

        /// <summary>
        /// Construct a new <see cref="AbstractParser"/>.
        /// </summary>
        /// <param name="tokens">The list of tokens to parse through.</param>
        /// <param name="currentLine"> 
        ///     The current line in the file we are parsing. This is
        ///     used to decorate exceptions and give us more meaningful
        ///     errors.
        /// </param>
		public AbstractParser(string[] tokens, int currentLine = 0)
		{
			_tokens = new LinkedList<string>(tokens);
			current = _tokens.First;
			this.currentLine = currentLine;
		}

		/// <summary>
		/// Reverse the parser by the given number of steps.
		/// </summary>
        /// <param name="steps">The number of tokens backwards to reverse.</param>
        /// <param name="strict">
        ///     Whether or not to throw an error if we could not 
        ///     reverse the desired number of steps. True by default.
        /// </param>
        /// <param name="exception">
        ///     The exception to throw if strict is true and we could
        ///     not reverse the desired number of steps. Null by default.
        /// </param>
		public void Reverse(int steps = 1, bool strict = true, Exception exception = null) 
		{
			// `current` will only ever be null if we advanced past the last token.
			if (current == null) 
			{
				current = _tokens.Last;
				steps--;
			}

            while (0 < steps-- && current.Previous != null)
            {
                current = current.Previous;
                // Newline tokens don't count towards the number of steps.
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.NEWLINE) && current.Previous != null)
                {
                    current = current.Previous;
                    currentLine--;
                }
            }
			
			// The value of `steps` will be zero unless we couldn't reverse anymore.
			if (steps != -1 && strict) 
			{
				if (exception == null)
					throw new ParserError("Cannot reverse the desired amount of steps.");
				throw exception;
			}
		}

        private bool HasNext(bool notNull = true)
        {
            return (current != null) || (notNull && current.Next != null);
        }

		/// <summary>
		/// Advance the parser by the given number of steps.
        /// 
        /// Note: This will also automatically ignore newline tokens (incrementing the newline counter)
        /// and will check for any expecting tokens.
		/// </summary>
        /// <param name="steps">The number of tokens forwards to advance.</param>
        /// <param name="notNull">
        ///     Whether or not to allow CurrentToken to advance 
        ///     past the last token and become null.
        /// </param>
        /// <param name="strict">
        ///     Whether or not to throw an error if we could not 
        ///     advance the desired number of steps. True by default.
        /// </param>
        /// <param name="exception">
        ///     The exception to throw if strict is true and we could
        ///     not advance the desired number of steps. Null by default.
        /// </param>
		public void Advance(int steps = 1, bool notNull = true, bool strict = true, Exception exception = null)
		{
			// If `notNull` is `true`, then we disallow the setting of `current` as `null`.
			while (0 < steps-- && HasNext(notNull))
            {
                current = current.Next;
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.NEWLINE) && HasNext(notNull))
                {
                    current = current.Next;
                    currentLine++;
                }
                if (expecting.Count > 0)
                {
                    string nextExpecting = expecting.Pop();
                    if (!DmlTokens.IsMatch(CurrentToken, nextExpecting))
                        throw DmlSyntaxError.UnexpectedToken(nextExpecting);
                }
            }

			// As with `Reverse`, the value of `steps` will be -1 unless we couldn't advance anymore.
			if (steps != -1 && strict)
			{
				if (exception == null)
					throw new ParserError("Cannot advance the desired amount of steps.");
				throw exception;
			}
		}

        /// <summary>
        /// Set the expected tokens. When Advance is called, it automatically checks to
        /// ensure that the current token matches the next expected tokens.
        /// </summary>
        /// <param name="tokens">The tokens to expect.</param>
        public void SetExpecting(params string[] tokens)
        {
            Array.Reverse(tokens);
            expecting = new Stack<string>(tokens);
        }

		/// <summary>
        /// Tell the parser to skip the next token.
        /// </summary>
        protected void SkipNext() { skipNext = 1; }

        /// <summary>
        /// Tell the parser to skip the next n tokens.
        /// </summary>
        /// <param name="n">The number of tokens to skip.</param>
        protected void SkipNext(int n) { skipNext = n; }

        /// <summary>
        /// Restart the parser by setting the current token to the first token.
        /// </summary>
        protected void Restart() { current = Tokens.First; }

		/// <summary>
        /// Return a list of all the tokens until a given token (not including the terminal token).
        /// This will either reposition the parser at the initial token's position, or position the
        /// parser at the terminal character depending on whether or not `reposition` is `true` or 
        /// `false`.
        ///
        /// If no terminal character is found then an error is thrown.
        ///
        /// No matter whether `ignoreBracketDepth` is `true` or `false`, this function will NOT
        /// check for mismatched brackets. That is the job of the DmlSyntaxChecker.
        /// </summary>
        /// <param name="token">The token to read until.</param>
        /// <param name="ignoreBracketDepth">
        ///     Whether or not to ignore bracket depth. If false (which it is by default),
        ///     then the parser will continue taking tokens until one the end token is encountered
        ///     outside brackets.
        /// </param>
        /// <param name="reposition">
        ///     Whether or not to reposition the parser after grabbing the tokens.
        /// </param>
        /// <param name="exception">
        ///     The exception to throw if the end of the tokens is reached before
        ///     the given end token is encountered.
        /// </param>
        protected string[] GetUntil(
            string token, bool ignoreBracketDepth = false, 
            bool reposition = false, Exception exception = null)
        {
            LinkedListNode<string> oldToken = current;

            int bracketDepth = 0;

            var tokens = new List<string>();
            while (true)
            {
                if (DmlTokens.IsMatch(CurrentToken, token) && (!ignoreBracketDepth && bracketDepth == 0))
                    break;
                if (!ignoreBracketDepth) 
                {
                	if (DmlTokens.IsLeftBracket(CurrentToken))
                		bracketDepth++;
                	else if (DmlTokens.IsRightBracket(CurrentToken))
                		bracketDepth--;
                }
                tokens.Add(CurrentToken);
                Advance(exception: exception);
            }

            if (reposition)
                current = oldToken;

            return tokens.ToArray();
        }

        /// <summary>
        /// Get the tokens within a namespace block.
        /// </summary>
        /// <param name="reposition">
        ///     Whether or not to reposition the parser after grabbing the tokens.
        /// </param>
        /// <param name="exception">
        ///     The exception to throw if the end of the tokens is reached before
        ///     the namespace is closed.
        /// </param>
        /// <returns>The tokens inside the namespace (not including the delimiters).</returns>
        protected string[] GetNamespaceBlock(bool reposition = false, Exception exception = null)
        {
            LinkedListNode<string> oldToken = current;

            if (CurrentToken != DmlTokens.LANGLE)
            {
                current = oldToken;
                throw new ParserError("Initial token of namespace must be DmlTokens.NS_START.");
            }

            int namespaceDepth = 1;
            var newTokens = new List<string>();
            Advance();

            while (true)
            {
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LANGLE))
                    namespaceDepth++;
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.RANGLE))
                {
                    namespaceDepth--;
                    if (namespaceDepth == 0)
                        break;
                }
                // FIXME: Aren't we double checking if current.Next == null? (Once here and once in Advance)
                if (current.Next == null)
                {
                    if (exception == null)
                        throw new DmlSyntaxError("Invalid syntax; mismatched namespace delimiters detected.");
                    throw exception;
                }
                newTokens.Add(CurrentToken);
                Advance(exception: exception);
            }

            if (reposition)
                current = oldToken;

            return newTokens.ToArray();
        }

		/// <summary>
		/// Parse the input tokens.
		/// </summary>
		public void Parse() 
		{
			PreParse();
			while (!Done)
                ParseNext();
			PostParse();
		}

        /// <summary>
        /// Parse the next token.
        /// </summary>
        protected void ParseNext()
        {
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.NEWLINE))
                Advance(notNull: false);
            else if (skipNext > 0)
            {
                skipNext--;
                Advance(notNull: false);
            }
            // Just kill the parsing if either of the above calls to Advance
            // caused the current token to push past the end.
            if (CurrentToken == null)
                return;

            if (expecting.Count > 0)
            {
                string nextExpecting = expecting.Pop();
				if (!DmlTokens.IsMatch(CurrentToken, nextExpecting))
				    throw DmlSyntaxError.UnexpectedToken(nextExpecting);
            }

            ProcessNext();

            Advance(notNull: false, strict: false);
            // By setting `notNull` to `false`, we allow `current` to be set to `null`,
            // which is what causes `Done` to return `true` causing the while loop to
            // end.
        }

		/// <summary>
		/// Perform any preliminary actions before parsing.
		/// </summary>
		protected virtual void PreParse() { }

		/// <summary>
		/// Process the next token.
		/// </summary>
        protected abstract void ProcessNext();

		/// <summary>
		/// Perform any closing actions after parsing.
		/// </summary>
        protected virtual void PostParse() { }

		/// <summary>
		/// Return the result of parsing the input tokens. This varies depending on the
		/// responsibility of the parser.
		/// </summary>
		public abstract object GetResult();

	}

}