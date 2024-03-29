sql script

CREATE TABLE Products (
	Id int IDENTITY(1,1) NOT NULL,
	ProductName text COLLATE Cyrillic_General_CI_AS NULL,
	Price int NULL,
	CONSTRAINT PK__Products__3214EC071EAD2633 PRIMARY KEY (Id)
);




CREATE TABLE Users (
	Id int IDENTITY(1,1) NOT NULL,
	UserName varchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
	UserLogin varchar(16) COLLATE Cyrillic_General_CI_AS NULL,
	Password varchar(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT PK__Users__3214EC07C3F50BE3 PRIMARY KEY (Id)
);



CREATE TABLE Orders (
	Id int IDENTITY(1,1) NOT NULL,
	UserID int NULL,
	OrderPrice int NOT NULL,
	CONSTRAINT PK__Orders__3214EC0707D027E5 PRIMARY KEY (Id),
	CONSTRAINT FK__Orders__UserID__3B75D760 FOREIGN KEY (UserID) REFERENCES Users(Id)
);



CREATE TABLE OrderPosition (
	Id int IDENTITY(1,1) NOT NULL,
	OrderID int NULL,
	ProductID int NULL,
	ProductQuantity int DEFAULT 1 NULL,
	CONSTRAINT PK__OrderPos__3214EC074B8B066A PRIMARY KEY (Id),
	CONSTRAINT FK__OrderPosi__Order__3E52440B FOREIGN KEY (OrderID) REFERENCES Orders(Id),
	CONSTRAINT FK__OrderPosi__Produ__3F466844 FOREIGN KEY (ProductID) REFERENCES Products(Id)
);
