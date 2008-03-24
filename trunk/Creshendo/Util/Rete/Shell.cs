using System;
using System.IO;
using Creshendo.Util.Messagerouter;

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
namespace Creshendo.Util.Rete
{
    public class Shell
    {
        public const String CHANNELNAME = "Shell";

        private readonly MessageRouter router;
        private IStreamChannel channel;

        public Shell(Rete engine)
        {
            router = engine.MessageRouter;
        }

        public void Run()
        {
            router.Message += OnMessage;
            channel = router.openChannel(CHANNELNAME, (TextReader) Console.In);
            Console.Out.Write(Constants.SHELL_PROMPT);
        }

        public void Stop()
        {
            router.Message -= OnMessage;
            router.closeChannel(channel);
        }

        private void OnMessage(object sender, MessageEventArgs args)
        {
            bool printPrompt = false;

            switch (args.Type)
            {
                case EventType.ADD_NODE_ERROR:
                case EventType.PARSE_ERROR:
                case EventType.ERROR:
                case EventType.CLIPSPARSER_ERROR:
                    System.Exception ex = args.Message as System.Exception;
                    if (ex != null)
                        Console.Out.WriteLine(ex.Message);
                    else
                        Console.Out.WriteLine(args.Message);
                    Console.Out.Write(Constants.SHELL_PROMPT);
                    break;

                case EventType.RESULT:
                    string msg = args.Message.ToString();
                    if (String.IsNullOrEmpty(msg) == false)
                        Console.Out.WriteLine(msg);
                    Console.Out.Write(Constants.SHELL_PROMPT);
                    break;

                case EventType.ENGINE:
                    string msg1 = args.Message.ToString();
                    Console.Out.Write(msg1);
                    break;

                case EventType.COMMAND:
                    break;

                case EventType.ADD_RULE_EVENT:
                case EventType.REMOVE_RULE_EVENT:
                case EventType.INVALID_RULE:
                case EventType.RULE_EXISTS:
                case EventType.TEMPLATE_NOTFOUND:
                case EventType.CLIPSPARSER_WARNING:
                case EventType.CLIPSPARSER_REINIT:
                case EventType.FUNCTION_NOT_FOUND:
                case EventType.FUNCTION_INVALID:
                    throw new ApplicationException(string.Format("Unexpected EventType: {0}", args.Type));
            }
        }
    }
}