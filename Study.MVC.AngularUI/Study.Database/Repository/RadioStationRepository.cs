using Study.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Database.Repository
{
    public class RadioStationRepository : GenericRepository<RadioStation>, IRadioStationRepository
    {
        private StudyContext context;

        public RadioStationRepository(StudyContext context) 
            : base(context)
        {
            this.context = context;
        }
    }

    public interface IRadioStationRepository : IGenericRepository<RadioStation>, IUnitOfWork<RadioStation>
    {
    }
}
