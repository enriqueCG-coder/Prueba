CREATE DATABASE DB_PRUEBATEC
GO

USE DB_PRUEBATEC
GO

CREATE TABLE TitularTarjeta (
    IdTitularTarjeta INT PRIMARY KEY IDENTITY,
    Nombres VARCHAR(50) NOT NULL,
	Apellidos VARCHAR(50) NOT NULL,
	Estado bit
);
GO


CREATE TABLE TarjetaCredito (
    IdTarjetaCredito INT PRIMARY KEY IDENTITY,
    IdTitularTarjeta INT NOT NULL,
    NumeroTarjeta VARCHAR(20) UNIQUE NOT NULL,
    SaldoActual DECIMAL(10, 2),
    LimiteCredito DECIMAL(10, 2) NOT NULL,
    SaldoDisponible AS (LimiteCredito - SaldoActual), 
	PorcentInteres DECIMAL(6,2) NOT NULL,
	PorcentIntSaldoMin DECIMAL(6,2) NOT NULL,
	Estado bit
    FOREIGN KEY (IdTitularTarjeta) REFERENCES TitularTarjeta(IdTitularTarjeta)
);
GO

CREATE TABLE Compra (
    IdCompra INT PRIMARY KEY IDENTITY,
    IdTarjetaCredito INT NOT NULL,
	NumAutoriza INT NOT NULL,
    FechaCompra DATETIME NOT NULL,
    Descripcion VARCHAR(255),
    Monto DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (IdTarjetaCredito) REFERENCES TarjetaCredito(IdTarjetaCredito)
);
GO


CREATE TABLE Pago (
    IdPago INT PRIMARY KEY IDENTITY,
    IdTarjetaCredito INT NOT NULL,
	NumAutoriza INT NOT NULL,
    FechaPago DATETIME NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (IdTarjetaCredito) REFERENCES TarjetaCredito(IdTarjetaCredito)
);
GO

