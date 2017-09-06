/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


INSERT INTO [Address] (Line1,Line2,City,StateProvince,ZipCode) VALUES ('4912 Jarvis',null,'Skokie','Illinois','60077');
INSERT INTO [Address] (Line1,Line2,City,StateProvince,ZipCode) VALUES ('4840 Elm St','Apt 2','Skokie','Illinois','60076');
INSERT INTO [Address] (Line1,Line2,City,StateProvince,ZipCode) VALUES ('222 W Ontario','#403','Chicago','Illinois','39483');
INSERT INTO [Address] (Line1,Line2,City,StateProvince,ZipCode) VALUES ('123 Street St',null,'Atlanta','Georgia','23423-3424');

INSERT INTO Category ([Name]) VALUES ('Electronic Instruments'),('Wind Instruments'),('String Instruments'),('Keyboard Instruments'),('Percussion'),('Brass Instruments');
INSERT INTO Category ([Name]) VALUES ('Misc');
INSERT INTO Category ([Name],ParentID) VALUES ('Slide Whistle', (SELECT ID FROM Category WHERE [Name] = 'Misc'));
INSERT INTO Category ([Name],ParentID) VALUES ('Vuvuzela', (SELECT ID FROM Category WHERE [Name] = 'Misc'));
INSERT INTO Category ([Name],ParentID) VALUES ('Otamatone', (SELECT ID FROM Category WHERE [Name] = 'Electronic Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Stylophone', (SELECT ID FROM Category WHERE [Name] = 'Electronic Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Theremin', (SELECT ID FROM Category WHERE [Name] = 'Electronic Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Banjo', (SELECT ID FROM Category WHERE [Name] = 'String Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Hurdy Gurdy', (SELECT ID FROM Category WHERE [Name] = 'String Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Accordion', (SELECT ID FROM Category WHERE [Name] = 'Keyboard Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Keytar', (SELECT ID FROM Category WHERE [Name] = 'Keyboard Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Melodica', (SELECT ID FROM Category WHERE [Name] = 'Keyboard Instruments'));
INSERT INTO Category ([Name],ParentID) VALUES ('Tuba', (SELECT ID FROM Category WHERE [Name] = 'Brass Instruments'));


INSERT INTO Customer (Title,FirstName,MiddleName,LastName,Suffix,DateOfBirth,PhoneNumber,EmailAddress,EmailPromotion,PasswordHash) VALUES ('Mr','Paul','I','Goldman',null,'08/05/1988','123-456-7890','abc@example.com',0,'hashashhas');
INSERT INTO Customer (Title,FirstName,MiddleName,LastName,Suffix,DateOfBirth,PhoneNumber,EmailAddress,EmailPromotion,PasswordHash) VALUES (null,'Joe',null,'Johnson','Jr','01/02/1934','222-222-2222','joe@joseph.com',1,'hashashasdkjhhas');
INSERT INTO Customer (Title,FirstName,MiddleName,LastName,Suffix,DateOfBirth,PhoneNumber,EmailAddress,EmailPromotion,PasswordHash) VALUES ('Mrs','Marina',null,'Goldman',null,'02/03/1966','203-234-2043','marina@example.com',0,'asdasdasd');
INSERT INTO Customer (Title,FirstName,MiddleName,LastName,Suffix,DateOfBirth,PhoneNumber,EmailAddress,EmailPromotion,PasswordHash) VALUES ('Ms','Renee','Lynn','Dippel',null,'07/29/1982','123-123-1231','renee@asd.com',0,'asdasedfxcb');


INSERT INTO Product (Brand,[Name],[Description],ListPrice,Rating,ProductLink,BrandLink) VALUES ('Roland','FR18 VAccordion','In 2004, after several years of research, a dream of Roland founder Mr. Ikutaro Kakehashi came true: the V-Accordion was born. This instrument was the world’s first fully digital accordion, powered by the groundbreaking new Physical Behavior Modeling technology. Bringing together the playability of a fine acoustic accordion with all the conveniences of a modern digital instrument, the V-Accordion was immediately embraced by players around the world. Today, the V-Accordion lineup has grown to include a wide range of models, from student instruments to full-featured professional accordions.',149.99, 3.6,'http://www.rolandus.com/go/v-accordion/','https://www.roland.com/us/');
INSERT INTO Product (Brand,[Name],[Description],ListPrice,Rating,ProductLink,BrandLink) VALUES (null,'Vuvuzela','A Stadium Horn for Every Event! Pump up the volume at your event with this Air Horn. A great way to spice up any party. Measures 29½\" long x 4½\" wide and compacts down to 15¾\" long! Made of plastic .Obnoxious air horn sound! A noisemaker is an ideal party favor give-a-way for your next New Year''s party or Mardi Gras event. Plus they also make a great prize for any carnival. Great for parties & sporting events...',28.93, 2.4,'https://en.wikipedia.org/wiki/Vuvuzela',null);
INSERT INTO Product (Brand,[Name],[Description],ListPrice,Rating,ProductLink,BrandLink) VALUES ('Deering', 'Classic Goodtime Special Open Back Banjo','The Classic Goodtime Special Openback is louder and brighter than the original Goodtime model because it has the patent pending Special Tone Ring and is great for players who like to jam and want to be heard. The rich brown stain, planetary tuners, and Deering peghead shape gives the classic look of a vintage banjo.',715.50, null, 'https://www.deeringbanjos.com/products/goodtime-special-openback-banjo','https://www.deeringbanjos.com/');

INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion') ,'/Content/images/products/accordion/Roland_FR18_VAccordion.jpg','This is a picture of the Roland FR18 VA Accordion');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion') ,'/Content/images/products/accordion/Roland_FR18_VAccordion2.jpg','This is the second picture of the Roland FR18 VA Accordion');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Classic Goodtime Special Open Back Banjo') ,'/Content/images/products/banjo/Deering-Classic-Goodtime-Special-Openback-Banjo_1.jpg','Heeeere''s a banjo!');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Vuvuzela'), '/Content/images/products/vuvuzela/vuvuzela_1.jpeg','This is a picture of a vuvuzela');

INSERT INTO ProductProductImage (ProductId, ProductImageId, [Primary]) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion'),1,0);
INSERT INTO ProductProductImage (ProductId, ProductImageId, [Primary]) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion'),2,1);
INSERT INTO ProductProductImage (ProductId, ProductImageId, [Primary]) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Vuvuzela'),4,1);
INSERT INTO ProductProductImage (ProductId, ProductImageId, [Primary]) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Classic Goodtime Special Open Back Banjo'),3,1);

INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion'), 3);
INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Vuvuzela'), 25);
INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Classic Goodtime Special Open Back Banjo'), 1);

INSERT INTO ProductReview (ProductID,CustomerID,Rating,Comments) VALUES ((SELECT ID FROM PRODUCT WHERE [Name] = 'FR18 VAccordion'), (SELECT ID FROM CUSTOMER WHERE [FirstName] = 'Renee' AND [LastName] = 'Dippel'),4,'Bestest accordion evar!!! LOL');

