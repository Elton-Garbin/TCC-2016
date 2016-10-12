using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using StartIdea.DataAccess;
using System.Web.Mvc;

namespace StartIdea.UI
{
    public class InjectionDependency
    {
        public static void Configurar()
        {
            var container = new Container();

            container.Register<StartIdeaDBContext>(Lifestyle.Singleton);
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}