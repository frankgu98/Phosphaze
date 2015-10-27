using System.Linq;
using System.Text.RegularExpressions;

namespace Phosphaze.Core.Dml.Parse
{
    public class DmlTokens
    {

        /// <summary>
        /// Plus; binary operator (addition).
        /// </summary>
        public const string PLUS = "+";

        /// <summary>
        /// Minus; binary operator (subtraction).
        /// </summary>
        public const string MINUS = "-";

        /// <summary>
        /// Star; binary operator (multiplication).
        /// </summary>
        public const string STAR = "*";
        
        /// <summary>
        /// Slash; binary operator (division).
        /// </summary>
        public const string SLASH = "/";

        /// <summary>
        /// Back slash; used in transition operator \>.
        /// </summary>
        public const string BSLASH = @"\";

        /// <summary>
        /// Percent; binary operator (modulo) and used to refer to named function parameters.
        /// </summary>
        public const string PERCENT = "%";

        /// <summary>
        /// Circumflex; binary operator (power).
        /// </summary>
        public const string CIRCUMFLEX = "^";

        /// <summary>
        /// Tilde; unary operator (absolute value).
        /// </summary>
        public const string TILDE = "~";

        /// <summary>
        /// At-sign; global namespace variable indicator.
        /// </summary>
        public const string AT = "@";

        /// <summary>
        /// Dollar-sign; instance namespace variable indicator. Equivalent to `self.` in Python.
        /// </summary>
        public const string DOLLAR = "$";

        /// <summary>
        /// Ampersand; binary operator? (Unused)
        /// </summary>
        public const string AMPERSAND = "&";

        /// <summary>
        /// Vertical-bar; binary operator? (Unused)
        /// </summary>
        public const string VBAR = "|";

        /// <summary>
        /// Exclamation-mark; unary operator (boolean-not).
        /// </summary>
        public const string BANG = "!";

        /// <summary>
        /// Hash; indicates a comment.
        /// </summary>
        public const string HASH = "#";

        public const string NEWLINE = "\n";

        /// <summary>
        /// Quote; string delimiter.
        /// </summary>
        public const string QUOTE = "\"";

        /// <summary>
        /// Left and right curly braces; function call and subexpression delimiters.
        /// </summary>
        public const string LPAR = "(";
        public const string RPAR = ")";

        /// <summary>
        /// Left and right curly braces; array delimiters.
        /// </summary>
        public const string LBRACE = "{";
        public const string RBRACE = "}";

        /// <summary>
        /// Left and right square brackets; external initialization delimiters.
        /// </summary>
        public const string LSQUARE = "[";
        public const string RSQUARE = "]";

        /// <summary>
        /// Left and right angle brackets; namespace and vector delimiters, equivalence 
        /// relation (less-than & greater-than respectively).
        /// </summary>
        public const string LANGLE = "<";
        public const string RANGLE = ">";

        /// <summary>
        /// Comma; separates function call parameters, array elements, && vector elements.
        /// </summary>
        public const string COMMA = ",";

        /// <summary>
        /// Dot; external instance access.
        /// </summary>
        public const string DOT = ".";

        /// <summary>
        /// Semicolon; end of line indicator.
        /// </summary>
        public const string EOL = ";";

        /// <summary>
        /// Equals; equivalence relation? (Unused)
        /// </summary>
        public const string EQUAL = "=";

        /// <summary>
        /// Double-equals; equivalence relation (equality).
        /// </summary>
        public const string DBLEQUAL = "==";

        /// <summary>
        /// Not-equals; equivalence relation (inequality).
        /// </summary>
        public const string NOTEQUAL = "!=";

        /// <summary>
        /// Equivalence relation (less than or equal to).
        /// </summary>
        public const string LTEQ = "<=";

        /// <summary>
        /// Equivalence relation (greater than or equal to).
        /// </summary>
        public const string GTEQ = ">=";

        /// <summary>
        /// Binary operator (boolean-and).
        /// </summary>
        public const string DBLAND = "&&";
        
