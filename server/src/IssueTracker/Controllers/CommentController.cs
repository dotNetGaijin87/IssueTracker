using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
using IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;
using IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;
using IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;
using IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;
using IssueTracker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    public class CommentController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<CommentVm>> CreateComment(CreateCommentCommand cmd)
        {
            CommentVm comment = await Mediator.Send(cmd);
            var routeValues = new { Id = comment.Id };


            return CreatedAtRoute("GetComment", routeValues, comment);
        }

        [HttpGet("{Id}", Name = "GetComment")]
        public async Task<CommentVm> GetComment(string Id)
        {
            CommentVm permission = await Mediator.Send(new GetCommentQuery { Id = Id });


            return permission;
        }

        [HttpPatch]
        public async Task<CommentVm> UpdateComment(UpdateCommentCommand cmd)
        {
            CommentVm comment = await Mediator.Send(cmd);


            return comment;
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(string Id)
        {
            await Mediator.Send(new DeleteCommentCommand { Id = Id });


            return NoContent();
        }

        [HttpGet]
        public async Task<CommentListVm> ListComments([FromQuery] ListCommentsQuery query)
        {
            CommentListVm commentList = await Mediator.Send(query);


            return commentList;
        }


    }
}
