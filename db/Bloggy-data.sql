USE [Bloggy]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'0e03ae3e-e8fa-4f05-a735-77acceb220d5', N'Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'73bbb3e0-54c5-4f63-99d9-00886491d6f1', N'Translator')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8c5a38df-0214-40e7-bec6-7cc327ff7c37', N'User')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'1cbc5be4-a51d-4450-b0e5-e7edf1633e3b', N'test2@test.com', 0, N'AEnIXBumsxeX27xpY8tBJVx/9Tga7fsUsFjON+zc5uLwwNzwcl7kW4r93osu25vq6w==', N'132c4452-bfee-4e84-a501-67476b575b29', NULL, 0, 0, NULL, 1, 0, N'test2@test.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'36eb3568-f4a3-46a2-bdee-26534a402c87', N'test@test.com', 0, N'ACJtFlZUVdxIhfWj9s3ZTcAweBgKoqrKsarymd8Vof6+QRnLptmgH8QUXXlLtvn3eQ==', N'322ba21d-087c-46f4-b669-1f45c0a3569d', NULL, 0, 0, NULL, 1, 0, N'test@test.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'4d1267ba-314e-4316-a082-3a3ccf5ae56a', N'dabtest@test.com', 0, N'AAZObUgdK59MlMw7Cl0iWPL+8XI8UhL7rlw9aJtr+n1TsmUQrsTJYIgrHQXOgekk/A==', N'33efe226-c8de-4b46-989c-b43393b44698', NULL, 0, 0, NULL, 1, 0, N'dabtest@test.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'4d7dd33e-4574-4431-af36-5a8a70ab33f6', N'text@test.com', 0, N'ALgglWgC5Mw1gzWRAG2ezgqkBhGMni8lD4pwf7S05tCa9eMoGQxSJaD1kqG+s8CGUQ==', N'712fd437-87d9-4474-931b-818436628091', NULL, 0, 0, NULL, 1, 0, N'text@test.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'a3677996-f252-47d0-a907-1e28a15b07c3', N'id@test.com', 0, N'ABu5rMdId7N9T9T+qEp03zb6hLZqlpojV08FdHWHP7T2cI0FYlWJkXdsFQaJKaYt/A==', N'ce5e1915-b486-40cf-9ad4-8eeb29c3dcee', NULL, 0, 0, NULL, 1, 0, N'id@test.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', N'admin@bloggy.com', 0, N'AJ80yG6ZeWINiniiF74JWNAff0O1NwIV+HktOebuWF3TfwlrV+QkReRxAC6uxGxOgA==', N'7bf8a94b-b5cf-4297-af44-8a1070dab4b5', NULL, 0, 0, NULL, 1, 0, N'admin@bloggy.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'eefdeb69-d83f-4a7a-a7b4-c3722006c9f8', N'plop@test.com', 0, N'ALb/Tog2Atmgv/X7w1PkVez5aI9MLlvn55cf1Pk0KJ1fyitN/NS7ppiiDB+04C3iSQ==', N'e1bf6dc4-5f8c-421a-a166-81c0f47b43e0', NULL, 0, 0, NULL, 1, 0, N'plop@test.com')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', N'0e03ae3e-e8fa-4f05-a735-77acceb220d5')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'4d1267ba-314e-4316-a082-3a3ccf5ae56a', N'73bbb3e0-54c5-4f63-99d9-00886491d6f1')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'eefdeb69-d83f-4a7a-a7b4-c3722006c9f8', N'73bbb3e0-54c5-4f63-99d9-00886491d6f1')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1cbc5be4-a51d-4450-b0e5-e7edf1633e3b', N'8c5a38df-0214-40e7-bec6-7cc327ff7c37')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'36eb3568-f4a3-46a2-bdee-26534a402c87', N'8c5a38df-0214-40e7-bec6-7cc327ff7c37')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'4d7dd33e-4574-4431-af36-5a8a70ab33f6', N'8c5a38df-0214-40e7-bec6-7cc327ff7c37')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a3677996-f252-47d0-a907-1e28a15b07c3', N'8c5a38df-0214-40e7-bec6-7cc327ff7c37')
SET IDENTITY_INSERT [dbo].[article] ON 

INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (1, N'fr-FR', N'Article#1-bis', N'url', N'desc', 1, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T01:04:04.117' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (2, N'fr-FR', N'Article#1', N'url', N'desc', 2, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-02T23:17:28.183' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (6, N'en-EN', N'SaltyTomatoes', N'urlz', N'desc', 3, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T04:25:50.997' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (7, N'fr-FR', N'TomastesAuSel', N'url', N'desc', 3, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T03:44:38.267' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (8, N'fr-FR', N'Po', N'ee', N'zz', 1, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T04:18:55.210' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (9, N'fr-FR', N'ddzee', N'zeze', N'zeze', 1, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T04:20:09.700' AS DateTime), 0)
INSERT [dbo].[article] ([id], [lang], [name], [banner], [description], [post_id], [user_id], [validated], [date], [views]) VALUES (10, N'fr-FR', N'SelTomate', N'eezrer', N'zerzer', 3, N'ad4fb596-0a2a-4a7d-9ffc-5876eef83fd3', 0, CAST(N'2017-07-03T04:23:51.923' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[article] OFF
INSERT [dbo].[element] ([id], [article_id], [type], [index], [link], [description]) VALUES (1, 6, 0, 0, N'ezrez', N'zerzre')
INSERT [dbo].[element] ([id], [article_id], [type], [index], [link], [description]) VALUES (2, 6, 0, 1, N'erzr', N'ezrrzezer')
SET IDENTITY_INSERT [dbo].[post] ON 

INSERT [dbo].[post] ([id], [name]) VALUES (1, N'Post#1')
INSERT [dbo].[post] ([id], [name]) VALUES (2, N'Post#2-bis')
INSERT [dbo].[post] ([id], [name]) VALUES (3, N'Post#3-LesTomatesAuSel')
SET IDENTITY_INSERT [dbo].[post] OFF
