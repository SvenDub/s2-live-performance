namespace Live_Performance.Peristence.Oracle
{
    /// <summary>
    ///     Connection parameters for the production environment.
    /// </summary>
    public class ProductionOracleConnectionParams : IOracleConnectionParams
    {
        public string Host { get; set; } = "localhost";
        public uint Port { get; set; } = 1521;
        public string ServiceName { get; set; } = "xe";
        public string Username { get; set; } = "sloepke";
        public string Password { get; set; } = "sloepke";
    }
}