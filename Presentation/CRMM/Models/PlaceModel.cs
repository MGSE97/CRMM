using System;
using System.ComponentModel.DataAnnotations;
using DatabaseContext.Models;
using Model.Database;
using Services;

namespace CRMM.Models
{
    public class PlaceModel
    {
        public ulong Id { get; set; }
        [Required]
        [MinLength(1)]
        [Display(Name = "Název")]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        [Display(Name = "Typ")]
        public string Type { get; set; }
        [Required]
        [MinLength(1)]
        [Display(Name = "Adresa")]
        public string Address { get; set; }
        [Display(Name = "Zeměpisná šířka")]
        public double X { get; set; }
        [Display(Name = "Zeměpisná výška")]
        public double Y { get; set; }

        public bool Validating { get; set; }

        public Place ToPlace(IDBContext context)
        {
            return new Place(context)
            {
                Id = Id,
                Name = Name,
                Address = Address,
                Type = Type,
                X = X,
                Y = Y
            };
        }
    }

    public static class PlaceExtensions
    {
        public static PlaceModel ToModel(this Place place)
        {
            return new PlaceModel()
            {
                Address = place.Address,
                Id = place.Id,
                Name = place.Name,
                Type = place.Type,
                X = place.X,
                Y = place.Y,
                Validating = place.HasState(PlaceStates.Validating)
            };
        }
    }
}