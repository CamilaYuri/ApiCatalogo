pipeline {
    agent any

    environment {
        DOTNET_ROOT = "/usr/share/dotnet"
        PATH = "${DOTNET_ROOT}:${PATH}"
    }

    stages {
        stage('Restore') {
            steps {
                echo 'Restaurando pacotes NuGet...'
                sh 'dotnet restore APICatalogo/APICatalogo.csproj'
            }
        }

        stage('Build') {
            steps {
                echo 'Construindo projeto...'
                sh 'dotnet build APICatalogo/APICatalogo.csproj --no-restore -c Release'
            }
        }

        stage('Unit Tests') {
            steps {
                echo 'Rodando testes unitarios...'
                sh 'dotnet test ApiCatalogoxUnitTests/ApiCatalogoxUnitTests.csproj --verbosity normal'
            }
        }

        stage('Integration Tests') {
            steps {
                script {
                    def networkName = 'my-app-network'
                    def mysqlContainerName = 'mysql-test'
                    
                    echo "Criando rede Docker '${networkName}' para testes..."
                    sh "docker network create ${networkName} || true"

                    try {
                        echo "Iniciando conteiner MySQL chamado '${mysqlContainerName}'..."
                        sh "docker run --rm --detach --name ${mysqlContainerName} --env MYSQL_ROOT_PASSWORD=SecretPassword01 --network ${networkName} mysql:8.0"

                        echo 'Conteiner MySQL iniciado. Aguardando para que ele esteja totalmente pronto para conexoes...'
                        sh """
                            docker run --rm --network ${networkName} busybox:latest sh -c 'until nc -z ${mysqlContainerName} 3306; do echo waiting for mysql; sleep 2; done;'
                        """

                        echo 'Rodando testes de integracao...'
                        
                        def connectionString = "Server=${mysqlContainerName};Port=3306;Database=db_test;Uid=root;Pwd=SecretPassword01;"

                        sh """
                            docker run --rm \\
                            --network ${networkName} \\
                            --env ConnectionStrings:DefaultConnection="${connectionString}" \\
                            --volume "/c/dev/APICatalogo:/app" \\
                            mcr.microsoft.com/dotnet/sdk:8.0 \\
                            sh -c "cd /app && dotnet test ApiCatalogo.IntegrationTests/ApiCatalogo.IntegrationTests.csproj --verbosity normal"
                        """

                    } finally {
                        echo 'Limpando recursos Docker...'
                        sh "docker stop ${mysqlContainerName} || true"
                        sh "docker network rm ${networkName} || true"
                    }
                }
            }
        }

        stage('Publish') {
            steps {
                echo 'Publicando a API...'
                sh 'dotnet publish APICatalogo/APICatalogo.csproj -c Release -o ./publish'
            }
        }
    }

    post {
        success {
            echo 'Pipeline finalizada com sucesso!'
        }
        failure {
            echo 'Pipeline falhou. Verifique os logs.'
        }
    }
}
