﻿using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Converters
{
    public class BoardElementEntityToBoardElementDAO : ITypeConverter<BoardElement, BoardElementDAO>
    {
        public BoardElementDAO Convert(BoardElement Source)
        {
            BoardElementDAO Target = new BoardElementDAO();
            return Target;
        }

        public BoardElementDAO Convert(ResolutionContext context)
        {
            return Convert((BoardElement)context.SourceValue);
        }
    }
}