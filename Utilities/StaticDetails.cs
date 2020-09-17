using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class StaticDetails
    {
        //Stored Procedures
        public const string SP_GETCOVERTYPE     = "SP_GETCOVERTYPE";
        public const string SP_INSERTCOVERTYPE  = "SP_INSERTCOVERTYPE";
        public const string SP_UPDATECOVERTYPE  = "SP_UPDATECOVERTYPE";
        public const string SP_DELETECOVERTYPE  = "SP_DELETECOVERTYPE";
        public const string SP_GETALLCOVERTYPES = "SP_GETALLCOVERTYPES";

        //Roles
        public const string IndividualCustomerRole = "Individual Customer";
        public const string OrganizationCustomerRole = "Organization Customer";
        public const string AdminRole = "Admin";
        public const string EmployeeRole = "Employee";

    }
}

