# MyMicroservice

Brief description of purpose of microservice.

## How to run MS locally

### Prerequisites
* Perform minikube environment initialization, please refer to files sitting here:
	* https://github.com/BHFDigital/BHF.Microservices/tree/main/local-development/Install-minikube.ps1
	* https://github.com/BHFDigital/BHF.Microservices/tree/main/local-development/start-minikube.ps1
	* https://github.com/BHFDigital/BHF.Microservices/blob/main/README.md

### To run only this microservice manually
* Copy code\BHF.MS.MyMicroservice\yaml\configMap-development.yml.example as "configMap-development.yml"
* Fill in the inner variables with relevant secrets located inside "uks-local-dev-kv" KeyVault on Azure portal, secret names to look for:
	* DEV--ConnectionStrings--ClientId
	* DEV--ConnectionStrings--TenantId
	* DEV--ConnectionStrings--ClientSecret

### To run all microservices in bulk (includes configMap bulk auto population)
* Please refer to bulk deployment script sitting here:
	* https://github.com/BHFDigital/BHF.Microservices/tree/main/local-development/bulk-deploy.ps1
	* https://github.com/BHFDigital/BHF.Microservices/tree/main/local-development/bulk-deploy-variables.json.example
	* https://github.com/BHFDigital/BHF.Microservices/blob/main/README.md

## Further references

Links to related Confluence pages/related third-party documentation.