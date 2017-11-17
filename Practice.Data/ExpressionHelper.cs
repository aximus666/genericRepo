using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Data
{
    public static class ExpressionHelper
    {

        public static Expression<Func<TTo, TResultTo>> TypeConvert<TFrom, TTo, TResultFrom, TResultTo>(
            this Expression<Func<TFrom, TResultFrom>> from)
        {
            if (from == null) return null;

            return ConvertImpl<Func<TFrom, TResultFrom>, Func<TTo, TResultTo>>(from);
        }

        private static Expression<TTo> ConvertImpl<TFrom, TTo>(Expression<TFrom> from)
            where TFrom : class
            where TTo : class
        {
            // figure out which types are different in the function-signature

            var fromTypes = from.Type.GetGenericArguments();
            var toTypes = typeof(TTo).GetGenericArguments();

            if (fromTypes.Length != toTypes.Length)
                throw new NotSupportedException("Incompatible lambda function-type signatures");

            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            for (int i = 0; i < fromTypes.Length; i++)
            {
                if (fromTypes[i] != toTypes[i])
                    typeMap[fromTypes[i]] = toTypes[i];
            }

            // re-map all parameters that involve different types
            Dictionary<Expression, Expression> parameterMap = new Dictionary<Expression, Expression>();
            ParameterExpression[] newParams = GenerateParameterMap<TFrom>(from, typeMap, parameterMap);

            // rebuild the lambda
            var body = new TypeConversionVisitor<TTo, TFrom>(parameterMap).Visit(from.Body);
            try
            {
                return Expression.Lambda<TTo>(body, newParams);
            }
            catch
            {
                Expression conversion = Expression.Convert(body, typeof(object));
                return Expression.Lambda<TTo>(conversion, newParams);
            }
        }

        private static ParameterExpression[] GenerateParameterMap<TFrom>(
            Expression<TFrom> from,
            Dictionary<Type, Type> typeMap,
            Dictionary<Expression, Expression> parameterMap
        )
            where TFrom : class
        {
            var newParams = new ParameterExpression[from.Parameters.Count];

            for (int i = 0; i < newParams.Length; i++)
            {
                Type newType;
                if (typeMap.TryGetValue(from.Parameters[i].Type, out newType))
                {
                    parameterMap[from.Parameters[i]] = newParams[i] = Expression.Parameter(newType, from.Parameters[i].Name);
                }
            }
            return newParams;
        }


        class TypeConversionVisitor<T, TFrom> : ExpressionVisitor
        {
            private readonly Dictionary<Expression, Expression> parameterMap;

            public TypeConversionVisitor(Dictionary<Expression, Expression> parameterMap)
            {
                this.parameterMap = parameterMap;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // re-map the parameter
                Expression found;
                if (!parameterMap.TryGetValue(node, out found))
                    found = base.VisitParameter(node);
                return found;
            }
            public override Expression Visit(Expression node)
            {
                LambdaExpression lambda = node as LambdaExpression;
                if (lambda != null && !parameterMap.ContainsKey(lambda.Parameters.First()))
                {
                    return new TypeConversionVisitor<T, TFrom>(parameterMap).Visit(lambda.Body);
                }
                return base.Visit(node);
            }

            //protected override Expression VisitMember(MemberExpression node)
            //{
            //    // re-perform any member-binding
            //    var expr = Visit(node.Expression);
            //    if (expr != null && expr.Type != node.Type)
            //    {
            //        if (expr.Type.GetMember(node.Member.Name).Any())
            //        {
            //            MemberInfo newMember = expr.Type.GetMember(node.Member.Name).Single();
            //            return Expression.MakeMemberAccess(expr, newMember);
            //        }
            //        else
            //        {
            //            Type[] generics = typeof(TFrom).GetGenericArguments();
            //            foreach (var prop in generics[0].GetProperties())
            //            {
            //                var attr = prop.GetCustomAttribute(typeof(ColumnNameAttribute), true) as ColumnNameAttribute;

            //                if (attr != null)
            //                {
            //                    if (node.Member.Name == prop.Name)
            //                    {
            //                        MemberInfo newMember = expr.Type.GetMember(attr.ColumnName).Single();
            //                        return Expression.MakeMemberAccess(expr, newMember);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    return base.VisitMember(node);
            //}
        }
    }
}
