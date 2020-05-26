m = function () {
    //
    var _m = {}
    var d = document;
    d.getid = d.getElementById;
    d.s = String.fromCharCode;
    //
    _m.g = (function () { // funções globais
        return {
            load: function (t) {
                return new Loading({
                    discription: t,
                    discriptionColor: 'rgb(255, 255, 255)',
                    animationOriginColor: 'rgb(255, 255, 255)',
                    mask: true,
                    loadingPadding: '20px 50px',
                    defaultApply: true,
                });
            },
            rsvlurl: function (p) {
                var url = '';
                if (location.host == 'localhost') url = 'http://localhost/connector/';
                else {
                    if (!(location.host.indexOf('host') > -1)) {
                        url = location.origin + '/';
                    }
                    else {
                        url = d.URL;
                    }
                }
                return url + p;
            },
            hrfrmt: function (v) {
                if (v) {
                    if (!isNaN(v[0]) && !isNaN(v[1]) && isNaN(v[2]) && !isNaN(v[3]) && !isNaN(v[4])) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            addmask: function (a, l) {
                for (var i = 0; i < l; i++) {
                    $('input[id$="' + a + (i + 1) + '"]').inputmask("hh:mm");
                }
            },
            number_format: function (number, decimals, dec_point, thousands_sep) {
                // *     example: number_format(1234.56, 2, ',', ' ');
                // *     return: '1 234,56'
                number = (number + '').replace(',', '').replace(' ', '');
                var n = !isFinite(+number) ? 0 : +number,
                    prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
                    sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
                    dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
                    s = '',
                    toFixedFix = function (n, prec) {
                        var k = Math.pow(10, prec);
                        return '' + Math.round(n * k) / k;
                    };
                // Fix for IE parseFloat(0.55).toFixed(0) = 0;
                s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
                if (s[0].length > 3) {
                    s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
                }
                if ((s[1] || '').length < prec) {
                    s[1] = s[1] || '';
                    s[1] += new Array(prec - s[1].length + 1).join('0');
                }
                return s.join(dec);
            },
            random_rgba: function (){
                var o = Math.round, r = Math.random, s = 255;
                return 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',' + r().toFixed(1) + ')';
            },
            rgba_graph: function () {
                return ['78, 115, 223, 1',
                    '28, 200, 138, 1',
                    '231, 74, 9, 1',
                    '54, 185, 204, 1',
                    '253, 126, 20, 1'];
            },
            remove_options: function (selectbox)
            {
                var i;
                for (i = selectbox.options.length - 1; i >= 0; i--) {
                    selectbox.remove(i);
                }
            },
            setcomboval: function (id, value) {
                var el = d.getid(id);
                for (var i = 0; i < el.options.length; i++) {
                    //
                    if (el.options[i].id == value) {
                        el.selectedIndex = i;
                        break;
                    }
                }
            },
            alert: function (title, msg, size) {
                if (!size) 
                    size = 'small';
                bootbox.alert({
                    size: size,
                    title: title,
                    message: msg,
                    centerVertical: true
                });
            },
            setstyle: function (cssText) {
                var sheet = document.createElement('style');
                sheet.type = 'text/css';
                window.customSheet = sheet;
                (document.head || document.getElementsByTagName('head')[0]).appendChild(sheet);
                return (m.g.setStyle = function (cssText, node) {
                    if (!node || node.parentNode !== sheet)
                        return sheet.appendChild(document.createTextNode(cssText));
                    node.nodeValue = cssText;
                    return node;
                })(cssText)
            }
        }
    })();
    //
    _m.sessao = (function () {
        return {
            cd_empresa_logado: null, tipo_empresa_logado: null, nome_empresa: null, cd_usuario: null, nome_usuario: null, emial_usuario: null,
            login: function () {

            },
            logout: function () {
                //
                bootbox.confirm({
                    title: "Logout",
                    message: "Você deseja realmente sair do sistema?",
                    centerVertical: true,
                    buttons: {
                        cancel: {
                            label: '<i class="fa fa-times"></i> Cancelar'
                        },
                        confirm: {
                            label: '<i class="fa fa-check"></i> Confirmar'
                        }
                    },
                    callback: function (result) {
                        //
                        if (result) {
                            m.sessao.l = m.g.load('Logout...');
                            $.ajax({
                                url: m.g.rsvlurl('/login/logout'),
                                type: "post",
                                success: function (resp) {
                                    //
                                    m.sessao.l.out()
                                    location = m.g.rsvlurl('/home/index');
                                },
                                error: function (xhr, error) {
                                    m.sessao.l.out();
                                    location = m.g.rsvlurl('/home/index');
                                    console.log(xhr);
                                    console.log(error);
                                }
                            })
                        }
                    }
                })
            }
        }
    })()
    //
    _m.empresa = (function () {
        return {
            init: function (data) {
                //
                var $table = $('#dataTableEmpresas')
                $table.bootstrapTable({ data: data });
                //if (m.sessao.tipo_empresa_logado != 1) {
                //    $table.bootstrapTable('hideColumn', 'DescricaoEmpresa');
                //}
            },
            setempresainclui: function () {
                $('.modal-title').text('Incluir Empresa');
                $('#empresaModal').modal('show');
                //
                $('#txtnomeempresa').val('');
                $('#txttelefoneempresa').val('');
                $('#txtenderecoempresa').val('');
                $('#txtbairroempresa').val('');
                $('#txtnumeroempresa').val('');
                $('#txtcidadeempresa').val('');
                $('#txtcepempresa').val('');
                $('#txtsiteempresa').val('');
                $('#txtemailempresa').val('');
                m.g.setcomboval('slestadoempresa', 0);
            },
            edit: function () {
                var tableEmpresas = $('#dataTableEmpresas');
                var selectEmpresas = tableEmpresas.bootstrapTable('getSelections');
                if (selectEmpresas && selectEmpresas.length > 0) {
                    //
                    if (selectEmpresas.length == 1) {
                        //
                        $('.modal-title').text('Editar Empresa');
                        $('#empresaModal').modal('show');
                        //
                        $('#txtnomeempresa').val(selectEmpresas[0].Nome);
                        $('#txttelefoneempresa').val(selectEmpresas[0].Telefone);
                        $('#txtenderecoempresa').val(selectEmpresas[0].Endereco);
                        $('#txtbairroempresa').val(selectEmpresas[0].Bairro);
                        $('#txtnumeroempresa').val(selectEmpresas[0].Numero);
                        $('#txtcidadeempresa').val(selectEmpresas[0].Cidade);
                        $('#txtcepempresa').val(selectEmpresas[0].CEP);
                        $('#txtsiteempresa').val(selectEmpresas[0].Site);
                        $('#txtemailempresa').val(selectEmpresas[0].Email);
                        m.g.setcomboval('slestadoempresa', selectEmpresas[0].Estado);
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas uma empresa para editar.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma empresa para editar.",
                        centerVertical: true
                    });
                }
            },
            salva: function () {
                //
                var txtnomeempresa = $('#txtnomeempresa').val();
                var txttelefoneempresa = $('#txttelefoneempresa').val();
                var txtenderecoempresa = $('#txtenderecoempresa').val();
                var txtbairroempresa = $('#txtbairroempresa').val();
                var txtnumeroempresa = $('#txtnumeroempresa').val();
                var txtcidadeempresa = $('#txtcidadeempresa').val();
                var txtcepempresa = $('#txtcepempresa').val();
                var txtsiteempresa = $('#txtsiteempresa').val();
                var txtemailempresa = $('#txtemailempresa').val();
                //
                if (!txtnomeempresa) {
                    m.g.alert('Restriçao', 'Informe o nome da empresa.');
                    return;
                }
                //
                if (!txttelefoneempresa) {
                    m.g.alert('Restriçao', 'Informe o telefone da empresa.');
                    return;
                }
                //
                if (!txtenderecoempresa) {
                    m.g.alert('Restriçao', 'Informe o endereço da empresa.');
                    return;
                }
                //
                if (!txtbairroempresa) {
                    m.g.alert('Restriçao', 'Informe o bairro da empresa.');
                    return;
                }
                //
                if (!txtnumeroempresa) {
                    m.g.alert('Restriçao', 'Informe o número da empresa.');
                    return;
                }
                //
                if (!txtcidadeempresa) {
                    m.g.alert('Restriçao', 'Informe o cidade da empresa.');
                    return;
                }
                //
                if (!txtcepempresa) {
                    m.g.alert('Restriçao', 'Informe o CEP da empresa.');
                    return;
                }
                //
                if (!txtsiteempresa) {
                    m.g.alert('Restriçao', 'Informe o site da empresa.');
                    return;
                }
                //
                if (!txtemailempresa) {
                    m.g.alert('Restriçao', 'Informe o email da empresa.');
                    return;
                }
                //
                var estado = $(d.getid('slestadoempresa')).children(":selected").attr("id");
                if (estado == 0) {
                    m.g.alert('Restriçao', 'Informe o estado da empresa.');
                    return;
                }
                //
                var tableEmpresas = $('#dataTableEmpresas');
                var selectEmpresas = tableEmpresas.bootstrapTable('getSelections');
                var idempresa = 0;
                if (selectEmpresas.length > 0) {
                    idempresa = selectEmpresas[0].Id;
                }
                //
                //if (m.sessao.cd_empresa_logado == idempresa) {
                //    idempresa = 0;
                //}
                //
                m.empresa.l = m.g.load('Salvando empresa...');
                //
                var url = m.g.rsvlurl('empresa/empresapost') +
                    '?idempresa=' + idempresa +
                    '&nome=' + txtnomeempresa +
                    '&telefone=' + txttelefoneempresa +
                    '&endereco=' + txtenderecoempresa +
                    '&bairro=' + txtbairroempresa +
                    '&numero=' + txtnumeroempresa +
                    '&cidade=' + txtcidadeempresa +
                    '&cep=' + txtcepempresa +
                    '&site=' + txtsiteempresa +
                    '&email=' + txtemailempresa +                    
                    '&estado=' + estado;
                //
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        //
                        if (resp && resp.data == 'ok') {
                            //
                            m.empresa.loadempresas();
                        }
                        else {
                            try {
                                m.empresa.loadingout();
                                $('#empresaModal').modal('hide');
                                bootbox.alert({
                                    size: "large",
                                    title: "Erro",
                                    message: 'Ocorreu um erro ao tentar incluir a empresa. Erro: ' + resp.erro,
                                    centerVertical: true
                                });
                            } catch (err) { }

                        }
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            exclui: function () {
                var tableEmpresas = $('#dataTableEmpresas');
                var selectEmpresas = tableEmpresas.bootstrapTable('getSelections');
                if (selectEmpresas && selectEmpresas.length > 0) {
                    //
                    if (selectEmpresas.length == 1) {
                        if (selectEmpresas[0].UsuarioAtivo) {
                            m.g.alert('Restriçao', 'Primeiro desassocie os usuários da empresa.');
                        }
                        else if (selectEmpresas[0].ColetorAtivo) {
                            m.g.alert('Restriçao', 'Primeiro desassocie os coletores da empresa.');
                        }
                        else if (selectEmpresas[0].MaquinaAtiva) {
                            m.g.alert('Restriçao', 'Primeiro desassocie as máquinas da empresa.');
                        }
                        else {
                            bootbox.confirm({
                                title: "Excluir Emresa?",
                                message: "Você deseja realmente excluir a empresa ('" + selectEmpresas[0].Nome + "')? <br />Esta ação não poderá ser disfeita.",
                                centerVertical: true,
                                buttons: {
                                    cancel: {
                                        label: '<i class="fa fa-times"></i> Cancelar'
                                    },
                                    confirm: {
                                        label: '<i class="fa fa-check"></i> Confirmar'
                                    }
                                },
                                callback: function (result) {
                                    //
                                    if (result) {
                                        m.empresa.l = m.g.load('Excluindo empresa...');
                                        $.get(m.g.rsvlurl('empresa/excluiempresapost') + '?idempresa=' + selectEmpresas[0].Id, function (data) {
                                            if (data.ret && data.ret == 'ok') {
                                                //
                                                m.empresa.loadempresas();
                                            }
                                            else {
                                                try { l.out() } catch (err) { }
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Erro",
                                                    message: "Ocorreu um erro ao tentar excluir a máquina, erro: " + data.erro,
                                                    centerVertical: true
                                                });
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas uma empresa para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma empresa para excluir.",
                        centerVertical: true
                    });
                }
            },
            loadempresas: function () {
                //
                $.get(m.g.rsvlurl('empresa/carregaempresas'), function (data) {
                    if (data && data.data == 'ok') {
                        //
                        var tableEmpresas = $('#dataTableEmpresas')
                        tableEmpresas.bootstrapTable('load', data.lista_retorno);
                        //
                        $('#empresaModal').modal('hide');
                        m.empresa.loadingout();
                    }
                    else {
                        m.empresa.loadingout();
                        m.g.alert('Erro', 'Ocorreu um erro ao salvar a empresa. Erro:' + data.erro);
                    }
                });                
            },
            loadingout: function () {
                try { m.empresa.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.maquina = (function () {
        return {
            id: 0, l: null, emp_ant: null, emp_nov: null, col_ant: null,
            init: function (data) {
                var $table = $('#dataTableMaquinas')
                $table.bootstrapTable({ data: data });
                if (m.sessao.tipo_empresa_logado != 1) {
                    $table.bootstrapTable('hideColumn', 'DescricaoEmpresa');
                }
            },
            salva: function () {
                if (!d.getid('txtdescmaquina').value) {
                    m.g.alert('Restriçao', 'Informe a descrição da Máquina!');
                    return;
                }
                var slcol = d.getid('slcoletor');
                var slemp = d.getid('slempresamaquina');
                m.maquina.l = m.g.load('Salvando máquina...');

                var emp_ant = m.maquina.emp_ant ? m.maquina.emp_ant : 0;
                var url = m.g.rsvlurl('maquina/maquinapost') +
                    '?descricao=' + d.getid('txtdescmaquina').value +
                    '&id_coletor=' + slcol.options[slcol.selectedIndex].id +
                    '&id_maquina=' + m.maquina.id + 
                    '&id_empresa_nova=' + slemp.options[slemp.selectedIndex].id + 
                    '&id_empresa_ant=' + emp_ant;

                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        //
                        if (resp.ret && resp.ret == 'ok') {
                            //
                            m.maquina.loadmaquinas()
                        }
                        else {
                            try {
                                l.out()
                                $('#maquinaModal').modal('hide');
                                m.maquina.l.out();
                                bootbox.alert({
                                    size: "large",
                                    title: "Erro",
                                    message: 'Ocorreu um erro ao tentar incluir a máquina.Erro: ' + data.erro,
                                    centerVertical: true
                                });
                                
                            } catch (err) { }

                        }
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            cancela: function () {
                try {
                    if (typeof m.maquina.id !== 'undefined') {
                        $('#slcoletor option[id=' + m.maquina.id + ']').remove();
                    }
                } catch (e) {}     
            },
            exclui: function () {
                var table = $('#dataTableMaquinas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    //
                    if (select.length == 1) {
                        if (select[0].Id_Coletor) {
                            bootbox.alert({
                                size: "large",
                                title: "Restriçao",
                                message: "Primeiro desassocie o coletor '" + select[0].DescricaoMedidor + "' da máquina.",
                                centerVertical: true
                            });
                        }
                        else {
                            bootbox.confirm({
                                title: "Excluir Máquina?",
                                message: "Você deseja realmente excluir a máquina ('" + select[0].Descricao + "')? <br />Esta ação não poderá ser disfeita.",
                                centerVertical: true,
                                buttons: {
                                    cancel: {
                                        label: '<i class="fa fa-times"></i> Cancelar'
                                    },
                                    confirm: {
                                        label: '<i class="fa fa-check"></i> Confirmar'
                                    }
                                },
                                callback: function (result) {
                                    //
                                    if (result) {
                                        m.maquina.l = m.g.load('Excluindo...');
                                        $.get(m.g.rsvlurl('maquina/excluimaquinapost') + '?idmaquina=' + select[0].ID, function (data) {
                                            if (data.ret && data.ret == 'ok') {
                                                m.maquina.loadmaquinas();
                                            }
                                            else {
                                                try { l.out() } catch (err) { }
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Erro",
                                                    message: "Ocorreu um erro ao tentar excluir a máquina, erro: " + data.erro,
                                                    centerVertical: true
                                                });
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas uma máquina para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma máquina para excluir.",
                        centerVertical: true
                    });
                }
            },
            editmaquina: function () {
                var table = $('#dataTableMaquinas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    if (select.length == 1) {
                        m.maquina.setmaquinaedita()
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas uma máquina para editar.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma máquina para editar.",
                        centerVertical: true
                    });
                }
            },
            setmaquinaedita: function () {                
                $('.modal-title').text('Editar Máquina');
                d.getid('txtdescmaquina').value = '';
                $('#btnexcluirmaquina').css('display', 'none');
                //
                var table = $('#dataTableMaquinas');
                var select = table.bootstrapTable('getSelections');
                //
                if (select && select[0]) {
                    d.getid('txtdescmaquina').value = select[0].Descricao;
                    //
                    m.maquina.id = select[0].ID;
                    m.maquina.emp_ant = select[0].Id_Empresa;
                    if (select[0].Id_Coletor) {
                        m.maquina.carrega_coletor = true;
                        m.maquina.colt_ant = select[0].Id_Coletor;
                        m.maquina.colt_desc_ant = select[0].DescricaoMedidor;
                    }
                    else {
                        m.maquina.carrega_coletor = false;
                    }
                    if (select[0].Id_Coletor) {
                        m.g.setcomboval('slcoletor', select[0].Id_Coletor);
                    }
                    m.g.setcomboval('slempresamaquina', m.maquina.emp_ant);
                    m.maquina.loadselectcoletores(select[0].Id_Empresa);
                    $('#maquinaModal').modal('show');
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma máquina para editar.",
                        centerVertical: true
                    })
                }
            },
            setmaquinainclui: function () {
                //
                var slemp = d.getid('slempresamaquina');
                if (slemp && slemp.options[slemp.selectedIndex].id) {
                    m.maquina.loadselectcoletores(slemp.options[slemp.selectedIndex].id);
                }
                //
                m.maquina.id = 0;
                $('.modal-title').text('Incluir Máquina');
                d.getid('txtdescmaquina').value = '';
                $('#btnexcluirmaquina').css('display', 'none');
            },
            chgcmbempresamaquina: function () {
                id_empresa = $(d.getid('slempresamaquina')).children(":selected").attr("id");
                //
                if (id_empresa) {
                    //
                    m.maquina.emp_nov = id_empresa;
                    m.maquina.loadselectcoletores(id_empresa);
                }
            },
            loadselectcoletores: function (empresa) {
                m.g.remove_options(d.getid('slcoletor'));
                $.get(m.g.rsvlurl('maquina/pegacoletores') + '?empresa=' + empresa, function (data) {
                    if (data.ret && data.ret == 'ok') {
                        //
                        if (m.maquina.carrega_coletor) {                        
                            //
                            if (!m.maquina.emp_nov) { // carrega item default
                                var x = d.getid('slcoletor');
                                var option = d.createElement('option');
                                option.text = m.maquina.colt_desc_ant;
                                option.id = m.maquina.colt_ant;
                                option.selected = true;
                                x.add(option);
                            }
                            else if (m.maquina.emp_ant == m.maquina.emp_nov) { // trocou mas para o mesmo
                                var x = d.getid('slcoletor');
                                var option = d.createElement('option');
                                option.text = m.maquina.colt_desc_ant;
                                option.id = m.maquina.colt_ant;
                                option.selected = true;
                                x.add(option);
                            }
                            else if (m.maquina.emp_ant != m.maquina.emp_nov){ // trocou mas para empresas diferentes
                                var naselected = true;    
                            }
                        }
                        else {
                            var naselected = true;
                        }
                        //
                        var x = d.getid('slcoletor');
                        var option = d.createElement('option');
                        option.text = 'N/A';
                        option.id = 0;
                        option.selected = naselected;
                        x.add(option);
                        //
                        data.lista_retorno.forEach(function (entry) {
                            //
                            var x = d.getid('slcoletor');
                            var option = d.createElement('option');
                            option.text = entry.Descricao;
                            option.id = entry.Id;
                            x.add(option);
                        });
                    }
                    else {
                        m.g.alert('Erro', 'Ocorreu um erro: ' + data.ret);
                    }
                });
            },
            exportar: function () {
                m.g.alert('Atenção', 'Funcionalidade ainda não disponível.');
            },
            loadmaquinas: function () {
                $.get(m.g.rsvlurl('maquina/carregadados'), function (data) {
                    if (data && data.data == 'ok') {
                        //
                        var $table = $('#dataTableMaquinas')
                        $table.bootstrapTable('load', data.lista_retorno);
                        //
                        $('#maquinaModal').modal('hide');
                        m.maquina.l.out();
                    }
                    else {
                        try { m.maquina.l.out(); } catch (err) { }
                        m.g.alert('Erro', 'Ocorreu um erro: ' + data.ret);
                    }
                });
            }
        }
    })();
    //
    _m.pm = (function () { // pm progrmação máquina
        return {
            l: null,
            q: function (b) {
	            if (!b) {
                    l = m.g.load(d.s(67) + d.s(111) + d.s(110) + d.s(115) + d.s(117) + d.s(108) + d.s(116) + d.s(97) + d.s(110) + d.s(100) + d.s(111));
	            }
                $.get(m.g.rsvlurl('tabelamaquinahorario/ppm') + '?b=' + d.getid("slm").options[d.getid("slm").selectedIndex].id, function (data) {
					try {l.out()}catch(err){}
		            if (data.data.length > 0) {
			            //
			            if (data.data[0].dia) {
				            //
				            $("#tbmh tr").remove();
				            //
				            var table = d.getid("tbmh");
				            //
				            for (var i = 0; i < data.data.length; i++) {
					            //
					            var row = table.insertRow(i);
					            var cell1 = row.insertCell(0);
					            var cell2 = row.insertCell(1);
					            var cell3 = row.insertCell(2);
					            var cell4 = row.insertCell(3);
					            cell1.innerHTML = data.data[i].dia;
					            cell1.style.paddingTop = '15px';
					            cell2.innerHTML = '<div class="col-3"><input id="idon' + (i+1) + '" onfocus="aaa(this);" type="text" class="form-control" style="width: 80px;" value="' + data.data[i].horaon + '"/></div>';
					            cell3.innerHTML = '<div class="col-3"><input id="idoff' + (i+1) + '" onfocus="aaa(this);" type="text" class="form-control" style="width: 80px;" value="' + data.data[i].horafim + '"/></div>';
					            //
					            if (data.data[i].ativo) {
						            cell4.innerHTML = '<div class="col-3"><img src="' + m.g.rsvlurl('/Content/imagens/activate.gif') + '" style="padding-left: 13px; padding-top: 9px;"></div>';
					            }
					            else {
						            cell4.innerHTML = '<div class="col-3"><img src="' + m.g.rsvlurl('/Content/imagens/deactivate.gif') + '" style="padding-left: 13px; padding-top: 9px;"></div>';
					            }
				            }
				            //
				            var header = table.createTHead();
				            var row = header.insertRow(0);
				            var cell = row.insertCell(0);
				            cell.innerHTML = "<b>Dia</b>";
				            cell.style.width = '100px';
				            var cell1 = row.insertCell(1);
				            cell1.innerHTML = "<b>Hora On</b>";
				            cell1.style.width = '110px';
				            var cell2 = row.insertCell(2);
				            cell2.innerHTML = "<b>Hora Off</b>";
				            cell2.style.width = '110px';
				            var cell3 = row.insertCell(3);
				            cell3.innerHTML = "<b>Status</b>";
                            cell3.style.width = '110px';
                            //
                            m.g.addmask('idon', 7);
                            m.g.addmask('idoff', 7);
			            }
		            }
		            else {
			            if (data.erro != '1') {
				            $("#tbmh tr").remove();
				            //
				            var table = d.getid("tbmh");
				            //
				            var dias = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'QUinta', 'Sexta', 'Sábado'];
				            //
				            for (var i = 0; i < dias.length; i++) {
					            //
					            var row = table.insertRow(i);
					            var cell1 = row.insertCell(0);
					            var cell2 = row.insertCell(1);
					            var cell3 = row.insertCell(2);
					            cell1.innerHTML = dias[i];
					            cell1.style.paddingTop = '15px';
					            cell2.innerHTML = '<div class="col-3" style="width: 200px !important;"><input type="text" class="form-control" style="width: 80px;" value="13:30"/></div>';
					            cell3.innerHTML = '<div class="col-3" style="width: 200px !important;"><input type="text" class="form-control" style="width: 80px;" value="14:30"/></div>';
					            cell4.innerHTML = '<div class="col-3"><img src="' + m.g.rsvlurl('/Content/imagens/deactivate.gif') + '" style="padding-left: 13px; padding-top: 9px;"></div>';
				            }
				            //
				            var header = table.createTHead();
				            var row = header.insertRow(0);
				            var cell = row.insertCell(0);
				            cell.innerHTML = "<b>Dia</b>";
				            cell.style.width = '100px';
				            var cell1 = row.insertCell(1);
				            cell1.innerHTML = "<b>Hora On</b>";
				            cell1.style.width = '110px';
				            var cell2 = row.insertCell(2);
				            cell2.innerHTML = "<b>Hora Off</b>";
				            cell2.style.width = '110px';
				            var cell3 = row.insertCell(3);
				            cell3.innerHTML = "<b>Status</b>";
				            cell3.style.width = '110px';
			            }
			            else {
                            m.g.alert('Atenção', 'Máquina não encontrada.');
			            }
		            }
	            });
            },
            salva: function () {
                var h = '';
                l = m.g.load('Salvando...');
                for (var i = 0, row; row = d.getid("tbmh").rows[i]; i++) {
                    if (i == 0) continue;
                    for (var j = 0; col = row.cells[j]; j++) {
                        if (j == 0) {
                            if (!h) {
                                h = row.cells[j].innerText;}
                            else {
                                h += '*' + row.cells[j].innerText;
                            }
                        }
                        else {
                            if (row.cells[j].childNodes[0].childNodes[0].value) {
                                h += '$' + row.cells[j].childNodes[0].childNodes[0].value;
                            }
                        }
                    }
                }
                var url = m.g.rsvlurl('tabelamaquinahorario/pmp') + '?idmaquina=' + d.getid("slm").options[d.getid("slm").selectedIndex].id + '&horarios=' + h;
                $.get(url, function (data) {
                    l.out();
                    if (data) {
                        d.getid('idbtmd').click()
                        m.pm.q(true);
                    }
                    else {
                        m.g.alert('Atenção', 'A máquina não possui dados!');
                    }
                });
            }
        }
    })()
    //
    _m.coletor = (function () {
        return {
            l: null, idcoletor: null, alertaedit: null, alertainit: null,
            init: function (data) {
                var $table = $('#dtcoletores')
                $table.bootstrapTable({ data: data });
                if (m.sessao.tipo_empresa_logado != 1) {
                    $table.bootstrapTable('hideColumn', 'Empresa');
                }
            },
            salva: function () {
                //
                if (!$('input[id=txtdesccoletor').val()) {
                    m.g.alert('Atenção', 'Informe a descrição do Coletor!');
                    return;
                }
                if (!$('input[id=txtmaccoletor').val()) {
                    m.g.alert('Atenção', 'Informe o MAC do Coletor!');
                    return;
                }
                m.coletor.l = m.g.load('Salvando');
                //
                var table = $('#dtcoletores');
                var select = table.bootstrapTable('getSelections');
                var alerta = $(d.getid('slcoletorgeraalerta')).children(":selected").attr("id");
                var empresa_anterior = select.length == 0 ? $(d.getid('slempresacoletor')).children(":selected").attr("id") : select[0].Id_Empresa;
                var empresa_nova = $(d.getid('slempresacoletor')).children(":selected").attr("id");
                var id_coletor = select.length == 0 ? 0 : select[0].Id;
                //
                var url = m.g.rsvlurl('coletor/coletorpost') + '?descricao=' + d.getid('txtdesccoletor').value +
                    '&mac=' + d.getid('txtmaccoletor').value +
                    '&idcoletor=' + id_coletor +
                    '&empresa_anterior=' + empresa_anterior +
                    '&empresa_nova=' + empresa_nova +
                    '&alerta=' + alerta;
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        if (resp && resp.data == 'ok') {
                            $('#idcoletormodal').modal('hide');
                            m.coletor.loadcoletores();
                        }
                        else {
                            try { m.coletor.l.out() } catch (err) { }
                            $('#maquinaModal').modal('hide');
                            m.g.alert('Atenção', 'Ocorreu um erro ao tentar incluir a máquina. Erro: ' + data.erro);
                        }
                        $('#coletorModal').modal('hide');
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            exclui: function () {
                var table = $('#dtcoletores');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    if (select.length == 1) {
                        if (!select[0].Id_Maquina) {
                            bootbox.confirm({
                                title: "Excluir Coletor?",
                                message: "Você deseja realmente excluir o coletor ('" + select[0].Descricao + "')? <br />Esta ação não poderá ser disfeita.",
                                centerVertical: true,
                                buttons: {
                                    cancel: {
                                        label: '<i class="fa fa-times"></i> Cancelar'
                                    },
                                    confirm: {
                                        label: '<i class="fa fa-check"></i> Confirmar'
                                    }
                                },
                                callback: function (result) {
                                    //
                                    if (result) {
                                        m.coletor.l = m.g.load('Excluindo...');
                                        m.coletor.idcoletor = select[0].Id;
                                        m.coletor.idempresa = select[0].Id_Empresa;
                                        $.get(m.g.rsvlurl('coletor/excluicoletorpost') + '?idcoletor=' + select[0].Id + '&idempresa=' + select[0].Id_Empresa, function (data) {
                                            if (data.ret && data.ret == 'ok') {
                                                m.coletor.loadcoletores();
                                            }
                                            else if (data.ret && data.ret == 'erro') {
                                                m.coletor.loadingout();
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Ocorreu um erro",
                                                    message: data.erro,
                                                    centerVertical: true
                                                });
                                            }
                                            else if (data.ret && data.ret == 'nao_encontrada') {
                                                m.coletor.loadingout();
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Ocorreu um erro",
                                                    message: "Coletor não encontrado!",
                                                    centerVertical: true
                                                });
                                            }
                                            else if ((data.ret.indexOf('hpres') > -1) || (data.ret.indexOf('htemp') > -1) || (data.ret.indexOf('hpro') > -1))
                                            {
                                                m.coletor.excluicoletorhistorico();
                                            }
                                            else {
                                                m.coletor.loadingout();
                                                m.g.alert('Atenção', 'Ocorreu um erro ao tentar excluir o coletor. Erro: ' + data.erro);
                                            }
                                        });
                                    }
                                }
                            });
                        }
                        else {
                            bootbox.alert({
                                size: "large",
                                title: "Restriçao",
                                message: "Primeiro desassocie a máquina '" + select[0].Maquina + "' do coletor!",
                                centerVertical: true
                            });
                        }
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um coletor para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione um coletor para excluir.",
                        centerVertical: true
                    });
                }
            },
            excluicoletorhistorico: function () {
                //
                m.coletor.loadingout();
                bootbox.confirm({
                    title: "Excluir coletor e histórico?",
                    message: "Este coletor possui log com histórico de dados! Você deseja realmente excluir o coletor e todo o seu histórico de dados?<br /><br />Esta ação não poderá ser disfeita.<br />Esta ação poderá demorar alguns minutos.",
                    centerVertical: true,
                    buttons: {
                        cancel: {
                            label: '<i class="fa fa-times"></i> Cancelar'
                        },
                        confirm: {
                            label: '<i class="fa fa-check"></i> Confirmar'
                        }
                    },
                    callback: function (result) {
                        //
                        if (result) {
                            m.maquina.l = m.g.load('Excluindo...');
                            $.get(m.g.rsvlurl('coletor/excluicoletorposthistorico') + '?idcoletor=' + m.coletor.idcoletor + '&idempresa=' + m.coletor.idempresa, function (data) {
                                if (data.ret && data.ret == 'ok') {
                                    //
                                    bootbox.alert({
                                        size: "small",
                                        title: "Confirmação de Exclusão",
                                        message: "Coletor excluído com sucesso!<br />Foram excluídos " + data.total_excl + " registros de log.",
                                        centerVertical: true
                                    });
                                    //
                                    m.coletor.idcoletor = null;
                                    m.coletor.idempresa = null;
                                    m.maquina.loadmaquinas();
                                }
                                else {
                                    try { m.maquina.l.out() } catch (err) { }
                                    bootbox.alert({
                                        size: "large",
                                        title: "Erro",
                                        message: "Ocorreu um erro ao tentar excluir o coletor ou seu log: " + data.ret,
                                        centerVertical: true
                                    });
                                }
                            });
                        }
                    }
                });
            },
            setcoletorinclui: function () {
                idcoletor = 0;
                $('#modalColetorLabel').text('Incluir Coletor');
                $('input[id=txtdesccoletor').val('');
                $('input[id=txtmaccoletor').val('');
                $('#divalertas').css('display', 'none');
                $('#idcoletormodal').modal('show');
            },
            editcoletor: function () {
                //
                m.coletor.l = m.g.load('Carregando coletor...');
                m.coletor.alertaedit = false;
                var table = $('#dtcoletores');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    //
                    if (select.length == 1) {
                        //
                        var tablealertas = $('#dtalertas')
                        tablealertas.bootstrapTable('hideColumn', 'Valor');
                        tablealertas.bootstrapTable('hideColumn', 'Id');

                        m.g.setcomboval('slempresacoletor', select[0].Id_Empresa);
                        $('input[id=txtdesccoletor').val(select[0].Descricao);
                        $('input[id=txtmaccoletor').val(select[0].MAC);
                        if (select[0].Alerta == 1) {
                            m.g.setcomboval('slcoletorgeraalerta', 1);
                            m.coletor.setenabdisabbtnalerta(false);
                        }
                        else {
                            m.g.setcomboval('slcoletorgeraalerta', 0);
                            m.coletor.setenabdisabbtnalerta(true);
                        }  
                        $('#divalertas').css('display', 'inline');
                        $('#modalColetorLabel').text('Editar Coletor');
                        //
                        m.coletor.loadalertascoletor();
                    }
                    else {
                        m.coletor.loadingout();
                        bootbox.alert({
                            size: "small",
                            title: "Restrição",
                            message: "Selecione apenas um coletor para editar.",
                            centerVertical: true
                        });
                    }
                }
                else {
                    m.coletor.loadingout();
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Selecione um coletor para editar.",
                        centerVertical: true
                    });
                }
            },
            setenabdisabbtnalerta: function (disabled) {
                d.getid('btnnovoalerta').disabled = disabled;
                d.getid('btneditaralerta').disabled = disabled;
                d.getid('btnexcluiralerta').disabled = disabled;
                var tablealertas = $('#dtalertas')
                tablealertas.bootstrapTable('hideColumn', 'Valor');
                tablealertas.bootstrapTable('hideColumn', 'Id');
                //
                var inputs = document.getElementById('dtalertas').getElementsByTagName('input');
                for (var i = 0; i < inputs.length; ++i)
                    inputs[i].disabled = disabled;
            },
            chgcmbgeraalertacoletor: function () {
                id = $(d.getid('slcoletorgeraalerta')).children(":selected").attr("id");
                //
                if (id == '1') {
                    //
                    m.coletor.setenabdisabbtnalerta(false);
                }
                else {
                    m.coletor.setenabdisabbtnalerta(true);
                }
            },
            editalerta: function () {
                //                
                var tableAlertas = $('#dtalertas');
                var selectedAlerta = tableAlertas.bootstrapTable('getSelections');
                if (selectedAlerta && selectedAlerta.length > 0) {
                    if (selectedAlerta.length == 1) {
                        //
                        m.coletor.l = m.g.load('Carregando alerta...');
                        $('#modalalertatituloLabel').text('Editar Alerta');
                        m.coletor.editaralerta = true;
                        //                        
                        m.g.remove_options(d.getid('slalertatipo'));
                        var id_empresa = $(d.getid('slempresacoletor')).children(":selected").attr("id");
                        //
                        $.get(m.g.rsvlurl('coletor/pegacoletortipoalerta') + '?idempresa=' + id_empresa, function (data) {                            
                            //
                            if (data.ret && data.ret == 'ok') {
                                //
                                data.lista_retorno.forEach(function (entry) {
                                    //
                                    var x = d.getid('slalertatipo');
                                    var option = d.createElement('option');
                                    option.text = entry.Descricao;
                                    option.id = entry.Id;
                                    x.add(option);
                                });
                                //
                                var tableAlertas = $('#dtalertas');
                                var selectedAlerta = tableAlertas.bootstrapTable('getSelections');
                                //
                                $('input[id=txtalertavalor').val(selectedAlerta[0].Valor);
                                $('input[id=txtalertaemail').val(selectedAlerta[0].Email);
                                m.g.setcomboval('slalertatipo', selectedAlerta[0].Id_TipoAlerta);
                                m.g.setcomboval('slalertaregra', selectedAlerta[0].Regra);
                                m.g.setcomboval('slalertaativo', selectedAlerta[0].Ativo);
                            }
                            else {
                                alert('Erro: ' + data.ret);
                                bootbox.alert({
                                    size: "large",
                                    title: "Erro",
                                    message: "Ocorreu um erro ao tentar buscar os tipos de alerta, erro: <br />" + data.erro,
                                    centerVertical: true
                                });
                            }
                            //
                            m.coletor.loadingout();
                        });
                        //
                        $('#idalertamodal').modal('show');
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restrição",
                            message: "Selecione apenas um alerta para editar.",
                            centerVertical: true
                        });
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Selecione um alerta para editar.",
                        centerVertical: true
                    });
                }
            },
            novoalerta: function () {
                //
                $('input[id=txtalertavalor').val('');
                $('input[id=txtalertaemail').val('');
                $('#modalalertatituloLabel').text('Incluir Alerta');
                m.coletor.editaralerta = false;
                m.g.setcomboval('slalertaregra', 1);
                m.g.remove_options(d.getid('slalertatipo'));                
                var id_empresa = $(d.getid('slempresacoletor')).children(":selected").attr("id");
                //
                $.get(m.g.rsvlurl('coletor/pegacoletortipoalerta') + '?idempresa=' + id_empresa, function (data) {
                    if (data.ret && data.ret == 'ok') {
                        //
                        data.lista_retorno.forEach(function (entry) {
                            //
                            var x = d.getid('slalertatipo');
                            var option = d.createElement('option');
                            option.text = entry.Descricao;
                            option.id = entry.Id;
                            x.add(option);
                        });                        
                    }
                    else {
                        alert('Erro: ' + data.ret);
                        bootbox.alert({
                            size: "large",
                            title: "Erro",
                            message: "Ocorreu um erro ao tentar buscar os tipos de alerta, erro: <br />" + data.erro,
                            centerVertical: true
                        });
                    }
                });
                //
                $('#idalertamodal').modal('show');
            },
            novoalertasalva: function () {
                //                
                var tableColetores = $('#dtcoletores');
                var selectColetor = tableColetores.bootstrapTable('getSelections');
                var tipo_alerta = $(d.getid('slalertatipo')).children(":selected").attr("id");
                var regra = $(d.getid('slalertaregra')).children(":selected").attr("id");
                var ativa = $(d.getid('slalertaativo')).children(":selected").attr("id");
                var valor = $('input[id=txtalertavalor').val();
                var email = $('input[id=txtalertaemail').val();
                var idcoletoralerta = 0;
                //
                if (!tipo_alerta) {
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Informe o tipo da regra",
                        centerVertical: true
                    });
                    return;
                }
                //
                if (!$('input[id=txtalertavalor').val()) {
                    //
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Informe o valor da regra",
                        centerVertical: true
                    });
                    return;
                }
                //
                m.coletor.l = m.g.load('Salvando alerta...');
                //
                if (m.coletor.editaralerta) {
                    var tableAlertas = $('#dtalertas');
                    var selectAlertas = tableAlertas.bootstrapTable('getSelections');
                    idcoletoralerta = selectAlertas[0].Id.replace("'", "").replace("'", "");
                }
                //
                var url = m.g.rsvlurl('coletor/salvacoletoralerta') + '?idcoletoralerta=' + idcoletoralerta +
                    '&idempresa=' + selectColetor[0].Id_Empresa +
                    '&idcoletor=' + selectColetor[0].Id +
                    '&idtipoalerta=' + tipo_alerta +
                    '&idregra=' + regra +
                    '&valor=' + valor +
                    '&email=' + email +
                    '&ativo=' + ativa;

                $.get(url, function (data) {
                    //
                    if (data.ret && data.ret == 'ok') {
                        //
                        $('#idalertamodal').modal('hide');
                        //
                        m.coletor.loadalertascoletor();                        
                    }
                    else {
                        alert('Erro: ' + data.ret);
                        bootbox.alert({
                            size: "large",
                            title: "Erro",
                            message: "Ocorreu um erro ao tentar salvar o alerta, erro: <br />" + data.erro,
                            centerVertical: true
                        });
                    }
                });
            },
            excluilerta: function () {
                var table = $('#dtalertas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    if (select.length == 1) {
                        //
                        bootbox.confirm({
                            title: "Excluir Alerta?",
                            message: "Você deseja realmente excluir o alerta ('" + select[0].Descricao + "')? <br />Esta ação não poderá ser disfeita.",
                            centerVertical: true,
                            buttons: {
                                cancel: {
                                    label: '<i class="fa fa-times"></i> Cancelar'
                                },
                                confirm: {
                                    label: '<i class="fa fa-check"></i> Confirmar'
                                }
                            },
                            callback: function (result) {
                                //
                                if (result) {
                                    //
                                    var tableColetores = $('#dtcoletores');
                                    var selectColetor = tableColetores.bootstrapTable('getSelections');
                                    var table = $('#dtalertas');
                                    var select = table.bootstrapTable('getSelections');
                                    //
                                    var url = m.g.rsvlurl('coletor/excluicoletoralertapost') + '?idalerta=' + select[0].Id.replace("'", "").replace("'", "") +
                                        '&idempresa=' + selectColetor[0].Id_Empresa +
                                        '&idcoletor=' + selectColetor[0].Id;
                                    //
                                    m.coletor.l = m.g.load('Excluindo alerta...');
                                    $.get(url, function (data) {
                                        //
                                        m.coletor.loadingout();
                                        if (data.ret && data.ret == 'ok') {
                                            m.coletor.loadalertascoletor();
                                        }
                                        else if (data.ret && data.ret == 'nao_encontrada')
                                        {
                                            bootbox.alert({
                                                size: "large",
                                                title: "Erro",
                                                message: "Não foi possível encontrar o alerta",
                                                centerVertical: true
                                            });
                                        }
                                        else {
                                            bootbox.alert({
                                                size: "large",
                                                title: "Erro",
                                                message: "Ocorreu um erro ao tentar excluir a máquina, erro: " + data.erro,
                                                centerVertical: true
                                            });
                                        }
                                    });
                                }
                            }
                        });                     

                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restrição",
                            message: "Selecione apenas um alerta para excluir.",
                            centerVertical: true
                        });
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Selecione um alerta para excluir.",
                        centerVertical: true
                    });
                }
            },
            loadcoletores: function () {
                $.get(m.g.rsvlurl('coletor/carregadados'), function (data) {
                    if (data && data.data == 'ok') {
                        var $table = $('#dtcoletores');
                        $table.bootstrapTable('load', data.lista_coletor);
                        m.coletor.loadingout();
                    }
                    else {
                        m.coletor.loadingout();
                        alert('Erro: ' + data.ret);
                    }
                });
            },
            loadalertascoletor: function () {
                var table = $('#dtcoletores');
                var select = table.bootstrapTable('getSelections');
                if (select) {
                    $.get(m.g.rsvlurl('coletor/pegacoletoralerta') + '?idempresa=' + select[0].Id_Empresa + '&idcoletor=' + select[0].Id, function (data) {
                        if (data.data && data.data == 'ok') {
                            //
                            var tablealertas = $('#dtalertas');
                            var dados = [];
                            data.lista_retorno.forEach(function (entry) {
                                //
                                var data =
                                {
                                    'Id': "'" + entry.Id + "'" ,
                                    'Descricao': entry.Descricao,
                                    'Email': entry.Email,
                                    'AtivoDescricao': entry.AtivoDescricao,
                                    'Valor': entry.Valor,
                                    'Regra': entry.Regra,
                                    'Ativo': entry.Ativo,
                                    'Id_TipoAlerta': entry.Id_TipoAlerta
                                }
                                dados.push(data);
                            });
                            //
                            if (!m.coletor.alertaedit) {
                                m.coletor.alertaedit = true;
                                //
                                if (!m.coletor.alertainit) {
                                    m.coletor.alertainit = true;
                                    tablealertas.bootstrapTable({ data: dados });
                                }
                                else {
                                    if (!m.coletor.alertainit) {
                                        m.coletor.alertainit = true;
                                        tablealertas.bootstrapTable({ data: dados });
                                    }
                                    else {
                                        tablealertas.bootstrapTable('load', dados);
                                    }
                                }
                            }
                            else {
                                tablealertas.bootstrapTable('load', dados);
                            }
                            //
                            tablealertas.bootstrapTable('hideColumn', 'Valor');
                            tablealertas.bootstrapTable('hideColumn', 'Id');
                            m.coletor.loadingout();
                            //
                            $('#idcoletormodal').modal('show');
                        }
                        else {
                            m.coletor.loadingout();
                            bootbox.alert({
                                size: "large",
                                title: "Erro",
                                message: "Ocorreu um erro ao tentar carregar os alertas, erro: " + data.erro,
                                centerVertical: true
                            });
                        }
                    });
                }
            },
            loadingout: function () {
                try { m.coletor.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.tipoalerta = (function () {
        return {
            l: null,
            init: function (data) {
                var tableTiposAlertas = $('#dataTabletiposAlertas')
                tableTiposAlertas.bootstrapTable({ data: data });
                if (m.sessao.tipo_empresa_logado != 1) {
                    tableTiposAlertas.bootstrapTable('hideColumn', 'DescricaoEmpresa');
                }
            },
            edittipoalerta: function () {
                var tableTipoAlerta = $('#dataTabletiposAlertas');
                var selectTipoAlerta = tableTipoAlerta.bootstrapTable('getSelections');
                if (selectTipoAlerta && selectTipoAlerta.length > 0) {
                    if (selectTipoAlerta.length == 1) {
                        m.tipoalerta.settipoalertaedita(selectTipoAlerta)
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um tipo de alerta para editar.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione um tipo de alerta para editar.",
                        centerVertical: true
                    });
                }
            },
            settipoalertaedita: function (selectTipoAlerta) {
                $('.modal-title').text('Editar Tipo Alerta');
                $('#txtdesctipoalerta').val(selectTipoAlerta[0].Descricao);
                $('#txtunidademedida').val(selectTipoAlerta[0].UnidadeMedida);
                //
                if (selectTipoAlerta[0].AtivoGrid == 'Sim') {
                    m.g.setcomboval('sltipoalertaativo', 1);
                }
                else {
                    m.g.setcomboval('sltipoalertaativo', 0);
                }
                //
                m.g.setcomboval('slempresatipoalerta', selectTipoAlerta[0].Id_Empresa);
                m.g.setcomboval('slopcaoestipoalerta', selectTipoAlerta[0].Id_Tipo);
                //
                d.getid('slempresatipoalerta').disabled = true;
                //
                $('#tipoaAlertaModal').modal('show');
            },
            settipoalertainclui: function () {
                $('.modal-title').text('Incluir Tipo Alerta');
                $('#txtdesctipoalerta').val('');
                $('#txtunidademedida').val('');
                d.getid('slempresatipoalerta').disabled = false;
                m.g.setcomboval('sltipoalertaativo', 1);
                $('#tipoaAlertaModal').modal('show');
            },
            salva: function () {
                //
                var descricao = $('#txtdesctipoalerta').val();
                var unidademedida = $('#txtunidademedida').val();                
                //
                if (!descricao) {
                    //
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Informe a descrição do tipo de alerta.",
                        centerVertical: true
                    });
                    return;
                }
                //
                if (!unidademedida) {
                    //
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Informe a unidade de medida do tipo de alerta.",
                        centerVertical: true
                    });
                    return;
                }
                //
                m.tipoalerta.l = m.g.load('Salvando tipo alerta...');
                //
                var tableTipoAlerta = $('#dataTabletiposAlertas');
                var selectTipoAlerta = tableTipoAlerta.bootstrapTable('getSelections');
                var id_tipoalerta = 0;
                if (selectTipoAlerta.length > 0) {
                    id_tipoalerta = selectTipoAlerta[0].Id;
                }
                //
                var url = m.g.rsvlurl('tipoalerta/tipoalertapost') + '?id_tipoalerta=' + id_tipoalerta +
                    '&id_empresa=' + $(d.getid('slempresatipoalerta')).children(":selected").attr("id") +
                    '&descricao=' + descricao +
                    '&unidade_medida=' + unidademedida +
                    '&ativo=' + $(d.getid('sltipoalertaativo')).children(":selected").attr("id") + 
                    '&tipo=' + $(d.getid('slopcaoestipoalerta')).children(":selected").attr("id");

                $.get(url, function (data) {
                    //
                    if (data.ret && data.ret == 'ok') {
                        //
                        $('#tipoaAlertaModal').modal('hide');
                        m.tipoalerta.loadtiposalertas();
                    }
                    else {
                        m.tipoalerta.loadingout();
                        bootbox.alert({
                            size: "large",
                            title: "Erro",
                            message: "Ocorreu um erro ao tentar salvar o tipo alerta, erro: <br />" + data.erro,
                            centerVertical: true
                        });
                    }
                });
            },
            exclui: function () {
                var tableTipoAlerta = $('#dataTabletiposAlertas');
                var selectTipoAlerta = tableTipoAlerta.bootstrapTable('getSelections');
                if (selectTipoAlerta && selectTipoAlerta.length > 0) {
                    if (selectTipoAlerta.length == 1) {
                        //
                        bootbox.confirm({
                            title: "Excluir Tipo Alerta?",
                            message: "Você deseja realmente excluir o Tipo Alerta ('" + selectTipoAlerta[0].Descricao + "')? <br />Esta ação não poderá ser disfeita.",
                            centerVertical: true,
                            buttons: {
                                cancel: {
                                    label: '<i class="fa fa-times"></i> Cancelar'
                                },
                                confirm: {
                                    label: '<i class="fa fa-check"></i> Confirmar'
                                }
                            },
                            callback: function (result) {
                                //
                                if (result) {
                                    m.tipoalerta.l = m.g.load('Excluindo tipo alerta...');
                                    var url = m.g.rsvlurl('tipoalerta/excluitipoalerta') + '?idtipoalerta=' + selectTipoAlerta[0].Id +
                                        '&idempresa=' + selectTipoAlerta[0].Id_Empresa;
                                    //
                                    $.get(url, function (data) {
                                        //
                                        if (data.ret && data.ret == 'ok') {
                                            //
                                            m.tipoalerta.loadtiposalertas();
                                        }
                                        else if (data.ret && data.ret == 'coletoralerta') {
                                            m.tipoalerta.loadingout();
                                            bootbox.alert({
                                                size: "large",
                                                title: "Restrição",
                                                message: "Este tipo de alerta já está associado com um alerta, desassocie ele para excluir.",
                                                centerVertical: true
                                            });
                                        }
                                        else if (data.ret && data.ret == 'nok') {
                                            m.tipoalerta.loadingout();
                                            bootbox.alert({
                                                size: "large",
                                                title: "Erro",
                                                message: "Não foi possível encontrar o tipo alerta.",
                                                centerVertical: true
                                            });
                                        }
                                        else {
                                            m.tipoalerta.loadingout();
                                            bootbox.alert({
                                                size: "large",
                                                title: "Erro",
                                                message: "Ocorreu um erro ao tentar salvar o tipo alerta, erro: <br />" + data.erro,
                                                centerVertical: true
                                            });
                                        }
                                    });
                                }
                            }
                        });
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um tipo de alerta para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione um tipo de alerta para excluir.",
                        centerVertical: true
                    });
                }
            },
            loadtiposalertas: function () {
                //
                var url = m.g.rsvlurl('tipoalerta/pegatipoasalertas');
                //
                $.get(url, function (data) {
                    //
                    m.tipoalerta.loadingout();
                    if (data.ret && data.ret == 'ok') {
                        //
                        var $table = $('#dataTabletiposAlertas');
                        $table.bootstrapTable('load', data.lista_retonro);
                        $('#tipoaAlertaModal').modal('hide');
                    }
                    else {
                        bootbox.alert({
                            size: "large",
                            title: "Erro",
                            message: "Ocorreu um erro ao tentar carregar os tipos de alerta, erro: <br />" + data.erro,
                            centerVertical: true
                        });
                    }
                });
            },
            loadingout: function () {
                try { m.tipoalerta.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.gateway = (function () {
        return {
            l: null,
            idgateway: null,
            init: function () {
                var table = $('#dtgateways').DataTable({
                    responsive: true
                });
                $('#dtgateways tbody').on('click', 'tr', function () {
                    $('input[id=txtdescgateway').val(table.row(this).data()[1]);
                    $('input[id=txtmacgateway').val(table.row(this).data()[2]);
                    idgateway = table.row(this).data()[0];
                    $('.modal-title').text('Alterar Gateway');
                    $('#btnexcluirgateway').css('display', 'inline');
                    $('#gatewayModal').modal("show");
                });
            },
            salva: function () {
                //
                if (!$('input[id=txtdescgateway').val()) {
                    bootbox.alert({
                        size: "large",
                        title: "Restrição",
                        message: "Informe a descrição do Gateway",
                        centerVertical: true
                    });
                    return;
                }
                if (!$('input[id=txtmacgateway').val()) {
                    bootbox.alert({
                        size: "large",
                        title: "Restrição",
                        message: "Informe o MAC do Gateway",
                        centerVertical: true
                    });
                    return;
                }
                m.g.load('Salvando');
                //
                var url = m.g.rsvlurl('gateway/gatewaypost') + '?descricao=' + d.getid('txtdescgateway').value + '&mac=' + d.getid('txtmacgateway').value + '&idgateway=' + idgateway;
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        if (resp && resp.data == 'ok') {
                            //
                            window.location = m.g.rsvlurl('gateway/index/');
                        }
                        else {
                            try { l.out() } catch (err) { }
                            $('#maquinaModal').modal('hide');
                            bootbox.alert({
                                size: "large",
                                title: "Erro",
                                message: 'Ocorreu um erro ao tentar incluir a máquina. Erro: ' + data.erro,
                                centerVertical: true
                            });
                        }
                        $('#gatewayModal').modal('hide');
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            exclui: function () {
                $('#modalgatewayExcluiLabel').text('Excluir Gateway');
                $('#excluiGatewayModal').modal('show');
            },
            excluipost: function () {
                $('#maquinaModal').modal("hide");
                $('#excluiGatewayModal').modal("hide");
                l = m.g.load('Excluindo...');
                $.get(m.g.rsvlurl('gateway/excluigatewaypost') + '?idgateway=' + idgateway, function (data) {
                    if (data.ret && data.ret == 'ok') {
                        location.href = m.g.rsvlurl('gateway/index/');
                    }
                    else {
                        try { l.out() } catch (err) { }
                        alert('Erro: ' + data.ret);
                    }
                });
            },
            setgatewayinclui: function () {
                idgateway = 0;
                $('.modal-title').text('Incluir Gateway');
                $('input[id=txtdescgateway').val('');
                $('input[id=txtmacgateway').val('');
                $('#btnexcluirgateway').css('display', 'none');
            }
        }
    })()
    //
    _m.usuario = (function () {
        return {
            init: function (data) {
                //
                var tableusuario = $('#dataTableUsusario')
                tableusuario.bootstrapTable({ data: data });
            },
            setusuarioinclui: function () {
                $('.modal-title').text('Incluir usuário');
                $('#usuarioModal').modal('show');
                //
                $('#txtnomeusuario').val('');
                $('#txtemailusuario').val('');
                $('#txtsenha1usuario').val('');
                $('#txtsenha2usuario').val('');
            },
            edit: function () {
                var tableusuario = $('#dataTableUsusario');
                var selectUsuario = tableusuario.bootstrapTable('getSelections');
                if (selectUsuario && selectUsuario.length > 0) {
                    //
                    if (selectUsuario.length == 1) {
                        //
                        $('.modal-title').text('Editar Usuário');
                        $('#usuarioModal').modal('show');
                        //
                        m.g.setcomboval('slempresausuario', selectUsuario[0].Id_Empresa);
                        d.getid('slempresausuario').disabled = true;
                        $('#txtnomeusuario').val(selectUsuario[0].Nome);
                        $('#txtemailusuario').val(selectUsuario[0].Email);
                        $('#txtsenha1usuario').val('');
                        $('#txtsenha2usuario').val('');
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um usuário para editar.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione um usuário para editar.",
                        centerVertical: true
                    });
                }
            },
            salva: function () {
                //
                var txtnomeusuario = $('#txtnomeusuario').val();
                var txtemailusuario = $('#txtemailusuario').val();
                var txtsenha1usuario = $('#txtsenha1usuario').val();
                var txtsenha2usuario = $('#txtsenha2usuario').val();
                //
                if (!txtnomeusuario) {
                    m.g.alert('Restriçao', 'Informe o nome do usuário.');
                    return;
                }
                //
                if (!txtemailusuario) {
                    m.g.alert('Restriçao', 'Informe o email do usuário.');
                    return;
                }
                //
                var tableUsuarios = $('#dataTableUsusario');
                var selectUsuarios = tableUsuarios.bootstrapTable('getSelections');
                var idusuario = 0;
                if (selectUsuarios.length > 0) {
                    idusuario = selectUsuarios[0].ID;
                }
                //
                if (idusuario == 0) {
                    //
                    if (!txtsenha1usuario) {
                        m.g.alert('Restriçao', 'Informe a senha do usuário.');
                        return;
                    }
                    //
                    if (txtsenha1usuario != txtsenha2usuario) {
                        m.g.alert('Restriçao', 'As senhas informadas são diferentes.');
                        return;
                    }
                }
                else {
                    if (txtsenha1usuario != txtsenha2usuario) {
                        m.g.alert('Restriçao', 'As senhas informadas são diferentes.');
                        return;
                    }
                }
                //
                var idempresa = $(d.getid('slempresausuario')).children(":selected").attr("id");
                //
                m.usuario.l = m.g.load('Salvando usuário...');
                //
                var url = m.g.rsvlurl('usuario/usuariopost') +
                    '?idusuario=' + idusuario +
                    '&idempresa=' + idempresa +
                    '&nome=' + txtnomeusuario +
                    '&email=' + txtemailusuario +
                    '&pass=' + txtsenha1usuario;
                //
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        //
                        if (resp && resp.data == 'ok') {
                            //
                            m.usuario.loadusuarios();
                        }
                        else {
                            try {
                                m.usuario.loadingout();
                                $('#usuarioModal').modal('hide');
                                bootbox.alert({
                                    size: "large",
                                    title: "Erro",
                                    message: 'Ocorreu um erro ao tentar incluir o usuário. Erro: ' + resp.erro,
                                    centerVertical: true
                                });
                            } catch (err) { }

                        }
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            exclui: function () {
                var tableUsuarios = $('#dataTableUsusario');
                var selectUsuarios = tableUsuarios.bootstrapTable('getSelections');
                if (selectUsuarios && selectUsuarios.length > 0) {
                    //
                    if (selectUsuarios.length == 1) {
                        bootbox.confirm({
                            title: "Excluir Usuário?",
                            message: "Você deseja realmente excluir o usuário ('" + selectUsuarios[0].Nome + "')? <br />Esta ação não poderá ser disfeita.",
                            centerVertical: true,
                            buttons: {
                                cancel: {
                                    label: '<i class="fa fa-times"></i> Cancelar'
                                },
                                confirm: {
                                    label: '<i class="fa fa-check"></i> Confirmar'
                                }
                            },
                            callback: function (result) {
                                //
                                if (result) {
                                    m.usuario.l = m.g.load('Excluindo usuário...');
                                    $.get(m.g.rsvlurl('usuario/excluiusuariopost') + '?idempresa=' + selectUsuarios[0].Id_Empresa + '&idusuario=' + selectUsuarios[0].ID, function (data) {
                                        if (data.ret && data.ret == 'ok') {
                                            //
                                            m.usuario.loadusuarios();
                                        }
                                        if (data.ret && data.ret == 'usu') {
                                            //
                                            m.usuario.loadingout();
                                            bootbox.alert({
                                                size: "large",
                                                title: "Atenção",
                                                message: "Você não pode se excluir!",
                                                centerVertical: true
                                            });
                                        }
                                        else {
                                            m.usuario.loadingout();
                                            bootbox.alert({
                                                size: "large",
                                                title: "Erro",
                                                message: "Ocorreu um erro ao tentar excluir a usuário, erro: " + data.erro,
                                                centerVertical: true
                                            });
                                        }
                                    });
                                }
                            }
                        });
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um usuário para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione um usuário para excluir.",
                        centerVertical: true
                    });
                }
            },
            loadusuarios: function () {
                //
                $.get(m.g.rsvlurl('usuario/carregausuarios'), function (data) {
                    if (data && data.data == 'ok') {
                        //
                        var tableUsuarios = $('#dataTableUsusario')
                        tableUsuarios.bootstrapTable('load', data.lista_retorno);
                        //
                        $('#usuarioModal').modal('hide');
                        m.usuario.loadingout();
                    }
                    else {
                        m.usuario.loadingout();
                        m.g.alert('Erro', 'Ocorreu um erro ao salvar o usuário. Erro:' + data.erro);
                    }
                });
            },
            loadingout: function () {
                try { m.usuario.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.logatividade = (function () {
        return {
            binit: false,
            init: function () {
                m.logatividade.binit = false;
                $('#slmaquinaslogatividade').select2({
                    placeholder: "Selecione a(s) máquina(s)",
                    width: "100%"
                })
            },
            carrega: function () {
                //
                var list_ids = [];
                var list_emp = [];
                if ($('#slmaquinaslogatividade').select2('val') && $('#slmaquinaslogatividade').select2('val').length > 0) {
                    //
                    for (var i = 0; i < $('#slmaquinaslogatividade').find(':selected').length; i++) {
                        //
                        list_ids.push($('#slmaquinaslogatividade').find(':selected')[i].id);
                        list_emp.push($(d.getid('slmaquinaslogatividade')).children(":selected").attr("data-idempresa"));
                    }
                    //
                    m.logatividade.l = m.g.load('Carregando log de atividades...');
                    //
                    var totreg = $('#sllogativtotreg').find(':selected').val();
                    var chkmaq = $('input[id="chkmaq"]:checked').length > 0;
                    var chkpress = $('input[id="chkpress"]:checked').length > 0;
                    var chktemp = $('input[id="chktemp"]:checked').length > 0;
                    var chkprod = $('input[id="chkprod"]:checked').length > 0;
                    //        
                    var url = m.g.rsvlurl('logatividade/pegalogatividade') +
                        '?list_emp=' + list_emp +
                        '&lista_ids=' + list_ids +
                        '&chkmaq=' + chkmaq +
                        '&chkpress=' + chkpress +
                        '&chktemp=' + chktemp +
                        '&chkprod=' + chkprod +
                        '&totreg=' + totreg;
                    //
                    $.ajax({
                        url: url,
                        type: "post",
                        success: function (resp) {
                            //
                            if (resp && resp.ret == 'ok') {
                                //
                                var tableLogAtividade = $('#dataTableLogAtividadeMaquina')
                                //
                                if (m.logatividade.binit) {
                                    tableLogAtividade.bootstrapTable('load', resp.lista_retorno);
                                }
                                else {
                                    tableLogAtividade.bootstrapTable({ data: resp.lista_retorno });
                                    m.logatividade.binit = true;
                                }                                
                                m.logatividade.loadingout();
                            }
                            else {
                                if (resp.ret == 'nok') {
                                    bootbox.alert({
                                        size: "large",
                                        title: "Erro",
                                        message: 'Ocorreu um erro ao tentar consultar o log de atividades. Erro: ' + resp.erro,
                                        centerVertical: true
                                    });
                                }
                                else {
                                    bootbox.alert({
                                        size: "large",
                                        title: "Consulta Log Atividade",
                                        message: 'A consulta não retornou dados!',
                                        centerVertical: true
                                    });
                                }
                                m.logatividade.loadingout();

                            }
                        },
                        error: function (xhr, error) {
                            console.log(xhr);
                            console.log(error);
                        }
                    });
                }
                else {
                    m.g.alert('Restriçao', 'Informe a máquina.');
                }
            },
            loadingout: function () {
                try { m.logatividade.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.pressao = (function () {
        return {
            grafico: null,
            load: null,
            list_ids: [],
            carrega_maquina: function () {
                //
                if ($('#slpressao').select2('val') && $('#slpressao').select2('val').length > 0) {
                    //
                    for (var i = 0; i < $('#slpressao').find(':selected').length; i++) {
                        //
                        m.pressao.list_ids.push($('#slpressao').find(':selected')[i].id);
                    }
                    //
                    m.pressao.load = m.g.load('Carregando gráfico pressão...');
                    //
                    $.ajax({
                        //RelatorioTemperatura
                        url: m.g.rsvlurl('Grafico/PressaoMaquina') + '?lista_ids=' + m.pressao.list_ids +
                            '&periodo=' + d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id,
                        type: "post",
                        success: function (resp) {
                            //
                            if (resp && resp.data == 'ok') {
                                //
                                m.pressao.grafico_pressao(resp.lista_dados_retorno, resp.lista_labels, d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id);
                            }
                            else {
                                $('#msg')[0].innerHTML = 'Ocorreu um erro ao tentar salvar a conta.';
                                $("#btnModal").click();
                            }
                        },
                        error: function (xhr, error) {
                            console.log(xhr);
                            console.log(error);
                        }
                    });
                }
                else {
                    m.g.alert('Restriçao', 'Informe a máquina.');
                }
            },
            grafico_pressao: function (lista_dados, lista_labels, tipo) {
                //
                m.pressao.load.out();
                // Set new default font family and font color to mimic Bootstrap's default styling
                Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
                Chart.defaults.global.defaultFontColor = '#858796';

                // Area Chart Example
                var ctx = d.getid("pressaochart");
                if (m.pressao.grafico) {
                    m.pressao.grafico.destroy();
                }
                //
                lista_dados = [];
                //
                if (tipo != '5') {
                    for (var i = 0; i < m.pressao.list_ids.length; i++) {
                        lista_dados[i] = [12];
                        //
                        switch (tipo) {

                            case '1':
                                break;
                        }

                        if (m.pressao.list_ids[i] == '57') { // Injetora Magna 2000T
                            lista_dados[i] = [448,	225,	319,	524,	575,	252,	399,	335,	518,	457,	464,	527] ;
                        }
                        else if (m.pressao.list_ids[i] == '58') { // Injetora Phoenix 3000T
                            lista_dados[i] = [532,	358,	477, 349,	387,	433,	250,	339,	434,	329,	402,	433];
                        }
                        else if (m.pressao.list_ids[i] == '59') { // Extrusora Carnevalli 60mm
                            lista_dados[i] = [447,	606,	743,	469,	790,	530,	550,	644,	767,	607,	698,	600];
                        }
                        else if (m.pressao.list_ids[i] == '60') { // Injetora EKII 4000T
                            lista_dados[i] = [649,	451,	604,	517,	596,	655,	641,	641,	485,	615,	661,	694];
                        }
                        else if (m.pressao.list_ids[i] == '61') { // Enchedoras Tribloco
                            lista_dados[i] = [406,	388,	425,	275,	489,	255,	270,	441,	237,	2145,	268,	409];
                        }
                        else if (m.pressao.list_ids[i] == '62') { // Tribloco Automática Carbo L 500
                            lista_dados[i] = [283,	439,	279,	364,	278,	318,	373,	407,	435,	316,	221,	482];
                        }
                    }
                }
                //
                for (var i = 0; i < length; i++) {

                }
                //
                if (lista_dados) {
                    //
                    lista_labels = lista_labels.reverse();
                    m.pressao.grafico = new Chart(ctx, {
                        type: 'line',
                        data: {
                            //labels: ["Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],   // aunal
                            //labels: ["Sexta", "Sabado", "Domingo", "Segunda-Feira", "Terca-Feira", "Quarta-Feira", "Quinta-Feira"],                                  // semanal
                            //labels: ["19:00", "21:00", "22:00", "00:00", "02:00", "05:00", "07:00", "09:00", "11:00", "13:00", "15:00", "17:00"],                    // diário
                            //labels: lista_labels.reverse(),
                            labels: lista_labels,
                            datasets: []
                        },
                        options: {
                            maintainAspectRatio: false,
                            layout: {
                                padding: {
                                    left: 10,
                                    right: 25,
                                    top: 25,
                                    bottom: 0
                                }
                            },
                            scales: {
                                xAxes: [{
                                    time: {
                                        unit: 'date'
                                    },
                                    gridLines: {
                                        display: false,
                                        drawBorder: false
                                    },
                                    ticks: {
                                        maxTicksLimit: 7
                                    }
                                }],
                                yAxes: [{
                                    ticks: {
                                        maxTicksLimit: 5,
                                        padding: 10,
                                        // Include a dollar sign in the ticks
                                        callback: function (value, index, values) {
                                            return 'Bar ' + m.g.number_format(value);
                                        }
                                    },
                                    gridLines: {
                                        color: "rgb(234, 236, 244)",
                                        zeroLineColor: "rgb(234, 236, 244)",
                                        drawBorder: false,
                                        borderDash: [2],
                                        zeroLineBorderDash: [2]
                                    }
                                }],
                            },
                            legend: {
                                display: true
                            },
                            tooltips: {
                                backgroundColor: "rgb(255,255,255)",
                                bodyFontColor: "#858796",
                                titleMarginBottom: 10,
                                titleFontColor: '#6e707e',
                                titleFontSize: 14,
                                borderColor: '#dddfeb',
                                borderWidth: 1,
                                xPadding: 15,
                                yPadding: 15,
                                displayColors: false,
                                intersect: false,
                                mode: 'index',
                                caretPadding: 10,
                                callbacks: {
                                    label: function (tooltipItem, chart) {
                                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                        return datasetLabel + ': ' + m.g.number_format(tooltipItem.yLabel) + ' Bar';
                                    }
                                }
                            }
                        }
                    });
                    //
                    for (var i = 0; i < lista_dados.length; i++) {
                        //
                        if (!lista_dados[i][i]) {
                            //
                            for (var j = 0; j < lista_dados[i][j].length; i++) {
                                //
                                lista_dados[i][j] = '0';
                            }
                        }
                        var rgba = 'rgba(' + m.g.rgba_graph()[i] + ')';
                        if (d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id == 5) {
                            lista_dados[i] = lista_dados[i].reverse();
                        }
                        //
                        m.pressao.grafico.data.datasets.push({
                            label: $('#slpressao').select2('val')[i],
                            lineTension: 0.3,
                            backgroundColor: "rgba(0,0,0,0.01)",
                            //borderColor: "rgba(78, 115, 223, 1)",
                            borderColor: rgba,
                            pointRadius: 3,
                            //pointBackgroundColor: "rgba(78, 115, 223, 1)",
                            pointBackgroundColor: rgba,
                            //pointBorderColor: "rgba(78, 115, 223, 1)",
                            pointBorderColor: rgba,
                            pointHoverRadius: 3,
                            //pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                            pointHoverBackgroundColor: rgba,
                            //pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                            pointHoverBorderColor: rgba,
                            pointHitRadius: 10,
                            pointBorderWidth: 2,
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // diario
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100]                               // semanal
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // anual
                            data: lista_dados[i]  // mensal
                        });
                    }
                    //
                    m.pressao.grafico.update();
                    m.pressao.list_ids = [];
                }
            }
        }
    })()
    //
    _m.grafico = (function () {
        return {
            grafico: null,
            load: null,
            list_ids: [],
            reportTempAtmos: function (maquinas, periodo) {
                let strBase64 = _m.grafico.grafico.toBase64Image();
                
                var xhr = new XMLHttpRequest();
                xhr.open("POST", m.g.rsvlurl('relatorio/RelatorioTemperaturaAtmosfera'), true);

                //Send the proper header information along with the request
                xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                xhr.onreadystatechange = function () { // Call a function when the state changes.
                    if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                        // Request finished. Do processing here.
                        document.getElementById('idframerelmaquina').src = 'http://localhost/connector/temp/' + JSON.parse(this.responseText).relatorio
                    }
                }
                xhr.send('idempresa=' + m.sessao.cd_empresa_logado + '&strimage1=' + strBase64 + '&periodo=' + periodo + '&maquinas=' + maquinas);
            },
            grafico_temperatura_report: function (lista_dados, lista_labels, maquinas, periodo) {
                //
                _m.grafico.grafico = new Chart(document.getElementById("idcanvastempreport"), {
                    type: 'line',
                    data: {
                        labels: lista_labels,
                        datasets: [{
                            data: lista_dados,
                            label: "Africa",
                            borderColor: "#3e95cd",
                            fill: false
                        }
                        //    , {
                        //    data: [282, 350, 411, 502, 635, 809, 947, 1402, 3700, 5267],
                        //    label: "Asia",
                        //    borderColor: "#8e5ea2",
                        //    fill: false
                        //}, {
                        //    data: [168, 170, 178, 190, 203, 276, 408, 547, 675, 734],
                        //    label: "Europe",
                        //    borderColor: "#3cba9f",
                        //    fill: false
                        //}, {
                        //    data: [40, 20, 10, 16, 24, 38, 74, 167, 508, 784],
                        //    label: "Latin America",
                        //    borderColor: "#e8c3b9",
                        //    fill: false
                        //}, {
                        //    data: [6, 3, 2, 2, 7, 26, 82, 172, 312, 433],
                        //    label: "North America",
                        //    borderColor: "#c45850",
                        //    fill: false
                        //}
                        ]
                    },
                    options: {
                        title: {
                            display: true,
                            text: 'World population per region (in millions)'
                        },
                        animation: {
                            onComplete: function () {
                                _m.grafico.reportTempAtmos(maquinas, periodo)
                            }
                        },
                        chartArea: {
                            backgroundColor: 'rgba(251, 85, 85, 0.4)'
                        }
                    }
                });
                

                /*
                console.log(strBase64);
                var xhr = new XMLHttpRequest();
                xhr.open("POST", m.g.rsvlurl('relatorio/RelatorioTemperaturaAtmosfera'), true);

                //Send the proper header information along with the request
                xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                xhr.onreadystatechange = function () { // Call a function when the state changes.
                    if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                        // Request finished. Do processing here.
                    }
                }
                xhr.send('idempresa=' + m.sessao.cd_empresa_logado + '&strimage1=' + strBase64);
                */
                /*
                let strBase64 = myChart.toBase64Image();
               // let blob = _m.grafico.makeblob(strBase64);
                let img1 = strBase64.substring(0, (strBase64.length / 2))
                let img2 = strBase64.substring((strBase64.length / 2), strBase64.length)
                //

                //
                */
                /*
                $.ajax({
                    url: m.g.rsvlurl('relatorio/RelatorioTemperaturaAtmosfera') + '?idempresa=' + m.sessao.cd_empresa_logado + '&strimage1=' + img1 + '&strimage2=' + img2,
                    processData: false,
                    contentType: 'application/octet-stream',
                    type: "POST",
                    success: function (resp) {
                        //
                        m.relatorio.load.out();
                        //
                        if (resp && resp.sreult == 'ok') {
                            //
                            _m.grafico.grafico_temperatura_report(xaxys, yaxys, label);
                            //document.getElementById('idframerelmaquina').src = 'http://localhost/connector/temp/' + resp.result.data
                            report = JSON.parse(resp.result).data;
                            document.getElementById('idframerelmaquina').src = location.href + "/Temp/" + report;


                        }
                        else {
                            $('#msg')[0].innerHTML = 'Ocorreu um erro ao tentar salvar a conta.';
                            $("#btnModal").click();
                        }
                    },
                    error: function (xhr, error) {
                        console.log('ERRO ', error)
                    }
                });
                */

                /*
                m.pressao.load.out();
                // Set new default font family and font color to mimic Bootstrap's default styling
                Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
                Chart.defaults.global.defaultFontColor = '#858796';

                // Area Chart Example
                var ctx = d.getid("idcanvastempreport");
                if (m.pressao.grafico) {
                    m.pressao.grafico.destroy();
                }
                //
                lista_dados = [];
                //
                if (tipo != '5') {
                    for (var i = 0; i < m.pressao.list_ids.length; i++) {
                        lista_dados[i] = [12];
                        //
                        switch (tipo) {

                            case '1':
                                break;
                        }

                        if (m.pressao.list_ids[i] == '57') { // Injetora Magna 2000T
                            lista_dados[i] = [448, 225, 319, 524, 575, 252, 399, 335, 518, 457, 464, 527];
                        }
                        else if (m.pressao.list_ids[i] == '58') { // Injetora Phoenix 3000T
                            lista_dados[i] = [532, 358, 477, 349, 387, 433, 250, 339, 434, 329, 402, 433];
                        }
                        else if (m.pressao.list_ids[i] == '59') { // Extrusora Carnevalli 60mm
                            lista_dados[i] = [447, 606, 743, 469, 790, 530, 550, 644, 767, 607, 698, 600];
                        }
                        else if (m.pressao.list_ids[i] == '60') { // Injetora EKII 4000T
                            lista_dados[i] = [649, 451, 604, 517, 596, 655, 641, 641, 485, 615, 661, 694];
                        }
                        else if (m.pressao.list_ids[i] == '61') { // Enchedoras Tribloco
                            lista_dados[i] = [406, 388, 425, 275, 489, 255, 270, 441, 237, 2145, 268, 409];
                        }
                        else if (m.pressao.list_ids[i] == '62') { // Tribloco Automática Carbo L 500
                            lista_dados[i] = [283, 439, 279, 364, 278, 318, 373, 407, 435, 316, 221, 482];
                        }
                    }
                }
                //
                if (lista_dados) {
                    //
                    lista_labels = lista_labels.reverse();
                    let graph = new Chart(ctx, {
                        type: 'line',
                        data: {
                            //labels: ["Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],   // aunal
                            //labels: ["Sexta", "Sabado", "Domingo", "Segunda-Feira", "Terca-Feira", "Quarta-Feira", "Quinta-Feira"],                                  // semanal
                            //labels: ["19:00", "21:00", "22:00", "00:00", "02:00", "05:00", "07:00", "09:00", "11:00", "13:00", "15:00", "17:00"],                    // diário
                            //labels: lista_labels.reverse(),
                            labels: lista_labels,
                            datasets: []
                        },
                        options: {
                            maintainAspectRatio: false,
                            layout: {
                                padding: {
                                    left: 10,
                                    right: 25,
                                    top: 25,
                                    bottom: 0
                                }
                            },
                            scales: {
                                xAxes: [{
                                    time: {
                                        unit: 'date'
                                    },
                                    gridLines: {
                                        display: false,
                                        drawBorder: false
                                    },
                                    ticks: {
                                        maxTicksLimit: 7
                                    }
                                }],
                                yAxes: [{
                                    ticks: {
                                        maxTicksLimit: 5,
                                        padding: 10,
                                        // Include a dollar sign in the ticks
                                        callback: function (value, index, values) {
                                            return 'Bar ' + m.g.number_format(value);
                                        }
                                    },
                                    gridLines: {
                                        color: "rgb(234, 236, 244)",
                                        zeroLineColor: "rgb(234, 236, 244)",
                                        drawBorder: false,
                                        borderDash: [2],
                                        zeroLineBorderDash: [2]
                                    }
                                }],
                            },
                            legend: {
                                display: true
                            },
                            tooltips: {
                                backgroundColor: "rgb(255,255,255)",
                                bodyFontColor: "#858796",
                                titleMarginBottom: 10,
                                titleFontColor: '#6e707e',
                                titleFontSize: 14,
                                borderColor: '#dddfeb',
                                borderWidth: 1,
                                xPadding: 15,
                                yPadding: 15,
                                displayColors: false,
                                intersect: false,
                                mode: 'index',
                                caretPadding: 10,
                                callbacks: {
                                    label: function (tooltipItem, chart) {
                                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                        return datasetLabel + ': ' + m.g.number_format(tooltipItem.yLabel) + ' Bar';
                                    }
                                }
                            }
                        }
                    });
                    //
                    for (var i = 0; i < lista_dados.length; i++) {
                        //
                        if (!lista_dados[i][i]) {
                            //
                            for (var j = 0; j < lista_dados[i][j].length; i++) {
                                //
                                lista_dados[i][j] = '0';
                            }
                        }
                        var rgba = 'rgba(' + m.g.rgba_graph()[i] + ')';
                        if (d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id == 5) {
                            lista_dados[i] = lista_dados[i].reverse();
                        }
                        //
                        m.pressao.grafico.data.datasets.push({
                            label: $('#slpressao').select2('val')[i],
                            lineTension: 0.3,
                            backgroundColor: "rgba(0,0,0,0.01)",
                            //borderColor: "rgba(78, 115, 223, 1)",
                            borderColor: rgba,
                            pointRadius: 3,
                            //pointBackgroundColor: "rgba(78, 115, 223, 1)",
                            pointBackgroundColor: rgba,
                            //pointBorderColor: "rgba(78, 115, 223, 1)",
                            pointBorderColor: rgba,
                            pointHoverRadius: 3,
                            //pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                            pointHoverBackgroundColor: rgba,
                            //pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                            pointHoverBorderColor: rgba,
                            pointHitRadius: 10,
                            pointBorderWidth: 2,
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // diario
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100]                               // semanal
                            //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // anual
                            data: lista_dados[i]  // mensal
                        });
                    }
                    //
                    m.pressao.grafico.update();
                    m.pressao.list_ids = [];
                    */
                
            },
            prestempprod: function () {
                //
                var maquina = document.getElementById('slmaquinas').value;
                //
                m.grafico.l = m.g.load('Carregando gráfico comparativo...');
                var e = document.getElementById("slmaquinas");
                $.get(m.g.rsvlurl('Grafico/GraficoPresTempProd') + '?idmaquina=' + e.options[e.selectedIndex].id + '&periodo=' + $('input[name=daterange]')[0].value, function (data) {
                    //
                    if (data.data = 'OK') {
                        //
                        var dados_temp = [];
                        var dados_pres = [];
                        var dados_prod = [];
                        //
                        try {
                            for (var i = 0; i < data.lista_temperatura[0].length; i++) {
                                dados_temp.push(parseFloat(data.lista_temperatura[0][i].replace(',', '.')));
                                dados_pres.push(parseFloat(data.lista_pressao[0][i].replace(',', '.')));
                                dados_prod.push(parseFloat(data.lista_producao[0][i]));
                            }
                            //
                            var labels = data.lista_labels.reverse();
                            dados_temp = dados_temp.reverse();
                            dados_pres = dados_pres.reverse();
                            dados_prod = dados_prod.reverse();
                            //
                            Highcharts.chart('container', {
                                chart: {
                                    zoomType: 'xy'
                                },
                                title: {
                                    text: 'Variação de Temperatura, Pressão e Produção: ' + maquina,
                                    align: 'left'
                                },
                                xAxis: [{
                                    categories: labels,
                                    crosshair: true
                                }],
                                yAxis: [{ // Primary yAxis
                                    labels: {
                                        format: '{value}°C',
                                        style: {
                                            color: Highcharts.getOptions().colors[2]
                                        }
                                    },
                                    title: {
                                        text: 'Temeratura',
                                        style: {
                                            color: Highcharts.getOptions().colors[2]
                                        }
                                    },
                                    opposite: true

                                }, { // Secondary yAxis
                                    gridLineWidth: 0,
                                    title: {
                                        text: 'Pressão',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    },
                                    labels: {
                                        format: '{value} bar',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    }
                                }, { // Tertiary yAxis
                                    gridLineWidth: 0,
                                    title: {
                                        text: 'Produção',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    labels: {
                                        format: '{value} ciclos',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    opposite: true
                                }],
                                tooltip: {
                                    shared: true
                                },
                                legend: {
                                    layout: 'vertical',
                                    align: 'left',
                                    x: 80,
                                    verticalAlign: 'top',
                                    y: 55,
                                    floating: true,
                                    backgroundColor:
                                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                                        'rgba(255,255,255,0.25)'
                                },
                                series: [{
                                    name: 'Produção',
                                    type: 'column',
                                    yAxis: 2,
                                    data: dados_prod,
                                    tooltip: {
                                        valueSuffix: ' ciclos'
                                    }

                                }, {
                                    name: 'Pressão',
                                    type: 'spline',
                                    yAxis: 1,
                                    data: dados_pres,
                                    marker: {
                                        enabled: false
                                    },
                                    dashStyle: 'shortdot',
                                    tooltip: {
                                        valueSuffix: ' Bar'
                                    }

                                }, {
                                    name: 'Temperatura',
                                    type: 'spline',
                                    data: dados_temp,
                                    tooltip: {
                                        valueSuffix: ' °C'
                                    }
                                }],
                                responsive: {
                                    rules: [{
                                        condition: {
                                            maxWidth: 500
                                        },
                                        chartOptions: {
                                            legend: {
                                                floating: false,
                                                layout: 'horizontal',
                                                align: 'center',
                                                verticalAlign: 'bottom',
                                                x: 0,
                                                y: 0
                                            }
                                        }
                                    }]
                                }
                            });
                            //
                            $('.highcharts-credits').remove();
                        } catch (e) {

                        }
                        //
                        m.grafico.loadingout();
                    }
                    else {
                        m.grafico.loadingout();
                        alert('A máquina não possui dados!');
                    }
                })
            },
            temperatura: function () {
                //
                var maquina = document.getElementById('slmaquinas').value;
                //
                m.grafico.l = m.g.load('Carregando gráfico comparativo...');
                var e = document.getElementById("slmaquinas");
                $.get(m.g.rsvlurl('Grafico/GraficoPresTempProd') + '?idmaquina=' + e.options[e.selectedIndex].id + '&periodo=' + $('input[name=daterange]')[0].value, function (data) {
                    //
                    if (data.data = 'OK') {
                        //
                        var dados_temp = [];
                        var dados_pres = [];
                        var dados_prod = [];
                        //
                        try {
                            for (var i = 0; i < data.lista_temperatura[0].length; i++) {
                                dados_temp.push(parseFloat(data.lista_temperatura[0][i].replace(',', '.')));
                                dados_pres.push(parseFloat(data.lista_pressao[0][i].replace(',', '.')));
                                dados_prod.push(parseFloat(data.lista_producao[0][i]));
                            }
                            //
                            var labels = data.lista_labels.reverse();
                            dados_temp = dados_temp.reverse();
                            dados_pres = dados_pres.reverse();
                            dados_prod = dados_prod.reverse();
                            //
                            Highcharts.chart('container', {
                                chart: {
                                    zoomType: 'xy'
                                },
                                title: {
                                    text: 'Variação de Temperatura, Pressão e Produção: ' + maquina,
                                    align: 'left'
                                },
                                xAxis: [{
                                    categories: labels,
                                    crosshair: true
                                }],
                                yAxis: [{ // Primary yAxis
                                    labels: {
                                        format: '{value}°C',
                                        style: {
                                            color: Highcharts.getOptions().colors[2]
                                        }
                                    },
                                    title: {
                                        text: 'Temeratura',
                                        style: {
                                            color: Highcharts.getOptions().colors[2]
                                        }
                                    },
                                    opposite: true

                                }, { // Secondary yAxis
                                    gridLineWidth: 0,
                                    title: {
                                        text: 'Pressão',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    },
                                    labels: {
                                        format: '{value} bar',
                                        style: {
                                            color: Highcharts.getOptions().colors[1]
                                        }
                                    }
                                }, { // Tertiary yAxis
                                    gridLineWidth: 0,
                                    title: {
                                        text: 'Produção',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    labels: {
                                        format: '{value} ciclos',
                                        style: {
                                            color: Highcharts.getOptions().colors[0]
                                        }
                                    },
                                    opposite: true
                                }],
                                tooltip: {
                                    shared: true
                                },
                                legend: {
                                    layout: 'vertical',
                                    align: 'left',
                                    x: 80,
                                    verticalAlign: 'top',
                                    y: 55,
                                    floating: true,
                                    backgroundColor:
                                        Highcharts.defaultOptions.legend.backgroundColor || // theme
                                        'rgba(255,255,255,0.25)'
                                },
                                series: [{
                                    name: 'Produção',
                                    type: 'column',
                                    yAxis: 2,
                                    data: dados_prod,
                                    tooltip: {
                                        valueSuffix: ' ciclos'
                                    }

                                }, {
                                    name: 'Pressão',
                                    type: 'spline',
                                    yAxis: 1,
                                    data: dados_pres,
                                    marker: {
                                        enabled: false
                                    },
                                    dashStyle: 'shortdot',
                                    tooltip: {
                                        valueSuffix: ' Bar'
                                    }

                                }, {
                                    name: 'Temperatura',
                                    type: 'spline',
                                    data: dados_temp,
                                    tooltip: {
                                        valueSuffix: ' °C'
                                    }
                                }],
                                responsive: {
                                    rules: [{
                                        condition: {
                                            maxWidth: 500
                                        },
                                        chartOptions: {
                                            legend: {
                                                floating: false,
                                                layout: 'horizontal',
                                                align: 'center',
                                                verticalAlign: 'bottom',
                                                x: 0,
                                                y: 0
                                            }
                                        }
                                    }]
                                }
                            });
                            //
                            $('.highcharts-credits').remove();
                        } catch (e) {

                        }
                        //
                        m.grafico.loadingout();
                    }
                    else {
                        m.grafico.loadingout();
                        alert('A máquina não possui dados!');
                    }
                })
            },
            loadingout: function () {
                try { m.grafico.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.relatorio = (function () {
        return {
            grafico: null,
            load: null,
            list_ids: [],
            carrega_maquina: function () {
                //
                if ($('#slmaquinasrelatorio').select2('val') && $('#slmaquinasrelatorio').select2('val').length > 0) {
                    //
                    for (var i = 0; i < $('#slmaquinasrelatorio').find(':selected').length; i++) {
                        //
                        m.relatorio.list_ids.push($('#slmaquinasrelatorio').find(':selected')[i].id);
                    }
                    //
                    m.relatorio.load = m.g.load('Carregando relatório de máquinas...');
                    //
                    $.ajax({
                        url: m.g.rsvlurl('relatorio/RelatorioTemperatura') + '?idempresa=' + m.sessao.cd_empresa_logado + '&lista_ids=' + m.relatorio.list_ids +
                            '&periodo=' + d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id,
                        type: "post",
                        success: function (resp) {
                            //

                            //
                            if (resp && resp.sreult == 'ok') {
                                //
                                document.getElementById('idframerelmaquina').src = 'http://localhost/connector/temp/Relat%C3%B3rio_M%C3%A1quinas31-07-2019_01-59-24_.pdf'
                                //m.relatorio.grafico_relatorio(resp.lista_dados_retorno, resp.lista_labels, d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id);
                                //alert('ok');

                            }
                            else {
                                $('#msg')[0].innerHTML = 'Ocorreu um erro ao tentar salvar a conta.';
                                $("#btnModal").click();
                            }
                        },
                        error: function (xhr, error) {
                            //
                            //console.log(xhr);
                            //console.log(error);

                        }
                    });
                }
                else {
                    m.g.alert('Restriçao', 'Informe a máquina.');
                }
            },
            temperatura_atmosfera: function () {
                //
                if ($('#slmaquinasrelatorio').select2('val') && $('#slmaquinasrelatorio').select2('val').length > 0) {
                    //
                    for (var i = 0; i < $('#slmaquinasrelatorio').find(':selected').length; i++) {
                        //
                        m.relatorio.list_ids.push($('#slmaquinasrelatorio').find(':selected')[i].id);
                    }
                    //
                    m.relatorio.load = m.g.load('Carregando relatório de máquinas...');
                    //
                    $.ajax({
                        url: m.g.rsvlurl('relatorio/RelatorioTemperaturaGetDados') +
                            '?idempresa=' + m.sessao.cd_empresa_logado +
                            '&lista_ids=' + m.relatorio.list_ids +
                            '&periodo=' + $('input[name=daterange]')[0].value + 
                            '&horaini=' + d.getid("hrinicial").value + 
                            '&horafim=' + d.getid("hrfinal").value,
                        type: "post",
                        //contentType: "application/json",
                        //dataType: 'text',
                        success: function (resp) {
                            //
                            m.relatorio.load.out();
                            //
                            m.relatorio.list_ids = []
                            if (resp && resp.data == 'OK') {
                                //
                                let yaxys = resp.dados[0][0];
                                let xaxys = resp.dados[0][1];
                                let label = resp.dados[0][2];
                                console.log('per', resp.periodo)
                                _m.grafico.grafico_temperatura_report(xaxys, yaxys, resp.maquinas, resp.periodo);
                                //document.getElementById('idframerelmaquina').src = 'http://localhost/connector/temp/' + resp.result.data
                                //report = JSON.parse(resp.result).data;
                                //document.getElementById('idframerelmaquina').src = location.href + "/Temp/" + report;
                                //m.relatorio.grafico_relatorio(resp.lista_dados_retorno, resp.lista_labels, d.getid("slperiodo").options[d.getid("slperiodo").selectedIndex].id);
                                //alert('ok');

                            }
                            else {
                                $('#msg')[0].innerHTML = 'Ocorreu um erro ao tentar salvar a conta.';
                                $("#btnModal").click();
                            }
                        },
                        error: function (xhr, error) {
                            m.relatorio.list_ids = []
                        }
                    });
                }
                else {
                    m.g.alert('Restriçao', 'Informe a máquina.');
                }
            }
        }
    })()
    //
    _m.alerta = (function () {
        return {
            grafico: null,
            load: null,
            init: function (data) {
                //
                var tablealertaMaquina = $('#dataTableAlertaMaquina')
                tablealertaMaquina.bootstrapTable({ data: data });
                //
                m.logatividade.binit = false;
                $('#slmaquinasalerta').select2({
                    placeholder: "Selecione a(s) máquina(s)",
                    width: "100%"
                })
            }
        }
    })()
    //
    _m.programa = (function () {
        return {
            grafico: null,
            idprograma: null,
            load: null,
            list_ids: [],
            init: function (data) {
                var $table = $('#dtprogramas')
                $table.bootstrapTable({ data: data });
                if (m.sessao.tipo_empresa_logado != 1) {
                    $table.bootstrapTable('hideColumn', 'Empresa');
                }
            },
            carrega_maquina: function () {
                //

            },
            salva: function () {
                //
                if (!$('input[id=txtdescprograma').val()) {
                    m.g.alert('Atenção', 'Informe a descrição do Programa!');
                    return;
                }
                m.programa.l = m.g.load('Salvando');
                //
                var table = $('#dtprogramas');
                var select = table.bootstrapTable('getSelections');
                var empresa_anterior = select.length == 0 ? $(d.getid('slempresaprograma')).children(":selected").attr("id") : select[0].Id_Empresa;
                var empresa_nova = $(d.getid('slempresaprograma')).children(":selected").attr("id");
                var id_programa = select.length == 0 ? 0 : select[0].Id;
                //
                if (!id_programa) {
                    id_programa = 0;
                }
                //
                if (!empresa_anterior) {
                    empresa_anterior = 0;
                }
                var url = m.g.rsvlurl('programa/programapost') +
                    '?descricao=' + d.getid('txtdescprograma').value +
                    '&idprograma=' + id_programa +
                    '&empresa_anterior=' + empresa_anterior +
                    '&empresa_nova=' + empresa_nova;
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        if (resp && resp.data == 'ok') {
                            $('#idprogramamodal').modal('hide');
                            m.programa.loadprogramas();
                        }
                        else {
                            try { m.programa.l.out() } catch (err) { }
                            $('#idprogramamodal').modal('hide');
                            m.g.alert('Atenção', 'Ocorreu um erro ao tentar incluir o programa. Erro: ' + data.erro);
                        }
                        $('#coletorModal').modal('hide');
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            loadprogramas: function () {
                $.get(m.g.rsvlurl('programa/carregadados'), function (data) {
                    if (data && data.data == 'ok') {
                        var $table = $('#dtprogramas');
                        $table.bootstrapTable('load', data.lista_programas);
                        m.programa.loadingout();
                    }
                    else {
                        m.programa.loadingout();
                        alert('Erro: ' + data.ret);
                    }
                });
            },
            setcoletorinclui: function () {
                idprograma = 0;
                $('#modalProgramaLabel').text('Incluir Programa');
                //$('input[id=txtdesccoletor').val('');
                //$('input[id=txtmaccoletor').val('');
                $('#idprogramamodal').modal('show');
            },
            editcoletor: function () {
                //
                m.coletor.l = m.g.load('Carregando programa...');
                m.coletor.alertaedit = false;
                var table = $('#dtprogramas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    //
                    if (select.length == 1) {
                        //
                        var tablealertas = $('#dtalertas')
                        tablealertas.bootstrapTable('hideColumn', 'Valor');
                        tablealertas.bootstrapTable('hideColumn', 'Id');

                        //m.g.setcomboval('slempresacoletor', select[0].Id_Empresa);
                        //$('input[id=txtdesccoletor').val(select[0].Descricao);

                        //if (select[0].Alerta == 1) {
                        //    m.g.setcomboval('slcoletorgeraalerta', 1);
                        //    m.coletor.setenabdisabbtnalerta(false);
                        //}
                        //else {
                        //    m.g.setcomboval('slcoletorgeraalerta', 0);
                        //    m.coletor.setenabdisabbtnalerta(true);
                        //}
                        $('#divalertas').css('display', 'inline');
                        $('#modalColetorLabel').text('Editar Coletor');
                        //
                        m.coletor.loadalertascoletor();
                    }
                    else {
                        m.coletor.loadingout();
                        bootbox.alert({
                            size: "small",
                            title: "Restrição",
                            message: "Selecione apenas um coletor para editar.",
                            centerVertical: true
                        });
                    }
                }
                else {
                    m.coletor.loadingout();
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Selecione um coletor para editar.",
                        centerVertical: true
                    });
                }
            },
            loadingout: function () {
                try { m.programa.l.out() } catch (e) { }
            }
        }
    })()
    //
    _m.receita = (function () {
        return {
            l: null, idreceita: null, alertaedit: null, alertainit: null,
            init: function (data) {
                var $table = $('#dtreceitas')
                $table.bootstrapTable({ data: data });
                if (m.sessao.tipo_empresa_logado != 1) {
                    $table.bootstrapTable('hideColumn', 'Empresa');
                }
            },
            salva: function () {
                //
                if (!$('input[id=txtdescreceita').val()) {
                    m.g.alert('Atenção', 'Informe a descrição da Receita!');
                    return;
                }
                m.receita.l = m.g.load('Salvando');
                //
                var table = $('#dtreceitas');
                var select = table.bootstrapTable('getSelections');
                var id_receita = select.length == 0 ? 0 : select[0].Id;
                //
                var url = m.g.rsvlurl('receita/receitapost') + '?descricao=' + d.getid('txtdescreceita').value + '&idreceita=' + id_receita;
                $.ajax({
                    url: url,
                    type: "post",
                    success: function (resp) {
                        if (resp && resp.data == 'ok') {
                            $('#idreceitamodal').modal('hide');
                            m.receita.loadreceitas();
                        }
                        else {
                            try { m.receita.l.out() } catch (err) { }
                            $('#maquinaModal').modal('hide');
                            m.g.alert('Atenção', 'Ocorreu um erro ao tentar incluir a máquina. Erro: ' + data.erro);
                        }
                        $('#coletorModal').modal('hide');
                    },
                    error: function (xhr, error) {
                        console.log(xhr);
                        console.log(error);
                    }
                });
            },
            exclui: function () {
                var table = $('#dtreceitas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    if (select.length == 1) {

                        if (!select[0].Id_Maquina) {
                            bootbox.confirm({
                                title: "Excluir Receita?",
                                message: "Você deseja realmente excluir a Receita ('" + select[0].Descricao + "')? <br />Esta ação não poderá ser disfeita.",
                                centerVertical: true,
                                buttons: {
                                    cancel: {
                                        label: '<i class="fa fa-times"></i> Cancelar'
                                    },
                                    confirm: {
                                        label: '<i class="fa fa-check"></i> Confirmar'
                                    }
                                },
                                callback: function (result) {
                                    //
                                    if (result) {
                                        m.receita.l = m.g.load('Excluindo receita...');
                                        m.receita.idreceita = select[0].Id;
                                        $.get(m.g.rsvlurl('receita/ExcluiReceitaPost') + '?idreceita=' + select[0].Id, function (data) {
                                            if (data.ret && data.ret == 'ok') {
                                                m.receita.loadreceitas();
                                            }
                                            else if (data.ret && data.ret == 'erro') {
                                                m.coletor.loadingout();
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Ocorreu um erro",
                                                    message: data.erro,
                                                    centerVertical: true
                                                });
                                            }
                                            else if (data.ret && data.ret == 'nao_encontrada') {
                                                m.coletor.loadingout();
                                                bootbox.alert({
                                                    size: "large",
                                                    title: "Ocorreu um erro",
                                                    message: "Receita não encontrada!",
                                                    centerVertical: true
                                                });
                                            }
                                            else {
                                                m.receita.loadingout();
                                                m.g.alert('Atenção', 'Ocorreu um erro ao tentar excluir a Receita. Erro: ' + data.erro);
                                            }
                                        });
                                    }
                                }
                            });
                        }
                        else {
                            bootbox.alert({
                                size: "large",
                                title: "Restriçao",
                                message: "Primeiro desassocie a máquina '" + select[0].Maquina + "' do coletor!",
                                centerVertical: true
                            });
                        }
                    }
                    else {
                        bootbox.alert({
                            size: "small",
                            title: "Restriçao",
                            message: "Selecione apenas um coletor para excluir.",
                            centerVertical: true
                        })
                    }
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Restriçao",
                        message: "Selecione uma receita para excluir.",
                        centerVertical: true
                    });
                }
            },
            setcoletorinclui: function () {
                idreceita = 0;
                $('#modalColetorLabel').text('Incluir Coletor');
                $('input[id=txtdescreceita').val('');
                //$('input[id=txtmaccoletor').val('');
                $('#divalertas').css('display', 'none');
                $('#idreceitamodal').modal('show');
            },
            editreceita: function () {
                //
                m.receita.l = m.g.load('Carregando receita...');
                m.receita.alertaedit = false;
                var table = $('#dtreceitas');
                var select = table.bootstrapTable('getSelections');
                if (select && select.length > 0) {
                    //
                    if (select.length == 1) {
                        //
                        var tablealertas = $('#dtalertas')
                        tablealertas.bootstrapTable('hideColumn', 'Valor');
                        tablealertas.bootstrapTable('hideColumn', 'Id');

                        $('input[id=txtdescreceita').val(select[0].Descricao);
                        $('.modal-title').text('Editar Receita');
                        $('#idreceitamodal').modal('show');
                        m.receita.loadingout();
                    }
                    else {
                        m.receita.loadingout();
                        bootbox.alert({
                            size: "small",
                            title: "Restrição",
                            message: "Selecione apenas uma receita para editar.",
                            centerVertical: true
                        });
                    }
                }
                else {
                    m.receita.loadingout();
                    bootbox.alert({
                        size: "small",
                        title: "Restrição",
                        message: "Selecione uma receita para editar.",
                        centerVertical: true
                    });
                }
            },
            loadreceitas: function () {
                $.get(m.g.rsvlurl('receita/carregadados'), function (data) {
                    if (data && data.data == 'ok') {
                        var $table = $('#dtreceitas');
                        $table.bootstrapTable('load', data.lista_receita);
                        m.receita.loadingout();
                    }
                    else {
                        m.receita.loadingout();
                        alert('Erro: ' + data.ret);
                    }
                });
            },
            loadingout: function () {
                try { m.receita.l.out() } catch (e) { }
            }
        }
    })()
    //
    return _m;
}()