using DatabaseContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMM.Models.API
{
    public class MapPlaceModel
    {
        public PlaceModel Place { get; set; }

        public UserModel User { get; set; }

        public IList<UserModel> Workers { get; set; }

        public IList<OrderModel> Orders { get; set; }

        public MapPlaceModel()
        {
            Workers = new List<UserModel>();
            Orders = new List<OrderModel>();
        }
    }
}
