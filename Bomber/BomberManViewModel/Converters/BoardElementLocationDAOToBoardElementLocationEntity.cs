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
    public class BoardElementLocationDAOToBoardElementLocationEntity : ITypeConverter<BoardElementLocationDAO, BoardElementLocation>
    {
        public BoardElementLocation Convert(BoardElementLocationDAO Source)
        {
            BoardElementLocation Target = new BoardElementLocation();
            Target.YLocation = Target.YLocation;
            Target.XLocation = Target.XLocation;
            Target.ID = Source.ID;
            Target.Game = Mapper.Map<Game>(Source.Game);
            Target.BoardElement = Mapper.Map<BoardElement>(Source.BoardElement);
            Target.Timeout = Source.Timeout;
            return Target;
        }

        public BoardElementLocation Convert(ResolutionContext context)
        {
            return Convert((BoardElementLocationDAO)context.SourceValue);
        }
    }
}
