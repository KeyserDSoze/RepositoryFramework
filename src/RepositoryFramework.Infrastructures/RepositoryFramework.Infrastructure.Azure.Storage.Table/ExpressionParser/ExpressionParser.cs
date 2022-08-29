using System.Linq.Expressions;
using System.Text.Json;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal static class ExpressionTypeExtensions
    {
        internal static string MakeLogic(this ExpressionType type)
            => type switch
            {
                ExpressionType.And => " and ",
                ExpressionType.Or => " or ",
                ExpressionType.OrElse => " or ",
                ExpressionType.LessThan => " lt ",
                ExpressionType.LessThanOrEqual => " le ",
                ExpressionType.GreaterThan => " gt ",
                ExpressionType.GreaterThanOrEqual => " ge ",
                ExpressionType.Equal => " eq ",
                ExpressionType.NotEqual => " ne ",
                _ => " and ",
            };
        internal static bool IsRightASingleValue(this ExpressionType type)
            => type switch
            {
                ExpressionType.AndAlso or ExpressionType.And or ExpressionType.Or or ExpressionType.OrElse => false,
                _ => true,
            };
    }
    internal sealed class QueryStrategy
    {
        internal static string Create(Expression expression, string partitionKey, string rowKey, string? timestamp)
        {
            IExpressionStrategy expressionFactory = new BinaryExpressionStrategy(partitionKey, rowKey, timestamp);
            if (expression is MethodCallExpression)
            {
                expressionFactory = new MethodCallerExpressionStrategy(partitionKey, rowKey, timestamp);
            }
            else if (expression is UnaryExpression)
            {
                expressionFactory = new UnaryExpressionStrategy();
            }
            else if (expression.GetType().Name == "PropertyExpression")
            {
                expressionFactory = new MemberExpressionStrategy();
            }
            return expressionFactory.Convert(expression);
        }
        internal static string ValueToString(object value)
        {
            if (value is string)
                return $"'{value}'";
            if (value is DateTime time)
                return $"datetime'{time:yyyy-MM-dd}T{time:HH:mm:ss}Z'";
            if (value is DateTimeOffset offset)
                return $"datetime'{offset:yyyy-MM-dd}T{offset:HH:mm:ss}Z'";
            if (value is Guid)
                return $"guid'{value}'";
            if (value is double @double)
                return @double.ToString(new System.Globalization.CultureInfo("en"));
            if (value is float single)
                return single.ToString(new System.Globalization.CultureInfo("en"));
            if (value is decimal @decimal)
                return @decimal.ToString(new System.Globalization.CultureInfo("en"));
            return $"'{value.ToJson()}'";
        }
    }
    internal interface IExpressionStrategy
    {
        string Convert(Expression expression);
        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";
        public const string Timestamp = "Timestamp";
    }
    internal sealed class BinaryExpressionStrategy : IExpressionStrategy
    {
        private readonly string PartitionKey;
        private readonly string RowKey;
        private readonly string Timestamp;
        public BinaryExpressionStrategy(string partitionKey, string rowKey, string timestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
        }
        public string Convert(Expression expression)
        {
            BinaryExpression binaryExpression = (BinaryExpression)expression;
            if (binaryExpression.NodeType.IsRightASingleValue())
            {
                dynamic leftPart = binaryExpression.Left;
                string name = leftPart.Member.Name;
                if (name == PartitionKey)
                    name = IExpressionStrategy.PartitionKey;
                else if (name == RowKey)
                    name = IExpressionStrategy.RowKey;
                else if (name == Timestamp)
                    name = IExpressionStrategy.Timestamp;
                object rightPart = Expression.Lambda(binaryExpression.Right).Compile().DynamicInvoke();
                return name + binaryExpression.NodeType.MakeLogic() + QueryStrategy.ValueToString(rightPart);
            }
            return null;
        }
    }
    internal sealed class UnaryExpressionStrategy : IExpressionStrategy
    {
        public string Convert(Expression expression)
        {
            UnaryExpression unaryExpression = (UnaryExpression)expression;
            dynamic operand = unaryExpression.Operand;
            if (operand.Member.PropertyType == typeof(bool))
                return $"{operand.Member.Name} eq false";
            return null;
        }
    }
    internal sealed class MethodCallerExpressionStrategy : IExpressionStrategy
    {
        private readonly string PartitionKey;
        private readonly string RowKey;
        private readonly string Timestamp;
        public MethodCallerExpressionStrategy(string partitionKey, string rowKey, string timestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
        }
        public string Convert(Expression expression)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
            if (methodCallExpression.NodeType.IsRightASingleValue())
            {
                dynamic argument = methodCallExpression.Arguments[0];
                string name = argument.Member.Name;
                if (name == PartitionKey)
                    name = IExpressionStrategy.PartitionKey;
                else if (name == RowKey)
                    name = IExpressionStrategy.RowKey;
                else if (name == Timestamp)
                    name = IExpressionStrategy.Timestamp;
                object value = Expression.Lambda(methodCallExpression.Arguments[1]).Compile().DynamicInvoke();
                return name + ((ExpressionType)Enum.Parse(typeof(ExpressionType), methodCallExpression.Method.Name)).MakeLogic() + QueryStrategy.ValueToString(value);
            }
            return null;
        }
    }
    internal sealed class MemberExpressionStrategy : IExpressionStrategy
    {
        public string Convert(Expression expression)
        {
            MemberExpression memberExpression = (MemberExpression)expression;
            dynamic property = memberExpression.Member;
            if (property.PropertyType == typeof(bool))
            {
                //qui ci entra solo per i booleani, altrimenti la query è sbagliata
                string name = property.Name;
                return $"{name} eq true";
            }
            return null;
        }
    }
}
