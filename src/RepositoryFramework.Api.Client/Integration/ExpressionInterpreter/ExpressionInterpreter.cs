using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryFramework.Api.Client
{
    internal static class ExpressionInterpreter
    {
        public sealed record ExpressionBearer(Expression Expression)
        {
            public MemberInfo? Member { get; set; }
            public string? Key { get; set; }
        }
        public static string Setup(string predicateAsString, ExpressionBearer bearer)
        {
            IEnumerable<ExpressionBearer> expressions = ReadExpressions(bearer, ref predicateAsString);
            foreach (var exp in expressions)
                predicateAsString = Setup(predicateAsString, exp);
            return predicateAsString;
        }
        private static IEnumerable<ExpressionBearer> ReadExpressions(ExpressionBearer bearer, ref string predicateAsString)
        {
            var expressions = new List<ExpressionBearer>();
            var expression = bearer.Expression;
            if (expression is BinaryExpression binaryExpression)
            {
                expressions.Add(new(binaryExpression.Left));
                expressions.Add(new(binaryExpression.Right));
            }
            else if (expression is BlockExpression blockExpression)
                throw new NotImplementedException($"{nameof(BlockExpression)} not implemented.");
            else if (expression is ConditionalExpression conditionalExpression)
                throw new NotImplementedException($"{nameof(ConditionalExpression)} not implemented.");
            else if (expression is ConstantExpression constantExpression)
            {
                if (bearer.Key != null && bearer.Member != null)
                    Evaluate(ref predicateAsString, bearer.Key,
                        (bearer.Member as FieldInfo)!.GetValue(constantExpression.Value)!);
            }
            else if (expression is DebugInfoExpression debugInfoExpression)
                throw new NotImplementedException($"{nameof(DebugInfoExpression)} not implemented.");
            else if (expression is DefaultExpression defaultExpression)
                throw new NotImplementedException($"{nameof(DefaultExpression)} not implemented.");
            else if (expression is DynamicExpression dynamicExpression)
                throw new NotImplementedException($"{nameof(DynamicExpression)} not implemented.");
            else if (expression is GotoExpression gotoExpression)
                throw new NotImplementedException($"{nameof(GotoExpression)} not implemented.");
            else if (expression is IndexExpression indexExpression)
                throw new NotImplementedException($"{nameof(IndexExpression)} not implemented.");
            else if (expression is InvocationExpression invocationExpression)
                throw new NotImplementedException($"{nameof(InvocationExpression)} not implemented.");
            else if (expression is LabelExpression labelExpression)
                throw new NotImplementedException($"{nameof(LabelExpression)} not implemented.");
            else if (expression is LambdaExpression lambdaExpression)
                expressions.Add(new(lambdaExpression.Body));
            else if (expression is ListInitExpression listInitExpression)
                throw new NotImplementedException($"{nameof(ListInitExpression)} not implemented.");
            else if (expression is LoopExpression loopExpression)
                throw new NotImplementedException($"{nameof(LoopExpression)} not implemented.");
            else if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == null)
                    Compile(memberExpression, ref predicateAsString);
                else
                    expressions.Add(new(memberExpression.Expression)
                    {
                        Key = memberExpression.ToString(),
                        Member = memberExpression.Member,
                    });
            }
            else if (expression is MemberInitExpression memberInitExpression)
                throw new NotImplementedException($"{nameof(MemberInitExpression)} not implemented.");
            else if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Arguments.Count > 0)
                    foreach (var argument in methodCallExpression.Arguments)
                        Compile(argument, ref predicateAsString);
                else
                    Compile(methodCallExpression, ref predicateAsString);
            }
            else if (expression is NewArrayExpression newArrayExpression)
                throw new NotImplementedException($"{nameof(NewArrayExpression)} not implemented.");
            else if (expression is NewExpression newExpression)
                throw new NotImplementedException($"{nameof(NewExpression)} not implemented.");
            else if (expression is ParameterExpression parameterExpression)
            {
                //if (parameterExpression.IsByRef)
                //{
                //}
            }
            else if (expression is RuntimeVariablesExpression runtimeVariablesExpression)
                throw new NotImplementedException($"{nameof(RuntimeVariablesExpression)} not implemented.");
            else if (expression is SwitchExpression switchExpression)
                throw new NotImplementedException($"{nameof(SwitchExpression)} not implemented.");
            else if (expression is TryExpression tryExpression)
                throw new NotImplementedException($"{nameof(TryExpression)} not implemented.");
            else if (expression is TypeBinaryExpression typeBinaryExpression)
                throw new NotImplementedException($"{nameof(TypeBinaryExpression)} not implemented.");
            else if (expression is UnaryExpression unaryExpression)
                throw new NotImplementedException($"{nameof(UnaryExpression)} not implemented.");
            return expressions;

            static void Compile(Expression argument, ref string predicateAsString)
            {
                var argumentKey = argument.ToString();
                try
                {
                    var value = Expression.Lambda(argument).Compile().DynamicInvoke();
                    Evaluate(ref predicateAsString, argumentKey, value!);
                }
                catch { }
            }
            static void Evaluate(ref string predicateAsString, string key, object value)
            {
                predicateAsString = predicateAsString.Replace(key, Interpretate(value!));
            }

            static string Interpretate(object value)
            {
                if (value is null)
                    return "null";
                if (value is string)
                    return $"\"{value}\"";
                else if (value is Guid)
                    return $"Guid.Parse(\"{value}\")";
                else
                    return value.ToString()!;
            }
        }
    }

}
