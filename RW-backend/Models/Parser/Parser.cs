using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Parser
{
    public class ErrorException : ApplicationException
    {
        public ErrorException(string msg) : base(msg)
        {
        }

    }
    public class Parser
    {
        public Parser(Dictionary<string, int> fluents)
        {
            if (fluents == null)
                throw new ErrorException("Parser requires dictionary with fluents' names and indexes");
            Fluents = fluents;
        }
        
        protected Scanner Scanner
        {
            get; set;
        }
        
        protected Tokens Token
        {
            get; set;
        }
        protected ParserClause Root
        {
            get; set;
        }

        protected Dictionary<string, int> Fluents
        {
            get; set;
        }
        public LogicClause ParseToLogicClause(string text)
        {
            throw new NotImplementedException();
        }

        protected ParserClause ParseText(string text)
        {
            Scanner = new Scanner(new MemoryStream(Encoding.UTF8.GetBytes(text)));
            Token = (Tokens)Scanner.Scan();
            Root = Start();
            if (Root == null)
                throw new ErrorException("The text is not a proper logic clause.");
            return Root;
        }

        private ParserClause Start(bool isNegation = false)
        {
            ParserClause lo;
            switch (Token)
            {
                case Tokens.End:
                    if (!string.IsNullOrEmpty(Scanner.Text))
                        lo = FluentClause();
                    else
                        lo = null;
                    break;
                case Tokens.Not:
                    Match(Tokens.Not);
                    lo = FluentClause(true);
                    break;
                case Tokens.Or:
                    lo = OrClause();
                    break;
                case Tokens.BracketStart:
                    Match(Tokens.BracketStart);
                    lo = Start();
                    Match(Tokens.BracketEnd);
                    break;
                
                //case Tokens.ArrayStart:
                //    Match(Tokens.ArrayStart);
                //    lo = Array0();
                //    Match(Tokens.ArrayEnd);
                //    break;
                //case Tokens.BlockStart:
                //    Match(Tokens.BlockStart);
                //    Match(Tokens.Sequence);
                //    lo = Image0();
                //    Match(Tokens.BlockEnd);
                //    break;
                //case Tokens.Sequence:
                //    lo = Sequence();
                //    Match(Tokens.Sequence);
                //    break;
                //case Tokens.Number:
                //    lo = Sequence();
                //    Match(Tokens.Number);
                //    break;
                //case Tokens.QuoteStart:
                //    Match(Tokens.QuoteStart);
                //    lo = Sequence();
                //    Match(Tokens.QuoteEnd);
                //    break;
                //case Tokens.Graph:
                //    Match(Tokens.Graph);
                //    lo = Graph(false);
                //    break;
                //case Tokens.Tree:
                //    Match(Tokens.Tree);
                //    lo = Graph(true);
                //    break;
                //case Tokens.VectorStart:
                //    Match(Tokens.VectorStart);
                //    lo = Vector();
                //    break;
                case Tokens.Error:
                    Root = null;
                    throw new ErrorException($"Error: Unexpected symbol: {Scanner.CurrentSymbol}, on position: {Scanner.CurrentPosition}, after text: {Scanner.UntilNowText}");
                default:
                    lo = null;
                    break;
            }
            //if (lo != null && lo.Next == null && CheckToken(isGraph))
            //{
            //    while (Token == Tokens.NewLine)
            //        Match(Tokens.NewLine);
            //    lo.Next = Start(isGraph);
            //    if (lo.Next != null)
            //    {
            //        lo.Next.Previous = lo;
            //    }
            //}
            return lo;
        }

        
        private ParserClause FluentClause(bool isNegation=false)
        {
            if(!Fluents.ContainsKey(Scanner.Text))
                throw new ErrorException($"No defined fluent with name {Scanner.Text}.");
            ParserClause pc = new FluentParserClause(Fluents[Scanner.Text], isNegation);
            Scanner.ClearText();
            return pc;
        }

        protected ParserClause OrClause()
        {
            ParserClause pc = new OrParserClause();

            return pc;
        }

        //protected ParserObject Image0()
        //{
        //    ImageObject obj = new ImageObject();

        //    if (!Fluents.ContainsKey(Scanner.Text))
        //    {
        //        throw new ErrorException($"Brak obrazka o nazwie: {Scanner.Text}");
        //    }
        //    obj.ImageBitmap = Fluents[Scanner.Text];
        //    Scanner.ClearText();
        //    return obj;
        //}

        ///// <summary>
        ///// Method checking whether after current token a new object can be started
        ///// </summary>
        ///// <param name="isGraph">Whether there is ongoing parsing of the graph</param>
        ///// <returns>True if the new object can be started, otherwise false</returns>
        //private bool CheckToken(bool isGraph)
        //{
        //    HashSet<Tokens> tokens = new HashSet<Tokens> {Tokens.ArrayStart, Tokens.Backslash, Tokens.BlockStart,
        //        Tokens.Graph, Tokens.QuoteStart, Tokens.Tree, Tokens.Sequence, Tokens.VectorStart, Tokens.Number, Tokens.NewLine};
        //    return tokens.Contains(Token) && !(isGraph && Token == Tokens.NewLine);
        //}

        ///// <summary>
        ///// Creates VectorObject
        ///// </summary>
        ///// <returns>Parsed VectorObject</returns>
        //protected ParserObject Vector()
        //{
        //    VectorObject obj = new VectorObject();
        //    if (Token == Tokens.VectorDot)
        //    {
        //        Match(Tokens.VectorDot);
        //        obj.IsStart = true;
        //    }
        //    obj.Number = MatchNumber();
        //    if (obj.IsStart)
        //    {
        //        if (Token == Tokens.Label)
        //        {
        //            obj.Label = MatchLabel();
        //        }
        //    }
        //    else
        //    {
        //        Match(Tokens.VectorDot);
        //    }

        //    Match(Tokens.VectorEnd);
        //    obj.Object = Start();
        //    if (obj.Object != null && obj.Object.Next != null)
        //    {
        //        obj.Next = obj.Object.Next;
        //        obj.Object.Next = null;
        //    }
        //    return obj;
        //}
        ///// <summary>
        ///// Creates GraphObject
        ///// </summary>
        ///// <param name="isTree">Whether the object is a tree</param>
        ///// <returns>Parsed GraphObject</returns>
        //protected ParserObject Graph(bool isTree)
        //{
        //    GraphObject obj = new GraphObject();
        //    obj.IsTree = isTree;
        //    Scanner.ClearText();
        //    while (Token == Tokens.NewLine)
        //    {
        //        Match(Tokens.NewLine);
        //    }
        //    Match(Tokens.BlockStart);
        //    if (Token == Tokens.NewLine)
        //    {
        //        Match(Tokens.NewLine);
        //    }
        //    //vertices
        //    while (Token == Tokens.Number)
        //    {
        //        int num = MatchNumber();
        //        Match(Tokens.Label);
        //        //vertice definition start
        //        obj.AddVertice(num, Start(true));
        //        while (Token == Tokens.NewLine)
        //        {
        //            Match(Tokens.NewLine);
        //        }
        //    }
        //    if (obj.VerticesNumber < 1)
        //    {
        //        throw new ErrorException($"Graf musi mieć przynajmniej jeden wierzchołek.");
        //    }
        //    if (Token == Tokens.GraphSeparator)
        //    {
        //        Match(Tokens.GraphSeparator);
        //        while (Token == Tokens.NewLine)
        //        {
        //            Match(Tokens.NewLine);
        //        }
        //        //edges
        //        while (Token == Tokens.Number)
        //        {
        //            int v1, v2;
        //            string label = null;
        //            bool directed;
        //            v1 = MatchNumber();
        //            if (Token == Tokens.Directed)
        //            {
        //                Match(Tokens.Directed);
        //                directed = true;
        //            }
        //            else if (Token == Tokens.Undirected)
        //            {
        //                Match(Tokens.Undirected);
        //                directed = false;
        //            }
        //            else
        //            {
        //                throw new ErrorException($"Oczekiwano krawędzi, otrzymano: {Scanner.Text}");
        //            }
        //            v2 = MatchNumber();
        //            if (Token == Tokens.Label)
        //            {
        //                label = MatchLabel();
        //            }
        //            obj.AddEdge(v1, v2, directed, label);
        //            while (Token == Tokens.NewLine)
        //            {
        //                Match(Tokens.NewLine);
        //            }
        //        }
        //    }

        //    Match(Tokens.BlockEnd);
        //    return obj;
        //}
        ///// <summary>
        ///// Creates ImageObject
        ///// </summary>
        ///// <returns>Parsed ImageObject</returns>
        //protected ParserObject Image0()
        //{
        //    ImageObject obj = new ImageObject();

        //    if (!Fluents.ContainsKey(Scanner.Text))
        //    {
        //        throw new ErrorException($"Brak obrazka o nazwie: {Scanner.Text}");
        //    }
        //    obj.ImageBitmap = Fluents[Scanner.Text];
        //    Scanner.ClearText();
        //    return obj;
        //}
        ///// <summary>
        ///// Creates SequenceObject
        ///// </summary>
        ///// <returns>Parsed SequenceObject</returns>
        //protected ParserObject Sequence()
        //{
        //    SequenceObject obj = new SequenceObject(Scanner.Text);
        //    Scanner.ClearText();
        //    return obj;
        //}
        ///// <summary>
        ///// Creates ArrayObject
        ///// </summary>
        ///// <returns>Parsed ArrayObject</returns>
        //protected ParserObject Array0()
        //{
        //    ArrayObject obj = new ArrayObject();
        //    MatchHash(obj, 0);
        //    int i = 1;
        //    while (Token == Tokens.ArraySeparator)
        //    {
        //        Match(Tokens.ArraySeparator);
        //        MatchHash(obj, i);
        //        i++;
        //    }
        //    return obj;
        //}
        ///// <summary>
        ///// Checks whether the current array field is a hash and creates object accordingly
        ///// </summary>
        ///// <param name="obj">Array to check</param>
        ///// <param name="index">Index in the array to check</param>
        //protected void MatchHash(ArrayObject obj, int index)
        //{
        //    if (Token == Tokens.HashStart)
        //    {
        //        Match(Tokens.HashStart);
        //        obj.IsHashTable = true;
        //        obj.HashObjects.Add(index, new HashWrapper(Start(), true));
        //        Match(Tokens.HashEnd);
        //        Match(Tokens.VectorEnd);
        //        obj.Add(Start());
        //    }
        //    else
        //    {
        //        obj.Add(Start());
        //        if (Token == Tokens.VectorEnd)
        //        {
        //            Match(Tokens.VectorEnd);
        //            obj.IsHashTable = true;
        //            obj.HashObjects.Add(index, new HashWrapper(Start(), false));
        //        }
        //    }
        //}
        ///// <summary>
        ///// Matches number
        ///// </summary>
        ///// <returns>Scanned number</returns>
        //protected int MatchNumber()
        //{
        //    Match(Tokens.Number);
        //    int num;
        //    if (!int.TryParse(Scanner.Text, out num))
        //    {
        //        throw new ErrorException($"Nieprawidłowy numer wierzchołka: {Scanner.Text}");
        //    }
        //    Scanner.ClearText();
        //    return num;
        //}
        ///// <summary>
        ///// Matches label
        ///// </summary>
        ///// <returns>Scanned labes</returns>
        //protected string MatchLabel()
        //{
        //    Match(Tokens.Label);
        //    StringBuilder label = new StringBuilder(Scanner.Text);
        //    Scanner.ClearText();
        //    if (Token == Tokens.Sequence)
        //        Match(Tokens.Sequence);
        //    else
        //        Match(Tokens.Number);
        //    while (Token == Tokens.Sequence || Token == Tokens.Number)
        //    {
        //        label.Append(" ").Append(Scanner.Text);
        //        Scanner.ClearText();
        //        Match(Token);
        //    }
        //    return label.ToString();
        //}

        protected void Match(Tokens expected)
        {
            if (Token != expected)
            {
                Root = null;
                throw new ErrorException($"Error: Unexpected symbol: {Scanner.CurrentSymbol}, on position: {Scanner.CurrentPosition}, after text: {Scanner.UntilNowText}");
            }
            Token = (Tokens)Scanner.Scan();
        }
    }
}
