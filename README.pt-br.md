# API de Avaliação de Risco de Trades

![.NET Core](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Cobertura de Testes](https://img.shields.io/badge/Cobertura-100%25-brightgreen?style=for-the-badge)
![Clean Architecture](https://img.shields.io/badge/Arquitetura-Clean-blue?style=for-the-badge)

Uma API REST de nível corporativo e altamente otimizada, construída em **.NET 8** para avaliar e categorizar operações financeiras (Trades) com base em critérios de risco.

Este projeto foi desenhado com forte foco em **Clean Architecture**.

## 🚀 Tecnologias e Bibliotecas
* **.NET 8** (C# 12)
* **MediatR** (Padrão CQRS e Pipeline Behaviors)
* **FluentValidation** (Validação Fail-Fast)
* **Swagger** (Documentação da API)
* **PLINQ** (Parallel LINQ para processamento de alta performance)

## 🏗️ Arquitetura e Padrões de Projeto

Esta aplicação vai além da arquitetura em camadas tradicional, combinando **Clean Architecture** com **Vertical Slice Architecture** (Fatias Verticais) na camada de Aplicação.

### 1. Vertical Slices e CQRS
A camada de aplicação é organizada por **Features** (Casos de Uso) em vez de pastas técnicas. Cada feature (ex: `CalculateRisk`, `CalculateRiskDistribution`) encapsula seu próprio Request, Command, Handler e Validator. Isso garante **Alta Coesão** e aderência estrita ao Princípio da Responsabilidade Única (SRP).

### 2. Padrão Strategy (Camada de Domínio)
A lógica de negócio central é completamente isolada. As regras de avaliação de risco respeitam o **Princípio do Aberto/Fechado (OCP)**: novas categorias de risco podem ser adicionadas simplesmente criando uma nova classe de regra, sem modificar o motor de avaliação existente.

### 3. Validação Fail-Fast (Pipeline Behaviors)
A validação de entrada é tratada de forma elegante usando **FluentValidation** acoplado a um **Pipeline Behavior do MediatR**. Requisições inválidas são interceptadas e rejeitadas *antes* mesmo de chegarem à regra de negócio, mantendo os Handlers limpos e focados apenas na orquestração.

### 4. Tratamento Global de Exceções
Utilizando o novo `IExceptionHandler` do .NET 8, todas as exceções não tratadas e erros de validação são interceptados globalmente e formatados de acordo com o padrão **RFC 7807 Problem Details**, evitando o vazamento de stack traces e garantindo respostas consistentes na API.

## ⚙️ Como Executar

1. Certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
2. Clone o repositório e navegue até a pasta do projeto da API:
   ```bash
   cd MeuApp.Api
3. Execute a aplicação:
    ```bash
    dotnet run
4. Abra seu navegador e acesse o Swagger UI para testar os endpoints:
http://localhost:5000/swagger

## 📡 Endpoints da API
**POST** `/api/v1/trades/calculate-risk`: Avalia uma lista de operações e retorna um array de categorias de risco.

**POST** `/api/v1/trades/distribution`: Avalia um lote de operações (até 100k) e retorna um resumo estatístico detalhado agrupado por categoria de risco, incluindo o cliente de maior exposição.
