var model = args[0];
var alias = args[1];
var name = args[2];
var folder = "";

if (args.Length > 3) 
    folder = args[3];

var root = @"../../src";
var templateRoot = @"Templates";
var folderByPath = string.IsNullOrEmpty(folder) ? model : folder;

if (!string.IsNullOrEmpty(model))
{
    Generate("RequestDto.tpl", $"{root}/Api.Domain/Dtos/{folderByPath}/{model}RequestDto.cs");
    Generate("ResponseDto.tpl", $"{root}/Api.Domain/Dtos/{folderByPath}/{model}ResponseDto.cs");
    Generate("Model.tpl", $"{root}/Api.Domain/Models/{model}Model.cs");
    Generate("Entity.tpl", $"{root}/Api.Domain/Entities/{model}Entity.cs");
    Generate("IService.tpl", $"{root}/Api.Domain/Interfaces/Services/I{model}Service.cs");
    Generate("IRepository.tpl", $"{root}/Api.Domain/Repository/I{model}Repository.cs");
    Generate("Map.tpl", $"{root}/Api.Data/Mapping/{model}Map.cs");
    Generate("Repository.tpl", $"{root}/Api.Data/Repository/{model}Repository.cs");
    Generate("Service.tpl", $"{root}/Api.Service/Services/{model}Service.cs");
    Generate("Controller.tpl", $"{root}/Api.Application/V1/Controllers/{model}Controller.cs");
//Testes
    Generate("DataTest/CrudComplete.tpl", $"{root}/Api.Data.Test/{folderByPath}/{model}CrudComplete.cs");
    Generate("DataTest/ExecuteGetAll.tpl", $"{root}/Api.Data.Test/{folderByPath}/{model}ExecuteGetAll.cs");

    Generate("ServiceTest/Mapper.tpl", $"{root}/Api.Service.Test/AutoMapper/{model}Mapper.cs");
    
    var completeFolder = string.IsNullOrEmpty(folder) ? folderByPath : $"{folderByPath}/{model}";
    Generate("ServiceTest/Test.tpl", $"{root}/Api.Service.Test/{completeFolder}/{model}Test.cs");
    Generate("ServiceTest/WhenExecuteCreate.tpl",
        $"{root}/Api.Service.Test/{completeFolder}/WhenExecuteCreate.cs");
    Generate("ServiceTest/WhenExecuteDelete.tpl",
        $"{root}/Api.Service.Test/{completeFolder}/WhenExecuteDelete.cs");
    Generate("ServiceTest/WhenExecuteGet.tpl", $"{root}/Api.Service.Test/{completeFolder}/WhenExecuteGet.cs");
    Generate("ServiceTest/WhenExecuteUpdate.tpl",
        $"{root}/Api.Service.Test/{completeFolder}/WhenExecuteUpdate.cs");

    Generate("ApplicationTest/Mapper.tpl", $"{root}/Api.Application.Test/AutoMapper/{model}Mapper.cs");
    
    Generate("ApplicationTest/BaseTest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/BaseTest{model}.cs");
    Generate("ApplicationTest/WhenRequestCreate/ReturnBadRequest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestCreate/ReturnBadRequest.cs");
    Generate("ApplicationTest/WhenRequestCreate/ReturnCreated.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestCreate/ReturnCreated.cs");
    Generate("ApplicationTest/WhenRequestDelete/ReturnBadRequest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestDelete/ReturnBadRequest.cs");
    Generate("ApplicationTest/WhenRequestDelete/ReturnDeleted.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestDelete/ReturnDeleted.cs");
    Generate("ApplicationTest/WhenRequestGet/ReturnBadRequest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestGet/ReturnBadRequest.cs");
    Generate("ApplicationTest/WhenRequestGet/ReturnRequestGet.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestGet/ReturnRequestGet.cs");
    Generate("ApplicationTest/WhenRequestGetAll/ReturnBadRequest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestGetAll/ReturnBadRequest.cs");
    Generate("ApplicationTest/WhenRequestGetAll/ReturnRequestGetAll.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestGetAll/ReturnRequestGetAll.cs");
    Generate("ApplicationTest/WhenRequestUpdate/ReturnBadRequest.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestUpdate/ReturnBadRequest.cs");
    Generate("ApplicationTest/WhenRequestUpdate/ReturnUpdated.tpl",
        $"{root}/Api.Application.Test/{completeFolder}/WhenRequestUpdate/ReturnUpdated.cs");

    Generate("IntegrationTest/BaseTest.tpl", $"{root}/Api.Integration.Test/{folderByPath}/BaseTest{model}.cs");
    Generate("IntegrationTest/WhenRequest.tpl", $"{root}/Api.Integration.Test/{folderByPath}/WhenRequest{model}.cs");
}

void Generate(string template, string output)
{
    var content = File.ReadAllText($"{templateRoot}/{template}");

    if (string.IsNullOrEmpty(content))
        return;
    
    content = content.Replace("{{model}}", model);
    content = content.Replace("{{alias}}", alias);
    content = content.Replace("{{name}}", name);
    
    if (string.IsNullOrEmpty(folder))
        content = content.Replace(".{{folder}}", "");
    else
        content = content.Replace("{{folder}}", folder);
    
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    
    if(!File.Exists(output))
        File.WriteAllText(output, content);
}