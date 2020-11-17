
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/28/2020 14:12:07
-- Generated from EDMX file: C:\Users\Alexandre\Documents\Git\connector\PlataformaWeb\ConnectorSolution\Models\ConnectorDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [inequil];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_atividade_empresa]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[LogAtividade] DROP CONSTRAINT [FK_ctrain_atividade_empresa];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_alert_empresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ColetorAlerta] DROP CONSTRAINT [FK_ctrain_coletor_alert_empresa];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_alerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ColetorAlerta] DROP CONSTRAINT [FK_ctrain_coletor_alerta];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_empresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coletor] DROP CONSTRAINT [FK_ctrain_coletor_empresa];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_maquina]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coletor] DROP CONSTRAINT [FK_ctrain_coletor_maquina];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_coletor_pressao_hist]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[ColetorPressaoHistorico] DROP CONSTRAINT [FK_ctrain_coletor_pressao_hist];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_coletor_producao_hist]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[ColetorProducaoHistorico] DROP CONSTRAINT [FK_ctrain_coletor_producao_hist];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_coletor_temperatura_hist]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[ColetorTemperaturaHistorico] DROP CONSTRAINT [FK_ctrain_coletor_temperatura_hist];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_tipoalerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ColetorAlerta] DROP CONSTRAINT [FK_ctrain_coletor_tipoalerta];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_coletor_tp_alert_empresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ColetorTipoAlerta] DROP CONSTRAINT [FK_ctrain_coletor_tp_alert_empresa];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_filial_empresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Filial] DROP CONSTRAINT [FK_ctrain_filial_empresa];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_gateway_empresa]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[Gateway] DROP CONSTRAINT [FK_ctrain_gateway_empresa];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[FK_ctrain_maulog_maq]', 'F') IS NOT NULL
    ALTER TABLE [ConnectorModelStoreContainer].[MaquinaLog] DROP CONSTRAINT [FK_ctrain_maulog_maq];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_pass_centr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceitaPassoCentrifugacao] DROP CONSTRAINT [FK_ctrain_pass_centr];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_pass_lava]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceitaPassoLavagem] DROP CONSTRAINT [FK_ctrain_pass_lava];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_rec_empresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Receita] DROP CONSTRAINT [FK_ctrain_rec_empresa];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_rec_pass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceitaPasso] DROP CONSTRAINT [FK_ctrain_rec_pass];
