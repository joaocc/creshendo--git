using System;
using System.Collections;
using System.IO;
using System.Threading;
using Creshendo.Util.Rete;

/// <summary> Copyright 2006 Alexander Wilden, Christoph Emonds, Sebastian Reinartz
/// *
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// *
/// http://ruleml-dev.sourceforge.net/
/// *
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// </summary>
namespace Creshendo.Util.Messagerouter
{
    /// <summary> A MessageRouter is responsible for sending messages to the Rete-engine and
    /// receive the answers. Possible MessageListeners will be notified of all events
    /// that occured.
    /// 
    /// </summary>
    /// <author>  Alexander Wilden, Christoph Emonds, Sebastian Reinartz
    /// 
    /// </author>
    [Serializable]
    public class MessageRouter
    {
        private const long serialVersionUID = 1L;
        private ArrayList commandQueue;
        private readonly CommandThread commandThread;

        private volatile String currentChannelId = "";

        /// <summary> The Rete-engine we work with
        /// </summary>
        private readonly Rete.Rete engine;

        // TODO is this threadsafe?

        private int idCounter = 0;

        /// <summary> The org.jamocha.rete.util.IList of MessageListeners
        /// </summary>
        private IMap idToChannel;

        private IMap idToMessages;
        private readonly CLIPSInterpreter interpreter;
        private ArrayList messageQueue;

        /// <summary>
        /// The constructor for a message router.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public MessageRouter(Rete.Rete engine)
        {
            InitBlock();
            this.engine = engine;
            interpreter = new CLIPSInterpreter(engine);
            commandThread = new CommandThread(this);
            commandThread.Start();
        }

        /// <summary>
        /// returns the underlying Rete-engine.
        /// </summary>
        /// <value>The rete engine.</value>
        /// <returns> The Rete-engine used in this MessageRouter-instance.
        /// </returns>
        public virtual Rete.Rete ReteEngine
        {
            get { return engine; }
        }

        /// <summary>
        /// Gets the current channel id.
        /// </summary>
        /// <value>The current channel id.</value>
        public virtual String CurrentChannelId
        {
            get { return currentChannelId; }
        }

        private void InitBlock()
        {
            idToChannel = new HashMap();
            idToMessages = new HashMap();
            commandQueue = ArrayList.Synchronized(new ArrayList());
            messageQueue = ArrayList.Synchronized(new ArrayList());
        }


        /// <summary>
        /// Posts the message event.
        /// </summary>
        /// <param name="event_Renamed">The event_ renamed.</param>
        public virtual void postMessageEvent(MessageEvent event_Renamed)
        {
            messageQueue.Add(event_Renamed);
        }

