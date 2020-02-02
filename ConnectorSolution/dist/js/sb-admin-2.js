    /*!
     * Start Bootstrap - SB Admin 2 v3.3.7+1 (http://startbootstrap.com/template-overviews/sb-admin-2)
     * Copyright 2013-2016 Start Bootstrap
     * Licensed under MIT (https://github.com/BlackrockDigital/startbootstrap/blob/gh-pages/LICENSE)
     */
    $(function() {
        $('#side-menu').metisMenu();
    });

    //Loads the correct sidebar on window load,
    //collapses the sidebar on window resize.
    // Sets the min-height of #page-wrapper to window size
    $(function() {
        $(window).bind("load resize", function() {
            var topOffset = 50;
            var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
            if (width < 768) {
                $('div.navbar-collapse').addClass('collapse');
                topOffset = 100; // 2-row-menu
            } else {
                $('div.navbar-collapse').removeClass('collapse');
            }

            var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
            height = height - topOffset;
            if (height < 1) height = 1;
            if (height > topOffset) {
                $("#page-wrapper").css("min-height", (height) + "px");
            }
        });

        var url = window.location;
        // var element = $('ul.nav a').filter(function() {
        //     return this.href == url;
        // }).addClass('active').parent().parent().addClass('in').parent();
        var element = $('ul.nav a').filter(function() {
            return this.href == url;
        }).addClass('active').parent();

        while (true) {
            if (element.is('li')) {
                element = element.parent().addClass('in').parent();
            } else {
                break;
            }
        }
    });

    var xhr = new XMLHttpRequest();
    xhr.open('GET', resolveURL('home/getgraf'));
    xhr.onload = function () {
        if (xhr.status === 200) {
            if (JSON.parse(xhr.responseText).data == 'ok') {
                //
                var dados = JSON.parse(xhr.responseText).lista_retorno;
                //var data = [
                //      { y: '1 Set.', a: 1902.60, b: 1652.60, c: 1150.60},
                //      { y: '10 Set.', a: 1402.50,  b: 1050.23, c: 940.60},
                //      { y: '20 Set.', a: 1940.56,  b: 1550.50, c: 1200.60},
                //      { y: '30 Set', a: 1870,  b: 1350, c: 980.60},
                //      { y: '1 Out.', a: 1920,  b: 1660, c: 1220.60},
                //      { y: '10 Out.', a: 1750,  b: 1730, c: 1520.60},
                //      { y: '20 Out.', a: 1652, b: 1520, c: 1315.60},
                //      { y: '30 Out.', a: 1752, b: 1580, c: 1480.60},
                //      { y: '10 Nov.', a: 1950, b: 1590, c: 1140.60},
                //      { y: '20 Nov.', a: 1850, b: 1200, c: 1190.60},
                //      { y: '30 Nov.', a: 1790, b: 1600, c: 1350.60}
                //  ],
                config = {
                      data: dados,
                      xkey: 'y',
                      ykeys: ['a', 'b', 'c'],
                      labels: ['Maquina B', 'Maquina A', 'Maquina C'],
                      fillOpacity: 0.6,
                      parseTime: false,
                      hideHover: 'auto',
                      behaveLikeLine: true,
                      resize: true,
                      pointFillColors:['#ffffff'],
                      pointStrokeColors: ['black'],
                      lineColors:['#ef5151','#f9d849', '#00d2a7']
                };
                config.element = 'area-chart';
                try {
                    Morris.Area(config);
                } catch (e) {
                    console.log('hj não!');
                }
                
            }
            else { alert('Ocorreu um erro ao tentar gerar o gráfico!'); }
        }
        else if (JSON.parse(xhr.responseText).data == 'login') {
            console.log('Erro(' + xhr.status + ') XMLHttpRequest "dados login"');
        }
    };
    xhr.send();
