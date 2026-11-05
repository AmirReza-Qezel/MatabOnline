using System.Security.Claims;

namespace AuthPracticing.Repositories
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim(ClaimTypeStore.EmployeeDetail,true.ToString()),
            new Claim(ClaimTypeStore.EmployeeList,true.ToString()),
            new Claim(ClaimTypeStore.EmployeeEdit,true.ToString()),
            new Claim(ClaimTypeStore.AddEmployee,true.ToString())
        };
    }
    public static class ClaimTypeStore
    {
        public const string EmployeeList = "EmployeeList";
        public const string EmployeeDetail = "EmployeeDetail";
        public const string EmployeeEdit = "EmployeeEdit";
        public const string AddEmployee = "AddEmployee";
    }
}
