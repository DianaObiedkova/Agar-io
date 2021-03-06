using ProtoBuf;
using System.Diagnostics;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    class End : BaseCommand
    {
        [ProtoMember(1)]
        public string Message { get; set; }
        public override async void Execute(Game game)
        {
            Debug.WriteLine("Stop..");
            game.Close(Message);
        }
        public End() { }
        public End(string Message)
        {
            this.Message = Message;
        }
    }
}
