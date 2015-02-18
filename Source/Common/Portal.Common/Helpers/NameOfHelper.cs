// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;

namespace Portal.Common.Helpers
{
    /// <summary>
    ///     Retrieves type's member names.
    /// </summary>
    public static class NameOfHelper
    {
        public static string PropertyName<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression ??
                                    ((UnaryExpression)expression.Body).Operand as MemberExpression;

            if (body != null)
            {
                return body.Member.Name;
            }

            throw new ArgumentException();
        }

        public static string PropertyName<TProp>(Expression<Func<TProp>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression ??
                                    ((UnaryExpression)expression.Body).Operand as MemberExpression;

            if (body != null)
            {
                return body.Member.Name;
            }

            throw new ArgumentException();
        }

        public static string MethodName<T>(Expression<Action<T>> expression)
        {
            var body = expression.Body as MethodCallExpression;
            if (body != null)
            {
                return body.Method.Name;
            }

            throw new ArgumentException();
        }
    }
}