using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class UserDepartment
    {
        protected UserDepartment() { }

        public AdminUserId AdminUserId { get; private set; } = default!;
        public DepartmentId DepartmentId { get; private set; } = default!;
        public string DepartmentName { get; private set; } = string.Empty;

        public string DepartmentCode { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public UserDepartment(DepartmentId departmentId, string departmentName)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
        }

        public void UpdateDepartmentInfo(string departmentName)
        {
            DepartmentName = departmentName;
        }
    }
}
