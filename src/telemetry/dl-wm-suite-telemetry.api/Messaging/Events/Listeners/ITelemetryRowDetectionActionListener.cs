using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Messaging.Events.EventArgs;

namespace dl.wm.suite.telemetry.api.Messaging.Events.Listeners
{
    public interface ITelemetryRowDetectionActionListener
    {
        void Update(object sender, TelemetryRowEventArgs e);
    }
}
