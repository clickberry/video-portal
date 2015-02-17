// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Asp.Infrastructure.Commands
{
    /// <summary>
    ///     Command handler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ControllerCommandApi<T>
    {
        private readonly ApiController _controller;

        protected ControllerCommandApi(ApiController controller)
        {
            _controller = controller;
        }

        /// <summary>
        ///     Gets a current user identity.
        /// </summary>
        protected string UserId
        {
            get { return Controller.User.Identity.Name; }
        }

        /// <summary>
        ///     Gets a controller.
        /// </summary>
        protected ApiController Controller
        {
            get { return _controller; }
        }

        /// <summary>
        ///     Gets a current application name.
        /// </summary>
        protected string AppName
        {
            get { return _controller.Request.RequestUri.Host; }
        }

        /// <summary>
        ///     Gets a model validation errors.
        /// </summary>
        protected IDictionary<string, string> ModelErrors
        {
            get
            {
                return Controller.ModelState.Where(kv => kv.Value.Errors.Count != 0)
                    .ToDictionary(kv => kv.Key.Split('.').Last(), kv => kv.Value.Errors.First().ErrorMessage);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether user is in group.
        /// </summary>
        /// <param name="group">group identifier.</param>
        /// <returns></returns>
        protected bool IsInGroup(string group)
        {
            return _controller.User.IsInRole(group);
        }

        /// <summary>
        ///     Executes command.
        /// </summary>
        /// <returns>Result.</returns>
        public abstract T Execute();
    }
}