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
    public class OponentLocationDAOToOponentLocationEntity : ITypeConverter<OponentLocationDAO, OponentLocation>
    {
        public OponentLocation Convert(OponentLocationDAO Source)
        {
            OponentLocation Target = new OponentLocation();
            return Target;
        }

        public OponentLocation Convert(ResolutionContext context)
        {
            return Convert((OponentLocationDAO)context.SourceValue);
        }
    }
}
