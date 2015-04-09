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
    public class OponentDAOToOponentEntity : ITypeConverter<OponentDAO, Oponent>
    {
        public Oponent Convert(OponentDAO Source)
        {
            Oponent Target = new Oponent();
            return Target;
        }

        public Oponent Convert(ResolutionContext context)
        {
            return Convert((OponentDAO)context.SourceValue);
        }
    }
}