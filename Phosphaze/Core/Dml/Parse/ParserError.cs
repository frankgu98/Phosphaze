using System;

namespace Phosphaze.Core.Dml.Parse
{

	public class ParserError : Exception
	{

		public ParserError(string msg) : base(msg) { }

		public ParserError(string msg, Exception inner) : base(msg, inner) { }

	}

}