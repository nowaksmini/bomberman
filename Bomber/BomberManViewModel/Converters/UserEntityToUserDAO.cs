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
    public class UserEntityToUserDAO : ITypeConverter<User, UserDAO>
    {
        public UserDAO Convert(User Source)
        {
            UserDAO Target = new UserDAO();
            return Target;
        }

        public UserDAO Convert(ResolutionContext context)
        {
            return Convert((User)context.SourceValue);
        }
    }
}