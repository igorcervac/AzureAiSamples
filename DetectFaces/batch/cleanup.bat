set "name=az-ai-samples-facedetection"
set "group=%name%-rg"
set "location=eastus"
set "kind=Face"
set "sku=F0"

call az cognitiveservices account delete --name %name% --resource-group %group%
call az cognitiveservices account purge --name %name% --resource-group %group% --location %location%
call az group delete --name %group% --no-wait --yes