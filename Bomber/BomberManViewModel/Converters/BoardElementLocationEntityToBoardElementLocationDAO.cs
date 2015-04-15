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
    public class BoardElementLocationEntityToBoardElementLocationDAO : ITypeConverter<BoardElementLocation, BoardElementLocationDAO>
    {
        public BoardElementLocationDAO Convert(BoardElementLocation Source)
        {
            BoardElementLocationDAO Target = new BoardElementLocationDAO();
            Target.YLocation = Target.YLocation;
            Target.XLocation = Target.XLocation;
            Target.ID = Source.ID;
            Target.Game = Mapper.Map<GameDAO>(Source.Game);
            Target.BoardElement = Mapper.Map<BoardElementDAO>(Source.BoardElement);
            Target.Timeout = Source.Timeout;
            return Target;
        }

        public BoardElementLocationDAO Convert(ResolutionContext context)
        {
            return Convert((BoardElementLocation)context.SourceValue);
        }
    }
}
