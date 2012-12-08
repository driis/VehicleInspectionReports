using System;
using System.Linq.Expressions;

namespace MobileVehicleInspection.Api.Library
{
    public static class Guard
    {
        public static void NotNullOrWhitespace(Expression<Func<string>> parameterExpression)
        {
            if (parameterExpression == null)
                throw new GuardUsageException("Parameter must be a MemberExpression (null was passed)");

            string value = parameterExpression.Compile()();
            if (String.IsNullOrWhiteSpace(value))
            {
                string name = GetParameterName(parameterExpression);
                throw new ArgumentException("Cannot be null or empty", name);
            }
        }

        public static void NotNull<T>(Expression<Func<T>> parameterExpression)
            where T : class
        {
            if (parameterExpression == null)
                throw new GuardUsageException("Parameter must be a MemberExpression (null was passed)");

            if (null == parameterExpression.Compile()())
            {
                string name = GetParameterName(parameterExpression);
                throw new ArgumentNullException(name);
            }
        }

        private static string GetParameterName<T>(Expression<Func<T>> parameterExpression)
        {
            dynamic body = parameterExpression.Body;
            return body.Member.Name;
        }

        public class GuardUsageException : Exception 
        {
            public GuardUsageException(string message) : base(message) { }
        }
    }
}
