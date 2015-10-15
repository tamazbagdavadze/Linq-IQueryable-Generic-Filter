using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Linq_IQueryable_Generic_Filter
{
    public static class GenericFilter
    {
        /// <summary>
        /// Filters generic IQuerable list by user defined contraints and properties
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List to filter</param>
        /// <param name="filter">Filter object</param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> list, Filter filter)
        {
            var predicationExpressionList = new List<Expression<Func<T, bool>>>();

            foreach (var args in filter.ConstraintList)
            {
                var constraintsValues = args.Constraints;
                var propName = args.Key;

                #region "Equals"

                if (constraintsValues.Equals != null)
                {
                    var newExp = list._FilterEquals(propName, constraintsValues.Equals);
                    predicationExpressionList.Add(newExp);
                }

                #endregion

                #region "string"

                if (constraintsValues.ContainsString != null)
                {
                    var newExp = list._FilterContainsString(propName, constraintsValues.ContainsString);
                    predicationExpressionList.Add(newExp);
                }

                if (constraintsValues.StartsWith != null)
                {
                    var newExp = list._FilterStartsWith(propName, constraintsValues.StartsWith);
                    predicationExpressionList.Add(newExp);
                }

                if (constraintsValues.EndsWith != null)
                {
                    var newExp = list._FilterEndWith(propName, constraintsValues.EndsWith);
                    predicationExpressionList.Add(newExp);
                }

                #endregion

                #region "Less More"

                if (constraintsValues.LessThan != null)
                {
                    var newExp = list._FilterLessThan(propName, constraintsValues.LessThan);
                    predicationExpressionList.Add(newExp);
                }

                if (constraintsValues.MoreThan != null)
                {
                    var newExp = list._FilterMoreThan(propName, constraintsValues.MoreThan);
                    predicationExpressionList.Add(newExp);
                }

                #endregion
            }
            
            Expression<Func<T, bool>> predicationDelegateExpression = filter.Or ?? false
                ? predicationExpressionList.Aggregate(ExpressionHelpers.CombineOr)
                : predicationExpressionList.Aggregate(ExpressionHelpers.CombineAnd);

            list = predicationDelegateExpression == null ? list : list.Where(predicationDelegateExpression);
            
            if (filter.Skip != null)
                list = list.Skip(filter.Skip.Value);
            if (filter.Take != null)
                list = list.Take(filter.Take.Value);

            return list;
        }
        
        
        public static IQueryable<T> FilterEquals<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var expression = _FilterEquals(list, propertyName, value);
            return list.Where(expression);
        }

        public static IQueryable<T> FilterLessThan<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var expression = _FilterLessThan(list, propertyName, value);
            return list.Where(expression);
        }

        public static IQueryable<T> FilterMoreThan<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var expression = _FilterMoreThan(list, propertyName, value);
            return list.Where(expression);
        }

        public static IQueryable<T> FilterContainsString<T>(this IQueryable<T> list, string propertyName, object key)
        {
            var expression = _FilterContainsString(list, propertyName, key);
            return list.Where(expression);
        }

        public static IQueryable<T> FilterEndWith<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var expression = _FilterEndWith(list, propertyName, value);
            return list.Where(expression);
        }

        public static IQueryable<T> FilterStartsWith<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var expression = _FilterStartsWith(list, propertyName, value);
            return list.Where(expression);
        }


        private static Expression<Func<T, bool>> _FilterEquals<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var parameterType = value.GetType();
            var typeCode = Type.GetTypeCode(parameterType);

            var expression = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
                .GetMethod("PredicateEquals")
                .MakeGenericMethod(typeof(T), parameterType)
                .Invoke(null, new[] { propertyName, Convert.ChangeType(value, typeCode) });

            return expression;
        }

        private static Expression<Func<T, bool>> _FilterLessThan<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var parameterType = value.GetType();
            var typeCode = Type.GetTypeCode(parameterType);

            var expression = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
               .GetMethod("PredicateLess")
               .MakeGenericMethod(typeof(T), parameterType)
               .Invoke(null, new[] { propertyName, Convert.ChangeType(value, typeCode) });

            return expression;
        }

        private static Expression<Func<T, bool>> _FilterMoreThan<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var parameterType = value.GetType();
            var typeCode = Type.GetTypeCode(parameterType);

            var expression = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
               .GetMethod("PredicateMore")
               .MakeGenericMethod(typeof(T), parameterType)
               .Invoke(null, new[] { propertyName, Convert.ChangeType(value, typeCode) });

            return expression;
        }

        private static Expression<Func<T, bool>> _FilterContainsString<T>(this IQueryable<T> list, string propertyName, object key)
        {
            return ExpressionHelpers.PredicateContains<T>(propertyName, key.ToString());
        }

        private static Expression<Func<T, bool>> _FilterEndWith<T>(this IQueryable<T> list, string propertyName, object value)
        {
            return ExpressionHelpers.PredicateStartsWith<T>(propertyName, value.ToString());
        }

        private static Expression<Func<T, bool>> _FilterStartsWith<T>(this IQueryable<T> list, string propertyName, object value)
        {
            return ExpressionHelpers.PredicateStartsWith<T>(propertyName, value.ToString());
        }
    }
}