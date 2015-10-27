using Phosphaze.Core.Dml.Parse.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse
{

    public class ExpressionBuilder : AbstractParser
    {

        private List<Instruction> instructions = new List<Instruction>();
        Stack<string> operators = new Stack<string>();

        static List<string> OPERATORS = new List<string>()
        {
            DmlTokens.PLUS,
            DmlTokens.MINUS,
            DmlTokens.STAR,
            DmlTokens.SLASH,
            DmlTokens.PERCENT,
            DmlTokens.UNARY_NEG,
            DmlTokens.UNARY_ABS,
            DmlTokens.CIRCUMFLEX,
            DmlTokens.DBLAND,
            DmlTokens.DBLVBAR,
            DmlTokens.BANG,
            DmlTokens.DBLEQUAL,
            DmlTokens.NOTEQUAL,
            DmlTokens.LANGLE,
            DmlTokens.RANGLE,
            DmlTokens.LTEQ,
            DmlTokens.GTEQ
        };

        static List<string> RIGHT_ASSOCIATIVE = new List<string>()
        {
            DmlTokens.CIRCUMFLEX,
            DmlTokens.UNARY_NEG,
            DmlTokens.UNARY_ABS,
            DmlTokens.BANG
        };

        static List<string> LEFT_ASSOCIATIVE = new List<string>()
        {
            DmlTokens.PLUS,
            DmlTokens.MINUS,
            DmlTokens.STAR,
            DmlTokens.SLASH,
            DmlTokens.PERCENT,
            DmlTokens.DBLAND,
            DmlTokens.DBLVBAR,
            DmlTokens.DBLEQUAL,
            DmlTokens.NOTEQUAL,
            DmlTokens.LANGLE,
            DmlTokens.RANGLE,
            DmlTokens.LTEQ,
            DmlTokens.GTEQ
        };

        static Dictionary<string, int> OPERATOR_PRECEDENCE = new Dictionary<string, int>()
        {
            {DmlTokens.DBLVBAR,    0},
            {DmlTokens.DBLAND,     1},
            {DmlTokens.BANG,       2},
            {DmlTokens.DBLEQUAL,   3},
            {DmlTokens.NOTEQUAL,   3},
            {DmlTokens.LANGLE,     3},
            {DmlTokens.RANGLE,     3},
            {DmlTokens.LTEQ,       3},
            {DmlTokens.GTEQ,       3},
            {DmlTokens.PLUS,       4},
            {DmlTokens.MINUS,      5},
            {DmlTokens.STAR,       6},
            {DmlTokens.SLASH,      7},
            {DmlTokens.PERCENT,    8},
            {DmlTokens.UNARY_NEG,  9},
            {DmlTokens.UNARY_ABS,  9},
            {DmlTokens.CIRCUMFLEX, 9}
        };

        static Dictionary<string, Instruction> OPERATOR_INSTRUCTIONS = new Dictionary<string, Instruction>()
        {
            {DmlTokens.PLUS,       BinaryAdd.Instance},
            {DmlTokens.MINUS,      BinarySub.Instance},
            {DmlTokens.STAR,       BinaryMul.Instance},
            {DmlTokens.SLASH,      BinaryDiv.Instance},
            {DmlTokens.PERCENT,    BinaryMod.Instance},
            {DmlTokens.CIRCUMFLEX, BinaryPow.Instance},
            {DmlTokens.UNARY_NEG,   UnaryNeg.Instance},
            {DmlTokens.UNARY_ABS,   UnaryAbs.Instance},
            {DmlTokens.DBLAND,     BinaryAnd.Instance},
            {DmlTokens.DBLVBAR,     BinaryOr.Instance},
            {DmlTokens.BANG,        UnaryNot.Instance},
            {DmlTokens.DBLEQUAL,    BinaryEq.Instance},
            {DmlTokens.NOTEQUAL,   BinaryNeq.Instance},
            {DmlTokens.LANGLE,      BinaryLt.Instance},
            {DmlTokens.RANGLE,      BinaryGt.Instance},
            {DmlTokens.LTEQ,      BinaryLtEq.Instance},
            {DmlTokens.GTEQ,      BinaryGtEq.Instance}
        };

        public ExpressionBuilder(string[] tokens, int currentLine) : base(tokens, currentLine) { }

        protected override void ProcessNext()
        {
            if (DmlTokens.IsMatch(CurrentToken, DmlTokens.AT) ||
                DmlTokens.IsMatch(CurrentToken, DmlTokens.DOLLAR) ||
                DmlTokens.IsNonKeywordName(CurrentToken) ||
                DmlTokens.IsBuiltin(CurrentToken))
            {
                Instruction load;
                string badName = "Invalid syntax; invalid name \"{0}\".";
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.AT))
                {
                    Advance(exception: new DmlSyntaxError("Invalid syntax: global identifier `@` must be followed by a name."));
                    if (!DmlTokens.IsName(CurrentToken))
                        throw new DmlSyntaxError(String.Format(badName, CurrentToken));
                    load = new LoadGlobal(CurrentToken);
                }
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.DOLLAR))
                {
                    Advance(exception: new DmlSyntaxError("Invalid syntax: identifier `$` must be followed by a name."));
                    if (DmlTokens.IsName(CurrentToken))
                    {
                        switch (CurrentToken)
                        {
                            case DmlTokens.INTRINSIC_DIRECTION:
                                load = LoadIntrinsicBulletProperty.Direction;
                                break;
                            case DmlTokens.INTRINSIC_SPEED:
                                load = LoadIntrinsicBulletProperty.Speed;
                                break;
                            case DmlTokens.INTRINSIC_COLOUR:
                                load = LoadIntrinsicBulletProperty.Colour;
                                break;
                            case DmlTokens.INTRINSIC_ORIGIN:
                                load = LoadIntrinsicBulletProperty.Origin;
                                break;
                            case DmlTokens.INTRINSIC_POSITION:
                                load = LoadIntrinsicBulletProperty.Position;
                                break;
                            case DmlTokens.INTRINSIC_VELOCITY:
                                load = LoadIntrinsicBulletProperty.Velocity;
                                break;
                            case DmlTokens.INTRINSIC_TIME:
                                load = LoadIntrinsicBulletProperty.Time;
                                break;
                            default:
                                load = new LoadInstanceBound(CurrentToken);
                                break;
                        }
                    }
                    else
                        throw new DmlSyntaxError(String.Format(badName, CurrentToken));

                }
                else if (DmlTokens.IsName(CurrentToken))
                    load = new LoadLocal(CurrentToken);
                else
                    load = new LoadConstant(BuiltinsDict.Builtins[CurrentToken]);

                bool isCall = false;
                // Check if this is a function call.
                // We actually want to catch the ParserError thrown by Advance() in this case
                // because it's very possible the expression could end with a name.
                if (NextToken != null)
                {
                    Advance();
                    if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LPAR))
                    {
                        isCall = true;
                        ParseFuncCall(load);
                    }
                    else
                        Reverse();
                }
                if (!isCall)
                    instructions.Add(load);
            }
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_TRUE))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Bool, true)));
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_FALSE))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Bool, false)));
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_NULL))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Null, null)));
            // Check for a left parenthesis.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LPAR))
                // We know this will never be the start of a function call because
                // that case would be caught by the above if block.
                operators.Push(CurrentToken);
            // Check for a right parenthesis.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.RPAR))
            {
                // By the shunting-yard algorithm, we pop every operator until we find a left
                // parenthesis. If we don't find one, then there are mismatched parentheses.
                if (operators.Count == 0 || !operators.Contains(DmlTokens.LPAR))
                    throw DmlSyntaxError.MismatchedParentheses();
                string top;
                while (operators.Count > 0)
                {
                    top = operators.First();
                    if (DmlTokens.IsMatch(top, DmlTokens.LPAR))
                    {
                        operators.Pop();
                        break;
                    }
                    instructions.Add(OPERATOR_INSTRUCTIONS[operators.Pop()]);
                }
            }
            // Check if the next token is a valid mathematical operator.
            else if (IsOperator(CurrentToken))
            {
                if (operators.Count == 0)
                    operators.Push(CurrentToken);
                // If there are prior operators, pop them and add them to the instructions according to their
                // associativity and precedence.
                else
                {
                    string top;
                    while (operators.Count > 0)
                    {
                        top = operators.First();
                        if ((IsLeftAssociative(top) && OPERATOR_PRECEDENCE[top] >= OPERATOR_PRECEDENCE[CurrentToken]) ||
                            (IsRightAssociative(top) && OPERATOR_PRECEDENCE[top] > OPERATOR_PRECEDENCE[CurrentToken]))
                            instructions.Add(OPERATOR_INSTRUCTIONS[operators.Pop()]);
                        else
                            break;
                    }
                    operators.Push(CurrentToken);
                }
            }
            // Check for a number.
            else if (DmlTokens.IsNumber(CurrentToken))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Number, Double.Parse(CurrentToken))));
            // Check for True.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_TRUE))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Bool, true)));
            // Check for False.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_FALSE))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Bool, false)));
            // Check for Null.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_NULL))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.Null, null)));
            else if (DmlTokens.IsString(CurrentToken))
                instructions.Add(new LoadConstant(new DmlObject(DmlType.String, CurrentToken)));
            // Check for Lambda.
            else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.KW_LAMBDA))
                throw new NotImplementedException("This hasn't been implemented yet, sorry!");
            else
                throw DmlSyntaxError.InvalidTokenForContext(CurrentToken, "expression");
        }

        protected override void PostParse()
        {
            base.PostParse();

            // Add all the remaining operators on the stack.
            foreach (string op in operators)
                instructions.Add(OPERATOR_INSTRUCTIONS[op]);
        }

        /// <summary>
        /// Return the result of parsing the expression. The object returned is an array of instructions.
        /// </summary>
        /// <returns></returns>
        public override object GetResult()
        {
            return instructions.ToArray();
        }

        /// <summary>
        /// Check if a token is a valid operator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsOperator(string token)
        {
            return OPERATORS.Contains(token);
        }

        /// <summary>
        /// Check if an operator is left associative.
        /// 
        /// If a binary operator ~ is left associative then a ~ b ~ c evaluates to (a ~ b) ~ c.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private bool IsLeftAssociative(string op)
        {
            return LEFT_ASSOCIATIVE.Contains(op);
        }

        /// <summary>
        /// Check if an operator is right associative.
        /// 
        /// If a binary operator ~ is right associative then a ~ b ~ c evaluates to a ~ (b ~ c).
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private bool IsRightAssociative(string op)
        {
            return RIGHT_ASSOCIATIVE.Contains(op);
        }

        /// <summary>
        /// Parse a function call using the given loading instruction.
        /// </summary>
        /// <param name="load"></param>
        private void ParseFuncCall(Instruction load)
        {
            int parenthDepth = 1;

            DmlSyntaxError badCall = new DmlSyntaxError(
                "Invalid syntax; mismatched brackets for function call.");

            var subExpressions = new List<List<string>>();
            var currentExpression = new List<string>();

            while (true)
            {
                Advance(exception: badCall);
                if (DmlTokens.IsMatch(CurrentToken, DmlTokens.LPAR))
                    parenthDepth++;
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.RPAR))
                    parenthDepth--;
                else if (DmlTokens.IsMatch(CurrentToken, DmlTokens.COMMA) && parenthDepth == 1) 
                {
                    subExpressions.Add(currentExpression);
                    currentExpression = new List<string>();
                    continue;
                }
                if (parenthDepth == 0) 
                {
                    if (currentExpression.Count > 0)
                        subExpressions.Add(currentExpression);
                    break;
                }
                currentExpression.Add(CurrentToken);
            }

            ExpressionBuilder eb;
            foreach (var subExpression in subExpressions)
            {
                eb = new ExpressionBuilder(subExpression.ToArray(), currentLine);
                eb.Parse();

                foreach (Instruction instruction in (Instruction[])(eb.GetResult()))
                    instructions.Add(instruction);
            }

            instructions.Add(load);
            instructions.Add(new CallFunction(subExpressions.Count));
        }

    }
}
