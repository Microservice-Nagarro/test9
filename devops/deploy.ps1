$repositoryFolder = Split-Path -Path $MyInvocation.MyCommand.Path -Parent | Split-Path -Parent
$microserviceName = "mymicroservice-microservice"

docker build -t $microserviceName -f $repositoryFolder\code\BHF.MS.MyMicroservice\Dockerfile $repositoryFolder
$dockerImageId = (docker inspect $microserviceName -f '{{.Id}}') -replace "sha256:"
docker tag "$($microserviceName):latest" "$($microserviceName):$($dockerImageId)"

minikube image load "$($microserviceName):$($dockerImageId)"
kubectl create ns bhf-microservices

$deploymentContent = Get-Content $repositoryFolder\code\BHF.MS.MyMicroservice\yaml\deployment-development.yml
$deploymentContent = $deploymentContent -replace "#{BuildRef}#", $dockerImageId

kubectl apply -f $repositoryFolder\code\BHF.MS.MyMicroservice\yaml\configMap-development.yml -n bhf-microservices
$deploymentContent | kubectl apply -n bhf-microservices -f -

Write-Output "Waiting for $microserviceName to start"
$waitResult = & kubectl wait pod -l app=$microserviceName --for=condition=Ready -n bhf-microservices --timeout=70s 2>&1
if($LASTEXITCODE -eq 0)
{
	Start-Job { kubectl port-forward svc/$microserviceName 8082:8080 -n bhf-microservices } | Out-Null # for entry like 8082:80 - switch left side (8082) to unique port across all the other microservices
	Write-Output "You can access $microserviceName via http://localhost:8082"
}
else
{
	Write-Error "$microserviceName failed to start, error: $waitResult"
}
