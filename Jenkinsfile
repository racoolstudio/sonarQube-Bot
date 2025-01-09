pipeline{
	agent any 
	environment{
		SONAR_TOKEN = credentials('sonar')

	}
	stages{
		stage("Building and SonarQube"){
			steps{
				echo "Building"
    				sh "ls"
				  withSonarQubeEnv('SonarMaster'){
						sh """
						dotnet-sonarscanner begin /k:"DummyProject_$BRANCH_NAME" /d:sonar.host.url="http://172.105.20.48/" /d:sonar.token="$SONAR_TOKEN" /d:sonar.scanner.scanAll="True" 
						dotnet build ./dummyProject/dummyProject.csproj
						dotnet-sonarscanner end /d:sonar.token="$SONAR_TOKEN"

"""
					}

					
			}
			
			}
		
		}
		
	
}
