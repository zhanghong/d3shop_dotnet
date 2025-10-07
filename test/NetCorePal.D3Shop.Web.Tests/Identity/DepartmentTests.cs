using NetCorePal.D3Shop.Admin.Shared.Dtos.Identity;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.Extensions.Dto;
using System.Net.Http.Headers;

namespace NetCorePal.D3Shop.Web.Tests.Identity
{
    [Collection("web")]
    public class DepartmentTests
    {
        private readonly HttpClient _client;

        public DepartmentTests(MyWebApplicationFactory factory)
        {
            _client = factory.WithWebHostBuilder(builder => { builder.ConfigureServices(_ => { }); })
           .CreateClient();
            const string json = $$"""
                              {
                                   "name": "{{AppDefaultCredentials.Name}}",
                                   "password": "{{AppDefaultCredentials.Password}}"
                              }
                              """;
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _client.PostAsync("api/AdminUserAccount/login", content).GetAwaiter().GetResult();
        }

        #region CreateDepartment Tests

        [Fact]
        public async Task CreateDepartment_ShouldReturnDepartmentId()
        {
            // Arrange
            var request = new CreateDepartmentRequest
            {
                Name = "TestDepartment",
                Remark = "test decription",
                Pid = new DepartmentId(0)


            };

            // Act
            var response = await _client.PostAsNewtonsoftJsonAsync("/api/Department/CreateDepartment", request);

            // Assert - 创建成功
            Assert.True(response.IsSuccessStatusCode);
            var responseData = await response.Content.ReadFromNewtonsoftJsonAsync<ResponseData<DepartmentId>>();
            Assert.NotNull(responseData);
            Assert.NotEqual(0, responseData.Data.Id); // 验证返回的用户ID不是默认值
        }

        #endregion

        #region GetAllDepartments Tests

        [Fact]
        public async Task GetAllDepartments_ShouldReturnPagedData()
        {
            const string testDepartmentName = "TestDepartment";
            // Arrange
            var request = new DepartmentQueryRequest
            {
                Name = testDepartmentName,
            };

            var url = "/api/Department/GetAllDepartments?Name=" + testDepartmentName;// + queryString;

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseData, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region UpdateDepartmentInfo Tests

        [Fact]
        public async Task UpdateDepartmentInfo_ShouldReturnSuccess()
        {
            // Arrange
            var departmentId = 1; // 假设部门 ID 为 1
            var request = new UpdateDepartmentInfoRequest
            {
                Name = "Updated Department",
                Remark = "Updated Description"
            };

            // Act
            var response = await _client.PutAsNewtonsoftJsonAsync($"/api/Department/UpdateDepartmentInfo/{departmentId}", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseData, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region DeleteDepartment Tests

        [Fact]
        public async Task DeleteDepartment_ShouldReturnSuccess()
        {
            // Arrange
            var departmentId = 1; // 假设部门 ID 为 1

            // Act
            var response = await _client.DeleteAsync($"/api/Department/DeleteDepartment/{departmentId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseData, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

    }



}
