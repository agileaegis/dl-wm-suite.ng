using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Tours
{
    public class TourBusinessRules
    {
        public static BusinessRule Name => new BusinessRule("Tour", "Tour Name must not be null or empty!");
        public static BusinessRule ScheduledDate => new BusinessRule("Tour", "Tour Scheduled Date must not be null or empty!");
    }
}