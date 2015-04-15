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
            Target.Game = Mapper.Map<Game>(Source.Game);
            Target.ID = Source.ID;
            Target.IsAlive = Source.IsAlive;
            Target.XLocation = Source.XLocation;
            Target.YLocation = Source.YLocation;
            Target.Oponent = Mapper.Map<Oponent>(Source.Oponent);
            return Target;
        }

        public OponentLocation Convert(ResolutionContext context)
        {
            return Convert((OponentLocationDAO)context.SourceValue);
        }
    }
}
