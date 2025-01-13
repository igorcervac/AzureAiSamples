set "group=az-ai-snippets-translator-rg"
set "name=az-ai-snippets-translator"
set "location=eastus"

call az cognitiveservices account delete --name %name% --resource-group %group%
call az cognitiveservices account purge --name %name% --resource-group %group% --location %location%
call az group delete --name %group% --no-wait --yes