        /// <summary>
        /// Binary operator (boolean-or).
        /// </summary>
        public const string DBLVBAR = "||";

        /// <summary>
        /// Range operator. Second component of the Transition shorthand.
        /// </summary>
        public const string TRANSOP = @"\>";

        /// <summary>
        /// Range indicator.
        /// </summary>
        public const string ELLIPSES = "...";

        /// <summary>
        /// (Internal) the unary negative operator token.
        /// </summary>
        public const string UNARY_NEG = "u-";

        /// <summary>
        /// (Internal) the unary abs operator token.
        /// </summary>
        public const string UNARY_ABS = "u~";

        /// <summary>
        /// The matcher for a numeric token.
        /// </summary>
        public const string NUMBER = @"[0-9]+(\.[0-9]+)?$";

        /// <summary>
        /// The matcher for an unqualified name token. This is any variable
        /// name not prefixed by a namespace indicator such as $ or @.
        /// </summary>
        public const string UQNAME = @"[a-zA-Z_][a-zA-Z0-9_]*$";

        /// <summary>
        /// The matcher for a Dml keyword.
        /// </summary>
        public const string KEYWORD = @"KEYWORD";

        /// <summary>
        /// The matcher for a builtin name.
        /// </summary>
        public const string BUILTIN = @"BUILTIN";

        #region Keywords

        /// <summary>
        /// Keyword True; corresponding boolean value.
        /// </summary>
        public const string KW_TRUE = "True";

        /// <summary>
        /// Keyword False; corresponding boolean value.
        /// </summary>
        public const string KW_FALSE = "False";

        /// <summary>
        /// Keyword Null; null-object pattern.
        /// </summary>
        public const string KW_NULL = "Null";

        /// <summary>
        /// Keyword Assign; used for assignment statements.
        /// </summary>
        public const string KW_ASSIGN = "Assign";

        /// <summary>
        /// Keyword Range; used to indicate a for loop over a range of values.
        /// </summary>
        public const string KW_RANGE = "Range";

        /// <summary>
        /// Keyword Bullet; used to indicate a new bullet namespace.
        /// </summary>
        public const string KW_BULLET = "Bullet";

        /// <summary>
        /// Keyword Pattern; used to indicate a new pattern namespace.
        /// </summary>
        public const string KW_PATTERN = "Pattern";

        /// <summary>
        /// Keyword Timeline; used to indicate a new timeline namespace.
        /// </summary>
        public const string KW_TIMELINE = "Timeline";

        /// <summary>
        /// Keyword Init; used to indicate the initialization block of a bullet or pattern.
        /// </summary>
        public const string KW_INIT = "Init";

        /// <summary>
        /// Keyword Update; used to indicate the update block of a bullet or pattern.
        /// </summary>
        public const string KW_UPDATE = "Update";

        /*

        /// <summary>
        /// Keyword At; time-command corresponding to a single point in time.
        /// </summary>
        public const string KW_AT = "At";

        /// <summary>
        /// Keyword From; time-command corresponding to a bounded range in time.
        /// </summary>
        public const string KW_FROM = "From";

        /// <summary>
        /// Keyword Before; time-command corresponding to a negatively unbounded range in time.
        /// </summary>
        public const string KW_BEFORE = "Before";

        /// <summary>
        /// Keyword After; time-command corresponding to a positively unbounded range in time.
        /// </summary>
        public const string KW_AFTER = "After";

        /// <summary>
        /// Keyword AtIntervals; time-command corresponding to a periodic instantaneous point in time.
        /// </summary>
        public const string KW_AT_INTERVALS = "AtIntervals";

        /// <summary>
        /// Keyword DuringIntervals; time-command corresponding to a periodic bounded range in time.
        /// </summary>
        public const string KW_DURING_INTERVALS = "DuringIntervals";

        */

        /// <summary>
        /// Keyword Children; used to indicate a set of commands to apply to all children bullets.
        /// </summary>
        public const string KW_CHILDREN = "Children";

