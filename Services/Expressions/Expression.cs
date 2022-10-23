using System;
using System.Linq.Expressions;

namespace SystemServiceAPICore3.Services.Expressions
{
    internal static class Expression<TFrom, TTo>
        where TFrom : class
        where TTo : class
    {
        public static Expression<Func<TTo, T>> Tranform<T>(Expression<Func<TFrom, T>> expression)
        {
            var param = Expression.Parameter(typeof(TTo));
            var body = new CustomVisitor<TTo>(param).Visit(expression.Body);

            var lambda = Expression.Lambda<Func<TTo, T>>(body, param);

            return lambda;
        }

        private class CustomVisitor<T> : ExpressionVisitor
        {
            #region -- Variables --

            private readonly ParameterExpression parameter;
            private int counter = 0;

            #endregion

            #region -- Properties --
            #endregion

            #region -- Constructors --

            public CustomVisitor(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }

            #endregion

            #region -- Overrides --

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameter;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                counter++;
                //try
                //{
                //only properties are allowed if you use fields then you need to extend
                // this method to handle them
                if (node.Member.MemberType != System.Reflection.MemberTypes.Property)
                {
                    return node;
                    //throw new NotImplementedException();
                }

                //name of a member referenced in original expression in your
                //sample Id in mine Prop
                var memberName = node.Member.Name;

                //find property on type T (=PersonData) by name
                var otherMember = typeof(T).GetProperty(memberName);
                if (otherMember == null)
                {
                    throw new Exception("Don't exist property " + memberName);
                }


                //visit left side of this expression p.Id this would be p
                var inner = Visit(node.Expression);

                return Expression.Property(inner, otherMember);
                //}
                //catch (Exception ex)
                //{
                //    throw;
                //}
            }

            #endregion

            #region -- Methods --
            #endregion
        }
    }
}