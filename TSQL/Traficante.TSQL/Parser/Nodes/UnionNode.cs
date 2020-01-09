﻿using System.Linq;
using Traficante.TSQL.Evaluator.Visitors;
using Traficante.TSQL.Parser.Tokens;

namespace Traficante.TSQL.Parser.Nodes
{
    public class UnionNode : SetOperatorNode
    {
        public UnionNode(string tableName, string[] keys, Node left, Node right, bool isNested, bool isTheLastOne)
            : base(TokenType.Union, keys, left, right, isNested, isTheLastOne)
        {
            ResultTableName = tableName;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var keys = Keys.Length == 0 ? string.Empty : Keys.Aggregate((a, b) => a + "," + b);
            return $"{Left.ToString()} union ({keys}) {Right.ToString()}";
        }
    }
}