﻿using System;
using System.Collections.Generic;
using System.Linq;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;

namespace RW_backend.Logic.Parser
{
    
    public class Parser
    {
        public Parser(Dictionary<string, int> fluents)
        {
            if (fluents == null)
                throw new ArgumentException("Parser requires dictionary with fluents' names and indexes");
            Fluents = fluents;
            //LastToken = Tokens.Error;
        }
        

        protected Dictionary<string, int> Fluents
        {
            get; set;
        }

        protected char OrSymbol = '|';
        protected char AndSymbol = '&';
        protected char NotSymbol = '!';
        protected char BracketStart = '(';
        protected char BracketEnd = ')';
        //protected Tokens LastToken
        //{
        //    get; set;
        //}
        public LogicClause ParseToLogicClause(string text)
        {
            return ParseText(text);
            //throw new NotImplementedException();
        }

        protected LogicClause ParseText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new LogicClausesFactory().CreateEmptyLogicClause();
            //assumption CNF or DNF
            bool isCNF = CheckIfCNF(text);
            text = text.Replace(BracketStart.ToString(), "");
            text = text.Replace(BracketEnd.ToString(), "");
            text = text.Replace(" ", "");
            if (isCNF)
            {
                return GetCNF(text);
            }
            return GetDNF(text);
        }

        protected LogicClause GetCNF(string text)
        {
            ConjunctionOfAlternatives lc = new ConjunctionOfAlternatives();
            string[] conjs = text.Split(AndSymbol);
            for (int i = 0; i < conjs.Length; i++)
            {
                if(conjs[i] == "")
                    throw new ArgumentException($"Error in clause text: {text}\nMaybe you missed fluent name");
                string[] alts = conjs[i].Split(OrSymbol);
                UniformAlternative alt = new UniformAlternative();
                for (int j = 0; j < alts.Length; j++)
                {
                    if (alts[j] == "")
                        throw new ArgumentException($"Error in clause text: {text}\nMaybe you missed fluent name");
                    bool isNegation = false;
                    if (alts[j][0] == NotSymbol)
                    {
                        isNegation = true;
                        alts[j] = alts[j].Substring(1);
                    }
                    if(!Fluents.ContainsKey(alts[j]))
                        throw new ArgumentException($"There is no defined fluent: {alts[j]}");
                    alt.AddFluent(Fluents[alts[j]], isNegation ? FluentSign.Negated : FluentSign.Positive);
                }
                lc.AddAlternative(alt);
            }
            return lc;
        }

        protected LogicClause GetDNF(string text)
        {
            AlternativeOfConjunctions lc = new AlternativeOfConjunctions();
            string[] alts = text.Split(OrSymbol);
            for (int i = 0; i < alts.Length; i++)
            {
                if (alts[i] == "")
                    throw new ArgumentException($"Error in clause text: {text}\nMaybe you missed fluent name");
                string[] conjs = alts[i].Split(AndSymbol);
                UniformConjunction conj = new UniformConjunction();
                for (int j = 0; j < conjs.Length; j++)
                {
                    if(conjs[j] == "")
                        throw new ArgumentException($"Error in clause text: {text}\nMaybe you missed fluent name");
                    bool isNegation = false;
                    if (conjs[j][0] == NotSymbol)
                    {
                        isNegation = true;
                        conjs[j] = conjs[j].Substring(1);
                    }
                    if (!Fluents.ContainsKey(conjs[j]))
                        throw new ArgumentException($"There is no defined fluent: {conjs[j]}");
                    conj.AddFluent(Fluents[conjs[j]], isNegation ? FluentSign.Negated : FluentSign.Positive);
                }
                lc.AddConjunction(conj);
            }
            return lc;
        }
        protected bool CheckIfCNF(string text)
        {
            //only fluent, fluent negation or conjuctions
            if (!text.Contains(OrSymbol))
                return true;

            //only alternatives
            if (!text.Contains(AndSymbol))
                return false;

            //assumption: no nested brackets
            text = text.TrimEnd();
            text = text.TrimStart();
            int bracket = text.IndexOf(BracketEnd);

            //if no brackets and there are conjuctions and alternatives - we assume it is CNF
            if (bracket == -1)
                return true;

            //if the bracket isn't at the end we check the next symbol
            if (bracket != text.Length - 1)
            {
                string temp = text.Substring(bracket + 1);
                temp = temp.TrimStart();
                //and is between brackets
                if (temp[0] == AndSymbol)
                    return true;
                //or is between brackets
                return false;
            }
            //the bracket must have been at the end
            bracket = text.LastIndexOf(BracketStart);
            if (bracket != 0)
            {
                string temp = text.Substring(0, bracket);
                temp = temp.TrimEnd();
                //and is between brackets
                if (temp[temp.Length-1] == AndSymbol)
                    return true;
                //or is between brackets
                return false;
            }
            throw new ArgumentException($"The text: {text} is not a proper clause.");
        }

        
    }
}
