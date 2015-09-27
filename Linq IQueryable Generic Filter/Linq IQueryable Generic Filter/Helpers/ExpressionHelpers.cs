using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linq_IQueryable_Generic_Filter
{
    internal static class ExpressionHelpers
    {
        private class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression _from, _to;

            public SwapVisitor(Expression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == _from ? _to : base.Visit(node);
            }
        }


        public static Expression<Func<T, bool>> CombineOr<T>(this Expression<Func<T, bool>> e1, Expression<Func<T, bool>> e2)
        {
            var lambdaExpression =
                Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(new SwapVisitor(e1.Parameters[0], e2.Parameters[0]).Visit(e1.Body), e2.Body),
                    e2.Parameters);

            return lambdaExpression;
        }

        public static Expression<Func<T, bool>> CombineAnd<T>(this Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2)
        {
            var lambdaExpression =
                Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(new SwapVisitor(exp1.Parameters[0], exp2.Parameters[0]).Visit(exp1.Body),
                        exp2.Body), exp2.Parameters);

            return lambdaExpression;
        }

        public static Expression<Func<T, bool>> PredicateStartsWith<T>(string propName, string key)
        {
            var param = Expression.Parameter(typeof(T), "arg");
            var member = Expression.Property(param, propName);

            var temp = Expression.Call(Expression.Convert(member, typeof(object)),
                typeof(object).GetMethod("ToString"));

            var refmethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var value = Expression.Constant(key, typeof(string));
            var containsMethodExp = Expression.Call(temp, refmethod, value);

            var lambda = Expression.Lambda(typeof(Func<T, bool>), containsMethodExp, param);
            var expression = (Expression<Func<T, bool>>)lambda;

            return expression;
        }

        public static Expression<Func<T, bool>> PredicateEndsWith<T>(string propName, string key)
        {
            var param = Expression.Parameter(typeof (T), "arg");
            var member = Expression.Property(param, propName);

            var temp = Expression.Call(Expression.Convert(member, typeof (object)),
                typeof (object).GetMethod("ToString"));

            var refmethod = typeof (string).GetMethod("EndsWith", new[] {typeof (string)});
            var value = Expression.Constant(key, typeof (string));
            var containsMethodExp = Expression.Call(temp, refmethod, value);

            var lambda = Expression.Lambda(typeof (Func<T, bool>), containsMethodExp, param);
            var expression = (Expression<Func<T, bool>>) lambda;

            return expression;
        }

        public static Expression<Func<T, bool>> PredicateEquals<T, TR>(string propName, TR value)
        {
            return _PredicateEquals<T, bool, TR>(propName, value);
        }

        public static Expression<Func<T, bool>> PredicateContains<T>(string propName, string value)
        {
            return _PredicateContains<T, bool>(propName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <typeparam name="TR">Return Type</typeparam>
        /// <param name="propertyName">Property Name</param>
        /// <param name="key">Search Key</param>
        /// <returns></returns>
        private static Expression<Func<T, TR>> _PredicateContains<T, TR>(string propertyName, string key)
        {
            var param = Expression.Parameter(typeof (T), "arg");
            var member = Expression.Property(param, propertyName);

            var temp = Expression.Call(Expression.Convert(member, typeof (object)),
                typeof (object).GetMethod("ToString"));

            var refmethod = typeof (string).GetMethod("Contains", new[] {typeof (string)});
            var value = Expression.Constant(key, typeof (string));
            var containsMethodExp = Expression.Call(temp, refmethod, value);

            var lambda = Expression.Lambda(typeof (Func<T, TR>), containsMethodExp, param);
            var expression = (Expression<Func<T, TR>>) lambda;

            return expression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <typeparam name="TR">Return Type</typeparam>
        /// <typeparam name="TL">Property Type</typeparam>
        /// <param name="propertyName">Property Name</param>
        /// <param name="key">Search Key</param>
        /// <returns></returns>
        private static Expression<Func<T, TR>> _PredicateEquals<T, TR, TL>(string propertyName, TL key)
        {
            var param = Expression.Parameter(typeof (T), "arg");
            var member = Expression.Property(param, propertyName);

            var refmethod = typeof (TL).GetMethod("Equals", new[] {typeof (TL)});
            var value = Expression.Constant(key, typeof (TL));
            var containsMethodExp = Expression.Call(member, refmethod, value);

            var lambda = Expression.Lambda(typeof (Func<T, TR>), containsMethodExp, param);
            var expression = (Expression<Func<T, TR>>) lambda;
            return expression;
        }
        
        public static Func<T, TR> Access<T, TR>(string fieldName)
        {
            var param = Expression.Parameter(typeof (T), "arg");
            var member = Expression.Property(param, fieldName);
            var lambda = Expression.Lambda(typeof (Func<T, TR>), member, param);
            var compiled = (Func<T, TR>) lambda.Compile();
            return compiled;
        }
    }
}
