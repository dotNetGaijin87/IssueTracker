using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
using IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
using IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;
using IssueTracker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    public class ProjectController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<ProjectVm>> CreateProject(CreateProjectCommand command)
        {
            ProjectVm createdProject = await Mediator.Send(command);
            var routeValues = new { Id = createdProject.Id };


            return CreatedAtRoute("GetProject", routeValues, createdProject);
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<ProjectVm> GetProject(string id)
        {
            ProjectVm projectVm = await Mediator.Send(new GetProjectQuery { Id = id });


            return projectVm;
        }

        [HttpPatch]
        public async Task<ProjectVm> UpdateProject(UpdateProjectCommand cmd)
        {
            ProjectVm project = await Mediator.Send(cmd);


            return project;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            await Mediator.Send(new DeleteProjectCommand { Id = id });


            return NoContent();
        }

        [HttpGet]
        public async Task<ProjectListVm> ListProjects([FromQuery] ListProjectsQuery query)
        {
            ProjectListVm projectList = await Mediator.Send(query);


            return projectList;
        }
    }
}
