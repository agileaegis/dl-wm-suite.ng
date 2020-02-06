using System.Collections.Generic;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders;
using dl.wm.suite.interprocess.api.Commanding.PackageRepository;
using dl.wm.suite.interprocess.api.Helpers.Exceptions.Commands;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound
{
    public sealed class WmInboundCommandBuilderRepository
    {
        private readonly Dictionary<byte, IWmInboundCommandBuilder> _cmdBuilders;


        private WmInboundCommandBuilderRepository()
        {
            _cmdBuilders = new Dictionary<byte, IWmInboundCommandBuilder>()
                               {
                                  {WmPackageRepository.GetPackageRepository.TelemetryRowTypeCode, 
                                      new TelemetryRowPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.TelemetryTypeCode, 
                                      new TelemetryPackageDetectedInboundCommandBuilder()},
                                  {WmPackageRepository.GetPackageRepository.AttributeTypeCode, 
                                      new AttributePackageDetectedInboundCommandBuilder()},
                               };
        }

        public static WmInboundCommandBuilderRepository GetCommandBuilderRepository { get; } = new WmInboundCommandBuilderRepository();

        public IWmInboundCommandBuilder this
            [byte index]
        {
            get
            {
                try
                {
                    return _cmdBuilders[index];
                }
                catch (KeyNotFoundException)
                {
                    throw new WmCommandNotFoundException();
                }
            }
        }
    }
}
