set "name=az-ai-snippets-translator"
set "group=%name%-rg"
set "location=eastus"

call az cognitiveservices account delete --name %name% --resource-group %group%
call az cognitiveservices account purge --name %name% --resource-group %group% --location %location%
call az group delete --name %group% --no-wait --yes