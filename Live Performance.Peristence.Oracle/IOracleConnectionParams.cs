namespace Live_Performance.Peristence.Oracle
{
    /// <summary>
    ///     Interfaces that contains connection parameters for connecting to the Oracle server.
    /// </summary>
    public interface IOracleConnectionParams
    {
        /// <summary>
        ///     Hostname or IP.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        ///     Port. (Usually 3306)
        /// </summary>
        uint Port { get; set; }

        /// <summary>
        ///     Service name.
        /// </summary>
        string ServiceName { get; set; }

        /// <summary>
        ///     Username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        ///     Password.
        /// </summary>
        string Password { get; set; }
    }
}