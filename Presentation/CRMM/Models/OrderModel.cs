using System;
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

        [Display(Name = "Zboží")]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Typ")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Lokalita")]
        public ulong LocationId { get; set; }
        public SelectListItem[] Locations { get; set; }

        public bool Validating { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public string State { get; set; }
        public DateTime? StateCreatedOnUtc { get; set; }

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
        public static OrderModel ToModel(this Order order, User user = null)
        {
            var state = OrderStates.GetState(order);
            return new OrderModel()
            {
                Id = order.Id,
                Name = order.Name,
                Type = order.Type,
                Description = order.Description,
                Validating = order.HasState(OrderStates.Validating),
                State = state,
                StateCreatedOnUtc = state != null ? order.GetState(state)?.CreatedOnUtc : null,
                NextState = OrderStates.GetNextState(order, user),
                CreatedOnUtc = order.GetState(OrderStates.DropOf)?.CreatedOnUtc
            };
        }
    }
}