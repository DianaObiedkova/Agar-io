using ProtoBuf;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    class Invalid : BaseCommand
    {
        [ProtoMember(1)]
        public string Message { get; set; }
        public Invalid(string message)
        {
            Message = message;
        }

        public override void Execute(Game game)
        {
            game.IsPredictionValid = false;
        }
    }
}
