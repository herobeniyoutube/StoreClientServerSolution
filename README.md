Для работы, необходимо зарегистрировать админа. Login должен быть admin, имя можно оставить пустым. Далее откроется меню, где будет кнопка, с помощью которой можно добавить товары в систему. 
Программа будет работать и без админа, но тогда не сформировать заказ, ведь товаров в системе не будет. Не добавляю товары заранее намеренно, ведь в задании указан пункт с необходимостью такой фичи. 



sql script

CREATE TABLE Products (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(50) COLLATE Cyrillic_General_CI_AS NULL,
	Price int NULL,
	CONSTRAINT PK_Products PRIMARY KEY (Id)
);


CREATE TABLE Users (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Login] nvarchar(50) COLLATE Cyrillic_General_CI_AS NULL,
	Password nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
);


CREATE TABLE Orders (
	Id int IDENTITY(1,1) NOT NULL,
	UserId int NULL,
	Price int NULL,
	[Date] datetime2 NOT NULL,
	CONSTRAINT PK_Orders PRIMARY KEY (Id),
	CONSTRAINT FK_Orders_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id)
);
 CREATE NONCLUSTERED INDEX IX_Orders_UserId ON dbo.Orders (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


CREATE TABLE OrderPositions (
	Id int IDENTITY(1,1) NOT NULL,
	OrderId int NULL,
	ProductId int NULL,
	ProductQuantity int NULL,
	CONSTRAINT PK_OrderPositions PRIMARY KEY (Id),
	CONSTRAINT FK_OrderPositions_Orders_OrderId FOREIGN KEY (OrderId) REFERENCES Orders(Id),
	CONSTRAINT FK_OrderPositions_Products_ProductId FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
 CREATE NONCLUSTERED INDEX IX_OrderPositions_OrderId ON dbo.OrderPositions (  OrderId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_OrderPositions_ProductId ON dbo.OrderPositions (  ProductId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
