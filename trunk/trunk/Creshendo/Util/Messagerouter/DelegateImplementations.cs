namespace Creshendo.Util.Messagerouter
{
    public delegate void CommandHandler(object sender, CommandEventArgs e);

    public delegate void MessageHandler(object sender, MessageEventArgs e);
}