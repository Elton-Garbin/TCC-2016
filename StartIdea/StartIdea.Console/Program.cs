using Newtonsoft.Json;
using StartIdea.DataAccess.Repositories.Base;
using StartIdea.Model.ScrumArtefatos;

namespace StartIdea.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var repo = new Repository<StatusTarefa>())
            {
                foreach (var item in repo.GetAll())
                    System.Console.WriteLine(JsonConvert.SerializeObject(item));
            }

            System.Console.ReadKey();
        }
    }
}
