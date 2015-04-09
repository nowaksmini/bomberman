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
    public class LocationDAOToLocationEntity : ITypeConverter<LocationDAO, Location>
    {
        public Location Convert(LocationDAO Source)
        {
            Location Target = new Location();

            return Target;
        }

        public Location Convert(ResolutionContext context)
        {
            return Convert((LocationDAO)context.SourceValue);
        }
    }
}
