﻿// Copyright (c) 2023 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel;
using AuthPermissions.AspNetCore.ShardingServices;
using StatusGeneric;

namespace Test.StubClasses;

public class StubSqliteDatabaseSpecificMethods : IDatabaseSpecificMethods
{
    /// <summary>
    /// This contains the short name of Database Provider the service supports
    /// </summary>
    public string DatabaseProviderShortName => "Sqlite";

    /// <summary>
    /// This changes the database to the <see cref="DatabaseInformation.DatabaseName"/> in the given connectionString
    /// NOTE: If the <see cref="DatabaseInformation.DatabaseName"/> is null / empty, then it returns the connectionString with no change
    /// </summary>
    /// <param name="databaseInformation">Information about the database type/name to be used in the connection string</param>
    /// <param name="connectionString"></param>
    /// <returns>A connection string containing the correct database to be used, or errors</returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public string SetDatabaseInConnectionString(DatabaseInformation databaseInformation, string connectionString)
    {
        return $"Data Source={databaseInformation.DatabaseName}.sqlite";
    }

    /// <summary>
    /// This will execute the function (which updates the shardingsettings json file) within a Distributed Lock. 
    /// Typically this will use a lock on the database provider.  
    /// </summary>
    /// <param name="connectionString">connection string to the database to place a Distributed Lock on</param>
    /// <param name="runInLock"></param>
    /// <returns></returns>
    public IStatusGeneric ChangeDatabaseInformationWithinDistributedLock(string connectionString, Func<IStatusGeneric> runInLock)
    {
        runInLock.Invoke();
        return new StatusGenericHandler();
    }

    /// <summary>
    /// This will execute the function (which updates the shardingsettings json file) within a Distributed Lock. 
    /// Typically this will use a lock on the database provider.  
    /// </summary>
    /// <param name="connectionString">connection string to the database to place a Distributed Lock on</param>
    /// <param name="runInLockAsync"></param>
    /// <returns></returns>
    public async Task<IStatusGeneric> ChangeDatabaseInformationWithinDistributedLockAsync(string connectionString, Func<Task<IStatusGeneric>> runInLockAsync)
    {
        await runInLockAsync.Invoke();
        return new StatusGenericHandler();
    }
}