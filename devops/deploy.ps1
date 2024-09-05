$repositoryFolder = Split-Path -Path $MyInvocation.MyCommand.Path -Parent | Split-Path -Parent
$microserviceName = "mymicroservice-microservice"
$port = 8082 #switch it to unique port across all the other microservices
$url = "http://localhost:$port/swagger"
$microserviceDockerfile = "$repositoryFolder\code\BHF.MS.MyMicroservice\Dockerfile"
$microserviceDockerfileContext = $repositoryFolder
$microserviceYamlFolder = "$repositoryFolder\code\BHF.MS.MyMicroservice\yaml"

docker build -t $microserviceName -f $microserviceDockerfile $microserviceDockerfileContext
$dockerImageId = (docker inspect $microserviceName -f '{{.Id}}') -replace "sha256:"
docker tag "$($microserviceName):latest" "$($microserviceName):$($dockerImageId)"

minikube image load --overwrite=false "$($microserviceName):$($dockerImageId)"
kubectl create ns bhf-microservices

$deploymentContent = Get-Content $microserviceYamlFolder\deployment-development.yml
$deploymentContent = $deploymentContent -replace "#{BuildRef}#", $dockerImageId

kubectl apply -f $microserviceYamlFolder\configMap-development.yml -n bhf-microservices
$deploymentContent | kubectl apply -n bhf-microservices -f -

Start-Job -InputObject @("services/$microserviceName", "$($port):8080") -ScriptBlock { kubectl port-forward $($input[0]) $($input[1]) -n bhf-microservices } | Out-Null
Write-Output "You can access $microserviceName via $url"