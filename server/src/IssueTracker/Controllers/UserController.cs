    using IssueTracker.Application.CommandQueries.Users.Commands.CreateUserSafelyCommand;
using IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;
using IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;
using IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;
using IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;
using IssueTracker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers;

public class UserController : ApiController
{
    [HttpGet("{id}", Name = "GetUser")]
    public async Task<UserVm> GetUser(string id)
    {
        UserVm user = await Mediator.Send(new GetUserQuery { Id = id});


        return user;
    }

    [HttpPatch]
    public async Task<UserVm> UpdateUser(UpdateUserCommand cmd)
    {
        UserVm user = await Mediator.Send(cmd);


        return user;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        await Mediator.Send(new DeleteUserCommand { Id = id });

        return NoContent();
    }

    [HttpGet]
    public async Task<UserListVm> ListUsers([FromQuery] ListUsersQuery query)
    {
        UserListVm userList = await Mediator.Send(query);


        return userList;
    }


    //
    // Custom methods
    //

    [HttpPost(":createUserSafely")]
    public async Task<ActionResult<UserVm>> CreateUserSafely(CreateUserSafelyCommand command)
    {
        UserVm user = await Mediator.Send(command);
        var routeValues = new { id = user.Id };


        return CreatedAtRoute("GetUser", routeValues, user);
    }
}
