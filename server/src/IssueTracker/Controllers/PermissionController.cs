using IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;
using IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;
using IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;
using IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;
using IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;
using IssueTracker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    public class PermissionController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<PermissionVm>> CreatePermission(CreatePermissionCommand command)
        {
            PermissionVm permission = await Mediator.Send(command);
            var routeValues = new { userId = permission.UserId, issueId = permission.IssueId };


            return CreatedAtRoute("GetUserIssue", routeValues, permission);
        }

        [HttpGet("{userId}/{issueId}", Name = "GetUserIssue")]
        public async Task<PermissionVm> GetPermission(string userId, string issueId)
        {
            PermissionVm permission = await Mediator.Send(
                new GetPermissionQuery { UserId = userId, IssueId = issueId });


            return permission;
        }

        [HttpPatch]
        public async Task<PermissionVm> UpdatePermission(UpdatePermissionCommand command)
        {
            PermissionVm permission = await Mediator.Send(command);
    

            return permission;
        }

        [HttpDelete("{userId}/{issueId}")]
        public async Task<IActionResult> DeletePermission(string userId, string issueId)
        {
            await Mediator.Send(new DeletePermissionCommand { UserId = userId, IssueId = issueId });
 

            return NoContent();
        }

        [HttpGet]
        public async Task<PermissionsVm> ListPermissions([FromQuery] ListPermissionsQuery query)
        {
            PermissionsVm permissions = await Mediator.Send(query);


            return permissions;
        }
    }
}
