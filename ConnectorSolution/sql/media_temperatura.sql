
select * from ColetorTemperaturaHistorico

SELECT Cast(datahora AS DATE)                   AS 'Data',
        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
        Datepart(month, datahora)                AS Mes, 
        Datepart(day, datahora)                  AS Dia, 
        Temperatura
    FROM   ColetorTemperaturaHistorico 
WHERE Id_Coletor = 47
    AND DataHora >= '2019-12-15 14:40:01.000'
    AND DataHora <= '2019-12-31 14:40:01.000'
    AND Temperatura IS NOT NULL 

    ORDER  BY Datepart(month, datahora), 
            Datepart(day, datahora)

/*select * from ColetorTemperaturaHistorico

SELECT Cast(datahora AS DATE)                   AS 'Data',
        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
        Datepart(month, datahora)                AS Mes, 
        Datepart(day, datahora)                  AS Dia, 
        ROUND((Avg(Cast(temperatura AS FLOAT)) / 10 ), 2) AS 'TemperaturaMedia' 
    FROM   ColetorTemperaturaHistorico 
WHERE Id_Coletor = 47
    AND DataHora >= '2019-12-15 14:40:01.000'
    AND DataHora <= '2019-12-31 14:40:01.000'
    AND Temperatura IS NOT NULL 
    GROUP  BY Cast(datahora AS DATE), 
            Datepart(day, datahora), 
            Datepart(month, datahora) 
    ORDER  BY Datepart(month, datahora), 
            Datepart(day, datahora)

			*/