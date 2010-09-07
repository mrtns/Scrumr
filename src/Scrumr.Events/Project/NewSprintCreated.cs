﻿using System;
using Ncqrs.Eventing.Sourcing;

namespace Scrumr.Events.Project
{
    public class SprintAddedToProject : ProjectEvent
    {
        public Guid ProjectId { get { return EventSourceId; } }
        public Guid SprintId { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public SprintAddedToProject()
        { }

        public SprintAddedToProject(string name, DateTime from, DateTime to)
        {
            Name = name;
            From = from;
            To = to;
        }
    }
}
