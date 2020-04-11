namespace CommandsSystem.Commands {
   /* public class ChangeGameObjectStateCommand<T> : ICommand
        where T: IGameObjectProperty, new()
    {
        
        public T state;

        public ChangeGameObjectStateCommand(T state)
        {
            this.state = state;
            
        }
        
        public ChangeGameObjectStateCommand() {}


        public void Run()
        {
            var gameObject = ObjectID.GetObject(state.ID);
            if (gameObject is null)
            {
                var spawnCommand = new SpawnPrefabCommand {
                    id = state.ID, 
                    //position = state.position, 
                    //rotation = state.rotation, 
                    prefabName = "PlayerGhost"
                };

      
                
                
                gameObject = Client.client.SpawnObject(spawnCommand);
            }

            var controller = gameObject.GetComponent<UnmanagedGameObject<T>>();
            if (controller is null) return;
            controller.SetStateAnimated(state);

            /*    GhostController controller = character.GetComponent<GhostController>();
                if (controller is null) return;
                controller.SetStateAnimated(state);
    */

            /// Assert. (!character.CompareTag("EntityPlayer")) return;
    /*    }
    }*/
}