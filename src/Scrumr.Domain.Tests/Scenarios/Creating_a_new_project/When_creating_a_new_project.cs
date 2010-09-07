﻿using System;
using System.Linq;
using FluentAssertions;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Commanding.CommandExecution.Mapping.Fluent;
using Ncqrs.Spec;
using Scrumr.Commands;
using Scrumr.Events.Project;

namespace Scrumr.Domain.Tests.Scenarios.Creating_a_new_project
{
    [Specification]
    public class When_creating_a_new_project : CommandTestFixture<CreateNewProject>
    {
        protected override ICommandExecutor<CreateNewProject> BuildCommandExecutor()
        {
            return Map.
                Command<CreateNewProject>().
                ToAggregateRoot<Project>().
                CreateNew(c => new Project(c.ProjectId, c.Name));
        }

        protected override CreateNewProject WhenExecutingCommand()
        {
            return new CreateNewProject
            {
                ProjectId = Guid.NewGuid(),
                Name = "My Project"
            };
        }

        [Then]
        public void Then_published_events_count_should_be_one()
        {
            PublishedEvents.Should().HaveCount(1);
        }

        [And]
        public void And_should_be_of_type_NewProjectCreated()
        {
            PublishedEvents.First().Should().BeOfType<NewProjectCreated>();
        }

        [And]
        public void And_the_event_should_contain_the_correct_details()
        {
            var theEvent = PublishedEvents.First().As<NewProjectCreated>();

            theEvent.ProjectId.Should().Be(ExecutedCommand.ProjectId);
            theEvent.Name.Should().Be(ExecutedCommand.Name);
        }
    }
}