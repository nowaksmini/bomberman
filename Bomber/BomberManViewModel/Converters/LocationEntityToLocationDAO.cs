using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Converters
{
    public class LocationEntityToLocationDAO : ITypeConverter<Location, LocationDAO>
    {
        public LocationDAO Convert(Location Source)
        {
            LocationDAO Target = new LocationDAO();

            return Target;
        }

        public LocationDAO Convert(ResolutionContext context)
        {
            return Convert((Location)context.SourceValue);
        }
    }
}
