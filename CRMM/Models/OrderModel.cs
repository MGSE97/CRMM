using System.ComponentModel.DataAnnotations;
using Data.Mapping.Attributes;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Database;
using Services;

namespace CRMM.Models
{
    public class OrderModel
    {
        public ulong Id { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Název")]
        public string Name { get; set; }

        [Display(Name = "Popis")]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Zboží")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Lokalita")]
        public ulong LocationId { get; set; }
        public SelectListItem[] Locations { get; set; }

        public bool Validating { get; set; }

        public string NextState { get; set; }

        public Order ToOrder(IDBContext context)
        {
            return new Order(context)
            {
                Description = Description,
                Id = Id,
                Name = Name,
                Type = Type
            };
        }

    }
    public static class OrderExtensions
    {
        public static OrderModel ToModel(this Order order)
        {
            return new OrderModel()
            {
                Id = order.Id,
                Name = order.Name,
                Type = order.Type,
                Description = order.Description,
                Validating = order.HasState(OrderStates.Validating),
                NextState = OrderStates.GetNextState(order)
            };
        }
    }
}