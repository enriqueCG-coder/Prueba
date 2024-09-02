USE DB_PRUEBATEC
GO


--*********************************************
--
--	PROCEDURES PARA TITULAR TARJETA
--
--*********************************************
CREATE OR ALTER PROC sp_InsertarTitularTarjeta
    @Nombres VARCHAR(50),
    @Apellidos VARCHAR(50)
AS
BEGIN
    INSERT INTO TitularTarjeta (Nombres, Apellidos, Estado)
    VALUES (@Nombres, @Apellidos, 1);
END
GO

CREATE OR ALTER PROC sp_ModificarTitularTarjeta
    @IdTitularTarjeta INT,
    @Nombres VARCHAR(50),
    @Apellidos VARCHAR(50),
	@Estado bit
AS
BEGIN
    UPDATE TitularTarjeta
    SET Nombres = @Nombres,
        Apellidos = @Apellidos,
		Estado = @Estado
    WHERE IdTitularTarjeta = @IdTitularTarjeta;
END
GO


CREATE OR ALTER PROC sp_EliminarTitularTarjeta
    @IdTitularTarjeta INT
AS
BEGIN
    DELETE FROM TitularTarjeta
    WHERE IdTitularTarjeta = @IdTitularTarjeta;
END
GO


CREATE OR ALTER PROC sp_ListarTitulares
AS
BEGIN
	SELECT * FROM TitularTarjeta
END
GO



--*********************************************
--
--	PROCEDURES PARA TARJETA DE CREDITO
--
--*********************************************

CREATE OR ALTER PROC sp_InsertarTC
    @IdTitularTarjeta INT,
    @NumeroTarjeta VARCHAR(20),
    @SaldoActual DECIMAL(10, 2),
    @LimiteCredito DECIMAL(10, 2),
	@PorcentInteres DECIMAL(6,2),
	@PorcentIntSaldoMin DECIMAL(6,2),
	@Estado bit
AS
BEGIN
    INSERT INTO TarjetaCredito (IdTitularTarjeta, NumeroTarjeta, SaldoActual, LimiteCredito, PorcentInteres, PorcentIntSaldoMin, Estado)
    VALUES (@IdTitularTarjeta, @NumeroTarjeta, @SaldoActual, @LimiteCredito, @PorcentInteres, @PorcentIntSaldoMin, @Estado);
END
GO

CREATE OR ALTER PROCEDURE sp_ModificarTC
    @IdTarjetaCredito INT,
    @IdTitularTarjeta INT,
    @NumeroTarjeta VARCHAR(20),
    @SaldoActual DECIMAL(10, 2),
    @LimiteCredito DECIMAL(10, 2),
	@PorcentInteres DECIMAL(6,2),
	@PorcentIntSaldoMin DECIMAL(6,2),
	@Estado bit
AS
BEGIN
    UPDATE TarjetaCredito
    SET IdTitularTarjeta = @IdTitularTarjeta,
        NumeroTarjeta = @NumeroTarjeta,
        SaldoActual = @SaldoActual,
        LimiteCredito = @LimiteCredito,
		PorcentInteres = @PorcentInteres,
		PorcentIntSaldoMin = @PorcentIntSaldoMin,
		Estado = @Estado
    WHERE IdTarjetaCredito = @IdTarjetaCredito;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarTC
    @IdTarjetaCredito INT
AS
BEGIN
    DELETE FROM TarjetaCredito
    WHERE IdTarjetaCredito = @IdTarjetaCredito;
END
GO



CREATE OR ALTER PROC sp_ListarTC
AS
BEGIN
	SELECT TC.IdTarjetaCredito, TT.IdTitularTarjeta, TC.NumeroTarjeta, TC.SaldoActual, 
	TC.LimiteCredito, TC.SaldoDisponible, TC.PorcentInteres, TC.PorcentIntSaldoMin, TC.Estado,  TT.Nombres, TT.Apellidos,
	ROUND((TC.SaldoActual * TC.PorcentIntSaldoMin)/100, 2) AS CuotaMinima, ROUND(TC.SaldoActual + ((TC.SaldoActual * TC.PorcentInteres))/100,2) AS MontoContado,
	ROUND((TC.SaldoActual * TC.PorcentInteres)/100,2) AS IntBonificable
	FROM TarjetaCredito TC 
	INNER JOIN TitularTarjeta TT ON
	TC.IdTitularTarjeta = TT.IdTitularTarjeta
END
GO

CREATE OR ALTER PROC sp_ListarTCbyID
	@IdTarjetaCredito INT
AS
BEGIN  
	SELECT TC.IdTarjetaCredito, TT.IdTitularTarjeta, TC.NumeroTarjeta, TC.SaldoActual, 
	TC.LimiteCredito, TC.SaldoDisponible, TC.PorcentInteres, TC.PorcentIntSaldoMin, TC.Estado,  TT.Nombres, TT.Apellidos,
	ROUND((TC.SaldoActual * TC.PorcentIntSaldoMin)/100, 2) AS CuotaMinima, ROUND(TC.SaldoActual + ((TC.SaldoActual * TC.PorcentInteres))/100,2) AS MontoContado,
	ROUND((TC.SaldoActual * TC.PorcentInteres)/100,2) AS IntBonificable
	FROM TarjetaCredito TC 
	INNER JOIN TitularTarjeta TT ON
	TC.IdTitularTarjeta = TT.IdTitularTarjeta
	WHERE IdTarjetaCredito = @IdTarjetaCredito
