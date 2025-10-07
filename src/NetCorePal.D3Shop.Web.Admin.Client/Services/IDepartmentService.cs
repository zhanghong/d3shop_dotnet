using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IDepartmentService
{
    [Post("/api/Department/CreateDepartment")]
    Task<ResponseData<DepartmentId>> CreateDepartment([Body] CreateDepartmentRequest request);

    [Get("/api/Department/GetAllDepartments")]
    Task<ResponseData<List<DepartmentResponse>>> GetAllDepartments([Query] DepartmentQueryRequest request);

    [Put("/api/Department/UpdateDepartmentInfo/{id}")]
    Task<ResponseData> UpdateDepartmentInfo(DepartmentId id, [Body] UpdateDepartmentInfoRequest request);

    [Delete("/api/Department/DeleteDepartment/{id}")]
    Task<ResponseData> DeleteDepartment(DepartmentId id);


}
