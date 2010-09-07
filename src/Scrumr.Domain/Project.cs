using System;
using System.Collections.Generic;
using Scrumr.Events.Project;

namespace Scrumr.Domain
{
    public class Project : ScrumrAggregateRoot
    {
        private string _name;
        private List<Guid> _members = new List<Guid>();

        private ProductBacklog _productBacklog;

        public ProductBacklog ProductBacklog
        {
            get { return _productBacklog; }
        }

        protected Project()
        {
        }

        public Project(Guid id, string name) : base(id)
        {
            ValidateName(name);

            var productBacklogId = Guid.NewGuid(); // TODO: maybe we need to add an GenerateEntityId method to the agg root class that uses the generator from the environment.

            var e = new NewProjectCreated(id, productBacklogId, name);
            ApplyEvent(e);
        }

        protected void ValidateName(string name)
        {
            const int NameMaxLenght = 50;

            if(string.IsNullOrEmpty(name))
            {
                throw new DomainException("The name of a project cannot be empty.");
            }
            if(name.Length > NameMaxLenght)
            {
                throw new DomainException("The name of a project cannot be longer then "+NameMaxLenght+".");
            }
        }

        public void Rename(string newName)
        {
            ValidateName(newName);

            var e = new ProjectRenamed(newName);
            ApplyEvent(e);
        }

        public void AddMember(Guid memberId)
        {
            if(_members.Contains(memberId)) throw new DomainException(string.Format("Already contains member with id {0}.", memberId));

            var e = new MemberAddedToProject(memberId);
            ApplyEvent(e);
        }

        public void RemoveMember(Guid memberId)
        {
            var e = new MemberRemovedFromProject(memberId);
            ApplyEvent(e);
        }

        private void OnProjectCreated(NewProjectCreated e)
        {
            _productBacklog = new ProductBacklog(this, e.ProjectId, "Product backlog");
            _name = e.Name;
        }

        private void OnProjectRenamed(ProjectRenamed e)
        {
            _name = e.NewName;
        }

        private void OnMemberAdded(MemberAddedToProject e)
        {
            _members.Add(e.MemberId);
        }

        private void OnMemberRemoved(MemberRemovedFromProject e)
        {
            _members.Remove(e.MemberId);
        }
    }
}
