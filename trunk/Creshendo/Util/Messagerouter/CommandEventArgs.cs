using System;

namespace Creshendo.Util.Messagerouter
{
    public class CommandEventArgs : EventArgs
    {
        private string _channelid;
        private object _command;

        public CommandEventArgs(object command, string channelid)
        {
            _command = command;
            _channelid = channelid;
        }

        public object Command
        {
            get { return _command; }
        }

        public String ChannelID
        {
            get { return _channelid; }
        }
    }
}