using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimizer
{
    public static class DataFrameHelper
    {
        public static DataFrame ToDataFrame<T>(this IEnumerable<T> records)
        {
            var properties = typeof(T).GetProperties();
            var columns = properties.Select(p => CreateColumn(p.Name, records.Select(r => p.GetValue(r)))).ToArray();
            return new DataFrame(columns);
        }

        private static DataFrameColumn CreateColumn(string name, IEnumerable<object> values)
        {
            var firstValue = values.FirstOrDefault();
            if (firstValue is int)
            {
                return new Int32DataFrameColumn(name, values.Cast<int>());
            }
            else if (firstValue is string)
            {
                return new StringDataFrameColumn(name, values.Cast<string>());
            }
            else if (firstValue is double)
            {
                return new DoubleDataFrameColumn(name, values.Cast<double>());
            }
            else if (firstValue is decimal)
            {
                return new DecimalDataFrameColumn(name, values.Cast<decimal>());
            }
            else
            {
                // Add more type checks as needed
                throw new NotSupportedException($"Type {firstValue?.GetType()} is not supported");
            }
        }
    }
}
