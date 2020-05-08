namespace Character.HP {
    public struct HPChange {
        public float delta;
        public int source;

        public HPChange(float delta, int source) {
            this.delta = delta;
            this.source = source;
        }
        
    }
}