#!
testDirectorys="$(ls -p)"
for testDirectory in $testDirectorys
do
	if [[ $testDirectory =~ .*Tests/ ]]; then
		testDirectoryContents="$(ls ./$testDirectory)"
		for testDirectoryFile in $testDirectoryContents
		do
			if [[ $testDirectoryFile =~ .*.csproj ]]; then
				dotnet test "./$testDirectory$testDirectoryFile"
			fi
		done
	fi
done
