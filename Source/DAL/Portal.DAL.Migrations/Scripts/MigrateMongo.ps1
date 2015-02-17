param($server, $port, $username, $password, $database, $migrationsAssemblyPath)

# Load assembly with migrations and MongoMigrations framework assembly
$migrationsAssembly = [System.Reflection.Assembly]::LoadFrom($migrationsAssemblyPath)
$migrationFrameworkAssemblyPath = join-path ([IO.Path]::GetDirectoryName($migrationsAssemblyPath)) 'MongoMigrations.dll'
[System.Reflection.Assembly]::LoadFrom($migrationFrameworkAssemblyPath)

# Create migration runner and load migrations
$connectionString = 'mongodb://' + $server + ':' + $port + '/' + $database
if ($username -and $password) { 
	$connectionString = 'mongodb://' + $username + ':' + $password + '@' + $server + ':' + $port + '/' + $database
}
$runner = new-object MongoMigrations.MigrationRunner($connectionString, $database)
$runner.MigrationLocator.LookForMigrationsInAssembly($migrationsAssembly)

Try
{ 
    $runner.UpdateToLatest()
}
Catch [MongoMigrations.MigrationException]{
    echo "Migrations failed: "
    Write-Host $_.Exception.ToString()
    throw
}