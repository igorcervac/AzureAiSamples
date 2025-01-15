set "name=az-ai-samples-objectdetection"
set "group=%name%-rg"
set "location=eastus"
set "kind=ComputerVision"
set "sku=F0"

call az cognitiveservices account delete --name %name% --resource-group %group%
call az cognitiveservices account purge --name %name% --resource-group %group% --location %location%
call az group delete --name %group% --no-wait --yes