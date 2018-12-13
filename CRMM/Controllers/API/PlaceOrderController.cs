using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;

namespace CRMM.Controllers.API
{
    [Route("api/place/{placeId}/order")]
    [ApiController]
    public class PlaceOrderController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public PlaceOrderController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/Order/5/set/nani
        [HttpGet("{id}/set/{state}")]
        public string SetGet(ulong id, ulong placeId, string state)
        {
            return SetState(id, placeId, state);
        }

        private string SetState(ulong id, ulong placeId, string state)
        {
            var order = new Order(_databaseService.Context) { Id = id }.Find().FirstOrDefault();
            order?.SetState(placeId, state, null, order.States.Value.Where(s => s.DeletedOnUtc == null && !s.Type.Equals(OrderStates.Validating) && !s.Type.Equals(OrderStates.DropOf) && !s.Type.Equals(ReclamationStates.Validating) && !s.Type.Equals(ReclamationStates.PickUp)).ToArray());
            return state;
        }

        // POST: api/Order
        [HttpPost("{id}/set")]
        public string SetPost(ulong id, ulong placeId, [FromBody] string state)
        {
            return SetState(id, placeId, state);
        }
    }
}
