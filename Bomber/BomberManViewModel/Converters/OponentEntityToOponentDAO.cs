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
    public class OponentEntityToOponentDAO : ITypeConverter<Oponent, OponentDAO>
    {
        public OponentDAO Convert(Oponent Source)
        {
            OponentDAO Target = new OponentDAO();
            return Target;
        }

        public OponentDAO Convert(ResolutionContext context)
        {
            return Convert((Oponent)context.SourceValue);
        }
    }
}
