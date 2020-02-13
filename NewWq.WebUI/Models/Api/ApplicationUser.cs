using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace NewWq.WebUI.Models.Api
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(string name, Guid id)
        {
            Name = name;
            Id = id;
        }
        public string Name { get; }
        public Guid Id { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }
    }

    public class ApplicationUser : IPrincipal
    {
        public ApplicationUser(string name, Guid id)
        {
            Identity = new UserIdentity(name, id);
        }
        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}