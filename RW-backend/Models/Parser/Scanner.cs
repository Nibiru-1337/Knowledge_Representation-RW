using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Parser
{
    public enum Tokens
    {
        Start, BracketStart, BracketEnd,
        FluentName,
        Or, And, Not,
        //ArrayStart, ArraySeparator, ArrayEnd, Sequence, Endl, Backslash, BlockStart, BlockEnd, QuoteStart, QuoteEnd, QuoteBackslash,
        //Separator, Tree, Graph, GraphSeparator, Number, Label, Directed, Undirected, NewLine, VectorStart, VectorEnd, VectorDot,
        //HashStart, HashEnd,
        End,
        Error = -1
    }
    public class Scanner
    {
        protected string SourceText;
        public int CurrentPosition
        {
            get; protected set;
        }
        public Tokens CurrentState
        {
            get; protected set;
        }
        public StringBuilder CurrentText
        {
            get; protected set;
        }

        protected Dictionary<Tokens, Dictionary<char, Action>> ActionTable;
        protected Dictionary<Tokens, Action> DefaultActionTable;
        
        public string Text => CurrentText.ToString();
        public string UntilNowText => SourceText.Substring(0, CurrentPosition);
        
        public char CurrentSymbol => SourceText[CurrentPosition];

        
        public Scanner(Stream s)
        {
            ActionTable = new Dictionary<Tokens, Dictionary<char, Action>>();
            DefaultActionTable = new Dictionary<Tokens, Action>();

            DefaultActionTable[Tokens.Start] = () =>
            {
                CurrentState = Tokens.Error;
                CurrentText.Append(SourceText[CurrentPosition]);
                CurrentPosition++;
            };

            ActionTable[Tokens.Start] = new Dictionary<char, Action>();
            ActionTable[Tokens.Start]['\0'] = () =>
            {
                CurrentState = Tokens.End;
            };

            AddChars(Tokens.Start, Tokens.Start);
            ChangeState('(', Tokens.Start, Tokens.BracketStart);
            ChangeState(')', Tokens.Start, Tokens.BracketEnd);
            ChangeState('!', Tokens.Start, Tokens.Not);
            ChangeState('&', Tokens.Start, Tokens.And);
            ChangeState('|', Tokens.Start, Tokens.Or);
            //AddChars("0123456789", Tokens.Start, Tokens.Number);
            //ChangeState(" \t", Tokens.Start, Tokens.Start);
            //ChangeState('[', Tokens.Start, Tokens.ArrayStart);
            //ChangeState(']', Tokens.Start, Tokens.ArrayEnd);
            //ChangeState('|', Tokens.Start, Tokens.ArraySeparator);
            //ChangeState('\\', Tokens.Start, Tokens.Backslash);
            //ChangeState('{', Tokens.Start, Tokens.BlockStart);
            //ChangeState('}', Tokens.Start, Tokens.BlockEnd);
            //ChangeState('=', Tokens.Start, Tokens.GraphSeparator);
            //ChangeState(':', Tokens.Start, Tokens.Label);
            //ChangeState('-', Tokens.Start, Tokens.Undirected);
            //ChangeState(";\n", Tokens.Start, Tokens.NewLine);
            //ChangeState('<', Tokens.Start, Tokens.VectorStart);
            //ChangeState('>', Tokens.Start, Tokens.VectorEnd);
            //ChangeState('.', Tokens.Start, Tokens.VectorDot);
            //ChangeState('(', Tokens.Start, Tokens.HashStart);
            //ChangeState(')', Tokens.Start, Tokens.HashEnd);
            //SetAction("\"", Tokens.Start,
            //    () =>
            //    {
            //        if (!WasQuoteStart)
            //        {
            //            CurrentState = Tokens.QuoteStart;
            //            WasQuoteStart = true;
            //        }
            //        else
            //        {
            //            CurrentState = Tokens.QuoteEnd;
            //            WasQuoteStart = false;
            //        }

            //        CurrentPosition++;
            //    });

            //SetAction("t", Tokens.Start,
            //    () =>
            //    {
            //        CurrentText.Append(SourceText[CurrentPosition]);
            //        Template = "tree";
            //        CurrentPositionInTemplate = 1;
            //        CurrentPosition++;
            //        CurrentState = Tokens.Tree;
            //    });
            //SetAction("g", Tokens.Start,
            //    () =>
            //    {
            //        CurrentText.Append(SourceText[CurrentPosition]);
            //        Template = "graph";
            //        CurrentPositionInTemplate = 1;
            //        CurrentPosition++;
            //        CurrentState = Tokens.Graph;
            //    });

            ////sequence
            //ActionTable[Tokens.Sequence] = new Dictionary<char, Action>();
            //AddChars(Tokens.Sequence, Tokens.Sequence);
            //AddChars("0123456789", Tokens.Sequence, Tokens.Sequence);

            ////backslash
            //ActionTable[Tokens.Backslash] = new Dictionary<char, Action>();
            //AddChars(Tokens.Backslash, Tokens.Sequence);
            //AddChars(GetSpecialChars, Tokens.Backslash, Tokens.Sequence);
            //AddChars(" \t", Tokens.Backslash, Tokens.Backslash);

            ////quote
            //ActionTable[Tokens.QuoteStart] = new Dictionary<char, Action>();
            //AddChars(Tokens.QuoteStart, Tokens.QuoteStart);
            //AddChars("0123456789", Tokens.QuoteStart, Tokens.QuoteStart);
            //AddChars(GetSpecialChars, Tokens.QuoteStart, Tokens.QuoteStart);
            //AddChars(" \t", Tokens.QuoteStart, Tokens.QuoteStart);
            //ChangeState('\\', Tokens.QuoteStart, Tokens.QuoteBackslash);
            //ActionTable[Tokens.QuoteStart].Remove('\"');

            ////backslash in Quote
            //ActionTable[Tokens.QuoteBackslash] = new Dictionary<char, Action>();
            //AddChars(Tokens.QuoteBackslash, Tokens.QuoteStart);
            //AddChars(GetSpecialChars, Tokens.QuoteBackslash, Tokens.QuoteStart);
            //AddChars(" \t", Tokens.QuoteBackslash, Tokens.QuoteBackslash);

            ////tree
            //ActionTable[Tokens.Tree] = new Dictionary<char, Action>();
            //AddChars(Tokens.Tree, Tokens.Sequence);
            //AddChars("0123456789", Tokens.Tree, Tokens.Sequence);
            //AddCharsWithTemplate("re", Tokens.Tree, Tokens.Sequence);


            ////graph
            //ActionTable[Tokens.Graph] = new Dictionary<char, Action>();
            //AddChars(Tokens.Graph, Tokens.Sequence);
            //AddChars("0123456789", Tokens.Tree, Tokens.Sequence);
            //AddCharsWithTemplate("raph", Tokens.Graph, Tokens.Sequence);

            //ActionTable[Tokens.Undirected] = new Dictionary<char, Action>();
            //ChangeState('>', Tokens.Undirected, Tokens.Directed);

            ////number
            //ActionTable[Tokens.Number] = new Dictionary<char, Action>();
            //AddChars("0123456789", Tokens.Number, Tokens.Number);


            CurrentText = new StringBuilder();
            var sr = new StreamReader(s);
            SourceText = sr.ReadToEnd() + '\0';
            sr.Close();
        }
        #region chars
        
        
        /// <summary>
        /// Regular chars
        /// </summary>
        /// <returns>String contating regular chars</returns>
        protected string GetChars()
        {
            StringBuilder sb = new StringBuilder();
            for (char c = 'a'; c <= 'z'; ++c)
            {
                sb.Append(c);
            }
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                sb.Append(c);
            }
            sb.Append("ąśżźćęółńĄŚŻŹĆĘÓŁŃ");
            return sb.ToString();
        }
        /// <summary>
        /// Sets action if any char from the string appears in the specified state
        /// </summary>
        /// <param name="chars">List of chars</param>
        /// <param name="t">Current state</param>
        /// <param name="action">Action to set</param>
        protected void SetAction(string chars, Tokens t, Action action)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                ActionTable[t][chars[i]] = action;
            }
        }

        protected void AddChars(Tokens t, Tokens newToken)
        {
            AddChars(GetChars(), t, newToken);
        }

        /// <summary>
        /// Adds chars and changes current state
        /// </summary>
        /// <param name="chars">List of chars</param>
        /// <param name="t">Current state</param>
        /// <param name="newToken">New state</param>
        protected void AddChars(string chars, Tokens t, Tokens newToken)
        {
            SetAction(chars, t, () =>
            {
                CurrentState = newToken;
                CurrentText.Append(SourceText[CurrentPosition]);
                CurrentPosition++;
            }
            );
        }
        /// <summary>
        /// Changes state when any char is met
        /// </summary>
        /// <param name="chars">List of chars</param>
        /// <param name="t">Current state</param>
        /// <param name="newToken">New state</param>
        protected void ChangeState(string chars, Tokens t, Tokens newToken)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                ChangeState(chars[i], t, newToken);
            }
        }
        /// <summary>
        /// Changes state when specified char is met
        /// </summary>
        /// <param name="c">The char to change state for</param>
        /// <param name="t">Current state</param>
        /// <param name="newToken">New state</param>
        protected void ChangeState(char c, Tokens t, Tokens newToken)
        {
            ActionTable[t][c] = () =>
            {
                CurrentState = newToken;
                CurrentPosition++;
            };
        }

        #endregion
        /// <summary>
        /// Scans text until the next state is met
        /// </summary>
        /// <returns>Current state</returns>
        public int Scan()
        {
            char c;
            CurrentState = Tokens.Start;
            while (true)
            {
                c = SourceText[CurrentPosition];
                if (ActionTable.ContainsKey(CurrentState) && ActionTable[CurrentState].ContainsKey(c))
                    ActionTable[CurrentState][c]();
                else
                {
                    if (DefaultActionTable.ContainsKey(CurrentState))
                        DefaultActionTable[CurrentState]();
                    return (int)CurrentState;
                }
            }
        }
        public void ClearText()
        {
            CurrentText = new StringBuilder();
        }
    }
}
