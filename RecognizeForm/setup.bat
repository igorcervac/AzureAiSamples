set "group=az-ai-snippets-formrecognizer-rg"
set "name=az-ai-snippets-formrecognizer"
set "location=eastus"
set "kind=FormRecognizer"
set "sku=F0"

call az group create --name %group% --location %location%
call az cognitiveservices account create --name %name% --resource-group %group% --sku %sku% --kind %kind% --location %location%
call az cognitiveservices account keys list --name %name% --resource-group %group% --output table
call az cognitiveservices account show  --name %name% --resource-group %group% --query "properties.endpoint"