END
GO

--*********************************************
--
--	PROCEDURES PARA COMPRA
--
--*********************************************
CREATE OR ALTER PROCEDURE sp_InsertarCompra
    @IdTarjetaCredito INT,
    @Descripcion VARCHAR(255),
    @Monto DECIMAL(10, 2)
AS
BEGIN
    DECLARE @NumeroAleatorio INT;
    DECLARE @Existe INT;
    DECLARE @SaldoDisponible DECIMAL(10, 2);

    SELECT @SaldoDisponible = SaldoDisponible
    FROM TarjetaCredito
    WHERE IdTarjetaCredito = @IdTarjetaCredito;

    IF @SaldoDisponible < @Monto
    BEGIN
        RAISERROR('El monto de la compra excede el saldo disponible de la tarjeta.', 16, 1);
        RETURN;
    END

    WHILE 1 = 1
    BEGIN
        SET @NumeroAleatorio = FLOOR(RAND() * (9999 - 1000 + 1)) + 1000;

        SELECT @Existe = COUNT(*)
        FROM Compra
        WHERE NumAutoriza = @NumeroAleatorio;

        IF @Existe = 0
            BREAK;
    END

    INSERT INTO Compra (IdTarjetaCredito, NumAutoriza, FechaCompra, Descripcion, Monto)
    VALUES (@IdTarjetaCredito, @NumeroAleatorio, GETDATE(), @Descripcion, @Monto);

    UPDATE TarjetaCredito
    SET SaldoActual = SaldoActual - @Monto
    WHERE IdTarjetaCredito = @IdTarjetaCredito;
END
GO



--*********************************************
--
--	PROCEDURES PARA PAGO
--
--*********************************************
CREATE OR ALTER PROCEDURE sp_InsertarPago
    @IdTarjetaCredito INT,
    @Monto DECIMAL(10, 2)
AS
BEGIN
    DECLARE @NumeroAleatorio INT;
    DECLARE @Existe INT;
    DECLARE @SaldoActual DECIMAL(10, 2);

    SELECT @SaldoActual = SaldoActual
    FROM TarjetaCredito
    WHERE IdTarjetaCredito = @IdTarjetaCredito;

    IF @SaldoActual <= 0
    BEGIN
        RAISERROR('No se pueden realizar pagos porque el saldo de la tarjeta ya es cero.', 16, 1);
        RETURN;
    END

    IF @Monto > @SaldoActual
    BEGIN
        RAISERROR('El monto del pago excede el saldo disponible de la tarjeta.', 16, 1);
        RETURN;
    END

    WHILE 1 = 1
    BEGIN
        SET @NumeroAleatorio = FLOOR(RAND() * (9999 - 1000 + 1)) + 1000;

        SELECT @Existe = COUNT(*)
        FROM Pago
        WHERE NumAutoriza = @NumeroAleatorio;

        IF @Existe = 0
            BREAK;
    END

    INSERT INTO Pago (IdTarjetaCredito, NumAutoriza, FechaPago, Monto)
    VALUES (@IdTarjetaCredito, @NumeroAleatorio, GETDATE(), @Monto);

    UPDATE TarjetaCredito
    SET SaldoActual = SaldoActual - @Monto
    WHERE IdTarjetaCredito = @IdTarjetaCredito;
END
GO



--*********************************************
--
--	PROCEDURES PARA TRANSACCIONES
--
--*********************************************

CREATE OR ALTER PROC sp_TransaccionesTC_ID
	@IdTarjetaCredito INT
AS
BEGIN
	SELECT
    COALESCE(c.NumAutoriza, p.NumAutoriza) AS NumAutorizacion,
    COALESCE(c.FechaCompra, p.FechaPago) AS Fecha,
    COALESCE(c.Descripcion, 'Pago de Tarjeta') AS Descripcion,
    ISNULL(c.Monto, 0.00) AS Cargo,
    ISNULL(p.Monto, 0.00) AS Abono
	FROM
		Compra c
	FULL OUTER JOIN Pago p ON
		c.IdTarjetaCredito = p.IdTarjetaCredito AND
		c.NumAutoriza = p.NumAutoriza
	WHERE COALESCE(c.IdTarjetaCredito, p.IdTarjetaCredito) = @IdTarjetaCredito ORDER BY Fecha DESC;
END
GO

--*********************************************
--
--	PROCEDURES PARA ESTADO DE CUENTA
--
--*********************************************

CREATE OR ALTER PROC sp_EstadoCuentaTC_ID
	@IdTarjetaCredito INT 
AS
BEGIN
	SELECT 
		NumAutoriza AS NumAutorizacion, 
		FechaCompra AS Fecha,
		Descripcion, 
		Monto AS Cargo, 
		Monto AS Abono 
	FROM Compra
	WHERE IdTarjetaCredito = @IdTarjetaCredito 
	ORDER BY Fecha DESC
END
GO

CREATE OR ALTER PROC sp_TotalEstadoCuentaTC_ID
    @IdTarjetaCredito INT,
    @Mes INT,
    @Anio INT
AS
BEGIN
    SELECT 
        SUM(Monto) AS TotalCargo
    FROM Compra
    WHERE IdTarjetaCredito = @IdTarjetaCredito 
      AND MONTH(FechaCompra) = @Mes
      AND YEAR(FechaCompra) = @Anio;
END

