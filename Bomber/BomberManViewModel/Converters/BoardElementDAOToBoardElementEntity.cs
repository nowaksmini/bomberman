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
    public class BoardElementDAOToBoardElementEntity : ITypeConverter<BoardElementDAO, BoardElement>
    {
        public BoardElement Convert(BoardElementDAO Source)
        {
            BoardElement Target = new BoardElement();
            return Target;
        }

        public BoardElement Convert(ResolutionContext context)
        {
            return Convert((BoardElementDAO)context.SourceValue);
        }
    }
}
