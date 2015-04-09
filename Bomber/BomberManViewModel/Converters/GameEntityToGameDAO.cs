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
            
            return Target;
        }

        public GameDAO Convert(ResolutionContext context)
        {
            return Convert((Game)context.SourceValue);
        }
    }
}
