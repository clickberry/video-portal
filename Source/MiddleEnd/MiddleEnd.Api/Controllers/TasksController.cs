// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Net.Http;
using System.Web.Http;
using MiddleEnd.Api.Attributes;
using MiddleEnd.Api.Commands.Tasks;
using MiddleEnd.Worker.Abstract;

namespace MiddleEnd.Api.Controllers
{
    [Authorize]
    [Authentication]
    [Route("api/tasks/{id?}")]
    public class TasksController : ApiController
    {
        private readonly ITaskProvider _taskKeeper;

        public TasksController(ITaskProvider taskKeeper)
        {
            _taskKeeper = taskKeeper;
        }

        // POST api/tasks
        public HttpResponseMessage Post()
        {
            var command = new TaskPostCommand(this, _taskKeeper);
            return command.Execute();
        }
    }
}