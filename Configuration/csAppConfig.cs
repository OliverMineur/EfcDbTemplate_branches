using System.Text.Json;
using Microsoft.Extensions.Configuration;


//Configuration namespace is base layer
//used for all classed needed to configure the application and it's components
namespace Configuration;

//csAppConfig will contain all configuration in our example
//but it could be segmented into several classes, each with its own
//configuration responisibily
public sealed class csAppConfig
{
    public const string Appsettingfile = "appsettings.json";

    #region Singleton design pattern
    private static readonly object instanceLock = new();

    private static csAppConfig _instance = null;
    private static IConfigurationRoot _configuration = null;
    #endregion

    #region All the DB Connections in the appsetting file
    private static DbSetDetail _dbSetActive = new DbSetDetail();
    private static List<DbSetDetail> _dbSets = new List<DbSetDetail>();
    #endregion

    private csAppConfig()
    {

        //Create  ConfigurationRoot 
        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile(Appsettingfile, optional: true, reloadOnChange: true);
        _configuration = builder.Build();

        //get DbSet details from Appsettingfile
        _configuration.Bind("DbSets", _dbSets);  //Need the NuGet package Microsoft.Extensions.Configuration.Binder

        //Set the active db set and fill in location and server into Login Details
        var i = int.Parse(_configuration["DbSetActiveIdx"]);
        _dbSetActive = _dbSets[i];
    }

    public static IConfigurationRoot ConfigurationRoot
    {
        get
        {
            lock (instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new csAppConfig();
                }
                return _configuration;
            }
        }
    }

    public static DbSetDetail DbSetActive
    {
        get
        {
            lock (instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new csAppConfig();
                }
                return _dbSetActive;
            }
        }
    }
}

public class DbSetDetail
{
    public string DbLocation { get; set; }
    public string DbServer { get; set; }
    public string DbConnection { get; set; }
    public string DbConnectionString => csAppConfig.ConfigurationRoot.GetConnectionString(DbConnection);

}
