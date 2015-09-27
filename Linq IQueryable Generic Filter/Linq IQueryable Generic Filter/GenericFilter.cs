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
        public static IQueryable<T> Filter<T>(Filter filter, IQueryable<T> list)
        {
            var predicationExpressionList = new List<Expression<Func<T, bool>>>();

            

            foreach (var args in filter.ConstraintList)
            {
                var constraintsValues = args.Constraints;
                var propName = args.Key;
                var type = args.Type;


                #region "Equals"

                if (constraintsValues.Equals != null)
                {
                    var newExp = (Expression<Func<T, bool>>) typeof (ExpressionHelpers)
                        .GetMethod("PredicateEquals")
                        .MakeGenericMethod(typeof (T), Type.GetType("System." + type))
                        .Invoke(null, new[] {propName, Convert.ChangeType(constraintsValues.Equals, type)});

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
                
                
            }

            Expression<Func<T, bool>> predicationDelegateExpression = filter.Or ?? false
                ? predicationExpressionList.Aggregate(ExpressionHelpers.CombineOr)
                : predicationExpressionList.Aggregate(ExpressionHelpers.CombineAnd);

            var resultList = predicationDelegateExpression == null ? list : list.Where(predicationDelegateExpression);


            if (filter.Skip != null)
                resultList = resultList.Skip(filter.Skip.Value);
            if (filter.Take != null)
                resultList = resultList.Take(filter.Take.Value);

            return resultList;
        }
        
    }
}

