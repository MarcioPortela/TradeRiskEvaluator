using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Domain.Entities
{
    public class Trade
    {
        public decimal Value { get; private set; }
        public Sector ClientSector { get; private set; }
        public string? ClientId { get; private set; }

        public Trade(decimal value, Sector clientSector)
        {
            Value = value;
            ClientSector = clientSector;
        }

        public Trade(decimal value, Sector clientSector, string clientId)
        {
            Value = value;
            ClientSector = clientSector;
            ClientId = clientId;
        }
    }
}
