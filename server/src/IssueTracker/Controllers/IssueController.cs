using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;
using IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;
using IssueTracker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    public class IssueController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<IssueVm>> CreateIssue(CreateIssueCommand command)
        {
            IssueVm issue = await Mediator.Send(command);
            var routeValues = new { id = issue.Id };


            return CreatedAtRoute("GetIssue", routeValues, issue);
        }

        [HttpGet("{id}", Name = "GetIssue")]
        public async Task<IssueVm> GetIssue(string id)
        {
            IssueVm issue = await Mediator.Send(new GetIssueQuery { Id = id });


            return issue;
        }

        [HttpPatch]
        public async Task<IssueVm> UpdateIssue(UpdateIssueCommand cmd)
        {
            IssueVm issue = await Mediator.Send(cmd);


            return issue;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(string id)
        {
            await Mediator.Send(new DeleteIssueCommand { Id = id });


            return NoContent();
        }

        [HttpGet]
        public async Task<IssueListVm> ListIssues([FromQuery] ListIssuesQuery query)
        {
            IssueListVm issueList = await Mediator.Send(query);


            return issueList;
        }

        //
        // Custom methods
        //

        [HttpPost(":GetIssueKanban")]
        public async Task<IEnumerable<KanbanCardVm>> GetIssueKanban(GetIssueKanbanQuery cmd)
        {
            IEnumerable<KanbanCardVm> kanbanCards = await Mediator.Send(cmd);


            return kanbanCards;
        }


        [HttpPost(":UpdateIssueKanban")]
        public async Task<IEnumerable<KanbanCardVm>> UpdateIssueKanban(UpdateIssueKanbanCommand cmd)
        {
            IEnumerable<KanbanCardVm> kanbanCards = await Mediator.Send(cmd);


            return kanbanCards;
        }


    }
}