GO
IF OBJECT_ID(N'[dbo].[FK_ctrain_unidade_filial]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Unidade] DROP CONSTRAINT [FK_ctrain_unidade_filial];
GO
IF OBJECT_ID(N'[dbo].[FK_id_empresa_constrain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT [FK_id_empresa_constrain];
GO
IF OBJECT_ID(N'[dbo].[FK_id_empresa_maquina_constrain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Maquina] DROP CONSTRAINT [FK_id_empresa_maquina_constrain];
GO
IF OBJECT_ID(N'[dbo].[FK_id_usu_med_login]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsuarioLogin] DROP CONSTRAINT [FK_id_usu_med_login];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Coletor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coletor];
GO
IF OBJECT_ID(N'[dbo].[ColetorAlerta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ColetorAlerta];
GO
IF OBJECT_ID(N'[dbo].[ColetorTipoAlerta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ColetorTipoAlerta];
GO
IF OBJECT_ID(N'[dbo].[Empresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Empresa];
GO
IF OBJECT_ID(N'[dbo].[Filial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Filial];
GO
IF OBJECT_ID(N'[dbo].[Maquina]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maquina];
GO
IF OBJECT_ID(N'[dbo].[Receita]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Receita];
GO
IF OBJECT_ID(N'[dbo].[ReceitaPasso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceitaPasso];
GO
IF OBJECT_ID(N'[dbo].[ReceitaPassoCentrifugacao]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceitaPassoCentrifugacao];
GO
IF OBJECT_ID(N'[dbo].[ReceitaPassoLavagem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceitaPassoLavagem];
GO
IF OBJECT_ID(N'[dbo].[Unidade]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Unidade];
GO
IF OBJECT_ID(N'[dbo].[Usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuario];
GO
IF OBJECT_ID(N'[dbo].[UsuarioLogin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsuarioLogin];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[ColetorAlertaLog]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[ColetorAlertaLog];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[ColetorPressaoHistorico]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[ColetorPressaoHistorico];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[ColetorProducaoHistorico]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[ColetorProducaoHistorico];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[ColetorSensorMovHistorico]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[ColetorSensorMovHistorico];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[ColetorTemperaturaHistorico]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[ColetorTemperaturaHistorico];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[Gateway]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[Gateway];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[LogAtividade]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[LogAtividade];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[MaquinaLog]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[MaquinaLog];
GO
IF OBJECT_ID(N'[ConnectorModelStoreContainer].[Setor]', 'U') IS NOT NULL
    DROP TABLE [ConnectorModelStoreContainer].[Setor];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Coletor'
CREATE TABLE [dbo].[Coletor] (
    [Id] int  NOT NULL,
    [Id_Maquina] int  NULL,
    [Id_Empresa] int  NULL,
    [Descricao] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL,
    [Ativo] bit  NULL,
    [MAC] varchar(20)  NOT NULL,
    [Alerta] int  NULL
);
GO

-- Creating table 'ColetorAlerta'
CREATE TABLE [dbo].[ColetorAlerta] (
    [Id] int  NOT NULL,
    [Id_Empresa] int  NOT NULL,
    [Id_Coletor] int  NOT NULL,
    [Id_TipoAlerta] int  NOT NULL,
    [Prioridade] int  NULL,
    [Descricao] varchar(50)  NULL,
    [Email] varchar(50)  NULL,
    [Regra] int  NULL,
    [Valor] varchar(8)  NULL,
    [Ativo] int  NULL
);
GO

-- Creating table 'ColetorTipoAlerta'
CREATE TABLE [dbo].[ColetorTipoAlerta] (
    [Id] int  NOT NULL,
    [Id_Empresa] int  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [UnidadeMedida] varchar(50)  NULL,
    [Ativo] bit  NULL,
    [Tipo] int  NULL
);
GO

-- Creating table 'Empresa'
CREATE TABLE [dbo].[Empresa] (
    [Id] int  NOT NULL,
    [Nome] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL,
    [Tipo] int  NULL,
    [Empresas] varchar(100)  NULL,
    [URL] varchar(50)  NULL,
    [AnalitycCode] varchar(20)  NULL,
    [Site] varchar(50)  NULL,
    [Endereco] varchar(50)  NULL,
    [CEP] varchar(12)  NULL,
    [Numero] varchar(5)  NULL,
    [Cidade] varchar(30)  NULL,
    [Estado] varchar(2)  NULL,
    [Telefone] varchar(15)  NULL,
    [NomeFantasia] varchar(30)  NULL,
    [Email] varchar(30)  NULL,
    [Bairro] varchar(30)  NULL
);
GO

-- Creating table 'Filial'
CREATE TABLE [dbo].[Filial] (
    [Id] int  NOT NULL,
    [Id_Empresa] int  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL
);
GO

-- Creating table 'Unidade'
CREATE TABLE [dbo].[Unidade] (
    [Id] int  NOT NULL,
    [Id_Filial] int  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL
);
GO

-- Creating table 'Usuario'
CREATE TABLE [dbo].[Usuario] (
    [ID] int  NOT NULL,
    [Id_Empresa] int  NOT NULL,
    [Email] varchar(30)  NOT NULL,
    [Pass] varchar(150)  NOT NULL,
    [Count] int  NULL,
    [Last] datetime  NULL,
    [Create] datetime  NULL,
    [Ative] int  NULL,
    [Hash] varchar(150)  NULL,
    [Nome] varchar(40)  NULL,
    [Tipo] int  NULL
);
GO

-- Creating table 'UsuarioLogin'
CREATE TABLE [dbo].[UsuarioLogin] (
    [ID] int  NOT NULL,
    [Id_MedidorUsuario] int  NOT NULL,
    [DataHora] datetime  NOT NULL
);
GO

-- Creating table 'ColetorAlertaLog'
CREATE TABLE [dbo].[ColetorAlertaLog] (
    [Id] int  NOT NULL,
    [Id_Coletor] int  NOT NULL,
    [Id_ColetorAlerta] int  NOT NULL,
    [DataHora] datetime  NULL,
    [ValorRegra] varchar(25)  NULL,
    [ValorEnviado] varchar(25)  NULL
);
GO

-- Creating table 'ColetorPressaoHistorico'
CREATE TABLE [dbo].[ColetorPressaoHistorico] (
    [Id] int  NOT NULL,
    [Id_Coletor] int  NOT NULL,
    [DataHora] datetime  NULL,
    [Pressao] varchar(25)  NULL
);
GO

-- Creating table 'ColetorProducaoHistorico'
CREATE TABLE [dbo].[ColetorProducaoHistorico] (
    [Id] int  NOT NULL,
    [Id_Coletor] int  NOT NULL,
    [DataHora] datetime  NULL,
    [Valor] varchar(25)  NULL
);
GO

-- Creating table 'ColetorSensorMovHistorico'
CREATE TABLE [dbo].[ColetorSensorMovHistorico] (
    [Id] int  NOT NULL,
    [Coletor] varchar(10)  NOT NULL,
    [DataHora] datetime  NULL,
    [Valor] varchar(10)  NULL
);
GO

-- Creating table 'ColetorTemperaturaHistorico'
CREATE TABLE [dbo].[ColetorTemperaturaHistorico] (
    [Id] int  NOT NULL,
    [Id_Coletor] int  NOT NULL,
    [DataHora] datetime  NULL,
    [Temperatura] varchar(25)  NULL
);
GO

-- Creating table 'Gateway'
CREATE TABLE [dbo].[Gateway] (
    [Id] int  NOT NULL,
    [Id_Empresa] int  NULL,
    [Descricao] varchar(50)  NULL,
    [IP] varchar(20)  NULL,
    [Resumo] varchar(250)  NULL,
    [Ativo] bit  NULL,
    [MAC] varchar(20)  NOT NULL
);
GO

-- Creating table 'LogAtividade'
CREATE TABLE [dbo].[LogAtividade] (
    [Id] int  NOT NULL,
    [Id_Empresa] int  NULL,
    [Id_Dispositivo] varchar(50)  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [Tipo] int  NOT NULL,
    [DataHora] datetime  NOT NULL
);
GO

-- Creating table 'MaquinaHorario'
CREATE TABLE [dbo].[MaquinaHorario] (
    [Id] int  NOT NULL,
    [Id_Maquina] int  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [DataHoraInicio] datetime  NULL,
    [DataHoraFim] datetime  NULL
);
GO

-- Creating table 'Setor'
CREATE TABLE [dbo].[Setor] (
    [Id] int  NOT NULL,
    [Descricao] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL,
    [Id_Unidade] int  NOT NULL
);
GO

-- Creating table 'Receita'
CREATE TABLE [dbo].[Receita] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_Maquina] int  NULL,
    [Id_Empresa] int  NULL,
    [Descricao] varchar(50)  NULL,
    [Resumo] varchar(250)  NULL,
    [Ativo] bit  NULL
);
GO

-- Creating table 'ReceitaPasso'
CREATE TABLE [dbo].[ReceitaPasso] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_Receita] int  NOT NULL,
    [Decricao] varchar(70)  NULL,
    [Tipo] varchar(1)  NULL,
    [Ativo] int  NULL
);
GO

-- Creating table 'ReceitaPassoCentrifugacao'
CREATE TABLE [dbo].[ReceitaPassoCentrifugacao] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_ReceitaPasso] int  NOT NULL,
    [Saida] varchar(30)  NULL,
    [Velocidade1] varchar(70)  NULL,
    [Tempo1] varchar(30)  NULL,
    [Velocidade2] varchar(70)  NULL,
    [Tempo2] varchar(30)  NULL,
    [Velocidade3] varchar(70)  NULL,
    [Tempo3] varchar(30)  NULL,
    [Velocidade4] varchar(70)  NULL,
    [Tempo4] varchar(30)  NULL,
    [Velocidade5] varchar(70)  NULL,
    [Tempo5] varchar(30)  NULL
);
GO

-- Creating table 'ReceitaPassoLavagem'
CREATE TABLE [dbo].[ReceitaPassoLavagem] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_ReceitaPasso] int  NOT NULL,
    [TempoOperacao] varchar(40)  NULL,
    [TempoReversao] varchar(40)  NULL,
    [RPM] varchar(40)  NULL,
    [Temperatura] varchar(30)  NULL,
    [SemVapor] varchar(40)  NULL,
    [Entrada] varchar(40)  NULL,
    [Nivel] varchar(40)  NULL,
    [Saida] varchar(40)  NULL,
    [ProdutoA] varchar(70)  NULL,
    [ProdutoB] varchar(70)  NULL,
    [ProdutoC] varchar(70)  NULL,
    [ProdutoD] varchar(70)  NULL,
    [ProdutoE] varchar(70)  NULL,
    [ProdutoF] varchar(70)  NULL,
    [ProdutoG] varchar(70)  NULL
);
GO

-- Creating table 'Maquina'
CREATE TABLE [dbo].[Maquina] (
    [ID] int  NOT NULL,
    [Id_Empresa] int  NOT NULL,
    [Descricao] varchar(100)  NULL,
    [Topico] varchar(50)  NULL
);
GO

-- Creating table 'MaquinaLog'
CREATE TABLE [dbo].[MaquinaLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_Maquina] int  NOT NULL,
    [DataHora] datetime  NULL,
    [DataHoraCLP] nchar(50)  NULL,
    [Kilos] nchar(5)  NULL,
    [Programa] nchar(12)  NULL,
    [NumeroCiclo] nchar(3)  NULL,
    [Temperatura] nchar(4)  NULL,
    [Status] nchar(2)  NULL,
    [Consumo] nchar(4)  NULL,
    [TempoTrabalhando] nchar(30)  NULL,
    [TempoParada] nchar(30)  NULL,
    [TempoFalha] nchar(30)  NULL,
    [TempoReserva1] nchar(30)  NULL,
    [TempoReserva2] nchar(30)  NULL,
    [TempoReserva3] nchar(30)  NULL,
    [Fator] nchar(30)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Coletor'
ALTER TABLE [dbo].[Coletor]
ADD CONSTRAINT [PK_Coletor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ColetorAlerta'
ALTER TABLE [dbo].[ColetorAlerta]
ADD CONSTRAINT [PK_ColetorAlerta]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ColetorTipoAlerta'
ALTER TABLE [dbo].[ColetorTipoAlerta]
ADD CONSTRAINT [PK_ColetorTipoAlerta]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Empresa'
ALTER TABLE [dbo].[Empresa]
ADD CONSTRAINT [PK_Empresa]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Filial'
ALTER TABLE [dbo].[Filial]
ADD CONSTRAINT [PK_Filial]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Unidade'
ALTER TABLE [dbo].[Unidade]
ADD CONSTRAINT [PK_Unidade]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Usuario'
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT [PK_Usuario]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UsuarioLogin'
ALTER TABLE [dbo].[UsuarioLogin]
ADD CONSTRAINT [PK_UsuarioLogin]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id], [Id_Coletor], [Id_ColetorAlerta] in table 'ColetorAlertaLog'
ALTER TABLE [dbo].[ColetorAlertaLog]
ADD CONSTRAINT [PK_ColetorAlertaLog]
    PRIMARY KEY CLUSTERED ([Id], [Id_Coletor], [Id_ColetorAlerta] ASC);
GO

-- Creating primary key on [Id], [Id_Coletor] in table 'ColetorPressaoHistorico'
ALTER TABLE [dbo].[ColetorPressaoHistorico]
ADD CONSTRAINT [PK_ColetorPressaoHistorico]
    PRIMARY KEY CLUSTERED ([Id], [Id_Coletor] ASC);
GO

-- Creating primary key on [Id], [Id_Coletor] in table 'ColetorProducaoHistorico'
ALTER TABLE [dbo].[ColetorProducaoHistorico]
ADD CONSTRAINT [PK_ColetorProducaoHistorico]
    PRIMARY KEY CLUSTERED ([Id], [Id_Coletor] ASC);
GO

-- Creating primary key on [Id], [Coletor] in table 'ColetorSensorMovHistorico'
ALTER TABLE [dbo].[ColetorSensorMovHistorico]
ADD CONSTRAINT [PK_ColetorSensorMovHistorico]
    PRIMARY KEY CLUSTERED ([Id], [Coletor] ASC);
GO

-- Creating primary key on [Id], [Id_Coletor] in table 'ColetorTemperaturaHistorico'
ALTER TABLE [dbo].[ColetorTemperaturaHistorico]
ADD CONSTRAINT [PK_ColetorTemperaturaHistorico]
    PRIMARY KEY CLUSTERED ([Id], [Id_Coletor] ASC);
GO

-- Creating primary key on [Id], [MAC] in table 'Gateway'
ALTER TABLE [dbo].[Gateway]
ADD CONSTRAINT [PK_Gateway]
    PRIMARY KEY CLUSTERED ([Id], [MAC] ASC);
GO

-- Creating primary key on [Id], [Id_Dispositivo], [Tipo], [DataHora] in table 'LogAtividade'
ALTER TABLE [dbo].[LogAtividade]
ADD CONSTRAINT [PK_LogAtividade]
    PRIMARY KEY CLUSTERED ([Id], [Id_Dispositivo], [Tipo], [DataHora] ASC);
GO

-- Creating primary key on [Id], [Id_Maquina] in table 'MaquinaHorario'
ALTER TABLE [dbo].[MaquinaHorario]
ADD CONSTRAINT [PK_MaquinaHorario]
    PRIMARY KEY CLUSTERED ([Id], [Id_Maquina] ASC);
GO

-- Creating primary key on [Id], [Id_Unidade] in table 'Setor'
ALTER TABLE [dbo].[Setor]
ADD CONSTRAINT [PK_Setor]
    PRIMARY KEY CLUSTERED ([Id], [Id_Unidade] ASC);
GO

-- Creating primary key on [Id] in table 'Receita'
ALTER TABLE [dbo].[Receita]
ADD CONSTRAINT [PK_Receita]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceitaPasso'
ALTER TABLE [dbo].[ReceitaPasso]
ADD CONSTRAINT [PK_ReceitaPasso]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceitaPassoCentrifugacao'
ALTER TABLE [dbo].[ReceitaPassoCentrifugacao]
ADD CONSTRAINT [PK_ReceitaPassoCentrifugacao]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceitaPassoLavagem'
ALTER TABLE [dbo].[ReceitaPassoLavagem]
ADD CONSTRAINT [PK_ReceitaPassoLavagem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Maquina'
ALTER TABLE [dbo].[Maquina]
ADD CONSTRAINT [PK_Maquina]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id], [Id_Maquina] in table 'MaquinaLog'
ALTER TABLE [dbo].[MaquinaLog]
ADD CONSTRAINT [PK_MaquinaLog]
    PRIMARY KEY CLUSTERED ([Id], [Id_Maquina] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id_Coletor] in table 'ColetorAlerta'
ALTER TABLE [dbo].[ColetorAlerta]
ADD CONSTRAINT [FK_ctrain_coletor_alerta]
    FOREIGN KEY ([Id_Coletor])
    REFERENCES [dbo].[Coletor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_alerta'
CREATE INDEX [IX_FK_ctrain_coletor_alerta]
ON [dbo].[ColetorAlerta]
    ([Id_Coletor]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Coletor'
ALTER TABLE [dbo].[Coletor]
ADD CONSTRAINT [FK_ctrain_coletor_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_empresa'
CREATE INDEX [IX_FK_ctrain_coletor_empresa]
ON [dbo].[Coletor]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Coletor] in table 'ColetorPressaoHistorico'
ALTER TABLE [dbo].[ColetorPressaoHistorico]
ADD CONSTRAINT [FK_ctrain_coletor_pressao_hist]
    FOREIGN KEY ([Id_Coletor])
    REFERENCES [dbo].[Coletor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_pressao_hist'
CREATE INDEX [IX_FK_ctrain_coletor_pressao_hist]
ON [dbo].[ColetorPressaoHistorico]
    ([Id_Coletor]);
GO

-- Creating foreign key on [Id_Coletor] in table 'ColetorProducaoHistorico'
ALTER TABLE [dbo].[ColetorProducaoHistorico]
ADD CONSTRAINT [FK_ctrain_coletor_producao_hist]
    FOREIGN KEY ([Id_Coletor])
    REFERENCES [dbo].[Coletor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_producao_hist'
CREATE INDEX [IX_FK_ctrain_coletor_producao_hist]
ON [dbo].[ColetorProducaoHistorico]
    ([Id_Coletor]);
GO

-- Creating foreign key on [Id_Coletor] in table 'ColetorTemperaturaHistorico'
ALTER TABLE [dbo].[ColetorTemperaturaHistorico]
ADD CONSTRAINT [FK_ctrain_coletor_temperatura_hist]
    FOREIGN KEY ([Id_Coletor])
    REFERENCES [dbo].[Coletor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_temperatura_hist'
CREATE INDEX [IX_FK_ctrain_coletor_temperatura_hist]
ON [dbo].[ColetorTemperaturaHistorico]
    ([Id_Coletor]);
GO

-- Creating foreign key on [Id_Empresa] in table 'ColetorAlerta'
ALTER TABLE [dbo].[ColetorAlerta]
ADD CONSTRAINT [FK_ctrain_coletor_alert_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_alert_empresa'
CREATE INDEX [IX_FK_ctrain_coletor_alert_empresa]
ON [dbo].[ColetorAlerta]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_TipoAlerta] in table 'ColetorAlerta'
ALTER TABLE [dbo].[ColetorAlerta]
ADD CONSTRAINT [FK_ctrain_coletor_tipoalerta]
    FOREIGN KEY ([Id_TipoAlerta])
    REFERENCES [dbo].[ColetorTipoAlerta]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_tipoalerta'
CREATE INDEX [IX_FK_ctrain_coletor_tipoalerta]
ON [dbo].[ColetorAlerta]
    ([Id_TipoAlerta]);
GO

-- Creating foreign key on [Id_Empresa] in table 'ColetorTipoAlerta'
ALTER TABLE [dbo].[ColetorTipoAlerta]
ADD CONSTRAINT [FK_ctrain_coletor_tp_alert_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_tp_alert_empresa'
CREATE INDEX [IX_FK_ctrain_coletor_tp_alert_empresa]
ON [dbo].[ColetorTipoAlerta]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Empresa] in table 'LogAtividade'
ALTER TABLE [dbo].[LogAtividade]
ADD CONSTRAINT [FK_ctrain_atividade_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_atividade_empresa'
CREATE INDEX [IX_FK_ctrain_atividade_empresa]
ON [dbo].[LogAtividade]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Filial'
ALTER TABLE [dbo].[Filial]
ADD CONSTRAINT [FK_ctrain_filial_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_filial_empresa'
CREATE INDEX [IX_FK_ctrain_filial_empresa]
ON [dbo].[Filial]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Gateway'
ALTER TABLE [dbo].[Gateway]
ADD CONSTRAINT [FK_ctrain_gateway_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_gateway_empresa'
CREATE INDEX [IX_FK_ctrain_gateway_empresa]
ON [dbo].[Gateway]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Usuario'
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT [FK_id_empresa_constrain]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_id_empresa_constrain'
CREATE INDEX [IX_FK_id_empresa_constrain]
ON [dbo].[Usuario]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Filial] in table 'Unidade'
ALTER TABLE [dbo].[Unidade]
ADD CONSTRAINT [FK_ctrain_unidade_filial]
    FOREIGN KEY ([Id_Filial])
    REFERENCES [dbo].[Filial]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_unidade_filial'
CREATE INDEX [IX_FK_ctrain_unidade_filial]
ON [dbo].[Unidade]
    ([Id_Filial]);
GO

-- Creating foreign key on [Id_MedidorUsuario] in table 'UsuarioLogin'
ALTER TABLE [dbo].[UsuarioLogin]
ADD CONSTRAINT [FK_id_usu_med_login]
    FOREIGN KEY ([Id_MedidorUsuario])
    REFERENCES [dbo].[Usuario]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_id_usu_med_login'
CREATE INDEX [IX_FK_id_usu_med_login]
ON [dbo].[UsuarioLogin]
    ([Id_MedidorUsuario]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Receita'
ALTER TABLE [dbo].[Receita]
ADD CONSTRAINT [FK_ctrain_rec_empresa]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_rec_empresa'
CREATE INDEX [IX_FK_ctrain_rec_empresa]
ON [dbo].[Receita]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Receita] in table 'ReceitaPasso'
ALTER TABLE [dbo].[ReceitaPasso]
ADD CONSTRAINT [FK_ctrain_rec_pass]
    FOREIGN KEY ([Id_Receita])
    REFERENCES [dbo].[Receita]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_rec_pass'
CREATE INDEX [IX_FK_ctrain_rec_pass]
ON [dbo].[ReceitaPasso]
    ([Id_Receita]);
GO

-- Creating foreign key on [Id_ReceitaPasso] in table 'ReceitaPassoCentrifugacao'
ALTER TABLE [dbo].[ReceitaPassoCentrifugacao]
ADD CONSTRAINT [FK_ctrain_pass_centr]
    FOREIGN KEY ([Id_ReceitaPasso])
    REFERENCES [dbo].[ReceitaPasso]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_pass_centr'
CREATE INDEX [IX_FK_ctrain_pass_centr]
ON [dbo].[ReceitaPassoCentrifugacao]
    ([Id_ReceitaPasso]);
GO

-- Creating foreign key on [Id_ReceitaPasso] in table 'ReceitaPassoLavagem'
ALTER TABLE [dbo].[ReceitaPassoLavagem]
ADD CONSTRAINT [FK_ctrain_pass_lava]
    FOREIGN KEY ([Id_ReceitaPasso])
    REFERENCES [dbo].[ReceitaPasso]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_pass_lava'
CREATE INDEX [IX_FK_ctrain_pass_lava]
ON [dbo].[ReceitaPassoLavagem]
    ([Id_ReceitaPasso]);
GO

-- Creating foreign key on [Id_Maquina] in table 'Coletor'
ALTER TABLE [dbo].[Coletor]
ADD CONSTRAINT [FK_ctrain_coletor_maquina]
    FOREIGN KEY ([Id_Maquina])
    REFERENCES [dbo].[Maquina]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_coletor_maquina'
CREATE INDEX [IX_FK_ctrain_coletor_maquina]
ON [dbo].[Coletor]
    ([Id_Maquina]);
GO

-- Creating foreign key on [Id_Empresa] in table 'Maquina'
ALTER TABLE [dbo].[Maquina]
ADD CONSTRAINT [FK_id_empresa_maquina_constrain]
    FOREIGN KEY ([Id_Empresa])
    REFERENCES [dbo].[Empresa]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_id_empresa_maquina_constrain'
CREATE INDEX [IX_FK_id_empresa_maquina_constrain]
ON [dbo].[Maquina]
    ([Id_Empresa]);
GO

-- Creating foreign key on [Id_Maquina] in table 'MaquinaHorario'
ALTER TABLE [dbo].[MaquinaHorario]
ADD CONSTRAINT [FK_maquina_hora_maquina]
    FOREIGN KEY ([Id_Maquina])
    REFERENCES [dbo].[Maquina]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_maquina_hora_maquina'
CREATE INDEX [IX_FK_maquina_hora_maquina]
ON [dbo].[MaquinaHorario]
    ([Id_Maquina]);
GO

-- Creating foreign key on [Id_Maquina] in table 'MaquinaLog'
ALTER TABLE [dbo].[MaquinaLog]
ADD CONSTRAINT [FK_ctrain_maulog_maq]
    FOREIGN KEY ([Id_Maquina])
    REFERENCES [dbo].[Maquina]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ctrain_maulog_maq'
CREATE INDEX [IX_FK_ctrain_maulog_maq]
ON [dbo].[MaquinaLog]
    ([Id_Maquina]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------