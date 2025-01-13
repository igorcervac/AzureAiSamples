set "group=az-ai-snippets-formrecognizer-rg"
set "name=az-ai-snippets-formrecognizer"
set "location=eastus"
set "kind=FormRecognizer"
set "sku=F0"

call az cognitiveservices account delete --name %name% --resource-group %group%
call az cognitiveservices account purge --name %name% --resource-group %group% --location %location%
call az group delete --name %group% --no-wait --yes