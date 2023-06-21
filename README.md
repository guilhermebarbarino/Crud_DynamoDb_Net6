# CRUD com DynamoDB no ASP.NET Core – Conceitos básicos do AWS DynamoDB simplificado
Veremos os fundamentos do DynamoDB, criando tabelas e chaves de partição por meio do Console AWS, usando o pacote AWS DynamoDB Nuget e conectando-se às tabelas de nosso aplicativo .NET 6, realizando operações básicas de CRUD e assim por diante.

# Sobre o AWS DynamoDB
.O AWS Dynamo DB é um serviço de banco de dados altamente disponível e totalmente gerenciado fornecido pela Amazon Web Services. É um banco de dados NoSQL, o que significa que não é um banco de dados relacional. Também é uma parte importante e principal de todo o modelo de aplicativo sem servidor da AWS. Como é autoescalável, ele pode lidar com milhões de solicitações por segundo e armazenar TBs de dados em suas linhas. Bem interessante, certo? A velocidade com que a recuperação de dados acontece também é bastante impressionante, a latência é bem pequena. Fora isso, ele é integrado ao AWS IAM para segurança (como qualquer outro serviço da AWS).
Sendo um banco de dados compatível com NoSQL, é um banco de dados de chave/valor. Chaves primárias são geralmente compostas por 1 ou 2 chaves. Outras propriedades (colunas) da tabela do DynamoDB são conhecidas internamente como atributos.

# O que vamos construir?
.Portanto, criaremos uma API WEB .NET 6 simples que pode se conectar ao nosso AWS DynamoDB com base em determinadas configurações que definimos por meio da AWS CLI local e executar algumas operações básicas de CRUD em uma entidade. Nesta demonstração, nossa entidade será Student, com propriedades como Id, Name, Class, Country, apenas as básicas. E escreveremos o código para criar, obter um único aluno, obter todos os alunos, excluir e atualizar os alunos.

Com isso claro, vamos começar!

# Console AWS – Dynamo DB
.Presumo que você já tenha uma conta AWS (de preferência uma conta GRATUITA que é mais que suficiente para nosso aprendizado). Vamos fazer alguns exercícios práticos com o Amazon DynamoDB para conhecê-lo melhor!

Faça login no seu Console AWS, procure por DynamoDB na barra de pesquisa superior de Serviços e abra-o.
Em seguida, clique em Tabelas ou Criar tabela.


![dinamo](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/778da860-0fe0-4202-8fdb-53f8a9df1b10)

# Criando uma nova tabela no DynamoDB via console
Conforme discutido anteriormente, teremos que criar uma nova tabela chamada Students . Clique em “Criar Tabela”.

![dinamo2](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/ff5f4930-624c-40fd-af17-f93fe5c9ed62)

Aqui, defina o nome da tabela como “ alunos ” e a chave de partição como “id” (número). Portanto, esta será a chave primária da tabela “ alunos” . Observe também que, usando a chave de classificação, podemos flexibilizar ainda mais a formação da chave primária. Isso é mais como um primário composto que é formado pela combinação da chave de partição e da chave de classificação.

Por enquanto, não estamos definindo nenhuma chave de classificação. Deixe os outros campos como estão e crie a tabela.

![dinamo3](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/efde3b7b-f9ca-4905-87ae-4f348ae9ab73)

Isso é tudo. Pode levar alguns segundos para provisionar completamente sua tabela para o DynamoDB. Depois de concluído, o status da sua mesa muda para Ativo.

![dinamo4](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/c7afadb6-7043-43b9-8d12-233eee6e17be)

# Adicionar dados ao AWS DynamoDB por meio do console

Depois que a tabela for provisionada, vamos adicionar alguns dados a ela por meio do console. Clique na mesa e clique em “Explorar itens da mesa”.

![dinamo5](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/a4ad9aff-2132-4537-af18-53f4da632c88)

Portanto, é aqui que você pode consultar os dados desta tabela (se houver algum dado). A interface do usuário parece bastante intuitiva para trabalhar com dados. Clique em “Criar item”.

![dinamo6](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/61bd4deb-e714-41a1-a0ee-aac51a9485c1)

Agora, aqui, o AWS DynamoDB permite definir dados em formatos JSON ou adicionar dados como campos de formulário. Clique na guia JSON para alternar para a exibição a seguir. Aqui, adiciono propriedades como Id, Nome, Sobrenome, Classe e País.

![dinamo7](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/391d3fe4-1815-4813-be21-799c122baf81)

