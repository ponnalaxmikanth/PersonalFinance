using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts;
using BusinessEntities.Contracts.MutualFunds;
using DataAccess.MutualFunds;
using System.Web.Mvc;
//using Microsoft.Practices.Unity;
using Unity;
using Unity.AspNet.Mvc;
//using Unity.Mvc4;

namespace PersonlaFinance
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
            container.RegisterType<IMutualFundsBusinessAccess, MutualFundsRepository>();
            container.RegisterType<IMutualFundDataAccess, MutualFundsDataAccess>();

            container.RegisterType<IDashboardRepository, DashboardRepository>();
            container.RegisterType<IDashboardDataAccess, DashboardDataAccess>();

            container.RegisterType<ICommonRepository, CommonRepository>();
            container.RegisterType<ICommonDataAccess, CommonDataAccess>();
        }
  }
}