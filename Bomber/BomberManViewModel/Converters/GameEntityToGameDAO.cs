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
    public class GameEntityToGameDAO : ITypeConverter<Game, GameDAO>
    {
        public GameDAO Convert(Game Source)
        {
            GameDAO Target = new GameDAO();
            Target.Finished = Source.Finished;
            Target.ID = Source.ID;
            Target.Level = Source.Level;
            Target.Life = Source.Life;
            Target.PlayerXLocation = Source.PlayerXLocation;
            Target.PlayerYLocation = Source.PlayerYLocation;
            Target.Points = Source.Points;
            Target.SaveTime = Source.SaveTime;
            Target.User = Mapper.Map<UserDAO>(Source.User);
            return Target;
        }

        public GameDAO Convert(ResolutionContext context)
        {
            return Convert((Game)context.SourceValue);
        }
    }
}
