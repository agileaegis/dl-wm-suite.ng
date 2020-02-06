﻿using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;

namespace dl.wm.suite.fleet.contracts.Trackables
{
    public interface ICreateTrackableProcessor
    {
        Task<TrackableUiModel> CreateTrackableAsync(string accountEmailToCreateThisTrackable, TrackableForCreationUiModel newTrackableUiModel);
    }
}