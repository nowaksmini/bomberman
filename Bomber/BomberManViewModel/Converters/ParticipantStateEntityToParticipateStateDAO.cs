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
    public class ParticipantStateEntityToParticipateStateDAO : ITypeConverter<ParticipantState, ParticipantStateDAO>
    {
        public ParticipantStateDAO Convert(ParticipantState Source)
        {
            ParticipantStateDAO Target = new ParticipantStateDAO();
            return Target;
        }

        public ParticipantStateDAO Convert(ResolutionContext context)
        {
            return Convert((ParticipantState)context.SourceValue);
        }
    }
}
