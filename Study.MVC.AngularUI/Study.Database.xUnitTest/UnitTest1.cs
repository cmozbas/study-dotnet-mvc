using Study.Database.Query;
using Study.Database.Query.Reponse;
using Study.Database.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Study.Database.xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Context()
        {
            using(var db = new StudyContext())
            {
                db.Database.EnsureCreated();
                var radioStations = db.RadioStation.ToList();
            }
        }

        [Fact]
        public void Test_Repository()
        {
            using (var context = new StudyContext())
            {
                var rep = new RadioStationRepository(context);
                var res = rep.GetQueryable().ToList();
            }
        }

        [Fact]
        public async Task Test_QueryRepository()
        {
            using (var context = new StudyContext())
            {
                var rep = new QueryRepository(context);
                var res = await rep.GetQueryFromStringAsync<RadioStationResponse>("select name, code from RadioStation");
            }
        }
    }
}
