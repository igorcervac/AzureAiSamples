set "name=az-ai-snippets-translator"
set "group=%name%-rg"
set "location=eastus"

call az group create --name %group% --location %location%
call az cognitiveservices account create --name %name% --resource-group %group% --sku F0 --kind TextTranslation --location %location%
call az cognitiveservices account keys list --name %name% --resource-group %group% --output table
call az cognitiveservices account show  --name %name% --resource-group %group% --query "properties.endpoint"