Você pode voltar para a exibição de formulário para a aparência abaixo. Eu prefiro usar a visualização JSON.

![dinamo8](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/dfdcb9ca-cefe-4cbb-90ef-4ba72248a16c)

Depois de inserir os valores e clicar em criar, o AWS DynamoDB insere o item na tabela de alunos, como você pode ver abaixo.

![dinamo9](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/f3cc7ef6-5b7c-4c9b-8574-b31cf0be24cc)

É assim que funciona a interface do DynamoDB. Vamos escrever algum código agora.


# Introdução – CRUD com DynamoDB no ASP.NET Core

Agora que temos uma prática básica com o AWS DynamoDB, vamos criar uma API da Web que possa consumir esse serviço da AWS.

Abra seu Visual Studio IDE (estou usando o Visual Studio 2022 Community com a última iteração do .NET SDK instalada, que é 6.0.2). Certifique-se de que seu ambiente de desenvolvimento esteja configurado.

Crie um novo projeto de API da Web ASP.NET Core e nomeie-o como abaixo.

![dinamo11](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/a202e5d5-3105-4c57-994e-3ba2d92127ab)

- Certifique-se de selecionar .NET 6 e marque a caixa de seleção “ Usar controladores ”. Além disso, certifique-se de ter ativado o suporte OpenAPI, pois testaremos nosso aplicativo via Swagger.
- 
![c1](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/d1199c92-b7c8-46f0-a44a-ac5134d07f6d)

Agora que você tem o projeto pronto, vamos falar sobre como configurar as credenciais da AWS.

# Configuração da AWS CLI
. precisa instalar o sdk da aws cli na sua maquina.

Navegue até AWS IAM para criar seu usuário que tem acesso ao Dynamo DB. Para criar um novo usuário no AWS IAM, vá no console da aws e pesquise IAM, depois clique em criar usuario, ao chegar no passo de permissões , Certifique-se de ter as permissões de leitura, gravação e exclusão incluídas na política selecionada. Aqui estão as políticas relacionadas ao DynamoDB.

![aws](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/0d9a4626-c622-4135-aba1-e8e263ff6af8)

Você pode escolher AmazonDynamoDBFullAccess por enquanto. Mas lembre-se sempre de selecionar apenas o que é realmente necessário para sua aplicação.

Depois de criar seu usuário, clique no usuario criado , depois vai na aba de credenciais de segurança e gere uma chave de acesso nova. Certifique-se de baixar este CSV gerado por segurança.

# Abra seu cmd
 digite o seguite comando 
 . AWS CONFIGURE --PROFILE NOME-DO-SEU-USUARIO

 EXEMPLO.
![cmd](https://github.com/guilhermebarbarino/Crud_DynamoDb_Net6/assets/74333587/d3e34650-7706-4bba-8c8a-87f7a9d48eac)

Então, agora que configuramos as credenciais da AWS, vamos abrir nosso appsettings.json no projeto e fazer as modificações conforme abaixo.

Observe que meu nome de perfil é aws-admin e a região da AWS mais próxima de mim é ap-south-1. Faça as alterações de acordo.

-CODIGO
-------------------------------------------------------
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AWS": {
    "Profile": "aws-admin",
    "Region": "ap-south-1"
  }
}
 
------------------------------------------------------------------------------------------------------

- Feito isso, o AWS SDK poderá ler o nome do perfil do appsettings.json e extrair os detalhes da credencial configurada, e utilizá-la durante o tempo de execução de nossa aplicação

  # Criando o modelo do aluno

  Lembra que criamos uma tabela no AWS DynamoDB com determinadas propriedades? Precisamos construir a mesma classe de modelo em nosso projeto .NET também.

Crie uma nova pasta chamada Models e crie uma classe dentro dela chamada Student.cs


~~~C#
Esta é uma linha de código em C#.

using Amazon.DynamoDBv2.DataModel;
namespace DynamoStudentManager.Models
{
    [DynamoDBTable("students")]
    public class Student
    {
        [DynamoDBHashKey("id")]
        public int? Id { get; set; }

        [DynamoDBProperty("first_name")]
        public string? FirstName { get; set; }

        [DynamoDBProperty("last_name")]
        public string? LastName { get; set; }

        [DynamoDBProperty("class")]
        public int Class { get; set; }

