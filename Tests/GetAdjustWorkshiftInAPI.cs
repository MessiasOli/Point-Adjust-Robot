using Point_Adjust_Robot.Core.Model;
using Repository.Nexti;

namespace Tests
{
    public class GetAdjustWorkshiftInAPI
    {
        [Fact]  
        public void GetAdjustId()
        {
            var repository = new PersonsRepository();
            Person person = repository.GetByParams(new string[] { "X000001" }).Result;

            Assert.Equal(person.name, "MESSIAS TESTE");
        }
    }
}
