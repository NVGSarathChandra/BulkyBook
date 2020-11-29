using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class StaticDetails
    {
        //Stored Procedures
        public const string SP_GETCOVERTYPE = "SP_GETCOVERTYPE";
        public const string SP_INSERTCOVERTYPE = "SP_INSERTCOVERTYPE";
        public const string SP_UPDATECOVERTYPE = "SP_UPDATECOVERTYPE";
        public const string SP_DELETECOVERTYPE = "SP_DELETECOVERTYPE";
        public const string SP_GETALLCOVERTYPES = "SP_GETALLCOVERTYPES";

        //Roles
        public const string IndividualCustomerRole = "Individual Customer";
        public const string OrganizationCustomerRole = "Organization Customer";
        public const string AdminRole = "Admin";
        public const string EmployeeRole = "Employee";


        public const string SessionShoppingCart = "Shopping Cart Session";

        public static string ConvertToRawHtml(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                char[] array = new char[html.Length];
                int arrayIndex = 0;
                bool inside = false;

                for (int i = 0; i < html.Length; i++)
                {

                    if (html[i] == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (html[i] == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = html[i];
                        arrayIndex++;
                    }
                }
            
            return new string(array, 0, arrayIndex);
            }
            return "";
        }

        public static double CalculateBooksPrice(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
            {
                return price;
            }
            else
            {
                if (quantity < 100)
                {
                    return price50;
                }
                else
                {
                    return price100;
                }
            }
        }

    }
}

