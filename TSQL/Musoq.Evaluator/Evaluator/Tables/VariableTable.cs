﻿using Traficante.TSQL.Schema;

namespace Traficante.TSQL.Evaluator.Tables
{
    internal class VariableTable : ITable
    {
        public VariableTable(string schema, string name, IColumn[] columns)
        {
            Schema = schema;
            Name = name;
            Columns = columns;
        }

        public IColumn[] Columns { get; }

        public string Name { get; }

        public string Schema { get; }
    }
}