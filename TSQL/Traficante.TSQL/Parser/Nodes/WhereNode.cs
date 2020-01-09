﻿using System;
using Traficante.TSQL.Evaluator.Visitors;

namespace Traficante.TSQL.Parser.Nodes
{
    public class WhereNode : Node
    {
        public WhereNode(Node expression)
        {
            Expression = expression;
            Id = $"{nameof(WhereNode)}{expression.Id}";
        }

        public Node Expression { get; }

        public override Type ReturnType => Expression.ReturnType;

        public override string Id { get; }

        public static WhereNode Empty => new WhereNode(new PutTrueNode());

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"where {Expression.ToString()}";
        }
    }
}