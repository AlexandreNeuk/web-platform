

ALTER TABLE [dbo].[Coletor]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[Coletor] CHECK CONSTRAINT [ctrain_coletor_empresa]
GO
ALTER TABLE [dbo].[Coletor]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_maquina] FOREIGN KEY([Id_Maquina])
REFERENCES [dbo].[Maquina] ([ID])
GO
ALTER TABLE [dbo].[Coletor] CHECK CONSTRAINT [ctrain_coletor_maquina]
GO
ALTER TABLE [dbo].[ColetorAlerta]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_alert_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[ColetorAlerta] CHECK CONSTRAINT [ctrain_coletor_alert_empresa]
GO
ALTER TABLE [dbo].[ColetorAlerta]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_alerta] FOREIGN KEY([Id_Coletor])
REFERENCES [dbo].[Coletor] ([Id])
GO
ALTER TABLE [dbo].[ColetorAlerta] CHECK CONSTRAINT [ctrain_coletor_alerta]
GO
ALTER TABLE [dbo].[ColetorAlerta]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_tipoalerta] FOREIGN KEY([Id_TipoAlerta])
REFERENCES [dbo].[ColetorTipoAlerta] ([Id])
GO
ALTER TABLE [dbo].[ColetorAlerta] CHECK CONSTRAINT [ctrain_coletor_tipoalerta]
GO
ALTER TABLE [dbo].[ColetorPressaoHistorico]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_pressao_hist] FOREIGN KEY([Id_Coletor])
REFERENCES [dbo].[Coletor] ([Id])
GO
ALTER TABLE [dbo].[ColetorPressaoHistorico] CHECK CONSTRAINT [ctrain_coletor_pressao_hist]
GO
ALTER TABLE [dbo].[ColetorProducaoHistorico]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_producao_hist] FOREIGN KEY([Id_Coletor])
REFERENCES [dbo].[Coletor] ([Id])
GO
ALTER TABLE [dbo].[ColetorProducaoHistorico] CHECK CONSTRAINT [ctrain_coletor_producao_hist]
GO
ALTER TABLE [dbo].[ColetorTemperaturaHistorico]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_temperatura_hist] FOREIGN KEY([Id_Coletor])
REFERENCES [dbo].[Coletor] ([Id])
GO
ALTER TABLE [dbo].[ColetorTemperaturaHistorico] CHECK CONSTRAINT [ctrain_coletor_temperatura_hist]
GO
ALTER TABLE [dbo].[ColetorTipoAlerta]  WITH NOCHECK ADD  CONSTRAINT [ctrain_coletor_tp_alert_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[ColetorTipoAlerta] CHECK CONSTRAINT [ctrain_coletor_tp_alert_empresa]
GO
ALTER TABLE [dbo].[Filial]  WITH NOCHECK ADD  CONSTRAINT [ctrain_filial_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[Filial] CHECK CONSTRAINT [ctrain_filial_empresa]
GO
ALTER TABLE [dbo].[Gateway]  WITH NOCHECK ADD  CONSTRAINT [ctrain_gateway_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[Gateway] CHECK CONSTRAINT [ctrain_gateway_empresa]
GO
ALTER TABLE [dbo].[LogAtividade]  WITH NOCHECK ADD  CONSTRAINT [ctrain_atividade_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[LogAtividade] CHECK CONSTRAINT [ctrain_atividade_empresa]
GO
ALTER TABLE [dbo].[Maquina]  WITH NOCHECK ADD  CONSTRAINT [id_empresa_maquina_constrain] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[Maquina] CHECK CONSTRAINT [id_empresa_maquina_constrain]
GO
ALTER TABLE [dbo].[MaquinaLog]  WITH NOCHECK ADD  CONSTRAINT [ctrain_maulog_maq] FOREIGN KEY([Id_Maquina])
REFERENCES [dbo].[Maquina] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaquinaLog] CHECK CONSTRAINT [ctrain_maulog_maq]
GO
ALTER TABLE [dbo].[MaquinaLogReport]  WITH NOCHECK ADD  CONSTRAINT [ctrain_maq_log_rpt] FOREIGN KEY([Id_Maquina])
REFERENCES [dbo].[Maquina] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaquinaLogReport] CHECK CONSTRAINT [ctrain_maq_log_rpt]
GO
ALTER TABLE [dbo].[Receita]  WITH NOCHECK ADD  CONSTRAINT [ctrain_rec_empresa] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Receita] CHECK CONSTRAINT [ctrain_rec_empresa]
GO
ALTER TABLE [dbo].[ReceitaPasso]  WITH NOCHECK ADD  CONSTRAINT [ctrain_rec_pass] FOREIGN KEY([Id_Receita])
REFERENCES [dbo].[Receita] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReceitaPasso] CHECK CONSTRAINT [ctrain_rec_pass]
GO
ALTER TABLE [dbo].[ReceitaPassoCentrifugacao]  WITH NOCHECK ADD  CONSTRAINT [ctrain_pass_centr] FOREIGN KEY([Id_ReceitaPasso])
REFERENCES [dbo].[ReceitaPasso] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReceitaPassoCentrifugacao] CHECK CONSTRAINT [ctrain_pass_centr]
GO
ALTER TABLE [dbo].[ReceitaPassoLavagem]  WITH NOCHECK ADD  CONSTRAINT [ctrain_pass_lava] FOREIGN KEY([Id_ReceitaPasso])
REFERENCES [dbo].[ReceitaPasso] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReceitaPassoLavagem] CHECK CONSTRAINT [ctrain_pass_lava]
GO
ALTER TABLE [dbo].[Unidade]  WITH NOCHECK ADD  CONSTRAINT [ctrain_unidade_filial] FOREIGN KEY([Id_Filial])
REFERENCES [dbo].[Filial] ([Id])
GO
ALTER TABLE [dbo].[Unidade] CHECK CONSTRAINT [ctrain_unidade_filial]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [id_empresa_constrain] FOREIGN KEY([Id_Empresa])
REFERENCES [dbo].[Empresa] ([Id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [id_empresa_constrain]
GO
ALTER TABLE [dbo].[UsuarioLogin]  WITH NOCHECK ADD  CONSTRAINT [id_usu_med_login] FOREIGN KEY([Id_MedidorUsuario])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[UsuarioLogin] CHECK CONSTRAINT [id_usu_med_login]
GO
