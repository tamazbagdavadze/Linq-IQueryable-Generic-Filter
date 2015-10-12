﻿using System;
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
                    var parameterType = constraintsValues.Equals.GetType();

                    var newExp = (Expression<Func<T, bool>>) typeof (ExpressionHelpers)
                        .GetMethod("PredicateEquals")
                        .MakeGenericMethod(typeof (T), parameterType)
                        .Invoke(null, new[] {propName, Convert.ChangeType(constraintsValues.Equals, Type.GetTypeCode(parameterType))});

                    predicationExpressionList.Add(newExp);
                }

                #endregion

                #region "string"

                if (constraintsValues.ContainsString != null)
                {
                    var newExp = ExpressionHelpers.PredicateContains<T>(propName, constraintsValues.ContainsString);
                    predicationExpressionList.Add(newExp);
                }
                
                if (constraintsValues.StartsWith != null)
                {
                    var newExp = ExpressionHelpers.PredicateStartsWith<T>(propName, constraintsValues.StartsWith);
                    predicationExpressionList.Add(newExp);
                }

                if (constraintsValues.EndsWith != null)
                {
                    var newExp = ExpressionHelpers.PredicateEndsWith<T>(propName, constraintsValues.EndsWith);
                    predicationExpressionList.Add(newExp);
                }

                #endregion

                #region "Less More"

                if (constraintsValues.LessThen != null)
                {
                    var parameterType = constraintsValues.LessThen.GetType();

                    var newExp = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
                       .GetMethod("PredicateLess")
                       .MakeGenericMethod(typeof(T), parameterType)
                       .Invoke(null, new[] { propName, Convert.ChangeType(constraintsValues.LessThen, Type.GetTypeCode(parameterType)) });
                    
                    predicationExpressionList.Add(newExp);
                }

                if (constraintsValues.MoreThen != null)
                {
                    var parameterType = constraintsValues.MoreThen.GetType();

                    var newExp = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
                       .GetMethod("PredicateMore")
                       .MakeGenericMethod(typeof(T), parameterType)
                       .Invoke(null, new[] { propName, Convert.ChangeType(constraintsValues.MoreThen, Type.GetTypeCode(parameterType)) });

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

        /// <summary>
        /// Test...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertyName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IQueryable<T> FilterContains<T>(this IQueryable<T> list, string propertyName, object key)
        {
            var expression =  ExpressionHelpers.PredicateContains<T>(propertyName, key.ToString());
            return list.Where(expression);

//            return list.Filter(new Filter
//            {
//                ConstraintList = new List<Pair>
//                {
//                    new Pair
//                    {
//                        propertyName,
//                        new FilterConstraint
//                        {
//                            ContainsString = key.ToString()
//                        }
//                    }
//                }
//            });
        }

        public static IQueryable<T> FilterEquals<T>(this IQueryable<T> list, string propertyName, object value)
        {
            var parameterType = value.GetType();

            var expression = (Expression<Func<T, bool>>)typeof(ExpressionHelpers)
                .GetMethod("PredicateEquals")
                .MakeGenericMethod(typeof(T), parameterType)
                .Invoke(null, new[] { propertyName, Convert.ChangeType(value, Type.GetTypeCode(parameterType)) });

            return list.Where(expression);
        }
    }
}