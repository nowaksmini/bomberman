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
    public class ParticipantStateDAOToParticipantEntity : ITypeConverter<ParticipantStateDAO, ParticipantState>
    {
        public ParticipantState Convert(ParticipantStateDAO Source)
        {
            ParticipantState Target = new ParticipantState();
            return Target;
        }

        public ParticipantState Convert(ResolutionContext context)
        {
            return Convert((ParticipantStateDAO)context.SourceValue);
        }
    }
}