        /// <summary>
        /// Keyword Lambda; used to indicate an anonymous inline function.
        /// </summary>
        public const string KW_LAMBDA = "Lambda";

        /// <summary>
        /// Keyword Direction; used to indicate the intrinsic bullet-bound variable "Direction".
        /// </summary>
        public const string INTRINSIC_DIRECTION = "Direction";

        /// <summary>
        /// Keyword Speed; used to indicate the intrinsic bullet-bound variable "Speed".
        /// </summary>
        public const string INTRINSIC_SPEED = "Speed";

        /// <summary>
        /// Keyword Colour; used to indicate the intrinsic bullet-bound variable "Colour".
        /// </summary>
        public const string INTRINSIC_COLOUR = "Colour";

        /// <summary>
        /// Keyword Sprite; used to indicate the intrinsic bullet-bound variable "Sprite".
        /// </summary>
        public const string INTRINSIC_SPRITE = "Sprite";

        /// <summary>
        /// Keyword Position; used to indicate the intrinsic bullet-bound variable "Position".
        /// </summary>
        public const string INTRINSIC_POSITION = "Position";

        /// <summary>
        /// Keyword Origin; used to indicate the intrinsic bullet-bound variable "Origin".
        /// </summary>
        public const string INTRINSIC_ORIGIN = "Origin";

        /// <summary>
        /// Keyword Velocity; used to indicate the intrinsic bullet-bound variable "Velocity".
        /// </summary>
        public const string INTRINSIC_VELOCITY = "Velocity";

        /// <summary>
        /// Keyword Time; used to indicate the intrinsic bullet-bound variable "Time" (called LocalTime in DmlBullet).
        /// </summary>
        public const string INTRINSIC_TIME = "Time";

        #endregion

        #region Lists

        /// <summary>
        /// All tokens that are single characters.
        /// </summary>
        public static string[] SINGLE_CHAR_TOKENS = new string[] {
            PLUS,
            MINUS,
            STAR,
            SLASH,
            BSLASH,
            PERCENT,
            CIRCUMFLEX,
            TILDE,
            AT,
            DOLLAR,
            AMPERSAND,
            VBAR,
            BANG,
            HASH,
            NEWLINE,
            QUOTE,
            LPAR,
            RPAR,
            LBRACE,
            RBRACE,
            LSQUARE,
            RSQUARE,
            LANGLE,
            RANGLE,
            COMMA,
            DOT,
            EOL,
            EQUAL,
            DBLEQUAL,
            DBLAND,
            DBLVBAR,
            GTEQ,
            LTEQ,
            TRANSOP
        };

        /// <summary>
        /// All tokens that are two characters long and not keywords.
        /// </summary>
        public static string[] DOUBLE_CHAR_TOKENS = new string[] {
            DBLEQUAL,
            LTEQ,
            GTEQ,
            DBLAND,
            DBLVBAR,
            TRANSOP
        };

        /// <summary>
        /// All tokens that are three characters long and not keywords.
        /// </summary>
        public static string[] TRIPLE_CHAR_TOKENS = new string[] {
            ELLIPSES
        };

        /// <summary>
        /// All tokens that are keywords.
        /// </summary>
        private static string[] KEYWORDS = new string[] {
            KW_TRUE,
            KW_FALSE,
            KW_NULL,
            KW_ASSIGN,
            KW_RANGE,
            KW_BULLET,
            KW_PATTERN,
            KW_TIMELINE,
            KW_INIT,
            KW_UPDATE,
            KW_CHILDREN,
            KW_LAMBDA
        };

        #endregion

        /// <summary>
        /// Check if an expression is an operator token.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsOperator(string expression)
        {
            return SINGLE_CHAR_TOKENS.Contains(expression) ||
                   DOUBLE_CHAR_TOKENS.Contains(expression) ||
                   TRIPLE_CHAR_TOKENS.Contains(expression);
        }

        /// <summary>
        /// Check if an expression is a numeric token.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumber(string expression)
        {
            return Regex.IsMatch(expression, NUMBER);
        }

