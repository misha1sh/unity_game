

using System;

namespace CommandsSystem.Commands
{
    
    [Serializable]
    public class CreateGhostCommand : Command<CreateGhostCommand>
    {
        public override CommandType type() => CommandType.CreateGhostCommand;

        public int id;
      /*  public CharacterState 
        public CreateGhostCommand() {
            
        }*/

        public override void Run()
        {
            throw new NotImplementedException();
        }

    }
}