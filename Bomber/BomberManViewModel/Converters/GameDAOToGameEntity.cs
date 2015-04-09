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
            return Target;
        }

        public Game Convert(ResolutionContext context)
        {
            return Convert((GameDAO)context.SourceValue);
        }
    }
}
