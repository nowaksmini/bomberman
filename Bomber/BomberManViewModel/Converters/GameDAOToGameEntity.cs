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
    public class GameDAOToGameEntity : ITypeConverter<GameDAO, Game>
    {
        public Game Convert(GameDAO Source)
        {
            Game Target = new Game();
            Target.Finished = Source.Finished;
            Target.ID = Source.ID;
            Target.Level = Source.Level;
            Target.Life = Source.Life;
            Target.PlayerXLocation = Source.PlayerXLocation;
            Target.PlayerYLocation = Source.PlayerYLocation;
            Target.Points = Source.Points;
            Target.SaveTime = Source.SaveTime;
            Target.User = Mapper.Map<User>(Source.User);
            return Target;
        }

        public Game Convert(ResolutionContext context)
        {
            return Convert((GameDAO)context.SourceValue);
        }
    }
}
