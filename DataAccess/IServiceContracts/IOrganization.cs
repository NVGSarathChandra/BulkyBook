using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IOrganization:IRepository<Organization>
    {
        void Update(Organization organization);
    }
}
