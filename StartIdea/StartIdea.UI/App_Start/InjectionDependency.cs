using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using StartIdea.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StartIdea.UI
{
    public class InjectionDependency
    {
        public static void Configurar()
        {
            var container = new Container();

            container.Register<StartIdeaDBContext>(Lifestyle.Singleton);

            // 3. Optionally verify the container's configuration.
            container.Verify();

            // 4. Store the container for use by the application
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}