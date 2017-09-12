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
INSERT INTO Product (Brand,[Name],[Description],ListPrice,ProductLink,BrandLink) VALUES ('Roland','FR18 VAccordion','In 2004, after several years of research, a dream of Roland founder Mr. Ikutaro Kakehashi came true: the V-Accordion was born. This instrument was the world’s first fully digital accordion, powered by the groundbreaking new Physical Behavior Modeling technology. Bringing together the playability of a fine acoustic accordion with all the conveniences of a modern digital instrument, the V-Accordion was immediately embraced by players around the world. Today, the V-Accordion lineup has grown to include a wide range of models, from student instruments to full-featured professional accordions.',1489.99, 'http://www.rolandus.com/go/v-accordion/','https://www.roland.com/us/');
INSERT INTO Product (Brand,[Name],[Description],ListPrice,ProductLink,BrandLink) VALUES ('Roland','FR-8X VAccordion','With their new FR-8x V-Accordion, Roland has perfected the synergy between traditional accordion playability and modern digital power. The latest flagship piano-type V-Accordion is jam packed with features and enhancements developed with input from the world’s top players, bringing a previously unattained level of expression and versatility to every accordionist. Innovative Dynamic Bellows Behavior technology delivers the true bellows response of an acoustic accordion, while the expanded sound set, four powerful multi-effects, an onboard looper, and much more offer a treasure chest of tools for dynamic musical exploration. Seamlessly fusing top-level technology with familiar acoustic tradition, the FR-8x V-Accordion ushers in a new era of creative freedom for players everywhere.',1349.99, 'http://www.rolandus.com/go/v-accordion/','https://www.roland.com/us/');

INSERT INTO Product (Brand,[Name],[Description],ListPrice,ProductLink,BrandLink) VALUES (null,'Vuvuzela','A Stadium Horn for Every Event! Pump up the volume at your event with this Air Horn. A great way to spice up any party. Measures 29½\" long x 4½\" wide and compacts down to 15¾\" long! Made of plastic .Obnoxious air horn sound! A noisemaker is an ideal party favor give-a-way for your next New Year''s party or Mardi Gras event. Plus they also make a great prize for any carnival. Great for parties & sporting events...',28.93, 'https://en.wikipedia.org/wiki/Vuvuzela',null);
INSERT INTO Product (Brand,[Name],[Description],ListPrice,ProductLink,BrandLink) VALUES ('Deering', 'Classic Goodtime Special Open Back Banjo','The Classic Goodtime Special Openback is louder and brighter than the original Goodtime model because it has the patent pending Special Tone Ring and is great for players who like to jam and want to be heard. The rich brown stain, planetary tuners, and Deering peghead shape gives the classic look of a vintage banjo.',715.50, 'https://www.deeringbanjos.com/products/goodtime-special-openback-banjo','https://www.deeringbanjos.com/');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion') ,'/Content/images/products/accordion/Roland_FR18_VAccordion.jpg','This is a picture of the Roland FR18 VA Accordion');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion') ,'/Content/images/products/accordion/Roland_FR18_VAccordion2.jpg','This is the second picture of the Roland FR18 VA Accordion');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Classic Goodtime Special Open Back Banjo') ,'/Content/images/products/banjo/Deering-Classic-Goodtime-Special-Openback-Banjo_1.jpg','Heeeere''s a banjo!');
INSERT INTO ProductImage (ProductID,ImagePath,AlternateText) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Vuvuzela'), '/Content/images/products/misc/vuvuzela_1.jpeg','This is a picture of a vuvuzela');
INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'FR18 VAccordion'), 3);
INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Vuvuzela'), 25);
INSERT INTO ProductInventory (ProductId, Quantity) VALUES ((SELECT ID FROM Product WHERE [Name] = 'Classic Goodtime Special Open Back Banjo'), 1);
