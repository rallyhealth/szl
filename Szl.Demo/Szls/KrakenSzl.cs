namespace Szl.Demo.Szls
{
    public class KrakenActionableSzl : ActionableSzl
    {
        public KrakenActionableSzl() : base("Kraken", "Attack", null)
        {
        }

        public override ISzl GetFurthestState()
        {
            return this;
        }
    }
}
