using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;
using Services.WorkContext;

namespace CRMM.Controllers.API
{
    [Route("api/place/{placeId}/order")]
    [ApiController]
    public class PlaceOrderController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkContext _workContext;

        public PlaceOrderController(IDatabaseService databaseService, IWorkContext workContext)
        {
            _databaseService = databaseService;
            _workContext = workContext;
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
            var place = new Place(_databaseService.Context) {Id = placeId}.Find().FirstOrDefault();
            //order?.SetState(placeId, state, null, order.States.Value.Where(s => s.DeletedOnUtc == null && !s.Type.Equals(OrderStates.Validating) && !s.Type.Equals(OrderStates.DropOf) && !s.Type.Equals(ReclamationStates.Validating) && !s.Type.Equals(ReclamationStates.PickUp)).ToArray());
            order?.SetState(placeId, state, state.Equals(ReclamationStates.Validating)?$"Nová reklamace od {_workContext.CurrentUser.Name} do {place.Name}":null, order.GetState(OrderStates.GetState(order)));
            if (state.Equals(OrderStates.Delivering) || state.Equals(ReclamationStates.PickedUp) || state.Equals(ReclamationStates.Delivering))
                order?.AddUser(_workContext.CurrentUser);
            else if (state.Equals(OrderStates.Delivered) || state.Equals(ReclamationStates.Running) || state.Equals(ReclamationStates.Delivered))
                order?.RemoveUser(_workContext.CurrentUser);
            return OrderStates.GetNextState(state, _workContext.CurrentUser);
        }

        // POST: api/Order
        [HttpPost("{id}/set")]
        public string SetPost(ulong id, ulong placeId, [FromBody] string state)
        {
            return SetState(id, placeId, state);
        }
    }
}
