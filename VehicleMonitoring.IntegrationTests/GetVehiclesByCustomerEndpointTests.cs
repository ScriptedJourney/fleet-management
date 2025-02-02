using FastEndpoints.Testing;
using FastEndpoints;
using System.Net;
using Xunit;
using Shouldly;
using VehicleMonitoring.API.Features.Common.ApiModels;
using VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint.Queries;
using VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomer;

namespace VehicleMonitoring.IntegrationTests
{
    public class Tests(App app) : TestBase<App>
    {
        [Fact, Priority(1)]
        public async Task GetVehiclesByCustomer_WhenCustomerAndVehicleExists_ReturnsVehicles()
        {
            var request = new GetVehiclesByCustomerRequest(CustomerId: 1);

            var (rsp, res) = await app.Client.GETAsync<GetVehiclesByCustomerEndpoint, GetVehiclesByCustomerRequest, List<Vehicle>>(request);

            rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
            res.ShouldNotBeNull();
            res.Count.ShouldBe(2);

            var regNumbers = res.Select(v => v.RegNumber).ToList();
            regNumbers.ShouldContain("ABC 123");
            regNumbers.ShouldContain("EYD 983");

            res.First().Customer?.Id.ShouldBe(1);
        }

        [Fact, Priority(2)]
        public async Task GetVehiclesByCustomer_WhenCustomerDoesNotExist_ReturnsNotFound()
        {
            var request = new GetVehiclesByCustomerRequest(CustomerId: 99999);

            var (rsp, _) = await app.Client.GETAsync<GetVehiclesByCustomerEndpoint, GetVehiclesByCustomerRequest, List<Vehicle>>(request);

            rsp.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact, Priority(3)]
        public async Task GetVehiclesByCustomer_WithInvalidCustomerId_ReturnsBadRequest()
        {
            var request = new GetVehiclesByCustomerRequest(CustomerId: -1);

            var (rsp, _) = await app.Client.GETAsync<GetVehiclesByCustomerEndpoint, GetVehiclesByCustomerRequest, List<Vehicle>>(request);

            rsp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
