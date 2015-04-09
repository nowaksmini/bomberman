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
    public class UserDAOToUserEntity : ITypeConverter<UserDAO, User>
    {
        public User Convert(UserDAO Source)
        {
            User Target = new User();
            return Target;
            
        }

        public User Convert(ResolutionContext context)
        {
            return Convert((UserDAO)context.SourceValue);
        }
    }
}
