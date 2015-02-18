// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Infrastructure
{
    public static class TaskExtensions
    {
        public static Task<T> HandleNotFoundException<T>(this Task<T> task)
        {
            return task.ContinueWith(p =>
            {
                if (p.Exception != null)
                {
                    p.Exception.Handle(q => q is NotFoundException);
                }

                if (p.IsCanceled || p.IsFaulted)
                {
                    return default(T);
                }

                return p.Result;
            });
        }

        public static Task HandleNotFoundExceptionNoResult(this Task task)
        {
            return task.ContinueWith(p =>
            {
                if (p.Exception != null)
                {
                    p.Exception.Handle(q => q is NotFoundException);
                }
            });
        }

        public static Task HandleConflictExceptionNoResult(this Task task)
        {
            return task.ContinueWith(p =>
            {
                if (p.Exception != null)
                {
                    p.Exception.Handle(q => q is ConflictException);
                }
            });
        }

        /// <summary>
        /// To prevent VS intellisense warning.
        /// Will be reduced by compilation.
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task)
        {
        }
    }
}