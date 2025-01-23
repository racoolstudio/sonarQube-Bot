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
      						export PATH="$PATH:$HOME/.dotnet/tools"
						dotnet-sonarscanner begin /k:"DummyProject_$BRANCH_NAME" /d:sonar.host.url="http://{SonarQube Url}/" /d:sonar.token="$SONAR_TOKEN" /d:sonar.scanner.scanAll="True" 
						dotnet build ./dummyProject/dummyProject.csproj
						dotnet-sonarscanner end /d:sonar.token="$SONAR_TOKEN"

"""
					}

					
			}
			
			}
		
		}
		
	
}
