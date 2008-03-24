using System;
using System.IO;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Messagerouter
{
    [Serializable]
    public class MessageRouter
    {
        private readonly Rete.Rete engine;
        private readonly CLIPSInterpreter interpreter;
        private int idCounter = 0;

        /// <summary>
        /// The constructor for a message router.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public MessageRouter(Rete.Rete engine)
        {
            this.engine = engine;
            this.engine.Message += engine_Message;
            interpreter = new CLIPSInterpreter(engine);
        }

        /// <summary>
        /// returns the underlying Rete-engine.
        /// </summary>
        /// <value>The rete engine.</value>
        /// <returns> The Rete-engine used in this MessageRouter-instance.
        /// </returns>
        public Rete.Rete ReteEngine
        {
            get { return engine; }
        }

        public event MessageHandler Message;

        private void engine_Message(object sender, MessageEventArgs e)
        {
            postMessageEvent(e);
        }

        /// <summary>
        /// Posts the message event.
        /// </summary>
        /// <param name="messageEventArgsRenamed">The event_ renamed.</param>
        private void postMessageEvent(MessageEventArgs messageEventArgsRenamed)
        {
            if (Message != null)
            {
                Message(this, messageEventArgsRenamed);
            }
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public IStreamChannel openChannel(String channelName, StreamReader inputStream)
        {
            return openChannel(channelName, inputStream, InterestType.MINE);
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="interestType">Type of the interest.</param>
        /// <returns></returns>
        public IStreamChannel openChannel(String channelName, TextReader inputStream, InterestType interestType)
        {
            IStreamChannel channel = new StreamChannelImpl(channelName + "_" + idCounter++, this, interestType);
            channel.init(inputStream);
            registerChannel(channel);
            return channel;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IStreamChannel openChannel(String channelName, TextReader reader)
        {
            return openChannel(channelName, reader, InterestType.MINE);
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="interestType">Type of the interest.</param>
        /// <returns></returns>
        public IStreamChannel openChannel(String channelName, StreamReader reader, InterestType interestType)
        {
            IStreamChannel channel = new StreamChannelImpl(channelName + "_" + idCounter++, this, interestType);
            channel.init(reader);
            registerChannel(channel);
            return channel;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns></returns>
        public virtual ICommunicationChannel openChannel(String channelName)
        {
            return openChannel(channelName, InterestType.MINE);
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="interestType">Type of the interest.</param>
        /// <returns></returns>
        public virtual ICommunicationChannel openChannel(String channelName, InterestType interestType)
        {
            ICommunicationChannel channel = new StringChannelImpl(channelName + "_" + idCounter++, this, interestType);
            registerChannel(channel);
            return channel;
        }

        /// <summary>
        /// Closes the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public virtual void closeChannel(ICommunicationChannel channel)
        {
            if (channel is StreamChannelImpl)
            {
                ((StreamChannelImpl) channel).close();
            }
            if (channel is IDisposable)
            {
                ((IDisposable)channel).Dispose();
            }
            channel.Command -= OnCommand;
            channel.Message -= OnChannelMessage;
        }

        private void registerChannel(ICommunicationChannel channel)
        {
            channel.Command += OnCommand;
            channel.Message += OnChannelMessage;
        }

        private void OnChannelMessage(object sender, MessageEventArgs e)
        {
            postMessageEvent(e);
        }

        private void OnCommand(object sender, CommandEventArgs args)
        {
            try
            {
                postMessageEvent(new MessageEventArgs(EventType.COMMAND, args.Command, args.ChannelID));
                IReturnVector result = interpreter.executeCommand(args.Command);
                postMessageEvent(new MessageEventArgs(EventType.RESULT, result, args.ChannelID));
            }
            catch (Exception e)
            {
                postMessageEvent(new MessageEventArgs(EventType.ERROR, e, args.ChannelID));
            }
        }
    }
}