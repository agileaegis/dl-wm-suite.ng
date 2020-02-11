﻿using dl.wm.presenter.Commanding.Servers.Base;

namespace dl.wm.presenter.Commanding.Events.Inbound

{
    public interface IInboundEventRaisingBehavior
    {
        void RaiseWmEvent(CommandingInboundBaseServer inboundEventServer);
    }
}