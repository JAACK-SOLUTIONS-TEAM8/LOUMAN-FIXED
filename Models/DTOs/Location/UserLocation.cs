using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Location
{
    public class UserLocation
    {
        public int UserId { get; set; }
        public LocationDto Location {get;set;}
    }
}
