# Wallet API

## 📑 Sumário

- [🧱 Arquitetura](#arquitetura)
- [🛠️ Tecnologias](#tecnologias)
- [📌 Regras de Negócio](#regras-de-negocio)
- [🚀 Como rodar o projeto](#como-rodar-o-projeto)
- [🔧 Pontos de melhoria](#pontos-de-melhoria)

<a id="arquitetura"></a>
## 🧱 Arquitetura
  Para a estrutura macro do projeto,  tomei como base o Clean Architecture, separando minha aplicação por 
  domínios, aplicando esse conceito na divisão dos arquivos. A imagem abaixo é para demonstrar os principais
  domínios que mapeei.

![image](https://github.com/user-attachments/assets/187380f1-372f-4db3-8783-3befa890d815)

Essa é uma representa a estrutura de pastas seguindo esse viés 
```text
Src/
├── Application/
│   ├── Client/
│   ├── Exceptions/
│   ├── Middleware/
│   └── Service/
├── Domain/
│   ├── Entity/
│   └── Enums/
├── Infrastructure/
│   └── Configuration/
│      
└── Presentation/
    ├── Controller/
    ├── DTO/
    └── Mapper/
```
  
O domínio da minha aplicação é composto por 3 entidades **User**, **Wallet** e **Transaction**
A imagem abaixo representa o relacionamento entre as entidades

![image](https://github.com/user-attachments/assets/bb230431-063d-4856-af6d-72b0a33a4ff3)

<a id="tecnologias"></a>
## 🛠️ Tecnologias
- .NET(8) com C#
- Keycloak (para o gerenciamento de usuários)

  - Keycloak é uma ferramenta de autenticação e autorização de código aberto, desenvolvida
    originalmente pela Red Hat, que permite adicionar facilmente funcionalidades de login,
    controle de acesso e gerenciamento de usuários em aplicações modernas.

    ⚙️ Casos de uso comuns:

    Proteger APIs REST (com tokens JWT)

    Aplicações web com login via Keycloak

    Microserviços que precisam compartilhar autenticação centralizada

    Portais com vários sistemas acessados via login único (SSO)
- Docker
- PostgreSQL

<a id="regras-de-negocio"></a>
## 📌 Regras de Negócio
Nesta seção vou elucidar as principais regras de negócio da aplicação
- Usuários só podem ser criados por usuários **ADMIN**
  - O usuário ADMIN default da aplicação tem as credenciais email: **admin@provider.com** senha: **admin**
    a partir dele você pode criar outros usuarios. No endpoint de criação basta colocar qual o tipo de usuário
    **"userType": "ADMIN"** ou **"userType": "USER"**
    
- Apenas usuários administradores podem adicionar saldo na carteira de outros usuários,
  isso impede que usuários adicionem saldo a sua própria carteira.

<a id="como-rodar-o-projeto"></a>
## 🚀 Como rodar o projeto
Requisitos:
  
  Ter o docker instalado na maquina

Passo a passo:

1. Clonar repositório
2. Abrir um terminal na pasta `PASTA_PESSOAL/WalletApi/WalletApi`
3. Executar o comando `docker-compose up -d`
  



