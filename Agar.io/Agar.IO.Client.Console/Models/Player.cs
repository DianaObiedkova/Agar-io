using ProtoBuf;

namespace Agar.IO.Client.WinForms.Models
{
    [ProtoContract]
    public class Player : Entity
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        public long FirstMovementTime { get; set; }
        public long LastMovementTime { get; set; }
        public long FirstMovementServerTime { get; set; }
        public double Speed
        {
            get
            {
                return Radius * 0.8;
            }
        }

        public Player Clone()
        {
            return (Player)MemberwiseClone();
        }
    }
}
