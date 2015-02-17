param($server, $port, $username, $password, $database, $restoreLocation)

echo "Attempting restore from " $restoreLocation

if ($username -and $password) { 
	C:\mongodb\bin\mongorestore -v -h $server --port $port -u $username -p $password -d $database -drop $restoreLocation
}
else {
	C:\mongodb\bin\mongorestore -v -h $server --port $port -d $database -drop $restoreLocation
}