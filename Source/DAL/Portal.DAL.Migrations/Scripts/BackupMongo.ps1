param($server, $port, $username, $password, $database, $backupLocation)

# Backup location is base\server\timestamp
$backupLocation = join-path (join-path $backupLocation $server)  $(get-date -f yyyy_MM_dd_HH_mm_ss)

# Backup current database
if ($username -and $password) { 
	C:\mongodb\bin\mongodump -h $server --port $port -u $username -p $password -d $database -o $backupLocation
}
else {
	C:\mongodb\bin\mongodump -h $server --port $port -d $database -o $backupLocation
}