        /// <summary>
        /// Check if an expression is an unqualified name token.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsName(string expression)
        {
            return Regex.IsMatch(expression, UQNAME) && 
                   !KEYWORDS.Contains(expression) && 
                   !BuiltinsDict.Builtins.ContainsKey(expression) &&
                   !BehavioursDict.Behaviours.ContainsKey(expression);
        }

        public static bool IsNonKeywordName(string expression)
        {
            return !KEYWORDS.Contains(expression) &&
                   Regex.IsMatch(expression, UQNAME);
        }

        /// <summary>
        /// Check if an expression is a builtin name.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsBuiltin(string expression)
        {
            return BuiltinsDict.Builtins.ContainsKey(expression);
        }

        /// <summary>
        /// Check if an expression is a time command.
        /// </summary>
        public static bool IsTimeCommand(string expression)
        {
            // Alright I'm cheating a bit these are magic strings but you know what who gives a flying fuck,
            // its 4 in the morning, third night in a row, I've run out of coffee, my music's getting boring,
            // I have a migraine the size of Kentucky, I think we can afford to not be completely rigorous
            // this one time.
            return IsMatch(expression, "At") ||
                   IsMatch(expression, "AtGlobal") ||
                   IsMatch(expression, "From") ||
                   IsMatch(expression, "FromGlobal") ||
                   IsMatch(expression, "Outside") ||
                   IsMatch(expression, "OutsideGlobal") ||
                   IsMatch(expression, "Before") ||
                   IsMatch(expression, "BeforeGlobal") ||
                   IsMatch(expression, "After") ||
                   IsMatch(expression, "AfterGlobal") ||
                   IsMatch(expression, "AtIntervals") ||
                   IsMatch(expression, "AtIntervalsGlobal") ||
                   IsMatch(expression, "DuringIntervals") ||
                   IsMatch(expression, "DuringIntervalsGlobal");
        }

        /// <summary>
        /// Check if an expression is a keyword.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsKeyword(string expression)
        {
            return KEYWORDS.Contains(expression);
        }

        /// <summary>
        /// Check if an expression is a left bracket token.
        /// </summary>
        public static bool IsLeftBracket(string expression)
        {
            return IsMatch(expression, LPAR) ||
                   IsMatch(expression, LSQUARE) ||
                   IsMatch(expression, LBRACE) ||
                   IsMatch(expression, LANGLE);
        }

        /// <summary>
        /// Check if an expression is a right bracket token.
        /// </summary>
        public static bool IsRightBracket(string expression)
        {
            return IsMatch(expression, RPAR) ||
                   IsMatch(expression, RSQUARE) ||
                   IsMatch(expression, RBRACE) ||
                   IsMatch(expression, RANGLE);
        }

        /// <summary>
        /// Check if an expression is a builtin behaviour.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsBehaviour(string expression)
        {
            return BehavioursDict.Behaviours.ContainsKey(expression);
        }

        public static bool IsString(string expression)
        {
            // We never have to check if there are any quotation marks inbetween the
            // first and last ones because if that's the case then the Tokenizer will
            // have already split the two up into separate strings anyways.
            return expression[0] == '"' && expression[expression.Length - 1] == '"';
        }

        /// <summary>
        /// Check if an expression matches a given token.
        /// </summary>
        /// <param name="expression">The unknown expression.</param>
        /// <param name="token">A known token, presumably a const string DmlTokens field.</param>
        /// <returns></returns>
        public static bool IsMatch(string expression, string token)
        {
            switch (token)
            {
                case UQNAME:  return IsName(expression);
                case KEYWORD: return IsKeyword(expression);
                case BUILTIN: return IsBuiltin(expression);
                case NUMBER:  return IsNumber(expression);
                default:      return expression == token;
            }
        }

        /// <summary>
        /// Check if an expression matches any of a list of given tokens.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static bool IsMatch(string expression, string[] tokens)
        {
            foreach (string token in tokens)
                if (IsMatch(expression, token))
                    return true;
            return false;
        }

    }
}
