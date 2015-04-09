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
    public class OponentLocationEntityToOponentLocationDAO : ITypeConverter<OponentLocation, OponentLocationDAO>
    {
        public OponentLocationDAO Convert(OponentLocation Source)
        {
            OponentLocationDAO Target = new OponentLocationDAO();
            return Target;
        }

        public OponentLocationDAO Convert(ResolutionContext context)
        {
            return Convert((OponentLocation)context.SourceValue);
        }
    }
}
