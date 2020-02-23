using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Tours;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Tours
{
  public class CreateTourProcessor : ICreateTourProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly ITourRepository _tourRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateTourProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      ITourRepository tourRepository)
    {
      _uOf = uOf;
      _tourRepository = tourRepository;
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

      
      return Task.Run(() => response);
    }


    private void MakeTourPersistent(Tour tourToBeCreated)
    {
      _tourRepository.Save(tourToBeCreated);
      _uOf.Commit();
    }
  }
}
