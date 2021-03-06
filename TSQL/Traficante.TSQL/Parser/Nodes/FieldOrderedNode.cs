﻿using Traficante.TSQL.Evaluator.Visitors;

namespace Traficante.TSQL.Parser.Nodes
{
    public class FieldOrderedNode : FieldNode
    {
        public FieldOrderedNode(Node expression, int fieldOrder, string fieldName, Order order)
            : base(expression, fieldOrder, fieldName) => Order = order;

        public Order Order { get; }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