        [DynamoDBProperty("country")]
        public string? Country { get; set; }
    }
}
~~~

  Observe que nomeamos nossa tabela do DynamoDB como “students”, mas o nome da nossa classe é “Student” (pode até ser algo totalmente diferente). Portanto, para que o AWS SDK entenda que ambos são essencialmente os mesmos modelos, precisamos mapeá-lo.

  - Linha 4: Aqui informamos ao SDK que a tabela DynamoDB “ students” deve ser mapeada para a classe C# “ Student ”.

  - Linha 7: DynamoDBHashKey define a chave primária da tabela. Aqui, indicamos que Id será nossa chave primária com o nome da propriedade como “id” na tabela DDB.

  - Linha 10,13,16,19: Definimos outras propriedades da tabela junto com seus respectivos nomes de propriedades DDB.

    # Registro de serviço

    Com os modelos configurados, vamos registrar os Serviços da AWS e a configuração dentro do contêiner ASP.NET Core. Abra o Program.cs e as seguintes linhas destacadas.

~~~~
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
var app = builder.Build();

~~~~

Isso basicamente carrega a configuração da AWS do arquivo appsettings.json e registra os serviços da AWS relacionados ao DynamoDB no contêiner do aplicativo.

# Controlador Aluno

Em seguida, vamos escrever o controlador de API que realmente executará as operações relacionadas ao CRUD com o DynamoDB no ASP.NET Core. Crie um novo Blank API Controller na pasta Controllers e o nome seja StudentController.

~~~~

namespace DynamoStudentManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IDynamoDBContext _context;
    public StudentsController(IDynamoDBContext context)
    {
        _context = context;
    }
    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetById(int studentId)
    {
        var student = await _context.LoadAsync<Student>(studentId);
        if (student == null) return NotFound();
        return Ok(student);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var student = await _context.ScanAsync<Student>(default).GetRemainingAsync();
        return Ok(student);
    }
    [HttpPost]
    public async Task<IActionResult> CreateStudent(Student studentRequest)
    {
        var student = await _context.LoadAsync<Student>(studentRequest.Id);
        if (student != null) return BadRequest($"Student with Id {studentRequest.Id} Already Exists");
        await _context.SaveAsync(studentRequest);
        return Ok(studentRequest);
    }
    [HttpDelete("{studentId}")]
    public async Task<IActionResult> DeleteStudent(int studentId)
    {
        var student = await _context.LoadAsync<Student>(studentId);
        if (student == null) return NotFound();
        await _context.DeleteAsync(student);
        return NoContent();
    }
    [HttpPut]
    public async Task<IActionResult> UpdateStudent(Student studentRequest)
    {
        var student = await _context.LoadAsync<Student>(studentRequest.Id);
        if (student == null) return NotFound();
        await _context.SaveAsync(studentRequest);
        return Ok(studentRequest);
    }
}

~~~~

Linha 7-11: Aqui, injetamos o IDynamoDBContext no construtor do controlador. Usando esta interface, poderemos acessar o AWS DynamoDB.


Linha 13-18: Obtém itens por Id (que é nossa chave primária aqui). Na linha 15, carregamos a tabela Student (que é mapeada para “students”) e passamos o StudentId recebido como HashKey para o DynamoDB. Isso nos retorna o registro do Aluno mapeado, se existir.

Linha 20-24: Obtenha todos os registros de alunos da tabela DDB.

Linha 26-32: Aqui, passamos um registro Student para o endpoint que então tenta criar um novo registro no DDB com os detalhes passados. Ele primeiro verifica se o ID passado já existe no banco de dados. Se encontrado, retorna uma exceção de solicitação inválida. Caso contrário, o registro do aluno será criado conforme o esperado.

Linha 34-40: Passamos um ID de aluno para este endpoint Delete. Ele primeiro verifica se tal registro existe com o ID passado. Se não for encontrado, o aplicativo lançará uma exceção NotFound e interromperá. Caso contrário, ele excluirá o item de nossa tabela do DynamoDB.

Linha 42-48: Aqui, tentamos atualizar o registro do aluno, passando todas as propriedades como tal para o terminal. Primeiro, verificamos se o ID do aluno existe e, em seguida, continuamos a atualizar o item. Tão simples como isso.

# Hora de testar

- Com os controladores conectados, vamos testar nosso aplicativo. Como ativamos o suporte OpenAPI para nossa API, o Swagger estaria funcionando imediatamente, sem nenhuma configuração adicional necessária.

Execute o aplicativo e navegue até localhost:xxxx/swagger em seu navegador da web. Isso deve abrir a IU do Swagger.

# obs
- o projeto no github eu fiz do meu gosto não está exatamente como o tutorial.














