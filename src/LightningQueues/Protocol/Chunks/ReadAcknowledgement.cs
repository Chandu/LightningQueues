﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using LightningQueues.Exceptions;
using LightningQueues.Logging;

namespace LightningQueues.Protocol.Chunks
{
    public class ReadAcknowledgement : Chunk
    {
        public ReadAcknowledgement(ILogger logger, string sender)
            : base(logger, sender)
        {
        }

        public ReadAcknowledgement(ILogger logger)
            : this(logger, null)
        {
        }

        protected override async Task ProcessInternalAsync(Stream stream)
        {
            _logger.Debug("Reading acknowledgement about accepting messages to {0}", _endpoint);
            var receiveBuffer = new byte[ProtocolConstants.AcknowledgedBuffer.Length];
            await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
            var recieveRespone = Encoding.Unicode.GetString(receiveBuffer);
            if (recieveRespone != ProtocolConstants.Acknowledged)
            {
                _logger.Info("Response from sender acknowledgement was the wrong format", _endpoint);
                throw new InvalidAcknowledgementException();
            }
        }

        public override string ToString()
        {
            return "Read Acknowledgement";
        }
    }
}