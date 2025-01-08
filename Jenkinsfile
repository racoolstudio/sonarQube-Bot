pipeline{
	agent any 
	stages{
		stage("Building"){
			steps{
				echo "Building"
    				sh "ls"
                                sh "dotnet build ./dummyProject/dummyProject.csproj"
					
			}
			
			}
		
		}
	 post {
	        success {
	            script {
	                githubNotify status: 'SUCCESS', description: 'Build succeeded', context: 'ci/jenkins',  credentialsId: 'pas-git'
	            }
	        }
	        failure {
	            script {
	                githubNotify status: 'FAILURE', description: 'Build failed', context: 'ci/jenkins'
	            }
	        }
    	}				
	
}
