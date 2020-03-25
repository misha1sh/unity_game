

using System;

namespace CommandsSystem.Commands
{
    
    [Serializable]
    public class CreateGhostCommand : Command<CreateGhostCommand>
    {
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