        /// <summary>
        /// Enqueues the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="channelId">The channel id.</param>
        public virtual void enqueueCommand(Object command, String channelId)
        {
            commandQueue.Add(new CommandObject(command, channelId));
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public virtual IStreamChannel openChannel(String channelName, Stream inputStream)
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
        public virtual IStreamChannel openChannel(String channelName, Stream inputStream, int interestType)
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
        public virtual IStreamChannel openChannel(String channelName, StreamReader reader)
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
        public virtual IStreamChannel openChannel(String channelName, StreamReader reader, int interestType)
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
        public virtual IStringChannel openChannel(String channelName)
        {
            return openChannel(channelName, InterestType.MINE);
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="interestType">Type of the interest.</param>
        /// <returns></returns>
        public virtual IStringChannel openChannel(String channelName, int interestType)
        {
            IStringChannel channel = new StringChannelImpl(channelName + "_" + idCounter++, this, interestType);
            registerChannel(channel);
            return channel;
        }

        /// <summary>
        /// Closes the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public virtual void closeChannel(ICommunicationChannel channel)
        {
            lock (idToChannel)
            {
                idToChannel.Remove(channel.ChannelId);
                idToMessages.Remove(channel.ChannelId);
                // If it's a StreamChannel, stop the Parser-Thread
                if (channel is StreamChannelImpl)
                {
                    ((StreamChannelImpl) channel).close();
                }
            }
        }

        private void registerChannel(ICommunicationChannel channel)
        {
            lock (idToChannel)
            {
                idToChannel.Put(channel.ChannelId, channel);
                idToMessages.Put(channel.ChannelId, new ArrayList());
            }
        }

        internal virtual void fillMessageList(String channelId, IList destinationList)
        {
            lock (idToChannel)
            {
                IList storedMessages = (IList) idToMessages.Get(channelId);
                if (storedMessages != null && destinationList != null)
                {
                    foreach (object val in storedMessages)
                    {
                        ((IList) destinationList).Add(val);
                    }
                    storedMessages.Clear();
                }
            }
        }

        public void ShutDown()
        {
            commandThread.Instance.Abort();
            while (commandThread.Instance.ThreadState != ThreadState.Stopped)
            {
                commandThread.Instance.Abort();
            }
        }

        #region Nested type: CommandObject

        private sealed class CommandObject
        {
            public readonly String channelId;
            public readonly Object command;

            public CommandObject(Object command, String channelId)
            {
                this.command = command;
                this.channelId = channelId;
            }
        }

        #endregion

        #region Nested type: CommandThread

        private sealed class CommandThread : SupportClass.ThreadClass
        {
            private MessageRouter enclosingInstance;

            public CommandThread(MessageRouter enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }

            public MessageRouter Enclosing_Instance
            {
                get { return enclosingInstance; }
            }

            private void InitBlock(MessageRouter enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public override void Run()
            {
                while (true)
                {
                    if (Enclosing_Instance.commandQueue.Count == 0)
                    {
                        try
                        {
                            Thread.Sleep(new TimeSpan(10000*10));
                        }
                        catch (ThreadInterruptedException e)
                        {
                            // TODO Auto-generated catch block
                            //SupportClass.WriteStackTrace(e, Console.Error);
                        }
                        catch(Exception e)
                        {
                            //SupportClass.WriteStackTrace(e, Console.Error);
                            return;
                        }
                    }
                    else
                    {
                        CommandObject schabau = (CommandObject) Enclosing_Instance.commandQueue[0];
                        Enclosing_Instance.commandQueue.RemoveAt(0);
                        if (schabau != null)
                        {
                            Enclosing_Instance.currentChannelId = schabau.channelId;
                            try
                            {
                                Enclosing_Instance.messageQueue.Add(new MessageEvent(MessageEvent.COMMAND, schabau.command, Enclosing_Instance.currentChannelId));
                                IReturnVector result = Enclosing_Instance.interpreter.executeCommand(schabau.command);
                                Enclosing_Instance.messageQueue.Add(new MessageEvent(MessageEvent.RESULT, result, Enclosing_Instance.currentChannelId));
                            }
                            catch (Exception e)
                            {
                                Enclosing_Instance.postMessageEvent(new MessageEvent(MessageEvent.ERROR, e, Enclosing_Instance.currentChannelId));
                            }
                            finally
                            {
                                Enclosing_Instance.currentChannelId = null;
                            }
                        }
                    }

                    MessageEvent[] evts = (MessageEvent[]) Enclosing_Instance.messageQueue.ToArray(typeof (MessageEvent));
                    ArrayList allMessages = new ArrayList(evts);

                    Enclosing_Instance.messageQueue.Clear();
                    for (int i = 0; i < allMessages.Count; ++i)
                    {
                        MessageEvent event_Renamed = (MessageEvent) allMessages[i];
                        lock (Enclosing_Instance.idToChannel)
                        {
                            IEnumerator ch = Enclosing_Instance.idToChannel.Values.GetEnumerator();
                            while (ch.MoveNext())
                            {
                                ICommunicationChannel channel = (ICommunicationChannel) ch.Current;
                                if (InterestType.ALL == channel.Interest || (InterestType.MINE == channel.Interest) && channel.ChannelId.Equals(event_Renamed.ChannelId))
                                {
                                    IList messageList = (IList) Enclosing_Instance.idToMessages.Get(channel.ChannelId);
                                    if (messageList != null)
                                    {
                                        messageList.Add(event_Renamed);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}