using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Tours;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Tours;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using NHibernate.Mapping;
using Serilog;

namespace dl.wm.suite.cms.services.Tours
{
  public class CreateTourProcessor : ICreateTourProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly ITourRepository _tourRepository;
    private readonly IContainerRepository _containerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateTourProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      ITourRepository tourRepository, IVehicleRepository vehicleRepository, IEmployeeRepository employeeRepository, IContainerRepository containerRepository)
    {
      _uOf = uOf;
      _tourRepository = tourRepository;
      _vehicleRepository = vehicleRepository;
      _employeeRepository = employeeRepository;
      _containerRepository = containerRepository;
      _autoMapper = autoMapper;
    }

    public Task<TourUiModel> CreateTourAsync(Guid accountIdToCreateThisTour, TourForCreationUiModel newTourUiModel)
    {
      var response =
        new TourUiModel()
        {
          Message = "START_CREATION"
        };

      if (newTourUiModel == null)
      {
        response.Message = "ERROR_INVALID_TOUR_MODEL";
        return Task.Run(() => response);
      }

      try
      {
        var tourToBeCreated = _autoMapper.Map<Tour>(newTourUiModel);

        tourToBeCreated.InjectWithAudit(accountIdToCreateThisTour);

        ThrowExcIfTourCannotBeCreated(tourToBeCreated);
        ThrowExcIfThisTourAlreadyExist(tourToBeCreated);

        var assetToBeInjected = _vehicleRepository.FindBy(newTourUiModel.TourAssetId);

        if (assetToBeInjected == null)
          throw new VehicleDoesNotExistException(newTourUiModel.TourAssetId);

        tourToBeCreated.InjectWithAsset(assetToBeInjected);
        //Add Employees
        IList<Employee> employeesToBeInjectedForTour = new List<Employee>();

        foreach (var tourEmployee in newTourUiModel.TourEmployees)
        {
          var employeeToBeInjected = _employeeRepository.FindBy(tourEmployee);
          if (employeeToBeInjected != null)
          {
            employeesToBeInjectedForTour.Add(employeeToBeInjected);
          }
        }

        if (employeesToBeInjectedForTour.Count <= 0)
          throw new NoneEmployeeFoundForThisTourException(newTourUiModel.TourName);

        //Todo: ThrowExcIfThisTourIsTodayScheduledForDriver
        ThrowExcIfThisTourIsTodayScheduledForDriver(newTourUiModel.TourScheduledDate, employeesToBeInjectedForTour);

        foreach (var employee in employeesToBeInjectedForTour)
        {
          var employeeTourToBeInjected = new EmployeeTour();

          employeeTourToBeInjected.InjectWithAttributes();
          employeeTourToBeInjected.InjectWithEmployee(employee);
          tourToBeCreated.InjectWithEmployeeTour(employeeTourToBeInjected);
        }
        //Add Containers
        IList<Container> containersToBeInjectedForTour = new List<Container>();

        foreach (var tourContainer in newTourUiModel.TourContainers)
        {
          var containerToBeInjected = _containerRepository.FindBy(tourContainer);
          if (containerToBeInjected != null)
          {
            containersToBeInjectedForTour.Add(containerToBeInjected);
          }
        }

        foreach (var container in containersToBeInjectedForTour)
        {
          var containerTourToBeInjected = new ContainerTour();

          containerTourToBeInjected.InjectWithContainer(container);
          tourToBeCreated.InjectWithContainerTour(containerTourToBeInjected);
        }


        Log.Debug(
          $"Create Tour: {newTourUiModel.TourName}" +
          "--CreateTour--  @NotComplete@ [CreateTourProcessor]. " +
          "Message: Just Before MakeItPersistence");

        MakeTourPersistent(tourToBeCreated);

        Log.Debug(
          $"Create Tour: {newTourUiModel.TourName}" +
          "--CreateTour--  @Complete@ [CreateTourProcessor]. " +
          "Message: Just After MakeItPersistence");
        response = ThrowExcIfTourWasNotBeMadePersistent(tourToBeCreated);
        response.Message = "SUCCESS_CREATION";
      }
      catch (Exception exxx)
      {
        response.Message = "UNKNOWN_ERROR";
        Log.Error(
          $"Create Tour: {newTourUiModel.TourName}" +
          $"--CreateTour--  @fail@ [CreateTourProcessor]. " +
          $"@innerfault:{exxx.Message} and {exxx.InnerException}");
      }


      return Task.Run(() => response);
    }

    private void ThrowExcIfThisTourIsTodayScheduledForDriver(in DateTime tourScheduledDate, IList<Employee> employeesToBeInjectedForTour)
    {
      
    }

    private void ThrowExcIfThisTourAlreadyExist(Tour tourToBeCreated)
    {
      //Todo: 
      var customerRetrieved = _tourRepository.FindByName(tourToBeCreated.Name);
      if (customerRetrieved != null)
      {
        throw new TourAlreadyExistsException(tourToBeCreated.Name,
          tourToBeCreated.GetBrokenRulesAsString());
      }
    }

    private void ThrowExcIfTourCannotBeCreated(Tour tourToBeCreated)
    {
      bool canBeCreated = !tourToBeCreated.GetBrokenRules().Any();
      if (!canBeCreated)
        throw new InvalidTourException(tourToBeCreated.GetBrokenRulesAsString());
    }

    private TourUiModel ThrowExcIfTourWasNotBeMadePersistent(Tour tourToHaveBeenCreated)
    {
      var retrievedTour = _tourRepository.FindByName(tourToHaveBeenCreated.Name);
      if (retrievedTour != null)
        return _autoMapper.Map<TourUiModel>(retrievedTour);
      throw new TourDoesNotExistAfterMadePersistentException(tourToHaveBeenCreated.Name);
    }


    private void MakeTourPersistent(Tour tourToBeCreated)
    {
      _tourRepository.Save(tourToBeCreated);
      _uOf.Commit();
    }
  }
}
